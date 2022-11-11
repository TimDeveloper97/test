using NTS.Common;
using NTS.Model.Error;
using NTS.Model.ErrorHistory;
using NTS.Model.Errors.ErrorChangePlan;
using NTS.Model.QLTKMODULE;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Error
{
    [RoutePrefix("api/Error")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060430")]
    public class ErrorsController : BaseController
    {
        private readonly ErrorBusiness _business = new ErrorBusiness();

        [Route("SearchError")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060400")]

        public HttpResponseMessage SearchError(ErrorSearchModel modelSearch)
        {
            var result = _business.SearchError(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProblemExist")]
        [HttpPost]
        public HttpResponseMessage SearchProblemExist(ErrorSearchModel modelSearch)
        {
            bool isAll = false;
            if (this.CheckPermission(Constants.Permission_Code_F060431))
            {
                isAll = true;
            }

            var departmentId = this.GetDepartmentIdByRequest();
            var result = _business.SearchProblemExist(modelSearch, departmentId, isAll);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchErrorHistory")]
        [HttpPost]
        public HttpResponseMessage SearchErrorHistory(ErrorHistorySearchModel modelSearch)
        {
            var result = _business.SearchErrorHistory(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        //ChangePlan
        [Route("SearchChangedPlan")]
        [HttpPost]
        public HttpResponseMessage SearchChangedPlan(ErrorHistoryChangePlanModel modelSearch)
        {
            var result = _business.SearchErrorHistoryChangePlan(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        //

        [Route("SearchModule")]
        [HttpPost]
        public HttpResponseMessage SearchModule(ModuleSearchModel modelSearch)
        {
            var result = _business.SearchModule(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProject")]
        [HttpPost]
        public HttpResponseMessage SearchProject(ModuleSearchModel modelSearch)
        {
            var result = _business.SearchProject(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddError")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060401")]
        public HttpResponseMessage AddError(ErrorModel model)
        {
            var userId = GetUserIdByRequest();

            model.DepartmentId = this.GetDepartmentIdByRequest();
            _business.AddError(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateErrorConfirm")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorConfirm(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();
            _business.UpdateErrorConfirm(model, userId, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateErrorPlan")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorPlan(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();
            bool isUpdateByOtherPermistion = CheckPermission("F060432");

            string employeeId = this.GetEmployeeIdByRequest();

            _business.UpdateErrorPlan(model, userId, employeeId, departmentId, isUpdateByOtherPermistion);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateError")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060402;F060403")]
        public HttpResponseMessage UpdateError(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            bool isUpdateByOtherPermistion = CheckPermission("F060403");

            _business.UpdateError(model, isUpdateByOtherPermistion, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteError")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060404;F060405")]
        public HttpResponseMessage DeleteError(ErrorModel model)
        {
            var isDeleteByOtherPermistion = CheckPermission("F060405");
            var userId = GetUserIdByRequest();
            _business.DeleteError(model, isDeleteByOtherPermistion, userId);
            return Request.CreateResponse(HttpStatusCode.OK, new { });
        }

        [Route("GetErrorInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060402;F060403;F060406")]
        public HttpResponseMessage GetErrorInfo(ErrorModel model)
        {
            var result = _business.GetErrorInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060407")]
        public HttpResponseMessage ExportExcel(ErrorSearchModel model)
        {
            var departmentId = this.GetDepartmentIdByRequest();
            string path = _business.ExportExcel(model, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("GetListProject")]
        [HttpPost]
        public HttpResponseMessage GetListProject()
        {
            var result = _business.GetListProject();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetModuleMobile")]
        [HttpPost]
        public HttpResponseMessage GetModuleMobile(string ProjectId)
        {
            var result = _business.GetModuleMobile(ProjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Yêu cầu xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ConfirmRequest")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060409")]
        public HttpResponseMessage ConfirmRequest(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            bool isUpdateByOtherPermistion = CheckPermission("F060403");

            _business.ConfirmRequest(model, isUpdateByOtherPermistion, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy yêu cầu xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelRequest")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060414")]
        public HttpResponseMessage CancelRequest(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CancelRequest(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }       

        /// <summary>
        /// Xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Confirm")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060412")]
        public HttpResponseMessage Confirm(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();
            _business.Confirm(model, userId, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelConfirm")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060412")]
        public HttpResponseMessage CancelConfirm(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();

            _business.CancelConfirm(model, userId, departmentId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Đã có kế hoạch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ConfirmPlan")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060412")]
        public HttpResponseMessage ConfirmPlan(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();
            string employeeId = this.GetEmployeeIdByRequest();
            bool isUpdateByOtherPermistion = CheckPermission("F060432");
            _business.ConfirmPlan(model, userId, employeeId, departmentId, isUpdateByOtherPermistion);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy Đã có kế hoạch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelConfirmPlan")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060412")]
        public HttpResponseMessage CancelConfirmPlan(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();
            string employeeId = this.GetEmployeeIdByRequest();
            bool isUpdateByOtherPermistion = CheckPermission("F060433");
            _business.CancelConfirmPlan(model, userId, employeeId, departmentId, isUpdateByOtherPermistion);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật dữ liệu đang xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateErrorProcess")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorProcess(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateErrorProcess(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Đã xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CompleteProccessing")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060418")]
        public HttpResponseMessage CompleteProccessing(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CompleteProccessing(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy đã xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelCompleteProccessing")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060418")]
        public HttpResponseMessage CancelCompleteProccessing(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();

            _business.CancelCompleteProccessing(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Cập nhật dữ liệu đang xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateErrorQC")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorQC(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateErrorQC(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// QC đạt
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("QCOK")]
        [HttpPost]
        public HttpResponseMessage QCOK(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            _business.QCOK(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// QC không đạt
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("QCNG")]
        [HttpPost]
        public HttpResponseMessage QCNG(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            _business.QCNG(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy kết quả QC
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelResultQC")]
        [HttpPost]
        public HttpResponseMessage CancelResultQC(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CancelResultQC(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Đóng vấn đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CloseError")]
        [HttpPost]
        public HttpResponseMessage CloseError(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CloseError(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy Đóng vấn đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelCloseError")]
        [HttpPost]
        public HttpResponseMessage CancelCloseError(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CancelCloseError(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy Đóng vấn đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateErrorDone")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorDone(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.UpdateErrorDone(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy Đóng vấn đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelDone")]
        [HttpPost]
        public HttpResponseMessage CancelDone(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CancelDone(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
