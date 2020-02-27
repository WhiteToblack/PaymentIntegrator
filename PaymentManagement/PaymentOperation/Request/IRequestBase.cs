using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.Models.PaymentModels.Request
{
    public interface IRequestBase
    {
        string MerchantUser { get; set; }
        string MerchantPassword { get; set; }
        string Merchant { get; set; }
        PaymentInformation PaymentInformation { get; set; }
        string MerchantPaymentId { get; set; }
        User Customer { get; set; }
        CardInformation CardInformation { get; set; }
    }
}
