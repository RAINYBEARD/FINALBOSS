(function () {
    'use strict';

    angular.module('bob').component('arbol', {
        controllerAs: 'vm',
        controller: function (caeceService) {
            var vm = this;
            vm.matricula;
            vm.materias;
            vm.nodos;
            vm.arcos;

           
            caeceService.getArbol('951282').then(function (response) {
                    vm.materias = JSON.parse(response);
                    vm.nodos    = vm.materias.nodos;
                    vm.arcos    = vm.materias.arcos;
                });
            
            
        },
        templateUrl: '/App/bob/arbol/arbol.component.html'
    });

    angular.module('bob').directive('d3Arbol', function () {
        var directive = {};
        directive.restrict = 'E';
        directive.scope = {
            nodos: '=?',
            arcos: '=?'
        };

        directive.link = function (scope, elements, attr) {

            var radius = 30;
            var width = 960;
            var height = 800;
            var svg = d3.select(elements[0]).append("svg")
                .attr("width", width)
                .attr("height", height);

            var nodes_data = scope.nodos;

            var simulation = d3.forceSimulation()
                .nodes(nodes_data);

            simulation
                .force("charge_force", d3.forceManyBody())
                .force("center_force", d3.forceCenter(width / 2, height / 2));

            var node = svg.append("g")
                .attr("class", "nodes")
                .selectAll("circle")
                .data(nodes_data)
                .enter()
                .append("circle")
                .attr("r", radius)
                .attr("fill", circleColour);


            var links_data = scope.arcos;

            var link_force = d3.forceLink(links_data)
                .id(function (d) { return d.materia_id; })

            simulation.force("links", link_force)

            function circleColour(d) {
                if (d.descrip == "APR") {
                    return "green";
                } else if (d.descrip == "CUR" || d.descrip == "EQP") {
                    return "blue";
                } else if (d.descrip == "PEN") {
                    return "orange";
                } else
                    return "red";

            }

            var link = svg.append("g")
                .attr("class", "links")
                .selectAll("line")
                .data(links_data)
                .enter().append("line")
                .attr("stroke-width", 2);

            function tickActions() {
                node
                    .attr("cx", function (d) { return d.x = Math.max(radius, Math.min(width - radius, d.x)); })
                    .attr("cy", function (d) { return d.y = Math.max(radius, Math.min(height - radius, d.y)); });

                link
                    .attr("x1", function (d) { return d.source.x; })
                    .attr("y1", function (d) { return d.source.y; })
                    .attr("x2", function (d) { return d.target.x; })
                    .attr("y2", function (d) { return d.target.y; });

                label
                    .attr("x", function (d) { return d.x; })
                    .attr("y", function (d) { return d.y; });
            }

            simulation.on("tick", tickActions);

            function splitting_force() {
                for (var i = 0, n = nodes_data.length; i < n; ++i) {
                    var curr_node = nodes_data[i];
                    if (curr_node.mat_anio == "1") {
                        if (curr_node.mat_cuatrim == "1") {
                            curr_node.x -= 3;
                        } else {
                            curr_node.x -= 2.5;
                        }
                    } else if (curr_node.mat_anio == "2") {
                        if (curr_node.mat_cuatrim == "1") {
                            curr_node.x -= 2;
                        } else {
                            curr_node.x -= 1.5;
                        }
                    } else if (curr_node.mat_anio == "3") {
                        if (curr_node.mat_cuatrim == "1") {
                            curr_node.x -= 1;
                        } else {
                            curr_node.x -= 0.5;
                        }
                    } else if (curr_node.mat_anio == "4") {
                        if (curr_node.mat_cuatrim == "1") {
                            curr_node.x += 0;
                        } else {
                            curr_node.x += 0.5;
                        }
                    } else if (curr_node.mat_anio == "5") {
                        if (curr_node.mat_cuatrim == "1") {
                            curr_node.x += 1;
                        } else {
                            curr_node.x += 1.5;
                        }
                    } else if (curr_node.mat_anio == "6") {
                        if (curr_node.mat_cuatrim == "1") {
                            curr_node.x += 2;
                        } else {
                            curr_node.x += 2.5;
                        }
                    }

                }
            }

            simulation.force("splitting", splitting_force);

            var label = svg.append("g")
                .selectAll("text")
                .data(nodes_data)
                .enter().append("text")
                .attr('text-anchor', 'middle')
                .attr('dominant-baseline', 'central')
                .text(function (d) { return d.mat_des; });
        }

        return directive;
    })

})();