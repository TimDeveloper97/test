using NTS.Common;
using NTS.Model.GeneralInfomationProject;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.GeneralInformationProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.GeneralInformationProject
{
    [RoutePrefix("api/GeneralInformationProject")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060903")]
    public class GeneralInformationProjectsController : BaseController
    {
        private readonly GeneralInformationProjectBussiness _bussiness = new GeneralInformationProjectBussiness();

        [Route("GetListGeneralInformationProject")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060900")]
        public HttpResponseMessage GetListGeneralInformationProject(GeneralInfomationProjectSearchModel model)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060902))
            {
                model.SBUId = this.GetSbuIdByRequest();
                model.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _bussiness.GetListGeneralInformationProject(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
