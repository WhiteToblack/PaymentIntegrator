using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Integrator.Models;
using Integrator.PaymentChecker.Operation;
using Integrator.PaymentChecker.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Integrator.PaymentChecker {
    public class EnqueueWorker : BaseQueueWorker {


        public EnqueueWorker(ILogger<BaseQueueWorker> logger) : base(logger) {
        }

        protected override async Task InternalExecuteAsync(CancellationToken stoppingToken) {
            while(!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Enqueue Worker running at: {time}", DateTimeOffset.Now);
                List<PaymentInformation> pendingPayments = PaymentOperation.GetPendingPayments();

                ConcurrentQueue<PaymentInformation> concurrentQueue = new ConcurrentQueue<PaymentInformation>(pendingPayments);
                pendingQueue.SetItems(concurrentQueue);
                _logger.LogInformation(string.Format("Pending'de bekleyen iþlem sayýsý: {0}", pendingPayments.Count));
                await Task.Delay(30 * (60 * 1000), stoppingToken);
            }
        }
    }
}
