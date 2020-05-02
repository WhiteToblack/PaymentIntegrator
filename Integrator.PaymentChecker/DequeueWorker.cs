using Integrator.Models;
using Integrator.PaymentChecker.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Response;
using PaymentManagement.RequestOperation;
using PaymentManagement.RequestOperation.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Integrator.PaymentChecker {
    public class DequeueWorker : BaseQueueWorker {
        public DequeueWorker(ILogger<BaseQueueWorker> logger) : base(logger) {
        }

        protected override async Task InternalExecuteAsync(CancellationToken stoppingToken) {
            while(!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Dequeue Worker running at: {time}", DateTimeOffset.Now);
                PaymentInformation paymentInformation = pendingQueue.Dequeue();
                if(paymentInformation == null) {
                    _logger.LogError("Kayıt yok");
                    continue;
                }

                string sessionToken = paymentInformation.SessionToken;
                IResponseBase response = apiOwner.QueryPayment(sessionToken);

                if(response.ResponseCode == PaymentResponseType.Declined) {
                    FailedResponse failedResponse = (FailedResponse)response;
                    _logger.LogError("Bankaya sorgu atılamadı: " + paymentInformation.PaymentId + " ===>" + response.ResponseCode + " - " + failedResponse.ErrorMsg + "\\n");

                    continue;
                }

                QueryPaymentResponse queryPaymentResponse = (QueryPaymentResponse)response;
                _logger.LogWarning(sessionToken + "\\n" + JsonConvert.SerializeObject(response) + "\\n");

                foreach(var payment in queryPaymentResponse.Payments) {
                    response.FinalAmount += Convert.ToDecimal(payment.PaidAmount);
                }

                PaymentStatus paymentStatus = paymentInformation.TotalAmount == response.FinalAmount ? PaymentStatus.Success : PaymentStatus.Fail;
                UpdatePaymentProcessStatusMessage updatePaymentProcessStatusMessage = new UpdatePaymentProcessStatusMessage {
                    PaymentId = paymentInformation.PaymentId,
                    PaymentStatus = (short)paymentStatus,
                    ActionType = ActionType.QUERYPAYMENT
                };

                apiMethodCaller.UpdatePaymentProcessStatus(updatePaymentProcessStatusMessage);
                _logger.LogInformation(paymentInformation.PaymentId + " için statü " + paymentStatus + "'e çekildi");
                await Task.Delay(1 * (60 * 1000));
            }
        }
    }
}
