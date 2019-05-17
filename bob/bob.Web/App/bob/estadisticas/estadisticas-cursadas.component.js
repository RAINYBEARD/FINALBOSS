(function () {
    'use strict';

    angular.module('bob').component('estadisticasCursadas', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.submit = submit;
            vm.cursadas;
            vm.noCursadas;

            function submit() {
                caeceService.getEstadisticas(vm.matricula).then(function (response) {
                    vm.materias = response;
                    vm.cursadas = (vm.materias.Cursadas / vm.materias.Total)*100;
                    vm.noCursadas = ((vm.materias.Total - vm.materias.Cursadas) / vm.materias.Total)*100;
                });
            }

        },
        templateUrl: '/App/bob/estadisticas/estadisticas-cursadas.component.html'
    });

})();