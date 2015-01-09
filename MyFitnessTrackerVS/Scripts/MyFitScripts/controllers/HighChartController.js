var highChartsController = {
    SetupHighChartsOperations : function()
    {
        this.LoadUserSets();

        $("#exerciseStartDatepicker").datepicker({
            dateFormat: Tools.DateTimeFormat,
            onSelect: function (dateText, inst) {
                //var date = $.datepicker.parseDate(inst.settings.dateFormat || $.datepicker._defaults.dateFormat, dateText, inst.settings);
                highChartsController.LoadChartForSets();
            }
        });

        $("#exerciseEndDatepicker").datepicker({
            dateFormat: Tools.DateTimeFormat,
            onSelect: function (dateText, inst) {
                //var date = $.datepicker.parseDate(inst.settings.dateFormat || $.datepicker._defaults.dateFormat, dateText, inst.settings);
                highChartsController.LoadChartForSets();
            }
        });

        WebAPIHelper.Get("/api/ChartSet",
                            function (data) {
                                $("#exerciseStartDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.StartDate)));
                                $("#exerciseEndDatepicker").val($.datepicker.formatDate("DD, d M yy", new Date(data.EndDate)));
                                highChartsController.LoadChartForSets();
                            },
                            function (jqXHR, textStatus, errorThrown) {
                                var x = 0;
                            });
        
        
    },
    LoadChartForSelectedExercise: function (exerciseId) {
        var options = GetAndSteupHighChartsBaseOptions();

        var chart = new Highcharts.Chart(options);

        return (chart);
    },
    LoadChartForSelectedSet: function (exerciseId) {
        var options = GetAndSteupHighChartsBaseOptions();

        var chart = new Highcharts.Chart(options);

        return (chart);
    },
    LoadChartForSets: function (exerciseId, setsData) {
        var options = this.GetAndSteupHighChartsBaseOptions();
        var uiStartDate = $.datepicker.formatDate("d.m.yy", new Date($("#exerciseStartDatepicker").val()));
        var uiEndDate = $.datepicker.formatDate("d.m.yy", new Date($("#exerciseEndDatepicker").val()));

        /*http://localhost:52797/api/ColumnDataHighChart?startDate=1.10.2014&endDate=8.1.2015*/
        WebAPIHelper.Get("/api/ColumnDataHighChart",
                            function (setsData) {
                                var t = 0;
                                $.each(setsData.Series, function (index, item) {
                                    options.series.push({
                                        name: item.Name,
                                        data: item.Data
                                    });
                                });

                                options.title = setsData.Title;
                                options.subtitle = setsData.SubTitle;

                                $.each(setsData.xAxisCategories, function (index, item) {
                                    options.xAxis.categories = setsData.xAxisCategories;
                                });

                                options.yAxis.title = setsData.yAxisTitle;

                                //$('#userSets').empty();
                                //$('#userSets').append(
                                //         $('<option></option>').val("").html("-- Choose a set --")
                                //     );

                                //$.each(data, function (index, item) {
                                //    $('#userSets').append(
                                //         $('<option></option>').val(item.Id).html(item.Name)
                                //     );
                                //});
                                var chart = new Highcharts.Chart(options);
                            },
                            function (jqXHR, textStatus, errorThrown) {
                                var x = 0;
                            },
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
                                         $('<option></option>').val("").html("-- Choose a set --")
                                     );

                                $.each(data, function (index, item) {
                                    $('#userSets').append(
                                         $('<option></option>').val(item.Id).html(item.Name)
                                     );
                                });
                            },
                            function (jqXHR, textStatus, errorThrown) {
                                var x = 0;
                            });
    },

    LoadUserExercises: function (setId) {
        WebAPIHelper.Get("/api/sets/" + setId,
                            function (data) {
                                $('#userExercises').empty();
                                $('#userExercises').append(
                                         $('<option></option>').val("").html("-- Choose an exercise --")
                                     );

                                $.each(data.Exercises, function (index, item) {
                                    $('#userExercises').append(
                                         $('<option></option>').val(item.Id).html(item.Name)
                                     );
                                });
                            },
                            function (jqXHR, textStatus, errorThrown) {
                                var x = 0;
                            });
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
