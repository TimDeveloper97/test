using NTS.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace QLTK.Api.Controllers.Common
{
    public class BaseController : ApiController
    {
        protected string GetUserIdByRequest()
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                return claimsPrincipal.Claims.Where(c => c.Type == "userId").Single().Value;
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        protected string GetSbuIdByRequest()
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                return claimsPrincipal.Claims.Where(c => c.Type == "sbuId").Single().Value;
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        protected string GetDepartmentIdByRequest()
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                return claimsPrincipal.Claims.Where(c => c.Type == "departmentId").Single().Value;
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        protected string GetSBUIdByRequest()
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                return claimsPrincipal.Claims.Where(c => c.Type == "sbuId").Single().Value;
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        protected string GetEmployeeIdByRequest()
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                return claimsPrincipal.Claims.Where(c => c.Type == "employeeId").Single().Value;
            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }

        protected List<string> GetSaleGroupByRequest()
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

                var saleGroup = claimsPrincipal.Claims.Where(c => c.Type == "saleGroup").Single().Value;

                if (!string.IsNullOrEmpty(saleGroup))
                {
                    return saleGroup.Split(';').ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return new List<string>();
        }

        protected bool CheckPermission(string functionCode)
        {
            var user = AuthRepository.GetUserAlive(GetUserIdByRequest());

            if (user != null && user.ListPermission != null && !string.IsNullOrEmpty(functionCode))
            {
                if (user.ListPermission.Count > 0 && user.ListPermission.Any(a => a.Equals(functionCode.Trim())))
                {
                    return true;
                }
            }

            //ClaimsPrincipal claimsPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            //string authorize = claimsPrincipal.Claims.Where(c => c.Type == "AuthorizeString").Single().Value;

            //var jss = new JavaScriptSerializer();

            //List<string> listPermission = jss.Deserialize<List<string>>(authorize).ToList();

            //if (listPermission != null && listPermission.Count() > 0)
            //{
            //    if (listPermission.Any(a => a.Equals(functionCode.Trim())))
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
