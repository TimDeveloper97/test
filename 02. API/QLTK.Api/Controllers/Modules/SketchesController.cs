using NTS.Model.Common;
using NTS.Model.Function;
using NTS.Model.Materials;
using NTS.Model.SketchAttach;
using NTS.Model.SketchMaterialElectronic;
using NTS.Model.SketchMaterialMechanical;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Sketches;
using QLTK.Business.SketchMaterialElectronic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static NTS.Model.SketchMaterialElectronic.SketchMaterialElectronicModel;

namespace QLTK.Api.Controllers.Sketches
{
    [RoutePrefix("api/Sketches")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F020129")]
    public class SketchesController : BaseController
    {
		private readonly SketchesBusiness _business = new SketchesBusiness();
        private readonly SketchMaterialElectronicBussiness _businessE = new SketchMaterialElectronicBussiness();

        [Route("GetSketchAttachInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020109")]
        public HttpResponseMessage GetSketchAttachInfo(SketchAttachModel model)
        {
            var result = _business.GetListInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020108")]
        public HttpResponseMessage AddFile(SketchAttachModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddSketch(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetSketchHistoryInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020109")]
        public HttpResponseMessage GetSketchHistoryInfo(SketchAttachHistoryModel model)
        {
            var result = _business.GetListHistoryInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020108")]
        public HttpResponseMessage ImportFile(UploadResultModelElectronic model)
        {
            try
            {
                string userId = GetUserIdByRequest();
                var result = _businessE.ImportFile(userId, model.Link, model.ModuleId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("SearchSketchesMaterialElectronic")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020109")]
        public HttpResponseMessage SearchSketchesMaterialElectronic(SketchMaterialElectronicModel modelSearch)
        {
            try
            {
                var result = _businessE.SearchSketchMaterialElectronic(modelSearch, modelSearch.ModuleId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("SearchSketchesMaterialMechanical")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020109")]
        public HttpResponseMessage SearchSketchesMaterialMechanical(SketchMaterialMechanicalModel modelSearch)
        {
            try
            {
                var result = _businessE.SearchSketchMaterialMechanical(modelSearch, modelSearch.ModuleId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("DeleteSketchMaterialMechanical")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020108")]
        public HttpResponseMessage DeleteDegree(SketchMaterialMechanicalModel model)
        {
            _businessE.DeleteSketchMaterialMechanical(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSketchMaterialElectronic")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F020108")]
        public HttpResponseMessage DeleteSketchMaterialElectronic(SketchMaterialElectronicModel model)
        {
            _businessE.DeleteSketchMaterialElectronic(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }

}
