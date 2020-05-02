using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Integrator.Api.DbOperation
{
    public class PaymentConfiguration
    {
        public Dictionary<string, string> ConfigurationInstance = null;
        private static PaymentConfiguration instance = null;
        private static readonly object lockObj = new object();

        public static PaymentConfiguration Instance {
            get {
                if (instance == null) {
                    lock (lockObj) {
                        if (instance == null) {
                            return new PaymentConfiguration();
                        }
                    }
                }

                return instance;
            }
        }

        public Dictionary<string, string> InitConfigurations() {
            if (instance == null) {
                instance = new PaymentConfiguration();
            }

            if (ConfigurationInstance == null) {
                GetInnerConfigurations();
            }

            return ConfigurationInstance;
        }


        public static string GetConfigurationValue(string key) {
            string configurationValue = instance.ConfigurationInstance.Where(x => x.Key == key).FirstOrDefault().Value;
            return configurationValue;
        }

        public static T TryGetConfigurationValue<T>(string key) {
            T configurationValue;
            try {
                configurationValue = (T)Convert.ChangeType(instance.ConfigurationInstance.Where(x => x.Key == key).FirstOrDefault().Value, typeof(T));
            } catch (FormatException ex) {
                throw ex;
            }

            return configurationValue;
        }

        private void GetInnerConfigurations() {
            SqlConnection conn = new SqlConnection(PaymentIntegrationDb.Instance.conn.ConnectionString);
            instance.ConfigurationInstance = new Dictionary<string, string>();
            using (conn) {
                conn.Open();
                SqlCommand comm = new SqlCommand("PAY.GET_ALL_CONFIGURATIONS", conn);
                using (SqlDataAdapter adp = new SqlDataAdapter(comm)) {
                    using (SqlDataReader reader = comm.ExecuteReader()) {
                        while (reader.Read()) {
                            instance.ConfigurationInstance.Add(reader["CONFIGURATION_KEY"].ToString(), reader["CONFIGURATION_VALUE"].ToString());
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}
