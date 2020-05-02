using Integrator.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Integrator.PaymentChecker.Queues {
    public class QueueBase : IQueueBase {

        private static readonly Lazy<ConcurrentQueue<PaymentInformation>> instance =
                        new Lazy<ConcurrentQueue<PaymentInformation>>(() => Activator.CreateInstance(typeof(ConcurrentQueue<PaymentInformation>), true) as ConcurrentQueue<PaymentInformation>);

        public static ConcurrentQueue<PaymentInformation> QueueInstance {
            get {
                return instance.Value;
            }
        }

        public PaymentInformation Dequeue() {
            if(QueueInstance == null) {
                return null;
            }

            PaymentInformation paymentInformation = QueueInstance.FirstOrDefault();
            if(paymentInformation == null) {
                return null;
            }

            QueueInstance.TryDequeue(out paymentInformation);
            return paymentInformation;
        }

        public void Enqueue(PaymentInformation paymentInformation) {
            QueueInstance.Enqueue(paymentInformation);
        }

        public ConcurrentQueue<PaymentInformation> GetItems() {
            if(QueueInstance == null) {
                return null;
            }

            return QueueInstance;
        }

        public void SetItems(ConcurrentQueue<PaymentInformation> queueItems) {
            foreach(var queueItem in queueItems) {
                QueueInstance.Enqueue(queueItem);
            }
        }
    }
}
