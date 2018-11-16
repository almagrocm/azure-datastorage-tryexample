angular.module('app.customer', [])
    .config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('/AddCustomer',
                {
                    url: '/BaseUI/Customer',
                    views: {
                        'containerOne': {
                            templateUrl: '/BaseUI/app/views/customer/customer.template.html',
                            controller: 'customerController',
                            controllerAs: 'ctrl'
                        }
                    }

                });
    }])
    .controller('customerController', customerController);