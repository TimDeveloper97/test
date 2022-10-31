using NTS.Model.Payment;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.Projects
{
    [RoutePrefix(prefix: "api/Payment")]
    [ApiHandleExceptionSystem]
    //[NTSIPAuthorize(AllowFeature = "F060506")]
    public class PaymentController : BaseController
    {
        private readonly PaymentBussiness payment = new PaymentBussiness();

        [Route("SearchPaymentModel")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage SearchPaymentModel(string projectId)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            var result = payment.GetAllPayment(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetTotal")]
        [HttpPost]
        public HttpResponseMessage GetTotal(string projectId)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            var result = payment.GetSum(projectId);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("SearchPaymentModelById")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage SearchPaymentById(PaymentModel model)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            var result = payment.GetAllPaymentById(model.Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("UpdatePayment")]
        [HttpPost]
        public HttpResponseMessage UpdatePayment(PaymentModel model)
        {
            payment.UpdatePayment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteById")]
        [HttpPost]
        public HttpResponseMessage DeleteByIdg(string Id)
        {
            payment.DeletePayment(Id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [Route("GetPaymentByPlanId")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage GetPaymentByPlanId(string PlanId)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            var result = payment.GetPaymentByPlanId(PlanId);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
        [Route("UpdatePlanPayment")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage UpdatePlanPayment(PlanPaymentModel model)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            payment.UpdatePlanPayment(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }
        [Route("UpdatePlanDate")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage UpdatePlanDate(PlanPaymentModel model)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            payment.UpdatePlanDate(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }
        [Route("GetPlanByPaymentId")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage GetPlanByPaymentId(string PaymentId)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            var result =payment.GetPlanByPaymentId(PaymentId);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
        [Route("DeletePlanById")]
        [HttpPost]
        //[NTSAuthorize(AllowFeature = "F020125")]
        public HttpResponseMessage DeletePlanById(string PlanId)
        {
            //model.DepartermentUserId = GetDepartmentIdByRequest();
            payment.DeletePlanById(PlanId);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);

        }


    }
}

