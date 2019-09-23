(function () {
    'use-strict';
    angular.module('bob').component('bobApp', {
        controllerAs: 'vm',
        controller: function (authService, $scope, caeceService) {
            var vm = this;
            vm.loaded = false;
                caeceService.setSesionUsuario(authService.authentication.username).then(function () {
                    vm.loaded = true;
                });
        },
        templateUrl: 'App/bob/bob-app.component.html'
    });

})();