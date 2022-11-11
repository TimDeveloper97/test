using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NTS.Model.CustomerRequirement;
using NTS.Model.Customers;
using NTS.Model.Solution;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;

namespace QLTK.Api.Controllers.Solutions
{
    [RoutePrefix("api/customer-requirement")]
    [ApiHandleExceptionSystem]
    public class CustomerRequirementController : BaseController
    {
        private readonly CustomerRequirementBussiness _business = new CustomerRequirementBussiness();



        /// <summary>
        /// Thêm yêu cầu khach hang
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateCustomerRequirement(CustomerRequirementCreateModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateCustomerRequirement(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Tự động tạo mã yêu cầu
        /// </summary>
        /// <returns></returns>
        [Route("GenerateCode")]
        [HttpPost]
        public HttpResponseMessage GenerateCode()
        {
            var result = _business.GenerateCode();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Xóa yêu cầu khach hang
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpPost]
        public HttpResponseMessage Delete(string id)
        {
            _business.DeleteCustomerRequirement(id, GetUserIdByRequest());
            return Request.CreateErrorResponse(HttpStatusCode.OK, string.Empty);
        }



        [Route("get-by-id/{id}")]
        [HttpGet]
        public HttpResponseMessage GetCandidateById(string id)
        {
            var result = _business.GetCustomerRequirementById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("getCustomerId")]
        [HttpPost]
        public HttpResponseMessage getCustomerId(string id)
        {
            var result = _business.getCustomerId(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        

        /// <summary>
        /// Sửa yêu cầu khach hang
        /// </summary>
        /// <returns></returns>
        [Route("update/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerRequirement(string id, CustomerRequirementInfoModel model)
        {
           _business.UpdateCustomerRequirement (id, model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        //[Route("UpdateCustomerRequirementContent")]
        //[HttpPost]
        //public HttpResponseMessage UpdateCustomerRequirementContent(string id, CustomerRequirementContentModel model)
        //{
        //    _business.UpdateCustomerRequirementContent(id, model);
        //    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //}

        [Route("CreateCustomerRequirementContent")]
        [HttpPost]
        public HttpResponseMessage CreateCustomerRequirementContent(string id, CustomerRequirementContentModel model)
        {
            _business.CreateCustomerRequirementContent(id, model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Tìm kiếm yêu cầu khach hang
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Route("search")]
        [HttpPost]
        public HttpResponseMessage SearchChannel(CustomerRequirementSearchResultModel searchModel)
        {
            var result = _business.SearchCustomerRequirement(searchModel, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("NextStep")]
        [HttpPost]
        public HttpResponseMessage NextStep(CustomerRequirementCreateModel model)
        {
            _business.NextStep(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("BackStep")]
        [HttpPost]
        public HttpResponseMessage BackStep(CustomerRequirementCodeModel model)
        {
            _business.BackStep(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        
        [Route("NextThreeStep")]
        [HttpPost]
        public HttpResponseMessage NextThreeStep(CustomerRequirementCreateModel model)
        {
            _business.NextThreeStep(model, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("BackThreeStep")]
        [HttpPost]
        public HttpResponseMessage BackThreeStep(CustomerRequirementCreateModel model)
        {
            _business.BackThreeStep(model.Id, GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CheckDelete/{customerRequirementId}")]
        [HttpDelete]
        public HttpResponseMessage CheckDeleteVillage(string customerRequirementId)
        {
            _business.CheckDeleteSurvey(customerRequirementId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("GetSurvey/{id}")]
        [HttpGet]
        public HttpResponseMessage GetCustomerContact(string id)
        {
            var result = _business.GetSurvey(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetDomain")]
        [HttpPost]
        public HttpResponseMessage GetDomain(string customerId)
        {
            var result = _business.GetDomain(customerId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetRequestName")]
        [HttpPost]
        public HttpResponseMessage GetRequestName(string id)
        {
            var result = _business.GetRequestName(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerContactById")]
        [HttpPost]
        public HttpResponseMessage GetCustomerContactById(string customerId)
        {
            var result = _business.GetCustomerContactById(customerId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-product-need-price/{id}")]
        [HttpGet]
        public HttpResponseMessage GetProductNeedPriceById(string id)
        {
            var result = _business.GetProductNeedPriceById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("get-product-need-solution/{id}")]
        [HttpGet]
        public HttpResponseMessage GetProductNeedSolutionById(string id)
        {
            var result = _business.GetProductNeedSolutionById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetCustomerContact")]
        [HttpPost]
        public HttpResponseMessage GetCustomerContact()
        {
            var result = _business.GetCustomerContact();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("get-customer-requirement-product-solution")]
        [HttpPost]
        public HttpResponseMessage GetCustomerRequirementProductSolutionById(CustomerRequirementSolutionContentModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            var result = _business.GetCustomerRequirementProductSolutionById(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("createUpdateCustomerRequirementContent")]
        [HttpPost]
        public HttpResponseMessage CreateUpdateCustomerRequirementContent(CustomerRequirementContentModel model)
        {   
           _business.CreateUpdateCustomerRequirementContent(model);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("deleteCustomerRequirementContent/{id}")]
        [HttpPost]
        public HttpResponseMessage DeleteCustomerRequirementContent(string id)
        {
            _business.DeleteCustomerRequirementContent(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("SearchCustomerRequirementContentModelById")]
        [HttpPost]
        public HttpResponseMessage SearchCustomerRequirementContentModelById(string id)
        {
            var result = _business.SearchCustomerRequirementContentModelById(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}