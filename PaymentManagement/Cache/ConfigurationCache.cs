using PaymentManagement.RequestOperation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.Cache
{
    public sealed class ConfigurationCache : CacheBase
    {
        private static ConfigurationCache instance = null;
        private static object lockObj = new object();
        public static ConfigurationCache Instance {
            get {
                if (instance == null) {
                    lock (lockObj) {
                        if (instance == null) {
                            instance = new ConfigurationCache();
                        }                       
                    }                    
                }

                return instance;
            }
        }
        public ConfigurationCache() : base() {         
        }

        public override Dictionary<string, string> GetAllValues() {
            ApiMethodCaller apiMethodCaller = new ApiMethodCaller();
            return apiMethodCaller.GetConfigurations();            
        }

        public override string GetValue(string key) {
            return Caches.Where(x => x.Key == key).FirstOrDefault().Value.ToString();
        }        
    }
}