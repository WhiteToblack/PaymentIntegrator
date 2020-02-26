using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.Models.PaymentModels
{
    public enum PaymentResponseType
    {
        //	Approved Action done successfully
        Approved = 00,
        //	 for Approval Action can not be done.It needs admin approval to be finished.
        Waiting = 01,
        //Action failed due to general error. A general error can be a runtime error during action processing or a payment gateway error that is not yet mapped in our error set.
        GeneralError = 98,
        // Declined Action failed due to invalid parameters or payment gateway error.
        Declined = 99
    }
}
