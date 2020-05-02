using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.Response;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentManagement.RequestOperation.Message;
using Moq;

namespace Integrator.Test
{
    [TestClass]
    public class Tests
    {
        Mock<IApiMethodCaller> mockedApiMethodCaller = null;
        ApiMethodCaller apiMethodCaller = null;
        ApiOwnerProvider apiOwnerProvider = null;
        IApiOwner apiOwner;

        [TestMethod]
        public void Setup() {
            var config = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.development.json")
                 .Build();

            apiOwnerProvider = new ApiOwnerProvider(PaymentApiOwner.UniSoft);
            mockedApiMethodCaller = new Mock<IApiMethodCaller>();
            apiMethodCaller = new ApiMethodCaller();
            apiOwner = apiOwnerProvider.GetApiOwner();
        }

        [TestMethod]
        public void SuccessIntegrationTest() {
            Setup();
            BankRequest request = Common.GetBankRequest(apiOwner, true);

            IResponseBase response = apiOwner.PaymentRequest<ResponseBase>(request);
            Assert.AreSame(response.GetType(), typeof(SuccessResponse));
        }

        [TestMethod]
        public void Success3DIntegrationTest() {
            Setup();
            string sessionToken = ((SessionResponseBase)Common.CreateBankSession(apiOwner)).SessionToken;
            BankRequest request = Common.GetBankRequest(apiOwner, true);
            request.SessionToken = sessionToken;
            request.Is3DUsed = true;
            IResponseBase response = apiOwner.PaymentRequest<ResponseBase>(request);
            Console.WriteLine(response.Redirect3dResponse);
            Assert.AreEqual(response.ResponseCode, PaymentResponseType.Waiting);
        }

        [TestMethod]
        public void FailedIntegrationTest() {
            Setup();
            BankRequest request = Common.GetBankRequest(apiOwner, false);

            IResponseBase response = apiOwner.PaymentRequest<ResponseBase>(request);
            Assert.AreSame(response.GetType(), typeof(FailedResponse));
        }

        [TestMethod]
        public void SessionCreatingTest() {
            Setup();
            IResponseBase response = Common.CreateBankSession(apiOwner);
            Assert.AreSame(response.GetType(), typeof(SessionResponseBase));
            SessionResponseBase _response = (SessionResponseBase)response;
            Assert.IsTrue(!string.IsNullOrWhiteSpace(_response.SessionToken));
        }

        [TestMethod]
        public void GetInstallmentCount() {
            Setup();
            int installmentCount = apiOwner.GetInstallementCount();
            Assert.AreNotEqual(0, installmentCount);
        }

        [TestMethod]
        public void StartPaymentProcessTest() {
            Setup();
            BankRequest request = Common.GetBankRequest(apiOwner, true);
            StartPaymentProcessMessage methodRequest = new StartPaymentProcessMessage {
                PaymentInformation = request.Request.PaymentInformation,
                Customer = request.Request.Customer,
                ApiOwner = (int)request.ApiOwner,
                ActionType = request.ActionType,
                Request = (RequestBase)request.Request
            };

            PaymentStatus paymentStatus = apiMethodCaller.StartPaymentProcess(methodRequest);
            Assert.AreEqual(paymentStatus, PaymentStatus.Pending);
        }

        [TestMethod]
        public void SavePaymentInformationTest() {
            Setup();
            BankRequest request = Common.GetBankRequest(apiOwner, true);
            SavePaymentInformationMessage savePaymentInformationMessage = new SavePaymentInformationMessage { 
            ActionType = request.ActionType,
            PaymentInformation = request.Request.PaymentInformation
            };
            PaymentStatus paymentStatus = apiMethodCaller.SavePaymentInformation(savePaymentInformationMessage);
            if (paymentStatus == PaymentStatus.FailedPending) {
                Assert.Fail();
            }

            Assert.AreEqual(paymentStatus, PaymentStatus.Pending);
        }

        [TestMethod]
        public void SuccessPaymentProcessingTest() {
            Setup();
            BankRequest request = Common.GetBankRequest(apiOwner, true);
            
            request.Request.PaymentInformation.PaymentStatus = StartPaymentProcess(request);
            try {
                if (request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                    Assert.Fail("PaymentStatus:" + PaymentStatus.FailedPending.ToString());
                }

                request.Request.PaymentInformation.PaymentStatus = SavePaymentInformation(request);
                if (request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                    UpdatePaymentProcessStatus(request);
                    Assert.Fail("PaymentStatus:" + PaymentStatus.FailedPending.ToString());
                }

                PrepareBulkSuccessResponse(request);
                mockedApiMethodCaller.Setup(x => x.FinalizePaymentProcess(It.IsAny<FinalizePaymentProcessMessage>())).Returns(PaymentStatus.Success);                
                FinalizePaymentProcessMessage finalizePaymentProcessMessage = new FinalizePaymentProcessMessage {
                    Response = (ResponseBase)request.Response
                };

                request.Request.PaymentInformation.PaymentStatus = mockedApiMethodCaller.Object.FinalizePaymentProcess(finalizePaymentProcessMessage);
                if (request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                    Assert.Fail("PaymentStatus:" + PaymentStatus.FailedPending.ToString());
                }

                UpdatePaymentProcessStatus(request);
                UpdateBankResponseMsg(request);
            } catch (Exception ex) {
                Assert.Fail(ex.Message);
            }

            Assert.AreEqual(request.Request.PaymentInformation.PaymentStatus, PaymentStatus.Success);
        }    

