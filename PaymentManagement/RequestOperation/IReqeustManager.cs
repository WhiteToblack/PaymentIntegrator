using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.RequestOperation
{
    public interface IReqeustManager
    {
        T MakeBankRequest<T>(BankRequest bankRequest) where T : IResponseBase;
    }
}
