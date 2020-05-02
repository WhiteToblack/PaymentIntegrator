using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.RequestOperation.Message.CollegePayment
{
    public class GetStudentMessage
    {
        public string AssociationCode { get; set; }
        public string StudentNumber{ get; set; }
    }
}