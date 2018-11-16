angular.module('app.services')
    .service('customerService', ['$http', function ($http, config) {

        var resourceUrl = 'http://localhost:7071/api/';

        return {
            getAll: function () {
                var requestUrl = resourceUrl + 'Customers';

                return $http.get(requestUrl).then(function (response) { return response.data; });
            },
            addCustomer: function (customer) {
                var requestUrl = resourceUrl + 'Saver';

                return $http.post(requestUrl, customer).then(function (response) { return response.status; });
            }
        };
    }]);