using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common
{
    public class LocalSettingFileReader 
    {
        private string m_BaseDir;

       
        public string BaseDir
        {
            get
            {
                return m_BaseDir;
            }
        }

        private string m_LastReadFilePath;

      
        public LocalSettingFileReader(string dir)
        {
            this.m_BaseDir = dir;
        }
        
        public string GetLastReadFilePath()
        {
            return m_LastReadFilePath;
        }
                     public static string FindFile(string dir, string fileName, string defaultFileName)
        {
            return findFile(dir, fileName, defaultFileName, 20);
        }

        private static string findFile(string dir, string fileName, string defaultFileName, int loopLimit)
        {
            if (--loopLimit < 0 || dir == null)
            {
                return null;
            }

            String filepath = null;
            filepath = ExistsFile(dir, fileName);
            if (filepath == null && fileName != defaultFileName)
            {
                filepath = ExistsFile(dir, defaultFileName);
            }
            if (filepath == null)
            {

                String[] dirs = System.IO.Directory.GetDirectories(dir);
                Array.Sort<String>(dirs);
                foreach (String d in dirs)
                {
                    filepath = ExistsFile(d, fileName);
                    if (filepath == null && fileName != defaultFileName)
                    {
                        filepath = ExistsFile(d, defaultFileName);
                    }
                    if (filepath != null)
                    {
                        break;
                    }
                }
            }
            if (filepath == null)
            {
                filepath = findFile(System.IO.Path.GetDirectoryName(dir), fileName, defaultFileName, loopLimit);
            }

            return filepath;
        }

        private static string ExistsFile(string dir, string filename)
        {
            string ret = null;
            if (dir == null || dir.Length < 1)
            {
                return ret;
            }
            string filepath = System.IO.Path.Combine(dir, filename);

            if (System.IO.File.Exists(filepath))
            {
                ret = filepath;
            }
            return ret;
        }
    }
}
