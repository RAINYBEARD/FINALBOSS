(function () {
    'use strict';

    angular.module('bob').component('register', {
        controllerAs: 'vm',
        controller: function ($location, $timeout, authService) {
            var vm = this;

            vm.validationModel = {
                userName: "",
                password: ""
            };
            vm.validation = false;

            vm.savedSuccessfully = false;
            vm.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            vm.message = "";

            vm.validate = validate;
            vm.register = register;

            function validate() {
                authService.validate(vm.validationModel).then(function (response) {
                    vm.validation = response;
                    vm.registration.userName = vm.validationModel.userName;
                },
                    function (err) {
                        vm.message = "Failed to register user due to:" + err.data.message;
                    });
            }

            function register() {


                authService.register(vm.registration).then(function (response) {
                    vm.savedSuccessfully = true;
                    vm.message = "User has been registered successfully, you will be redirected to login page in 2 seconds";
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $location.path('/login');
                    }, 2000);
                },
                    function (err) {
                        vm.message = "Failed to register user due to:" + err.data.message;
                    });
            }
        },

        templateUrl: '/App/bob/register/register.component.html'
    });

})();