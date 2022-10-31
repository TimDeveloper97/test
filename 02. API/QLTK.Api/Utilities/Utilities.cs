using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;

namespace NTS.Api
{
    public class Utilities
    {
        public static string GetMessageForeignKey(Exception ex, string message)
        {
            var sqlException = ex.GetBaseException() as SqlException;

            if (sqlException != null)
            {
                if (sqlException.Errors.Count > 0)
                {
                    switch (sqlException.Errors[0].Number)
                    {
                        case 547:
                            return message;
                        default:
                            return "Lỗi hệ thống: " + ex.Message;
                    }
                }
                else
                {
                    return "Lỗi hệ thống: " + ex.Message;
                }
            }
            else
            {
                return "Lỗi hệ thống: " + ex.Message;
            }
        }

        public static string GetFolderExport()
        {
            string pathExport = HostingEnvironment.MapPath("~/Template/Export");
            if (!Directory.Exists(pathExport))
            {
                Directory.CreateDirectory(pathExport);
            }
            //Xóa các file export tạo từ ngày hôm trước
            FileInfo fileDelete;
            foreach (var itemFile in Directory.GetFiles(pathExport))
            {
                fileDelete = new FileInfo(itemFile);
                if (fileDelete.Exists && fileDelete.CreationTime.Date < DateTime.Now.Date)
                {
                    fileDelete.Delete();
                }
            }

            return pathExport;
        }
    }    
}