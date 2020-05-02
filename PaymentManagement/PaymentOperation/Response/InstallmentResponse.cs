using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Response
{
    public class InstallmentResponse : ResponseBase
    {
        public int MinAllowedInstallmentCount { get; set; }
        public int MaxAllowedInstallmentCount { get; set; }
    }
}