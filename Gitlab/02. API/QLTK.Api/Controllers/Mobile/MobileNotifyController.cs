using NTS.Common;
using NTS.Model.Mobile;
using NTS.Model.Notify;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Notify
{
    [RoutePrefix("api/NotifyMobile")]
    [Authorize]
    public class MobileNotifyController : BaseController
    {
        private MobileNotifyBussiness _bussiness = new MobileNotifyBussiness();

        [Route("GetNotify")]
        [HttpPost]
        public HttpResponseMessage GetNotify(NotifySearchModel model)
        {
            try
            {
                model.UserId = this.GetUserIdByRequest();

                var data = _bussiness.GetNotify(model);
                return Request.CreateResponse(HttpStatusCode.OK, new ResultModel<List<NotifyModel>>
                {
                    Status = Constants.ResponseSuccess,
                    Data = data
                });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ResultModel<List<string>>
                {
                    Status = Constants.ResponseError,
                    Message = ex.Message
                });
            }
        }

        [Route("TickNotify")]
        [HttpPost]
        public HttpResponseMessage TickNotify(NotifySearchModel model)
        {
            try
            {
                model.UserId = this.GetUserIdByRequest();
                _bussiness.TickNotify(model);
                return Request.CreateResponse(HttpStatusCode.OK,
                     new ResultModel<string>
                     {
                         Status = Constants.ResponseSuccess,
                         Data = "Thay đổi thành công"
                     });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                     new ResultModel<string>
                     {
                         Status = Constants.ResponseError,
                         Message = ex.Message
                     });
            }
        }

    }
}
