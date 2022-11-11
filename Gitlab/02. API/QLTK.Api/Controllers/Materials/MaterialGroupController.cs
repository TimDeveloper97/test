using QLTK.Api.Controllers.Common;
using QLTK.Business.MaterialGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.MaterialGroup
{
    [RoutePrefix("api/MaterialGroup")]
    [NTSIPAuthorize(AllowFeature = "F000105")]
    public class MaterialGroupController : BaseController
    {
        private readonly MaterialGroupBusiness business = new MaterialGroupBusiness();
        [Route("GetListMaterialGroup")]
        [HttpPost]
        public HttpResponseMessage GetListMaterialGroup()
        {
            var rs = business.GetListMaterialGroup();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }

        [Route("GetMaterialGroups")]
        [HttpPost]

        public HttpResponseMessage GetMaterialGroups()
        {
            var rs = business.GetMaterialGroups();
            return Request.CreateResponse(HttpStatusCode.OK, rs);
        }
    }
}
