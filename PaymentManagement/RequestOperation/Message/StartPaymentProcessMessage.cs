using Integrator.Models;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.RequestOperation.Message
{
    [Serializable]
    public class StartPaymentProcessMessage : BaseRequest
    {
        public PaymentInformation PaymentInformation { get; set; }
        public User Customer { get; set; }
        public int ApiOwner { get; set; }
        public RequestBase Request { get; set; }
    }
}