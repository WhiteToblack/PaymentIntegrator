using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.Response
{
    public class SuccessResponse : ResponseBase
    {     
        public string Merchant { get; set; }
        public decimal Amount { get; set; }
        public string Installment { get; set; }
        public string Currency { get; set; }
        public string ApiMerchantId { get; set; }
        public string PaymentSystem { get; set; }
        public string PaymentSystemType { get; set; }
        public string PaymentSystemEftCode { get; set; }
        public string PgTranDate { get; set; }
        public string PgTranId { get; set; }
        public string PgTranRefId { get; set; }
        public string PgOrderId { get; set; }
        public string PgTranApprCode { get; set; }
        
        public string PanLast4 { get; set; }
        public string CardBin { get; set; }     
        public object BankResponseExtras { get; set; }
    }
}

 
  