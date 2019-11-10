(function () {
    'use strict';

    angular.module('bob').factory('authService', function ($http, $q, localStorageService, base, authApi, ngAuthSettings) {

        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            username: ""
        };

        var _validate = function (validationModel) {
            _logout();
            return $http.post(authApi + "validate", validationModel).then(function (response) {
                return response;
            });
        };

        var _register = function (registration) {

            _logout();

            return $http.post(authApi + "register", registration).then(function (response) {
                return response;
            });

        };

        var _changepassword = function (changepassword) {
            _logout();

            return $http.post(authApi + "changepassword", changepassword).then(function (response) {
                return response;
            });
        };

        var _login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.username + "&password=" + loginData.password;

            //data = data + "&client_id=" + ngAuthSettings.clientId;

            var deferred = $q.defer();

            $http.post(base + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

                localStorageService.set('authorizationData', { token: response.data.access_token, username: loginData.username });

                _authentication.isAuth = true;
                _authentication.username = loginData.username;

                deferred.resolve(response);

            }, function (err) {
                _logout();
                deferred.reject(err);
            });
            return deferred.promise;
        };

        var _logout = function () {

            localStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.username = "";
        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.username = authData.username;
            }
        };

        var _isAdmin = function () {
            var deferred = $q.defer();
            $http.get(authApi + "is-admin").then(function (response) {
                deferred.resolve(response.data);

            }, function () {
                deferred.resolve(false);
            });
            return deferred.promise;
        };

        var _getAlumnos = function () {
            var deferred = $q.defer();
            $http.get(authApi + "get-alumnos").then(function (response) {
                deferred.resolve(response.data);
            }, function () {
                deferred.resolve(false);
            });
            return deferred.promise;
        };

        var _borrarAlumno = function (matricula) {
            var deferred = $q.defer();
            $http.delete(authApi + "borrar-alumno/" + matricula).then(function (response) {
                deferred.resolve(response.data);
            }, function () {
                deferred.resolve(false);
            });
            return deferred.promise;
        };

        //var _refreshToken = function () {
        //    var deferred = $q.defer();

        //    var authData = localStorageService.get('authorizationData');

        //    if (authData) {


        //        var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

        //        localStorageService.remove('authorizationData');

        //        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

        //            localStorageService.set('authorizationData', { token: response.access_token, username: response.username, refreshToken: response.refresh_token});

        //            deferred.resolve(response);

        //        }).error(function (err, status) {
        //            _logOut();
        //            deferred.reject(err);
        //        });

        //    }

        //    return deferred.promise;
        //};

        authServiceFactory.register = _register;
        authServiceFactory.isAdmin = _isAdmin;
        authServiceFactory.changepassword = _changepassword;
        authServiceFactory.validate = _validate;
        authServiceFactory.login = _login;
        authServiceFactory.logout = _logout;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.getAlumnos = _getAlumnos;
        authServiceFactory.borrarAlumno = _borrarAlumno;
        //authServiceFactory.refreshToken = _refreshToken;

        return authServiceFactory;
    });
})();