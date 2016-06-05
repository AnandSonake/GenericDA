using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;

namespace GenericDA
{
    public static class Logger
    {
        private static ILog logAgent = LogManager.GetLogger("GenericDA");

        static Logger()
        {
            string log4NetConfigFilePath = "log4net.config"; //Assumed log4net config will be in the same directory as GenericDA to reduce configuration
            FileInfo fi = new FileInfo(log4NetConfigFilePath);
            XmlConfigurator.Configure(fi);
        }

        public static void Debug(string message)
        {
            logAgent.Debug(message);
        }

        public static void Debug(string message, Exception ex)
        {
            logAgent.Debug(message, ex);
        }

        public static void Warn(string message)
        {
            if (logAgent != null)
                logAgent.Warn(message);
        }

        public static void Fatal(string message)
        {
            logAgent.Fatal(message);
        }
        public static void Error(string message)
        {
            logAgent.Error(message);
        }
        public static void Error(string message, Exception ex)
        {
            logAgent.Error(message, ex);
        }
        public static void Info(string message)
        {
            logAgent.Info(message);
        }
    }
}
