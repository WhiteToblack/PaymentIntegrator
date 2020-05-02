using Newtonsoft.Json;
using Integrator.Models;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.PaymentOperation.Response;
using PaymentManagement.PaymentOperation.UniPay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PaymentManagement.Log;
using System.Reflection;

namespace PaymentManagement.RequestOperation {
    public sealed class RequestManager : IReqeustManager {
        private static readonly ILog logger = LogManager.GetLogger("PaymentManager");

        public ApiOwnerProvider apiProvider = null;
        public RequestManager(PaymentApiOwner apiOwner) {
            apiProvider = new ApiOwnerProvider(apiOwner);
        }
        public int GetInstallementCount() {
            BankRequest bankRequest = new BankRequest() {
                ActionType = ActionType.QUERYMAXINSTALLMENTCOUNTS
            };

            IResponseBase response = MakeBankRequest<InstallmentResponse>(bankRequest);
            return ((InstallmentResponse)response).MaxAllowedInstallmentCount;
        }

        public SessionResponseBase CreateSession(BankRequest bankRequest) {
            IResponseBase response = MakeBankRequest<SessionResponseBase>(bankRequest);
            return (SessionResponseBase)response;
        }

        public IResponseBase MakeBankRequest<T>(BankRequest bankRequest) where T : ResponseBase, IResponseBase {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), bankRequest);

            StreamReader responseReader = GetBankResponse(bankRequest, apiProvider);
            string fullResponse = responseReader.ReadToEnd();
            IResponseBase responseObj = null;

