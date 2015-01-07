/*

var myFitnessDataController = function ($scope, authService, userFactory) {
    $scope.models = {
        helloAngular: 'I work!'
    };

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    getFormData();
    function getFormData() {
        userFactory.getFormData(function (results) {
            $scope.sets = results;
        })
    }
    $scope.Save = function () {
        $scope.message = "User Data Submitted"
    }

    $scope.authentication = authService.authentication;
}

// The inject property of every controller (and pretty much every other type of object in Angular) needs to be a string array equal to the controllers arguments, only as strings
myFitnessDataController.$inject = ['$scope', 'authService', 'myFitnessDataFactory'];

//// Creating a module
//var myFitnessDataApp = angular.module("myFitnessDataApp", [])

//// Creating a controller from the module
//myFitnessDataApp.controller("myFitnessDataController", function ($scope) {
//})*/


