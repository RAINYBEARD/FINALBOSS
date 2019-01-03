(function () {
    'use strict';

    angular.module('bob').component('estadisticas', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.submit = submit;

            function submit() {
                caeceService.getEstadisticas(vm.matricula).then(function (response) {
                    vm.materias = response;
                });
            }

        },
        templateUrl: '/App/bob/estadisticas/estadisticas.component.html'
    });

})();