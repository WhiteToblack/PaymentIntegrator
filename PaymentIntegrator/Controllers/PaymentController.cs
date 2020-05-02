using Integrator.Models;
using PaymentManagement.GeneralOperation.UniPay;
using PaymentManagement.GeneralOperation;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using PaymentManagement.PaymentOperation.Response;
using PaymentManagement.RequestOperation.Message;
using PaymentIntegrator.UI.Models;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using PaymentManagement.Log;

namespace PaymentIntegrator.UI.Controllers {
    [Route("Payment/{action}")]
    [ValidateInput(true)]
    public class PaymentController : Controller {
        private static readonly ILog logger = LogManager.GetLogger("PaymentIntegrator");

        //protected override void OnActionExecuting(ActionExecutingContext filterContext) {
        //    string antiForgeryToken = Session["RequestVerificationToken"].ToString();
        //    if (filterContext.HttpContext.Request.Headers["RequestVerificationToken"] == null) {
        //        throw new EntryPointNotFoundException();
        //    }

        //    if (filterContext.HttpContext.Request.Headers["RequestVerificationToken"].ToString() != antiForgeryToken) {
        //        Response.RedirectToRoute("Entry/Security/UnsecureAccess");
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

        ApiMethodCaller apiMethodCaller = null;
        ApiOwnerProvider apiOwnerProvider = null;
        IApiOwner apiOwner;
        public PaymentController() {
            //UI a göre karar verilebilir
            apiOwnerProvider = new ApiOwnerProvider(PaymentApiOwner.UniSoft);
            apiMethodCaller = new ApiMethodCaller();
            apiOwner = apiOwnerProvider.GetApiOwner();
        }

        [HttpGet]
        public ActionResult Index() {
            PayingUser payingUser = (PayingUser)Session["PayingUser"];
            payingUser.PaymentInformation.MaxInstallmentCount = apiOwner.GetInstallementCount();
            ViewBag.AmountListStr = JsonConvert.SerializeObject(payingUser.PaymentInformation.AmountInformation);
            return View(payingUser);
        }

        [HttpPost]
        [Route("Payment/AmountChanged")]
        public ActionResult AmountChanged(AmountInformation amountInformation) {
            PayingUser payingUser = (PayingUser)Session["PayingUser"];
            payingUser.PaymentInformation.AmountInformation.ForEach(x => x.IsSelected = false);
            payingUser.PaymentInformation.AmountInformation.Where(x => x.Id == amountInformation.Id).FirstOrDefault().IsSelected = true;
            return View("Index", payingUser);
        }

