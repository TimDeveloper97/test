using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NTS.Model.BOMDesignTwo;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.BOMDesignTwos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.BOMDesignTwo
{
    [RoutePrefix("api/BOMDesignTwo")]
    [ApiHandleExceptionSystem]
    [Authorize]
    [NTSIPAuthorize(AllowFeature = "F060036")]
    public class BOMDesignTwosController : BaseController
    {
        private readonly BOMDesignTwoBusiness _business = new BOMDesignTwoBusiness();
        string keyAuthorize = System.Configuration.ConfigurationManager.AppSettings["keyAuthorize"];

        [Route("SearchBOMDesignTwo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060004")]
        public HttpResponseMessage SearchBOMDesignTwo(BOMDesignTwoModel model)
        {
            var result = _business.SearchBOMDesignTwo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateBOMDesignTwo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060030")]
        public HttpResponseMessage CreateBOMDesignTwo(BOMDesignTwoModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            //_business.CreateBOMDesignTwo(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateBOMDesignTwo")]
        [HttpPost]
        public HttpResponseMessage UpdateBOMDesignTwo(BOMDesignTwoModel model)
        {
            _business.UpdateBOMDesignTwo(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteBOMDesignTwo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060031")]
        public HttpResponseMessage DeleteBOMDesignTwo(BOMDesignTwoModel model)
        {
            _business.DeleteBOMDesignTwo(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetBOMDesignTwoInfo")]
        [HttpPost]
        public HttpResponseMessage GetBOMDesignTwoInfo(BOMDesignTwoModel model)
        {
            var result = _business.GetBOMDesignTwoInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[HttpPost]
        //[Route("ImportMaterialElectric")]
        //public HttpResponseMessage ImportMaterialElectric()
        //{
        //    var bOMDesignTwoId = HttpContext.Current.Request.Form["BOMDesignTwoId"];
        //    var projectProductId = HttpContext.Current.Request.Form["ProjectProductId"];
        //    var materialType = NTS.Common.Constants.BOM_Electric;
        //    var userId = GetUserIdByRequest();

        //    HttpFileCollection hfc = HttpContext.Current.Request.Files;
        //    if (hfc.Count > 0)
        //    {
        //        _business.ImportMaterialElectric(userId, hfc[0], bOMDesignTwoId, materialType, projectProductId);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        //[HttpPost]
        //[Route("ImportMaterialManufacture")]
        //public HttpResponseMessage ImportMaterialManufacture()
        //{
        //    var bOMDesignTwoId = HttpContext.Current.Request.Form["BOMDesignTwoId"];
        //    var materialType = NTS.Common.Constants.BOM_Manufacture;
        //    var userId = GetUserIdByRequest();

        //    HttpFileCollection hfc = HttpContext.Current.Request.Files;
        //    if (hfc.Count > 0)
        //    {
        //        _business.ImportMaterialManufacture(userId, hfc[0], bOMDesignTwoId, materialType);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        //[HttpPost]
        //[Route("ImportMaterialTPA")]
        //public HttpResponseMessage ImportMaterialTPA()
        //{
        //    var bOMDesignTwoId = HttpContext.Current.Request.Form["BOMDesignTwoId"];
        //    var materialType = NTS.Common.Constants.BOM_Manufacture;
        //    var userId = GetUserIdByRequest();

        //    HttpFileCollection hfc = HttpContext.Current.Request.Files;
        //    if (hfc.Count > 0)
        //    {
        //        _business.ImportMaterialTPA(userId, hfc[0], bOMDesignTwoId, materialType);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        //[HttpPost]
        //[Route("ImportMaterialOther")]
        //public HttpResponseMessage ImportMaterialOther()
        //{
        //    var bOMDesignTwoId = HttpContext.Current.Request.Form["BOMDesignTwoId"];
        //    var projectProductId = HttpContext.Current.Request.Form["ProjectProductId"];
        //    var materialType = NTS.Common.Constants.BOM_Electric;
        //    var userId = GetUserIdByRequest();

        //    HttpFileCollection hfc = HttpContext.Current.Request.Files;
        //    if (hfc.Count > 0)
        //    {
        //        _business.ImportMaterialOther(userId, hfc[0], bOMDesignTwoId, materialType, projectProductId);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        [HttpPost]
        [Route("ImportData")]
        [NTSAuthorize(AllowFeature = "F060030")]
        public HttpResponseMessage ImportData()
        {
            var modelJson = HttpContext.Current.Request.Form["Model"];
            var model = JsonConvert.DeserializeObject<BOMDesignTwoModel>(modelJson);
            var userId = GetUserIdByRequest();
            model.CreateBy = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;

            _business.CreateBOMDesignTwo(model, hfc);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060034")]
        public HttpResponseMessage ExportExcel(BOMDesignTwoModel model)
        {
            var result = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetVersion")]
        [HttpPost]
        public HttpResponseMessage GetVersion(BOMDesignTwoModel model)
        {
            var result = _business.GetIndex(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
