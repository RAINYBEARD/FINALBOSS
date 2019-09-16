(function () {
    'use strict';

    angular.module('bob').component('login', {
        controllerAs: 'vm',
        controller: function (caeceService, authService, $state) {
            var vm = this;

            vm.loginData = {
                userName: "",
                password: "",
                clientId: ""
            };

            vm.message = "";
            vm.login = login;

            function login() {

                authService.login(vm.loginData).then(function (response) {

                    caeceService.savePlanEstudio(vm.loginData.userName).then(function (response) {
                        caeceService.setSesionUsuario(vm.loginData.userName).then(function (response) {
                            $state.go('bob');
                        });
                    });
                },
                    function (err) {
                        vm.message = err.data.error_description;
                    });

            }
        },

        templateUrl: '/App/bob/login/login.component.html'
    });

})();