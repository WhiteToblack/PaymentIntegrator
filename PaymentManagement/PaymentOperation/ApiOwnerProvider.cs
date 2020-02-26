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
            throw new NotImplementedException();
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

        public IRequestBase GetRequestInstance() {
            switch (apiOwner) {
                case PaymentApiOwner.UniSoft:
                    return PrepareRequestInstance<UniPayRequest>();             
                default:
                    return new Request.RequestBase();
            }
        }

        private T PrepareRequestInstance<T>() where T: RequestBase, new() {
            T _request = new T();
            _request.Merchant = "";
            _request.MerchantPassword = "";
            _request.MerchantUser = "";

            return _request;
        }
    }
}
