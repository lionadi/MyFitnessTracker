var highChartsController = {
    Chart: null,
    ChartOptions: null,
    UserOverallDataByDateRange : [],
    SetupHighChartsOperations : function()
    {
        highChartsController.ChartOptions = this.GetAndSteupHighChartsBaseOptions();

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
        this.LoadUserSets();

        //------------------------------------------------

        $("#exerciseStartDatepicker").datepicker({
            dateFormat: Tools.DateTimeFormat,
            onSelect: function (dateText, inst) {
                //var date = $.datepicker.parseDate(inst.settings.dateFormat || $.datepicker._defaults.dateFormat, dateText, inst.settings);
                highChartsController.LoadProperChartByUserSelection();
            }
        });

        $("#exerciseEndDatepicker").datepicker({
            dateFormat: Tools.DateTimeFormat,
            onSelect: function (dateText, inst) {
                //var date = $.datepicker.parseDate(inst.settings.dateFormat || $.datepicker._defaults.dateFormat, dateText, inst.settings);
                highChartsController.LoadProperChartByUserSelection();
            }
        });

        WebAPIHelper.Get("/api/ChartSet",
                            function (data) {
                                $("#exerciseStartDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.StartDate)));
                                $("#exerciseEndDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.EndDate)));
                                highChartsController.LoadProperChartByUserSelection();
                            }, null);
        
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
                                if (setsData.Series.length > 0) {
                                    $.each(setsData.Series, function (index, item) {
                                        highChartsController.ChartOptions.series.push({
                                            name: item.Name,
                                            data: item.Data
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

                                $.each(setsData.Series, function (index, item) {
                                    highChartsController.ChartOptions.series.push({
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
                                
                                $.each(setsData.Series, function (index, item) {
                                    highChartsController.ChartOptions.series.push({
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
                }
            },
            series: []
        }
        return (options);
    }
};
