using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.UniPay.Response.Customer;
using PaymentManagement.PaymentOperation.UniPay.Response.Merchant;
using PaymentManagement.PaymentOperation.UniPay.Response.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Response
{
    public class SessionResponseBase : ResponseBase
    {
        public QuerySessionResponse Session { get; set; }
        public QuerySessionCustomerResponse Merchant { get; set; }
        public QuerySessionMerchantResponse Customer { get; set; }    
    }
}