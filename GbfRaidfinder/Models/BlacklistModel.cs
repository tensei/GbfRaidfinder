using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace GbfRaidfinder.Models {
    [ImplementPropertyChanged]
    public class BlacklistModel {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public List<string> Users { get; set; } = new List<string>();
    }
}
