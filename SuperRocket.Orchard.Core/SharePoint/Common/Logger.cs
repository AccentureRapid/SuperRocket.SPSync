using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperRocket.Orchard.Core.SharePoint.Common
{
    public static class Logger
    {
        private static object _logLockObject = new object();
        private static bool _enableDebugLog = false;
        private static string _logPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data/Logs"), string.Format(@"Log_Sharepoint_Sync_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd")));
        private static string _logDebugPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data/Logs"), string.Format(@"Log_Debug_Sharepoint_Sync_{0}.txt", DateTime.Now.ToString("yyyy_MM_dd")));

        static Logger()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data/Logs");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            bool.TryParse(ConfigurationManager.AppSettings["EnableDebugLog"], out _enableDebugLog);

            if (!_enableDebugLog && File.Exists(_logPath))
            {
                File.Delete(_logPath);
            }
          }

        public static void Log(string message, params object[] args)
        {
            lock (_logLockObject)
            {
                File.AppendAllText(_logPath, string.Format(message, args) + Environment.NewLine);
            }
        }

        internal static void LogDebug(string message, params object[] args)
        {
            LogDebug(Guid.Empty, Guid.Empty, message, args);
        }

        internal static void LogDebug(Guid correlationId, Guid itemId, string message, params object[] args)
        {
            if (!_enableDebugLog)
                return;

            lock (_logLockObject)
            {
                File.AppendAllText(_logDebugPath, string.Format("{0} [{1}] {2} ", DateTime.Now, correlationId.ToString("N"), itemId.ToString("N")) + string.Format(message, args) + Environment.NewLine);
            }
        }
    }
}
