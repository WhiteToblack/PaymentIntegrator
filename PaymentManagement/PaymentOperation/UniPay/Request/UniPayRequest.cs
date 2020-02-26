using PaymentManagement.Models.PaymentModels;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.UniPay
{
    public class UniPayRequest : RequestBase
    {      
        public ActionType Action { get; set; }
    }
}
