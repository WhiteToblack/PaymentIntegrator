using Integrator.Models;
using Microsoft.Security.Application;
using PaymentIntegrator.UI.Models;
using PaymentManagement.GeneralOperation;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentIntegrator.UI.Controllers {
    [Route("Entry/{action}/{associationCode?}/{userId?}")]
    public class EntryController : Controller {
        ///TODO: Route düzenlenip, gerekli kurum detaylarıda eklenecek.
        public ActionResult Payment() {
            Session[CommonValues.URL_REFERRER_CONST] = Request.UrlReferrer;
            Session["RequestVerificationToken"] = Guid.NewGuid();
            //RequestVerificationToken çağıran clientdan alınırsa daha sağlıklı olacaktır..
            Request.Headers.Add("RequestVerificationToken", Session["RequestVerificationToken"].ToString());

            if(Request.RequestContext.RouteData.Values["userId"] == null) {
                return View("UnsecureAccessView");
            }

            if(Request.RequestContext.RouteData.Values["associationCode"] == null) {
                return View("UnsecureAccessView");
            }

            long userId = Convert.ToInt64(Request.RequestContext.RouteData.Values["userId"]);
            ///TODO: Servisten user ve amount bilgileri getirilip, PayingUser doldurulacak. Sonrasında web.config den environment değeri Development dısında bir değer set edilmeli.
            //GetStudentMessage getStudentMessage = new GetStudentMessage {
            //    AssociationCode = Request.RequestContext.RouteData.Values["associationCode"].ToString(),
            //    StudentNumber = Request.RequestContext.RouteData.Values["userId"].ToString()
            //};

            //StudentEntity studentEntity = new ExternalApiMethodCaller().GetStudent(getStudentMessage);
            PayingUser payingUser = new PayingUser() { };// {
            //    User = new User {
            //        Name = studentEntity.Name,
            //        Surname = studentEntity.Surname,
            //        Division = studentEntity.DepartmentCode,
            //        Id = Convert.ToInt64(studentEntity.IdentityNumber)
            //    }
            //};

            if(ConfigurationManager.AppSettings["Environment"] != null && ConfigurationManager.AppSettings["Environment"].ToString() == "Development") {
                payingUser = PrepareTestUser(userId);
            }

            Session["PayingUser"] = payingUser;
            //PayingUser payingUser = PrepareUserFromApi();
            return RedirectToAction("Index", "Payment");
        }

        public ActionResult UnsecureAccess() {
            return View("UnsecureAccessView");
        }

        public class TestExtras {
            public string TestObject { get; set; }
        }

        private static PayingUser PrepareTestUser(long userId) {
            //Kullanıcıya ilk gösterilecek amount un selected true olması gerek. Sadece 1 tane olması gerektiğinden .firstOrDefault ile alınıyor.
            List<AmountInformation> amountInformations = new List<AmountInformation> {
                new AmountInformation {
                    TotalAmount = 100,
                    UseInstallment = true,
                    CurrencyCode = "TRY",
                    InstallmentAmount = 100,
                    IsSelected = false,
                    Id=2,
                    DisplayText = "Taksit ile öde"
                },
                new AmountInformation {
                    TotalAmount = 200,
                    UseInstallment= false,
                    CurrencyCode = "TRY",
                    InstallmentAmount = 100,
                    IsSelected = true,
                    Id=1,
                    DisplayText = "Nakit"
                }
            };

            PayingUser payingUser = new PayingUser {
                User = new User {
                    Id = userId,
                    Name = "Aragorn",
                    Surname = "II Elessar Telcontar",
                    Division = "Gondor"
                },
                //Save işleminde servise geri dönmesi istenen extra değerler, dynamic obje alıp JSON döner
                Extras = "123",
                PaymentInformation = new PaymentInformation {
                    AmountInformation = amountInformations
                }
            };

            return payingUser;
        }
    }
}