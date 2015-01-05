var CookieID_loginTokenType = "CookieID_loginTokenType";
var CookieID_loginTokenData = "CookieID_loginTokenData";
var webAPILocation = "http://localhost:52797"

function LoginUserToWebAPI()
{
    

    var password = $("#Password").val();
    var userName = $("#Email").val();
    $.ajax({
        url: webAPILocation + "/Token",
        type: "Post",
        data: "grant_type=password&password=" + encodeURI(password) + "&username=" + encodeURI(userName),
        success: function (data) {
            loginToken = data;
            $.cookie(CookieID_loginTokenData, data.access_token, { path: "/" });
            $.cookie(CookieID_loginTokenType, data.token_type, { path: "/" });
        },
        error: function () { alert('error'); }
    });
}

function LogOutUserToWebAPI() {
    $.removeCookie(CookieID_loginTokenData, { path: "/" });
    $.removeCookie(CookieID_loginTokenType, { path: "/" });
}



function GetAndSteupHighChartsBaseOptions() {
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
    };

 

    return (options);
}

function SetupHTMLControlsSettings()
{
    // DateTimePickers
    //------------------------------------------------
    $('.datepicker').datepicker(); //Initialise any date pickers with CSS class datepicker
    $("#exerciseStartDatepicker").datepicker();
    $("#exerciseEndDatepicker").datepicker();

    // Charts initializations
    //------------------------------------------------
    GetAndSteupHighChartsBaseOptions();

    // Sets and Exercise drop down selectors initializations
    //------------------------------------------------

    // You can only load excercises if the user has selected a set
    $('#userSets').change(function () {
        $("select option:selected").each(function () {
            if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val()) {
                LoadUserExercises($(this).val());
            }
        });
    });

    $('#userSets').change(function () {
        $("select option:selected").each(function () {
            if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val()) {
                LoadUserExercises($(this).val());
            }
        });
    });
}

function LoadChartForSelectedExercise(exerciseId)
{
    var options = GetAndSteupHighChartsBaseOptions();

    var chart = new Highcharts.Chart(options);

    return (chart);
}

function LoadChartForSelectedSet(exerciseId) {
    var options = GetAndSteupHighChartsBaseOptions();

    var chart = new Highcharts.Chart(options);

    return (chart);
}

function LoadChartForSets(exerciseId, setsData) {
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
}

function LoadUserSets() {
    var loginToken = $.cookie(CookieID_loginTokenData);
    var loginTokenType = $.cookie(CookieID_loginTokenType);
    if ((loginToken === null) != true && (typeof loginToken === 'undefined') != true) {
        if ((loginTokenType === null) != true && (typeof loginTokenType === 'undefined') != true) {
            $.ajax({
                beforeSend: function (request) {
                    request.setRequestHeader("Accept", 'application/json');
                    request.setRequestHeader("Authorization", loginTokenType + " " + loginToken);
                    request.setRequestHeader("Content-Type", 'application/json');
                },
                dataType: "json",
                url: webAPILocation + "/api/sets",
                success: function (data) {
                    $('#userSets').empty();
                    $('#userSets').append(
                             $('<option></option>').val("").html("-- Choose a set --")
                         );

                    $.each(data, function (index, item) {
                        $('#userSets').append(
                             $('<option></option>').val(item.Id).html(item.Name)
                         );
                    });
                }
            });
        }
    }
}

function LoadUserExercises(setId)
{
    var loginToken = $.cookie(CookieID_loginTokenData);
    var loginTokenType = $.cookie(CookieID_loginTokenType);
    if ((loginToken === null) != true && (typeof loginToken === 'undefined') != true) {
        if ((loginTokenType === null) != true && (typeof loginTokenType === 'undefined') != true) {
            $.ajax({
                beforeSend: function (request) {
                    request.setRequestHeader("Accept", 'application/json');
                    request.setRequestHeader("Authorization", loginTokenType + " " + loginToken);
                    request.setRequestHeader("Content-Type", 'application/json');
                },
                dataType: "json",
                url: webAPILocation + "/api/sets/" + setId,
                success: function (data) {
                    $('#userExercises').empty();
                    $('#userExercises').append(
                             $('<option></option>').val("").html("-- Choose an exercise --")
                         );

                    $.each(data.Exercises, function (index, item) {
                        $('#userExercises').append(
                             $('<option></option>').val(item.Id).html(item.Name)
                         );
                    });
                }
            });
        }
    }
}

$(document).ready(function () {
    LoadUserSets();
    SetupHTMLControlsSettings();
});