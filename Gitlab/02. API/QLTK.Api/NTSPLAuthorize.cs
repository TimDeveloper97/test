using NTS.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;
using NTS.Api.Models;
using Newtonsoft.Json;

namespace QLTK.Api
{
    public class NTSPLAuthorize : System.Web.Http.Filters.ActionFilterAttribute
    {
        public string AllowFeature { get; set; }
        private string userName = string.Empty;
        private string userId = string.Empty;
        
        private string authorizeString = string.Empty;
        private string authorizeParkingLotString = string.Empty;
        private ClaimsPrincipal principal;
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var filterAttribute = actionContext.ActionDescriptor.GetCustomAttributes<NTSAuthorize>(true)
                .Where(a => a.GetType() == typeof(NTSAuthorize));

            if (filterAttribute != null)
            {
                foreach (NTSAuthorize attr in filterAttribute)
                {
                    AllowFeature = attr.AllowFeature;
                }
                principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

                try
                {
                    userName = principal.Claims.Where(c => c.Type == "user").Single().Value;
                    userId = principal.Claims.Where(c => c.Type == "userId").Single().Value;

                    bool isTokenAlive = false;
                    isTokenAlive = AuthRepository.IsTokenAlive(userId);

                    if (!isTokenAlive)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.");
                    }
                    else
                    {
                        if (actionContext.ActionArguments.Count > 0)
                        {
                            var entity = actionContext.ActionArguments.First();
                            var parkingLotId = entity.Value.GetType().GetProperty("ParkingLotId").GetValue(entity.Value, null);
                            principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                            authorizeParkingLotString = principal.Claims.Where(c => c.Type == "AuthorizeParkingLotString").Single().Value;
                        }
                        else
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Bạn không có quyền thao tác chức năng này.");
                        }
                    }

                }
                catch
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.");
                }
            }

            base.OnActionExecuting(actionContext);
        }


    }
}