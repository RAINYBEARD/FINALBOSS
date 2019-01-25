(function () {
    'use strict';

    angular.module('bob').component('pendientes', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.pendientes;
            vm.submit = submit;

            function submit() {

                caeceService.getPendientes(vm.matricula).then(function (response) {
                    vm.pendientes = response;
                });
            }

        },
        templateUrl: '/App/bob/pendientes/pendientes.component.html'
    });

})();