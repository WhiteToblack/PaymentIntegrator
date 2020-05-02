using Integrator.Models;
using System;

namespace PaymentManagement.Models.PaymentModels.Response
{
    public class ResponseBase : IResponseBase
    {
        public ActionType Action { get; set; }
        public PaymentResponseType ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
        public decimal FinalAmount { get; set; }
        public string Redirect3dResponse { get; set; }
        public string MerchantPaymentId { get; set; }
        public object OrderItems { get; set; }
        public string SessionToken { get; set; }
    }
}
