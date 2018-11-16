var customerController = function ($scope, services) {

    $scope.customer = {};

    function getCustomers() {
        services.customer.getAll().then(function (data) {
            $scope.customers = data;
        });
    }

    getCustomers();

    $scope.addCustomer = function () {
        services.customer.addCustomer($scope.customer).then(function (responseStatus) {
            if (responseStatus === 200) {
                //NOT UPDATING UI CORRECTLY
                //$scope.customers.push(
                //    {
                //        Name: $scope.customer.Name,
                //        Address: $scope.customer.Address,
                //        Phone: $scope.customer.Phone,
                //        Email: $scope.customer.Email,
                //        Age: $scope.customer.Age
                //    }
                //);

                //TEMPORARY
                getCustomers();

                cleanCustomer();
            }
        });
    };

    function cleanCustomer() {
        $scope.customer = {};
    }
};

customerController.$inject = ['$scope', 'services'];