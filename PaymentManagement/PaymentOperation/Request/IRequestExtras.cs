using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Request
{
    public interface IRequestExtras
    {
        string ReferrerUrl { get; set; }
    }
}