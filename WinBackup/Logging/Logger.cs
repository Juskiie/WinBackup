using System;
using Microsoft.Extensions.Logging;

namespace WinBackup.Logging
{
    public class Logger : ILogger
    {
        private readonly string _filePath;
        private readonly string _category;

        public Logger(string category, string filePath)
        {
            _category = category;
            _filePath = filePath;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            string message = $"{DateTime.Now:u} [{logLevel}] {_category}: {formatter(state, exception)}";
            File.AppendAllText(_filePath, message + Environment.NewLine);
        }
    }

    public class SimpleFileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleFileLoggerProvider"/> class.
        /// Ensures the log directory exists and generates a timestamped log file.
        /// </summary>
        /// <param name="logDirectory">The directory where the log file will be created.</param>
        public SimpleFileLoggerProvider(string logDirectory)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            _filePath = Path.Combine(logDirectory, $"log_{timestamp}.log");
            Directory.CreateDirectory(logDirectory);
        }

        /// <summary>
        /// Creates a new <see cref="Logger"/> instance for the specified category name.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>A configured <see cref="Logger"/> instance.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, _filePath);
        }

        /// <summary>
        /// Disposes resources used by the provider. No resources to release in this implementation.
        /// </summary>
        public void Dispose() { }
    }
}
