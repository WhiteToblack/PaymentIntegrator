using System;
using System.Collections.Generic;
using System.Text;

namespace Integrator.Models
{
    public class AmountInformation
    {
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public bool UseInstallment{ get; set; }     
        public decimal InstallmentAmount { get; set; }
        public bool IsSelected { get; set; }
        public string DisplayText { get; set; }
        public int Id { get; set; }
    }
}
