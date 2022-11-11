using Newtonsoft.Json;
using NTS.Common;
using NTS.Model.DownloadListModules;
using NTS.Model.DownloadModule;
using NTS.Model.QLTKMODULE;
using NTS.Model.WebService;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DownloadListModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.DownloadListModule
{
    [RoutePrefix("api/downloadmodule")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F090903")]
    public class DownloadListModuleController : BaseController
    {
        private readonly DownloadListModuleBussiness _business = new DownloadListModuleBussiness();

        [Route("SearchModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090900")]
        public HttpResponseMessage SearchModule(ModuleSearchModel modelSearch)
        {
            var result = _business.SearchModule(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetData")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090901;F090902")]
        public HttpResponseMessage GetData(DownloadModuleDesignModel model)
        {
            var departmentId = this.GetDepartmentIdByRequest();
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = _business.GetData(model, departmentId);
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);

        }

        [Route("GetDataMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090901;F090902")]
        public HttpResponseMessage GetDataMaterial(DownloadMarterialDesignModel model)
        {
            var departmentId = this.GetDepartmentIdByRequest();
            ResultApiModel resultApiModel = new ResultApiModel();

            resultApiModel.Data = _business.GetDataMaterial(model, departmentId);
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("ImportExcelListModel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090901")]
        public HttpResponseMessage ImportExcelListModel()
        {
            var modelJson = HttpContext.Current.Request.Form["Model"];
            DownloadModuleSearchModel model = JsonConvert.DeserializeObject<DownloadModuleSearchModel>(modelJson);
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            var result = _business.ImportExcelListModel(model, hfc[0]);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
