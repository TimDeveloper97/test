using QLTK.Business.GroupUser;
using NTS.Model.Combobox;
using NTS.Model.GroupUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using QLTK.Api.Controllers.Common;
using QLTK.Api.Attributes;
using QLTK.Api;
using NTS.Common;
using NTS.Common.Resource;

namespace NTS.Api.Controllers.GroupUser
{
    [RoutePrefix("api/GroupUser")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080909")]
    public class GroupUserController : BaseController
    {
        private readonly GroupUserBusiness _business = new GroupUserBusiness();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("SearchGroupUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080900")]
        public HttpResponseMessage SearchEmSearchGroupUserployee(GroupUserSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F080905))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.SearchGroupUser(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("DeleteGroupUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080903;F080908")]
        public HttpResponseMessage DeleteGroupUser(GroupUserModel model)
        {
            var departmentId = GetDepartmentIdByRequest();
            bool isEditOther = CheckPermission(Constants.Permission_Code_F080908);
            _business.DeleteGroupUser(model, departmentId, GetUserIdByRequest(), isEditOther);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateGroupUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080901")]
        public HttpResponseMessage CreateGroupUser(GroupUserModel model)
        {
            bool isEditOther = CheckPermission(Constants.Permission_Code_F080906);

            if (!isEditOther && !this.GetDepartmentIdByRequest().Equals(model.DepartmentId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0033, TextResourceKey.GroupUser);
            }

            model.CreateBy = GetUserIdByRequest();
            _business.CreateGroupUser(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateGroupUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080902;F080907")]
        public HttpResponseMessage UpdateGroupUser(GroupUserModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            bool isEditOther = CheckPermission(Constants.Permission_Code_F080907);
            _business.UpdateGroupUser(model, this.GetDepartmentIdByRequest(), isEditOther);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetGroupUserInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080902;F080904")]
        public HttpResponseMessage GetGroupUserInfo(GroupUserModel model)
        {
            var result = _business.GetGroupUserInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}