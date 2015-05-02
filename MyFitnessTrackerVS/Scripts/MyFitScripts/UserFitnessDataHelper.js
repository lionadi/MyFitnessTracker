
var UserFitnessDataHelper =
    {
        IsUserLoggedOn : function()
        {
            if (!Tools.IsEmpty(CookieHelper.LoginToken) && !Tools.IsEmpty(CookieHelper.LoginTokenType))
                return true;

            return false;
        },
        Services: { authService : authenticationService },
        Controllers: { hsController: highChartsController },
        SetupUserFitnessApp: function()
        {
            UserFitnessDataHelper.GetServerStatus();
            UserFitnessDataHelper.SetupHTMLControlsSettings();
            
        },
        GetServerStatus : function()
        {
            WebAPIHelper.Get("/api/ServerStatus",
                            function (data) {
                                $.cookie(Constants.CookieID_ServerLanguage, data.LanguageCode, { path: "/" });
                            }, null);
        },
        SetupHTMLControlsSettings: function ()
        {
            // DateTimePickers
            //------------------------------------------------
            $('.datepicker').datepicker({
                dateFormat: Tools.DateTimeFormat
            }); //Initialise any date pickers with CSS class datepicker
            $('#userFitnessData').toggle("fast");
            $('#userNoLogOn').toggle("fast");

            // Charts initializations
            //------------------------------------------------
            UserFitnessDataHelper.Controllers.hsController.SetupHighChartsOperations();
        }  
    };







$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    var t = 0;
});




$(document).ready(function () {
    //var password ="Qwerty123!";
    //var userName = "demo@account.com";
    //$.ajax({
    //    url: Constants.WebAPILocation + "/Token",
    //    type: "Post",
    //    data: "grant_type=password&password=" + encodeURI(password) + "&username=" + encodeURI(userName),
    //    success: function (data) {
    //        loginToken = data;
            

    //        $.cookie(Constants.CookieID_loginTokenData, data.access_token, { path: "/" });
    //        $.cookie(Constants.CookieID_loginTokenType, data.token_type, { path: "/" });
    //    },
    //    error: function (jqXHR, textStatus, errorThrown) {
    //        var x = 0;

    //    }
    //});
    //var connection = $.hubConnection();
    

    $('#userFitnessData').hide();
    $('#userNoLogOn').show();

    // Perform any loading data when the user has logged in
    if (UserFitnessDataHelper.IsUserLoggedOn()) {
        UserFitnessDataHelper.SetupUserFitnessApp();
    }
    else {
        $('#userFitnessData').toggle("fast");
        $('#userNoLogOn').toggle("fast");
    }
    
    
    
});