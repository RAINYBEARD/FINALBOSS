(function () {
    'use strict';

    angular.module('bob').component('admin', {
        templateUrl: '/App/bob/admin/admin.component.html',
        controllerAs: 'vm',
        controller: AdminController
    });

    AdminController.$inject = ['authService'];

    function AdminController(authService) {
        var vm = this;
        vm.matricula = '';
        authService.getAlumnos().then(function (response) {
            vm.alumnos = response;
        });

        vm.modalBorrar = function (matricula) {
            vm.matricula = matricula;
        };

        vm.borrarAlumno = function (matricula) {
            authService.borrarAlumno(matricula).then(function () {
                authService.getAlumnos().then(function (response) {
                    vm.alumnos = response;
                });
            });
        };
    }

})();