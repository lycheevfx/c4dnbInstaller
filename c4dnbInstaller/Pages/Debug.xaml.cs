using System;
using System.Collections.Generic;
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
using c4dnbInstaller.Data;
using System.Diagnostics;

namespace c4dnbInstaller.Pages
{
    /// <summary>
    /// Debug.xaml 的交互逻辑
    /// </summary>
    public partial class Debug : Page
    {
        public Debug()
        {
            InitializeComponent();
            GlobalData.MainWindowH.RadioButtonDebug.Visibility = Visibility.Visible;
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalData.TextBoxH = (TextBox)sender;
            GlobalData.TextBoxH.Text = GlobalData.InstallationResults;
            
        }
    }
}
