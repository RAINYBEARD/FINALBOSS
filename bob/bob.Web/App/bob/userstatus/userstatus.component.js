(function () {
    'use strict';

    angular.module('bob').component('userstatus', {
        controllerAs: 'vm',
        controller: function (caeceService, authService) {
            var vm = this;

            vm.user = authService.authentication.username;



        },

        templateUrl: '/App/bob/userstatus/userstatus.component.html'
    });

})();