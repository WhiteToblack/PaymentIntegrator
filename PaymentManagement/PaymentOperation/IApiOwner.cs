using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public interface IApiOwner
    {
        IResponseBase PaymentRequest<T>(IBankRequest request) where T : ResponseBase, IResponseBase;
        IBankRequest PrepareRequest(IRequestBase request);
        int GetInstallementCount();
        SessionResponseBase CreateSession(IBankRequest bankRequest);
        IResponseBase QuerySession(string sessionId);
        IResponseBase QueryPayment(string sessionId);
    }
}
