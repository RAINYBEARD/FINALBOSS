(function () {
    'use strict';

    angular.module('bob').component('porvencerse', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.finales;
            vm.submit = submit;

            function submit() {

                caeceService.getFinales(vm.matricula).then(function (response) {
                    vm.finales = response;
                });
            }

        },
        templateUrl: '/App/bob/porvencerse/porvencerse.component.html'
    });

})();
