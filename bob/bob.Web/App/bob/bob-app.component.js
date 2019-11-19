(function () {
    'use-strict';
    angular.module('bob').component('bobApp', {
        controllerAs: 'vm',
        controller: bobApp,
        templateUrl: 'App/bob/bob-app.component.html'
    });

    bobApp.$inject = ['authService', 'caeceService'];

    function bobApp(authService, caeceService) {
        var vm = this;
        vm.loaded = false;
        caeceService.setSesionUsuario(authService.authentication.username).then(function () {
            vm.loaded = true;
        });
    }
})();