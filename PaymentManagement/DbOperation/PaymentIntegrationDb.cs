using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace PaymentManagement.DbOperation
{
    public class PaymentIntegrationDb
    {
        public static string connectionString = "";
        private string ConnectionString;
        private static PaymentIntegrationDb instance = null;
        private static readonly object lockObj = new object();
        public SqlConnection conn;
      
        public static PaymentIntegrationDb Instance {
            get {
                if (instance == null) {
                    lock (lockObj) {
                        return new PaymentIntegrationDb(connectionString);
                    }
                }

                return instance;
            }
        }

        public PaymentIntegrationDb(string connectionString) {
            ConnectionString = connectionString;
            conn = new SqlConnection(ConnectionString);            
        }      

        public static void SetupDb(string connectionString) {
            instance = new PaymentIntegrationDb(connectionString);
        }       
    }
}
