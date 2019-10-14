(function () {
    'use strict';

    angular.module('bob').component('changepassword', {
        controllerAs: 'vm',
        controller: function ($location, $timeout, authService) {
            var vm = this;

            vm.validationModel = {
                username: "",
                password: ""
            };
            vm.validation = false;

            vm.savedSuccessfully = false;
            vm.changePasswordModel = {
                username: "",
                password: "",
                confirmPassword: ""
            };

            vm.message = "";

            vm.validate = validate;
            vm.changepassword = changepassword;

            function validate() {
                authService.validate(vm.validationModel).then(function (response) {
                    vm.validation = response;
                    vm.changePasswordModel.username = vm.validationModel.username;
                },
                    function (err) {
                        vm.message = "La registracion del usuario ha fallado debido a: " + err.data.message;
                    });
            }

            function changepassword() {

                authService.changepassword(vm.changePasswordModel).then(function (response) {
                    vm.savedSuccessfully = true;
                    vm.message = "El usuario ha cambiado su contraseña satisfactoriamente y sera redirigido";
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $location.path('/ingresar');
                    }, 5000);
                },
                    function (err) {
                        vm.message = "El cambio de contraseña ha fallado debido a: " + err.data.message;
                    });
            }
        },

        templateUrl: '/App/bob/changepassword/changepassword.component.html'
    });

})();