(function () {
    'use strict';

    angular.module('bob').component('cursos', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.cursos;
            vm.submit = submit;

            function submit() {
                caeceService.getCursos(vm.matricula).then(function (response) {
                    vm.cursos = response;
                });   
            }

        },
        templateUrl: '/App/bob/cursos/cursos.component.html'
    });

})();