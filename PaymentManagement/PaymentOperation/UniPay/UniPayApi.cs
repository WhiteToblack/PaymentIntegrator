using Integrator.Models;
using log4net;
using Newtonsoft.Json;
using PaymentManagement.Log;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.Response;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace PaymentManagement.PaymentOperation {
    public class UniPayApi : IApiOwner {
        private static readonly ILog logger = LogManager.GetLogger("PaymentManager");

        RequestManager requestManager = null;
        public UniPayApi() {
            requestManager = new RequestManager(PaymentApiOwner.UniSoft);
        }

        public IResponseBase PaymentRequest<T>(IBankRequest request) where T : ResponseBase, IResponseBase {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), request);

            request.ActionType = ActionType.SALE;
            IResponseBase response = requestManager.MakeBankRequest<ResponseBase>((UniPayRequest)request);
            if(response.ResponseCode == PaymentResponseType.Approved) {
                return (SuccessResponse)response;
            }

            if(response.ResponseCode == PaymentResponseType.Waiting) {
                return (ResponseBase)response;
            }

            return (FailedResponse)response;
        }

        public IBankRequest PrepareRequest(IRequestBase request) {
            UniPayRequest uniPayRequest = new UniPayRequest {
                ApiOwner = PaymentApiOwner.UniSoft,
                Request = request
            };

            return uniPayRequest;
        }

        public int GetInstallementCount() {
            UniPayRequest bankRequest = new UniPayRequest() {
                ActionType = ActionType.QUERYMAXINSTALLMENTCOUNTS
            };

            IResponseBase response = requestManager.MakeBankRequest<InstallmentResponse>(bankRequest);
            return ((InstallmentResponse)response).MaxAllowedInstallmentCount;
        }

        public SessionResponseBase CreateSession(IBankRequest bankRequest) {
            bankRequest.ActionType = ActionType.SESSIONTOKEN;
            if(string.IsNullOrWhiteSpace(bankRequest.ReturnUrl)) {
                ///TODO: Buradaki URL tabloda parametreye alınmalı.
                bankRequest.ReturnUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/SanalPos.Integrator.UI/Payment/On3DCompleted";
                //bankRequest.ReturnUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/PaymentIntegrator.UI/Payment/On3DCompleted";
            }

            IResponseBase response = requestManager.MakeBankRequest<SessionResponseBase>((UniPayRequest)bankRequest);
            return (SessionResponseBase)response;
        }

        public IResponseBase QuerySession(string sessionId) {
            UniPayRequest bankRequest = new UniPayRequest {
                ActionType = ActionType.QUERYSESSION,
                SessionToken = sessionId
            };

            IResponseBase response = requestManager.MakeBankRequest<SessionResponseBase>(bankRequest);
            if(response.ResponseCode == PaymentResponseType.Declined) {
                FailedResponse failedResponse = ((FailedResponse)response);
                throw new Exception(string.Concat("Session couldnt created: \n" + JsonConvert.SerializeObject(failedResponse)));
            }

            return (SessionResponseBase)response;
        }

        public IResponseBase QueryPayment(string sessionId) {
            UniPayRequest bankRequest = new UniPayRequest {
                ActionType = ActionType.QUERYPAYMENT,
                SessionToken = sessionId
            };

            IResponseBase response = requestManager.MakeBankRequest<SessionResponseBase>(bankRequest);
            return response;
        }
    }
}
