using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using GbfRaidfinder.Data;
using GbfRaidfinder.Interfaces;

namespace GbfRaidfinder.Common {
    internal static class Extensions {
        private static readonly HttpClient HttpClient = new HttpClient {
            DefaultRequestHeaders = {{"Application-Id", Credentials.AppId}}
        };

        public static void Remove<T>(this ObservableCollection<T> collection,
            Func<T, bool> condition) {
            for (var i = collection.Count - 1; i >= 0; i--) {
                if (!condition(collection[i])) {
                    continue;
                }
                collection.RemoveAt(i);
                break;
            }
        }

        public static async Task Translate(this ITweetInfo tweet) {
            if (string.IsNullOrWhiteSpace(tweet.Text) || tweet.Translated) {
                return;
            }

            const string target = "en";
            const string source = "ja";
            //var link = $"http://104.131.147.227/api/v1/translate?t={target}&s={source}&q={tweet.Text}";
            var link = $"http://tensei.moe/api/v1/translate?t={target}&s={source}&q={tweet.Text}";

            try {
                var response = await HttpClient.GetStringAsync(link);
                if (response == "error") {
                    return;
                }
                tweet.Text = response;
                tweet.Translated = true;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}