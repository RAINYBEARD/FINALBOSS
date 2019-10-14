(function () {
    'use strict';

    angular.module('bob').component('register', {
        controllerAs: 'vm',
        controller: function ($state, authService, caeceService) {
            var vm = this;

            vm.validationModel = {
                username: "",
                password: ""
            };
            vm.validation = false;

            vm.savedSuccessfully = false;
            vm.registration = {
                username: "",
                password: "",
                confirmPassword: ""
            };

            vm.message = "";

            vm.validate = validate;
            vm.register = register;

            function validate() {
                authService.validate(vm.validationModel).then(function (response) {
                    vm.validation = response;
                    vm.registration.username = vm.validationModel.username;
                },
                    function (err) {
                        vm.message = "La registracion del usuario ha fallado debido a: " + err.data.message;
                    });
            }

            function register() {


                authService.register(vm.registration).then(function (response) {
                    authService.login({ username: vm.registration.username, password: vm.registration.password }).then(function () {
                        caeceService.savePlanEstudio(vm.registration.username).then(function (response) {
                            vm.savedSuccessfully = true;
                            $state.go('bob.arbol');
                        });
                    });
                },
                    function (err) {
                        vm.message = "La registracion del usuario ha fallado debido a: " + err.data.message;
                    });
            }
        },

        templateUrl: '/App/bob/register/register.component.html'
    });

})();