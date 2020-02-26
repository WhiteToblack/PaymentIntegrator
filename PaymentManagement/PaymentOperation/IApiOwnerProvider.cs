using PaymentManagement.Models.PaymentModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public interface IApiOwnerProvider
    {
        IApiOwner GetApiOwner();
        IRequestBase GetRequestInstance();
    }
}
