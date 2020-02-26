using Microsoft.AspNetCore.Mvc;
using PaymentManagement.Models.PaymentModels;
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
            payingUser.PaymentInformation.PaymentStatus = PaymentStatus.Success;
            return View(payingUser);
        }
    }
}
