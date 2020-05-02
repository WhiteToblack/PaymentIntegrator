using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;

namespace PaymentManagement.PaymentOperation.Request
{
    public interface IBankRequest
    {
       ActionType ActionType { get; set; }
       IRequestBase Request { get; set; }
       IResponseBase Response { get; set; }
       PaymentApiOwner ApiOwner { get; set; }
       SessionType SessionType { get; set; }
       string SessionToken { get; set; }
       string ReturnUrl { get; set; }
       bool Is3DUsed { get; set; }
    }
}