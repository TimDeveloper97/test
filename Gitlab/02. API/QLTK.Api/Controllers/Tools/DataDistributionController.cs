using NTS.Model.DataDistribution;
using NTS.Model.DataDistributionFile;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.DataDistributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.DataDistribution
{
    [RoutePrefix(prefix: "api/DataDistribution")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F090808")]
    public class DataDistributionController : BaseController
    {
        private readonly DataDistributionBusiness _dataDistributionBusiness = new DataDistributionBusiness();
        [Route("SearchDataDistribution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090800")]
        public HttpResponseMessage SearchDataDistribution(DataDistributionModel modelSearch)
        {
            var result = _dataDistributionBusiness.SearchDataDistribution(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddDataDistribution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090801")]
        public HttpResponseMessage AddDataDistribution(DataDistributionModel model)
        {
            _dataDistributionBusiness.AddDataDistribution(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetDataDistributionInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090802;F090804")]
        public HttpResponseMessage GetManufactureInfo(DataDistributionModel model)
        {
            var result = _dataDistributionBusiness.GetDataDistributionInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateDataDistribution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090802")]
        public HttpResponseMessage UpdateManufacture(DataDistributionModel model)
        {
            _dataDistributionBusiness.UpdateDataDistribution(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteDataDistribution")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090803")]
        public HttpResponseMessage DeleteDataDistribution(DataDistributionModel model)
        {
            _dataDistributionBusiness.DeleteDataDistribution(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetDataDistributionFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090804")]
        public HttpResponseMessage SearchDataDistributioFile(DataDistributionModel modelSearch)
        {
            var result = _dataDistributionBusiness.GetDataDistributionFile(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("DeleteDataDistributionFileByFolder")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090605")]
        public HttpResponseMessage DeleteDataDistributionFileByFolder(DataDistributionFileModel model)
        {
            _dataDistributionBusiness.DeleteDataDistributionFileByFolder(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("SearchDataDistributionFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090805;F090806")]
        public HttpResponseMessage SearchDataDistributionFile(DataDistributionFileSearchModel model)
        {
            var result = _dataDistributionBusiness.SearchDataDistributionFile(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddDataDistributionFileLink")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090805")]
        public HttpResponseMessage AddDataDistributionFileLink(DataDistributionModel model)
        {
            _dataDistributionBusiness.AddDataDistributionFileLink(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddDataDistributionFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090806")]
        public HttpResponseMessage AddDataDistributionFile(DataDistributionFileModel model)
        {
            _dataDistributionBusiness.AddDataDistributionFile(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateDataDistributionFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090806")]
        public HttpResponseMessage UpdateDataDistributionFile(DataDistributionFileModel model)
        {
            _dataDistributionBusiness.UpdateDataDistributionFile(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteDataDistributionFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090807")]
        public HttpResponseMessage DeleteDataDistributionFile(DataDistributionFileModel model)
        {
            _dataDistributionBusiness.DeleteDataDistributionFile(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetDataDistributionFileInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F090806")]
        public HttpResponseMessage GetDataDistributionFileInfo(DataDistributionFileModel model)
        {
            var result = _dataDistributionBusiness.GetDataDistributionFileInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
