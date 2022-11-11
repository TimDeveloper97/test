using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using QLTK.Service.Model.Api;
using NTS.Common.Logs;
using QLTK.Service.Business.Model;
using QLTK.Service.Model;
using QLTK.Service.Model.Modules;
using QLTK.Service.Model.Design3D;
using QLTK.Service.Model.Files;
using QLTK.Service.Model.MAT;
using QLTK.Service.Model.FTP;
using QLTK.Service.Model.Products;
using QLTK.Service.Model.IGS;
using QLTK.Service.Model.ClassRoom;
using QLTK.Service.Model.Materials;
using QLTK.Service.Model.Solution;
using System.Drawing;
using System.Net.Http;
using System.IO;
using iTextSharp.xmp.impl;

namespace QLTK.Service.Business.Utilities
{
    public class ApiUtil
    {
        /// <summary>
        /// Download file trên server NTSFile
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="path"></param>
        /// <param name="pathDisk"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DownloadFile(string apiUrl, string path, string pathDisk, string fileName)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("api/download/download-file", Method.GET);

            request.AddParameter("pathFile", path);
            request.AddParameter("fileName", fileName);
            request.AddParameter("keyAuthorize", ConfigurationManager.AppSettings["keyAuthorize"]);

            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }
            var data = client.DownloadData(request, true);

            if (data == null || data.Length == 0)
            {
                return false;
            }

            data.SaveAs(pathDisk + "/" + fileName);

