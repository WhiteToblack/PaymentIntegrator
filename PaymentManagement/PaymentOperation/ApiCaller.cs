using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public class ApiCaller : IApiOwner, IApiCaller
    {

        IApiOwner api = null;     

        public ApiCaller(PaymentApiOwner apiOwner) {
            ApiOwnerProvider apiOwnerProvider = new ApiOwnerProvider(apiOwner);
            api = apiOwnerProvider.GetApiOwner();            
        }
        public T PaymentRequest<T>(IRequestBase _request) where T : IResponseBase {          
            return api.PaymentRequest<T>(_request); 
        }
    }
}
