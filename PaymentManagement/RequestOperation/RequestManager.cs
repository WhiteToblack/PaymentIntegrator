using Newtonsoft.Json;
using PaymentManagement.Models.PaymentModels.Request;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Config = PaymentManagement.DbOperation.AuthConfig;

namespace PaymentManagement.RequestOperation
{
    public class RequestManager : IReqeustManager
    {
        public T MakeBankRequest<T>(BankRequest bankRequest) where T : IResponseBase {
            ApiOwnerProvider apiProvider = new ApiOwnerProvider(bankRequest.ApiOwner);
            bankRequest.Request = apiProvider.GetRequestInstance(bankRequest.Request);
            string url = apiProvider.GetApiUrl();    
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = String.Format("{0};boundary={1}", "multipart/form-data", formDataBoundary);           

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            PrepareRequestParameters(bankRequest, postParameters);
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            HttpWebResponse webResponse = PostForm(url, "", contentType, formData);
            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();

            var responseObj = JsonConvert.DeserializeObject<object>(fullResponse);
            
            throw new NotImplementedException();
        }

        private static void PrepareRequestParameters(BankRequest bankRequest, Dictionary<string, object> formData) {
            formData.Add("ACTION", "SALE");
            formData.Add("MERCHANTUSER", bankRequest.Request.MerchantUser);
            formData.Add("MERCHANTPASSWORD", bankRequest.Request.MerchantPassword);
            formData.Add("MERCHANT", bankRequest.Request.Merchant);
            formData.Add(FormObj.MERCHANTPAYMENTID, bankRequest.Request.PaymentInformation.PaymentId);
            formData.Add(FormObj.CUSTOMER, bankRequest.Request.Customer.NameSurname);
            formData.Add(FormObj.AMOUNT, bankRequest.Request.PaymentInformation.TotalAmount);
            formData.Add(FormObj.CURRENCY, bankRequest.Request.PaymentInformation.CurrencyCode);
            formData.Add(FormObj.CARDEXPIRY, bankRequest.Request.CardInformation.ExpiryDate);
            formData.Add(FormObj.CARDCVV, bankRequest.Request.CardInformation.Cvc);
            formData.Add(FormObj.NAMEONCARD, bankRequest.Request.CardInformation.HolderName);
            formData.Add(FormObj.CARDPAN, bankRequest.Request.CardInformation.CardNumber);
        }

        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData) {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null) {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }


        private static readonly Encoding encoding = Encoding.UTF8;
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary) {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters) {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter) {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header),0 ,encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                } else {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype) {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }

    }
}
