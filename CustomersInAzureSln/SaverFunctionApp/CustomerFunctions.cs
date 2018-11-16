using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CustomerFunctionsApp.Interfaces;
using CustomerFunctionsApp.Service;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using SaverFunctionApp.Model;

namespace SaverFunctionApp
{
    public static class CustomerFunctions
    {
        [FunctionName("Customers")]
        public static async Task<HttpResponseMessage> RunCustomers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req,
            [Table("customers")]CloudTable customerTable, TraceWriter log)
        {
            log.Info("HTTP trigger function 'Customers' processing a request.");

            ICustomerService customerService = new AzureCustomerService();

            try
            {
                var customers = await customerService.GetAllCustomer(customerTable);

                log.Info("HTTP trigger function 'Customers' processed a request sucessfully.");

                return req.CreateResponse(HttpStatusCode.OK, customers);
            }
            catch (Exception ex)
            {
                log.Info("HTTP trigger function 'Customers' fail to processed a request, with the message: '{ex.Message}'.");

                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [FunctionName("Saver")]
        public static async Task<HttpResponseMessage> RunSaver([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
            [Queue(queueName: "customersQueue")]IAsyncCollector<Customer> queue,
            TraceWriter log)
        {
            log.Info("HTTP trigger function 'Saver' processing a request.");

            try
            {
                // Get request body
                var customer = await req.Content.ReadAsAsync<Customer>();

                if (customer == null || string.IsNullOrWhiteSpace(customer.Email) || string.IsNullOrWhiteSpace(customer.Name))
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Customer is null, or required information like Email and Name is missing");

                ICustomerService customerService = new AzureCustomerService();

                await customerService.AddCustomerToQueueAsync(queue, customer);

                log.Info("HTTP trigger function 'Saver' processed a request sucessfully.");

                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                log.Info("HTTP trigger function 'Saver' fail to processed a request, with the message: '{ex.Message}'.");

                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [FunctionName("Extractor")]
        public static async Task RunExtractor([Table("customers")]CloudTable customerTable, 
            [Table("customers", "1", "KEY", Take = 1)]CustomerKey customerKey,
             [QueueTrigger("customersQueue")]Customer customer, TraceWriter log)
        {
            try
            {
                 ICustomerService customerService = new AzureCustomerService();

                await customerService.AddCustomerToStore(customerKey, customerTable, customer);
            }
            catch (Exception ex)
            {
                log.Info($"HTTP trigger function 'Extractor' fail to processed a request, with the message: '{ex.Message}'.");

                throw;
            }
        }
    }
}
