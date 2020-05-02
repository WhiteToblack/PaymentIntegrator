using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integrator.Test
{
    public class Common
    {
        /// TODO: parametre tipine göre kart bilgilerinin hatalı olması durumlarının testi yazılacak
        public static BankRequest GetBankRequest(IApiOwner apiOwner, bool valid) {
            string cvv = valid ? "000" : "0001";
            List<AmountInformation> amountInformations = new List<AmountInformation> {
                new AmountInformation {
                    TotalAmount = 400,
                    UseInstallment = true,
                    CurrencyCode = "TRY",
                    InstallmentAmount = 100,
                    IsSelected = true
                },
                new AmountInformation {
                    TotalAmount = 300,
                    UseInstallment = false,
                    CurrencyCode = "TRY",
                    InstallmentAmount = 500,
                    IsSelected = false
                }
            };

            RequestBase request = new RequestBase {
                PaymentInformation = new PaymentInformation {
                    PaymentId = PaymentManagement.GeneralOperation.UniPay.GuidMaker.GetGuid(),
                   AmountInformation = amountInformations
                },
                CardInformation = new CardInformation {
                    CardNumber = "4355084355084358",
                    Cvv = cvv,
                    ExpiryDate = "10.2020",
                    HolderName = "a"
                },
                Customer = new User {
                    Name = "Test",
                    Surname = "Customer",
                    Division = "Test Divison"
                }
            };


            RequestBase requestBase = new RequestBase {
                PaymentInformation = request.PaymentInformation,
                CardInformation = request.CardInformation,
                Customer = request.Customer
            };

            BankRequest bankRequest = (BankRequest)apiOwner.PrepareRequest(requestBase);
            bankRequest.Response = new ResponseBase {
                Action = ActionType.SALE,
                ResponseCode = 00,
                ResponseMsg = "Succeded For Test"
            };

            return bankRequest;
        }

        public static IResponseBase CreateBankSession(IApiOwner apiOwner) {
            BankRequest request = GetBankRequest(apiOwner,true);
            request.ReturnUrl = "http://localhost/PaymentIntegrator/Payment/On3dCompleted";

            IResponseBase response = apiOwner.CreateSession(request);
            return response;
        }
    }
}
