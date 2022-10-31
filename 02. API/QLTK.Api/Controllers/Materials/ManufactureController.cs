using NTS.Model.Common;
using NTS.Model.Manufacture;
using NTS.Model.MaterialGroup;
using NTS.Model.Supplier;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Manufacturer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Manufacture
{
    [RoutePrefix("api/QLTKM")]
    [ApiHandleExceptionSystemAttribute]
    [NTSIPAuthorize(AllowFeature = "F000507")]
    public class ManufactureController : BaseController
    {
        private readonly ManufactureBusiness _manufactureBusiness = new ManufactureBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000500")]
        public HttpResponseMessage SearchManufacture(ManufactureSearchModel modelSearch)
        {
            var result = _manufactureBusiness.SearchManufacture(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSupplierManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000500")]
        public HttpResponseMessage SearchSupplierManufacture(SupplierSearchModel modelSearch)
        {
            var result = _manufactureBusiness.SearchSupplierManufacture(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xoá hãng sản xuát
        /// </summary>
        /// <param name="manufactureModel"></param>
        /// <returns></returns>
        [Route("DeleteManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000503")]

        public HttpResponseMessage DeleteManufacture(ManufactureModel model)
        {
            _manufactureBusiness.DeleteManufacture(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm mới hãng sản xuất
        /// </summary>
        /// <param name="manufactureModel"></param>
        /// <returns></returns>
        [Route("AddManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000501")]

        public HttpResponseMessage AddManufacture(ManufactureModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _manufactureBusiness.AddManufacture(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật hãng sản xuất
        /// </summary>
        /// <param name="manufactureModel"></param>
        /// <returns></returns>
        [Route("UpdateManufacture")]
        [NTSAuthorize(AllowFeature = "F000502")]

        [HttpPost]
        public HttpResponseMessage UpdateManufacture(ManufactureModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _manufactureBusiness.UpdateManufacture(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin hãng sản xuất
        /// </summary>
        /// <param name="manufactureModel"></param>
        /// <returns></returns>
        [Route("GetManufactureInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000504;F000502")]
        public HttpResponseMessage GetManufactureInfo(ManufactureModel model)
        {
            var result = _manufactureBusiness.GetManufactureInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000504;F000505")]

        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = _manufactureBusiness.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000506")]

        public HttpResponseMessage ExportExcel(ManufactureSearchModel model)
        {
            try
            {
                string path = _manufactureBusiness.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ManufactureImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000505")]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _manufactureBusiness.ImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetListManufacture")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F000504;F000502")]
        public HttpResponseMessage GetListManufacture()
        {
            var rs = _manufactureBusiness.GetListManufacture();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }

        //[Route("SearchParentId")]
        //[HttpPost]
        //public HttpResponseMessage SearchParentId(MaterialGroupModel model)
        //{
        //    var rs = _manufactureBusiness.SearchParentId(model);
        //    return Request.CreateResponse(HttpStatusCode.OK, rs);
        //}
    }
}