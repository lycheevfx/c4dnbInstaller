using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c4dnbInstaller.Tools;

namespace c4dnbInstaller.Data
{
    public class C4dInfo
    {
        public C4dInfo() { }
        public C4dInfo(string C4dPath) 
        {
            string pathVersion = System.IO.Path.Combine(C4dPath, "resource", "version.h");
            if (File.Exists(pathVersion))
            {
                string strVersion = File.ReadAllText(pathVersion);

                VersionArray = new string[4];

                for (int i = 0; i < VersionArray.Length; i++)
                {
                    VersionArray[i] = Helper.StringClamp(strVersion, "C4D_V" + (i + 1).ToString() + "\t\t", "\r\n");
                }

                Version = float.Parse(VersionArray[0] + "." + VersionArray[1] + VersionArray[2] + VersionArray[3]);
            }
            else { return; }
            Path = C4dPath;
        }
        public readonly string Path;
        public readonly float? Version;
        public readonly string[] VersionArray;
        public override string ToString()
        {
            return VersionArray[0].ToString();
        }
    }
}
