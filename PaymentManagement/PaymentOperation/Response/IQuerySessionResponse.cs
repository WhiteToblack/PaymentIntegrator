using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.UniPay.Response.Session
{
    public interface IQuerySessionResponse
    {
       string Token { get; set; }
       string ApiAction { get; set; }    
       string FirstAmount { get; set; }
       string Amount { get; set; }
       string Currency { get; set; }
       string ReturnUrl { get; set; }
       string Language { get; set; }
       string TimeZone { get; set; }
       string RedirectWaitingTime { get; set; }
       string Extra { get; set; }
       string Status { get; set; }
       string MaxInstallmentCount { get; set; }
       string SessionCreateTimestamp { get; set; }
    }
}