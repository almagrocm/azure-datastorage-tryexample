var baseUIApp = angular.module("BaseUIApp", [
    'ui.router',
    'app.services',
    'app.customer'
]);

var configFunction =
    function ($stateProvider, $urlRouterProvider, $locationProvider) {

        $urlRouterProvider.otherwise('/BaseUI/Customer');

        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    };

configFunction.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

baseUIApp.config(configFunction);

baseUIApp.controller('BaseUIController', baseUIController);