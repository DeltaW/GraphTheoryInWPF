using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GraphTheoryInWPF.Model {

    public class Ini {

        // Reference: https://www.codeproject.com/Articles/1966/An-INI-file-handling-class-using-C

        public string Path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public Ini(string path) {
            this.Path = path;
        }

        public void WriteValue(string section, string key, string value) {
            WritePrivateProfileString(section, key, value, this.Path);
        }

        public string ReadValue(string section, string key) {
            StringBuilder stringBuilder = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", stringBuilder, 255, this.Path);
            return stringBuilder.ToString();
        }
    }
}
