(function () {
    'use strict';

    angular.module('bob').component('userstatus', {
        controllerAs: 'vm',
        controller: userstatus,

        templateUrl: '/App/bob/userstatus/userstatus.component.html'
    });

    userstatus.$inject = ['$state', 'authService'];

    function userstatus($state, authService) {
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
    }

})();