using Integrator.Models;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.RequestOperation.Message
{
    [Serializable]
    public class SaveSessionIdMessage : BaseRequest
    {
        public string SessionId{ get; set; }        
    }
}