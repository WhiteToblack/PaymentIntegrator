using Integrator.Models;
using Newtonsoft.Json;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentManagement.PaymentOperation.Request
{
    [Serializable]
    public class BankRequest : IBankRequest
    {
        //public BankRequest(RequestBase requestBase, ResponseBase responseBase) {
        //    Request = requestBase;
        //    Response = responseBase;
        //}

        public ActionType ActionType { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<RequestBase>))]
        public IRequestBase Request { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<ResponseBase>))]
        public IResponseBase Response { get; set; }
        public PaymentApiOwner ApiOwner { get; set; }
        public SessionType SessionType { get; set; }
        public string SessionToken { get; set; }
        public string ReturnUrl { get; set; }
        public bool Is3DUsed { get; set; }
    }
}

public class ConcreteTypeConverter<TConcrete> : JsonConverter
{
    public override bool CanConvert(Type objectType) {
        //assume we can convert to anything for now
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        //explicitly specify the concrete type we want to create
        return serializer.Deserialize<TConcrete>(reader);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        //use the default serialization - it works fine
        serializer.Serialize(writer, value);
    }
}
