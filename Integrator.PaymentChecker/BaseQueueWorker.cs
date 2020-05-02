using Integrator.PaymentChecker.Queues;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentManagement.PaymentOperation;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Integrator.PaymentChecker {
    public abstract class BaseQueueWorker : BackgroundService {
        public readonly PendingQueue pendingQueue;
        public readonly ApiMethodCaller apiMethodCaller = null;
        public readonly ApiOwnerProvider apiOwnerProvider = null;
        public readonly IApiOwner apiOwner;
        public readonly ILogger<BaseQueueWorker> _logger;
        public BaseQueueWorker(ILogger<BaseQueueWorker> logger) {
            pendingQueue = new PendingQueue();
            _logger = logger;            
            apiOwnerProvider = new ApiOwnerProvider(PaymentApiOwner.UniSoft);
            apiMethodCaller = new ApiMethodCaller();
            apiOwner = apiOwnerProvider.GetApiOwner();
        }

        protected virtual async Task InternalExecuteAsync(CancellationToken stoppingToken) {
            while(!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Queue Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) {
            return InternalExecuteAsync(stoppingToken);
        }
    }
}
