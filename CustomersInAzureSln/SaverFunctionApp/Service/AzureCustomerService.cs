using CustomerFunctionsApp.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using SaverFunctionApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFunctionsApp.Service
{
    public class AzureCustomerService : ICustomerService
    {
        public async Task AddCustomerToQueueAsync(IAsyncCollector<Customer> queue, Customer customer)
        {
            if(queue == null || customer == null)
                throw new ArgumentNullException();

            //Add Customer to the Queue
            await queue.AddAsync(customer);
        }

        public async Task<List<Customer>> GetAllCustomer(CloudTable customerTable)
        {
            if(customerTable == null)
                throw new ArgumentNullException(nameof(customerTable));

            TableContinuationToken token = null;

            var customers = new List<Customer>();

            //Get Segmented Collection of Customer from Azure Storage.
            do
            {
                var queryResult = await customerTable.ExecuteQuerySegmentedAsync(new TableQuery<Customer>(), token);
                customers.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;

            }while (token != null);

            return customers;
        }

        public async Task AddCustomerToStore(CustomerKey customerKey, CloudTable customerTable, Customer customer)
        {
            if(customerTable == null || customer == null)
                throw new ArgumentNullException();

            //If Key do not exists, create one. Should happens only the first time.(Found in example, need a litle more research to understand if is neccessary)
            if (customerKey == null)
            {
                customerKey = new CustomerKey
                {
                    PartitionKey = "1",
                    RowKey = "KEY",
                    Id = 1024
                };
                var addKey = TableOperation.Insert(customerKey);
                await customerTable.ExecuteAsync(addKey);
            }

            string code = GetCodeByKey(customerKey);

            //Update customer properties related to Key.(Found in example, need a litle more research to understand if is neccessary)
            customer.PartitionKey = $"{code[0]}";
            customer.RowKey = code;

            //Increment and insert the Key value(Found in example, need a litle more research to understand if is neccessary)
            customerKey.Id++;
            var operation = TableOperation.Replace(customerKey);
            await customerTable.ExecuteAsync(operation);

            //Insert Customer object to Azure Storage.
            operation = TableOperation.Insert(customer);
            await customerTable.ExecuteAsync(operation);
        }

        /// <summary>
        /// Get the code generated for the provided key.
        /// </summary>
        /// <param name="customerKey"></param>
        /// <returns></returns>
        private string GetCodeByKey(CustomerKey customerKey)
        {
            //(Found in example, need a litle more research to understand if is neccessary)

            int idx = customerKey.Id;
            const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var s = string.Empty;
            while (idx > 0)
            {
                s += ALPHABET[idx % ALPHABET.Length];
                idx /= ALPHABET.Length;
            }
            var code = string.Join(string.Empty, s.Reverse());
            return code;
        }
    }
}
