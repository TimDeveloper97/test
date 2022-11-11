using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using NTS.Model.VMSMF;
using QLTK.Business.NTSUser;
using NTS.Api.Models;
using NTS.Model.Common;
using NTS.Model.Combobox;

namespace NTS.Api.Controllers
{
    [RoutePrefix("api/NTSUser")]
    public class NTSUserController : ApiController
    {
        private readonly NTSUserBusiness _business = new NTSUserBusiness();
        [Route("ChangePass")]
        [HttpPost]
        public HttpResponseMessage ChangePass(Model.Common.UserModel model)
        {
            try
            {
                _business.ChangePass(model);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ChangeUserInfo")]
        [HttpPost]
        public HttpResponseMessage ChangeUserInfo()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                UserInfoModel model = JsonConvert.DeserializeObject<UserInfoModel>(modelJson);

                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                _business.ChangeUserInfo(model, hfc);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetUserInfo")]
        [HttpPost]
        public HttpResponseMessage GetUserInfo(string UserId)
        {
            try
            {
                var rs = _business.GetUserInfo(UserId);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("Forgotpassword")]
        [HttpPost]
        public HttpResponseMessage Forgotpassword(UserInfoModel model)
        {
            try
            {
                _business.Forgotpassword(model.Email);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("Resetpassword")]
        [HttpPost]
        public HttpResponseMessage Resetpassword(ForgotpassModel model)
        {
            try
            {
                _business.Resetpassword(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ValidCache")]
        [HttpPost]
        public HttpResponseMessage ValidCache(ForgotpassModel model)
        {
            try
            {
                _business.ValidCache(model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
