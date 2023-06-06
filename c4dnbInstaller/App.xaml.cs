using System;
using System.Diagnostics;
using System.Windows;
using c4dnbInstaller.Data;
using HandyControl.Data;
using HandyControl.Tools;

namespace c4dnbInstaller
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            GlobalData.Init();
            Installation.c4dInfos = Installation.GetC4dInfos();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            GlobalData.Save();
        }
        public static void UpdateSkin(string Skin)
        {
            SkinType skin = (SkinType)Enum.Parse(typeof(SkinType), Skin);

            var skins0 = Current.Resources.MergedDictionaries[0];
            skins0.MergedDictionaries.Clear();
            skins0.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));

            var skins1 = Current.Resources.MergedDictionaries[1];
            skins1.MergedDictionaries.Clear();
            skins1.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });

            Current.MainWindow?.OnApplyTemplate();
        }
        public static void UpdateLanguage()
        {
            ResourceDictionary dic = new ResourceDictionary();

            dic.Source = new Uri("./Resources/Langs/" + GlobalData.Config.Lang + ".Xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries[2] = dic;

        }
    }
}
