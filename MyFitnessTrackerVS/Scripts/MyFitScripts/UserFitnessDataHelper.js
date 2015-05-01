
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
    var connection = $.hubConnection(Constants.SignalRGatewayLocation);
    var contosoChatHubProxy = connection.createHubProxy(Constants.SignalRHubProxyName);
    contosoChatHubProxy.on(Constants.SignalRHubMethod_IsDataUpdateRequiredForWeb, function (name, isRequired, message) {
        // Html encode display name and message.
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text("isDataUpdateRequiredForWeb is Update Required: " + isRequired + " Message: " + message).html();
        // Add the message to the page.
        $('#notifications').append('<ul><li><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li></ul>');
    });
    connection.start()
        .done(function () { console.log('Now connected, connection ID=' + connection.id); })
        .fail(function () { console.log('Could not connect'); });

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