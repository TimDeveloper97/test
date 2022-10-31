using NTS.Model.Quotation;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Solutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace QLTK.Api.Controllers.SummaryQuote
{
    [RoutePrefix("api/SummaryQuotes")]
    [ApiHandleExceptionSystem]

    public class SummaryQuotesController : BaseController
    {
        private readonly SummaryQuotesBussiness _business = new SummaryQuotesBussiness();

        [Route("GetCustomerById")]
        [HttpPost]
        public HttpResponseMessage GetCustomerById(string Id)
        {
            var result = _business.GetCustomerById(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetQuotationById")]
        [HttpPost]
        public HttpResponseMessage GetQuotationById(string Id)
        {
            var result = _business.GetQuotationById(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetFileInfor")]
        [HttpPost]
        public HttpResponseMessage GetFileInfor(string Id)
        {
            var result = _business.GetFileInfor(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetCustomerRequire")]
        [HttpPost]
        public HttpResponseMessage GetCustomerRequire()
        {
            var result = _business.GetCustomerRequire();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetRequireByNumberYCKH")]
        [HttpPost]
        public HttpResponseMessage GetRequireByNumberYCKH(string number)
        {
            var result = _business.GetRequireByNumberYCKH(number);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetQuotesBySBU")]
        [HttpPost]
        public HttpResponseMessage GetQuotesBySBU(string SBUid)
        {
            var result = _business.GetQuotesBySBU(SBUid);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("CheckSoldQuotation")]
        [HttpPost]
        public HttpResponseMessage CheckSoldQuotation(string quotationId)
        {
            var result = _business.CheckSoldQuotation(quotationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetEmployeeCharge")]
        [HttpPost]
        public HttpResponseMessage GetEmployeeCharge(string quotationId)
        {
            var result = _business.GetEmployeeCharge(quotationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListEmployee")]
        [HttpPost]
        public HttpResponseMessage GetCbbEmployee()
        {
            var result = _business.GetCbbEmployee();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddQuote")]
        [HttpPost]
        public HttpResponseMessage AddQuote(QuotationModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddQuote(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateQuote")]
        [HttpPost]
        public HttpResponseMessage UpdateQuote(QuotationModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.UpdateQuote(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ChangeStatusQuotation")]
        [HttpPost]
        public HttpResponseMessage ChangeStatusQuotation(string quotationId)
        {
            var CreateBy = GetUserIdByRequest();
            _business.ChangeStatusQuotation(quotationId, CreateBy);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("CreateQuotationPlan")]
        [HttpPost]
        public HttpResponseMessage CreateQuotationPlan(QuotationPlanModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.CreateQuotationPlan(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("AddProduct")]
        [HttpPost]
        public HttpResponseMessage AddProduct(QuotationProductModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.AddProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateProduct")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(QuotationProductModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.UpdateProduct(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("UpdateQuotationPlan")]
        [HttpPost]
        public HttpResponseMessage UpdateQuotationPlan(QuotationPlanModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _business.UpdateQuotationPlan(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        

        [Route("DeleteProduct")]
        [HttpPost]
        public HttpResponseMessage DeleteProduct(string Id)
        {
            _business.DeleteProduct(Id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteQuotation")]
        [HttpPost]
        public HttpResponseMessage DeleteQuotation(string Id)
        {
            _business.DeleteQuotation(Id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
        
        [Route("DeleteQuotationByQuotationId")]
        [HttpPost]
        public HttpResponseMessage DeleteQuotationByQuotationId(string Id)
        {
            _business.DeleteQuotationByQuotationId(Id);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetAllQuotationInfo")]
        [HttpPost]
        public HttpResponseMessage GetAllQuotationInfo(QuotationSearchModel modelSearch)
        {
           
            var result = _business.GetAllQuotationInfo(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        [Route("GetQuotationByCustomerId")]
        [HttpPost]
        public HttpResponseMessage GetQuotationByCustomerId(QuotationSearchModel modelSearch, string CustomerId)
        {

            var result = _business.GetQuotationByCustomerId(modelSearch, CustomerId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ChangeIndustry")]
        [HttpPost]
        public HttpResponseMessage ChangeIndustry(string Id)
        {
            var result = _business.ChangeIndustry(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ChangeModule")]
        [HttpPost]
        public HttpResponseMessage ChangeModule(string ObjectId)
        {
            var result = _business.ChangeModule(ObjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ChangeProduct")]
        [HttpPost]
        public HttpResponseMessage ChangeProduct(string ObjectId)
        {
            var result = _business.ChangeProduct(ObjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ChangeSaleProduct")]
        [HttpPost]
        public HttpResponseMessage ChangeSaleProduct(string ObjectId)
        {
            var result = _business.ChangeSaleProduct(ObjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ChangeMaterial")]
        [HttpPost]
        public HttpResponseMessage ChangeMaterial(string ObjectId)
        {
            var result = _business.ChangeMaterial(ObjectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetQuotationProduct")]
        [HttpPost]
        public HttpResponseMessage GetQuotationProduct(string Id)
        {
            var result = _business.GetQuotationProduct(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetQuotationProductInfor")]
        [HttpPost]
        public HttpResponseMessage GetQuotationProductInfor(string Id)
        {
            var result = _business.GetQuotationProductInfor(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
        [Route("GetQuotationPlan")]
        [HttpPost]
        public HttpResponseMessage GetQuotationPlan(string Id)
        {
            var result = _business.GetQuotationPlan(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetGroupInTemplate")]
        [HttpPost]

        public HttpResponseMessage GetGroupInTemplate()
        {
            string path = _business.GetGroupInTemplate();
            return Request.CreateResponse(HttpStatusCode.OK, path);
        }

        [Route("QuotationProductImportFile")]
        [HttpPost]
        public HttpResponseMessage ImportFile(string QuotationId)
        {
            string userId = GetUserIdByRequest();
            HttpFileCollection hfc = HttpContext.Current.Request.Files;
            if (hfc.Count > 0)
            {
                _business.ImportFile(userId, QuotationId, hfc[0]);
            }
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetQuotationPlanById")]
        [HttpPost]
        public HttpResponseMessage GetQuotationPlanById(string Id)
        {
            var result = _business.GetQuotationPlanById(Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListQuotationStep")]
        [HttpPost]
        public HttpResponseMessage GetListQuotationStep(string QuotationId)
        {
            var result = _business.GetListQuotationStep(QuotationId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("ExportExcel")]
        [HttpPost]
        public HttpResponseMessage ExportExcel(QuotationSearchModel model, string QuotationId)
        {
            try
            {
                string path = _business.ExportExcel(model, QuotationId);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}