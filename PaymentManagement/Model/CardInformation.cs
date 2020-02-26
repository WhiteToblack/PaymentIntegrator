using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.Models.PaymentModels
{
    public class CardInformation
    {
        public string CardNumber { get; set; }
        public short Cvc { get; set; }
        public string HolderName { get; set; }
        public string ExpiryDate { get; set; }
    }
}
