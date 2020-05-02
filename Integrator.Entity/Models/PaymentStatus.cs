namespace Integrator.Models
{
    public enum PaymentStatus
    {
        Fail = 0,
        Success = 1,
        Pending = 2,
        FailedPending = 3 // if payment process will be failed for reason of code, take this state. it will may process after sometime with a batch work.
    }
}