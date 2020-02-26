using PaymentTest.Models.PaymentModels;
using PaymentTest.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.PaymentOperation.UniPay
{
    public class UniPayRequest : RequestBase
    {      
        public ActionType Action { get; set; }
    }
}
