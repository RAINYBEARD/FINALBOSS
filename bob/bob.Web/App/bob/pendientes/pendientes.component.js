(function () {
    'use strict';

    angular.module('bob').component('pendientes', {
        controllerAs: 'vm',
        controller: function (caeceService, authService) {
            var vm = this;
            vm.pendientes;

            if (authService.authentication.isAuth) {
                caeceService.getPendientes(authService.authentication.username).then(function (response) {
                    vm.pendientes = response;
                });

                caeceService.getPorVencerse(authService.authentication.username).then(function (response) {
                    vm.porvencerse = response;
                });
            }

        },
        templateUrl: '/App/bob/pendientes/pendientes.component.html'
    });

})();