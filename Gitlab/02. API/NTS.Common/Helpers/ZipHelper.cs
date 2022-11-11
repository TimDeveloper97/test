using Ionic.Zip;
using NTS.Common.Logs;
using NTS.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Helpers
{
    public static class ZipHelper
    {
        /// <summary>
        /// Zip thư mục
        /// </summary>
        /// <param name="folderPath">Thư mục zip</param>
        /// <param name="fileZipName">Tên file zip</param>
        /// <param name="folderNameInZip">Tên Thư mục zịp trong file zip</param>
        /// <returns>Tên file zip</returns>
        public static string ZipFileInFolder(string folderPath, string fileZipName, string folderNameInZip = null)
        {
            string[] filePaths = Directory.GetFiles(folderPath);

            int countErro = 0;
            string tempError = string.Empty;
            var files = new List<string>();

            if (filePaths.Length == 0)
            {
                throw NTSException.CreateInstance("Không có DMVT để tạo BOM");
            }

            string fileName = fileZipName + ".zip";
            string pathZip = Path.Combine(folderPath, fileName);
            try
            {
                string tempPath = string.Empty;

                for (int i = 0; i < filePaths.Length; i++)
                {
                    tempPath = filePaths[i];
                    if (!File.Exists(filePaths[i]))
                    {
                        countErro++;
                        tempError += "! ";
                        throw new FileNotFoundException();
                    }
                    else
                    {
                        files.Add(filePaths[i]);
                    }
                }

                using (var zip = new ZipFile())
                {
                    if (string.IsNullOrEmpty(folderNameInZip))
                    {
                        zip.AddFiles(files);
                    }
                    else
                    {
                        string pathFolderZip = Path.Combine(folderPath, folderNameInZip);
                        if (!Directory.Exists(pathFolderZip))
                        {
                            Directory.CreateDirectory(pathFolderZip);
                        }
                        zip.AddFiles(files, folderNameInZip);
                    }

                    zip.Save(pathZip);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new NTSLogException(null, ex);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }

            if (!string.IsNullOrEmpty(tempError))
            {
                NtsLog.LogError(tempError);
                throw NTSException.CreateInstance("Xuất dữ liệu không thành công!");
            }

            return fileName;
        }
    }
}
