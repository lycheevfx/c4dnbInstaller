using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using c4dnbInstaller.Data;
using c4dnbInstaller.Windows;
using c4dnbInstaller.Tools;

namespace c4dnbInstaller.Pages
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Setup : Page
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void ButtonOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                C4DInfo_Add(dialog.FileName);
            }
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            Helper.LinkOpen("https://c4dnb.com/posts/83ff2157.html#C4D%E6%A0%B9%E7%9B%AE%E5%BD%95%E5%9C%A8%E5%93%AA%EF%BC%9F");
        }

        private void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.MainWindowH.RadioButtonDebug.IsChecked = true;

            Process[] ps = Process.GetProcessesByName("Cinema 4D");
            if (ps.Length > 0)
            {
                TextBoxInsertContent("DetectedC4DProcess");
                TextBoxInsertContent("\r\n");
                TextBoxInsertContent("\r\n");
                return;
            }


            if (Installation.c4dInfos == null | Installation.c4dInfos.Count == 0)
            {
                TextBoxInsertContent("AtLeastOneC4DVersionMustBeAdded");
                TextBoxInsertContent("\r\n");
                TextBoxInsertContent("\r\n");
                return;
            }
            try
            {
                Task.Run(() =>
                {
                    string dirCurrent = System.IO.Directory.GetCurrentDirectory();
                    string dirPlugins = System.IO.Path.Combine(dirCurrent, "plugins");

                    if (!Directory.Exists(dirPlugins))
                    {
                        TextBoxInsertContent("InstallationFailed");
                        TextBoxInsertContent("\r\n");
                        TextBoxInsertContent("UnableToFindThePluginsFolder");
                        return;
                    }
                    string[] pluginName = Directory.GetDirectories(dirPlugins, "*");
                    if (pluginName.Length <= 0)
                    {
                        TextBoxInsertContent("InstallationFailed");
                        TextBoxInsertContent("\r\n");
                        TextBoxInsertContent("UnableToFindThePlugin");
                        return;
                    }

                    TextBoxInsertContent("RunAsAdministrator");
                    TextBoxInsertContent(Helper.IsAdministrator().ToString());
                    TextBoxInsertContent("\r\n");
                    TextBoxInsertContent("\r\n");

                    TextBoxInsertContent("PluginName");
                    TextBoxInsertContent(System.IO.Path.GetFileName(pluginName[0]));
                    TextBoxInsertContent("\r\n");

                    foreach (C4dInfo c4dInfo in Installation.c4dInfos)
                    {
                        TextBoxInsertContent("\r\n");
                        TextBoxInsertContent("AboutToInstallTo");
                        TextBoxInsertContent("\r\n    ");
                        TextBoxInsertContent("C4DVersion");
                        TextBoxInsertContent(c4dInfo.Version.ToString());
                        TextBoxInsertContent("\r\n    ");
                        TextBoxInsertContent("C4DRootDirectory");
                        TextBoxInsertContent(c4dInfo.Path);
                        TextBoxInsertContent("\r\n");

                        string C4DStringsCN = c4dInfo.Version < 20 ? "strings_cn" : "strings_zh-CN";
                        string C4DStringsEN = c4dInfo.Version < 20 ? "strings_us" : "strings_en-US";

                        string dirC4DPlugins = System.IO.Path.Combine(c4dInfo.Path, "plugins");
                        string dirC4DPluginsSite = System.IO.Path.Combine(dirC4DPlugins, System.IO.Path.GetFileName(pluginName[0]));
                        string dirC4DRes = System.IO.Path.Combine(dirC4DPluginsSite, "res");

                        if (Directory.Exists(dirC4DPluginsSite)) { Directory.Delete(dirC4DPluginsSite, true); }


                        TextBoxInsertContent("CopyingPlugin");
                        TextBoxInsertContent("\r\n");

                        try
                        {
                            Helper.DirectoryCopy(dirPlugins, dirC4DPlugins);
                        }
                        catch (Exception e2)
                        {
                            TextBoxInsertContent(e2.ToString());
                            TextBoxInsertContent("\r\n");
                            TextBoxInsertContent("InstallationFailed");
                            return;
                        }

                        if (!string.IsNullOrEmpty(Installation.Language.Path))
                        {
                            string dirStrings = System.IO.Path.GetFileName(Installation.Language.Path).ToLower();
                            dirStrings = dirStrings.IndexOf("us") < 0 ? C4DStringsCN : C4DStringsEN;

                            if (GlobalData.Config.DeleteOriginalStrings)
                            {
                                TextBoxInsertContent("RemovingOriginalLanguagePack");
                                TextBoxInsertContent("\r\n");

                                string[] dirC4DResStrings = Directory.GetDirectories(dirC4DRes, "*trings_*");

                                foreach (string path in dirC4DResStrings)
                                {
                                    Directory.Delete(path, true);

                                }
                            }

                            TextBoxInsertContent("CopyingLanguagePack");
                            TextBoxInsertContent("\r\n");

                            string targetPath = System.IO.Path.Combine(dirC4DRes, dirStrings);

                            if (Directory.Exists(targetPath)) { Directory.Delete(targetPath, true); }

                            Helper.DirectoryCopy(Installation.Language.Path, targetPath);
                        }
                    }

                    TextBoxInsertContent("InstalledSuccessfully");
                });
            }
            catch (Exception e2)
            {
                TextBoxInsertContent("InstallationFailed");
                TextBoxInsertContent("\r\n");
                TextBoxInsertContent(e2.ToString());
            }

        }

        private void ListBoxLanguageMode_Selected(object sender, RoutedEventArgs e)
        {
            Installation.Language = (LanguageMode)((RadioButton)sender).Tag;
        }

        private void ListBoxLanguageMode_Loaded(object sender, RoutedEventArgs e)
        {
            string dirCurrent = System.IO.Directory.GetCurrentDirectory();
            string dirStrings = System.IO.Path.Combine(dirCurrent, "strings");

            if (!Directory.Exists(dirStrings)) { return; }

            string[] dirsStrings = Directory.GetDirectories(dirStrings, "*");
            if (dirsStrings.Length <= 0) { return; }

            foreach (var item1 in dirsStrings)
            {
                string[] dirsTemp = Directory.GetDirectories(item1, "?tring*");
                if (dirsTemp.Length <= 0) { continue; }

                LanguageMode languageMode = new LanguageMode();
                RadioButton item = new RadioButton();

                var dirFather = System.IO.Directory.GetParent(dirsTemp[0]);
                languageMode.Path = dirsTemp[0];
                item.Content = System.IO.Path.GetFileNameWithoutExtension(dirFather.FullName);
                languageMode.Name = item.Content.ToString();
                item.FontSize = 16;
                item.Checked += ListBoxLanguageMode_Selected;
                item.Tag = languageMode;

                ListBoxLanguageMode.Items.Add(item);

                if (Object.Equals(item.Tag, Installation.Language)) { item.IsChecked = true; }
            }
        }
        private void TextBoxInsertContent(string content)
        {
            string _content;
            try
            {
                _content = App.Current.FindResource(content).ToString();
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                _content = content;
            }

            //TextBoxReturn.Dispatcher.BeginInvoke(new Action(() => { TextBoxReturn.AppendText(_content); }));

            GlobalData.InstallationResults += _content;
            if (GlobalData.TextBoxH != null)
            {
                GlobalData.TextBoxH.Dispatcher.Invoke(new Action(() => { GlobalData.TextBoxH.Text = GlobalData.InstallationResults; GlobalData.TextBoxH.ScrollToEnd(); }));
                
            }

            Thread.Sleep(10);
        }

        private void Page_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                int count = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).Length;
                for (int i = 0; i < count; i++)
                {
                    var fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(i).ToString();
                    if (File.Exists(fileName))
                    {
                        C4DInfo_Add(Directory.GetParent(fileName).FullName);
                    }
                }
            }
        }

        private void Page_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void C4DInfo_Add(string path)
        {
            C4dInfo c4dInfo = new C4dInfo(path);

            if (string.IsNullOrEmpty(c4dInfo.Path))
            {
                MessageBox2 window = new MessageBox2();
                window.ButtonLeft.Content = App.Current.FindResource("WhatIsTheC4DRootDirectory");
                window.ButtonLeft.Visibility = Visibility.Visible;
                window.TextBlockContent.Text = App.Current.FindResource("NotAValidC4DRootDirectory").ToString();

                window.ButtonLeft.Click += ButtonHelp_Click;

                window.Owner = GlobalData.MainWindowH;

                window.ShowDialog();
            }
            else
            {
                foreach (C4dInfo item in Installation.c4dInfos)
                {
                    if (c4dInfo.Version == item.Version)
                    {
                        MessageBox2 window = new MessageBox2();
                        window.TextBlockContent.Text = App.Current.FindResource("YouHaveAddedThisVersionOfC4D").ToString();

                        window.Owner = GlobalData.MainWindowH;

                        window.ShowDialog();
                        return;
                    }
                }
                Installation.c4dInfos.Add(c4dInfo);

                TagContainerC4dList.ItemsSource = null;
                TagContainerC4dList.ItemsSource = Installation.c4dInfos;
                
            }
        }

        private void TagContainerC4dList_Loaded(object sender, RoutedEventArgs e)
        {
            TagContainerC4dList.ItemsSource = null;
            TagContainerC4dList.ItemsSource = Installation.c4dInfos;
        }
    }
}
