﻿using PaymentTest.Models.PaymentModels;
using PaymentTest.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.PaymentOperation.UniPay
{
    public class FailedResponse : ResponseBase
    {
        public PaymentResponseType ErrorCode { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMsg { get; set; }
        public string ResponseMsg { get; set; }
        public string ViolatorParam { get; set; }
    }
}
