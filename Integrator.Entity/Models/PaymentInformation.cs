using System;
using System.Collections.Generic;
using System.Linq;

namespace Integrator.Models
{
    [Serializable]
    public class PaymentInformation
    {
        public string PaymentId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaymentStartDate { get; set; }
        public DateTime PaymentCompleteDate { get; set; }
        public List<AmountInformation> AmountInformation { get; set; }
        public int SelectedAmountId { get; set; }
        public bool Use3DPayment { get; set; }
        public int MaxInstallmentCount { get; set; }
        public string SessionToken { get; set; }
        public decimal TotalAmount {
            get {
                if (this.AmountInformation == null) {
                    return 0;
                }

                return this.AmountInformation.Where(x => x.IsSelected).FirstOrDefault().TotalAmount;
            }
        }
        public string CurrencyCode {
            get {
                if (this.AmountInformation == null) {
                    return "";
                }

                return this.AmountInformation.Where(x => x.IsSelected).FirstOrDefault().CurrencyCode;
            }
        }
        public int InstallmentCount { get; set; }
        public decimal InstallmentAmount {
            get {
                if (this.AmountInformation == null) {
                    return 0;
                }

                return this.AmountInformation.Where(x => x.IsSelected).FirstOrDefault().InstallmentAmount;
            }
        }
    }
}
