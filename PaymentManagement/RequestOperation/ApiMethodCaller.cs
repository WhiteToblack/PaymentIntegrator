using Integrator.Models;
using PaymentManagement.PaymentOperation;
using PaymentManagement.RequestOperation.Message;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PaymentManagement.RequestOperation
{
    public class ApiMethodCaller : ApiCaller, IApiMethodCaller {
        public ApiMethodCaller() : base() {
        }

        public Dictionary<string, string> GetConfigurations() {
            return Task<Dictionary<string, string>>.Run(() => Call<Dictionary<string, string>>("Configurations", "GetAllConfigurations", null)).Result;
        }

        public List<PaymentInformation> GetPendingPayments() {
            return Task<List<PaymentInformation>>.Run(() => Call<List<PaymentInformation>>("PaymentProcess", "GetPendingPayments", null)).Result;
        }

        public PaymentStatus StartPaymentProcess(StartPaymentProcessMessage request) {                        
            return Task<PaymentStatus>.Run(() => Call<PaymentStatus>("PaymentProcess", "StartPaymentProcess", request)).Result;
        }

        public PaymentStatus SavePaymentInformation(SavePaymentInformationMessage request) {           
            return Task<PaymentStatus>.Run(() => Call<PaymentStatus>("PaymentProcess", "SavePaymentInformation", request)).Result;
        }

        public bool UpdatePaymentProcessStatus(UpdatePaymentProcessStatusMessage request) {        
            return Task<bool>.Run(() => Call<bool>("PaymentProcess", "UpdatePaymentProcessStatus", request)).Result;
        }

        public PaymentStatus FinalizePaymentProcess(FinalizePaymentProcessMessage request) {            
            return Task<PaymentStatus>.Run(() => Call<PaymentStatus>("PaymentProcess", "FinalizePaymentProcess", request)).Result;
        }

        public bool UpdateBankResponse(UpdateBankResponseMessage request) {        
            return Task<bool>.Run(() => Call<bool>("PaymentProcess", "UpdateBankResponse", request)).Result;
        }

        public bool SaveSessionId(SaveSessionIdMessage request) {
            return Task<bool>.Run(() => Call<bool>("PaymentProcess", "SaveSessionId", request)).Result;
        }

    }
}