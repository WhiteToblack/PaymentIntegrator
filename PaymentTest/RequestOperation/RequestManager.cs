using PaymentTest.Models.PaymentModels.Response;
using PaymentTest.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.RequestOperation
{
    public class RequestManager : IReqeustManager
    {
        public T MakeHttpRequest<T>(PaymentApiOwner apiOwner) where T : IResponseBase {
            ApiOwnerProvider apiProvider = new ApiOwnerProvider();

        }
    }
}
