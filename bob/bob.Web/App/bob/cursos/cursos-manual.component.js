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
            }

            vm.filtro = vm.checkboxModel.lun + vm.checkboxModel.mar + vm.checkboxModel.mie + vm.checkboxModel.jue + vm.checkboxModel.vie + vm.checkboxModel.sab + '0';

            function submit() {
                caeceService.getCursos(vm.matricula).then(function (response) {
                    vm.cursos = response;
                });
            }

            function seleccionmateria(materiaid) {
                    console.log(materiaid);
            }

        },

        templateUrl: '/App/bob/cursos/cursos-manual.component.html'
    })

    angular.module('bob').filter('cursosfiltermanual', function () {
        return function (cursos, filtro) {
            var out = [];

            angular.forEach(cursos, function (curso) {
                var i = 0;
                while (i < 7 && (((filtro.substr(i, 1) == '1' && curso.Dia.substr(i, 1) == '1') ||
                        (filtro.substr(i, 1) == '1' && curso.Dia.substr(i, 1) == '0') ||
                        (filtro.substr(i, 1) == '0' && curso.Dia.substr(i, 1) == '0')))) {

                    i++;

                }

                if (i == 7) {
                    out.push(curso);
                }

            });
            return out;
        }
    });

})();