(function () {
    'use strict';

    angular.module('bob').component('cursosManual', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.cursos;
            vm.submit = submit;
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

            function submit() {
                caeceService.getCursos(vm.matricula).then(function (response) {
                    vm.cursos = response;
                });
            }

            function deseleccionaritems() {
                vm.materiasSeleccionadas.forEach(function (item, key) {
                    if (vm.checkboxModel.lun === '0' && item.Dia[0] === '1') {
                        vm.materiasSeleccionadas.splice(key, 1);
                    }
                    if (vm.checkboxModel.mar === '0' && item.Dia[1] === '1') {
                        vm.materiasSeleccionadas.splice(key, 1);
                    }
                    if (vm.checkboxModel.mie === '0' && item.Dia[2] === '1') {
                        vm.materiasSeleccionadas.splice(key, 1);
                    }
                    if (vm.checkboxModel.jue === '0' && item.Dia[3] === '1') {
                        vm.materiasSeleccionadas.splice(key, 1);
                    }
                    if (vm.checkboxModel.vie === '0' && item.Dia[4] === '1') {
                        vm.materiasSeleccionadas.splice(key, 1);
                    }
                    if (vm.checkboxModel.sab === '0' && item.Dia[5] === '1') {
                        vm.materiasSeleccionadas.splice(key, 1);
                    }
                    //var cantDiasMateria = item.Dia.split('1').length - 1;
                    //// Resuelvo el bug cuando una materia tiene mas de un dia que se cursa y destrabo la materia que se cursa tambien otro dia
                    //if (cantDiasMateria > 1) {
                    //    vm.cursos.forEach(function (curso) {
                    //        for (var i = 0; i < item.Dia.length; i++) {
                    //            if ((item.Dia[i] === '1' && curso.Dia[i] == '1') ||
                    //                (item.Dia[i] === '1' && curso.Dia[i] == '0')) {
                    //                vm.materiasSeleccionadas.splice(key, 1);
                    //            }
                    //        }
                    //    });
                    //}
                });
                
                
            }

            function trabarMateria(curso) {
                vm.mismoDia = false;

                    vm.materiasSeleccionadas.forEach(function (item, key) {
                        for (var i = 0; i < curso.Dia.length; i++) {
                            if (curso.Materia_Id !== item.Materia_Id && curso.Dia[i] === '1' && curso.Dia[i] === item.Dia[i]) {
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
                    console.log('Pushing: ', curso.Materia_Id);
                    console.log('Contenido de variable idx : ', idx);

                    vm.materiasSeleccionadas.push(curso);
                    console.log(vm.materiasSeleccionadas);
                } else {
                    vm.materiasSeleccionadas.splice(idx, 1);
                    console.log('Contenido de variable idx : ', idx);
                    console.log(vm.materiasSeleccionadas);
                }
            }

        },

        templateUrl: '/App/bob/cursos/cursos-manual.component.html'
    });

    angular.module('bob').filter('cursosfiltermanual', function () {
        return function (cursos, filtro) {
            var out = [];
            console.log('Materias Seleccionadas: ',cursos);
            angular.forEach(cursos, function (curso) {
                var i = 0;
                while (i < 7 && (((filtro.substr(i, 1) === '1' && curso.Dia.substr(i, 1) === '1') ||
                    (filtro.substr(i, 1) === '1' && curso.Dia.substr(i, 1) === '0') ||
                    (filtro.substr(i, 1) === '0' && curso.Dia.substr(i, 1) === '0')))) {

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