using Integrator.Models;
using PaymentManagement.RequestOperation.Message;
using System.Collections.Generic;

namespace PaymentManagement.RequestOperation {
    public interface IApiMethodCaller {
        Dictionary<string, string> GetConfigurations();
        List<PaymentInformation> GetPendingPayments();

        PaymentStatus StartPaymentProcess(StartPaymentProcessMessage request);

        PaymentStatus SavePaymentInformation(SavePaymentInformationMessage request);

        bool UpdatePaymentProcessStatus(UpdatePaymentProcessStatusMessage request);

        PaymentStatus FinalizePaymentProcess(FinalizePaymentProcessMessage request);

        bool UpdateBankResponse(UpdateBankResponseMessage request);
    }
}