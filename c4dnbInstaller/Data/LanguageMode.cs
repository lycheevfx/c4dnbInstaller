using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace c4dnbInstaller.Data
{
    public class LanguageMode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public bool Equals(LanguageMode languageMode)
        {
            if (languageMode == null) return false;

            if (Name == languageMode.Name && Path == languageMode.Path)
                return true;
            else
                return false;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            LanguageMode languageObj = obj as LanguageMode;
            if (languageObj == null)
                return false;
            else
                return Equals(languageObj);
        }
    }
}
