(function () {
    'use strict';

    angular.module('bob').component('admin', {
        templateUrl: '/App/bob/admin/admin.component.html',
        controllerAs: 'vm',
        controller: function (authService) {
            var vm = this;

            authService.getAlumnos().then(function (response) {
                vm.alumnos = response;
            });

            vm.borrarAlumno = function (matricula) {
                authService.borrarAlumno(matricula).then(function () {
                    authService.getAlumnos().then(function (response) {
                        vm.alumnos = response;
                    });
                });
            };
        }
    });

})();