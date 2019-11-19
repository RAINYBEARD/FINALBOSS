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
                name: 'registrar',
                url: '/registrar',
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
                name: 'landing',
                url: '/',
                views: {
                    'content@': {
                        templateProvider: ['$state', 'authService', function ($state, authService) {
                            if (authService.authentication.isAuth) {
                                authService.isAdmin().then(function (response) {
                                    if (response) {
                                        $state.go('admin');
                                    }
                                    else {
                                        $state.go('bob.pendientes');
                                    }
                                });
                            }
                        }]
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
                name: 'admin',
                url: '/admin',
                views: {
                    'content@': {
                        template: '<admin></admin>'
                    }
                }
            },
            {
                name: 'bob.planestudio',
                url: '/planestudio',
                views: {
                    'bob@bob': {
                        template: '<planestudio></planestudio>'
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
                name: 'bob.pendientes',
                url: '/pendientes',
                views: {
                    'bob@bob': {
                        template: '<pendientes></pendientes>'
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

        $urlRouterProvider.otherwise('/');
        $urlRouterProvider.when('/bob', '/bob/planestudio');
        states.forEach(function (state) {
            $stateProvider.state(state);
        });
    });


    appModule.run(function ($rootScope, $state, authService, localStorageService) {
        $rootScope.$on('unauthorized', function (event) {
            event.preventDefault();
            $state.go('ingresar');
        });
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                authService.authentication.isAuth = true;
                authService.authentication.username = authData.username;
            }
            let loggedIn = authService.authentication.isAuth;
            if (toState.name !== 'ingresar' && toState.name !== 'registrar' && toState.name !== 'cambiar' && !loggedIn) {
                event.preventDefault();
                $state.go('ingresar');
            }

            if (loggedIn) {
                authService.isAdmin().then(function (response) {
                    if (response) {
                        event.preventDefault();
                        $state.go('admin');
                    }
                    else {
                        if (toState.name === 'admin') {
                            event.preventDefault();
                            $state.go('bob');
                        }
                    }
                });
            }
        });

    });

})();