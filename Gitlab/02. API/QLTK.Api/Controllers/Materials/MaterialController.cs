using Newtonsoft.Json;
using NTS.Model.Common;
using NTS.Model.MaterialBuyHistory;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Materials
{
    [RoutePrefix("api/Material")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000017;F110202")]
    public class MaterialController : BaseController
    {
        private readonly MaterialBusiness _materialBusiness = new MaterialBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000000")]
        public HttpResponseMessage SearchMaterial(MaterialSearchModel modelSearch)
        {
            var result = _materialBusiness.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000003")]
        public HttpResponseMessage DeleteMaterial(MaterialModel model)
        {
            _materialBusiness.DeleteMaterial(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000001")]
        public HttpResponseMessage AddMaterial()
        {
            var modelJson = HttpContext.Current.Request.Form["Model"];
            MaterialModel MaterialModel = JsonConvert.DeserializeObject<MaterialModel>(modelJson);
            MaterialModel.CreateBy = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            _materialBusiness.AddMaterial(MaterialModel, hfc, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000002")]
        public HttpResponseMessage UpdateMaterial()
        {
            var modelJson = HttpContext.Current.Request.Form["Model"];
            MaterialModel MaterialModel = JsonConvert.DeserializeObject<MaterialModel>(modelJson);
            MaterialModel.UpdateBy = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            _materialBusiness.UpdateMaterial(MaterialModel, hfc, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetMaterialInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000002;F000004")]
        public HttpResponseMessage GetMaterialInfo(MaterialModel model)
        {
            var result = _materialBusiness.GetMaterialInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000005")]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _materialBusiness.ImportFile(userId, hfc[0]);
            }

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreatNewBuyHistory")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000005")]
        public HttpResponseMessage CreatNewBuyHistory(ImportMaterialBuyHistoryResult model)
        {
            string userId = GetUserIdByRequest();
            var result = _materialBusiness.CreatNewBuyHistory(userId, model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("OverwriteBuyHistory")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000005")]
        public HttpResponseMessage OverwriteBuyHistory(ImportMaterialBuyHistoryResult model)
        {
            string userId = GetUserIdByRequest();
            var result = _materialBusiness.OverwriteBuyHistory(userId, model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("GetHistoryByMaterialId")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000008")]
        public HttpResponseMessage GetHistoryByMaterialId(MaterialBuyHistorySearchModel model)
        {
            var result = _materialBusiness.GetHistoryByMaterialId(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000100")]
        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = _materialBusiness.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000007")]

        public HttpResponseMessage ExportExcel(MaterialSearchModel model)
        {
            string path = _materialBusiness.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("MaterialImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000006")]
        public HttpResponseMessage MaterialImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _materialBusiness.ImportFileMaterial(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }


        [Route("GetListMaterial")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000006")]
        public HttpResponseMessage GetListMaterial()
        {
            var result = _materialBusiness.GetListMaterial();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportDMVTNotDB")]
        [HttpPost]
        public HttpResponseMessage ExportDMVTNotDB(List<MaterialExportModel> listDMVTNotDB)
        {
            var result = _materialBusiness.ExportDMVTNotDB(listDMVTNotDB);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CheckPriceMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110201")]
        public HttpResponseMessage CheckPriceMaterial()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            var results = _materialBusiness.CheckPriceMaterial(userId, hfc[0]);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("ExportCheckPrice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110201")]
        public HttpResponseMessage ExportCheckPrice(List<ModuleMaterialResultModel> materials)
        {
            string userId = GetUserIdByRequest();
            var results = _materialBusiness.ExportCheckPrice(materials);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("ImportFileSync")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000018")]
        public HttpResponseMessage ImportFileSync()
        {
            var data = HttpContext.Current.Request.Form["isConfirm"];
            var isConfirm = Convert.ToBoolean(data);
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                var result = _materialBusiness.ImportFileSync(hfc[0], isConfirm, GetUserIdByRequest());
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SyncSaleMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000018")]
        public HttpResponseMessage SyncSaleMaterial(bool check, bool isConfirm, List<string> listMaterialId)
        {
            var result = _materialBusiness.SyncSaleMaterial(check, isConfirm, listMaterialId, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupMaterialCodeInTemplate")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000100")]
        public HttpResponseMessage GetGroupMaterialCodeInTemplate()
        {
            string path = _materialBusiness.GetGroupMaterialCodeInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("MaterialCodeImportFile")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000006")]
        public HttpResponseMessage MaterialCodeImportFile()
        {
            var result = new List<MaterialDocumentDownloadModel>();
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
               result = _materialBusiness.MaterialCodeImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetDataDownloadMaterialDocument3Ds")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000006")]
        public HttpResponseMessage GetDataDownloadMaterialDocument3Ds(List<MaterialDocumentDownloadModel> model)
        {

            var result = _materialBusiness.GetDataDownloadMaterialDocument3Ds(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("MaterialImportFileBOM")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000006")]
        public HttpResponseMessage MaterialImportFileBOM(string projectProductId, string moduleId, bool isExit, bool confirm)
        {
            var result = new MaterialChangeDataModel();
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                result = _materialBusiness.MaterialImportFileBOM(userId, hfc[0], projectProductId, moduleId ,isExit, confirm);
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcelListError")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060037")]
        public HttpResponseMessage ExportExcelListError(List<FileErrorModel> model)
        {
            string path = _materialBusiness.ExportExcelListError(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }
    }
}