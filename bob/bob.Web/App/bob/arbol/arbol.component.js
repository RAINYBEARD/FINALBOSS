(function () {
    'use strict';

    angular.module('bob').component('arbol', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.tabla;            
            vm.total;
            vm.aprobadas;

            caeceService.getArbol(vm.matricula).then(function (response) {
                vm.tabla = response;
                vm.materias = vm.tabla.materias;
                vm.total = vm.tabla.total;
                vm.aprobadas = vm.tabla.aprobadas;

            });

        },
        templateUrl: '/App/bob/arbol/arbol.component.html'
    });

})();