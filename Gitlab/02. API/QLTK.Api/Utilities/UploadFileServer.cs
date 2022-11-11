using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace NTS.Api
{
    public class UploadFileServer
    {
        /// <summary>
        /// Hàm upload file lên server
        /// </summary>
        /// <param name="file">File ảnh cần upload lên server</param>
        /// <param name="teacher">Đối tượng để lưu link ảnh sau khi upload</param>
        public static string UploadFile(HttpPostedFile file, string folderName)
        {
            string pathFolder = "fileUpload/" + folderName + "/";
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            string fileResult = string.Empty;
            // Kiểm tra folder là tên của ProjectId đã tồn tại chưa.
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            // kiểm tra size file > 0
            if (file.ContentLength > 0)
            {
                //DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" +
                string pathFile =  Path.GetFileName(file.FileName);
                file.SaveAs(pathFolderServer + pathFile);
                fileResult = pathFolder + pathFile;
            }
            return fileResult;
        }

        public static void DeleteFile(string fileName)
        {
            // Xóa folder chứa file ảnh đại diện cũ
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = HostingEnvironment.MapPath("~/" + fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagebase64"></param>
        /// <param name="folderName"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static string SaveImageBase64(string imagebase64, string folderName, string imagePath)
        {
            byte[] bytes = Convert.FromBase64String(imagebase64.Replace("data:image/png;base64,", ""));

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            string pathFolder = "fileUpload/" + folderName + "/";
            string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            string pathFile = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_Anh.png";
            image.Save(pathFolderServer + pathFile);

            if (!string.IsNullOrEmpty(imagePath))
            {
                File.Delete(HostingEnvironment.MapPath("~/" + imagePath));
            }

            return "/" + pathFolder + pathFile;
        }

        public static string GetImageBase64(string imagePath)
        {
            string pathFile = HostingEnvironment.MapPath("~/" + imagePath);

            if (!File.Exists(pathFile))
            {
                return string.Empty;
            }

            string base64 = string.Empty;

            using (Image image = Image.FromFile(pathFile))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    base64 = Convert.ToBase64String(imageBytes);
                }
            }

            return "data:image/png; base64," + base64;
        }
    }
}