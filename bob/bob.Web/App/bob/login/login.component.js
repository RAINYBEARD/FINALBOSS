(function () {
    'use strict';

    angular.module('bob').component('login', {
        controllerAs: 'vm',
        controller: function (apiBase, $http, caeceService) {
            var vm = this;
            vm.matricula;
            vm.submit = submit;

            function submit() {
                caeceService.savePlanEstudio(vm.matricula);
                caeceService.setSesionUsuario(vm.matricula);
            }
        },

        templateUrl: '/App/bob/login/login.component.html'
    });

})();