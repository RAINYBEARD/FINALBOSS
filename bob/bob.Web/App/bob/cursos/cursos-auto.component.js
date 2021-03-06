﻿(function () {
    'use strict';

    angular.module('bob').component('cursosAuto', {
        controllerAs: 'vm',
        controller: CursosAutoController,
        templateUrl: '/App/bob/cursos/cursos-auto.component.html'
    });

    CursosAutoController.$inject = ['caeceService', 'authService'];

    function CursosAutoController(caeceService, authService) {
        var vm = this;
        vm.cursos;

        vm.checkboxModel = {
            lun: '1',
            mar: '1',
            mie: '1',
            jue: '1',
            vie: '1',
            sab: '1'
        };

        vm.filtroCantDias = {
            valor: 6
        };

        vm.filtro = vm.checkboxModel.lun + vm.checkboxModel.mar + vm.checkboxModel.mie + vm.checkboxModel.jue + vm.checkboxModel.vie + vm.checkboxModel.sab + '0';

        caeceService.getCursos(authService.authentication.username).then(function (response) {
            vm.cursos = response;
        });


    }

    angular.module('bob').filter('cursosfilterauto', function () {
        return function (cursos, filtro, filtroCantDias) {
            var out = [];
            var diasQueCursa = '0000000';

            angular.forEach(cursos, function (curso) {
                var i = 0;
                while (i < 7 && ((diasQueCursa.substr(i, 1) === "0" && curso.dia.substr(i, 1) === "1") ||
                    (diasQueCursa.substr(i, 1) === "0" && curso.dia.substr(i, 1) === "0") ||
                    (diasQueCursa.substr(i, 1) === "1" && curso.dia.substr(i, 1) === "0") ||
                    (diasQueCursa.substr(i, 1) === "0" && curso.dia.substr(i, 1) === "2") ||
                    (diasQueCursa.substr(i, 1) === "2" && curso.dia.substr(i, 1) === "0") ||
                    (diasQueCursa.substr(i, 1) === "2" && curso.dia.substr(i, 1) === "3") ||
                    (diasQueCursa.substr(i, 1) === "0" && curso.dia.substr(i, 1) === "3") ||
                    (diasQueCursa.substr(i, 1) === "3" && curso.dia.substr(i, 1) === "0") ||
                    (diasQueCursa.substr(i, 1) === "0" && curso.dia.substr(i, 1) === "4") ||
                    (diasQueCursa.substr(i, 1) === "1" && curso.dia.substr(i, 1) === "4") ||
                    (diasQueCursa.substr(i, 1) === "2" && curso.dia.substr(i, 1) === "4") ||
                    (diasQueCursa.substr(i, 1) === "3" && curso.dia.substr(i, 1) === "4") ||
                    (diasQueCursa.substr(i, 1) === "3" && curso.dia.substr(i, 1) === "2")) &&
                    ((filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '1') ||
                        (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '2') ||
                        (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '3') ||
                        (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '4') ||
                        (filtro.substr(i, 1) === '0' && curso.dia.substr(i, 1) === '4') ||
                        (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '0') ||
                        (filtro.substr(i, 1) === '0' && curso.dia.substr(i, 1) === '0'))) {

                    i++;

                }

                var cantDias = 0;
                var j = 0;
                while (j < 7) {
                    if (diasQueCursa.substr(j, 1) !== "0") {
                        cantDias++;
                    }
                    j++;
                }

                // Le sumo la cantidad de dias de la materia nueva
                //var cantDiasMateria = curso.dia.split('1').length - 1;
                //cantDias = cantDias + cantDiasMateria;
                //var cantDiasMateria = curso.dia.split('1').length - 1;
                cantDias = cantDias + (curso.dia.split('1').length - 1) + (curso.dia.split('2').length - 1) + (curso.dia.split('3').length - 1);

                if (i === 7 && (cantDias <= filtroCantDias)) {
                    var diasParaCursar = diasQueCursa.split('');
                    var diasMateria = curso.dia.split('');
                    j = 0;
                    while (j < 7) {
                        if ((diasParaCursar[j] === '0' && diasMateria[j] === '1') || (diasParaCursar[j] === '0' && diasMateria[j] === '4') || (diasParaCursar[j] === '3' && diasMateria[j] === '2') || (diasParaCursar[j] === '2' && diasMateria[j] === '3')) {
                            diasParaCursar[j] = '1';
                        }
                        else {
                            if (diasParaCursar[j] === '0' && diasMateria[j] === '2') {
                                diasParaCursar[j] = '2';
                            }
                            else {
                                if (diasParaCursar[j] === '0' && diasMateria[j] === '3') {
                                    diasParaCursar[j] = '3';
                                }
                            }
                        }

                        j++;
                    }
                    diasQueCursa = diasParaCursar.join('');
                    out.push(curso);
                }

            });
            return out;
        }
    });

})();