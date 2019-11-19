(function () {
    'use strict';

    angular.module('bob').factory('caeceService', caeceService);

    caeceService.$inject = ['caeceApi', '$http'];

    function caeceService(caeceApi, $http) {

        var self = this;

        self.savePlanEstudio = function (matricula) {
            return $http.post(caeceApi + 'save-plan-estudio' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.setSesionUsuario = function (matricula) {
            return $http.post(caeceApi + 'set-sesion-usuario' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getFinales = function (matricula) {
            return $http.get(caeceApi + 'get-finales' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getCursos = function (matricula) {
            return $http.get(caeceApi + 'get-cursos' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getEstadisticas = function (matricula) {
            return $http.get(caeceApi + 'get-estadisticas' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPlanEstudio = function (matricula) {
            return $http.get(caeceApi + 'get-plan-estudio' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPendientes = function () {
            return $http.get(caeceApi + 'get-pendientes')
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPorVencerse = function () {
            return $http.get(caeceApi + 'get-porvencerse')
                .then(function (result) {
                    return result.data;
                });
        };

        return this;
    }
})();