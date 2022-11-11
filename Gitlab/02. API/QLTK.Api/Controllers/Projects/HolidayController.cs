using NTS.Model.Holiday;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Holidays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Holiday
{
    [RoutePrefix("api/Holiday")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F110003")]
    public class HolidayController : BaseController
    {
        HolidayBussiness _holidayBussiness = new HolidayBussiness();
        [Route("GetCalendarOfYear")]
        [HttpPost]
        public HttpResponseMessage GetCalendarOfYear(HolidayModel modelSearch)
        {
            var result = _holidayBussiness.GetCalendarOfYear(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CreateHoliday")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F000201")]
        public HttpResponseMessage CreateHoliday(HolidayModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _holidayBussiness.CreateHoliday(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
