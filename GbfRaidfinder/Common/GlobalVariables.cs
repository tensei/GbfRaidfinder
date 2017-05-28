using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GbfRaidfinder.Interfaces;
using PropertyChanged;

namespace GbfRaidfinder.Common {
    public class GlobalVariables : IGlobalVariables, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsLoggedIn { get; set; }
        public bool ForceLogout { get; set; }

    }
}
