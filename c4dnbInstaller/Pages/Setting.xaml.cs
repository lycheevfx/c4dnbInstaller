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
using HandyControl.Data;
using c4dnbInstaller.Data;
using c4dnbInstaller.Tools;

namespace c4dnbInstaller.Pages
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Page
    {
        public Setting()
        {
            InitializeComponent();
        }
        private void ComboBoxSkin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string skin = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();

            GlobalData.Config.Skin = (SkinType)Enum.Parse(typeof(SkinType), skin);
            GlobalData.Save();
            App.UpdateSkin(skin);
        }

        private void ComboBoxSkin_Loaded(object sender, RoutedEventArgs e)
        {
            ((ComboBox)sender).SelectedIndex = (int)GlobalData.Config.Skin;
        }

        private void ComboBoxLanguage_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                ComboBoxItem item = ((ComboBoxItem)comboBox.Items[i]);
                if (item.Tag.ToString() == GlobalData.Config.Lang)
                {
                    comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ComboBoxLanguage_Selected(object sender, RoutedEventArgs e)
        {
            GlobalData.Config.Lang = ((ComboBoxItem)sender).Tag.ToString();
            App.UpdateLanguage();
        }

        private void ButtonLink_Click(object sender, RoutedEventArgs e)
        {
            Helper.LinkOpen(((Button)sender).Tag.ToString());
        }

        private void CheckBoxStickyOnTop_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.MainWindowH.Topmost = GlobalData.Config.StickyOnTop;
        }
    }
}
