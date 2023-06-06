using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace c4dnbInstaller.Data
{
    internal class Installation
    {
        public static LanguageMode Language = new LanguageMode();
        public static List<C4dInfo> c4dInfos = new List<C4dInfo>();

        public static List<C4dInfo> GetC4dInfos () 
        {
            List<C4dInfo> list = new List<C4dInfo>();

            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var keyUninstall = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);
                foreach (String keyName in keyUninstall.GetSubKeyNames())
                {
                    string name = keyName;
                    name = keyName.ToLower();
                    
                    if (name.IndexOf("maxon") > -1)
                    {
                        RegistryKey sitekey = keyUninstall.OpenSubKey(keyName);

                        foreach (string valName in sitekey.GetValueNames())
                        {
                            if (valName == "InstallLocation")
                            {
                                string value = sitekey.GetValue(valName).ToString();
                                Debug.WriteLine(value);
                                C4dInfo c4dInfo = new C4dInfo(value);
                                list.Add(c4dInfo);
                            }
                        }
                    }
                }
            }

            List<C4dInfo> list1 = list.OrderByDescending(t => t.Version).ToList();

            return list1;
        }

        
        private static List<C4dInfo> GetInstalledSoftwareList()
        {
            List<C4dInfo> gInstalledSoftware = new List<C4dInfo>();

            //using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false))
            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var keyUninstall = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);
                foreach (String keyName in keyUninstall.GetSubKeyNames())
                {
                    string name = keyName;
                    name = keyName.ToLower();
                    if (name.IndexOf("maxon") > -1)
                    {
                        RegistryKey sitekey = keyUninstall.OpenSubKey(keyName);

                        foreach (string valName in sitekey.GetValueNames())
                        {

                            string value = sitekey.GetValue(valName).ToString();
                            if (valName == "InstallLocation")
                            {
                                C4dInfo c4dInfo = new C4dInfo(value);
                                gInstalledSoftware.Add(c4dInfo);
                            }
                        }
                    }
                }
            }
            return gInstalledSoftware;
        }

    }
}
