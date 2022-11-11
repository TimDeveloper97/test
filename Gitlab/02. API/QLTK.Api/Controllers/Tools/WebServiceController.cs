using Newtonsoft.Json;
using NTS.Common.Resource;
using NTS.Model.DataCheckModuleUpload;
using NTS.Model.Product;
using NTS.Model.WebService;
using QLTK.Business;
using QLTK.Business.CodeRules;
using QLTK.Business.FileDefinitions;
using QLTK.Business.FolderDefinitions;
using QLTK.Business.Manufacturer;
using QLTK.Business.MaterialGroups;
using QLTK.Business.Materials;
using QLTK.Business.RawMaterials;
using QLTK.Business.Units;
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
using QLTK.Api.Attributes;
using NTS.Common;
using QLTK.Api.Controllers.Common;
using NTS.Model.ClassRoom;
using NTS.Model.Solution;

namespace QLTK.Api.Controllers.WebService
{
    [RoutePrefix("api/WebService")]
    [ApiHandleExceptionSystemResult]
    [NTSIPAuthorize(AllowFeature = "F090702;F090903;F091005")]
    public class WebServiceController : BaseController
    {
        WebServiceBussiness webServiceBussiness = new WebServiceBussiness();
        [Route("CheckFile3D")]
        [HttpPost]
        public HttpResponseMessage LoadCAD(Design3DFileModel model)
        {
            ResultApiModel resultApiModel = webServiceBussiness.CheckFile3D(model);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetAllModuleDesignDocument")]
        [HttpGet]
        public HttpResponseMessage GetAllModuleDesignDocument(string moduleId, int designType)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = webServiceBussiness.GetAllModuleDesignDocument(moduleId, designType);
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetModuleByModuleId")]
        [HttpGet]
        public HttpResponseMessage GetModuleByModuleId(string moduleId)
        {
            ResultApiModel resultApiModel = webServiceBussiness.GetModuleByModuleId(moduleId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetFTPServer")]
        [HttpGet]
        //[NTSA]
        [Authorize]
        public HttpResponseMessage GetFTPServer()
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;

            resultApiModel.Data = webServiceBussiness.GetFTPServer();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }


        [Route("GetFolderDownloadModuleDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFolderDownloadModuleDesignDocument(string moduleId, string folderId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;

            resultApiModel.Data = webServiceBussiness.GetFolderDownloadModuleDesignDocument(moduleId, folderId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetData")]
        [HttpGet]
        public HttpResponseMessage GetData(int designType)
        {
            DataCheckModuleUploadModel data = new DataCheckModuleUploadModel();
            //Get ListCodeRule
            CodeRuleBusiness codeRuleB = new CodeRuleBusiness();
            data.ListCodeRule = codeRuleB.GetCodeRules();

            //Get ListMaterial
            MaterialBusiness materialB = new MaterialBusiness();
            data.ListMaterialModel = materialB.GetListMaterial();

            //Get MaterialGroup
            MaterialGroupBusiness materialGroupB = new MaterialGroupBusiness();
            data.ListMaterialGroupModel = materialGroupB.GetListMaterialGroup();

            //Get Unit
            UnitBusiness unitB = new UnitBusiness();
            data.ListUnitModel = unitB.GetListUnit();

            //Get FileDefinition
            FileDefinitionBusiness fileDefinitionB = new FileDefinitionBusiness();
            var departmentId = this.GetDepartmentIdByRequest();
            data.ListFileDefinition = fileDefinitionB.GetFileDefinitions(designType, Constants.Definition_ObjectType_Module, departmentId);

            //Get FolderDefinition
            FolderDefinitionBusiness folderDefinitionB = new FolderDefinitionBusiness();
            data.ListFolderDefinition = folderDefinitionB.GetFolderDefinitions(designType, Constants.Definition_ObjectType_Module, departmentId);

            //Get Manufacture
            ManufactureBusiness manufactureB = new ManufactureBusiness();
            data.ListManufacturerModel = manufactureB.GetListManufacture();

            //Get RawMaterial
            RawMaterialBusiness rawMaterialB = new RawMaterialBusiness();
            data.ListRawMaterialsModel = rawMaterialB.GetListRawMaterial();

            data.Modules = webServiceBussiness.GetModules();

            data.SuccessStatus = true;

            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;
            resultApiModel.Data = data;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetDataCheckDMVT")]
        [HttpGet]
        public HttpResponseMessage GetDataCheckDMVT()
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;
            resultApiModel.Data = webServiceBussiness.GetDataCheckDMVT();

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetDataDefinitions")]
        [HttpGet]
        public HttpResponseMessage GetDataDefinitions(int designType, int objectType)
        {
            DataCheckModuleUploadModel data = new DataCheckModuleUploadModel();

            var departmentId = this.GetDepartmentIdByRequest();
            //Get FileDefinition
            FileDefinitionBusiness fileDefinitionB = new FileDefinitionBusiness();
            data.ListFileDefinition = fileDefinitionB.GetFileDefinitions(designType, objectType, departmentId);

            //Get FolderDefinition
            FolderDefinitionBusiness folderDefinitionB = new FolderDefinitionBusiness();
            data.ListFolderDefinition = folderDefinitionB.GetFolderDefinitions(designType, objectType, departmentId);

            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;
            resultApiModel.Data = data;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        /// <summary>
        /// Lấy dữ liệu sản phẩm upload
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        [Route("GetUploadProductData")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUploadProductData(string productId, int designType)
        {

            ResultApiModel resultApiModel = new ResultApiModel();
            UploadProductDataModel data = new UploadProductDataModel();

            data.Product = webServiceBussiness.GetProductById(productId);
            if (data.Product == null)
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.Product);
                return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
            }

            var departmentId = this.GetDepartmentIdByRequest();
            //Get FileDefinition
            FileDefinitionBusiness fileDefinitionB = new FileDefinitionBusiness();
            data.FileDefinitions = fileDefinitionB.GetFileDefinitions(designType, Constants.Definition_ObjectType_Product, departmentId);

            //Get FolderDefinition
            FolderDefinitionBusiness folderDefinitionB = new FolderDefinitionBusiness();
            data.FolderDefinitions = folderDefinitionB.GetFolderDefinitions(designType, Constants.Definition_ObjectType_Product, departmentId);

            data.Modules = webServiceBussiness.GetModules();

            resultApiModel.SuccessStatus = true;
            resultApiModel.Data = data;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetAllProductDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllProductDesignDocument(string productId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = webServiceBussiness.GetAllProductDesignDocument(productId);
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetFolderDownloadProductDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFolderDownloadProductDesignDocument(string productId, string folderId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;

            resultApiModel.Data = webServiceBussiness.GetFolderDownloadProductDesignDocument(productId, folderId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        /// <summary>
        /// Lấy dữ liệu sản phẩm upload
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        [Route("GetUploadClassRoomData")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUploadClassRoomData(string classRoomId, int designType)
        {

            ResultApiModel resultApiModel = new ResultApiModel();
            UploadClassRoomDataModel data = new UploadClassRoomDataModel();

            data.ClassRoom = webServiceBussiness.GetClassRoomById(classRoomId);
            if (data.ClassRoom == null)
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.ClassRoom);
                return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
            }

            var departmentId = this.GetDepartmentIdByRequest();
            //Get FileDefinition
            FileDefinitionBusiness fileDefinitionB = new FileDefinitionBusiness();
            data.FileDefinitions = fileDefinitionB.GetFileDefinitions(designType, Constants.Definition_ObjectType_ClassRoom, departmentId);

            //Get FolderDefinition
            FolderDefinitionBusiness folderDefinitionB = new FolderDefinitionBusiness();
            data.FolderDefinitions = folderDefinitionB.GetFolderDefinitions(designType, Constants.Definition_ObjectType_ClassRoom, departmentId);

            data.Modules = webServiceBussiness.GetModules();

            resultApiModel.SuccessStatus = true;
            resultApiModel.Data = data;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetAllClassRoomDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllClassRoomDesignDocument(string classRoomId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = webServiceBussiness.GetAllClassRoomDesignDocument(classRoomId);
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetFolderDownloadClassRoomDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFolderDownloadClassRoomDesignDocument(string classRoomId, string folderId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;

            resultApiModel.Data = webServiceBussiness.GetFolderDownloadClassRoomDesignDocument(classRoomId, folderId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        /// <summary>
        /// Lấy dữ liệu sản phẩm upload
        /// </summary>
        /// <param name="solutionId"></param>
        /// <param name="designType"></param>
        /// <returns></returns>
        [Route("GetUploadSolutionData")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetUploadSolutionData(string solutionId, int designType)
        {

            ResultApiModel resultApiModel = new ResultApiModel();
            UploadSolutionDataModel data = new UploadSolutionDataModel();

            data.Solution = webServiceBussiness.GetSolutionById(solutionId);
            if (data.Solution == null)
            {
                resultApiModel.SuccessStatus = false;
                resultApiModel.Message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.Solution);
                return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
            }

            var departmentId = this.GetDepartmentIdByRequest();
            //Get FileDefinition
            FileDefinitionBusiness fileDefinitionB = new FileDefinitionBusiness();
            data.FileDefinitions = fileDefinitionB.GetFileDefinitions(designType, Constants.Definition_ObjectType_Solution, departmentId);

            //Get FolderDefinition
            FolderDefinitionBusiness folderDefinitionB = new FolderDefinitionBusiness();
            data.FolderDefinitions = folderDefinitionB.GetFolderDefinitions(designType, Constants.Definition_ObjectType_Solution, departmentId);

            data.Modules = webServiceBussiness.GetModules();

            resultApiModel.SuccessStatus = true;
            resultApiModel.Data = data;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetAllSolutionDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetAllSolutionDesignDocument(string solutionId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.Data = webServiceBussiness.GetAllSolutionDesignDocument(solutionId);
            resultApiModel.SuccessStatus = true;

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        [Route("GetFolderDownloadSolutionDesignDocument")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetFolderDownloadSolutionDesignDocument(string solutionId, string folderId)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;

            resultApiModel.Data = webServiceBussiness.GetFolderDownloadSolutionDesignDocument(solutionId, folderId);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }

        /// <summary>
        /// Lấy thông tin version mới
        /// </summary>
        /// <param name="solutionId"></param>
        /// <param name="folderId"></param>
        /// <returns></returns>
        [Route("GetNewVersion")]
        [HttpGet]
        public HttpResponseMessage GetNewVersion(string version)
        {
            ResultApiModel resultApiModel = new ResultApiModel();
            resultApiModel.SuccessStatus = true;

            resultApiModel.Data = webServiceBussiness.GetNewVersion(version);

            return Request.CreateResponse(HttpStatusCode.OK, resultApiModel);
        }
    }
}
