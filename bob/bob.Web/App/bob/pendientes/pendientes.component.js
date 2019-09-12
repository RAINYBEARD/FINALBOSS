(function () {
    'use strict';

    angular.module('bob').component('pendientes', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.pendientes;
            
            caeceService.getPendientes(vm.matricula).then(function (response) {
                vm.pendientes = response;
            });

            caeceService.getPorVencerse(vm.matricula).then(function (response) {
                vm.porvencerse = response;
            });
            
        },
        templateUrl: '/App/bob/pendientes/pendientes.component.html'
    });

})();