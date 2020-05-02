using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.Cache
{
    public interface ICacheBase : IDisposable
    {
        string GetValue(string key);
        Dictionary<string, string> GetAllValues();
    }
}