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
                name: 'login',
                url: '/login',
                template: '<login></login>'
            },
            {
                name: 'register',
                url: '/register',
                template: '<register></register>'
            },
            {
                name: 'changepassword',
                url: '/changepassword',
                template: '<changepassword></changepassword>'
            },
            {
                name: 'arbol',
                url: '/arbol',
                template: '<arbol></arbol>'
            },

            {
                name: 'cursos',
                url: '/cursos',
                template: '<cursos></cursos>'
            },
            {
                name: 'cursos.auto',
                url: '/auto',
                template: '<cursos-auto></cursos-auto>'
            },
            {
                name: 'cursos.manual',
                url: '/manual',
                template: '<cursos-manual></cursos-manual>'
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
        ];

        $urlRouterProvider.otherwise('/login');

        states.forEach(function (state) {
            $stateProvider.state(state);
        });
    });


    appModule.run(function ($rootScope, $state, authService) {
        $rootScope.$on('unauthorized', function (event) {
            event.preventDefault();
            $state.go('login');
        });
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            let loggedIn = authService.authentication.isAuth;
            if (toState.name !== 'login' && toState.name !== 'register' && toState.name !== 'changepassword' && !loggedIn) {
                event.preventDefault();
                $state.go('login');
            }
        });

    });

})();