using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using SaverFunctionApp.Model;

namespace CustomerFunctionsApp.Interfaces
{
    interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomer(CloudTable customerTable);
        Task AddCustomerToQueueAsync(IAsyncCollector<Customer> queue, Customer customer);
        Task AddCustomerToStore(CustomerKey customerKey, CloudTable customerTable, Customer customer);
    }
}
