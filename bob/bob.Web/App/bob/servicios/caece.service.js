(function () {
    'use strict';

    angular.module('bob').factory('caeceService', function (caeceApi, $http) {

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

        self.getArbol = function (matricula) {
            return $http.get(apiBase + 'get-arbol' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPendientes = function (matricula) {
            return $http.get(caeceApi + 'get-pendientes' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPorVencerse = function (matricula) {
            return $http.get(caeceApi + 'get-porvencerse' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        return this;
    });
})();