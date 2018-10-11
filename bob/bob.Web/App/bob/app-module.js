(function () {
    'use strict';
    var appModule = angular.module('bob', ['ui.router']);

    appModule.value('apiBase', 'http://localhost:52178/api/caece/');

    appModule.config(function ($stateProvider, $urlRouterProvider) {
        var states = [
            {
                name: 'cursos',
                url: '/cursos',
                template: '<cursos></cursos>'
            },
            {
                name: 'finales',
                url: '/finales',
                template: '<finales></finales>'
            },
            {
                name: 'estadisticas',
                url: '/estadisticas',
                template: '<estadisticas></estadisticas>'
            }
            //{
            //    name: 'course',
            //    url: '/course/{courseId}',
            //    resolve: {
            //        courseId: function ($stateParams) {
            //            return $stateParams.courseId;
            //        }
            //    },
            //    template: '<course course-id="$resolve.courseId"></course>'
            //}
        ];

        $urlRouterProvider.otherwise('/');

        states.forEach(function (state) {
            $stateProvider.state(state);
        });
    });

    appModule.value('componentBorders', false);

    appModule.run(function (componentBorders) {
        if (componentBorders) {
            if (appModule._invokeQueue) {
                appModule._invokeQueue.forEach(function (item) {
                    if (item[1] === 'component') {
                        var componentName = item[2][0];
                        var componentProperties = item[2][1];
                        if (componentProperties.templateUrl) {
                            var templateUrl = componentProperties.templateUrl;
                            delete componentProperties.templateUrl;
                            componentProperties.template = '<div class="component-borders"><b>' + componentName + '</b><div ng-include="\'' + templateUrl + '\'"></div></div>';
                        }
                        else {
                            var template = '<div class="component-borders">' + componentName + '<div>' + componentProperties.template + '</div></div>';
                            componentProperties.template = template;
                        }
                    }
                });
            }
        }
    });

})();


