using NTS.Api.Repositories;
using NTS.Business;
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


namespace QLTK.Api
{
    public class NTSAuthorize : System.Web.Http.Filters.AuthorizationFilterAttribute
    {
        public string AllowFeature { get; set; }
        private string userName = string.Empty;
        private string userId = string.Empty;
        
        private string authorizeString = string.Empty;
        private string authorizeItemString = string.Empty;
        private ClaimsPrincipal principal;
        private string[] allowFeatureList;
        private bool isAuthorize;
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
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

                    var userAlive = AuthRepository.GetUserAlive(userId);

                    if (userAlive == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.");
                    }
                    else
                    {   //authorizeString = principal.Claims.Where(c => c.Type == "AuthorizeString").Single().Value;
                        //var jss = new JavaScriptSerializer();
                        //List<string> listPermission = jss.Deserialize<List<string>>(authorizeString).ToList();
                        //if not have permission
                        if (AllowFeature != null && !CheckRole(AllowFeature, userAlive.ListPermission))
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Bạn không có quyền thao tác dữ liệu này.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.");
                }
            }

            base.OnAuthorization(actionContext);
        }

        private bool CheckRole(string allowFeature, List<string> listPermission)
        {
            isAuthorize = false;
            allowFeatureList = allowFeature.Split(';');
            var jss = new JavaScriptSerializer();
            //List<string> listPermission = jss.Deserialize<List<string>>(authorize).ToList();
            if (listPermission != null && listPermission.Count() > 0)
            {
                foreach (var item in allowFeatureList)
                {
                    if (listPermission.Any(a => a.Equals(item.Trim())))
                    {
                        isAuthorize = true;
                    }
                }
            }

            return isAuthorize;
        }

        public class ParkingLotAction : System.Web.Http.Filters.ActionFilterAttribute
        {
            private string userName = string.Empty;
            private string authorizeString = string.Empty;
            private string authorizeItemString = string.Empty;
            private ClaimsPrincipal principal;
            public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
            {
                if (actionContext.ActionArguments.Count > 0)
                {
                    var entity = actionContext.ActionArguments.First();
                    var projectId = entity.Value.GetType().GetProperty("ParkingLotId").GetValue(entity.Value, null);
                    principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                    authorizeItemString = principal.Claims.Where(c => c.Type == "AuthorizeItemString").Single().Value;
                    authorizeString = principal.Claims.Where(c => c.Type == "AuthorizeString").Single().Value;

                    if (!authorizeString.Contains("admin") && (projectId == null || !authorizeItemString.Contains(projectId.ToString())))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Bạn không có quyền thao tác dữ liệu này.");
                    }
                }
                base.OnActionExecuting(actionContext);
            }

        }
    }
}