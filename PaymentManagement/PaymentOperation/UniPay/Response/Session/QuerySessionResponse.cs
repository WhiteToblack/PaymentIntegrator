using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.UniPay.Response.Session
{
    public class QuerySessionResponse : IQuerySessionResponse
    {
        public string Token { get; set; }
        public string ApiAction { get; set; }
        public string MerchantPaymentId { get; set; }
        public string FirstAmount { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string ReturnUrl { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public string RedirectWaitingTime { get; set; }
        public string Extra { get; set; }
        public string Status { get; set; }
        public string MaxInstallmentCount { get; set; }
        public string SessionCreateTimestamp { get; set; }
    }
}

