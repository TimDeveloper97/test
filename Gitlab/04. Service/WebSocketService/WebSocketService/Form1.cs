using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTK.Service.Business.Service;
using QLTK.Service.Business.Hubs;
using System.Collections;

using QLTK.Service.Business.Model;
using QLTK.Service.Model;
using QLTK.Service.Business.WebServer;
using BMS;
using System.Threading;

namespace WebSocketService
{
    public partial class Form1 : Form
    {
        FileService fileService = new FileService();
        GoogleApi googleApi = new GoogleApi();
        private IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8080";

        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Starting server...");
            btnStart.Enabled = false;
            Task.Run(() => StartServer());
        }

        private void StartServer()
        {
            try
            {
                NTSWebServer nTSWebServer = new NTSWebServer();
                nTSWebServer.StartServer();
            }
            catch (TargetInvocationException)
            {
                MessageBox.Show("Server failed to start. A server is already running on " + ServerURI);
                this.Invoke((Action)(() => btnStart.Enabled = true));
                return;
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            hubContext.Clients.All.sendMessage("Hello world!");
        }

        private void BtnListFile_Click(object sender, EventArgs e)
        {
            googleApi.ListFile();
        }

        private void BtnDownloadFile_Click(object sender, EventArgs e)
        {
            string Id = "1JXtSY97UrFGY5GwkgpZ6P9uaBsJpVJ8A";
            string root = @"C:\Download";
            googleApi.DownloadFile(Id, "abc.doc", root);
        }

        private void BtnDownloadFolder_Click(object sender, EventArgs e)
        {
            string Id = "17CBUXGrC6VavKZwjhBrAGlB2_pGHDlQi";
            googleApi.DownloadFolder(Id, "");
        }

        private void BtnChooseFile_Click(object sender, EventArgs e)
        {
            var list = fileService.GetRoot();
        }

        private void BtnUploadFolder_Click(object sender, EventArgs e)
        {
            UploadFolderModel model = new UploadFolderModel();
            model.Path = "C:\\Users\\minh_\\Desktop\\Upload";
            googleApi.UploadFolder(model);
        }
        string _initPath;
        string _duongDanChinh;
        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string productCode = "A0101";
            //    int type = 2;

            //    if (Directory.Exists(@"D:\"))
            //    {
            //        if (type == 0)//cơ khí
            //        {
            //            _initPath = "D:\\Thietke.Ck\\TPAD." + productCode.ToUpper().Substring(0, 1);
            //        }
            //        if (type == 1)//điện
            //        {
            //            _initPath = "D:\\Thietke.Dn\\TPAD." + productCode.ToUpper().Substring(0, 1);
            //        }
            //        if (type == 2)//điện tử
            //        {
            //            _initPath = "D:\\Thietke.Dt\\PCB." + productCode.ToUpper().Substring(0, 1);
            //        }
            //    }

            //    var listDesignStructure = (from a in db.DesignStructures.AsNoTracking()
            //                               where a.Type == type
            //                               select new DesignStructureModel
            //                               {
            //                                   Name = a.Name,
            //                                   //Id = a.Id,
            //                                   //ParentId = a.ParentId,
            //                                   Type = a.Type,
            //                                   ParentPath = a.ParentPath,
            //                                   Path = a.Path,
            //                               }).ToList();

            //    for (int i = 0; i < listDesignStructure.Count; i++)
            //    {
            //        DesignStructureModel model = (DesignStructureModel)listDesignStructure[i];

            //        string directtion = "";
            //        if (model.Path.Contains("code"))
            //        {
            //            if (type == 0 || type == 1)//cơ khí
            //            {
            //                directtion = (_initPath + @"\" + model.Path).Replace("code", "TPAD." + productCode);
            //            }
            //            if (type == 2)//điện tử
            //            {
            //                directtion = (_initPath + @"\" + model.Path).Replace("code", productCode);
            //            }
            //        }
            //        else
            //            directtion = (_initPath + @"\" + model.Path);
            //        if (i == 0)
            //        {
            //            _duongDanChinh = directtion;
            //        }
            //        Directory.CreateDirectory(directtion);
            //    }



            //    if (type == 0)//cơ khí
            //    {
            //        var module = db.Modules.Where(a => a.Code.Equals("TPAD." + productCode.ToUpper())).FirstOrDefault();
            //        #region Danh muc vat tu
            //        try
            //        {

            //            File.Copy(Application.StartupPath + "\\Templates\\PhongThietKe\\VT.Code.xlsm",
            //           _duongDanChinh + "\\VT.TPAD." + productCode + ".xlsm", true);

            //            //Excel.Application objXLApp = default(Excel.Application);
            //            //Excel.Workbook objXLWb = default(Excel.Workbook);
            //            //Excel.Worksheet objXLWs = default(Excel.Worksheet);

            //            try
            //            {
            //                //objXLApp = new Excel.Application();
            //                //objXLApp.Workbooks.Open(_duongDanChinh + "\\VT.TPAD." + productCode + ".xlsm");
            //                //objXLWb = objXLApp.Workbooks[1];
            //                //objXLWs = (Excel.Worksheet)objXLWb.Worksheets[1];

            //                //objXLWs.Cells[4, 3] = "TEST"; //Global.AppUserName.ToUpper();
            //                //objXLApp.Cells[3, 3] = module.Name.ToUpper();
            //                //objXLApp.Cells[3, 10] = "Mã: TPAD." + productCode.ToUpper();
            //            }
            //            catch
            //            {
            //            }
            //            finally
            //            {
            //                //objXLApp.ActiveWorkbook.Save();
            //                //objXLApp.Workbooks.Close();
            //                //objXLApp.Quit();
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            MessageBox.Show("Không tạo được danh mục vật tư do module [TPAD." + productCode.ToUpper() + "] chưa có trên nguồn",
            //                     "[TÂN PHÁT] - Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        #endregion

            //        #region Thư mục Doc
            //        string moduleCode = "TPAD." + productCode;
            //        string toFolder = string.Format(@"D:\Thietke.Ck\{0}\{1}.Ck\DOC.{1}\", moduleCode.Substring(0, 6), moduleCode);
            //        string fromFolder = Application.StartupPath + "/Templates/PhongThietKe/DOC.TPAD.XYYYY";
            //        string[] listFile = Directory.GetFiles(fromFolder);
            //        foreach (string filePath in listFile)
            //        {
            //            string fileName = Path.GetFileNameWithoutExtension(filePath);
            //            string extension = Path.GetExtension(filePath);
            //            try
            //            {
            //                File.Copy(filePath, toFolder + fileName + moduleCode + extension, true);
            //            }
            //            catch
            //            {
            //            }
            //        }
            //        #endregion

            //        #region Bảng kiểm tra phương án
            //        string path = "D:\\Thietke.Ck\\" + moduleCode.Substring(0, 6) + "\\" + moduleCode + ".Ck\\DOC." + moduleCode;
            //        string filePathSource = Application.StartupPath + "\\Templates\\KTPA.docm";
            //        string filePathKTPA = path + "\\KTPA." + moduleCode + ".docm";

            //        if (!Directory.Exists(path))
            //        {
            //            Directory.CreateDirectory(path);
            //        }

            //        File.Copy(filePathSource, filePathKTPA, true);

            //        string groupCode = moduleCode.Substring(0, 6).ToLower();
            //        string groupName = db.ModuleGroups.Where(a => a.Id.Equals(module.ModuleGroupId)).Select(b => b.Name).FirstOrDefault(); //((ModuleGroupModel)ModuleGroupBO.Instance.FindByCode(groupCode)).Name.ToLower();
            //        string userName = "TEST"; //Global.AppUserName.ToLower();

            //        //string parametter = TextUtils.ConvertUnicode(groupName + "$" + groupCode + "$" + module.Name.ToLower() + "$" + moduleCode.ToLower()
            //        //    + "$" + userName, 0).ToUpper();
            //        //TextUtils.RunMacroInWord(filePathKTPA, "Macro1", parametter);
            //        #endregion
            //    }
            //    MessageBox.Show("Tạo cấu trúc thiết kế thành công!", "[TÂN PHÁT] - Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    System.Diagnostics.Process.Start(_duongDanChinh);
            //}
            //catch (System.Exception ex)
            //{

            //}
        }

        private void TxtCheckFileExist_Click(object sender, EventArgs e)
        {
            string folderName = "TPAD.E3109.Ck";
            //var rs = googleApi.CheckFileExist(folderName,"root");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Link = "D:\\24.302.3.ipt";
            //IPTDetail.LoadData(file);
            Thread t = new Thread(() => IPTDetail.LoadData(Link));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

       
    }
}
