﻿(function () {
    'use strict';

    angular.module('bob').component('cursosManual', {
        controllerAs: 'vm',
        controller: CursosManualController,
        templateUrl: '/App/bob/cursos/cursos-manual.component.html'
    });

    CursosManualController.$inject = ['caeceService', 'authService'];

    function CursosManualController (caeceService, authService) {
        var vm = this;
        vm.cursos;
        vm.seleccionmateria = seleccionmateria;
        vm.materiasSeleccionadas = [];
        vm.trabarMateria = trabarMateria;
        vm.deseleccionaritems = deseleccionaritems;
        vm.checkboxModel = {
            lun: '1',
            mar: '1',
            mie: '1',
            jue: '1',
            vie: '1',
            sab: '1',
            dom: '0'
        };
        vm.mismoDia = false;

        vm.filtro = vm.checkboxModel.lun + vm.checkboxModel.mar + vm.checkboxModel.mie + vm.checkboxModel.jue + vm.checkboxModel.vie + vm.checkboxModel.sab + vm.checkboxModel.dom;

        caeceService.getCursos(authService.authentication.username).then(function (response) {
            vm.cursos = response;
        });

        function deseleccionaritems(diaid) {
            vm.materiasSeleccionadas.forEach(function (item, key) {
                if (vm.checkboxModel.lun === '0' && item.dia[0] === '1') {
                    vm.materiasSeleccionadas.splice(key, 1);
                }
                if (vm.checkboxModel.mar === '0' && item.dia[1] === '1') {
                    vm.materiasSeleccionadas.splice(key, 1);
                }
                if (vm.checkboxModel.mie === '0' && item.dia[2] === '1') {
                    vm.materiasSeleccionadas.splice(key, 1);
                }
                if (vm.checkboxModel.jue === '0' && item.dia[3] === '1') {
                    vm.materiasSeleccionadas.splice(key, 1);
                }
                if (vm.checkboxModel.vie === '0' && item.dia[4] === '1') {
                    vm.materiasSeleccionadas.splice(key, 1);
                }
                if (vm.checkboxModel.sab === '0' && item.dia[5] === '1') {
                    vm.materiasSeleccionadas.splice(key, 1);
                }
                var cantDiasMateria = item.dia.split('1').length - 1;
                // Resuelvo el bug cuando una materia tiene mas de un dia que se cursa y destrabo la materia que se cursa tambien otro dia
                if (cantDiasMateria > 1 && ((diaid === '0' && item.dia[0] === '1')
                    || (diaid === '1' && item.dia[1] === '1')
                    || (diaid === '2' && item.dia[2] === '1')
                    || (diaid === '3' && item.dia[3] === '1')
                    || (diaid === '4' && item.dia[4] === '1')
                    || (diaid === '5' && item.dia[5] === '1'))
                ) {
                    vm.cursos.forEach(function (curso) {
                        for (var i = 0; i < item.dia.length; i++) {
                            if ((item.dia[i] === '1' && curso.dia[i] === '1') ||
                                (item.dia[i] === '1' && curso.dia[i] === '0')) {
                                vm.materiasSeleccionadas.splice(key, 1);
                            }
                        }
                    });
                }
            });


        }

        function trabarMateria(curso) {
            vm.mismoDia = false;

            vm.materiasSeleccionadas.forEach(function (item, key) {
                for (var i = 0; i < curso.dia.length; i++) {
                    if ((curso.materia_Id !== item.materia_Id) &&
                        ((curso.dia[i] === item.dia[i]) ||
                            ((curso.dia[i] === '1') && ((item.dia[i] === '2') || (item.dia[i] === '3'))) ||
                            (((curso.dia[i] === '2') || (curso.dia[i] === '3')) && (item.dia[i] === '1'))) &&
                        ((curso.dia[i] === '1') || (curso.dia[i] === '2') || (curso.dia[i] === '3'))) {
                        vm.mismoDia = true;
                        break;
                    }
                }
            });

            return vm.mismoDia;
        }

        function seleccionmateria(curso) {
            var idx = vm.materiasSeleccionadas.indexOf(curso);
            if (vm.materiasSeleccionadas.indexOf(curso) === -1) {

                vm.materiasSeleccionadas.push(curso);
            } else {
                vm.materiasSeleccionadas.splice(idx, 1);
            }
        }

    }

    angular.module('bob').filter('cursosfiltermanual', function () {
        return function (cursos, filtro) {
            var out = [];
            angular.forEach(cursos, function (curso) {
                var i = 0;
                while (i < 7 && (((filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '1') ||
                    (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '0') ||
                    (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '2') ||
                    (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '3') ||
                    (filtro.substr(i, 1) === '0' && curso.dia.substr(i, 1) === '0') ||
                    (filtro.substr(i, 1) === '0' && curso.dia.substr(i, 1) === '4') ||
                    (filtro.substr(i, 1) === '1' && curso.dia.substr(i, 1) === '4')))) {

                    i++;

                }

                if (i === 7) {
                    out.push(curso);
                }

            });
            return out;
        };
    });

})();