using Integrator.Models;
using log4net;
using Newtonsoft.Json;
using PaymentManagement.Log;
using PaymentManagement.Models.PaymentModels.Response;
using PaymentManagement.PaymentOperation.Request;
using PaymentManagement.RequestOperation.Message;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Integrator.Api.DbOperation.Payment {
    public class PaymentProcessor {
        private static readonly ILog logger = LogManager.GetLogger("IntegratorApi");

        SqlConnection conn = null;
        public PaymentProcessor() {
            conn = PaymentIntegrationDb.Instance.conn;
        }

        public PaymentStatus StartPaymentProcess(StartPaymentProcessMessage request) {
            LogOperation.Logger(LogFormat.DEBUG, logger, MethodBase.GetCurrentMethod(), request);

            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            try {
                using (_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.INSERT_PAYMENT_PROCESS", _conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@PAYMENT_ID", request.PaymentInformation.PaymentId);
                    comm.Parameters.AddWithValue("@CUSTOMER_ID", request.Customer.Id);
                    comm.Parameters.AddWithValue("@CUSTOMER_NAME", request.Customer.Name);
                    comm.Parameters.AddWithValue("@CUSTOMER_SURNAME", request.Customer.Surname);
                    comm.Parameters.AddWithValue("@CUSTOMER_DIVISION", request.Customer.Division);
                    comm.Parameters.AddWithValue("@BANK_REQUEST_JSON", JsonConvert.SerializeObject(request.Request));
                    comm.Parameters.AddWithValue("@IS_3D", request.PaymentInformation.Use3DPayment);
                    comm.Parameters.AddWithValue("@API_OWNER", (short)request.ActionType);
                    int recordNumber = comm.ExecuteNonQuery();
                    if (recordNumber < 1) {
                        return PaymentStatus.FailedPending;
                    }
                }
            } catch (Exception ex) {
                LogOperation.Logger(LogFormat.ERROR, logger, MethodBase.GetCurrentMethod(), ex);
                //updatedb
                return PaymentStatus.FailedPending;
            } finally {
                // Close the connection
                if (_conn != null)
                    _conn.Close();
            }

            return PaymentStatus.Pending;
        }

        public PaymentStatus SavePaymentInformation(SavePaymentInformationMessage request) {
            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            try {
                using (_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.INSERT_PAYMENT_PROCESS_INFORMATION", _conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@PAYMENT_ID", request.PaymentInformation.PaymentId);
                    comm.Parameters.AddWithValue("@ACTION", request.ActionType);
                    comm.Parameters.AddWithValue("@TOTAL_AMOUNT", request.PaymentInformation.TotalAmount);
                    comm.Parameters.AddWithValue("@CURRENCY_CODE", request.PaymentInformation.CurrencyCode);
                    comm.Parameters.AddWithValue("@PAYMENT_STATUS", request.PaymentInformation.PaymentStatus);
                    comm.Parameters.AddWithValue("@PAYMENT_START_DATE", DateTime.Now);
                    comm.Parameters.AddWithValue("@INSTALLMENT", request.PaymentInformation.InstallmentCount > 1);
                    comm.Parameters.AddWithValue("@INSTALLMENT_COUNT", request.PaymentInformation.InstallmentCount);
                    comm.Parameters.AddWithValue("@BANK_SESSION_ID", request.PaymentInformation.SessionToken);
                    int recordNumber = comm.ExecuteNonQuery();
                    if (recordNumber < 1) {
                        return PaymentStatus.FailedPending;
                    }
                }
            } catch (Exception ex) {
                LogOperation.Logger(LogFormat.ERROR, logger, MethodBase.GetCurrentMethod(), ex);
                //updatedb
                return PaymentStatus.FailedPending;
            } finally {
                // Close the connection
                if (_conn != null)
                    _conn.Close();
            }

            return PaymentStatus.Pending;
        }

        public bool UpdatePaymentProcessStatus(UpdatePaymentProcessStatusMessage request) {
            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            try {
                using (_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.UPDATE_PAYMENT_PROCESS_STATUS", _conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@PAYMENT_ID", request.PaymentId);
                    comm.Parameters.AddWithValue("@PAYMENT_STATUS", request.PaymentStatus);

                    int recordNumber = comm.ExecuteNonQuery();
                    if (recordNumber != 1) {
                        //todo: Make Custom Exceptions
                        throw new DataException("RecordNotFound");
                    }
                }
            } catch (Exception ex) {
                LogOperation.Logger(LogFormat.ERROR, logger, MethodBase.GetCurrentMethod(), ex);                
                return false;
            } finally {
                // Close the connection
                if (_conn != null) {
                    _conn.Close();
                }
            }
            return true;
        }

        public PaymentStatus FinalizePaymentProcess(FinalizePaymentProcessMessage request) {
            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            try {
                using (_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.UPDATE_PAYMENT_PROCESS_INFORMATION", _conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@PAYMENT_ID", request.Response.MerchantPaymentId);
                    comm.Parameters.AddWithValue("@ERROR_CODE", request.Response.ResponseCode);
                    comm.Parameters.AddWithValue("@FINAL_AMOUNT", request.Response.FinalAmount);
                    comm.Parameters.AddWithValue("@RESPONSE_MESSAGE", request.Response.ResponseMsg);

                    int recordNumber = comm.ExecuteNonQuery();
                    if (recordNumber != 1) {
                        //todo: Make Custom Exceptions
                        throw new DataException("RecordNotFound");
                    }
                }
            } catch (Exception ex) {
                LogOperation.Logger(LogFormat.ERROR, logger, MethodBase.GetCurrentMethod(), ex);
                return PaymentStatus.FailedPending;
            } finally {
                // Close the connection
                if (_conn != null)
                    _conn.Close();
            }

            UpdatePaymentProcessStatusMessage updatePaymentProcessStatusMessage = new UpdatePaymentProcessStatusMessage {
                PaymentId = request.Response.MerchantPaymentId,
                PaymentStatus = (short)PaymentStatus.Success
            };

            if (request.Response.ResponseCode != PaymentResponseType.Approved) {
                updatePaymentProcessStatusMessage.PaymentStatus = (short)PaymentStatus.Success;
                UpdatePaymentProcessStatus(updatePaymentProcessStatusMessage);
                return PaymentStatus.Fail;
            }

            UpdatePaymentProcessStatus(updatePaymentProcessStatusMessage);
            return PaymentStatus.Success;
        }

        public bool UpdateBankResponse(UpdateBankResponseMessage request) {
            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            try {
                using (_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.UPDATE_PAYMENT_PROCESS_RESPONSE", _conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@PAYMENT_ID", request.PaymentId);
                    comm.Parameters.AddWithValue("@BANK_RESPONSE", request.BankResponse);

                    int recordNumber = comm.ExecuteNonQuery();
                    if (recordNumber != 1) {
                        //todo: Make Custom Exceptions
                        return false;
                    }
                }
            } catch (Exception ex) {
                LogOperation.Logger(LogFormat.ERROR, logger, MethodBase.GetCurrentMethod(), ex);
                return false;
            } finally {
                // Close the connection
                if (_conn != null) {
                    _conn.Close();
                }
            }

            return true;
        }

        public string SelectPaymentProcessRequestJson(string paymentId) {
            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            try {
                using (_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.SELECT_PAYMENT_PROCESS", _conn) {
                        CommandType = CommandType.StoredProcedure
                    };
                    comm.Parameters.AddWithValue("@PAYMENT_ID", paymentId);
                    using (SqlDataAdapter adp = new SqlDataAdapter(comm)) {
                        using (SqlDataReader reader = comm.ExecuteReader()) {
                            while (reader.Read()) {
                                return reader["BANK_REQUEST_JSON"].ToString();
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                //updatedb
                return null;
            } finally {
                // Close the connection
                if (_conn != null)
                    _conn.Close();
            }
            return null;
        }

        public List<PaymentInformation> SelectPendingPayments() {
            SqlConnection _conn = new SqlConnection(conn.ConnectionString);
            List<PaymentInformation> paymentInformationList = new List<PaymentInformation>();
            try {
                using(_conn) {
                    _conn.Open();
                    SqlCommand comm = new SqlCommand("PAY.SELECT_PENDING_PAYMENT_PROCESS_INFORMATIONS", _conn) {
                        CommandType = CommandType.StoredProcedure
                    };
                  
                    using(SqlDataAdapter adp = new SqlDataAdapter(comm)) {
                        using(SqlDataReader reader = comm.ExecuteReader()) {
                            while(reader.Read()) {
                                FillPaymentInformationList(paymentInformationList, reader);
                            }
                        }
                    }
                }
            } catch(Exception ex) {
                LogOperation.Logger(LogFormat.ERROR, logger, MethodBase.GetCurrentMethod(), ex);
                //updatedb
                return null;
            } finally {
                // Close the connection
                if(_conn != null)
                    _conn.Close();
            }

            return paymentInformationList;
        }

        private static void FillPaymentInformationList(List<PaymentInformation> paymentInformationList, SqlDataReader reader) {
            AmountInformation amountInformation = new AmountInformation {
                TotalAmount = Convert.ToDecimal(reader["TOTAL_AMOUNT"]),
                CurrencyCode = reader["CURRENCY_CODE"].ToString(),
                IsSelected = true
            };

            PaymentInformation paymentInformation = new PaymentInformation {
                PaymentId = reader["PAYMENT_ID"].ToString(),
                PaymentStatus = (PaymentStatus)Enum.Parse(typeof(PaymentStatus), reader["PAYMENT_STATUS"].ToString()),
                PaymentStartDate = DateTime.Parse(reader["PAYMENT_START_DATE"].ToString()),
                PaymentCompleteDate = reader["PAYMENT_COMPLETE_DATE"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(reader["PAYMENT_COMPLETE_DATE"].ToString()),
                InstallmentCount = Convert.ToInt32(reader["INSTALLMENT_COUNT"]),
                SessionToken = reader["BANK_SESSION_ID"].ToString()
            };

            paymentInformation.AmountInformation = new List<AmountInformation>();
            paymentInformation.AmountInformation.Add(amountInformation);
            paymentInformationList.Add(paymentInformation);
        }
    }
}