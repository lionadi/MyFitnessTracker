var highChartsController = {
    
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
        var options = GetAndSteupHighChartsBaseOptions();

        $.each(setsData, function (index, item) {
            options.series.push({
                name: item.Name,
                data: [3, 4, 2]
            });
        });

        /*
     options.series.push({
     name: 'John',
     data: [3, 4, 2]
    })
     */

        var chart = new Highcharts.Chart(options);

        return (chart);
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
                type: 'bar'
            },
            title: {
                text: "Your exercise data"
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} mm</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    pointPadding: 0.2,
                    borderWidth: 0
                }
            },
            series: [{}]
        }
        return (options);
    }
};
