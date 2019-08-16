(function () {
    'use strict';

    angular.module('bob').factory('caeceService', function (apiBase, $http) {

        var self = this;

        self.savePlanEstudio = function (matricula) {
            return $http.post(apiBase + 'save-plan-estudio' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.setSesionUsuario = function (matricula) {
            return $http.post(apiBase + 'set-sesion-usuario' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getFinales = function (matricula) {
            return $http.get(apiBase + 'get-finales' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getCursos = function (matricula) {
            return $http.get(apiBase + 'get-cursos' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getEstadisticas = function (matricula) {
            return $http.get(apiBase + 'get-estadisticas' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPendientes = function (matricula) {
            return $http.get(apiBase + 'get-pendientes' + '/' + matricula)
                .then(function (result) {
                    return result.data;
                });
        };

        self.getPorVencerse = function (matricula) {
            return $http.get(apiBase + 'get-porvencerse' + '/' + matricula)
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

        return this;
    });
})();