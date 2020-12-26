using System.Threading;

namespace Common.Logger
{
    public class DERMSLogger
    {
        private ReaderWriterLock locker;
        private static DERMSLogger instance;
        private ILogger logger;

        private DERMSLogger()
        {
            locker = new ReaderWriterLock();
            logger = new ConsoleLogger();
        }

        public static DERMSLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DERMSLogger();
                }

                return instance;
            }
        }

        public void Log(string message)
        {
            locker.AcquireWriterLock(10000);

            logger.Log(message);

            locker.ReleaseWriterLock();
        }
    }
}
