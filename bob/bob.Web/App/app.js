(function () {
    'use strict';
    var appModule = angular.module('app', ['ngRoute']);

    appModule.config(function ($routeProvider) {
        $routeProvider.when('/', {
            templateUrl: '/App/ArbolView.html',
            controller: ''
            controllerAs: ''
        });
    });
})();
