using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.Models.PaymentModels
{
    [Serializable]
    public class PaymentInformation
    {
        public string PaymentId { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public DateTime PaymentCompleteDate { get; set; }
        public string CurrencyCode { get; set; }
    }
}
