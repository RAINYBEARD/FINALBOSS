﻿(function () {
    'use strict';

    angular.module('bob').component('userstatus', {
        controllerAs: 'vm',
        controller: function ($state, authService) {
            var vm = this;
            vm.authenticated = authService.authentication.isAuth;
            vm.user = authService.authentication.username;

            vm.changepassword = function () {
                authService.logout();
                $state.go('cambiar');
            };

            vm.logout = function () {
                authService.logout();
                $state.go('ingresar');
            };


        },

        templateUrl: '/App/bob/userstatus/userstatus.component.html'
    });

})();