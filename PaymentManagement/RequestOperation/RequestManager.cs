using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.RequestOperation
{
    public class RequestManager : IReqeustManager
    {
        public T MakeHttpRequest<T>(PaymentApiOwner apiOwner) where T : IResponseBase {
            ApiOwnerProvider apiProvider = new ApiOwnerProvider(apiOwner);
            string url = apiProvider.GetApiUrl();

            throw new NotImplementedException();

        }

     
    }
}
