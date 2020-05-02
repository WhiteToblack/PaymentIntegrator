using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentManagement.CollegePaymentEntity
{
    public class StudentEntity : PersonEntity
    {
        public string StudentNumber;
        public string DepartmentCode;
        public string DepartmentName;
        public string EducationPeriod;
        public string ParentName;
        public string ParentSurname;
        public string ParentIdentityNumber;
        public string EntranceYear;
        public string IsForeign;
        public string MaliOnay;
        //public ContactEntity Contact;
    }
}