            IResponseBase responseBase;
            if(bankRequest.Is3DUsed) {
                ResponseBase response = new ResponseBase {
                    ResponseCode = PaymentResponseType.Waiting,
                    ResponseMsg = "3D redirection",
                    Redirect3dResponse = fullResponse
                };

                LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), response);
                return response;
            } else {
                responseBase = JsonConvert.DeserializeObject<ResponseBase>(fullResponse);
            }

            if(responseBase.ResponseCode != PaymentResponseType.Waiting && responseBase.ResponseCode != PaymentResponseType.Approved) {
                responseObj = JsonConvert.DeserializeObject<FailedResponse>(fullResponse);
                LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), responseObj);
                return responseObj;
            }

            responseObj = GetResponseFromType(bankRequest, fullResponse, responseObj);
            return responseObj;
        }

        private static IResponseBase GetResponseFromType(BankRequest bankRequest, string fullResponse, IResponseBase responseObj) {
            switch(bankRequest.ActionType) {
                case ActionType.SALE:
                    return JsonConvert.DeserializeObject<SuccessResponse>(fullResponse);
                case ActionType.DENY:
                    break;
                case ActionType.PREAUTH:
                    break;
                case ActionType.POSTAUTH:
                    break;
                case ActionType.QUERYPOINTS:
                    break;
                case ActionType.DETACHEDREFUND:
                    break;
                case ActionType.EXTERNALREFUND:
                    break;
                case ActionType.RECURRINGPLANDELETE:
                    break;
                case ActionType.QUERYCAMPAIGNONLINE:
                    break;
                case ActionType.QUERYMAXINSTALLMENTCOUNTS:
                    return JsonConvert.DeserializeObject<InstallmentResponse>(fullResponse);
                case ActionType.SESSIONTOKEN:
                case ActionType.QUERYSESSION:
                    return JsonConvert.DeserializeObject<SessionResponseBase>(fullResponse);
                case ActionType.QUERYPAYMENT:
                    return JsonConvert.DeserializeObject<QueryPaymentResponse>(fullResponse);
                default:
                    return JsonConvert.DeserializeObject<SuccessResponse>(fullResponse);
            }

            return responseObj;
        }

        private static void PrepareRequestParameters(BankRequest bankRequest, Dictionary<string, object> postParameters) {
            PrepareRequestAuthParameters(bankRequest, postParameters);
            switch(bankRequest.ActionType) {
                case ActionType.SALE:
                    if(bankRequest.Is3DUsed) {
                        PrepareRequest3DSaleParameters(bankRequest, postParameters);
                        break;
                    }

                    PrepareRequestSaleParameters(bankRequest, postParameters);
                    break;
                case ActionType.DENY:
                    break;
                case ActionType.PREAUTH:
                    break;
                case ActionType.POSTAUTH:
                    break;
                case ActionType.QUERYPOINTS:
                    break;
                case ActionType.DETACHEDREFUND:
                    break;
                case ActionType.EXTERNALREFUND:
                    break;
                case ActionType.RECURRINGPLANDELETE:
                    break;
                case ActionType.QUERYCAMPAIGNONLINE:
                    break;
                case ActionType.QUERYMAXINSTALLMENTCOUNTS:
                    break;
                case ActionType.SESSIONTOKEN:
                    PrepareRequestSessionParameters(bankRequest, postParameters);
                    break;
                case ActionType.QUERYSESSION:
                case ActionType.QUERYPAYMENT:
                    PrepareRequestQuerySessionParameters(bankRequest, postParameters);
                    break;
                default:
                    break;
            }
        }

        private static StreamReader GetBankResponse(BankRequest bankRequest, ApiOwnerProvider apiProvider) {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), bankRequest, apiProvider);

            bankRequest.Request = apiProvider.GetRequestInstance(bankRequest.Request);
            string url = CalculateUrl(bankRequest, apiProvider);
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = String.Format("{0};boundary={1}", "multipart/form-data", formDataBoundary);

            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            PrepareRequestParameters(bankRequest, postParameters);

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), url, contentType, formData, bankRequest.Is3DUsed);
            HttpWebResponse webResponse = PostForm(url, "", contentType, formData, bankRequest.Is3DUsed);
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), webResponse);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            return responseReader;
        }

        private static string CalculateUrl(BankRequest bankRequest, ApiOwnerProvider apiProvider) {
            string baseUrl = apiProvider.GetApiUrl();
            string url = bankRequest.Is3DUsed ? string.Format("{0}/post/sale3d/{1}", baseUrl, bankRequest.SessionToken) : baseUrl;
            return url;
        }

        private static void PrepareRequestAuthParameters(BankRequest bankRequest, Dictionary<string, object> formData) {
            formData.Add("ACTION", bankRequest.ActionType.ToString());
            formData.Add("MERCHANTUSER", bankRequest.Request.MerchantUser);
            formData.Add("MERCHANTPASSWORD", bankRequest.Request.MerchantPassword);
            formData.Add("MERCHANT", bankRequest.Request.Merchant);
        }

        private static void PrepareRequestSaleParameters(BankRequest bankRequest, Dictionary<string, object> formData) {
            formData.Add(FormObj.MERCHANTPAYMENTID, bankRequest.Request.PaymentInformation.PaymentId);
            formData.Add(FormObj.CUSTOMER, bankRequest.Request.Customer.NameSurname);
            formData.Add(FormObj.AMOUNT, bankRequest.Request.PaymentInformation.TotalAmount);
            formData.Add(FormObj.CURRENCY, bankRequest.Request.PaymentInformation.CurrencyCode);
            formData.Add(FormObj.CARDEXPIRY, bankRequest.Request.CardInformation.ExpiryDate);
            formData.Add(FormObj.CARDCVV, bankRequest.Request.CardInformation.Cvv);
            formData.Add(FormObj.NAMEONCARD, bankRequest.Request.CardInformation.HolderName);
            formData.Add(FormObj.CARDPAN, bankRequest.Request.CardInformation.CardNumber);
            formData.Add(FormObj.SESSIONTOKEN, bankRequest.SessionToken);
        }

        private static void PrepareRequest3DSaleParameters(BankRequest bankRequest, Dictionary<string, object> formData) {
            string[] splittedExpiry = bankRequest.Request.CardInformation.ExpiryDate.Split('.');
            formData.Add(FormObj.MERCHANTPAYMENTID, bankRequest.Request.PaymentInformation.PaymentId);
            formData.Add(FormObj.EXTRA, bankRequest.Request.Extra);
            formData.Add(_3DFormObj.EXPIRY_MONTH, splittedExpiry[0]);
            formData.Add(_3DFormObj.EXPIRY_YEAR, splittedExpiry[1]);
            formData.Add(_3DFormObj.CVV, bankRequest.Request.CardInformation.Cvv);
            formData.Add(_3DFormObj.CARD_OWNER, bankRequest.Request.CardInformation.HolderName);
            formData.Add(_3DFormObj.PAN, bankRequest.Request.CardInformation.CardNumber);
            formData.Add(_3DFormObj.SESSIONTOKEN, bankRequest.SessionToken);
        }

        private static void PrepareRequestSessionParameters(BankRequest bankRequest, Dictionary<string, object> formData) {
            formData.Add(FormObj.MERCHANTPAYMENTID, bankRequest.Request.PaymentInformation.PaymentId);
            formData.Add(FormObj.CUSTOMER, bankRequest.Request.Customer.NameSurname);
            formData.Add(FormObj.AMOUNT, bankRequest.Request.PaymentInformation.TotalAmount);
            formData.Add(FormObj.CURRENCY, bankRequest.Request.PaymentInformation.CurrencyCode);
            formData.Add(FormObj.RETURNURL, bankRequest.ReturnUrl);
            formData.Add(FormObj.SESSIONTYPE, "PAYMENTSESSION");
            formData.Add(FormObj.EXTRA, bankRequest.Request.Extra);
        }

        private static void PrepareRequestQuerySessionParameters(BankRequest bankRequest, Dictionary<string, object> formData) {
            formData.Add(FormObj.SESSIONTYPE, "PAYMENTSESSION");
            formData.Add(FormObj.SESSIONTOKEN, bankRequest.SessionToken);
        }

        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, bool autoRedirect) {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if(request == null) {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            request.Expect = null;
            request.AllowAutoRedirect = autoRedirect;

            using(Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }


        private static readonly Encoding encoding = Encoding.UTF8;
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary) {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach(var param in postParameters) {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if(needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if(param.Value is FileParameter) {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

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

        public class FileParameter {
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
