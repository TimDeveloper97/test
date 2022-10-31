using NTS.Model.Common;
using NTS.Model.Manufacture;
using NTS.Model.Supplier;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Supplier
{
    [RoutePrefix("api/Supplier")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F000807")]
    public class SupplierController : BaseController
    {
        private readonly SupplierBusiness _supplierBusiness = new SupplierBusiness();

        [Route("SearchSupplier")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000800")]
        public HttpResponseMessage SearchSupplier(SupplierSearchModel modelSearch)
        {
            var result = _supplierBusiness.SearchSupplier(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000104")]
        public HttpResponseMessage SearchManufacture(ManufactureSearchModel model)
        {
            var result = _supplierBusiness.SearchManufacture(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSupplierManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000801;F000802;F000804")]

        public HttpResponseMessage SearchSupplierManufacture(ManufactureSearchModel modelSearch)
        {
            var result = _supplierBusiness.SearchSupplierManufacture(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddSupplier")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000801;F120808")]
        public HttpResponseMessage AddManufacture(SupplierModel model)
        {
            model.CreateBy = GetUserIdByRequest();
          var  suppliserId = _supplierBusiness.AddSupplier(model);
            return Request.CreateResponse(HttpStatusCode.OK, suppliserId);
        }

        [Route("GetSupplierCode")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000801;F120808")]
        public HttpResponseMessage GetSupplierCode()
        {
            var suppliserId = _supplierBusiness.GetSupplierCode();
            return Request.CreateResponse(HttpStatusCode.OK, suppliserId);
        }

        [Route("GetSupplierInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000804;F000802")]
        public HttpResponseMessage GetManufactureInfo(SupplierModel model)
        {
            var result = _supplierBusiness.GetSupplierInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetSupplierContractInfo")]
        [HttpPost]
        public HttpResponseMessage GetManufactureInfo(SupplierContractModel model)
        {
            var result = _supplierBusiness.GetSupplierContractInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateSupplier")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000802")]
        public HttpResponseMessage UpdateSupplier(SupplierModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _supplierBusiness.UpdateSupplier(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateSupplierContract")]
        [HttpPost]
        public HttpResponseMessage UpdateSupplierContract(SupplierContractModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _supplierBusiness.UpdateSupplierContract(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSupplier")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000803")]
        public HttpResponseMessage DeleteSupplier(SupplierModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _supplierBusiness.DeleteSupplier(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000802")]
        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = _supplierBusiness.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000806")]
        public HttpResponseMessage ExportExcel(SupplierSearchModel model)
        {
            string path = _supplierBusiness.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000805")]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _supplierBusiness.ImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateSupplierContract")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000802")]
        public HttpResponseMessage CreateSupplierContract(SupplierContractModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _supplierBusiness.CreateSupplierContract(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteSupplierContract")]
        [HttpPost]
        public HttpResponseMessage DeleteSupplierContract(SupplierContractModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _supplierBusiness.DeleteSupplierContract(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
