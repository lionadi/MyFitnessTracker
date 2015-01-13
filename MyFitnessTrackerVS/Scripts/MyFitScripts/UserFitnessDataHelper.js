
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












$(document).ready(function () {
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