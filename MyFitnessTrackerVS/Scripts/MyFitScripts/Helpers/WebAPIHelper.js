var WebAPIHelper = {
    Get: function (getWebAPIFunctionPath, successEventFunction, errorEventFunction) {
        if ((CookieHelper.LoginToken === null) != true && (typeof CookieHelper.LoginToken === 'undefined') != true) {
            if ((CookieHelper.LoginTokenType === null) != true && (typeof CookieHelper.LoginTokenType === 'undefined') != true) {
                $.ajax({
                    beforeSend: function (request) {
                        request.setRequestHeader("Accept", 'application/json');
                        request.setRequestHeader("Authorization", CookieHelper.LoginTokenType + " " + CookieHelper.LoginToken);
                        request.setRequestHeader("Content-Type", 'application/json');
                    },
                    dataType: "json",
                    url: Constants.WebAPILocation + getWebAPIFunctionPath,
                    success: successEventFunction,
                    error: errorEventFunction
                });
            }
        }
    }
};