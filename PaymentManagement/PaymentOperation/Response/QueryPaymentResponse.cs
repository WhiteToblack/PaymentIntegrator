using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.PaymentOperation.Response {
    public class QueryPaymentResponse : ResponseBase {
        public List<Payment> Payments { get; set; }
    }
}