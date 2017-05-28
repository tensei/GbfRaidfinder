using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace GbfRaidfinder.Common {
    internal class UniqueId {
        public static string Id { get; set; }

        public static string Create() {
            var cpuInfo = string.Empty;
            try {
                var mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();

                foreach (var mo in moc) {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
                return Sha256(cpuInfo + Environment.MachineName);
            }
            catch (Exception) {
                var name = Environment.MachineName;
                var hmm = Environment.UserName;
                return Sha256(name + hmm);
            }
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