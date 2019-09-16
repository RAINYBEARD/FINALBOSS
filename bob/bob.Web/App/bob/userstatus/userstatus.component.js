(function () {
    'use strict';

    angular.module('bob').component('userstatus', {
        controllerAs: 'vm',
        controller: function (authService) {
            var vm = this;
            vm.authenticated = authService.authentication.isAuth;
            vm.user = authService.authentication.userName;
        },

        templateUrl: '/App/bob/userstatus/userstatus.component.html'
    });

})();