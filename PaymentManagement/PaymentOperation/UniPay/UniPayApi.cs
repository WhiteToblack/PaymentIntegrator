using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public class UniPayApi : IApiOwner
    {
        public T PaymentRequest<T>(IRequestBase request) where T : IResponseBase {
            UniPayRequest uniPayRequest = (UniPayRequest)request;
            IResponseBase response = new SuccessResponse();

            return (T)response;
        }
    }
}
