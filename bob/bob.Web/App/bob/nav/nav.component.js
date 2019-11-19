(function () {
    'use strict';

    angular.module('bob').component('nav', {
        controllerAs: 'vm',
        controller: nav,
        templateUrl: '/App/bob/nav/nav.component.html'
    });

    function nav() {
        var vm = this;
    }


})();