using NUnit.Framework;
using PaymentManagement.DbOperation;
using PaymentManagement.Models.PaymentModels;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using System.Configuration;

namespace PaymentIntegratorTest
{
    public class Tests
    {
        [SetUp]
        public void Setup() {
            PaymentIntegrationDb.SetupDb("Server=.;Database=PaymentDb;User Id=PaymentUser;Password=123456;MultipleActiveResultSets=false");
            PaymentConfiguration.Instance.InitConfigurations();
        }

        [Test]
        public void Test1() {
            Assert.Pass();
        }

        [Test]
        public void BankIntegrationTest() {            
            RequestManager requestManager = new RequestManager();
            BankRequest request = new BankRequest {
                ActionType = ActionType.SALE,
                ApiOwner = PaymentApiOwner.UniSoft,
                Request = new UniPayRequest {
                    PaymentInformation = new PaymentInformation {
                        PaymentId = "1111-2222",
                        CurrencyCode="TRY",
                        TotalAmount=500
                    },
                    CardInformation = new CardInformation {
                        CardNumber = "4355084355084358",
                        Cvc = 000,
                        ExpiryDate = "10.2020",
                        HolderName="a"
                    },
                    Customer = new User {
                        Name = "Test",
                        Surname = "Customer"
                    },
                    Action = ActionType.SALE
                }
            };

            IResponseBase response = requestManager.MakeBankRequest<SuccessResponse>(request);
        }
    }
}
