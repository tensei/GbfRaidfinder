using System;
using System.IO;
using Newtonsoft.Json;

namespace GbfRaidfinder.Common {
    internal static class ExceptionHandler {
        private static readonly string LogsPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        public static void AddGlobalHandlers() {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => {
                try {
                    if (!Directory.Exists(LogsPath))
                        Directory.CreateDirectory(LogsPath);

                    var filePath = Path.Combine(LogsPath,
                        $"UnhandledException_{DateTime.Now.ToShortDateString().Replace("/", "-")}.json");

                    File.AppendAllText(filePath,
                        JsonConvert.SerializeObject(args.ExceptionObject, Formatting.Indented) + "\r\n\r\n");
                } catch {
                    // ignored
                }
            };
        }
    }
}
