using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Request
{
    public class RequestExtras : IRequestExtras
    {
        public string ReferrerUrl { get; set; }
        public string ExternalRefNumber { get; set; }
    }
}