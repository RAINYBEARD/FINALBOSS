(function () {
    'use strict';
    var appModule = angular.module('bob', ['ui.router', 'LocalStorageModule']);

    appModule.value('base', 'http://localhost:52178/');
    appModule.value('caeceApi', 'http://localhost:52178/api/v1/caece/');
    appModule.value('authApi', 'http://localhost:52178/api/v1/auth/');

    appModule.constant('ngAuthSettings', {
        clientId: 'bob'
    });
    appModule.config(function ($stateProvider, $urlRouterProvider, $httpProvider) {

        $httpProvider.interceptors.push('authInterceptorService');

        var states = [
            {
                name: 'ingresar',
                url: '/ingresar',
                views: {
                    'content@': {
                        template: '<login></login>'
                    }
                }
            },
            {
                name: 'registro',
                url: '/registro',
                views: {
                    'content@': {
                        template: '<register></register>'
                    }
                }
            },
            {
                name: 'cambiar',
                url: '/cambiar',
                views: {
                    'content@': {
                        template: '<changepassword></changepassword>'
                    }
                }
            },
            {
                name: 'bob',
                url: '/bob',
                views: {
                    'content@': {
                        template: '<bob-app></bob-app>'
                    }
                }
            },
            {
                name: 'bob.arbol',
                url: '/arbol',
                views: {
                    'bob@bob': {
                        template: '<arbol></arbol>'
                    }
                }
            },
            {
                name: 'bob.finales',
                url: '/finales',
                views: {
                    'bob@bob': {
                        template: '<finales></finales>'
                    }
                }
            },
            {
                name: 'bob.estadisticas',
                url: '/estadisticas',
                views: {
                    'bob@bob': {
                        template: '<estadisticas></estadisticas>'
                    }
                }
            },
            {
                name: 'bob.cursos',
                url: '/cursos',
                views: {
                    'bob@bob': {
                        template: '<cursos></cursos>'
                    }
                }
            },
            {
                name: 'bob.cursos.auto',
                url: '/auto',
                template: '<cursos-auto></cursos-auto>'
            },
            {
                name: 'bob.cursos.manual',
                url: '/manual',
                template: '<cursos-manual></cursos-manual>'
            }
        ];

        $urlRouterProvider.otherwise('/bob');
        $urlRouterProvider.when('/bob', '/bob/arbol');
        states.forEach(function (state) {
            $stateProvider.state(state);
        });
    });


    appModule.run(function ($rootScope, $state, authService) {
        $rootScope.$on('unauthorized', function (event) {
            event.preventDefault();
            $state.go('ingresar');
        });
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            authService.fillAuthData();
            let loggedIn = authService.authentication.isAuth;
            if (toState.name !== 'ingresar' && toState.name !== 'registro' && toState.name !== 'cambiar' && !loggedIn) {
                event.preventDefault();
                $state.go('ingresar');
            }
        });

    });

})();