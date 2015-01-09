
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
            UserFitnessDataHelper.SetupHTMLControlsSettings();
            UserFitnessDataHelper.Controllers.hsController.SetupHighChartsOperations();
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
            this.Controllers.hsController.GetAndSteupHighChartsBaseOptions();

            // Sets and Exercise drop down selectors initializations
            //------------------------------------------------

            // You can only load excercises if the user has selected a set
            $('#userSets').change(function () {
                $("select option:selected").each(function () {
                    if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val()) {
                        UserFitnessDataHelper.Controllers.hsController.LoadUserExercises($(this).val());
                    }
                });
            });

            $('#userExercises').change(function () {
                $("select option:selected").each(function () {
                    if (($(this).val() === null) != true && (typeof $(this).val() === 'undefined') != true && $(this).val()) {
                        UserFitnessDataHelper.Controllers.hsController.LoadUserExercises($(this).val());
                    }
                });
            });
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