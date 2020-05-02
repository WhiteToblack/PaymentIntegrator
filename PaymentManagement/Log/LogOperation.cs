using log4net;
using Newtonsoft.Json;
using System;
using System.Reflection;


namespace PaymentManagement.Log {
    public class LogOperation {
        public static string LoggerFormat = "Class : {0} Method : {1}  Log : {2}";
    
        public static void Logger(LogFormat logFormat, ILog logger, MethodBase methodBase, params dynamic[] log) {
            try {           
                var json = JsonConvert.SerializeObject(log, Newtonsoft.Json.Formatting.None);
                switch(logFormat) {
                    case LogFormat.DEBUG:
                        logger.Debug(string.Format(LoggerFormat, methodBase.ReflectedType.Name, methodBase.Name, json));
                        break;
                    case LogFormat.INFO:
                        logger.Info(string.Format(LoggerFormat, methodBase.ReflectedType.Name, methodBase.Name, json));
                        break;
                    case LogFormat.WARN:
                        logger.Warn(string.Format(LoggerFormat, methodBase.ReflectedType.Name, methodBase.Name, json));
                        break;
                    case LogFormat.FATAL:
                        logger.Fatal(string.Format(LoggerFormat, methodBase.ReflectedType.Name, methodBase.Name, json));
                        break;
                    case LogFormat.ERROR:
                        logger.Error(string.Format(LoggerFormat, methodBase.ReflectedType.Name, methodBase.Name, json));
                        break;
                    default:
                        break;
                }
              
            } catch { }
        }

    }
}