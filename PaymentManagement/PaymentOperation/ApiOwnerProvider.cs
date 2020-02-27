using PaymentManagement.DbOperation;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation
{
    public class ApiOwnerProvider : IApiOwnerProvider
    {

        PaymentApiOwner apiOwner;
        public ApiOwnerProvider(PaymentApiOwner _apiOwner) {
            apiOwner = _apiOwner;
        }

        internal string GetApiUrl() {
            string apiUrl = PaymentConfiguration.GetConfigurationValue(((int)apiOwner).ToString() + AuthConfig.API_URL);
            return apiUrl;
        }

        // Provide all owners after all integrations OK
        public IApiOwner GetApiOwner() {
            switch (apiOwner) {
                case PaymentApiOwner.UniSoft:
                    return new UniPayApi();
                default:
                    return new UniPayApi();
            }
        }

        public IRequestBase GetRequestInstance(IRequestBase request) {
            switch (apiOwner) {
                case PaymentApiOwner.UniSoft:
                    return PrepareRequestInstance<UniPayRequest>(request);
                default:
                    return new Request.RequestBase();
            }
        }

        public Type GetRequestObjType() {
            switch (apiOwner) {
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
            request.Merchant = PaymentConfiguration.GetConfigurationValue(((int)apiOwner).ToString() + AuthConfig.MERCHANT);
            request.MerchantPassword = PaymentConfiguration.GetConfigurationValue(((int)apiOwner).ToString() + AuthConfig.MERCHANT_PASSWORD);
            request.MerchantUser = PaymentConfiguration.GetConfigurationValue(((int)apiOwner).ToString() + AuthConfig.MERCHANT_USER);
            return request;
        }
    }
}
