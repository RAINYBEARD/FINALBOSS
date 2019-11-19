(function () {
    'use strict';

    angular.module('bob').component('cursos', {
        controllerAs: 'vm',
        controller: CursosController,
        templateUrl: '/App/bob/cursos/cursos.component.html'
    });

    function CursosController() {

    }

})();