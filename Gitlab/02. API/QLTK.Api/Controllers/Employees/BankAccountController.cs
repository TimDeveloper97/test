using NTS.Model.BankAccount;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.BankAccount
{
    [RoutePrefix("api/bankaccount")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F121204")]
    public class BankAccountController : BaseController
    {
        private readonly BankAccountBussiness _business = new BankAccountBussiness();

        /// <summary>
        /// Tìm kiếm ngân hàng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121200")]
        public HttpResponseMessage SearchBankAccount(BankAccountSearchModel searchModel)
        {
            var result = _business.SearchBankAccount(searchModel);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        /// <summary>
        /// Xóa ngân hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121203")]
        public HttpResponseMessage DeleteJobPositions(BankAccountModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.DeleteBankAccount(model);
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Thêm ngân hàng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121201")]
        public HttpResponseMessage AddJobPositions(BankAccountModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateBankAccount(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Lấy thông tin ngân hàng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [Route("getBankAccountInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121202")]
        public HttpResponseMessage GetBankAccount(BankAccountModel modelSearch)
        {

            var result = _business.GetBankAccount(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Cập nhật chức vụ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F121202")]
        public HttpResponseMessage UpdateJobPositions(BankAccountModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _business.UpdateBankAccount(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