        [HttpPost]
        [Route("Payment/On3DCompleted")]
        public ActionResult On3DCompleted() {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), Request.Form);
            string serializedForm = JsonConvert.SerializeObject(ToDictionary(Request.Form));
            IResponseBase response = JsonConvert.DeserializeObject<ResponseBase>(serializedForm);

            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), response);
            UpdateBankResponse(serializedForm, response);

            PaymentStatus paymentStatus;
            if(response.ResponseCode == PaymentResponseType.Approved) {
                response = JsonConvert.DeserializeObject<SuccessResponse>(serializedForm);
            } else {
                response = JsonConvert.DeserializeObject<FailedResponse>(serializedForm);
            }

            paymentStatus = FinalizePaymentProcess(response);
            if(paymentStatus == PaymentStatus.FailedPending || paymentStatus == PaymentStatus.Fail) {
                UpdatePaymentProcessStatus(response);
            }

            string returnUrl, returnQueryString;
            CalculateReturnUrl(response, paymentStatus, out returnUrl, out returnQueryString);
            return Redirect(returnUrl + returnQueryString);
        }

        private void CalculateReturnUrl(IResponseBase response, PaymentStatus paymentStatus, out string returnUrl, out string returnQueryString) {
            RequestExtras extras = GetExtras(response);
            returnUrl = extras.ReferrerUrl;
            string errMsg = string.Empty;
            if(response.GetType() == typeof(FailedResponse)) {
                errMsg = string.IsNullOrWhiteSpace(((FailedResponse)response).ErrorMsg) ? response.ResponseMsg : ((FailedResponse)response).ErrorMsg;
            }

            returnQueryString = string.Format("?{0}={1}&{2}={3}", "paymentStatus", paymentStatus.ToString(), "errorMsg", errMsg);
        }

        private RequestExtras GetExtras(IResponseBase response) {
            SessionResponseBase sessionResponse = (SessionResponseBase)apiOwner.QuerySession(response.SessionToken);
            RequestExtras extras = JsonConvert.DeserializeObject<RequestExtras>(sessionResponse.Session.Extra);
            return extras;
        }

        [HttpPost]
        public ActionResult SendRequestToBank(PayingUser payingUser) {
            ((PayingUser)Session["PayingUser"]).PaymentInformation.AmountInformation.ForEach(x => x.IsSelected = false);
            ((PayingUser)Session["PayingUser"]).PaymentInformation.AmountInformation
                .Where(x => x.Id == payingUser.PaymentInformation.SelectedAmountId).FirstOrDefault().IsSelected = true;

            payingUser.PaymentInformation.AmountInformation = ((PayingUser)Session["PayingUser"]).PaymentInformation.AmountInformation;
            payingUser.Extras = ((PayingUser)Session["PayingUser"]).Extras;

            PrepareDefaultPayment(payingUser);
            BankRequest request = PrepareBankRequest(payingUser);

            request.Request.PaymentInformation.PaymentStatus = StartPaymentProcess(request);
            if(request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                ResponseBase exResponse = GetExceptionResponse(request.ActionType);
                return View("OnFailView", exResponse);
            }

            PrepareSession(request);
            request.Request.PaymentInformation.PaymentStatus = SavePaymentProcessInformation(request);
            if(request.Request.PaymentInformation.PaymentStatus == PaymentStatus.FailedPending) {
                ResponseBase exResponse = GetExceptionResponse(request.ActionType);
                return View("OnFailView", exResponse);
            }

           
            if(payingUser.PaymentInformation.Use3DPayment) {
                Prepare3DRequest(payingUser, request);
            }

           
            Session["PayingUser"] = payingUser;
            return CalculateResponseView(payingUser, request);
        }

        private void PrepareSession(BankRequest request) {
            string returnUrl = Request.Url.GetLeftPart(UriPartial.Authority) + "/" + Request.Url.Segments[1] + "/" + Request.Url.Segments[2] + "/On3DCompleted";
            request.ReturnUrl = returnUrl;
            request.Request.Extra = GetExtrasForRequest(request);
            SessionResponseBase sessionResponse = apiOwner.CreateSession(request);
            request.SessionToken = sessionResponse.SessionToken;
            request.Request.PaymentInformation.SessionToken = sessionResponse.SessionToken;
        }

        private void Prepare3DRequest(PayingUser payingUser, BankRequest request) {         
            request.Is3DUsed = payingUser.PaymentInformation.Use3DPayment;
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), request);
        }

        private string GetExtrasForRequest(BankRequest bankRequest) {

            RequestExtras extras = new RequestExtras {
                ExternalRefNumber = bankRequest.Request.Extra
            };

            if(Session[CommonValues.URL_REFERRER_CONST] == null) {
                return string.Empty;
            } else {
                extras.ReferrerUrl = Session[CommonValues.URL_REFERRER_CONST].ToString();
            }
            return JsonConvert.SerializeObject(extras);
        }
        private ActionResult CalculateResponseView(PayingUser payingUser, BankRequest request) {
            IResponseBase response = apiOwner.PaymentRequest<SuccessResponse>(request);
            if(!request.Is3DUsed) {
                string returnUrl = Session[CommonValues.URL_REFERRER_CONST].ToString();
                StringBuilder queryStringSb = new StringBuilder();
                queryStringSb.Append("?");
                PaymentStatus paymentStatus = PaymentStatus.Fail;
                if(response.ResponseCode == PaymentResponseType.Declined) {
                    FailedResponse failedResponse = (FailedResponse)response;
                    queryStringSb.AppendFormat("{0}={1}", "paymentStatus", paymentStatus.ToString());
                    queryStringSb.AppendFormat("&{0}={1}", "errorCode", failedResponse.ErrorCode);
                    queryStringSb.AppendFormat("&{0}={1}", "errorMessage", failedResponse.ErrorMsg);
                    return Redirect(returnUrl + queryStringSb.ToString());
                }

                if(response.ResponseCode == PaymentResponseType.Approved) {
                    paymentStatus = PaymentStatus.Success;
                }

                queryStringSb.AppendFormat("{0}={1}", "paymentStatus", paymentStatus.ToString());
                return Redirect(returnUrl + queryStringSb.ToString());
            }

            if(response.ResponseCode == PaymentResponseType.Waiting) {
                Response.Write(response.Redirect3dResponse);
                return View("_3DSecureView");
            }

            Response.Write("3d Secure yönlendirmesinde hata oluştu.");
            return View("_3DSecureView");
        }

        private BankRequest PrepareBankRequest(PayingUser payingUser) {
            SetPaymentId();
            payingUser.PaymentInformation.PaymentId = Session[CommonValues.PAYMENT_SESSION_ID].ToString();
            payingUser.PaymentInformation.PaymentStartDate = DateTime.Now;
            RequestBase requestBase = new RequestBase {
                PaymentInformation = payingUser.PaymentInformation,
                CardInformation = payingUser.CardInformation,
                Customer = payingUser.User,
                Extra = JsonConvert.SerializeObject(payingUser.Extras)
            };

            BankRequest request = (BankRequest)apiOwner.PrepareRequest(requestBase);
            return request;
        }

        private static ResponseBase GetExceptionResponse(ActionType actionType) {
            ResponseBase response = new ResponseBase {
                Action = actionType,
                ResponseCode = PaymentResponseType.Exception,
                ResponseMsg = "Ödeme işleminde bir sorun oluştu, lütfen tekrar deneyiniz",
                FinalAmount = 0
            };

            return response;
        }

        private void SetPaymentId() {
            Session[CommonValues.PAYMENT_SESSION_ID] = GuidMaker.GetGuid();
        }

        private static void PrepareDefaultPayment(PayingUser payingUser) {
            payingUser.PaymentInformation.Use3DPayment = true;
            payingUser.PaymentInformation.PaymentStatus = PaymentStatus.Pending;
            payingUser.CardInformation.CardNumber = payingUser.CardInformation.CardNumber;
            payingUser.CardInformation.Cvv = payingUser.CardInformation.Cvv;
            payingUser.CardInformation.ExpiryDate = payingUser.CardInformation.ExpiryDate;
            //güvenlik amaçlı
            if(payingUser.PaymentInformation.AmountInformation.Where(x => x.IsSelected).FirstOrDefault().UseInstallment == false) {
                payingUser.PaymentInformation.InstallmentCount = 1;
            }
        }

        public static IDictionary<string, string> ToDictionary(NameValueCollection col) {
            var dict = new Dictionary<string, string>();

            foreach(string key in col.Keys) {
                dict.Add(key, col[key]);
            }

            return dict;
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

        private PaymentStatus SavePaymentProcessInformation(BankRequest request) {
            PaymentStatus paymentStatus;
            SavePaymentInformationMessage savePaymentInformationMessage = new SavePaymentInformationMessage {
                ActionType = request.ActionType,
                PaymentInformation = request.Request.PaymentInformation
            };
            paymentStatus = apiMethodCaller.SavePaymentInformation(savePaymentInformationMessage);
            return paymentStatus;
        }

        private PaymentStatus FinalizePaymentProcess(IResponseBase response) {
            PaymentStatus paymentStatus;
            FinalizePaymentProcessMessage finalizePaymentProcessMessage = new FinalizePaymentProcessMessage();
            finalizePaymentProcessMessage.Response = (ResponseBase)response;
            paymentStatus = apiMethodCaller.FinalizePaymentProcess(finalizePaymentProcessMessage);
            return paymentStatus;
        }

        private void UpdatePaymentProcessStatus(IResponseBase response) {
            UpdatePaymentProcessStatusMessage updatePaymentProcessStatusMessage = new UpdatePaymentProcessStatusMessage {
                PaymentId = response.MerchantPaymentId,
                PaymentStatus = (short)PaymentStatus.FailedPending
            };
            apiMethodCaller.UpdatePaymentProcessStatus(updatePaymentProcessStatusMessage);
        }

        private void UpdateBankResponse(string serializedForm, IResponseBase response) {
            UpdateBankResponseMessage updateBankResponseMessage = new UpdateBankResponseMessage {
                PaymentId = response.MerchantPaymentId,
                BankResponse = serializedForm
            };

            apiMethodCaller.UpdateBankResponse(updateBankResponseMessage);
        }
    }
}