        [TestMethod]
        public void FailedPaymentProcessingTest() {
            Setup();
            BankRequest request = Common.GetBankRequest(apiOwner, false);
            request.Request.PaymentInformation.PaymentStatus = StartPaymentProcess(request);
            try {
                if (request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                    Assert.Fail("PaymentStatus:" + PaymentStatus.FailedPending.ToString());
                }

                request.Request.PaymentInformation.PaymentStatus = SavePaymentInformation(request);
                if (request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {                    
                    UpdatePaymentProcessStatus(request);
                    Assert.Fail("PaymentStatus:" + PaymentStatus.FailedPending.ToString());
                }

                PrepareBulkErrorResponse(request);
                mockedApiMethodCaller.Setup(x => x.FinalizePaymentProcess(It.IsAny<FinalizePaymentProcessMessage>())).Returns(PaymentStatus.Fail);
                request.Request.PaymentInformation.PaymentStatus = FinalizePaymentProcess(request);
                
                if (request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                    Assert.Fail("PaymentStatus:" + PaymentStatus.FailedPending.ToString());
                }

                UpdatePaymentProcessStatus(request);
                UpdateBankResponseMsg(request);
            } catch (Exception ex) {
                Assert.Fail(ex.Message);
            }

            Assert.AreEqual(request.Request.PaymentInformation.PaymentStatus, PaymentStatus.Fail);
        }

        /*-------------------------------------------------------------- Private Methods --------------------------------------------------------------*/

        private void UpdateBankResponseMsg(BankRequest request) {
            UpdateBankResponseMessage updateBankResponseMessage = new UpdateBankResponseMessage {
                PaymentId = request.Response.MerchantPaymentId,
                BankResponse = request.Response.ResponseMsg
            };
            apiMethodCaller.UpdateBankResponse(updateBankResponseMessage);
        }

        private PaymentStatus StartPaymentProcess(BankRequest request) {
            StartPaymentProcessMessage methodRequest = new StartPaymentProcessMessage {
                PaymentInformation = request.Request.PaymentInformation,
                Customer = request.Request.Customer,
                ApiOwner = (int)request.ApiOwner,
                ActionType = request.ActionType,
                Request = (RequestBase)request.Request
            };

            PaymentStatus paymentStatus = apiMethodCaller.StartPaymentProcess(methodRequest);
            return paymentStatus;
        }

        private static void PrepareBulkErrorResponse(BankRequest request) {
            request.Response.ResponseCode = PaymentResponseType.Exception;
            request.Response.ResponseMsg = "Failed Test";
            request.Response.MerchantPaymentId = request.Request.PaymentInformation.PaymentId;
        }

        private static void PrepareBulkSuccessResponse(BankRequest request) {
            request.Response.ResponseCode = PaymentResponseType.Approved;
            request.Response.ResponseMsg = "Success Test";
            request.Response.MerchantPaymentId = request.Request.PaymentInformation.PaymentId;
        }

        private PaymentStatus FinalizePaymentProcess(BankRequest request) {
            PaymentStatus paymentStatus;            
            FinalizePaymentProcessMessage finalizePaymentProcessMessage = new FinalizePaymentProcessMessage {
                Response = (ResponseBase)request.Response
            };
           
            paymentStatus = apiMethodCaller.FinalizePaymentProcess(finalizePaymentProcessMessage);
            return paymentStatus;
        }

        private void UpdatePaymentProcessStatus(BankRequest request) {
            UpdatePaymentProcessStatusMessage updatePaymentProcessStatusMessage = new UpdatePaymentProcessStatusMessage {
                PaymentId = request.Request.PaymentInformation.PaymentId,
                PaymentStatus = (short)request.Request.PaymentInformation.PaymentStatus
            };
           
            apiMethodCaller.UpdatePaymentProcessStatus(updatePaymentProcessStatusMessage);
        }

        private PaymentStatus SavePaymentInformation(BankRequest request) {
            PaymentStatus paymentStatus;
            SavePaymentInformationMessage savePaymentInformationMessage = new SavePaymentInformationMessage {
                ActionType = request.ActionType,
                PaymentInformation = request.Request.PaymentInformation
            };

            paymentStatus = apiMethodCaller.SavePaymentInformation(savePaymentInformationMessage);
            return paymentStatus;
        }
    }
}
