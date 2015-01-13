var WebAPIHelper = {
    Get: function (getWebAPIFunctionPath, successEventFunction, errorEventFunction) {
        if (!Tools.IsEmpty(CookieHelper.LoginToken === null)) {
            if (!Tools.IsEmpty(CookieHelper.LoginTokenType === null)) {
                $.ajax({
                    beforeSend: function (request) {
                        request.setRequestHeader("Accept", 'application/json');
                        request.setRequestHeader("Authorization", CookieHelper.LoginTokenType + " " + CookieHelper.LoginToken);
                        request.setRequestHeader("Content-Type", 'application/json');
                    },
                    dataType: "json",
                    url: Constants.WebAPILocation + getWebAPIFunctionPath,
                    success: successEventFunction,
                    error: !Tools.IsEmpty(errorEventFunction) ? errorEventFunction : function (jqXHR, textStatus, errorThrown) {
                        var x = 0;
                    }
                });
            }
        }
    },
    Get: function (getWebAPIFunctionPath, successEventFunction, errorEventFunction, getCallData) {
        if (!Tools.IsEmpty(CookieHelper.LoginToken === null)) {
            if (!Tools.IsEmpty(CookieHelper.LoginTokenType === null)) {
                $.ajax({
                    beforeSend: function (request) {
                        request.setRequestHeader("Accept", 'application/json');
                        request.setRequestHeader("Authorization", CookieHelper.LoginTokenType + " " + CookieHelper.LoginToken);
                        request.setRequestHeader("Content-Type", 'application/json');
                    },
                    dataType: "json",
                    data: getCallData,
                    url: Constants.WebAPILocation + getWebAPIFunctionPath,
                    success: successEventFunction,
                    error: !Tools.IsEmpty(errorEventFunction) ? errorEventFunction : function (jqXHR, textStatus, errorThrown) {
                        var x = 0;
                    }
                });
            }
        }
    }
};