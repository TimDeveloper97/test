using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NTS.Common.Logs;
using Owin;

namespace QLTK.Service.Business.WebServer
{
   public class NTSWebServer
    {
        private IDisposable signalR { get; set; }
        private string serverURI = "http://localhost:2712";

        public bool StartServer()
        {
            try
            {
                signalR = WebApp.Start(serverURI);
                return true;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                return false;
            }
        }

        public bool StopServer()
        {
            try
            {
                signalR.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                return false;
            }
        }
    }
}
