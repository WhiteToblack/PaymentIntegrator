using log4net;
using Newtonsoft.Json;
using PaymentManagement.Log;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation {
    public class ApiCaller : IApiCaller {
        private static readonly ILog logger = LogManager.GetLogger("PaymentManager");

        public ApiCaller() {
        }

        public async Task<T> Call<T>(string api, string methodName, object requestObj = null) where T : new() {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), api, methodName, requestObj);

            using (var client = new HttpClient()) {
                string apiUrl = GetPaymentProcessApiUrl() + api + "/" + methodName;
                client.BaseAddress = new Uri(apiUrl);

                string inputJson = JsonConvert.SerializeObject(requestObj);
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage responseTask;
                if (requestObj == null) {
                    responseTask = await client.GetAsync(methodName);
                } else {
                    responseTask = await client.PostAsJsonAsync(methodName, requestObj);
                }

                if (responseTask.IsSuccessStatusCode) {
                    string resultContent = await responseTask.Content.ReadAsStringAsync();
                    LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), JsonConvert.DeserializeObject<T>(resultContent));

                    return JsonConvert.DeserializeObject<T>(resultContent);
                } else //web api sent error response 
                {
                    LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), responseTask);
                    //log response status here..
                    return new T();
                }
            }
        }

        public static string GetPaymentProcessApiUrl() {
            if (ConfigurationManager.AppSettings["IntegratorApiUrl"] == null) {
                throw new Exception("IntegratorApiUrl not found");
            }

            return ConfigurationManager.AppSettings["IntegratorApiUrl"];
        }
    }
}
