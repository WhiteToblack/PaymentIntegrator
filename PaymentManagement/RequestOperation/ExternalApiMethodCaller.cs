using Integrator.Entity.ExternalEntity.Altinbas;
using Integrator.Models;
using PaymentManagement.PaymentOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PaymentManagement.RequestOperation
{
    public class ExternalApiMethodCaller : ApiCaller
    {
        public ExternalApiMethodCaller() : base() {
        }

        public AltinbasPaymentEntity SelectCreditCardPayment(AltinbasPaymentInputEntity request) {
            return Task<AltinbasPaymentEntity>.Run(() => Call<AltinbasPaymentEntity>("AltinbasPayment", "SelectCreditCardPayment", request)).Result;
        }


        public AltinbasResponseEntity InsertCreditCardPayment(AltinbasPaymentInputEntity request) {
            return Task<AltinbasResponseEntity>.Run(() => Call<AltinbasResponseEntity>("AltinbasPayment", "InsertCreditCardPayment", request)).Result;
        }

    }
}