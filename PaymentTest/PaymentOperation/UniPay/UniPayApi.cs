using PaymentTest.Models.PaymentModels.Request;
using PaymentTest.Models.PaymentModels.Response;
using PaymentTest.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.PaymentOperation
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
