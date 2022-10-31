using NTS.Model.GeneralTemplate;
using NTS.Model.ProjectProducts;
using QLTK.Api.Attributes;
using QLTK.Business.GeneralTempalteMechanical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.GeneralDesign
{
    [RoutePrefix("api/GeneralDesign")]
    [ApiHandleExceptionSystemAttribute]
    public class GeneralDesignsController : ApiController
    {
        private readonly GeneralDesignBussiness generalDesign = new GeneralDesignBussiness();

        [Route("GeneralDesign")]
        [HttpPost]
        public HttpResponseMessage GeneralDesign(ProjectProductsResuldModel model)
        {
            var result = generalDesign.GeneralDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerByProjectId")]
        public HttpResponseMessage ExpoetGeneralDesign(string projectId)
        {
            string Name = generalDesign.GetCustomerByProjectId(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, Name);
        }

        [Route("GetModuleByProjectproductId")]
        public HttpResponseMessage GetModuleByProjectproductId(string projectProductId)
        {
            var result = generalDesign.GetModuleByProjectproductId(projectProductId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExpoetGeneralDesign")]
        public HttpResponseMessage ExpoetGeneralDesign(ExportGeneralDesignModel model)
        {
            string path = generalDesign.ExpoetGeneralDesign(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }
    }
}
