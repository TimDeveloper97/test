using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Common;
using NTS.Model.CustomerContact;
using NTS.Model.CustomerRequirement;
using NTS.Model.Customers;
using NTS.Model.Meeting;
using NTS.Model.Project;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Customers;
using QLTK.Business.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.Customers
{
    [RoutePrefix("api/Customer")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F060208")]
    public class CustomersController : BaseController
    {
        private readonly CustomersBussiness _business = new CustomersBussiness();
        private readonly CustomerContactBussiness _customerContactBussiness = new CustomerContactBussiness();

        [Route("SearchCustomer")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060200")]
        public HttpResponseMessage SearchCustomer(CustomersSearchModel modelSearch)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060205))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
            }
            var result = _business.SearchCustomer(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchCustomerProject")]
        [HttpPost]
        public HttpResponseMessage SearchCustomerProject(ProjectSearchModel modelSearch, string Id)
        {
            if (!this.CheckPermission(Constants.Permission_Code_F060205))
            {
                modelSearch.SBUId = this.GetSbuIdByRequest();
                modelSearch.DepartmentId = this.GetDepartmentIdByRequest();
            }
            var result = _business.SearchCustomerProject(modelSearch, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListCustomerRequirement")]
        [HttpPost]
        public HttpResponseMessage GetListCustomerRequirement(CustomerRequirementSearchResultModel searchModel, string Id)
        {
            var result = _business.GetListCustomerRequirement(searchModel, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddCustomer")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060201")]
        public HttpResponseMessage AddCustomer(CustomersModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddCustomer(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateCustomer")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060202")]
        public HttpResponseMessage UpdateCustomer(CustomersModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            var sbuId = GetSbuIdByRequest();

            _business.UpdateCustomer(model, sbuId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteCustomer")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060203")]
        public HttpResponseMessage DeleteCustomerType(CustomersModel model)
        {
            var sbuId = GetSbuIdByRequest();
            model.UpdateBy = GetUserIdByRequest();

            _business.DeleteCustomer(model, sbuId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetCustomerInfo")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060202;F060204;F060205")]
        public HttpResponseMessage GetCustomerTypeInfo(CustomersModel model)
        {
            var result = _business.GetCustomerInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        [Route("GetCustomerMeetings")]
        [HttpPost]
        public HttpResponseMessage GetCustomerMeetings(CustomerMeetingSearchResultModel searchModel, string Id)
        {
            var result = _business.GetCustomerMeetings(searchModel, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerCode")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060201")]
        public HttpResponseMessage GetCustomerCode(CustomersModel model)
        {
            var result = _business.GetCustomerCode(model.CodeChar);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060101;F060206")]
        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = _business.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("ImportFile")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060206")]
        public HttpResponseMessage ImportFile()
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _business.ImportFile(userId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060207")]
        public HttpResponseMessage ExportExcel(CustomersModel model)
        {
            string path = _business.ExportExcel(model);
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("CheckDeletes/{customerContactId}")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F060203")]
        public HttpResponseMessage CheckDeleteVillage(string customerContactId)
        {
            _business.CheckDeleteCustomerContact(customerContactId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }

        [Route("GetCustomerContact/{id}")]
        [HttpGet]
        public HttpResponseMessage GetCustomerContact(string id)
        {
            var result = _business.GetCustomerContact(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("contact/{id}")]
        [HttpGet]
        public HttpResponseMessage GetListCustomerContact(string id)
        {
            var result = _business.GetListCustomerContacts(id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("UpdateCustomerContact/{id}")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomerContact(string id, CustomerContactModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            model.CustomerId = id;
            _customerContactBussiness.Create(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("Delete/{customerContactId}")]
        [HttpPost]
        public HttpResponseMessage Checkdelete(string customerContactId)
        {
            _business.Checkdelete(customerContactId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}