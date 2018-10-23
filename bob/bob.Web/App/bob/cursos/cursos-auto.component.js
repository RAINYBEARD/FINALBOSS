(function () {
    'use strict';

    angular.module('bob').component('cursosAuto', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.cursos;
            vm.submit = submit;
            vm.filtrar = filtrar;

            function submit() {
                caeceService.getCursos(vm.matricula).then(function (response) {
                    vm.cursos = response;
                });
            }

            function filtrar() {
                return vm.cursos === vm.cursos.item === 2;
            }


        },

        templateUrl: '/App/bob/cursos/cursos-auto.component.html'
    });

})();