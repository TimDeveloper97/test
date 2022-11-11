using BMS;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.WindowsAPICodePack.Shell;
using NTS.Common;
using NTS.Common.Logs;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Service;
using QLTK.Service.Common;
using QLTK.Service.Model;
using QLTK.Service.Model.ClassRoom;
using QLTK.Service.Model.Downloads;
using QLTK.Service.Model.Modules;
using QLTK.Service.Model.Products;
using QLTK.Service.Model.Solution;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Service.Business.Hubs
{
    [HubName("MyHub")]
    public class MyHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool status)
        {
            return base.OnDisconnected(status);
        }

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        #region Thiết kế module

         /// <summary>
        /// Download file thiết kế module
        /// </summary>
        /// <param name="model"></param>
        public async Task DownloadFileModuleDesignDocument(DownloadFileModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ModuleDesignDocumentNew dowloadFileGoogle = new ModuleDesignDocumentNew();
                resultModel = await dowloadFileGoogle.DownloadFileDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download file " + model.Name + " không thành công!";
            }

            Clients.Caller.downloadFileModuleDesignDocument(resultModel);
        }

        /// <summary>
        /// Download folder thiết kế module
        /// </summary>
        /// <param name="model"></param>
        public async Task DownloadFolderModuleDesignDocument(DownloadFolderModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ModuleDesignDocumentNew dowloadFileGoogle = new ModuleDesignDocumentNew();
                resultModel = await dowloadFileGoogle.DownloadFolderDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download folder không thành công!";
            }

            Clients.Caller.downloadFolderModuleDesignDocument(resultModel);
        }

        /// <summary>
        /// Upload tài liệu thiết kế module
        /// </summary>
        /// <param name="model"></param>
        public async Task UploadModuleDesignDocument(UploadFolderModel model)
        {
            UploadFolderResultModel rs = new UploadFolderResultModel();
            try
            {
                ModuleDesignDocumentNew moduleDesignDocument = new ModuleDesignDocumentNew();
                rs = await moduleDesignDocument.UploadFolderDesignDocument(model);
                Clients.Caller.uploadFolder(rs);
            }
            catch (NTSException ex)
            {
                NtsLog.LogError(ex);
                rs.LstError.Add(ex.Message);
                Clients.Caller.uploadFolder(rs);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                rs.LstError.Add("Có lỗi trong quá trình xử lý!");
                Clients.Caller.uploadFolder(rs);
            }
        }

        /// <summary>
        /// Download tài liệu thiết kế module
        /// </summary>
        /// <param name="model"></param>
        public void DownloadModuleDesignDocumentShare(DownloadModuleDesignModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ModuleDesignDocument dowloadFileGoogle = new ModuleDesignDocument();
                var data = dowloadFileGoogle.DownloadDesignDocumentShare(model);

                if (data.Count > 0)
                {
                    resultModel.StatusCode = ResultModel.StatusCodeError;
                    resultModel.Data = data;
                }
                else
                {
                    resultModel.StatusCode = ResultModel.StatusCodeSuccess;
                }
            }
            catch (NTSException ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
                NtsLog.LogError(ex);
            }
            catch (Exception ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download không thành công";
                NtsLog.LogError(ex);
            }

            Clients.Caller.downloadModuleDesignDocumentShare(resultModel);
        }

        /// <summary>
        /// Download tài liệu sản phẩm của Dự Án
        /// </summary>
        /// <param name="model"></param>
        public void DownloadProductDocuments(DownloadProjectProductModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                DownloadProjectProduct _business = new DownloadProjectProduct();
                var rs = _business.DownloadProductDocuments(model);

                if (!rs)
                {
                    resultModel.StatusCode = ResultModel.StatusCodeError;
                }
                else
                {
                    resultModel.StatusCode = ResultModel.StatusCodeSuccess;
                }
            }
            catch (NTSException ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
                NtsLog.LogError(ex);
            }
            catch (Exception ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download không thành công";
                NtsLog.LogError(ex);
            }

            Clients.Caller.downloadProductDocuments(resultModel);
        }

        /// <summary>
        /// Download tài liệu vật tư
        /// </summary>
        /// <param name="model"></param>
        public async Task DownloadMaterialDesignDocumentShare(DownloadMaterialDesignModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ModuleDesignDocument dowloadFileGoogle = new ModuleDesignDocument();
                var data = await dowloadFileGoogle.DownloadMaterialDesignDocumentShare(model);

                if (data.Count > 0)
                {
                    resultModel.StatusCode = ResultModel.StatusCodeError;
                    resultModel.Data = data;
                }
                else
                {
                    resultModel.StatusCode = ResultModel.StatusCodeSuccess;
                }
            }
            catch (NTSException ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
                NtsLog.LogError(ex);
            }
            catch (Exception ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download không thành công";
                NtsLog.LogError(ex);
            }

            Clients.Caller.downloadModuleDesignDocumentShare(resultModel);
        }


        /// <summary>
        /// Download tài liệu thiết kế vật tư
        /// </summary>
        /// <param name="model"></param>
        /// 

        #endregion

        #region Thiết kế thiết bị 

        public void UploadProductDesignDocument(UploadProductFolderModel model)
        {
            UploadFolderProductResultModel rs = new UploadFolderProductResultModel();
            try
            {
                ProductDesignDocumentBusiness productDesignDocumentBusiness = new ProductDesignDocumentBusiness();
                rs = productDesignDocumentBusiness.UploadFolderDesignDocument(model);

                Clients.Caller.uploadFolderProduct(rs);
            }
            catch (NTSException ex)
            {
                NtsLog.LogError(ex);
                rs.IsUploadSuccess = false;
                rs.LstError.Add(ex.Message);
                Clients.Caller.uploadFolderProduct(rs);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                rs.IsUploadSuccess = false;
                rs.LstError.Add("Có lỗi trong quá tringh xử lý!");
                Clients.Caller.uploadFolderProduct(rs);
            }
        }

        /// <summary>
        /// Download file thiết kế thiết bị
        /// </summary>
        /// <param name="model"></param>
        public void DownloadFileProductDesignDocument(DownloadFileModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ProductDesignDocumentBusiness productDesignDocumentBusiness = new ProductDesignDocumentBusiness();
                resultModel = productDesignDocumentBusiness.DownloadFileDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download file " + model.Name + " không thành công!";
            }

            Clients.Caller.downloadFileProductDesignDocument(resultModel);
        }

        /// <summary>
        /// Download folder thiết kế module
        /// </summary>
        /// <param name="model"></param>
        public void DownloadFolderProductDesignDocument(DownloadFolderModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ProductDesignDocumentBusiness productDesignDocumentBusiness = new ProductDesignDocumentBusiness();
                resultModel = productDesignDocumentBusiness.DownloadFolderDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download folder không thành công!";
            }

            Clients.Caller.downloadFolderProductDesignDocument(resultModel);
        }

        #endregion

        #region Thiết kế phòng học 

        public void UploadClassRoomDesignDocument(UploadClassRoomFolderModel model)
        {
            UploadFolderClassRoomResultModel rs = new UploadFolderClassRoomResultModel();
            try
            {
                ClassRoomDesignDocumentBusiness classRoomDesignDocumentBusiness = new ClassRoomDesignDocumentBusiness();
                rs = classRoomDesignDocumentBusiness.UploadFolderDesignDocument(model);

                Clients.Caller.uploadFolderClassRoom(rs);
            }
            catch (NTSException ex)
            {
                NtsLog.LogError(ex);
                rs.IsUploadSuccess = false;
                rs.LstError.Add(ex.Message);
                Clients.Caller.uploadFolderClassRoom(rs);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                rs.IsUploadSuccess = false;
                rs.LstError.Add("Có lỗi trong quá tringh xử lý!");
                Clients.Caller.uploadFolderClassRoom(rs);
            }
        }

        /// <summary>
        /// Download file thiết kế phòng học
        /// </summary>
        /// <param name="model"></param>
        public void DownloadFileClassRoomDesignDocument(DownloadFileModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ClassRoomDesignDocumentBusiness classRoomDesignDocumentBusiness = new ClassRoomDesignDocumentBusiness();
                resultModel = classRoomDesignDocumentBusiness.DownloadFileDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download file " + model.Name + " không thành công!";
            }

            Clients.Caller.downloadFileClassRoomDesignDocument(resultModel);
        }

        /// <summary>
        /// Download folder thiết kế module
        /// </summary>
        /// <param name="model"></param>
        public void DownloadFolderClassRoomDesignDocument(DownloadFolderModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                ClassRoomDesignDocumentBusiness classRoomDesignDocumentBusiness = new ClassRoomDesignDocumentBusiness();
                resultModel = classRoomDesignDocumentBusiness.DownloadFolderDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download folder không thành công!";
            }

            Clients.Caller.downloadFolderClassRoomDesignDocument(resultModel);
        }

        #endregion

        #region Thiết kế giải pháp

        public void UploadSolutionDesignDocument(UploadSolutionFolderModel model)
        {
            UploadFolderSolutionResultModel rs = new UploadFolderSolutionResultModel();
            try
            {
                SolutionDesignDocumentBusiness solutionDesignDocumentBusiness = new SolutionDesignDocumentBusiness();
                rs = solutionDesignDocumentBusiness.UploadFolderDesignDocument(model);

                Clients.Caller.uploadFolderSolution(rs);
            }
            catch (NTSException ex)
            {
                NtsLog.LogError(ex);
                rs.IsUploadSuccess = false;
                rs.LstError.Add(ex.Message);
                Clients.Caller.uploadFolderSolution(rs);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                rs.IsUploadSuccess = false;
                rs.LstError.Add("Có lỗi trong quá tringh xử lý!");
                Clients.Caller.uploadFolderSolution(rs);
            }
        }

        /// <summary>
        /// Download file thiết kế giải pháp
        /// </summary>
        /// <param name="model"></param>
        public void DownloadFileSolutionDesignDocument(DownloadFileModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                SolutionDesignDocumentBusiness solutionDesignDocumentBusiness = new SolutionDesignDocumentBusiness();
                resultModel = solutionDesignDocumentBusiness.DownloadFileDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download file " + model.Name + " không thành công!";
            }

            Clients.Caller.downloadFileSolutionDesignDocument(resultModel);
        }

        /// <summary>
        /// Download folder thiết kế giải phamps
        /// </summary>
        /// <param name="model"></param>
        public void DownloadFolderSolutionDesignDocument(DownloadFolderModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                SolutionDesignDocumentBusiness solutionDesignDocumentBusiness = new SolutionDesignDocumentBusiness();
                resultModel = solutionDesignDocumentBusiness.DownloadFolderDesignDocument(model);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download folder không thành công!";
            }

            Clients.Caller.downloadFolderSolutionDesignDocument(resultModel);
        }

        #endregion

        #region Choose folder, file
        /// <summary>
        /// Lấy thư mục
        /// </summary>
        /// <param name="model"></param>
        public void GetFolder(DataFolderModel model)
        {
            FileService service = new FileService();
            SelectForderModel data = new SelectForderModel();
            if (string.IsNullOrEmpty(model.Id))
            {
                data.ListForder = service.GetRoot();
            }
            else
            {
                data.ListForder = service.GetChild(model.Id, model.ListFolder);

            }
            Clients.Caller.sendListFolder(data);
        }

        /// <summary>
        /// Lấy thư mục select
        /// </summary>
        /// <param name="model"></param>
        public void GetSelectFolder(TestDesignStructureModel model)
        {
            FileService service = new FileService();
            TestDesign testDesign = new TestDesign();
            SelectForderModel data = new SelectForderModel();
            data.Path = testDesign.GetPathModule(model);
            data.ListForder = service.GetFoder(data.Path);
            Clients.Caller.sendListFolder(data);
        }

        /// <summary>
        /// Lấy thư mục thiết bị
        /// </summary>
        /// <param name="model"></param>
        public void GetSelectFolderProduct(UploadProductFolderModel model)
        {
            FileService service = new FileService();
            TestDesign testDesign = new TestDesign();
            SelectForderModel data = new SelectForderModel();
            data.Path = testDesign.GetPathProduct(model);
            data.ListForder = service.GetFoder(data.Path);
            Clients.Caller.sendListFolder(data);
        }

        public void GetSelectFolderClassRoom(UploadClassRoomFolderModel model)
        {
            FileService service = new FileService();
            TestDesign testDesign = new TestDesign();
            SelectForderModel data = new SelectForderModel();
            data.Path = testDesign.GetPathClassRoom(model);
            data.ListForder = service.GetFoder(data.Path);
            Clients.Caller.sendListFolder(data);
        }

        public void GetSelectFolderSolution(UploadSolutionFolderModel model)
        {
            FileService service = new FileService();
            TestDesign testDesign = new TestDesign();
            SelectForderModel data = new SelectForderModel();
            data.Path = testDesign.GetPathSolution(model);
            data.ListForder = service.GetFoder(data.Path);
            Clients.Caller.sendListFolder(data);
        }

        public void GetFolderFile(DataFolderModel model)
        {
            FileService service = new FileService();
            List<FolderModel> list = new List<FolderModel>();
            if (string.IsNullOrEmpty(model.Id))
            {
                list = service.GetRoot();
            }
            else
            {
                list = service.GetChild(model.Id, model.ListFolder);
            }
            IList<FileInfo> listFile = new List<FileInfo>();
            listFile = service.GetChildFiles(model.Id);

            FolderModel folderModel;
            if (listFile.Count > 0)
            {
                foreach (var item in listFile)
                {
                    folderModel = new FolderModel();
                    folderModel.Id = item.FullName;
                    folderModel.ParentId = model.Id;
                    folderModel.Path = item.FullName;
                    folderModel.Name = item.FullName.Replace(model.Id, "").Replace(@"\", "");
                    list.Add(folderModel);
                }
            }
            Clients.Caller.sendListFolderFile(list);
        }
        #endregion

        #region Tạo cấu trúc thư mục
        /// <summary>
        /// Tạo cấu trúc thư mục
        /// </summary>
        /// <param name="model"></param>
        public void CreateFolder(DesignStructureCreateModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                DesignStructure service = new DesignStructure();
                service.CreateDesignStructure(model);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.sendCreateFolder(resultModel);
        }
        #endregion

        #region Kiểm tra dữ liệu thiết kế
        public void CheckElectronic(UploadFolderModel model)
        {
            CheckElectronicModel rs = new CheckElectronicModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                rs = testDesign.CheckElectronic(model);
                Clients.Caller.checkElectronic(rs);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                rs.LstError.Add("Có lỗi trong quá trình xử lý!");
                Clients.Caller.checkElectronic(rs);
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu thiết kế cơ khí
        /// </summary>
        /// <param name="path"></param>
        public void CheckMechanical(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                resultModel.Data = testDesign.CheckDocumentDesign(modelTest);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listSoftDesgin(resultModel);
        }

        /// <summary>
        /// Tải dữ liệu bản ứng Kiểm tra dữ liệu thết kế
        /// </summary>
        /// <param name="path"></param>
        public void LoadHardDesign(string path)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                //resultModel.Data = testDesign.LoadHardDesign(path);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listHardDesgin(resultModel);
        }

        /// <summary>
        /// Tab Chuẩn danh mục vật tư
        /// </summary>
        /// <param name="path"></param>
        public void LoadDMVT(string path)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                resultModel.Data = testDesign.LoadDMVT(path);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listDMVT(resultModel);
        }

        /// <summary>
        /// CheckFile3D
        /// </summary>
        /// <param name="Link"></param>
        public void CheckFile3D(string Link)
        {
            try
            {
                //ShellFile shellFile = ShellFile.FromFilePath("D:\\AT123.ipt");
                //Bitmap shellThumb = shellFile.Thumbnail.ExtraLargeBitmap;
                //shellThumb.Save("D:\\111.jpg", ImageFormat.Jpeg);
                //IPTDetail.LoadData(Link);

                Thread t = new Thread(() => IPTDetail.LoadData(Link));
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.CheckFile3D("Có lỗi trong quá trình xử lý!" + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra cấu trúc thiết kế Tab3
        /// </summary>
        /// <param name="path"></param>
        public void GetListErrorDesignStructure(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                resultModel.Data = testDesign.CheckStructuralDesign(modelTest);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listError(resultModel);
        }

        public void ExportReportNot3D(ReportTestDesignModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.ExportReportNot3D(model);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.exportReportNot3D(resultModel);
        }

        public void ExportExcelReportDesignStructure(ReportTestDesignModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.ExportExcelTestDesignStructure(model);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.exportExcelTestDesignStructure(resultModel);
        }

        public void CheckDesignCAD(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                List<string> listError = new List<string>();
                resultModel.Data = testDesign.CheckCAD(modelTest, listError);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listErrorCAD(resultModel);
        }

        public void DownLoadAllCAD(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.DownloadAll(modelTest);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.downloadAllCAD(resultModel);
        }

        /// <summary>
        /// KIểm tra bản cứng bản mềm CAD
        /// </summary>
        /// <param name="modelTest"></param>
        public void CheckSoftHardCAD(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                List<string> listError = new List<string>();
                resultModel.Data = testDesign.CheckSoftHardCAD(modelTest, listError);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.checkSoftHardCAD(resultModel);
        }

        public void DownloadHardCAD(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.DownloadHardCAD(modelTest);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.downloadCAD(resultModel);
        }

        public void CheckFileDMVT(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                List<string> listError = new List<string>();
                resultModel.Data = testDesign.CheckFileDMVT(modelTest, listError);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listDMVTResult(resultModel);
        }

        public void CheckElectric(UploadFolderModel model)
        {
            ElectricModel rs = new ElectricModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                rs = testDesign.CheckFileElectric(model);
                Clients.Caller.listFoderElec(rs);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                rs.LstError.Add(ex.Message);
                Clients.Caller.listFoderElec(rs);
            }

        }

        public void ExportResuleDMVT(TestDesignStructureModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.ExportResutlDMVT(model);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.exportResultDMVT(resultModel);
        }

        public void CheckMAT(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                resultModel.Data = testDesign.CheckMAT(modelTest);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listErrorMAT(resultModel);
        }

        public void CheckJGS(TestDesignStructureModel modelTest)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                resultModel.Data = testDesign.CheckIGS(modelTest);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }

            Clients.Caller.listErrorIGS(resultModel);
        }

        public void DeleteListFile(List<string> ListPath)
        {
            try
            {
                ScanFile scanFile = new ScanFile();
                foreach (var item in ListPath)
                {
                    scanFile.DeleteListFile(item);
                }
                Clients.Caller.getDeleteList(true);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.getDeleteList(false);
            }
        }

        public void DeleteListFileFolder(List<string> ListPath)
        {
            try
            {
                ScanFile scanFile = new ScanFile();

                ListPath = ListPath.OrderByDescending(r => r.Length).ToList();
                foreach (var item in ListPath)
                {
                    scanFile.DeleteFile(item);
                }
                Clients.Caller.getDeleteList(true);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.getDeleteList(false);
            }
        }

        public void LoadHardDesign(TestDesignStructureModel modelTest)
        {
            string link = string.Empty;
            try
            {
                TestDesign testDesign = new TestDesign();
                link = testDesign.GetPathModule(modelTest);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            Clients.Caller.linkModule(link);
        }
        public void CompareFileIGS(TestDesignStructureModel modelTest)
        {
            List<ProductModuleModel> list = new List<ProductModuleModel>();
            try
            {
                TestDesign testDesign = new TestDesign();
                list = testDesign.CompareFileIGS(modelTest);

            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            Clients.Caller.compareFileIGS(list);
        }
        #endregion

        #region Scan file
        public void ScanFileJPG(ScanFileModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                var check = model.CheckDelete;
                ScanFile scanFile = new ScanFile();
                scanFile.CheckCode(model);
                scanFile.ScanFileJPG(model);
                if (check && model.CheckDelete)
                {
                    model.CheckDelete = false;
                }
                resultModel.Data = model.CheckDelete;
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }
            Clients.Caller.scanfileJPG(resultModel);
        }

        public void ScanFilePDF(ScanFileModel model)
        {
            try
            {
                ScanFile scanFile = new ScanFile();
                scanFile.CheckCode(model);
                scanFile.ScanFilePDF(model);
                Clients.Caller.scanfilePDF(true);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.scanfilePDF(false);
            }
        }

        public void Clear(ScanFileModel model)
        {
            try
            {
                ScanFile scanFile = new ScanFile();
                scanFile.CheckCode(model);
                Clients.Caller.clearCode(scanFile);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.clearCode(false);
            }
        }

        public void DeleteFileScan(string Path)
        {
            try
            {
                ScanFile scanFile = new ScanFile();
                scanFile.DeleteFile(Path);
                Clients.Caller.getDelete(true);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.getDelete(false);
            }
        }

        public void GetBase64(string Path)
        {
            try
            {
                ScanFile scanFile = new ScanFile();
                var link = scanFile.GetBase64(Path);
                Clients.Caller.getBase64(link);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.getBase64(false);
            }
        }
        public void MoveFile(MoveFileModel model)
        {
            try
            {
                ScanFile scanFile = new ScanFile();
                scanFile.MoveFile(model);
                Clients.Caller.getMoveFile(true);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.getMoveFile(false);
            }
        }
        #endregion

        #region Xuất biểu mẫu

        public void DowloadTemplateToFolder(TestDesignStructureModel model)
        {
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.GenaralTemplate(model);
                Clients.Caller.generalDownload(true);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.generalDownload(false);
            }
        }

        public void DownloadFile3DMaterial(TestDesignStructureModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.DownloadFile3DMaterial(model);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }
            Clients.Caller.downloadAFile(resultModel);
        }

        public void craeteMechanicalProfile(MechanicalRecordsModel model)
        {
            ResultModel resultModel = new ResultModel();

            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.CraeteMechanicalProfile(model);

                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
                Clients.Caller.onCraeteMechanicalProfile(resultModel);
            }
            catch (NTSException ex)
            {
                NtsLog.LogError(ex);
                resultModel.Message = ex.Message;
                resultModel.StatusCode = ResultModel.StatusCodeError;
                Clients.Caller.onCraeteMechanicalProfile(resultModel);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.Message = "Tạo hồ sơ lỗi!";
                resultModel.StatusCode = ResultModel.StatusCodeError;
                Clients.Caller.onCraeteMechanicalProfile(resultModel);
            }
        }

        public void GetDMVT(TestDesignStructureModel model)
        {
            try
            {
                TestDesign testDesign = new TestDesign();
                var materials = testDesign.GetDMVT(model);
                Clients.Caller.generalDMVT(materials);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.generalDownload(false);
            }
        }

        public void GetUserNamePC(string ad)
        {
            try
            {
                TestDesign testDesign = new TestDesign();
                string userNamePC = testDesign.GetUserNamePC();
                Clients.Caller.getUserNamePC(userNamePC);

            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                Clients.Caller.getUserNamePC(false);
            }
        }

        #endregion

        public void DownloadFileModuleDocument(DownloadFileDesignDocumentModel model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                TestDesign testDesign = new TestDesign();
                testDesign.DownloadFileModuleDocument(model);
                resultModel.StatusCode = ResultModel.StatusCodeSuccess;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
            }
            Clients.Caller.downloadFileModuleDesignDocument(resultModel);
        }

        /// <summary>
        /// Download tài liệu 3D của vật tư
        /// </summary>
        /// <param name="model"></param>
        public void DownloadMaterialDocument3Ds(DowloadMaterialDocument model)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                DownloadMaterialDocument3D _business = new DownloadMaterialDocument3D();
                var rs = _business.DownloadMaterialDocument3Ds(model);

                if (!rs.IsSuccess)
                {
                    resultModel.StatusCode = ResultModel.StatusCodeError;
                }
                else
                {
                    resultModel.StatusCode = ResultModel.StatusCodeSuccess;
                    resultModel.Data = rs.LinkExcel;
                }
            }
            catch (NTSException ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = ex.Message;
                NtsLog.LogError(ex);
            }
            catch (Exception ex)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download không thành công";
                NtsLog.LogError(ex);
            }
            Clients.Caller.downloadFileMaterialDocument3Ds(resultModel);
        }

    }
}
