using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Common;
using QLTK.Service.Model;
using RestSharp;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Service.Business.Service
{
    public class DesignStructure
    {
        private ApiUtil apiUtil = new ApiUtil();

        public void CreateDesignStructure(DesignStructureCreateModel designStructureCreateModel)
        {
            try
            {
                string rootPath = Constants.Disk_Design;
                ResultModel resultModel = new ResultModel();
                string group = string.Empty;
                string parentGroup = string.Empty;
                string pathOpen = string.Empty;

                bool isDiskExist = false;
                if (Directory.Exists(rootPath))
                {
                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                    foreach (DriveInfo d in allDrives)
                    {
                        if (d.IsReady == true && d.Name.Equals("D:\\"))
                        {
                            rootPath = d.Name;
                            isDiskExist = true;
                            break;
                        }
                    }
                }

                if (isDiskExist == false)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0005);
                }

                string folderPath = string.Empty;
                DesignStructureModel model;
                for (int i = 0; i < designStructureCreateModel.ListDesignStructure.Count; i++)
                {
                    model = designStructureCreateModel.ListDesignStructure[i];

                    folderPath = (rootPath + @"" + model.Path).Replace("code", designStructureCreateModel.ObjectCode).Replace("manhomcha", designStructureCreateModel.ParentGroupCode).Replace("manhom", designStructureCreateModel.ObjectGroupCode);

                    DirectoryUtil.CreateFolder(folderPath);

                    if (model.listFile != null)
                    {
                        CreateFile(model.listFile, folderPath, designStructureCreateModel.ApiUrl, designStructureCreateModel.ObjectCode, designStructureCreateModel.ObjectName, designStructureCreateModel.CreateBy);
                    }

                    if (model.IsOpen)
                    {
                        pathOpen = folderPath;
                    }
                }

                if (!string.IsNullOrEmpty(pathOpen))
                {
                    System.Diagnostics.Process.Start(pathOpen);
                }
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
        }

        private void CreateFile(List<DesignStructureFileModel> listFile, string folderPath, string apiUrl, string objectCode, string objectName, string createBy)
        {
            string fileName = string.Empty;
            foreach (var item in listFile)
            {
                fileName = item.Name.Replace("code", objectCode);
                if (item.IsTemplate)
                {
                    if (!apiUtil.DownloadFile(apiUrl, item.Path, folderPath, fileName))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0006, fileName);
                    }

                    if (item.IsInsertData)
                    {
                        ExportExcel(folderPath + "/" + fileName, objectName, objectCode, createBy);
                    }
                }
            }
        }

        private void ExportExcel(string pathFile, string objectName, string objectCode, string createBy)
        {
            try
            {
                string pathExport = string.Empty;

                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(pathFile);
                IWorksheet sheet = workbook.Worksheets[0];

                sheet.Replace("<modulename>", objectName, ExcelFindOptions.MatchCase);
                sheet.Replace("<modulecode>", objectCode, ExcelFindOptions.MatchCase);
                sheet.Replace("<createby>", createBy, ExcelFindOptions.MatchCase);

                workbook.Save();
                workbook.Close();
                excelEngine.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
