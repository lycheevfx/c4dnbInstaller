using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using HandyControl.Controls;

namespace c4dnbInstaller.Data
{
    internal class GlobalData
    {
        public static void Init()
        {
            if (File.Exists(AppConfig.PathFileConfig))
            {
                try
                {
                    var json = File.ReadAllText(AppConfig.PathFileConfig);
                    Config = (string.IsNullOrEmpty(json) ? new AppConfig() : JsonConvert.DeserializeObject<AppConfig>(json)) ?? new AppConfig();
                }
                catch
                {
                    Config = new AppConfig();
                }
            }
            else
            {

                Config = new AppConfig();
            }
        }
        
        public static void Save()
        {
            var json = JsonConvert.SerializeObject(Config);
            System.IO.Directory.CreateDirectory(AppConfig.PathFolderConfig);
            File.WriteAllText(AppConfig.PathFileConfig, json);
        }
        public static MainWindow MainWindowH { get; set; }
        public static System.Windows.Controls.TextBox TextBoxH { get; set; }
        public static string InstallationResults { get; set; }
        public static AppConfig Config { get; set; }

    }
}