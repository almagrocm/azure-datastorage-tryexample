using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaverFunctionApp.Model
{
    public class CustomerKey: TableEntity
    {
        public int Id {get; set;}
    }

    public class Customer : TableEntity
    {
        public string Name {get; set;}

        public string Address {get; set;}

        public string Phone {get; set;}

        public string Email {get; set;}

        public int Age {get; set;}
    }    
}
