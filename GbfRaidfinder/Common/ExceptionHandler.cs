using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GbfRaidfinder.Data;
using Newtonsoft.Json;
using RestSharp;

namespace GbfRaidfinder.Common {
    internal static class ExceptionHandler {
        private static readonly string LogsPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");

        public static void AddGlobalHandlers() {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => {
                try {
                    if (!Directory.Exists(LogsPath))
                        Directory.CreateDirectory(LogsPath);
                    var filename = $"UnhandledException_{DateTime.Now.ToShortDateString().Replace("/", "-")}.json";
                    var filePath = Path.Combine(LogsPath, filename);
                    var error = JsonConvert.SerializeObject(args.ExceptionObject, Formatting.Indented);
                    File.AppendAllText(filePath, error);
#if DEBUG
                    SendReport(filePath, filename);
#endif
                } catch {
                    // ignored
                }
            };
        }
        private static void SendReport(string filepath, string filename) {
            var client = new RestClient("http://tensei.moe/api/v1/error_report");
            //var client = new RestClient("http://127.0.0.1:5000/api/v1/error_report");
            var request = new RestRequest(Method.POST) {
                RequestFormat = DataFormat.Json,
            };
            request.AddHeader("Application-Id", Credentials.AppId);
            request.AddHeader("Application-UserId", UniqueId.Id);
            request.AddHeader("Application-Name", "GbfRaidfinder");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Application-Ver", UpdateController.Localver.ToString(CultureInfo.InvariantCulture));
            request.AddFile("file", File.ReadAllBytes(filepath), filename, "multipart/form-data");
            client.Execute(request);
        }
    }
}