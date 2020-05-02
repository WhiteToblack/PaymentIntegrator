using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.Response
{
    public class FailedResponse : ResponseBase
    {
        public string ErrorCode { get; set; }
        public string PgTranErrorText { get; set; }
        public string PgTranErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public string ViolatorParam { get; set; }
    }
}
