using Integrator.Api.DbOperation.Payment;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Integrator.Api.DbOperation;
using Newtonsoft.Json;
using System.Web.Http.Description;
using PaymentManagement.RequestOperation.Message;
using Integrator.Models;
using PaymentManagement.RequestOperation;
using Integration.Api.Controllers;
using Newtonsoft.Json.Linq;
using log4net;
using PaymentManagement.Log;
using System.Reflection;
using System.Web.Http.Results;

namespace Integrator.Api.Controllers {
    [Route("api/PaymentProcess/{action}")]
    public class PaymentProcessController : ApiController {
        private static readonly ILog logger = LogManager.GetLogger("IntegratorApi");

        PaymentProcessor paymentProcessor = null;
        PaymentProcessController() {
            paymentProcessor = new PaymentProcessor();
        }

        [HttpGet]
        public JsonResult<List<PaymentInformation>> GetPendingPayments() {
            List<PaymentInformation> paymentInformations = paymentProcessor.SelectPendingPayments();
            return Json(paymentInformations);
        }


        [HttpPost]
        [ResponseType(typeof(PaymentStatus))]
        public IHttpActionResult StartPaymentProcess(StartPaymentProcessMessage request) {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), request);

            PaymentStatus paymentStatus = paymentProcessor.StartPaymentProcess(request);

            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), paymentStatus);
            return Ok<PaymentStatus>(paymentStatus);
        }

        [HttpPost]
        [ResponseType(typeof(PaymentStatus))]
        public IHttpActionResult SavePaymentInformation(SavePaymentInformationMessage request) {
            PaymentStatus paymentStatus = paymentProcessor.SavePaymentInformation(request);
            return Ok<PaymentStatus>(paymentStatus);
        }

        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdatePaymentProcessStatus(UpdatePaymentProcessStatusMessage request) {
            bool res = paymentProcessor.UpdatePaymentProcessStatus(request);
            return Ok<bool>(res);
        }

        [HttpPost]
        [ResponseType(typeof(PaymentStatus))]
        public IHttpActionResult FinalizePaymentProcess(FinalizePaymentProcessMessage request) {
            PaymentStatus paymentStatus = paymentProcessor.FinalizePaymentProcess(request);
            try {
                var requestJson = paymentProcessor.SelectPaymentProcessRequestJson(request.Response.MerchantPaymentId);
                var data = (JObject)JsonConvert.DeserializeObject(requestJson);
                string extra = data["Extra"].Value<string>();

                LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), request, extra, paymentStatus);
                if(paymentStatus == PaymentStatus.Success) {

                    AltinbasPaymentInputEntity entity = new AltinbasPaymentInputEntity {
                        TransactionId = extra,
                        PaymentExternalRefNumber = request.Response.MerchantPaymentId
                    };
                    var paymentController = new AltinbasPaymentController();
                    AltinbasResponseEntity paymentEntity = paymentController.InsertCreditCardPayment(entity);

                }
            } catch(Exception) {
                return Ok<PaymentStatus>(PaymentStatus.FailedPending);
            }
            return Ok<PaymentStatus>(paymentStatus);
        }

        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UpdateBankResponse(UpdateBankResponseMessage request) {
            bool res = paymentProcessor.UpdateBankResponse(request);
            return Ok<bool>(res);
        }
    }
}
