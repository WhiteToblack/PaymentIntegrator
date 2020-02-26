using PaymentManagement.Models.PaymentModels;
using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.UniPay
{
    public class SuccessResponse : ResponseBase
    {
        public string Action { get; set; }
        public string Merchant { get; set; }
        public decimal Amount { get; set; }
        public string Installment { get; set; }
        public string Currency { get; set; }
        public string ApiMerchantId { get; set; }
        public string PaymentSystem { get; set; }
        public string PaymentSystemType { get; set; }
        public string PaymentSystemEftCode { get; set; }
        public DateTime PgTranDate { get; set; }
        public string MerchantPaymentId { get; set; }
        public string PgTranId { get; set; }
        public string PgTranRefId { get; set; }
        public string PgOrderId { get; set; }
        public string PgTranApprCode { get; set; }
        public PaymentResponseType ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
        public string PanLast4 { get; set; }
        public string CardBin { get; set; }
        public decimal FinalAmount { get; set; }
        public object BankResponseExtras { get; set; }

    }
}
