using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbfRaidfinder.Data {
    public class RaidBoss {
        public RaidBoss(string en, string lng, string link) {
            Name = en;
            Language = lng;
            Image = link;
        }
        public string Name { get; set; }
        public string Language  { get; set; }
        public string Image { get; set; }
    }
}
