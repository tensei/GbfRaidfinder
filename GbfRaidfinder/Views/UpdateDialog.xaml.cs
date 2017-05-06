using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GbfRaidfinder.Views {
    /// <summary>
    /// Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : UserControl {
        public UpdateDialog() {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {

            //https://github.com/tensei/GbfRaidfinder/releases
            try {
                Process.Start("https://github.com/tensei/GbfRaidfinder/releases");
            } catch (Exception exception) {
                Console.WriteLine(exception);
            }
        }
    }
}
