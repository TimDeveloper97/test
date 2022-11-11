using Newtonsoft.Json;
using NTS.Model.GeneralTemplate;
using NTS.Model.WebService;
using QLTK.Business;
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

namespace QLTK.Api.Controllers.WebService
{
    [RoutePrefix("api/download")]
    //[NTSIPAuthorize(AllowFeature = "F090702;F090903;F091005")]
    public class DownloadController : ApiController
    {
        string keyAuthorize = System.Configuration.ConfigurationManager.AppSettings["keyAuthorize"];


        [Route("download-files")]
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
                    throw new Exception("Chưa có file");

                var path = pathFile;

                var memory = new MemoryStream();
                using (var stream = new FileStream(HostingEnvironment.MapPath("~/" + path), FileMode.Open))
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
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("load-cad-file")]
        [HttpPost]
        public HttpResponseMessage LoadCAD(List<CADModel> listCAD)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.LoadCAD(listCAD);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("checkFileTPAUse")]
        [HttpGet]
        public HttpResponseMessage CheckFileTPAUse(string path, string moduleCode, long size)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness();
            ResultApiModel resultApiModel = webServiceBussiness.CheckFileTPAUse(path, moduleCode, size);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("CheckFileSize")]
        [HttpGet]
        public HttpResponseMessage CheckFileSize(string path, string moduleCode, long size)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness();
            ResultApiModel resultApiModel = webServiceBussiness.CheckFileSize(path, moduleCode, size);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("checkFileCADShare")]
        [HttpGet]
        public HttpResponseMessage CheckFileCADShare(string path, string moduleCode, long size)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.CheckFileCADShare(path, moduleCode, size);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getDesignStruture")]
        [HttpGet]
        public HttpResponseMessage GetDesignStruture(string moduleCode, int type)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetDesignStructure(moduleCode, type);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getDesign3d")]
        [HttpGet]
        public HttpResponseMessage GetDesign3d()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetDesign3D();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getMaterial")]
        [HttpGet]
        public HttpResponseMessage GetMaterial()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data  = webServiceBussiness.GetListMaterial();
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getModuleDesignDocument")]
        [HttpGet]
        public HttpResponseMessage GetModuleDesignDocument()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetModuleDesignDocument();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetModuleDesignDocumentByType")]
        [HttpGet]
        public HttpResponseMessage getModuleDesignDocumentByType(string designType)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetModuleDesignDocument(designType);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

   

        [Route("getRawMaterial")]
        [HttpGet]
        public HttpResponseMessage GetRawMaterial()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetRawMaterial();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getErrorModuleNotDone")]
        [HttpGet]
        public HttpResponseMessage GetErrorModuleNotDone()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetErrorModuleNotDone();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getConvertUnit")]
        [HttpGet]
        public HttpResponseMessage GetConvertUnit()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetConvertUnit();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getDesignStructure")]
        [HttpGet]
        public HttpResponseMessage GetDesignStructure()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetDesignStructure();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getDesignStructureFile")]
        [HttpGet]
        public HttpResponseMessage GetDesignStructureFile()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetDesignStructureFile();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getDesignStructureFile")]
        [HttpGet]
        public HttpResponseMessage GetDesignStructureFile(string DesignStrctureId)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetDesignStructureFile(DesignStrctureId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getListProductModule")]
        [HttpGet]
        public HttpResponseMessage GetListProductModule(string ModuleId)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.getListProductModule(ModuleId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        /// <summary>
        /// Kiểm tra thư mục MAT
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <param name="fileLocal"></param>
        /// <returns></returns>
        [Route("getFileInFolderMAT")]
        [HttpPost]
        public HttpResponseMessage GetFileInFolderMAT(MATModel model)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetFileInFolderMAT(model);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        /// <summary>
        /// Kiểm tra thư mục JGS
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <param name="fileLocal"></param>
        /// <returns></returns>
        [Route("GetFileInFolderIGS")]
        [HttpPost]
        public HttpResponseMessage GetFileInFolderIGS(IGSModel model)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetFileInFolderIGS(model);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getModuleInfo")]
        [HttpGet]
        public HttpResponseMessage GetModuleInfo(string moduleCode)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetModuleInfo(moduleCode);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getMosuleStand")]
        [HttpGet]
        public HttpResponseMessage GetMosuleStand(string moduleCode)
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetMosuleStand(moduleCode);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getListManufacture")]
        [HttpGet]
        public HttpResponseMessage GetListManufacture()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.GetListManufacture();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getListUnit")]
        [HttpGet]
        public HttpResponseMessage getListUnit()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.SearchListUnit();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("getListModule")]
        [HttpGet]
        public HttpResponseMessage getListModule()
        {
            WebServiceBussiness webServiceBussiness = new WebServiceBussiness(); ;
            ResultApiModel resultApiModel = webServiceBussiness.SearchListModule();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }
    }
}
