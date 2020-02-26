using PaymentTest.Models.PaymentModels.Request;
using PaymentTest.Models.PaymentModels.Response;
using PaymentTest.PaymentOperation.Request;
using PaymentTest.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.PaymentOperation
{
    public class ApiCaller : IApiOwner, IApiCaller
    {

        IApiOwner api = null;
        IRequestBase request = null;

        public ApiCaller(PaymentApiOwner apiOwner) {
            ApiOwnerProvider apiOwnerProvider = new ApiOwnerProvider(apiOwner);
            api = apiOwnerProvider.GetApiOwner();            
        }
        public T PaymentRequest<T>(IRequestBase _request) where T : IResponseBase {          
            return api.PaymentRequest<T>(_request); 
        }
    }
}
