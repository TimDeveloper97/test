using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Api.Controllers
{
    public class CallHubContext
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<PCTPMTHub>();

        public static void HubNotifyDocument()
        {
            hubContext.Clients.All.NotifyDocument(new { Message ="Thêm mới khách hàng !"});
        }

        public static void HubNotifyWarning()
        {
            hubContext.Clients.All.Warning(new { Message = "" });
        }
    }
}
