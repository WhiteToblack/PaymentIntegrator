using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentManagement.PaymentOperation.Request
{
    public class BankRequest
    {
        public ActionType ActionType{ get; set; }
        public IRequestBase Request{ get; set; }
        public PaymentApiOwner ApiOwner { get; set; }
    }
}
