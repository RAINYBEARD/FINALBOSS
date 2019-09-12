(function () {
    'use strict';

    angular.module('bob').factory('authInterceptorService', function ($q, $rootScope, localStorageService) {

    var authInterceptorServiceFactory = {};

        var _request = function (config) {

            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'bearer ' + authData.token;
            }

            return config;
        };

        var _responseError = function (rejection) {
            if (rejection.status === 401) {
                $rootScope.$emit("unauthorized");
            }
            return $q.reject(rejection);
        };

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
    });
})();
