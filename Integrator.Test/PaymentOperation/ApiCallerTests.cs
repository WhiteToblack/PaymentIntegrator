using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentManagement.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.Tests
{
    [TestClass()]
    public class ApiCallerTests
    {       
        public void Setup() {
            new ConfigurationBuilder()
                 .AddJsonFile("appsettings.development.json")
                 .Build();
        }

        [TestMethod()]
        public void CallTest() {
            Setup();
            ApiCaller apiCaller = new ApiCaller();
            Dictionary<string, string> configurations = Task<Dictionary<string,string>>.Run(()=>apiCaller.Call<Dictionary<string, string>>("Configurations", "", null)).Result;
            Assert.IsTrue(configurations.Values.Count > 2);
        }
    }
}