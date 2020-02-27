using Microsoft.AspNetCore.Mvc;
using PaymentManagement.DbOperation;
using PaymentManagement.Models.PaymentModels;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using PaymentTest.Models.PaymentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.Controllers
{
    [Route("CardPayment")]
    public class PaymentController : Controller
    {
       
        [HttpPost]       
        public IActionResult Index(PayingUser payingUser) {           
            return View(payingUser);
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        [Route("SendRequestToBank")]
        public IActionResult SendRequestToBank(PayingUser payingUser) {
            PrepareDefaultPayment(payingUser);
            PaymentStatus Response = PaymentStatus.Success;

            //RequestManager requestManager = new RequestManager();
            //BankRequest request = new BankRequest {
            //    ActionType = ActionType.SALE,
            //    ApiOwner = PaymentApiOwner.UniSoft,
            //    Request = new UniPayRequest {
            //        PaymentInformation = new PaymentInformation(),
            //        CardInformation = new CardInformation(),
            //        Customer = new User(),
            //        Action = ActionType.SALE
            //    }
            //};
            //requestManager.MakeBankRequest<SuccessResponse>(request);


            if (Response == PaymentStatus.Success) {
                return View("OnSuccessView", payingUser);
            }
            
            return View("OnFailView", payingUser);
        }

        private static void PrepareDefaultPayment(PayingUser payingUser) {
            payingUser.PaymentInformation.PaymentId = "000-111";
            payingUser.PaymentInformation.PaymentStatus = PaymentStatus.Pending;
        }

    }
}
