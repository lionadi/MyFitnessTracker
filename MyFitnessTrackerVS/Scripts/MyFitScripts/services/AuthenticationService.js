var authenticationService = {
    LoginUserToWebAPI: function () {
        var password = $("#Password").val();
        var userName = $("#Email").val();
        $.ajax({
            url: Constants.WebAPILocation + "/Token",
            type: "Post",
            data: "grant_type=password&password=" + encodeURI(password) + "&username=" + encodeURI(userName),
            success: function (data, status, xhr) {
                loginToken = data;
                var language = xhr.getResponseHeader("Accept-Language");
                if (!Tools.IsEmpty(language))
                {
                    var languageArray = language.split(";");
                    if (!Tools.IsEmpty(languageArray))
                        $.cookie(Constants.CookieID_ServerLanguage, languageArray[0], { path: "/" });
                }
                $.cookie(Constants.CookieID_loginTokenData, data.access_token, { path: "/" });
                $.cookie(Constants.CookieID_loginTokenType, data.token_type, { path: "/" });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var x = 0;
            }
        });
    },
    LogOutUserToWebAPI : function() {
        $.removeCookie(Constants.CookieID_loginTokenData, { path: "/" });
        $.removeCookie(Constants.CookieID_loginTokenType, { path: "/" });
    }

};

