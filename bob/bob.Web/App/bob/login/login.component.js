(function () {
    'use strict';

    angular.module('bob').component('login', {
        controllerAs: 'vm',
        controller: login,
        templateUrl: '/App/bob/login/login.component.html'
    });

    login.$inject = ['caeceService', 'authService', '$state'];

    function login(caeceService, authService, $state) {
        var vm = this;

        vm.loginData = {
            username: "",
            password: "",
            clientId: ""
        };

        vm.message = "";
        vm.login = login;

        function login() {

            authService.login(vm.loginData).then(function (response) {
                caeceService.setSesionUsuario(vm.loginData.username).then(function (response) {
                    $state.go('landing');
                });

            },
                function (err) {
                    vm.message = err.data.error_description;
                });

        }
    }

})();