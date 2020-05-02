using PaymentManagement.PaymentOperation.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.UniPay.Response.Customer
{
    public class QuerySessionCustomerResponse : IQuerySessionCustomerResponse
    {
        public string Id { get; set; }
        public DateTime LastLogin { get; set; }
        public string IdentityNumber { get; set; }
    }
}