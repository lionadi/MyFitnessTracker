
var UserFitnessDataHelper =
    {
        Services: { authService : authenticationService },
        Controllers: { hsController: highChartsController },
        SetupUserFitnessApp: function()
        {
            UserFitnessDataHelper.SetupHTMLControlsSettings();
            UserFitnessDataHelper.Controllers.hsController.LoadUserSets();
        },
        SetupHTMLControlsSettings: function ()
        {
            // DateTimePickers
            //------------------------------------------------
            $('.datepicker').datepicker(); //Initialise any date pickers with CSS class datepicker
            $("#exerciseStartDatepicker").datepicker();
            $("#exerciseEndDatepicker").datepicker();

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
    UserFitnessDataHelper.SetupUserFitnessApp();
    
    
    
});