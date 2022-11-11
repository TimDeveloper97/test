using NTS.Common.Logs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.FTP
{
    public class NtsFTPClient
    {
        public NetworkCredential Credentials
        {
            get;
            set;
        }

        private int Port;

        private string m_host = null;

        /// <summary>
        /// The server to connect to
        /// </summary>
        public string Host
        {
            get => m_host;
            set
            {
                if (!value.StartsWith("ftp://"))
                {
                    value = "ftp://" + value;
                }

                if (value.EndsWith("/"))
                {
                    value = value.Replace("/", "");
                }

                m_host = value;
            }
        }

        public NtsFTPClient(string host, int port, string user, string pass)
        {
            Host = host;
            Port = port;
            Credentials = new NetworkCredential(user, pass);
        }

        public NtsFTPResult UploadFile(string localPath, string serverPath)
        {
            NtsFTPResult result = new NtsFTPResult();

            if (string.IsNullOrEmpty(localPath))
            {
                result.Message = "Đường dẫn file không được để trống";
                result.FtpStatus = FtpStatus.Failed;
                return result;
            }

            if (!File.Exists(localPath))
            {
                result.Message = "Đường dẫn file không tồn tại";
                result.FtpStatus = FtpStatus.Failed;
                return result;
            }

            if (string.IsNullOrEmpty(serverPath))
            {
                result.Message = "Đường dẫn file server không được để trống";
                result.FtpStatus = FtpStatus.Failed;
                return result;
            }

            try
            {
                using (FileStream fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
                {
                    var request = CreateFtpWebRequest(serverPath, WebRequestMethods.Ftp.UploadFile);

                    //Khai báo stream với file trên server
                    Stream requestStream = request.GetRequestStream();
                    CopyDataToDestination(fileStream, requestStream);
                    WebResponse response = request.GetResponse();
                    response.Close();
                }

                result.FtpStatus = FtpStatus.Success;
            }
            catch (WebException wex)
            {
                NtsLog.LogError(wex);
                NtsLog.LogContent(((FtpWebResponse)wex.Response).StatusDescription);
                result.Message = "Upload file lỗi";
                result.FtpStatus = FtpStatus.Failed;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                result.Message = "Upload file lỗi";
                result.FtpStatus = FtpStatus.Failed;
            }

            return result;
        }

        public NtsFTPResult DownloadFile(string localPath, string serverPath)
        {
            NtsFTPResult result = new NtsFTPResult();

            if (string.IsNullOrEmpty(localPath))
            {
                result.Message = "Đường dẫn file không được để trống";
                result.FtpStatus = FtpStatus.Failed;
                return result;
            }

            if (string.IsNullOrEmpty(serverPath))
            {
                result.Message = "Đường dẫn file server không được để trống";
                result.FtpStatus = FtpStatus.Failed;
                return result;
            }

            try
            {
                using (FileStream fileStream = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                {
                    var request = CreateFtpWebRequest(serverPath, WebRequestMethods.Ftp.DownloadFile);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();

                    CopyDataToDestinationDownload(responseStream, fileStream);
                    response.Close();
                }

                result.FtpStatus = FtpStatus.Success;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                if (ex.Message.Contains("file not found"))
                {
                    result.Message = "File không tồn tại trên server";
                }
                else
                {
                    result.Message = "Download file lỗi";
                }
                result.FtpStatus = FtpStatus.Failed;
            }

            return result;
        }

        public bool CreateDirectory(string path)
        {
            try
            {
                FtpWebRequest request = CreateFtpWebRequest(path, WebRequestMethods.Ftp.MakeDirectory);
                WebResponse response = request.GetResponse();
                response.Close();
                return true;
            }
            catch(WebException wex)
            {
                NtsLog.LogError(wex);
                NtsLog.LogContent(((FtpWebResponse)wex.Response).StatusDescription);
                return false;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra tồn tại thư mục trên server
        /// </summary>
        /// <param name="path">Đường dẫn thư mục trên server</param>
        /// <returns>True: Tồn tại thư mục; False: Không tồn tại</returns>
        public bool DirectoryExist(string path)
        {
            try
            {
                FtpWebRequest request = CreateFtpWebRequest(path, WebRequestMethods.Ftp.ListDirectory);
                WebResponse response = request.GetResponse();
                response.Close();
                return true;
            }
            catch (WebException wex)
            {
                NtsLog.LogError(wex);
                NtsLog.LogContent(((FtpWebResponse)wex.Response).StatusDescription);
                return false;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                if (ex.Message.Contains("(421)"))
                {
                    throw ex;
                }

                return false;
            }
        }

        /// <summary>
        /// Kiểm tra tồn tại thư mục trên server
        /// </summary>
        /// <param name="path">Đường dẫn thư mục trên server</param>
        /// <returns>True: Tồn tại thư mục; False: Không tồn tại</returns>
        public bool GetDirectoryList(string path)
        {
            return true;
        }

        public bool DeleteDirectory(string path)
        {
            try
            {
                FtpWebRequest request = CreateFtpWebRequest(path, WebRequestMethods.Ftp.RemoveDirectory);
                WebResponse response = request.GetResponse();
                response.Close();
                return true;
            }
            catch (WebException wex)
            {
                NtsLog.LogError(wex);
                NtsLog.LogContent(((FtpWebResponse)wex.Response).StatusDescription);
                return false;
            }
            catch (Exception e)
            {
                NtsLog.LogError(e);
                return false;
            }
        }

        private void CopyDataToDestination(Stream sourceStream, Stream destinationStream)
        {
            byte[] buffer = new byte[2048];
            int bytesRead = sourceStream.Read(buffer, 0, 2048);
            long lengt = 0;
            while (bytesRead != 0)
            {

                destinationStream.Write(buffer, 0, bytesRead);
                bytesRead = sourceStream.Read(buffer, 0, 2048);
                lengt += bytesRead;
            }

            destinationStream.Close();
            sourceStream.Close();
        }

        private void CopyDataToDestinationDownload(Stream sourceStream, Stream destinationStream)
        {
            byte[] buffer = new byte[2048];
            int bytesRead = sourceStream.Read(buffer, 0, 2048);
            long lengt = 0;
            while (bytesRead != 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
                bytesRead = sourceStream.Read(buffer, 0, 2048);
                lengt += bytesRead;
            }

            destinationStream.Flush();
            destinationStream.Close();
            sourceStream.Close();
        }

        private FtpWebRequest CreateFtpWebRequest(string path, string method)
        {
            var ftpclientRequest = (FtpWebRequest)WebRequest.Create(new Uri(Host + ":" + Port + "//" + path));
            ftpclientRequest.Proxy = null;

            if (Credentials != null)
            {
                ftpclientRequest.Credentials = Credentials;
            }

            ftpclientRequest.EnableSsl = false;
            ftpclientRequest.Method = method;
            ftpclientRequest.KeepAlive = false;
            ftpclientRequest.UsePassive = true;
            //ftpclientRequest.UseBinary= true;
            return ftpclientRequest;
        }

        ~NtsFTPClient()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
