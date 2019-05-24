(function () {
    'use strict';

    angular.module('bob').component('arbol', {
        controllerAs: 'vm',
        controller: function () {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.submit = submit;

            function submit() {
                caeceService.getArbol(vm.matricula).then(function (response) {
                    vm.materias = response;
                });
            }
            
        },
        templateUrl: '/App/bob/arbol/arbol.component.html'
    });

})();