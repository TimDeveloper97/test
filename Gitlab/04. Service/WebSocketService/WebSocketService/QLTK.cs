using Microsoft.Win32;
using QLTK.Service.Business.WebServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebSocketService
{
    public partial class QLTK : Form
    {
        const string ServerURI = "http://localhost:8080";
        public QLTK()
        {
            InitializeComponent();
        }

        private void QLTK_Load(object sender, EventArgs e)
        {
            try
            {
                tsmVersion.Text = "Version: " + ConfigurationManager.AppSettings["version"];
                NTSWebServer nTSWebServer = new NTSWebServer();
                nTSWebServer.StartServer();
                var direct = Application.StartupPath + "/config";
                if (!Directory.Exists(direct))
                {
                    Directory.CreateDirectory(direct);
                }

                var fileConfig = Application.StartupPath + "/config/config.txt";
                if (File.Exists(fileConfig))
                {
                    string text = File.ReadAllText(fileConfig);
                    if (!string.IsNullOrEmpty(text))
                    {
                        Hide();
                        ShowInTaskbar = false;
                        notifyQLTK.Visible = true;
                        if (text.Equals("check"))
                        {
                            chkStartAutomatic.Checked = true;
                        }
                    }
                }
                else
                {
                    using (FileStream fs = File.Create(fileConfig))
                    {
                    }
                    //File.Create(fileConfig);
                }

            }
            catch (TargetInvocationException)
            {
                MessageBox.Show("Server failed to start. A server is already running on " + ServerURI);
                return;
            }
        }

        private void QLTK_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyQLTK.Visible = true;
            }
        }

        private void NotifyQLTK_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            //notifyQLTK.Visible = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (chkStartAutomatic.Checked)
            {
                AddApplicationToStartup();
            }
            else
            {
                RemoveApplicationFromStartup();
            }
            Hide();
            ShowInTaskbar = false;
            notifyQLTK.Visible = true;
        }

        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("QLTKService", "\"" + Application.ExecutablePath + "\"");
                var fileConfig = Application.StartupPath + "/config/config.txt";
                if (File.Exists(fileConfig))
                {
                    File.WriteAllText(fileConfig, "check");
                }

            }
        }

        public static void RemoveApplicationFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("QLTKService", false);
                var fileConfig = Application.StartupPath + "/config/config.txt";
                if (File.Exists(fileConfig))
                {
                    File.WriteAllText(fileConfig, string.Empty);
                }
            }
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void tsmUpdate_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Application.StartupPath, "UpdateVersion/UpdateService.exe"));
        }
    }
}
