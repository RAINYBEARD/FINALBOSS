(function () {
    'use strict';

    angular.module('bob').component('estadisticasAprobadas', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.aprobadas;
            vm.noaprobadas;

            priority: 1000;
            caeceService.getEstadisticas(vm.matricula).then(function (response) {
                vm.materias = response;
                vm.aprobadas = (vm.materias.Aprobadas / vm.materias.Total) * 100;
                vm.noaprobadas = ((vm.materias.Total - vm.materias.Aprobadas) / vm.materias.Total) * 100;
            });

        },
        templateUrl: '/App/bob/estadisticas/estadisticas-aprobadas.component.html'
    });

    angular.module('bob').directive('d3Aprobadas', function () {
        var directive = {};
        directive.restrict = 'E';
        directive.scope = {
            aprobadas: '=?',
            noaprobadas: '=?'
        };

        directive.link = function (scope, elements, attr) {
            var svg = d3.select(elements[0]).append("svg")
                .style("background", "white")
                .attr("width", "950")
                .attr("height", "350")
                .attr("class", "row");
            var colors = d3.scaleOrdinal(d3.schemeDark2);
            var details = [{
                grade: "Aprobado", number: scope.aprobadas
            }, { grade: "No Aprobado", number: scope.noaprobadas }];

            var data = d3.pie().sort(null).value(function (d) { return d.number; })(details);
            console.log(data);
            var segments = d3.arc()
                .innerRadius(0)
                .outerRadius(150)
                .padAngle(.05)
                .padRadius(40);
            var sections = svg.append("g").attr("transform", "translate(425,180)")
                .selectAll("path").data(data);
            sections.enter().append("path").attr("d", segments).attr("fill", function
                (d) { return colors(d.data.number); });
            var content = d3.select("g").selectAll("text").data(data);
            content.enter().append("text").classed("inside", true).each(function (d) {
                var center = segments.centroid(d);
                d3.select(this).attr("x", center[0]).attr("y", center[1])
                    .text(d.data.number);
            });
            var legends = svg.append("g").attr("transform", "translate(800,250)")
                .selectAll(".legends").data(data);
            var legends = legends.enter().append("g").classed("legends", true).attr("transform", function (d, i) {
                return "translate(0," + (i + 1) * 30 + " )";
            });
            legends.append("rect").attr("width", 10).attr("height", 10).attr("fill", function (d) {
                return colors(d.data.number);
            });
            legends.append("text").classed("label", true).text(function (d) { return d.data.grade; })
                .attr("fill", function (d) { return colors(d.data.number); })
                .attr("x", 10)
                .attr("y", 10);

            
        }

        return directive;
    });
})();