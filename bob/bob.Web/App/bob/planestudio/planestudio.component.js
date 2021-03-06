﻿(function () {
    'use strict';

    angular.module('bob').component('planestudio', {
        controllerAs: 'vm',
        controller: planestudio,
        templateUrl: '/App/bob/planestudio/planestudio.component.html'
    });

    planestudio.$inject = ['caeceService', 'authService'];

    function planestudio(caeceService, authService) {
        var vm = this;
        vm.materias;
        vm.tabla;
        vm.total;
        vm.aprobadas;

        vm.todas = todas;
        vm.materiasaprobadas = materiasaprobadas;
        vm.materiascursadas = materiascursadas;
        vm.materiasnocursadas = materiasnocursadas;
        vm.materiaspendientes = materiaspendientes;
        vm.filtro;

        caeceService.getPlanEstudio(authService.authentication.username).then(function (response) {
            vm.tabla = response;
            vm.materias = vm.tabla.materias;
            vm.total = vm.tabla.total;
            vm.aprobadas = vm.tabla.aprobadas;

        });

        function todas() {
            vm.filtro = '';
        }

        function materiasaprobadas() {
            vm.filtro = 'Aprobada';
        }
        function materiascursadas() {
            vm.filtro = 'Cursada';
        }

        function materiasnocursadas() {
            vm.filtro = 'Sin Cursar';
        }

        function materiaspendientes() {
            vm.filtro = 'Pendiente';
        }

    }

    angular.module('bob').filter('Aprobada', function () {
        return function (items) {
            var materias = items.materias;
            var filteredItems;
            for (var i = 0; i < materias.length; i++) {
                var item = materias[i];
                if (item.estado === 'Aprobada') {
                    filteredItems.push(item);
                }
            }
            return filteredItems;
        }
    });

    angular.module('bob').filter('Cursada', function () {
        return function (items) {
            var materias = items.materias;
            var filteredItems;
            for (var i = 0; i < materias.length; i++) {
                var item = materias[i];
                if (item.estado === 'Cursada') {
                    filteredItems.push(item);
                }
            }
            return filteredItems;
        }
    });

    angular.module('bob').filter('Sin Cursar', function () {
        return function (items) {
            var materias = items.materias;
            var filteredItems;
            for (var i = 0; i < materias.length; i++) {
                var item = materias[i];
                if (item.estado === 'Sin Cursar') {
                    filteredItems.push(item);
                }
            }
            return filteredItems;
        }
    });

    angular.module('bob').filter('Pendiente', function () {
        return function (items) {
            var materias = items.materias;
            var filteredItems;
            for (var i = 0; i < materias.length; i++) {
                var item = materias[i];
                if (item.estado === 'Pendiente') {
                    filteredItems.push(item);
                }
            }
            return filteredItems;
        }
    });

})();