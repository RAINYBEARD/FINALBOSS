(function () {
    'use strict';

    angular.module('bob').component('cursosAuto', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.cursos;
            vm.submit = submit;
            vm.filterBy = "W";
            vm.checkboxModel = {
                lun: '1',
                mar: '1',
                mie: '1',
                jue: '1',
                vie: '1',
                sab: '1'
            };
            vm.filtro = vm.checkboxModel.lun + vm.checkboxModel.mar + vm.checkboxModel.mie + vm.checkboxModel.jue + vm.checkboxModel.vie + vm.checkboxModel.sab + '0';
            vm.agregarDias = agregarDias;

            function submit() {
                caeceService.getCursos(vm.matricula).then(function (response) {
                    vm.cursos = response;
                });
            }

            function agregarDias(dia) {
                var i = 0;
                var letras = vm.filtro.split('');

                while (i < 7 && ((dia.substr(i, 1) == "1" && vm.filtro.substr(i, 1) == "0") ||
                                 (dia.substr(i, 1) == "0" && vm.filtro.substr(i, 1) == "0") ||
                                 (dia.substr(i, 1) == "0" && vm.filtro.substr(i, 1) == "1"))) {
                    if (dia.substr(i, 1) == "1" && vm.filtro.substr(i, 1) == "0") {
                        letras[i]='1';
                    }
                    i++;
                }
                vm.filtro = letras.join('');
            }

            vm.people = [{
                age: 46,
                name: 'Wendy'
            }, {
                age: 50,
                name: 'Joe'
            }, {
                age: 11,
                name: 'Frank'
            }, {
                age: 6,
                name: 'Jenny'
            }];
        },
        //https://plnkr.co/edit/YPn9lZOX1vlalgjinwXK?p=preview
        templateUrl: '/App/bob/cursos/cursos-auto.component.html'
    })

    angular.module('bob').filter('icfilter', function () {
        return function (people, filterBy) {
            var out = [],
                lowerFilter = filterBy.toLowerCase();

            console.log(people)

            angular.forEach(people, function (person) {

                if (person.name.toLowerCase().includes(lowerFilter)) {
                    out.push(person);
                }
            });

            return out;

        }
    })

    angular.module('bob').filter('cursosfilter', function () {
        return function (cursos, filtro) {
            var out = [];
            angular.forEach(cursos, function (curso) {
                var i = 0;
                while (i < 7 && ((filtro.substr(i, 1) == "1" && curso.Dia.substr(i, 1) == "1") ||
                    (filtro.substr(i, 1) == "1" && curso.Dia.substr(i, 1) == "0") ||
                    (filtro.substr(i, 1) == "0" && curso.Dia.substr(i, 1) == "0"))) {
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