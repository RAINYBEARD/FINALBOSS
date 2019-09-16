(function () {
    'use-strict';
    angular.module('bob').component('bobApp', {
        controllerAs: 'vm',
        controller: function (authService) {
            var vm = this;
            vm.authenticated = authService.authentication.IsAuth;
        },
        templateUrl: 'App/bob/bob-app.component.html'
    });

})();