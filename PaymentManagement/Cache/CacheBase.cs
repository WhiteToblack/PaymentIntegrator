using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.Cache
{
    public abstract class CacheBase : ICacheBase
    {

        public CacheBase() {
            Caches = GetAllValues();
        }

        public Dictionary<string, string> Caches { get; set; }
        private static readonly Lazy<Dictionary<string, string>> instance =
                        new Lazy<Dictionary<string, string>>(() => Activator.CreateInstance(typeof(Dictionary<string, string>), true) as Dictionary<string, string>);

        public static Dictionary<string, string> CacheInstance {
            get {
                return instance.Value;
            }
        }

        public abstract string GetValue(string key);
        public abstract Dictionary<string, string> GetAllValues();

        public void Dispose() {
            Caches = new Dictionary<string, string>();
        }
    }

}