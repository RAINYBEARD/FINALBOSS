(function () {
    'use strict';

    angular.module('bob').component('changepassword', {
        controllerAs: 'vm',
        controller: ChangePasswordController,
        templateUrl: '/App/bob/changepassword/changepassword.component.html'
    });

    ChangePasswordController.$inject = ['$location', '$timeout', 'authService'];

    function ChangePasswordController($location, $timeout, authService) {
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
                    vm.message = "Error al intentar validar el usuario: " + err.data.message;
                });
        }
        $('#msg').fadeOut(9500);

        function changepassword() {

            authService.changepassword(vm.changePasswordModel).then(function (response) {
                vm.savedSuccessfully = true;
                vm.message = "El usuario ha cambiado su contraseña satisfactoriamente. Aguarde mientras es redirigido.";
                var timer = $timeout(function () {
                    $timeout.cancel(timer);
                    $location.path('/ingresar');
                }, 5000);
            },
                function (err) {
                    vm.message = "Error al intentar cambiar la contraseña: " + err.data.message;
                });
        }
        $('#msg2').fadeOut(10000);
    }

})();