using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net.Config;

namespace Common.Logger
{
    public class Logger
    {
        private static Logger _instance;
        private readonly ILog log;

        private Logger()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public static Logger Instance
        {
            get { return _instance ?? (_instance = new Logger()); }
        }

        public void Log(string message)
        {
            log.Info(message);
        }

        public void Log(Exception e)
        {
            log.Error("Message: " + e.Message + "\n" + "Stack Trace: " + e.StackTrace);
        }
    }
}
