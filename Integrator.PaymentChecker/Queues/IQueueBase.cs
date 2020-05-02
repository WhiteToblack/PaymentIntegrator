using Integrator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Integrator.PaymentChecker.Queues {
    public interface IQueueBase {
        ConcurrentQueue<PaymentInformation> GetItems();
        void SetItems(ConcurrentQueue<PaymentInformation> queueItems);
        void Enqueue(PaymentInformation paymentInformation);
        PaymentInformation Dequeue();
    }
}
