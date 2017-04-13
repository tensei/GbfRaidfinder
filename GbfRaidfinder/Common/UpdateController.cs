using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GbfRaidfinder.Common {
    public static class UpdateController {
        private const float Localver = 0.6f;
        public static async Task<bool> Check() {
            //https://raw.githubusercontent.com/tensei/GbfRaidfinder/master/GbfRaidfinder/Assets/version.txt
            try {
                string ver;
                using (var web = new WebClient{Encoding = Encoding.UTF8}) {
                    ver = await web.DownloadStringTaskAsync(
                        "https://raw.githubusercontent.com/tensei/GbfRaidfinder/master/GbfRaidfinder/Assets/version.txt");
                }
                return float.Parse(ver, CultureInfo.InvariantCulture) > Localver;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
