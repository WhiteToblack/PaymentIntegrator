using Integration.Api.CollegePaymentSystemService;
using PaymentManagement.RequestOperation.Message.CollegePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Integration.Api.Controllers
{
    [Route("api/CollegePayment/{action}")]
    public class CollegePaymentController : ApiController
    {
        public CollegePaymentSystemSoapClient CPSService = null;
        public CollegePaymentController() {
            CPSService = new CollegePaymentSystemSoapClient();            
        }

        [HttpPost]
        [ResponseType(typeof(StudentEntity))]
        public IHttpActionResult GetStudent(GetStudentMessage request) {
            ProcessHeaderEntity processHeaderEntity = new ProcessHeaderEntity {
                AssociationCode = request.AssociationCode
            };

            StudentEntity studentEntity = new StudentEntity {
                StudentNumber = request.StudentNumber
            };

            StudentEntity student = CPSService.SelectStudent(processHeaderEntity, studentEntity).ToList().FirstOrDefault();
            return Ok<StudentEntity>(student);
        }

    }
}
