using AM.Core.Logging;
using NLog;
using NLog.Config;
using NLog.Common;
using NLog.Targets;
using System;
using System.IO;
using System.Text;

namespace AM.Tools.Logging
{
    public class NLogLogger : IAMLogger
    {
        private static readonly object SyncRoot = new object();
        private static bool _initialized;
        private static string _configFilePath = string.Empty;
        private static string _fallbackFileName = string.Empty;

        private readonly Logger _logger;

        public NLogLogger(string name)
        {
            EnsureInitialized();
            _logger = LogManager.GetLogger(string.IsNullOrWhiteSpace(name) ? "System" : name);
        }

        public bool IsInitialized
        {
            get { return _initialized && LogManager.Configuration != null; }
        }

        public string ConfigFilePath
        {
            get { return _configFilePath; }
        }

        public string CurrentLogFilePath
        {
            get
            {
                var configuration = LogManager.Configuration;
                if (configuration == null)
                    return string.Empty;

                var fileTarget = configuration.FindTargetByName<FileTarget>("file");
                if (fileTarget == null || fileTarget.FileName == null)
                    return string.Empty;

                try
                {
                    var logEvent = new LogEventInfo
                    {
                        TimeStamp = DateTime.Now,
                        Level = LogLevel.Info,
                        LoggerName = _logger == null ? "System" : _logger.Name
                    };

                    var filePath = fileTarget.FileName.Render(logEvent);
                    if (string.IsNullOrWhiteSpace(filePath))
                        return string.Empty;

                    if (!Path.IsPathRooted(filePath))
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                    }

                    return Path.GetFullPath(filePath);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            _logger.Error(ex, message);
        }

        public void Flush()
        {
            LogManager.Flush();
        }

        public void Shutdown()
        {
            LogManager.Shutdown();
        }

        private static void EnsureInitialized()
        {
            if (_initialized)
                return;

            lock (SyncRoot)
            {
                if (_initialized)
                    return;

                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var configDir = Path.Combine(baseDir, "Configuration");
                var logsDir = Path.Combine(baseDir, "logs");

                Directory.CreateDirectory(logsDir);

                _configFilePath = Path.Combine(configDir, "nlog.config");
                _fallbackFileName = Path.Combine("logs", "${shortdate}.log");

                ConfigureInternalLogger(logsDir);

                if (!TryLoadExternalConfiguration(_configFilePath))
                {
                    BuildFallbackConfiguration();
                }

                _initialized = true;
            }
        }

        private static void ConfigureInternalLogger(string logsDir)
        {
            InternalLogger.LogLevel = LogLevel.Warn;
            InternalLogger.IncludeTimestamp = true;
            InternalLogger.LogFile = Path.Combine(logsDir, "nlog-internal.log");
        }

        private static bool TryLoadExternalConfiguration(string configFilePath)
        {
            if (!File.Exists(configFilePath))
                return false;

            try
            {
                LogManager.Configuration = new XmlLoggingConfiguration(configFilePath);
                return LogManager.Configuration != null;
            }
            catch (Exception ex)
            {
                InternalLogger.Error("Load NLog config failed: " + ex);
                return false;
            }
        }

        private static void BuildFallbackConfiguration()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("file")
            {
                FileName = _fallbackFileName,
                Layout = "${longdate}|${level}|${logger}|${message}|${exception}",
                CreateDirs = true,
                KeepFileOpen = false,
                //ConcurrentWrites = true,
                Encoding = Encoding.UTF8
            };

            config.AddTarget(fileTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
        }
    }
}
