using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public interface IApiCaller
    {
        Task<T> Call<T>(string api, string methodName, object jsonObj) where T : new();
    }
}
