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
            vm.checkboxModel = {
                lun: '1',
                mar: '1',
                mie: '1',
                jue: '1',
                vie: '1',
                sab: '1',
                dom: '0'
            };

            vm.filtro = vm.checkboxModel.lun + vm.checkboxModel.mar + vm.checkboxModel.mie + vm.checkboxModel.jue + vm.checkboxModel.vie + vm.checkboxModel.sab + vm.checkboxModel.dom;

            function submit() {
                caeceService.getCursos(vm.matricula).then(function (response) {
                    vm.cursos = response;
                });
            }

            function trabarMateria(curso) {
                var mismoDia = false;
                //if (vm.materiasSeleccionadas.length > 0) {
                    vm.materiasSeleccionadas.forEach(function (item, key) {
                        for (var i = 0; i < curso.Dia.length; i++) {
                            if (curso.Materia_Id !== item.Materia_Id && curso.Dia[i] === '1' && curso.Dia[i] === item.Dia[i]) {
                                mismoDia = true;
                                break;
                            }
                        }
                    });
                //} else {
                //    mismoDia = false;
                //}
                return mismoDia;
            }


            function seleccionmateria(curso) {
                var idx = vm.materiasSeleccionadas.indexOf(curso);
                if (vm.materiasSeleccionadas.indexOf(curso) === -1) {
                    console.log('Pushing: ', curso.Materia_Id);
                    console.log('Contenido de variable idx : ', idx);

                    //console.log('dia de la materia que habilito : ', );
                    vm.materiasSeleccionadas.push(curso);
                    console.log(vm.materiasSeleccionadas);
                } else {
                    vm.materiasSeleccionadas.splice(idx, 1);
                    console.log('Contenido de variable idx : ', idx);
                    console.log(vm.materiasSeleccionadas);
                }
                //angular.forEach(vm.materiasSeleccionadas, function (materiaSeleccionada) {
                //    console.log(materiaSeleccionada);
                //});
            }

        },

        templateUrl: '/App/bob/cursos/cursos-manual.component.html'
    });

    angular.module('bob').filter('cursosfiltermanual', function () {
        return function (cursos, filtro) {
            var out = [];

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