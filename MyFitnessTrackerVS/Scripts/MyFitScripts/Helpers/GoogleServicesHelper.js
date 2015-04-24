var GoogleServicesHelper = {
    GetDistance: function (origin, destication, successEventFunction, errorEventFunction) {
        if (!Tools.IsEmpty(CookieHelper.LoginToken === null)) {
            if (!Tools.IsEmpty(CookieHelper.LoginTokenType === null)) {
                $.ajax({
                    url: Constants.GoogleDistanceMatrixURL + "/json?origins=" + origin.Latitude + "," + origin.Longitude + "&destinations=" + destication.Latitude + "," + destication.Longitude,
                    success: successEventFunction,
                    error: !Tools.IsEmpty(errorEventFunction) ? errorEventFunction : function (jqXHR, textStatus, errorThrown) {
                        var x = 0;
                    }
                });
            }
        }
    }
};