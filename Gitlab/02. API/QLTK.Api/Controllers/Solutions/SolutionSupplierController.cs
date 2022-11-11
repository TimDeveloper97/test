using NTS.Model.Solution.SolutionsSupplier;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.SolutionSupplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.SolutionSupplier
{
    [RoutePrefix(prefix: "api/SolutionSupplier")]
    [ApiHandleExceptionSystem]
    public class SolutionSupplierController : BaseController
    {
        private readonly SolutionSupplierBussiness solutionSupplier = new SolutionSupplierBussiness();

        [Route("GetlistSupplier")]
        [HttpPost]
        public HttpResponseMessage GetlistSupplier(SolutionSupplierSearchModel searchModel)
        {
            var result = solutionSupplier.GetlistSupplier(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


    }
}
