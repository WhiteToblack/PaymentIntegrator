using Integrator.Entity.ExternalEntity.Altinbas;
using Integrator.Models;
using PaymentIntegrator.UI.Models;
using PaymentManagement.GeneralOperation;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegrator.UI.Controllers.ExternalController {
    [Route("AltinbasEntry/{action}/{association?}/{source?}/{transactionMessageId?}")]
    public class AltinbasEntryController : Controller {
        // GET: AltinbasFreePayment
        public ActionResult PaymentExternal() {
            Session[CommonValues.URL_REFERRER_CONST] = Request.UrlReferrer;
            Session["RequestVerificationToken"] = Guid.NewGuid();


            //RequestVerificationToken çağıran clientdan alınırsa daha sağlıklı olacaktır..
            Request.Headers.Add("RequestVerificationToken", Session["RequestVerificationToken"].ToString());

            string association = Request.RequestContext.RouteData.Values["association"].ToString();
            string source = Request.RequestContext.RouteData.Values["source"].ToString();
            string transactionMessageId = Request.RequestContext.RouteData.Values["transactionMessageId"].ToString();

            AltinbasPaymentInputEntity entity = new AltinbasPaymentInputEntity {
                TransactionId = transactionMessageId
            };

            AltinbasPaymentEntity paymentEntity = new ExternalApiMethodCaller().SelectCreditCardPayment(entity);

            var payingUser = PrepareUser(paymentEntity);

            Session["PayingUser"] = payingUser;
            return RedirectToAction("Index", "Payment");
        }

        private static PayingUser PrepareUser(AltinbasPaymentEntity paymentEntity) {
            List<AmountInformation> amountInformations = new List<AmountInformation> {
                new AmountInformation {
                    TotalAmount = paymentEntity.PaymentAmount,
                    UseInstallment = paymentEntity.PaymentElement == "4" ? false : true,
                    CurrencyCode = paymentEntity.PaymentCurrencyCode,
                    InstallmentAmount = 0,
                    IsSelected = true,
                    Id =1,
                    DisplayText = paymentEntity.PaymentElementName
                }
            };

            PayingUser payingUser = new PayingUser {
                User = new User {
                    Name = paymentEntity.Name,
                    Surname = paymentEntity.Surname,
                    Division = paymentEntity.AssociationCode,
                    Id = Convert.ToInt64(paymentEntity.StudentNumber)
                },
                //Save işleminde servise geri dönmesi istenen extra değerler, dynamic obje alıp JSON döner
                //Extras = new object(),
                Extras = paymentEntity.TransactionId,
                PaymentInformation = new PaymentInformation {
                    AmountInformation = amountInformations
                }
            };

            return payingUser;
        }
    }
}