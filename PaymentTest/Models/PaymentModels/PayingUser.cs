using PaymentManagement.Models.PaymentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.Models.PaymentModels
{
    public class PayingUser
    {
        public User User { get; set; }
        public PaymentInformation PaymentInformation { get; set; }
        public CardInformation CardInformation { get; set; }
    }
}
