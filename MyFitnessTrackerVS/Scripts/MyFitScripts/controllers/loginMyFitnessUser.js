var LoginController = function ($scope, $location, authService, ngAuthSetting) {
    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };
    $scope.myAuthService = authService;
    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            // DO something after the login to web api

        },
         function (err) {
             $scope.message = err.error_description;
         });
    };
}

LoginController.$inject = ['$scope', '$location', 'authService', 'ngAuthSettings'];