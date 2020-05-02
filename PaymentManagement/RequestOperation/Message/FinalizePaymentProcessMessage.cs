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
    public class UpdateBankResponseMessage : BaseRequest
    {
        public string PaymentId{ get; set; }
        public string BankResponse { get; set; }
    }
}