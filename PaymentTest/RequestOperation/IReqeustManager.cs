using PaymentTest.Models.PaymentModels.Response;
using PaymentTest.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.RequestOperation
{
    public interface IReqeustManager
    {
        T MakeHttpRequest<T>(PaymentApiOwner apiOwner) where T : IResponseBase;
    }
}
