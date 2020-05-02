using Integrator.Models;
namespace PaymentManagement.Models.PaymentModels.Response
{
    public interface IResponseBase
    {
        ActionType Action { get; set; }
        PaymentResponseType ResponseCode { get; set; }
        string ResponseMsg { get; set; }
        decimal FinalAmount { get; set; }
        string Redirect3dResponse { get; set; }
        string MerchantPaymentId { get; set; }
        string SessionToken { get; set; }
    }
}
