using NTS.Common.Logs;
using QLTK.Service.Business.WebServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service
{
    public partial class QLTKService : ServiceBase
    {
        private NTSWebServer nTSWebServer = new NTSWebServer();

        public QLTKService()
        {
            //StartServer();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           StartServer();
        }

        protected override void OnStop()
        {
            StopServer();
        }

        private void StartServer()
        {
            nTSWebServer.StartServer();
        }

        private void StopServer()
        {
            nTSWebServer.StopServer();
        }
    }

}
