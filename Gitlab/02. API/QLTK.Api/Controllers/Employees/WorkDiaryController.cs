using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.WorkDiary;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.WorkDiarys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.WorkDiary
{
    [RoutePrefix("api/WorkDiary")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F080808;F110104")]
    public class WorkDiaryController : BaseController
    {
        private readonly WorkDiaryBussiness _workDiary = new WorkDiaryBussiness();

        [Route("SearchWorkDiary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080800")]
        public HttpResponseMessage SearchWorkPlace(WorkDiarySearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F080805))
            {
                modelSearch.EmployeeId = this.GetEmployeeIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
                modelSearch.SBUId = this.GetSbuIdByRequest();
            }

            var result = _workDiary.SearchWorkDiary(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddWorkDiary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080801")]
        public HttpResponseMessage AddWorkPlace(WorkDiaryModel model)
        {
            bool outOfDate = false;
            if (this.CheckPermission(Constants.Permission_Code_F080809))
            {
                outOfDate = true;
            }

            _workDiary.AddWorkDiary(model, this.GetUserIdByRequest(), this.GetEmployeeIdByRequest(), outOfDate);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetByIdWorkDiary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080804;F080802;F080805")]
        public HttpResponseMessage GetWorkPlace(WorkDiaryModel model)
        {
            var result = _workDiary.GetByIdWorkDiary(model);

            if (!result.CreateBy.Equals(this.GetEmployeeIdByRequest()) && !this.CheckPermission(Constants.Permission_Code_F080805))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0030, TextResourceKey.WorkDiary);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetWorkDiaryView")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080804;F080805")]
        public HttpResponseMessage GetWorkDiaryView(string id)
        {
            var result = _workDiary.GetWorkDiaryView(id);

            if (!result.CreateBy.Equals(this.GetEmployeeIdByRequest()) && !this.CheckPermission(Constants.Permission_Code_F080805))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0030, TextResourceKey.WorkDiary);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("UpdateWorkDiary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080802")]
        public HttpResponseMessage UpdateWorkDiary(WorkDiaryModel model)
        {
            bool outOfDate = false;
            if (this.CheckPermission(Constants.Permission_Code_F080810))
            {
                outOfDate = true;
            }

            _workDiary.UpdateWorkDiary(model, this.GetUserIdByRequest(), outOfDate);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteWorkDiary")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080803")]
        public HttpResponseMessage DeleteWorkDiary(WorkDiaryModel model)
        {
            bool outOfDate = false;
            if (this.CheckPermission(Constants.Permission_Code_F080811))
            {
                outOfDate = true;
            }

            _workDiary.DeleteWorkDiary(model, this.GetEmployeeIdByRequest(), outOfDate);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("Excel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080806;F080807")]
        public HttpResponseMessage Excel(WorkDiarySearchModel model)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F080807))
            {
                model.EmployeeId = this.GetEmployeeIdByRequest();
                model.DepartmentId = this.GetDepartmentIdByRequest();
                model.SBUId = this.GetSbuIdByRequest();
            }

            var result = _workDiary.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbEmployeeByUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080801;F080802;F080805;F080804;F080809;F080810")]
        public HttpResponseMessage GetCbbEmployeeByUser()
        {
            string userId = GetUserIdByRequest();
            var result = _workDiary.GetCbbEmployeeByUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCbbprojectByUser")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F080801;F080802;F080805;F080804;F080809;F080810")]
        public HttpResponseMessage GetCbbprojectByUser()
        {
            string userId = GetUserIdByRequest();
            var result = _workDiary.GetCbbprojectByUser(userId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchWorkingTime")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F110101;F110102;F110103")]
        public HttpResponseMessage SearchWorkingTime(SearchWorkDiaryModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F110102))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }

            if (!this.CheckPermission(Constants.Permission_Code_F110103))
            {
                modelSearch.EmployeeId = this.GetEmployeeIdByRequest();
            }

            var result = _workDiary.SearchWorkingTime(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
