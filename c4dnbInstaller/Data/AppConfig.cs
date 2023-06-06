using HandyControl.Data;
using System.IO;

namespace c4dnbInstaller.Data
{
    class AppConfig
    {
        public static readonly string PathFolderConfig = "C:\\ProgramData\\c4dnb\\pluginInstaller";
        public static readonly string PathFileConfig = Path.Combine(PathFolderConfig, "AppConfig.json");
        public string Lang { get; set; } = "zh-CN";
        public SkinType Skin { get; set; }
        public bool StickyOnTop { get; set; } = false;
        public bool StartC4DAfterSuccessfulInstallation { get; set; } = false;
        public bool DeleteOriginalStrings { get; set; } = false;
    }
}
