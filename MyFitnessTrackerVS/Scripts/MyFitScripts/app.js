
var app = angular.module('myFitnessDataApp', ['LocalStorageModule', 'angular-loading-bar']);

// Creating a factory from the module
app.factory('myFitnessDataFactory', function ($http) {
    return {
        getFormData: function (callback) {
            $http.get('http://localhost:52797/api/sets').success(callback);
        }
    }
});

var serviceBase = 'http://localhost:52797/';
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.controller('myFitnessDataController', myFitnessDataController);
app.controller('LoginController', LoginController);

var configFunction = function ($httpProvider) {
    
    $httpProvider.interceptors.push('authInterceptorService');
}
configFunction.$inject = ['$httpProvider'];

app.config(configFunction);

app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

var x = 0;