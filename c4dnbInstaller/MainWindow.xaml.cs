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
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using c4dnbInstaller.Data;
using c4dnbInstaller.Pages;

namespace c4dnbInstaller
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            App.UpdateSkin(GlobalData.Config.Skin.ToString());
            App.UpdateLanguage();

            RadioButtonSetup.IsChecked = true;

            Topmost = GlobalData.Config.StickyOnTop;

            GlobalData.MainWindowH = this;
        }
        private void ToggleButtonStickyOnTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = GlobalData.Config.StickyOnTop;
        }

        private void RadioButtonNonClientArea_Checked(object sender, RoutedEventArgs e)
        {
            FrameMain.Source = new Uri("/Pages/" + ((RadioButton)sender).Tag.ToString() + ".xaml", UriKind.Relative);
        }
    }
}
