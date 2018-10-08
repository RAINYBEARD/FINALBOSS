(function () {
    'use strict';

    angular.module('app').component('arbol', {
        controllerAs: 'vm',
        controller: function () {
            var vm = this;
            vm.name = { first: '', last: '' };
            vm.submit = function () {
                vm.fullName = vm.name.first + ' ' + vm.name.last;
            };
        },
        templateUrl:
    });

})();