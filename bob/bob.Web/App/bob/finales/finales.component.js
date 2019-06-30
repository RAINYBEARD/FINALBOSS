(function () {
    'use strict';

    angular.module('bob').component('finales', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.finales;
            vm.submit = submit;
            vm.vencimiento = vencimiento;
            vm.numeroCorrelativas = numeroCorrelativas;
            vm.equivalenciasParciales = equivalenciasParciales;
            vm.ultimoIntento = ultimoIntento;
            vm.orden;
            vm.filtro;

            function submit() {

                caeceService.getFinales(vm.matricula).then(function (response) {
                    vm.finales = response;
                });
            }

            function vencimiento() {
                vm.orden = 'fechaVencimiento';
                vm.filtro = {};
            }
            function numeroCorrelativas() {
                vm.orden = '-nCorrelativas';
                vm.filtro = {};
            }

            function equivalenciasParciales() {
                vm.filtro = 'equivalencias';
            }

            function ultimoIntento() {
                vm.filtro = 'ultimo';
            }

            angular.module('bob').filter('equivalencias', function () {
                return function (items) {
                   var filteredItems;
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        if (item.descrip === 'EQP') {
                            filteredItems.push(item);
                        }
                    }
                    return filteredItems;
                }
            });

            //function equivalenciasParciales() {
            //    vm.filtro = { 'descrip': 'EQP' };
            //}

            angular.module('bob').filter('ultimo', function () {
                return function (items) {
                    var filteredItems;
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        if (item.reprobado === 'Si') {
                            filteredItems.push(item);
                        }
                    }
                    return filteredItems;
                }
            });

            //function ultimoIntento() {
            //    vm.filtro = { 'reprobado': 'Si' };
            //}
        },
        templateUrl: '/App/bob/finales/finales.component.html'
    });

})();