            return true;
        }

        /// <summary>
        /// Download file trên server QLTK API
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="path"></param>
        /// <param name="pathDisk"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DownloadFiles(string apiUrl, string path, string pathDisk, string fileName)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("api/download/download-files", Method.GET);

            request.AddParameter("pathFile", path);
            request.AddParameter("fileName", fileName);
            request.AddParameter("keyAuthorize", ConfigurationManager.AppSettings["keyAuthorize"]);

            var data = client.DownloadData(request, true);

            if (data == null || data.Length == 0)
            {
                return false;
            }

            data.SaveAs(pathDisk + "/" + fileName);

            return true;
        }

        public ResultApiModel CheckFileServer(string apiUrl, string path, string moduleCode, long size, string token)
        {
            var client = new RestClient(apiUrl + "api/download/checkFileTPAUse");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("path", path);
            request.AddParameter("moduleCode", moduleCode);
            request.AddParameter("size", size);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel resultModel = new ResultApiModel();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            return resultModel;
        }

        public ResultApiModel CheckFileSize(string apiUrl, string path, string moduleCode, long size, string token)
        {
            var client = new RestClient(apiUrl + "api/download/CheckFileSize");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("path", path);
            request.AddParameter("moduleCode", moduleCode);
            request.AddParameter("size", size);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel resultModel = new ResultApiModel();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            return resultModel;
        }

        public ResultApiModel<List<CADModel>> LoadCADFile(string apiUrl, List<CADModel> listCAD, string token)
        {
            var client = new RestClient(apiUrl + "/api/download/load-cad-file");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            request.AddJsonBody(listCAD);

            ResultApiModel<List<CADModel>> resultModel = new ResultApiModel<List<CADModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<CADModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel CheckCADShare(string apiUrl, string path, string moduleCode, long size, string token)
        {
            var client = new RestClient(apiUrl + "/api/download/checkFileCADShare");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("path", path);
            request.AddParameter("moduleCode", moduleCode);
            request.AddParameter("size", size);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel resultModel = new ResultApiModel();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        public ResultApiModel<List<DesignStructureModel>> GetDesignStructure(string apiUrl, string moduleCode, int type, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getDesignStruture");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("moduleCode", moduleCode);
            request.AddParameter("type", type);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<DesignStructureModel>> resultModel = new ResultApiModel<List<DesignStructureModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<DesignStructureModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<Design3DModel>> GetListDesign3D(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getDesign3d");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<Design3DModel>> resultModel = new ResultApiModel<List<Design3DModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<Design3DModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<MaterialModel>> GetListMaterial(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getMaterial");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<MaterialModel>> resultModel = new ResultApiModel<List<MaterialModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<MaterialModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<ModuleDesignDocumentModel>> GetModuleDesignDocument(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getModuleDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ModuleDesignDocumentModel>> resultModel = new ResultApiModel<List<ModuleDesignDocumentModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ModuleDesignDocumentModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<ModuleDesignDocumentModel>> GetModuleDesignDocumentByType(string apiUrl, int designType, string token)
        {
            var client = new RestClient(apiUrl + "api/download/GetModuleDesignDocumentByType");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("designType", designType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ModuleDesignDocumentModel>> resultModel = new ResultApiModel<List<ModuleDesignDocumentModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ModuleDesignDocumentModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }


        public ResultApiModel<List<RawMaterialModel>> GetRawMaterial(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getRawMaterial");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<RawMaterialModel>> resultModel = new ResultApiModel<List<RawMaterialModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<RawMaterialModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<ErrorModel>> GetErrorModuleNotDone(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getErrorModuleNotDone");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ErrorModel>> resultModel = new ResultApiModel<List<ErrorModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ErrorModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<ConverUnitModel>> GetConvertUnit(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getConvertUnit");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ConverUnitModel>> resultModel = new ResultApiModel<List<ConverUnitModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ConverUnitModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<DesignStructureModel>> GetDesignStructure(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getDesignStructure");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<DesignStructureModel>> resultModel = new ResultApiModel<List<DesignStructureModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<DesignStructureModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<DesignStructureFileModel>> GetDesignStructureFile(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getDesignStructureFile");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<DesignStructureFileModel>> resultModel = new ResultApiModel<List<DesignStructureFileModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<DesignStructureFileModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<DesignStructureFileModel>> GetDesignStructureFile(string apiUrl, string DesignStrctureId, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getDesignStructureFile");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("DesignStrctureId", DesignStrctureId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<DesignStructureFileModel>> resultModel = new ResultApiModel<List<DesignStructureFileModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<DesignStructureFileModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }


        public ResultApiModel<List<MATFileResultModel>> GetFileInFolderMAT(string apiUrl, MATModel model, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getFileInFolderMAT");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<MATFileResultModel>> resultModel = new ResultApiModel<List<MATFileResultModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<MATFileResultModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        // Get file folder jgs
        public ResultApiModel<List<IGSFileResultModel>> GetFileInFolderIGS(string apiUrl, IGSModel model, string token)
        {
            var client = new RestClient(apiUrl + "api/download/GetFileInFolderIGS");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<IGSFileResultModel>> resultModel = new ResultApiModel<List<IGSFileResultModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<IGSFileResultModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }
        public ResultApiModel<ModuleModel> GetModuleInfo(string apiUrl, string moduleCode, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getModuleInfo");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("moduleCode", moduleCode);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<ModuleModel> resultModel = new ResultApiModel<ModuleModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<ModuleModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<ModuleModel>>(response.Content);
            }
            return resultModel;
        }

        public ResultApiModel GetMosuleStand(string apiUrl, string moduleCode, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getMosuleStand");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("moduleCode", moduleCode);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel resultModel = new ResultApiModel();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy thông tin moduel theo Id
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ResultApiModel<ModuleModel> GetModuleByModuleId(string apiUrl, string moduleId, string token)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetModuleByModuleId");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("moduleId", moduleId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<ModuleModel> resultModel = new ResultApiModel<ModuleModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<ModuleModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<ModuleModel>>(response.Content);
            }
            return resultModel;
        }

        public ResultApiModel<List<ManufactureResultModel>> getListManufacture(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getListManufacture");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ManufactureResultModel>> resultModel = new ResultApiModel<List<ManufactureResultModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ManufactureResultModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<UnitModel>> getListUnit(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getListUnit");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<UnitModel>> resultModel = new ResultApiModel<List<UnitModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<UnitModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<ModuleModel>> getListModule(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getListModule");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ModuleModel>> resultModel = new ResultApiModel<List<ModuleModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ModuleModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        public ResultApiModel<List<DownloadModuleDesignDataModel>> GetDataDownloadModule(DownloadModuleDesignModel model)
        {
            var client = new RestClient(model.ApiUrl + "api/downloadmodule/Getdata");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.Timeout = 300000;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            request.AddParameter("Authorization", "Bearer " + model.Token, ParameterType.HttpHeader);

            ResultApiModel<List<DownloadModuleDesignDataModel>> resultModel = new ResultApiModel<List<DownloadModuleDesignDataModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<DownloadModuleDesignDataModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        public ProjectProductFolderModel GetDataDownloadProductDocuments(DownloadProjectProductModel model)
        {
            var client = new RestClient(model.ApiUrl + "api/DownloadProjectProduct/GetDataDownloadProductDocuments");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            request.AddParameter("Authorization", "Bearer " + model.Token, ParameterType.HttpHeader);

            ProjectProductFolderModel resultModel = new ProjectProductFolderModel();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ProjectProductFolderModel>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        public async Task<ResultApiModel<List<DownloadModuleDesignDataModel>>> GetDataDownloadMaterial(DownloadMaterialDesignModel model)
        {
            var client = new RestClient(model.ApiUrl + "api/downloadmodule/GetDataMaterial");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model);
            request.AddParameter("Authorization", "Bearer " + model.Token, ParameterType.HttpHeader);

            ResultApiModel<List<DownloadModuleDesignDataModel>> resultModel = new ResultApiModel<List<DownloadModuleDesignDataModel>>();
            var response = await client.ExecuteTaskAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<DownloadModuleDesignDataModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        /// <summary>
        /// Kiểm tra file 3D
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dModel"></param>
        /// <returns></returns>
        public ResultApiModel<Design3DFileResultModel> CheckFile3D(TestDesignStructureModel model, Design3DFileModel dModel, string token)
        {
            var client = new RestClient(model.ApiUrl + "api/WebService/CheckFile3D");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            request.AddJsonBody(dModel);

            ResultApiModel<Design3DFileResultModel> resultModel = new ResultApiModel<Design3DFileResultModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<Design3DFileResultModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        public ResultApiModel<DataCheckModuleUploadModel> GetDataDefinitions(string apiUrl, string token, int designType, int objectType)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetDataDefinitions");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("designType", designType);
            request.AddParameter("objectType", objectType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<DataCheckModuleUploadModel> resultModel = new ResultApiModel<DataCheckModuleUploadModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<DataCheckModuleUploadModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }


        #region Tài liệu thiết kế module
        public ResultApiModel<DataCheckModuleUploadModel> GetData(string apiUrl, string token, int designType)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetData");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("designType", designType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<DataCheckModuleUploadModel> resultModel = new ResultApiModel<DataCheckModuleUploadModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<DataCheckModuleUploadModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        public ResultApiModel<List<ModuleDesignDocumentModel>> GetAllModuleDesignDocument(string apiUrl, string token, string moduleId, int designType)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetAllModuleDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("moduleId", moduleId);
            request.AddParameter("designType", designType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ModuleDesignDocumentModel>> resultModel = new ResultApiModel<List<ModuleDesignDocumentModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ModuleDesignDocumentModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy thông tin folder download
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ResultApiModel<List<FolderUploadModel>> GetFolderDownloadModuleDesignDocument(string apiUrl, string token, string moduleId, string folderId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetFolderDownloadModuleDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("moduleId", moduleId);
            request.AddParameter("folderId", folderId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<FolderUploadModel>> resultModel = new ResultApiModel<List<FolderUploadModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<FolderUploadModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy thông tin FTP
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ResultApiModel<FTPServer> GetFTPServer(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetFTPServer");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<FTPServer> resultModel = new ResultApiModel<FTPServer>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<FTPServer>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }
        #endregion

        public ResultApiModel GetListProductModule(string apiUrl, string moduleId, string token)
        {
            var client = new RestClient(apiUrl + "api/download/getListProductModule");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("ModuleId", moduleId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel resultModel = new ResultApiModel();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel>(response.Content);
            }
            return resultModel;
        }

        #region Thiết bị
        /// <summary>
        /// Lấy dữ liệu upload thiết bị
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="productId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public ResultApiModel<UploadProductDataModel> GetUploadProductData(string apiUrl, string token, string productId, int designType)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetUploadProductData");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("productId", productId);
            request.AddParameter("designType", designType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            ResultApiModel<UploadProductDataModel> resultModel = new ResultApiModel<UploadProductDataModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<UploadProductDataModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<UploadProductDataModel>>(response.Content);
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy tất cả tài liệu thiết kế thiết bị
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="moduleId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public ResultApiModel<List<ProductDesignDocumentModel>> GetAllProductDesignDocument(string apiUrl, string token, string productId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetAllProductDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("productId", productId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ProductDesignDocumentModel>> resultModel = new ResultApiModel<List<ProductDesignDocumentModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ProductDesignDocumentModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy thông tin folder download thiết bị
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultApiModel<List<FolderUploadModel>> GetFolderDownloadProductDesignDocument(string apiUrl, string token, string productId, string folderId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetFolderDownloadProductDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("productId", productId);
            request.AddParameter("folderId", folderId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<FolderUploadModel>> resultModel = new ResultApiModel<List<FolderUploadModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<FolderUploadModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }
        #endregion

        #region Phòng học
        /// <summary>
        /// Lấy dữ liệu upload phòng học
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="classRoomId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public ResultApiModel<UploadClassRoomDataModel> GetUploadClassRoomData(string apiUrl, string token, string classRoomId, int designType)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetUploadClassRoomData");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("classRoomId", classRoomId);
            request.AddParameter("designType", designType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            ResultApiModel<UploadClassRoomDataModel> resultModel = new ResultApiModel<UploadClassRoomDataModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<UploadClassRoomDataModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<UploadClassRoomDataModel>>(response.Content);
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy tất cả tài liệu thiết kế phòng học
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="classRoomId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public ResultApiModel<List<ClassRoomDesignDocumentModel>> GetAllClassRoomDesignDocument(string apiUrl, string token, string classRoomId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetAllClassRoomDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("classRoomId", classRoomId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<ClassRoomDesignDocumentModel>> resultModel = new ResultApiModel<List<ClassRoomDesignDocumentModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<ClassRoomDesignDocumentModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy thông tin folder download phòng học
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public ResultApiModel<List<FolderUploadModel>> GetFolderDownloadClassRoomDesignDocument(string apiUrl, string token, string classRoomId, string folderId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetFolderDownloadClassRoomDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("classRoomId", classRoomId);
            request.AddParameter("folderId", folderId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<FolderUploadModel>> resultModel = new ResultApiModel<List<FolderUploadModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<FolderUploadModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }
        #endregion

        #region Giải pháp
        /// <summary>
        /// Lấy dữ liệu upload giải pháp
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="solutionId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public ResultApiModel<UploadSolutionDataModel> GetUploadSolutionData(string apiUrl, string token, string solutionId, int designType)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetUploadSolutionData");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("solutionId", solutionId);
            request.AddParameter("designType", designType);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            ResultApiModel<UploadSolutionDataModel> resultModel = new ResultApiModel<UploadSolutionDataModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<UploadSolutionDataModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<UploadSolutionDataModel>>(response.Content);
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy tất cả tài liệu thiết kế giải pháp
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="solutionId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        public ResultApiModel<List<SolutionDesignDocumentModel>> GetAllSolutionDesignDocument(string apiUrl, string token, string solutionId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetAllSolutionDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("solutionId", solutionId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<SolutionDesignDocumentModel>> resultModel = new ResultApiModel<List<SolutionDesignDocumentModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<SolutionDesignDocumentModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }

        /// <summary>
        /// Lấy thông tin folder download giải pháp
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="token"></param>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        public ResultApiModel<List<FolderUploadModel>> GetFolderDownloadSolutionDesignDocument(string apiUrl, string token, string solutionId, string folderId)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetFolderDownloadSolutionDesignDocument");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("solutionId", solutionId);
            request.AddParameter("folderId", folderId);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<List<FolderUploadModel>> resultModel = new ResultApiModel<List<FolderUploadModel>>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<List<FolderUploadModel>>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return resultModel;
        }
        #endregion

        #region DMVT
        public ResultApiModel<DataCheckDMVTModel> GetDataCheckDMVT(string apiUrl, string token)
        {
            var client = new RestClient(apiUrl + "api/WebService/GetDataCheckDMVT");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ResultApiModel<DataCheckDMVTModel> resultModel = new ResultApiModel<DataCheckDMVTModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<DataCheckDMVTModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }
        #endregion

        public static ImageResult UploadFile(string apiUrl, Image image, string token, string name)
        {
            UploadModel upload = new UploadModel();
            upload.FolderName = "Solution/";
            var pathFile = string.Empty;
            string a = string.Empty;
            var client = new RestClient(apiUrl + "api/HandlingImage/UploadImage");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "multipart/form-data");
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            string output = JsonConvert.SerializeObject(upload);
            request.AddParameter("Model", output);
            //request.AddParameter("file", xByte);
            //request.AddParameter("file", xByte, ParameterType.RequestBody);
            Stream newStream = new MemoryStream(xByte);
            request.Files.Add(new FileParameter
            {
                Name = "file",
                Writer = (s) =>
                {
                    newStream.CopyTo(s);
                },
                FileName = name,
                // ContentType = "text/csv",
                ContentLength = newStream.Length
            });
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            ImageResult resultModel = new ImageResult();

            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ImageResult>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
                resultModel = JsonConvert.DeserializeObject<ImageResult>(response.Content);
            }

            return resultModel;
        }

        public List<MaterialFolderModel> GetDataDownloadMaterialDocument3Ds(DowloadMaterialDocument model)
        {
            var client = new RestClient(model.ApiUrl + "api/Material/GetDataDownloadMaterialDocument3Ds");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model.Material);
            request.AddParameter("Authorization", "Bearer " + model.Token, ParameterType.HttpHeader);

            List<MaterialFolderModel> resultModels = new List<MaterialFolderModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModels = JsonConvert.DeserializeObject<List<MaterialFolderModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModels;
        }

        public string ExportExcelListError(DowloadMaterialDocument model)
        {
            var client = new RestClient(model.ApiUrl + "api/Material/ExportExcelListError");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(model.ListError);
            request.AddParameter("Authorization", "Bearer " + model.Token, ParameterType.HttpHeader);
            string path = "";
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                path = JsonConvert.DeserializeObject<string>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(JsonConvert.SerializeObject(response)));
                NtsLog.LogError(new Exception(response.Content));
            }
            return path;
        }
    }
}
