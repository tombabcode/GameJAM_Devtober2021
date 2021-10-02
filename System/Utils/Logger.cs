using GameJAM_Devtober2021.System.Models;
using GameJAM_Devtober2021.System.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameJAM_Devtober2021.System.Utils {
    public static class Logger {

        public static List<LogModel> Logs { get; private set; }
        public static string LogFileName { get; private set; }
        public static bool SaveToFile { get; set; }

        public static void Initialize( ) {
            Logs = new List<LogModel>( );
            LogFileName = "log_" + DateTime.UtcNow.ToFileTimeUtc( ) + ".log";
            SaveToFile = true;
        }

        private static void Log(LogType type, string message, Exception exception) {
            if (Logs.Count >= 500) {
                Logs.RemoveAt(0);
            }

            LogModel log = new LogModel( ) {
                Type = type,
                Time = DateTime.UtcNow,
                Message = message,
                Exception = exception
            };

            Console.WriteLine($"[{log.Time.ToLongTimeString( )}][{type}]: {message}");
            if (exception != null) {
                Console.WriteLine(exception);
            }

            Logs.Add(log);

            if (SaveToFile) {
                Save(log);
            }
        }

        private static void Save(LogModel log) {
            if (!Directory.Exists("Logs")) {
                Directory.CreateDirectory("Logs");
            }

            File.AppendAllText(Path.Combine("Logs", LogFileName), $"[{log.Time.ToLongTimeString( )}][{log.Type}]: {log.Message}" + '\n');

            if (log.Exception != null) {
                File.AppendAllText(Path.Combine("Logs", LogFileName), log.Exception.ToString( ) + '\n');
            }
        }

        public static void Info(string message, Exception exception = null) => Log(LogType.Info, message, exception);
        public static void Warn(string message, Exception exception = null) => Log(LogType.Warn, message, exception);
        public static void Error(string message, Exception exception = null) => Log(LogType.Error, message, exception);

    }
}
