(function () {
    'use strict';

    angular.module('bob').component('finales', {
        controllerAs: 'vm',
        controller: function (caeceService) {            var vm = this;            vm.matricula;            vm.finales;            vm.submit = submit;            function submit() {                caeceService.getFinales(vm.matricula).then(function (response) {                    vm.finales = response;                });            }            var btnContainer = document.getElementById("myBtnContainer");
            var btns = btnContainer.getElementsByClassName("btn");
            for (var i = 0; i < btns.length; i++) {
                btns[i].addEventListener("click", function () {
                    var current = document.getElementsByClassName("active");
                    current[0].className = current[0].className.replace(" active", "");
                    this.className += " active";
                });
            }        },

        templateUrl: '/App/bob/finales/finales.component.html'
    });

})();