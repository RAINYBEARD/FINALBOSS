(function () {
    'use strict';

    angular.module('bob').component('login', {
        controllerAs: 'vm',
        controller: function () {
            var vm = this;
            vm.matricula;
            vm.submit = submit;

            function submit() {

            }


        },

        templateUrl: '/App/bob/login/login.component.html'
    });

})();