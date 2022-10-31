using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace NTS.Api.SignalR
{
    public class test : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}