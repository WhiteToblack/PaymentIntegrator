using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.RequestOperation.Message
{
    [Serializable]
    public class FinalizePaymentProcessMessage : BaseRequest
    {
        public ResponseBase Response{ get; set; }        
    }
}