using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GbfRaidfinder.Common {
    class UniqueId {
        public static string Id { get; set; }

        public static string Create() {
            var cpuInfo = string.Empty;
            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();

            foreach (var mo in moc) {
                cpuInfo = mo.Properties["processorID"].Value.ToString();
                break;
            }
            const string drive = "C";
            var dsk = new ManagementObject(
                @"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            var volumeSerial = dsk["VolumeSerialNumber"].ToString();
            return Sha256(cpuInfo + volumeSerial);
        }
        private static string Sha256(string password) {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (var theByte in crypto) {
                hash.Append(theByte.ToString("x2"));
            }
            Id = hash.ToString();
            return Id;
        }
    }
}
