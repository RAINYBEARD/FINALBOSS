﻿(function () {
    'use strict';

    angular.module('bob').component('changepassword', {
        controllerAs: 'vm',
        controller: function ($location, $timeout, authService) {
            var vm = this;

            vm.validationModel = {
                userName: "",
                password: ""
            };
            vm.validation = false;

            vm.savedSuccessfully = false;
            vm.changePasswordModel = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            vm.message = "";

            vm.validate = validate;
            vm.changepassword = changepassword;

            function validate() {
                authService.validate(vm.validationModel).then(function (response) {
                    vm.validation = response;
                    vm.changePasswordModel.userName = vm.validationModel.userName;
                });
            }

            function changepassword() {

                authService.changepassword(vm.changePasswordModel).then(function (response) {
                    vm.savedSuccessfully = true;
                    vm.message = "User has changed his password successfully, you will be redirected to login page in 2 seconds";
                    var timer = $timeout(function () {
                        $timeout.cancel(timer);
                        $location.path('/login');
                    }, 2000);
                },
                    function (err) {
                        vm.message = "Failed to change user password due to:" + err.data.message;
                    });
            }
        },

        templateUrl: '/App/bob/changepassword/changepassword.component.html'
    });

})();