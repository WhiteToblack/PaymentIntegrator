using PaymentTest.Models.PaymentModels.Request;
using PaymentTest.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.PaymentOperation
{
    public interface IApiOwner
    {
        T PaymentRequest<T>(IRequestBase request) where T : IResponseBase;
    }
}
