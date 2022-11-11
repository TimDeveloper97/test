using NTS.Business.Combobox;
using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.Error;
using NTS.Model.Mobile;
using NTS.Model.Mobile.Error;
using NTS.Model.User;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Errors;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Error
{
    [RoutePrefix("api/ErrorMobile")]
    [Authorize]
    public class ErrorMobileController : BaseController
    {
        private readonly ErrorBusiness _business = new ErrorBusiness();
        private readonly UserBusiness _businessUser = new UserBusiness();
        private readonly ComboboxBusiness _combobox = new ComboboxBusiness();
        [Route("SearchProblemExistMobile")]
        [HttpPost]
        public HttpResponseMessage SearchProblemExist(ErrorSearchCondition modelSearch)
        {
            // Kiểm tra xem tài khoản có quyền xem tất cả vấn đề của tất cả dự án hay không
            modelSearch.IsAllPermission = CheckPermission("F060434");
            var departmentId = this.GetDepartmentIdByRequest();
            var result = _business.SearchErrorsMobile(modelSearch, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("need-confirm")]
        [HttpPost]
        public HttpResponseMessage SearchErrorNeedConfirm(ErrorSearchCondition modelSearch)
        {
            // lấy thông tin tài khoản có quyền Xác nhận vấn đề
            bool hasPermit = CheckPermission("F060411");
            ErrorSearchResult result = new ErrorSearchResult();
            if (hasPermit)
            {
                var departmentId = this.GetDepartmentIdByRequest();
                modelSearch.Status = Constants.Problem_Status_Awaiting_Confirm;
                result = _business.SearchErrorsMobile(modelSearch, departmentId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("need-close")]
        [HttpPost]
        public HttpResponseMessage SearchErrorNeedClose(ErrorSearchCondition modelSearch)
        {
            // lấy thông tin tài khoản có quyền Đóng vấn đề
            var departmentId = this.GetDepartmentIdByRequest();
            modelSearch.Status = Constants.Problem_Status_Awaiting_QC;
            modelSearch.ErrorBy = this.GetEmployeeIdByRequest();
            var result = _business.SearchErrorsMobile(modelSearch, departmentId);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("statistic")]
        [HttpGet]
        public HttpResponseMessage StatisticErrors()
        {
            // lấy thông tin tài khoản có quyền Xác nhận vấn đề
            bool hasPermitConfirm = CheckPermission("F060411");
            bool hasPermitClose = CheckPermission("F060428");

            string employeeId = this.GetEmployeeIdByRequest();
            var result = _business.Statistic(employeeId, hasPermitConfirm, hasPermitClose);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddErrorMobile")]
        [HttpPost]
        public HttpResponseMessage AddErrorMobile(ErrorModel model)
        {
            try
            {
                string userId = GetUserIdByRequest();
                model.DepartmentId = this.GetDepartmentIdByRequest();
                model.Status = 1;
                _business.AddError(model, userId);
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = "1",
                      Data = "Thêm mới thành công"
                  });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = "0",
                      Message = ex.Message
                  });
            }

        }

        [Route("GetErrorInfoMobile")]
        [HttpPost]
        public HttpResponseMessage GetErrorInfoMobile(ErrorModel model)
        {
            try
            {
                var returnErrorInfo = _business.GetErrorInfo(model);
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseSuccess,
                      Data = returnErrorInfo
                  });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseError,
                      Message = ex.Message
                  });
            }

        }

        [Route("UpdateErrorMobile")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorMobile(ErrorModel model)
        {
            try
            {
                string userId = GetUserIdByRequest();
                bool isUpdateByOtherPermistion;
                isUpdateByOtherPermistion = CheckPermission("F060403");
                _business.UpdateError(model, isUpdateByOtherPermistion, userId);

                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseSuccess,
                      Data = "Chỉnh sửa thành công"
                  });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseError,
                      Message = ex.Message
                  });
            }

        }


        [Route("ChangePassword")]
        [HttpPost]
        public HttpResponseMessage ChangePasswordMobile(UserModel model)
        {
            try
            {
                model.Id = GetUserIdByRequest();
                _businessUser.ChangePassword(model);
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseSuccess,
                      Data = "Đổi mật khẩu thành công"
                  });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseError,
                      Message = ex.Message
                  });
            }

        }

        [Route("GetProblemCode")]
        [HttpPost]
        public HttpResponseMessage GetProblemCode(int type)
        {
            try
            {
                var returnCode = _combobox.GetCodeProblem(type);
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseSuccess,
                      Data = returnCode,
                  }); ;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                  new ResultModel<object>
                  {
                      Status = Constants.ResponseError,
                      Message = ex.Message
                  });
            }

        }

        [Route("GetModuleMobile")]
        [HttpPost]
        public HttpResponseMessage GetModuleMobile(string ProjectId)
        {
            var result = _business.SearchModuleMobile(ProjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetProductMobile")]
        [HttpPost]
        public HttpResponseMessage GetProductMobile(string ProjectId)
        {
            var result = _business.SearchProductMobile(ProjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchErrorFixMobile")]
        [HttpPost]
        public HttpResponseMessage SearchErrorFix(ErrorSearchModel modelSearch)
        {
            var departmentId = this.GetDepartmentIdByRequest();
            modelSearch.FixBy = this.GetEmployeeIdByRequest();
            var result = _business.SearchErrorFixMobile(modelSearch, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật dữ liệu đang xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UpdateErrorProcess")]
        [HttpPost]
        public HttpResponseMessage UpdateErrorProcess(ErrorFixModel model)
        {
            var userId = GetUserIdByRequest();
            model.EmployeeFixId = GetEmployeeIdByRequest();
            _business.UpdateErrorProcessMobile(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetErrorFixInfo")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F060402;F060403;F060406")]
        public HttpResponseMessage GetErrorInfo(ErrorFixModel model)
        {
            var result = _business.GetErrorFixInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
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
            model.ActualFinishDate = DateTime.Now;
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
            model.ActualFinishDate = null;
            _business.QCNG(model, userId);

            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }


        /// <summary>
        /// Xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Confirm")]
        [HttpPost]
        public HttpResponseMessage Confirm(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            var departmentId = this.GetDepartmentIdByRequest();
            _business.Confirm(model, userId, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Hủy yêu cầu xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("CancelRequest")]
        [HttpPost]
        public HttpResponseMessage CancelRequest(ErrorStatusModel model)
        {
            var userId = GetUserIdByRequest();
            _business.CancelRequest(model, userId);
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
        /// Yêu cầu xác nhận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("RequestConfirm")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060409")]
        public HttpResponseMessage ConfirmRequest(ErrorModel model)
        {
            var userId = GetUserIdByRequest();
            bool isUpdateByOtherPermistion = CheckPermission("F060403");

            _business.ConfirmRequest(model, isUpdateByOtherPermistion, userId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
