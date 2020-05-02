using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Response
{
    public class SessionResponse : ResponseBase
    {
        public string SessionToken { get; set; }
    }
}