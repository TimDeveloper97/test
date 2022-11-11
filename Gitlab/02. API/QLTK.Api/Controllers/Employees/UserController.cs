
using NTS.Model.GroupFunction;
using NTS.Model.User;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.User
{
    [RoutePrefix("api/USER")]
    [ApiHandleExceptionSystem]
    public class UserController : BaseController
    {
        private readonly UserBusiness _business = new UserBusiness();
      
        [Route("Create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080007")]
        public HttpResponseMessage Create(UserModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.Create(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("UpdateUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080007")]
        public HttpResponseMessage UpdateUser(UserModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateUser(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("GetUserInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080008;F080002;F080004")]
        public HttpResponseMessage GetUserInfo(UserModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            model.UpdateBy = GetUserIdByRequest();
            var result = _business.GetUserInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupPermission")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080007;F080008")]
        public HttpResponseMessage GetGroupUser(string Id)
        {
            var result = _business.GetGroupPermission(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ChangePassword")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage ChangePassword(UserModel model)
        {
            try
            {
                _business.ChangePassword(model);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ResetPassword")]
        [HttpPost]
        [Authorize]
        [NTSAuthorize(AllowFeature = "F080017")]
        public HttpResponseMessage ResetPassword(string Id)
        {
            try
            {
                _business.ResetPassword(Id);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
