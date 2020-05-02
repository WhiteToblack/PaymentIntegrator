using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.UniPay.Response.Merchant
{
    public class QuerySessionMerchantResponse : IQuerySessionMerchantResponse
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WebAddress { get; set; }
        public string SecretKey { get; set; }
        public string WalletModel { get; set; }
        public bool ImmediateDealerActivation { get; set; }
        public bool ImmediateSubDealerActivation { get; set; }
        public bool ActivateDebitCardSupport { get; set; }
        public bool UseBankMerchantId { get; set; }
    }
}