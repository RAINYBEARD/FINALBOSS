(function () {
    'use strict';

    angular.module('bob').component('admin', {
        controllerAs: 'vm',
        controller: function () {
            var vm = this;
            vm.loaded = false;
            //caeceService.setSesionUsuario(authService.authentication.username).then(function () {
            //    vm.loaded = true;
            //});

        },
        templateUrl: '/App/bob/admin/admin.component.html'
    });

})();