using Integrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PaymentIntegrator.UI.Models
{
    public class PayingUser
    {
        public User User { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public CardInformation CardInformation { get; set; }
        public dynamic Extras { get; set; }
        public int InstallmentCount { get; set; }
        public IEnumerable<SelectListItem> Installments {
            get {
                var selectList = new List<SelectListItem>();
                AmountInformation amountInformation = this.PaymentInformation.AmountInformation.Where(x => x.UseInstallment).FirstOrDefault();
                if (amountInformation != null) {
                    for (int i = 1; i < this.PaymentInformation.MaxInstallmentCount; i++) {
                        selectList.Add(new SelectListItem {
                            Value = i.ToString(),
                            Text = i.ToString()
                        });
                    }
                }

                return selectList;
            }
        }

        public IEnumerable<SelectListItem> Amounts {
            get {
                var selectList = new List<SelectListItem>();
                for (int i = 0; i < this.PaymentInformation.AmountInformation.Count; i++) {
                    selectList.Add(new SelectListItem {
                        Value = this.PaymentInformation.AmountInformation[i].Id.ToString(),
                        Text = this.PaymentInformation.AmountInformation[i].DisplayText.ToString()
                    });
                }

                return selectList;
            }
        }
        public int MaxInstallmentCount { get; set; }
    }
}
