angular.module('app.services', [])
    .service('services',
        [
            'customerService',

            function (
                customerService
            ) {
                return {
                    customer: customerService
                };
            }]);