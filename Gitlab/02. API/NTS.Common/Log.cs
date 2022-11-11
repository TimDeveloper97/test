using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NTS.Common
{
    public class Log
    {
        public static IniFile iniFile = new IniFile(System.Windows.Forms.Application.StartupPath + "\\" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".ini");

        public static void LogError(string service, string function, string content)
        {
            if (!Directory.Exists(Application.StartupPath + "\\Log"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Log");
            }

            Constants.iniFile = new IniFile(Application.StartupPath + "\\Log\\Error_" + service + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            Constants.iniFile.IniWriteValue("Error", function + " : " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff") + " : ", content);
        }

        public static void LogContent(string content)
        {
            if (!Directory.Exists(Application.StartupPath + "\\Log"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Log");
            }

            Constants.iniFile = new IniFile(Application.StartupPath + "\\Log\\Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            Constants.iniFile.IniWriteValue("Log", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff") + " : ", content);
        }
    }
}
