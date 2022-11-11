using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTS.Common
{
    public static class UtilFile
    {
        private static string m_exePath = string.Empty;

        /// <summary>
        /// Đọc mảng file
        /// </summary>
        /// <param name="fileName">Tên file</param>
        /// <returns></returns>
        public static string[] ReadArrayFile(string fileName)
        {
            string[] content = { };
            m_exePath = UtilFile.GetFilePath(Path.GetDirectoryName(Application.ExecutablePath), fileName);
            if (m_exePath == null)
                return null;
            try
            {
                content = File.ReadAllLines(m_exePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return content;
        }

        /// <summary>
        /// Đọc mảng file
        /// </summary>
        /// <param name="fileName">Tên file</param>
        /// <returns></returns>
        public static void WriteArrayFile(string fileName, string[] content)
        {
            m_exePath = UtilFile.GetFilePath(Path.GetDirectoryName(Application.ExecutablePath), fileName);
            if (m_exePath == null)
            {
                var file = File.Create(m_exePath);
                file.Close();
            }
            try
            {
                File.WriteAllLines(m_exePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Ghi file log
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="fileName"></param>
        public static void LogWrite(string logMessage, string fileName)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(m_exePath + "\\" + fileName))
            {
                var file = File.Create(m_exePath + "\\" + fileName);
                file.Close();
            }


            using (StreamWriter w = File.AppendText(m_exePath + "\\" + fileName))
            {
                try
                {
                    AppendLog(logMessage, w);
                    w.Close();
                }
                catch (Exception ex)
                {
                    w.Close();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Đọc file log
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string LogRead(string fileName)
        {
            string content = string.Empty;
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(m_exePath + "\\" + fileName))
            {
                var file = File.Create(m_exePath + "\\" + fileName);
                file.Close();
            }

            using (StreamReader r = new StreamReader(m_exePath + "\\" + fileName))
            {
                try
                {
                    content = r.ReadToEnd();
                    r.Close();
                }
                catch (Exception ex)
                {
                    r.Close();
                    Console.WriteLine(ex.Message);
                }
            }
            return content;
        }

        /// <summary>
        /// Xóa nội dung file log
        /// </summary>
        /// <param name="fileName"></param>
        public static void LogClear(string fileName)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(m_exePath + "\\" + fileName))
            {
                var file = File.Create(m_exePath + "\\" + fileName);
                file.Close();
            }

            File.WriteAllText(m_exePath + "\\" + fileName, "");
        }

        private static void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.ToString("dd-MM-yyyy"));
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        public static string GetFilePath(String dir, String filename)
        {
            string mf = GetFileNameForMachine(filename);
            return LocalSettingFileReader.FindFile(dir, mf, filename);
        }

        private static string GetFileNameForMachine(string baseFileName)
        {
            string ret = baseFileName;
            int ix = ret.LastIndexOf(".");
            if (ix < 0) ix = ret.Length;
            string mn = ("_" + Environment.MachineName).ToLower();
            ret = ret.Insert(ix, mn);
            return ret;
        }
    }

    public class ItemList
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
