using PaymentManagement.Cache;
using PaymentManagement.GeneralOperation;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using PaymentManagement.RequestOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public class ApiOwnerProvider : IApiOwnerProvider
    {

        PaymentApiOwner apiOwnerType;        
        public ApiOwnerProvider(PaymentApiOwner _apiOwner) {
            apiOwnerType = _apiOwner;                    
        }

        internal string GetApiUrl() {
            string apiUrl = ConfigurationCache.Instance.GetValue(((int)apiOwnerType).ToString() + AuthConfig.API_URL);
            return apiUrl;
        }

        // Provide all owners after all integrations OK
        public IApiOwner GetApiOwner() {
            switch (apiOwnerType) {
                case PaymentApiOwner.UniSoft:
                    return new UniPayApi();
                default:
                    return new UniPayApi();
            }
        }

        public IRequestBase GetRequestInstance(IRequestBase request) {
            switch (apiOwnerType) {
                case PaymentApiOwner.UniSoft:
                    return PrepareRequestInstance<RequestBase>(request);
                default:
                    return new Request.RequestBase();
            }
        }

        public Type GetRequestObjType() {
            switch (apiOwnerType) {
                case PaymentApiOwner.UniSoft:
                    return typeof(UniPayRequest);
                default:
                    return typeof(UniPayRequest);
            }
        }

        private T PrepareRequestInstance<T>(IRequestBase request) where T : RequestBase, new() {
            if (request == null) {
                request = new T();
            }
          
            FillAuthHeader(request);

            return (T)request;
        }

        private IRequestBase FillAuthHeader(IRequestBase request) {
            request.Merchant = ConfigurationCache.Instance.GetValue(((int)apiOwnerType).ToString() + AuthConfig.MERCHANT);
            request.MerchantPassword = ConfigurationCache.Instance.GetValue(((int)apiOwnerType).ToString() + AuthConfig.MERCHANT_PASSWORD);
            request.MerchantUser = ConfigurationCache.Instance.GetValue(((int)apiOwnerType).ToString() + AuthConfig.MERCHANT_USER);
            return request;
        }
    }
}
