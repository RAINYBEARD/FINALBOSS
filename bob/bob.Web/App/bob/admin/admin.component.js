(function () {
    'use strict';

    angular.module('bob').component('admin', {
        controllerAs: 'vm',
        controller: function () {
            var vm = this;
            vm.loaded = false;

        },
        templateUrl: '/App/bob/admin/admin.component.html'
    });

})();