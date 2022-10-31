using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace UpdateService
{
    public partial class UpdateServiceView : Form
    {
        private string fileUrl = string.Empty;
        private VersionModel versionInfo;
        private string keyAuthorize;
        private string folderParentPath = string.Empty;

        public UpdateServiceView()
        {
            InitializeComponent();
        }

        private void UpdateServiceView_Load(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            lblMessage.Text = "Không có phiên bản mới của phần mềm!";

            // Lấy path thư mục cha
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
            folderParentPath = di.Parent.FullName;

            // Lấy thông tin vesion trong config
            string configFile = System.IO.Path.Combine(folderParentPath, "WebSocketService.exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            keyAuthorize = config.AppSettings.Settings["keyAuthorize"].Value;
            lblVersionCurrent.Text = config.AppSettings.Settings["version"].Value;
            fileUrl = config.AppSettings.Settings["fileUrl"].Value;

            // Lấy thông tin version mới
            var resultAPI = GetNewVersion(config.AppSettings.Settings["apiUrl"].Value, lblVersionCurrent.Text);
            if (!resultAPI.SuccessStatus)
            {
                MessageBox.Show("Không kiểm tra được phiên bản trên server", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                var versionInfo = resultAPI.Data;
                if (versionInfo != null)
                {
                    lblMessage.Text = "Có bản cập nhật mới. Bạn hãy cập nhật phần mềm!";
                    lblVersionNew.Text = versionInfo.VersionNew;
                    btnUpdate.Enabled = versionInfo.IsUpdate;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }

        /// <summary>
        /// Cập nhật phần mềm
        /// </summary>
        private void Update()
        {
            if (versionInfo != null && versionInfo.IsUpdate)
            {
                Cursor.Current = Cursors.WaitCursor;
                lblMessage.Text = "Đang cập nhật phần mềm!";

                // Xóa file zip update
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, "UpdateFileZip"));
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                // Download file update
                string pathSave = Path.Combine(Application.StartupPath, "UpdateFileZip", versionInfo.VersionNew + ".zip");
                if (DownloadUpdateFile(fileUrl, versionInfo.Path, versionInfo.VersionNew + ".zip", pathSave))
                {
                    // Tắt service
                    InterupApplication("WebSocketService");

                    Thread.Sleep(5000);
                    //ZipFile.ExtractToDirectory(pathSave, folderParentPath);

                    // Giải nén
                    ZipFile zipFile = null;
                    try
                    {
                        FileStream fileStream = File.OpenRead(pathSave);

                        zipFile = new ZipFile(fileStream);
                        string entryFileName = string.Empty;

                        foreach (ZipEntry zipEntry in zipFile)
                        {
                            entryFileName = zipEntry.Name;
                            byte[] buffter = new byte[4096];
                            Stream zipStream = zipFile.GetInputStream(zipEntry);
                            String fullZipPath = Path.Combine(folderParentPath, entryFileName);
                            // Check entry is directory
                            if (zipEntry.IsDirectory)
                            {
                                DirectoryInfo dir = new DirectoryInfo(fullZipPath);
                                dir.Create();
                                continue;
                            }
                            using (FileStream streamWriter = File.Create(fullZipPath))
                            {
                                StreamUtils.Copy(zipStream, streamWriter, buffter);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        NtsLog.LogError(ex);
                    }
                    finally
                    {
                        if (zipFile != null)
                        {
                            zipFile.IsStreamOwner = true;
                            zipFile.Close();
                        }
                    }

                    lblVersionCurrent.Text = versionInfo.VersionNew;
                    lblMessage.Text = "Cập nhật phần mềm thành công!";

                    // Cập nhạt vesion vào config
                    string configFile = System.IO.Path.Combine(folderParentPath, "WebSocketService.exe.config");
                    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                    configFileMap.ExeConfigFilename = configFile;
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                    config.AppSettings.Settings["version"].Value = versionInfo.VersionNew;
                    config.Save();


                    Process.Start(Path.Combine(folderParentPath, "WebSocketService.exe"));
                }

                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// End processes
        /// </summary>
        /// <param name="applicationName"></param>
        private void InterupApplication(string applicationName)
        {
            Process[] processes;
            processes = Process.GetProcessesByName(applicationName);

            foreach (Process proc in processes)
            {

                try
                {
                    int procID = System.Convert.ToInt32(proc.Id);
                    Process tempProc = Process.GetProcessById(procID);

                    //tempProc.CloseMainWindow();                    
                    tempProc.Kill();
                    //tempProc.WaitForExit();
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Lấy thông tin version mới
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private ResultApiModel<VersionModel> GetNewVersion(string apiUrl, string version)
        {
            var client = new RestClient(apiUrl + "WebService/GetNewVersion");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("version", version);

            ResultApiModel<VersionModel> resultModel = new ResultApiModel<VersionModel>();
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ResultApiModel<VersionModel>>(response.Content);
            }
            else
            {
                NtsLog.LogError(new Exception(response.Content));
            }

            return resultModel;
        }

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="pathSave"></param>
        /// <returns></returns>
        private bool DownloadUpdateFile(string fileUrl, string path, string fileName, string pathSave)
        {
            var client = new RestClient(fileUrl);
            var request = new RestRequest("download/download-file", Method.GET);

            request.AddParameter("pathFile", path);
            request.AddParameter("fileName", fileName);
            request.AddParameter("keyAuthorize", keyAuthorize);

            var data = client.DownloadData(request, true);

            if (data == null || data.Length == 0)
            {
                return false;
            }

            data.SaveAs(pathSave);

            return true;
        }
    }
}
