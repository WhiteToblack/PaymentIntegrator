using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.RequestOperation
{
    public interface IReqeustManager
    {
        T MakeHttpRequest<T>(PaymentApiOwner apiOwner) where T : IResponseBase;
    }
}
