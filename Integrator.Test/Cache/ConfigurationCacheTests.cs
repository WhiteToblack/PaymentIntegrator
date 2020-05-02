using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentManagement.Cache;
using PaymentManagement.GeneralOperation;
using PaymentManagement.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Cache.Tests
{
    [TestClass()]
    public class ConfigurationCacheTests
    {
        [TestMethod()]
        public void GetValueTest() {
            string apiUrl = ConfigurationCache.Instance.GetValue(((int)PaymentApiOwner.UniSoft).ToString() + AuthConfig.API_URL);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(apiUrl));
        }
    }
}