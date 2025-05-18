using Microsoft.Extensions.Logging;

namespace WinBackup.Logging
{
    public class Log
    {
        public static ILoggerFactory Factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddProvider(new SimpleFileLoggerProvider("Logs"));
        });

        public static ILogger<T> For<T>() => Factory.CreateLogger<T>();
    }
}
