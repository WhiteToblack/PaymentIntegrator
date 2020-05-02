using Integrator.Models;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.RequestOperation.Message
{
    [Serializable]
    public class SavePaymentInformationMessage : BaseRequest
    {
        public PaymentInformation PaymentInformation { get; set; }      
    }
}