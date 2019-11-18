(function () {
    'use strict';

    angular.module('bob').component('finales', {
        controllerAs: 'vm',
        controller: finales,
        templateUrl: '/App/bob/finales/finales.component.html'
    });

    finales.$inject = ['caeceService', 'authService'];

    function finales(caeceService, authService) {
        var vm = this;
        vm.finales;
        vm.vencimiento = vencimiento;
        vm.numeroCorrelativas = numeroCorrelativas;
        vm.equivalenciasParciales = equivalenciasParciales;
        vm.ultimoIntento = ultimoIntento;
        vm.orden;
        vm.filtro;

        caeceService.getFinales(authService.authentication.username).then(function (response) {
            vm.finales = response;
        });


        function vencimiento() {
            vm.orden = 'fechaVencimiento';
            vm.filtro = {};
        }
        function numeroCorrelativas() {
            vm.orden = '-nCorrelativas';
            vm.filtro = {};
        }
        
        function equivalenciasParciales() {
            vm.filtro = { 'descrip': 'EQP' };
        }

        function ultimoIntento() {
            vm.filtro = { 'reprobado': 'Si' };
        }
    }

})();
