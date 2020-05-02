using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Response {
    public class Payment {

        public string PaymentDate { get; set; }
        public string PaymentAmount { get; set; }
        public string PaidAmount { get; set; }
        public string PanIIN { get; set; }
        public string PanLast4 { get; set; }
        public string DealerCode { get; set; }
        public string DealerName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameMasked { get; set; }
        public string MerchantPaymentId { get; set; }
        public string MerchantName { get; set; }
        public string Currency { get; set; }
        public string InvoiceIds { get; set; }
    }
}