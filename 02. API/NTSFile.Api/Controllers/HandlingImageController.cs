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
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace NTSFile.Api.Controllers
{
    [RoutePrefix("api/HandlingImage")]
    [ApiHandleExceptionSystem]
    public class HandlingImageController : ApiController
    {
        string keyAuthorize = System.Configuration.ConfigurationManager.AppSettings["keyAuthorize"];

        [Route("UploadFile")]
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            var dateNow = DateTime.Now;
            ImageResult imageResult = null;
            try
            {
                string fileFolder = HttpContext.Current.Request.Form["filefolder"];
                //var modelJson = HttpContext.Current.Request.Form["Model"];
                //var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
                //if (!model.KeyAuthorize.Equals(keyAuthorize))
                //{
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                //}
                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    imageResult = UploadFileServer.UploadFile(hfc[0], fileFolder);
                }
                return Request.CreateResponse(HttpStatusCode.OK, imageResult);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }



        [Route("UploadImage")]
        [HttpPost]
        public HttpResponseMessage UploadImage()
        {
            var dateNow = DateTime.Now;
            ImageResult imageResult = null;
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
                //if (!model.KeyAuthorize.Equals(keyAuthorize))
                //{
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                //}
                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    imageResult = UploadFileServer.UploadImage(hfc[0], model.FolderName);
                }
                return Request.CreateResponse(HttpStatusCode.OK, imageResult);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        [Route("UploadListFile")]
        [HttpPost]
        public HttpResponseMessage UploadListFile()
        {
            var dateNow = DateTime.Now;
            List<ImageResult> lstResult = new List<ImageResult>();
            try
            {
                string fileFolder = HttpContext.Current.Request.Form["filefolder"];
                //if (!model.KeyAuthorize.Equals(keyAuthorize))
                //{
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                //}
                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    for (int i = 0; i < hfc.Count; i++)
                    {
                        // Upload file lên server
                        var rs = UploadFileServer.UploadFile(hfc[i], fileFolder);
                        lstResult.Add(rs);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstResult);
            }
            catch (Exception ex)
            {
                //Xóa file trên server nếu cập nhật lỗi
                foreach (var item in lstResult)
                {
                    UploadFileServer.DeleteFile(item.FileUrl);
                }
                throw new NTSLogException(null, ex);
            }
        }


        [Route("upload-list-file-convert-pdf")]
        [HttpPost]
        public HttpResponseMessage UploadListFileConvertPDF()
        {
            var dateNow = DateTime.Now;
            List<ImageResult> lstResult = new List<ImageResult>();
            try
            {
                string fileFolder = HttpContext.Current.Request.Form["filefolder"];
                //if (!model.KeyAuthorize.Equals(keyAuthorize))
                //{
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                //}
                HttpFileCollection hfc = HttpContext.Current.Request.Files;
                if (hfc.Count > 0)
                {
                    for (int i = 0; i < hfc.Count; i++)
                    {
                        // Upload file lên server
                        var rs = UploadFileServer.UploadFileConvertPDF(hfc[i], fileFolder);
                        lstResult.Add(rs);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstResult);
            }
            catch (Exception ex)
            {
                //Xóa file trên server nếu cập nhật lỗi
                foreach (var item in lstResult)
                {
                    UploadFileServer.DeleteFile(item.FileUrl);
                }
                throw new NTSLogException(null, ex);
            }
        }


        //[Route("UploadForderFile")]
        //[HttpPost]
        //public HttpResponseMessage UploadForderFile()
        //{
        //    string forderDate = "Foder" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "/";
        //    List<string> lstResult = new List<string>();
        //    string ImagePath = "";
        //    try
        //    {
        //        var modelJson = HttpContext.Current.Request.Form["Model"];
        //        var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
        //        if (!model.KeyAuthorize.Equals(keyAuthorize))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
        //        }
        //        HttpFileCollection hfc = HttpContext.Current.Request.Files;
        //        if (hfc.Count > 0)
        //        {
        //            for (int i = 0; i < hfc.Count; i++)
        //            {
        //                // Upload file lên server
        //                ImagePath = UploadFileServer.UploadFile(hfc[i], forderDate + model.FolderName, model.FileName);
        //                lstResult.Add(ImagePath);
        //            }
        //            lstResult.Add(NTSFile.API.Constants.fileUpload + forderDate + model.FolderName);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, lstResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Xóa file trên server nếu cập nhật lỗi
        //        foreach (var item in lstResult)
        //        {
        //            UploadFileServer.DeleteFile(item);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        [Route("DownloadFile")]
        [HttpPost]
        public HttpResponseMessage DownloadFile()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);

                if (!model.KeyAuthorize.Equals(keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }

                string url = model.FileName;
                return Request.CreateResponse(HttpStatusCode.OK, url);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        //[Route("DownloadForder")]
        //[HttpPost]
        //public HttpResponseMessage DownloadForder()
        //{
        //    string dateName = "File" + DateTime.Now.ToString("ddMMyyyyHHmmss");
        //    try
        //    {
        //        var modelJson = HttpContext.Current.Request.Form["Model"];
        //        var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
        //        var folderNamePath = HostingEnvironment.MapPath("~/" + model.FolderName);
        //        var folderNamePathZip = HostingEnvironment.MapPath("~/" + Constants.fileUpload + Constants.fileZip);

        //        if (!model.KeyAuthorize.Equals(keyAuthorize))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
        //        }
        //        //kiểm tra tồn tại
        //        if (!Directory.Exists(folderNamePath))
        //        {
        //            return Request.CreateResponse(HttpStatusCode.InternalServerError, "Thư mục không tồn tại!");
        //        }
        //        //tạo thư mục chứa file nén
        //        if (!Directory.Exists(folderNamePathZip))
        //        {
        //            Directory.CreateDirectory(folderNamePathZip);
        //        }
        //        using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
        //        {
        //            zip.AddDirectory(folderNamePath);
        //            zip.Save(folderNamePathZip + "/" + dateName + ".zip");
        //        }
        //        string url = Constants.fileUpload + Constants.fileZip + "/" + dateName + ".zip";
        //        return Request.CreateResponse(HttpStatusCode.OK, url);
        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //    }
        //}

        [Route("DeleteFile")]
        [HttpPost]
        public HttpResponseMessage DeleteFile()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);

                if (!model.KeyAuthorize.Equals(keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }
                string path = HostingEnvironment.MapPath("~/" + model.FileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "File không tồn tại!");
                }
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        [Route("DeleteForder")]
        [HttpPost]
        public HttpResponseMessage DeleteForder()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
                model.FolderName = HostingEnvironment.MapPath("~/" + model.FolderName);
                if (!model.KeyAuthorize.Equals(keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }
                if (Directory.Exists(model.FolderName))
                {
                    Directory.Delete(model.FolderName, true);
                    return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Thư mục không tồn tại!");
                }
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }


        [Route("GetFileByForder")]
        [HttpPost]
        public HttpResponseMessage GetFileByForder()
        {
            List<string> lstResult = new List<string>();
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
                var folderNamePath = HostingEnvironment.MapPath("~/" + model.FolderName);
                if (!model.KeyAuthorize.Equals(keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }
                if (!Directory.Exists(folderNamePath))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Thư mục không tồn tại!");
                }
                string filePath;
                string[] filePathArray;
                string[] dirs = Directory.GetFiles(folderNamePath, "*");
                for (int i = 0; i < dirs.Length; i++)
                {
                    filePath = dirs[i];
                    if (File.Exists(filePath))
                    {
                        filePathArray = filePath.Split('\\');
                        lstResult.Add(model.FolderName + "/" + filePathArray[filePathArray.Length - 1]);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstResult);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        [Route("GetForderByForder")]
        [HttpPost]
        public HttpResponseMessage GetForderByForder()
        {
            List<string> lstResult = new List<string>();
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UploadModel>(modelJson);
                var folderNamePath = HostingEnvironment.MapPath("~/" + model.FolderName);
                if (!model.KeyAuthorize.Equals(keyAuthorize))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Không có quyền sử dụng chức năng này!");
                }
                if (!Directory.Exists(folderNamePath))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Thư mục không tồn tại!");
                }
                string filePath;
                string[] filePathArray;
                string[] dirs = Directory.GetDirectories(folderNamePath);
                for (int i = 0; i < dirs.Length; i++)
                {
                    filePath = dirs[i];
                    if (Directory.Exists(filePath))
                    {
                        filePathArray = filePath.Split('\\');
                        lstResult.Add(model.FolderName + "/" + filePathArray[filePathArray.Length - 1]);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, lstResult);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }

        [Route("DownAllDocumentProcess")]
        [HttpPost]
        public HttpResponseMessage DownAllDocumentProcess(ListDataModel model)
        {
            //try
            //{
                var path = UploadFileServer.DownAllDocumentProcess(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            //}
            //catch (Exception ex)
            //{
            //    throw new NTSLogException(model, ex);
            //}
        }

    }
}
