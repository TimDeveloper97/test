using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using NTS.Common.Logs;
using System.Web.Http.Controllers;

namespace QLTK.Api.Attributes
{
    public class MonitoringDelegate : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var watcher = Stopwatch.StartNew();
            //string data = request.Content.ReadAsStringAsync().Result;
            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);
            watcher.Stop();

            var routerData = request.GetRouteData();

            if (routerData != null && routerData.Route.DataTokens !=null)
            {
                try
                {
                    var actions = (HttpActionDescriptor[])routerData.Route.DataTokens["actions"];

                    NtsLog.LogInfo($"{  actions[0].ControllerDescriptor.ControllerType.FullName}.{actions[0].ActionName}: Time - {watcher.ElapsedMilliseconds}");
                    //NtsLog.LogInfo($"{  actions[0].ControllerDescriptor.ControllerType.FullName}.{actions[0].ActionName}: Data - {data}");
                }
                catch
                {

                }
            }
            return response;
        }
    }
}