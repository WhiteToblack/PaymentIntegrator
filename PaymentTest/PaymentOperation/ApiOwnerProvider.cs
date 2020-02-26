using PaymentTest.Models.PaymentModels.Request;
using PaymentTest.PaymentOperation.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentTest.PaymentOperation
{
    public class ApiOwnerProvider : IApiOwnerProvider
    {

        PaymentApiOwner apiOwner;
        public ApiOwnerProvider(PaymentApiOwner _apiOwner) {
            apiOwner = _apiOwner;
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
            PrepareRequestAuth(new UniPayRequest());


            switch (apiOwner) {
                case PaymentApiOwner.UniSoft:
                    return new UniPay.UniPayRequest();             
                default:
                    return new Request.RequestBase();
            }
        }

        private T PrepareRequestInstance<T>(IRequestBase request) where T: RequestBase, new() {
            T _request = new T();
            _request.Merchant = "";
            _request.MerchantPassword = "";
            _request.MerchantUser = "";

            return _request;
        }
    }
}
