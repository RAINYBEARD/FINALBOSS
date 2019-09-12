(function () {
    'use strict';

    angular.module('bob').factory('authService', function ($http, $q, localStorageService, base, authApi, ngAuthSettings) {

        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var _validate = function (validationModel) {
            _logOut();
            return $http.post(authApi + "validate", validationModel).then(function (response) {
                return response;
            });
        };

        var _register = function (registration) {

            _logOut();

            return $http.post(authApi + "register", registration).then(function (response) {
                return response;
            });

        };

        var _changepassword = function (changepassword) {
            _logOut();

            return $http.post(authApi + "changepassword", changepassword).then(function (response) {
                return response;
            });
        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            data = data + "&client_id=" + ngAuthSettings.clientId;

            var deferred = $q.defer();

            $http.post(base + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

                localStorageService.set('authorizationData', { token: response.data.access_token, userName: loginData.userName });

                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;

                deferred.resolve(response);

            }, function (err) {
                _logOut();
                deferred.reject(err);
            });
            return deferred.promise;
        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.userName = "";

        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
            }

        };

        //var _refreshToken = function () {
        //    var deferred = $q.defer();

        //    var authData = localStorageService.get('authorizationData');

        //    if (authData) {


        //        var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

        //        localStorageService.remove('authorizationData');

        //        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

        //            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token});

        //            deferred.resolve(response);

        //        }).error(function (err, status) {
        //            _logOut();
        //            deferred.reject(err);
        //        });

        //    }

        //    return deferred.promise;
        //};

        authServiceFactory.register = _register;
        authServiceFactory.changepassword = _changepassword;
        authServiceFactory.validate = _validate;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        //authServiceFactory.refreshToken = _refreshToken;

        return authServiceFactory;
    });
})();