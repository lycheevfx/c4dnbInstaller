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
using System.Windows.Shapes;
using System.Diagnostics;

namespace c4dnbInstaller.Windows
{
    /// <summary>
    /// MessageBox2.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox2 : HandyControl.Controls.Window
    {
        public MessageBox2()
        {
            InitializeComponent();
            StackPanelMain.MouseLeftButtonDown += (o, e) => { DragMove(); };
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
