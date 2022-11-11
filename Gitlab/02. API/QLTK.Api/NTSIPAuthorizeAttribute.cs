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
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Configuration;
using NTS.Common.Logs;
using Microsoft.Owin;

namespace QLTK.Api
{
    public class NTSIPAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public string AllowFeature { get; set; }
        private string userId = string.Empty;
        private ClaimsPrincipal principal;
        private string[] allowFeatureList;
        private bool isAuthorize;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException("actionContext");

            try
            {
                string userIpAddress = string.Empty;
                if (actionContext.Request.Properties.ContainsKey("MS_HttpContext"))
                {
                    userIpAddress = IPAddress.Parse(((HttpContextBase)actionContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
                }

                if (actionContext.Request.Properties.ContainsKey("MS_OwinContext"))
                {
                    userIpAddress = IPAddress.Parse(((OwinContext)actionContext.Request.Properties["MS_OwinContext"]).Request.RemoteIpAddress).ToString();
                }

                bool finallyAllowed = true;

                // Get config IsCheck
                bool isCheck = Convert.ToBoolean(ConfigurationManager.AppSettings["Authorize-IP-IsCheck"]);
                bool isLog = Convert.ToBoolean(ConfigurationManager.AppSettings["Authorize-IP-Log"]);

                if (isCheck)
                {
                    var ipAccept = ConfigurationManager.AppSettings["Authorize-IP-Accept"].Split(';').ToList();

                    finallyAllowed = ipAccept.IndexOf(userIpAddress) >= 0;

                    if (isLog)
                    {
                        NtsLog.LogError("Kết quả kiểm tra ngoài TPA " + finallyAllowed.ToString());
                    }
                }

                if (!finallyAllowed)
                {
                    if (isLog)
                    {
                        NtsLog.LogError(userIpAddress);
                    }

                    principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                    userId = principal.Claims.Where(c => c.Type == "userId").Single().Value;
                    var userAlive = AuthRepository.GetUserAlive(userId);
                    if (userAlive == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.");
                    }
                    else
                    {
                        if (AllowFeature != null && !CheckRole(AllowFeature, userAlive.ListPermission))
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Bạn không có quyền thao tác dữ liệu này.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Bạn đã hết phiên làm việc. Bạn hãy đăng nhập lại để tiếp tục.");
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
    }
}