using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.UniPay.Response.Merchant
{
    public interface IQuerySessionMerchantResponse
    {
        string Name { get; set; }
        string Address { get; set; }
        string Phone { get; set; }
        string WebAddress { get; set; }
        string SecretKey { get; set; }
        string WalletModel { get; set; }
        bool ImmediateDealerActivation { get; set; }
        bool ImmediateSubDealerActivation { get; set; }
        bool ActivateDebitCardSupport { get; set; }
        bool UseBankMerchantId { get; set; }   
    }
}