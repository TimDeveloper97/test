using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Customers;
using NTS.Model.Product;
using NTS.Model.Project;
using NTS.Model.ProjectProducts;
using NTS.Model.Projects.Project;
using NTS.Model.TestCriteria;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Projects;

namespace QLTK.Api.Controllers.ProductNeedPublication
{
    [RoutePrefix("api/ProductNeedPublication")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F030417")]
    public class ProductNeedPublicationController : BaseController
    {
        private readonly ProductNeedPublicationsBusiness project = new ProductNeedPublicationsBusiness();

        [Route("SearchProject/{id}")]
        [HttpGet]
        public HttpResponseMessage SearchProject(string id)
        {
            var result = project.SearchProject(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("SearchProduct")]
        [HttpPost]
        public HttpResponseMessage SearchProduct(ProductSearchModel modelSearch)
        {
            var result = project.SearchProduct(modelSearch, false);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
