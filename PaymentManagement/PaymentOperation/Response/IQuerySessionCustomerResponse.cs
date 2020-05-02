using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Response
{
    public interface IQuerySessionCustomerResponse
    {
        string Id { get; set; }
        DateTime LastLogin { get; set; }
        string IdentityNumber { get; set; }       
    }
}