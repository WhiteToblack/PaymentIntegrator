using PaymentManagement.Models.PaymentModels;
using PaymentManagement.Models.PaymentModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.Request
{
    public class RequestBase:IRequestBase
    {
        public string MerchantUser { get; set; }
        public string MerchantPassword { get; set; }
        public string Merchant { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public string MerchantPaymentId { get; set; }
        public User Customer { get; set; }
    }
}
