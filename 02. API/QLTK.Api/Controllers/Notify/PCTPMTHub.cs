using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Api.Controllers
{
    public class PCTPMTHub:Hub
    {
        public void NotifyDocument()
        {
            Clients.All.NotifyDocument();
        }
    }
}
