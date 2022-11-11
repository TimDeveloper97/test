using NTS.Business.Combobox;
using NTS.Model.QLTKMG;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NTS.Common;
using NTS.Model;
using QLTK.Api;
using NTS.Model.Combobox;
using NTS.Model.SalaryLevel;

namespace NTS.Api.Controllers.Combobox
{
    [RoutePrefix("api/TiviCombobox")]
    [ApiHandleExceptionSystem]
    public class TiviComboboxController : BaseController
    {
        private readonly ComboboxBusiness _Business = new ComboboxBusiness();
              

        [Route("GetListDepartmentUse")]
        [HttpPost]
        public HttpResponseMessage GetListDepartmentUse()
        {
            try
            {
                var result = _Business.GetListDepartmentUse();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
