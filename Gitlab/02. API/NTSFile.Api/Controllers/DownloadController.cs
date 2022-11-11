using Newtonsoft.Json;
using NTS.Common;
using NTSFile.Api.Attributes;
using NTSFile.Api.Utilities;
using NTSFile.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;


namespace NTSFile.Api.Controllers
{
    [RoutePrefix("api/download")]
    [ApiHandleExceptionSystem]
    public class DownloadController : ApiController
    {
        string keyAuthorize = System.Configuration.ConfigurationManager.AppSettings["keyAuthorize"];


        [Route("download-file")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetFile(string pathFile, string fileName, string keyAuthorize)
        {
            try
            {
                if (!keyAuthorize.Equals(this.keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }

                if (string.IsNullOrEmpty(pathFile))
                    throw NTSException.CreateInstance("Chưa có file");

                var path = HostingEnvironment.MapPath("~/" + pathFile);

                if (!File.Exists(path))
                {
                    throw NTSException.CreateInstance("File không tồn tại!");
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;

                HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new StreamContent(memory);
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return httpResponseMessage;
            }
            catch (NTSException ntsex)
            {
                throw ntsex;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        [Route("download-file-new")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetFileNew(string pathFile, string keyAuthorize)
        {
            try
            {
                if (!keyAuthorize.Equals(this.keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }

                if (string.IsNullOrEmpty(pathFile))
                    throw new Exception("Chưa có file");

                var path = HostingEnvironment.MapPath("~/" + pathFile);

                if (!File.Exists(path))
                {
                    throw NTSException.CreateInstance("File không tồn tại!");
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;

                HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new StreamContent(memory);
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                //httpResponseMessage.Content.Headers.ContentDisposition.FileName = fileName;
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                return httpResponseMessage;
            }
            catch (NTSException ntsex)
            {
                throw ntsex;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }
    }
}
