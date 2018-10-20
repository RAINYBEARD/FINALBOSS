(function () {
    'use strict';

    angular.module('bob').component('finales', {
        controllerAs: 'vm',
        controller: function (apiBase, $http, caeceService) {            var vm = this;            vm.matricula;            vm.finales;            vm.submit = submit;            function submit() {                caeceService.savePlanEstudio(vm.matricula);                caeceService.setSesionUsuario(vm.matricula).then(function (response) {                    caeceService.getFinales(vm.matricula).then(function (response) {                        vm.finales = response;                    });                });            }        },

        templateUrl: '/App/bob/finales/finales.component.html'
    });

})();