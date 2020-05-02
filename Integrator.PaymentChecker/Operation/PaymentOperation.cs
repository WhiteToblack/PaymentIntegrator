using Integrator.Models;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integrator.PaymentChecker.Operation {
    public static class PaymentOperation {      

        public static List<PaymentInformation> GetPendingPayments() {
            ApiMethodCaller apiMethodCaller = new ApiMethodCaller();
            List<PaymentInformation> paymentInformationList = apiMethodCaller.GetPendingPayments();
            return paymentInformationList;
        }
       
    }
}
