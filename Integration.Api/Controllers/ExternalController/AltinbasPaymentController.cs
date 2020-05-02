using Integration.Api.CollegePaymentSystemService;
using Integrator.Entity.ExternalEntity.Altinbas;
using Integrator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Integration.Api.Controllers {
    [Route("api/AltinbasPayment/{action}")]
    public class AltinbasPaymentController : ApiController {
        public CollegePaymentSystemSoapClient CPSService = null;
        public AltinbasPaymentController() {
            CPSService = new CollegePaymentSystemSoapClient();
        }

        [HttpPost]
        [ResponseType(typeof(AltinbasPaymentEntity))]
        public IHttpActionResult SelectCreditCardPayment(AltinbasPaymentInputEntity request) {

            CreditCardPaymentEntity entity = CPSService.SelectCreditCardPayment(request.TransactionId);

            var entityJson = JsonConvert.SerializeObject(entity);
            var paymentEntity = JsonConvert.DeserializeObject<AltinbasPaymentEntity>(entityJson);

            return Ok<AltinbasPaymentEntity>(paymentEntity);
        }


        [HttpPost]
        [ResponseType(typeof(AltinbasResponseEntity))]
        public AltinbasResponseEntity InsertCreditCardPayment(AltinbasPaymentInputEntity request) {

            ResponseEntity entity = CPSService.InsertCreditCardPayment(request.TransactionId, request.PaymentExternalRefNumber);

            var entityJson = JsonConvert.SerializeObject(entity);
            var paymentEntity = JsonConvert.DeserializeObject<AltinbasResponseEntity>(entityJson);

            return (paymentEntity);
        }

    }
}
