(function () {
    'use strict';

    angular.module('bob').component('estadisticasAprobadas', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.submit = submit;
            vm.aprobadas;
            vm.noAprobadas;

            function submit() {
                caeceService.getEstadisticas(vm.matricula).then(function (response) {
                    vm.materias = response;
                    vm.aprobadas = (vm.materias.Aprobadas / vm.materias.Total)*100;
                    vm.noAprobadas = ((vm.materias.Total - vm.materias.Aprobadas) / vm.materias.Total)*100;
                });
            }

        },
        templateUrl: '/App/bob/estadisticas/estadisticas-aprobadas.component.html'
    });

})();


