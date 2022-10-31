using Microsoft.AspNet.SignalR;
using NTS.Model.Report;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ReportStatusModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using QLTK.Api.Attributes;
using QLTK.Business.Reports;

namespace QLTK.Api.Controllers.Reports
{
    public class Timeline
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    
    public class PostEmployee : Timeline
    {
        public string CodeDepartment { get; set; }
        public List<string> CodeEmployees { get; set; }
    }


    [RoutePrefix("api/ReportBussiness")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F100013")]
    public class ReportSaleController : BaseController
    {
        private readonly ReportSaleBussiness _bussiness = new ReportSaleBussiness();

        #region Region
        [HttpPost]
        [Route("SalesTargetRegion")]
        public HttpResponseMessage SalesTargetRegion(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesTargetRegion(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesRealityRegion")]
        public HttpResponseMessage SalesRealityByRegion(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesRealityByRegion(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Job - Application - Industry
        [HttpPost]
        [Route("SalesJob")]
        public HttpResponseMessage SalesJob(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesJob(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesIndustry")]
        public HttpResponseMessage SalesIndustry(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesIndustry(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesApplication")]
        public HttpResponseMessage SalesApplication(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesApplication(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region NV, PB kinh doanh
        [HttpGet]
        [Route("Departments")]
        public HttpResponseMessage Departments()
        {
            try
            {
                var result = _bussiness.Departments();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Employees")]
        public HttpResponseMessage Employees(string code)
        {
            try
            {
                var result = _bussiness.Employees(code);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesEmployeeByDepartment")]
        public HttpResponseMessage SalesEmployeeByDepartment(PostEmployee post)
        {
            try
            {
                var result = _bussiness.SalesEmployeeByDepartment(post.From, post.To, post.CodeDepartment, post.CodeEmployees);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesEmployee")]
        public HttpResponseMessage SalesEmployee(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesEmployee(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("SalesDepartment")]
        public HttpResponseMessage SalesDepartment(Timeline sale)
        {
            try
            {
                var result = _bussiness.SalesDepartment(sale.From, sale.To);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}
