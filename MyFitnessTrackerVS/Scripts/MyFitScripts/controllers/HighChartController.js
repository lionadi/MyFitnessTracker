var highChartsController = {
    Chart: null,
    MonthNames: [],
    ChartOptions: null,
    UserOverallDataByDateRange: [],
    gmGeocoder : "",
    gmBounds : "",
    mapOptions : "",
    gmMap : "",
    DataDateRange: { StartDate: null, EndDate: null },
    ResetCalled: false,
    CreateGoogleMapsObject: function(){

        highChartsController.gmBounds = new google.maps.LatLngBounds();

        highChartsController.mapOptions = {
            zoom: 5,
            mapTypeId: google.maps.MapTypeId.HYBRID
        };

        highChartsController.gmMap = new google.maps.Map($("#map_canvas")[0], highChartsController.mapOptions);
        google.maps.event.addListenerOnce(highChartsController.gmMap, 'idle', function () {

        });
    },
    SetMarkerForGoogleMapForSharePointList: function(gmLatLng, gmMap, gmBounds, title, contentForInfoWindow, isExternalMarker) {
    var icon = "";
    if (isExternalMarker)
        icon = "yellow";
    else
        icon = "red";
    icon = "http://maps.google.com/mapfiles/ms/icons/" + icon + ".png";
    //var pos = new google.maps.LatLng(gmLatLng.k, gmLatLng.D);
    var pos = new google.maps.LatLng(gmLatLng.Latitude, gmLatLng.Longitude);
    
    var gmMarker = new google.maps.Marker({
        position: pos,
        map: gmMap,
        title: title,
        zIndex: 0,
        icon: new google.maps.MarkerImage(icon)
    });
    gmBounds.extend(pos);
    gmMap.setCenter(gmBounds.getCenter());
    gmMap.fitBounds(gmBounds);
    
    if (contentForInfoWindow != null && contentForInfoWindow != "") {
        var gmInfowindow = new google.maps.InfoWindow({
            content: contentForInfoWindow,
            maxWidth: 500,
            maxHeight: 600
        });
    
        google.maps.event.addListener(gmMarker, 'click', function () {
            gmInfowindow.open(gmMap, gmMarker);
            
        });
    }
},
    SetupHighChartsOperations : function()
    {
        //MonthNames.push({ ID: 0, Name: "January" });
        //MonthNames.push({ ID: 1, Name: "February" });
        //MonthNames.push({ ID: 2, Name: "March" });
        //MonthNames.push({ ID: 3, Name: "April" });

        //MonthNames.push({ ID: 4, Name: "May" });
        //MonthNames.push({ ID: 5, Name: "June" });
        //MonthNames.push({ ID: 6, Name: "July" });
        //MonthNames.push({ ID: 7, Name: "August" });

        //MonthNames.push({ ID: 8, Name: "September" });
        //MonthNames.push({ ID: 9, Name: "October" });
        //MonthNames.push({ ID: 10, Name: "November" });
        //MonthNames.push({ ID: 11, Name: "December" });

        // Sets and Exercise drop down selectors initializations
        //------------------------------------------------

        // You can only load excercises if the user has selected a set
        $('#userSets').change(function () {
            $("select option:selected").each(function () {
                if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val() != "none") {
                    UserFitnessDataHelper.Controllers.hsController.LoadUserExercises($(this).val());
                    highChartsController.LoadProperChartByUserSelection();
                }
            });
        });

        $('#chartTypes').change(function () {
            $("select option:selected").each(function () {
                if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val() != "none") {
                    highChartsController.ChartOptions.chart.type = $(this).val();
                    highChartsController.LoadProperChartByUserSelection();
                }
            });
        });

        

        $('#userExercises').change(function () {
            $("select option:selected").each(function () {
                if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val() != "none") {
                    highChartsController.LoadProperChartByUserSelection();
                }
            });
        });

        highChartsController.ChartOptions = this.GetAndSteupHighChartsBaseOptions();
        this.LoadUserSets();

        //------------------------------------------------

        

        $("#fitnessDataDateRangeSlider").bind("valuesChanged", function (e, data) {
            if (highChartsController.ResetCalled)
            {
                highChartsController.ResetCalled = false;
                return;
            }
            $("#exerciseStartDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.values.min)));
            $("#exerciseEndDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.values.max)));
            highChartsController.LoadProperChartByUserSelection();
        });

        $("#exerciseStartDatepicker").datepicker({
            dateFormat: Tools.DateTimeFormat,
            onSelect: function (dateText, inst) {
                //var date = $.datepicker.parseDate(inst.settings.dateFormat || $.datepicker._defaults.dateFormat, dateText, inst.settings);
                highChartsController.LoadProperChartByUserSelection();
                highChartsController.ResetCalled = true;
                var uiStartDate = new Date($("#exerciseStartDatepicker").val());
                var uiEndDate = new Date($("#exerciseEndDatepicker").val());
                $("#fitnessDataDateRangeSlider").dateRangeSlider("values", uiStartDate, uiEndDate);
            }
        });

        $("#exerciseEndDatepicker").datepicker({
            dateFormat: Tools.DateTimeFormat,
            onSelect: function (dateText, inst) {
                //var date = $.datepicker.parseDate(inst.settings.dateFormat || $.datepicker._defaults.dateFormat, dateText, inst.settings);
                highChartsController.LoadProperChartByUserSelection();
                highChartsController.ResetCalled = true;
                var uiStartDate = new Date($("#exerciseStartDatepicker").val());
                var uiEndDate = new Date($("#exerciseEndDatepicker").val());
                $("#fitnessDataDateRangeSlider").dateRangeSlider("values", uiStartDate, uiEndDate);
            }
        });

        $("#resetChartSelections").click(function () {
            $("#exerciseStartDatepicker").val($.datepicker.formatDate("DD, d M yy", highChartsController.DataDateRange.StartDate));
            $("#exerciseEndDatepicker").val($.datepicker.formatDate("DD, d M yy", highChartsController.DataDateRange.EndDate));
            $("#fitnessDataDateRangeSlider").dateRangeSlider("values", highChartsController.DataDateRange.StartDate, highChartsController.DataDateRange.EndDate);
            $("#userExercises").val("none");
            $("#userSets").val("none");
            $("#chartTypes").val("none");
            highChartsController.LoadProperChartByUserSelection();
            highChartsController.ResetCalled = true;
        });

        WebAPIHelper.Get("/api/ChartSet",
                            function (data) {
                                $("#exerciseStartDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.StartDate)));
                                $("#exerciseEndDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.EndDate)));
                                highChartsController.DataDateRange.StartDate = new Date(data.StartDate);
                                highChartsController.DataDateRange.EndDate = new Date(data.EndDate);
                                highChartsController.LoadProperChartByUserSelection();

                                $("#fitnessDataDateRangeSlider").dateRangeSlider({
                                    bounds: { min: highChartsController.DataDateRange.StartDate, max: highChartsController.DataDateRange.EndDate },
                                    defaultValues: { min: highChartsController.DataDateRange.StartDate, max: highChartsController.DataDateRange.EndDate },
                                    formatter: function (val) {
                                        return $.datepicker.formatDate("DD, d M yy", new Date(val));
                                    }
                                });
                            }, null);
        /*
        TODO: Incorporate ajaxcomplete for webapi complete requests:
        $( document ).ajaxComplete(function() {
  $( ".log" ).text( "Triggered ajaxComplete handler." );
});

Improving code logic
        */

        highChartsController.SignalRWebUpdateRequestedByMobileClient();
    },
    SignalRWebUpdateRequestedByMobileClient: function()
    {
        var connection = $.hubConnection(Constants.SignalRGatewayLocation);
        var contosoChatHubProxy = connection.createHubProxy(Constants.SignalRHubProxyName);
        contosoChatHubProxy.on(Constants.SignalRHubMethod_IsDataUpdateRequiredForWeb, function (name, isRequired, message) {
            // Html encode display name and message.
            var encodedName = $('<div />').text(name).html();
            //var encodedMsg = $('<div />').text("isDataUpdateRequiredForWeb is Update Required: " + isRequired + " Message: " + message).html();
            var encodedMsg = $('<div />').text("Updating UI. New data from the mobile app.").html();
            // Add the message to the page.
            $('#notifications').append('<ul><li><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li></ul>');
            highChartsController.LoadProperChartByUserSelection();
        });
        connection.start()
            .done(function () { console.log('Now connected, connection ID=' + connection.id); })
            .fail(function () { console.log('Could not connect'); });
    },
    LoadProperChartByUserSelection : function ()
    {
        if (highChartsController.ChartOptions != null) {
            
                highChartsController.ChartOptions.series = [];
        }
        // No set was selected by the user so show all data
        if ($('#userSets').find(":selected").val() == "none")
        {
            highChartsController.LoadChartForSets();
        // If no exercise was selected and a set was selected then show the set data
        } else if ($('#userExercises').find(":selected").val() == "none")
        {
            highChartsController.LoadChartForSelectedSet($('#userSets').find(":selected").val());
            // If a set was selected and a exercise was selected then show the exercise
        } else if ($('#userExercises').find(":selected").val() != "none")
        {
            highChartsController.LoadChartForSelectedExercise($('#userSets').find(":selected").val(), $('#userExercises').find(":selected").val());
        }
    },
    LoadDataPanelForSelectedExerciseRecord: function (exerciseRecordId) {
        WebAPIHelper.Get("/api/ExerciseRecords/" + exerciseRecordId,
                            function (data) {
                                // Process here the record data
                                var x = 0;
                                // Process attributes for the record
                                $.each(data.ExerciseRecordAttributes, function (index, item) {
                                    // Add Name(Attribute) identification
                                    var obj = JSON.parse(item.Data);
                                    var geoLocationIndex = 0;

                                    // Notice the length - 1 => means that we want to stop one location too son to not pass go all the way to the last position and pass as the next position undefined data
                                    while(geoLocationIndex < obj.length -1)
                                    {
                                        var gDistanceMatrix = new google.maps.DistanceMatrixService();
                                        var time = obj[geoLocationIndex + 1].LocationTime - obj[geoLocationIndex].LocationTime;
                                        var destination = obj[geoLocationIndex + 1];

                                        var flightPlanCoordinates = [
                                            new google.maps.LatLng(obj[geoLocationIndex].Latitude, obj[geoLocationIndex].Longitude),
                                            new google.maps.LatLng(obj[geoLocationIndex +1].Latitude, obj[geoLocationIndex +1].Longitude)
                                        ];
                                        var flightPath = new google.maps.Polyline({
                                            path: flightPlanCoordinates,
                                            geodesic: true,
                                            strokeColor: '#FF0000',
                                            strokeOpacity: 1.0,
                                            strokeWeight: 3
                                        });

                                        flightPath.setMap(highChartsController.gmMap);

                                        // Get the distance between the two points
                                        gDistanceMatrix.getDistanceMatrix({
                                            origins: [new google.maps.LatLng(obj[geoLocationIndex].Latitude, obj[geoLocationIndex].Longitude)],
                                            destinations: [new google.maps.LatLng(obj[geoLocationIndex +1].Latitude, obj[geoLocationIndex +1].Longitude)],
                                            travelMode: google.maps.TravelMode.DRIVING
                                        },function (time, destination) {
                                            return (function (response, status) {
                                                if (status == google.maps.DistanceMatrixStatus.OK) {
                                                    var origins = response.originAddresses;
                                                    var destinations = response.destinationAddresses;

                                                    for (var i = 0; i < origins.length; i++) {
                                                        var results = response.rows[i].elements;
                                                        for (var j = 0; j < results.length; j++) {
                                                            var element = results[j];
                                                            var distance = element.distance.text;
                                                            var duration = element.duration.text;
                                                            var from = origins[i];
                                                            var to = destinations[j];
                                                            var msgForInfoWindow = "";
                                                            msgForInfoWindow += "<b>From: </b>" + from + "</br>";
                                                            msgForInfoWindow += "<b>To: </b>" + to + "</br>";
                                                            msgForInfoWindow += "<b>Distance: </b>" + distance + "</br>";
                                                            msgForInfoWindow += "<b>Lap time: </b>" + (time / 1000) + " seconds</br>";
                                                            highChartsController.SetMarkerForGoogleMapForSharePointList(destination, highChartsController.gmMap, highChartsController.gmBounds, "Your track time.", msgForInfoWindow, true);
                                                        }
                                                    }
                                                }
                                            });
                                        }(time, destination));
                                        
                                        
                                        // Skip only once since we want to move from one location to another and get the distance from each data point
                                        geoLocationIndex++;
                                    }
                                    // process geo location data
                                    //$.each(data.obj, function (index, itemObj) {
                                    //    // Add Name(Attribute) identification
                                        
                                    //    var msgForInfoWindow = "";
                                    //    highChartsController.SetMarkerForGoogleMapForSharePointList(itemObj, highChartsController.gmMap, highChartsController.gmBounds, "Your track time.", "TEST TEST TEST 123#", true);
                                    //});
                                });
                            },
                            null);
    },
    LoadChartForSelectedExercise: function (setId, exerciseId) {
        var uiStartDate = new Date($("#exerciseStartDatepicker").val()).toLocaleDateString(CookieHelper.ServerLanguage);
        var uiEndDate = new Date($("#exerciseEndDatepicker").val()).toLocaleDateString(CookieHelper.ServerLanguage);

        /*http://localhost:52797/api/ColumnDataHighChart?startDate=1.10.2014&endDate=8.1.2015*/
        WebAPIHelper.Get("/api/ColumnDataHighChart",
                            function (setsData) {
                                //highChartsController.UserOverallDataByDateRange = setsData;
                                if (setsData.Series.length <= 0)
                                {
                                    highChartsController.Chart.destroy();
                                    return;
                                }
                                if (highChartsController.ChartOptions != null) {

                                    highChartsController.ChartOptions.series = [];
                                }

                                if (setsData.Series.length > 0) {
                                    $.each(setsData.Series, function (index, item) {
                                        highChartsController.ChartOptions.series.push({
                                            id: item.ID,
                                            name: item.Name,
                                            data: item.Data,
                                            uiStartDate: uiStartDate,
                                            uiEndDate: uiEndDate
                                        });
                                    });
                                }

                                highChartsController.ChartOptions.title = setsData.Title;
                                highChartsController.ChartOptions.subtitle = setsData.SubTitle;

                                if (setsData.xAxisCategories.length > 0) {
                                    $.each(setsData.xAxisCategories, function (index, item) {
                                        highChartsController.ChartOptions.xAxis.categories = setsData.xAxisCategories;
                                    });
                                }

                                highChartsController.ChartOptions.yAxis.title = setsData.yAxisTitle;

                                //$('#userSets').empty();
                                //$('#userSets').append(
                                //         $('<option></option>').val("").html("-- Choose a set --")
                                //     );

                                //$.each(data, function (index, item) {
                                //    $('#userSets').append(
                                //         $('<option></option>').val(item.Id).html(item.Name)
                                //     );
                                //});
                                highChartsController.Chart = new Highcharts.Chart(highChartsController.ChartOptions);
                            }, null,
                            { setId: setId, exerciseId: exerciseId, startDate: uiStartDate, endDate: uiEndDate });
    },
    LoadChartForSelectedSet: function (setId) {
        
        var uiStartDate = new Date($("#exerciseStartDatepicker").val()).toLocaleDateString(CookieHelper.ServerLanguage);
        var uiEndDate = new Date($("#exerciseEndDatepicker").val()).toLocaleDateString(CookieHelper.ServerLanguage);

        /*http://localhost:52797/api/ColumnDataHighChart?startDate=1.10.2014&endDate=8.1.2015*/
        WebAPIHelper.Get("/api/ColumnDataHighChart",
                            function (setsData) {
                                //highChartsController.UserOverallDataByDateRange = setsData;
                                if (setsData.Series.length <= 0) {
                                    highChartsController.Chart.destroy();
                                    return;
                                }

                                if (highChartsController.ChartOptions != null) {

                                    highChartsController.ChartOptions.series = [];
                                }

                                $.each(setsData.Series, function (index, item) {
                                    highChartsController.ChartOptions.series.push({
                                        id: item.ID,
                                        name: item.Name,
                                        data: item.Data
                                    });
                                });

                                highChartsController.ChartOptions.title = setsData.Title;
                                highChartsController.ChartOptions.subtitle = setsData.SubTitle;

                                $.each(setsData.xAxisCategories, function (index, item) {
                                    highChartsController.ChartOptions.xAxis.categories = setsData.xAxisCategories;
                                });

                                highChartsController.ChartOptions.yAxis.title = setsData.yAxisTitle;

                                //$('#userSets').empty();
                                //$('#userSets').append(
                                //         $('<option></option>').val("").html("-- Choose a set --")
                                //     );

                                //$.each(data, function (index, item) {
                                //    $('#userSets').append(
                                //         $('<option></option>').val(item.Id).html(item.Name)
                                //     );
                                //});
                                highChartsController.Chart = new Highcharts.Chart(highChartsController.ChartOptions);
                            }, null,
                            { setId: setId, startDate: uiStartDate, endDate: uiEndDate });
    },
    LoadChartForSets: function () {
        var uiStartDate = new Date($("#exerciseStartDatepicker").val()).toLocaleDateString(CookieHelper.ServerLanguage);
        var uiEndDate = new Date($("#exerciseEndDatepicker").val()).toLocaleDateString(CookieHelper.ServerLanguage);

        /*http://localhost:52797/api/ColumnDataHighChart?startDate=1.10.2014&endDate=8.1.2015*/
        WebAPIHelper.Get("/api/ColumnDataHighChart",
                            function (setsData) {
                                //highChartsController.UserOverallDataByDateRange = setsData;
                                if (setsData.Series.length <= 0) {
                                    highChartsController.Chart.destroy();
                                    return;
                                }
                                if (highChartsController.ChartOptions != null) {

                                    highChartsController.ChartOptions.series = [];
                                }
                                $.each(setsData.Series, function (index, item) {
                                    highChartsController.ChartOptions.series.push({
                                        id: item.ID,
                                        name: item.Name,
                                        data: item.Data
                                    });
                                });

                                highChartsController.ChartOptions.title = setsData.Title;
                                highChartsController.ChartOptions.subtitle = setsData.SubTitle;

                                $.each(setsData.xAxisCategories, function (index, item) {
                                    highChartsController.ChartOptions.xAxis.categories = setsData.xAxisCategories;
                                });

                                highChartsController.ChartOptions.yAxis.title = setsData.yAxisTitle;

                                //$('#userSets').empty();
                                //$('#userSets').append(
                                //         $('<option></option>').val("").html("-- Choose a set --")
                                //     );

                                //$.each(data, function (index, item) {
                                //    $('#userSets').append(
                                //         $('<option></option>').val(item.Id).html(item.Name)
                                //     );
                                //});
                                highChartsController.Chart = new Highcharts.Chart(highChartsController.ChartOptions);
                            }, null,
                            { startDate: uiStartDate, endDate: uiEndDate });

        

        /*
     options.series.push({
     name: 'John',
     data: [3, 4, 2]
    })
     */

        

        //return (chart);
    },

    LoadUserSets: function () {
        WebAPIHelper.Get(   "/api/sets",
                            function (data) {
                                $('#userSets').empty();
                                $('#userSets').append(
                                         $('<option></option>').val("none").html("-- Choose a set --")
                                     );

                                $.each(data, function (index, item) {
                                    $('#userSets').append(
                                         $('<option></option>').val(item.Id).html(item.Name)
                                     );
                                });
                            }, null);
    },

    LoadUserExercises: function (setId) {
        WebAPIHelper.Get("/api/sets/" + setId,
                            function (data) {
                                $('#userExercises').empty();
                                $('#userExercises').append(
                                         $('<option></option>').val("none").html("-- Choose an exercise --")
                                     );

                                $.each(data.Exercises, function (index, item) {
                                    $('#userExercises').append(
                                         $('<option></option>').val(item.Id).html(item.Name)
                                     );
                                });
                            },
                            null);
    },
    GetAndSteupHighChartsBaseOptions: function () {
        var options = {
            chart: {
                renderTo: 'container',
                type: 'column'
            },
            title: {
                text: "Your exercise data"
            },
            xAxis: { categories: [] },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            yAxis: { title: { text: 'no yAxis title' } },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                },
                series: {
                    cursor: 'pointer',
                    allowPointSelect: true,
                    point: {
                        events: {
                            click: function () {
                                // If No set was selected by the user so show show the set data
                                if ($('#userSets').find(":selected").val() == "none") {
                                    //highChartsController.LoadChartForSelectedSet($('#userSets').find(":selected").val());
                                    $("#userSets").val(this.series.userOptions.id).change();
                                    // If no exercise was selected and a set was selected then show the exercise data
                                } else if ($('#userExercises').find(":selected").val() == "none") {
                                    //highChartsController.LoadChartForSelectedExercise($('#userSets').find(":selected").val(), $('#userExercises').find(":selected").val());
                                    $("#userExercises").val(this.series.userOptions.id).change();
                                    // If a set was selected and a exercise was selected then show the exercise record if there is a single record selected and not a date with multiple values calculated
                                } else if ($('#userExercises').find(":selected").val() != "none") {
                                    if(this.series.userOptions.id <= 0)
                                    {
                                        alert("Multiple values(calculated values, nested) selection not finnished!");
                                    } else
                                    {
                                        highChartsController.CreateGoogleMapsObject();
                                        highChartsController.LoadDataPanelForSelectedExerciseRecord(this.series.userOptions.id);
                                    }
                                }
                                alert('Category: ' + this.category + ', value: ' + this.y + ', id: ' + this.series.userOptions.id);
                            }
                        }
                    }
                }
            },
            series: []
        }
        return (options);
    }
};
