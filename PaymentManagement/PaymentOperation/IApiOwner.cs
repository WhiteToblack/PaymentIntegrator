using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public interface IApiOwner
    {
        T PaymentRequest<T>(IRequestBase request) where T : IResponseBase;
    }
}
