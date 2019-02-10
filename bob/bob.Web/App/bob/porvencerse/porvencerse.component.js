(function () {
    'use strict';

    angular.module('bob').component('porvencerse', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.porvencerse;
            vm.submit = submit;

            function submit() {

                caeceService.getPorVencerse(vm.matricula).then(function (response) {
                    vm.porvencerse = response;
                });
            }

        },
        templateUrl: '/App/bob/porvencerse/porvencerse.component.html'
    });

})();
