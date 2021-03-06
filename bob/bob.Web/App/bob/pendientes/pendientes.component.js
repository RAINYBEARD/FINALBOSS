﻿(function () {
    'use strict';

    angular.module('bob').component('pendientes', {
        controllerAs: 'vm',
        controller: pendientes,
        templateUrl: '/App/bob/pendientes/pendientes.component.html'
    });

    pendientes.$inject = ['caeceService', 'authService'];

    function pendientes(caeceService, authService) {
        var vm = this;
        vm.pendientes;

        if (authService.authentication.isAuth) {
            caeceService.getPendientes().then(function (response) {
                vm.pendientes = response;
            });

            caeceService.getPorVencerse().then(function (response) {
                vm.porvencerse = response;
            });
        }
    }

})();