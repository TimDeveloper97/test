using NTS.Model.ClassRoom;
using NTS.Model.ClassRoomDesignDocument;
using NTS.Model.ClassRoomMaterial;
using NTS.Model.Materials;
using NTS.Model.ModuleRoomType;
using NTS.Model.Skills;
using QLTK.Api.Attributes;
using QLTK.Api.Controllers.Common;
using QLTK.Business.ClassRoom;
using QLTK.Business.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QLTK.Api.Controllers.ClassRoom
{
    [RoutePrefix("api/ClassRoom")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050313")]
    public class ClassRoomController : BaseController
    {
        private readonly ClassRoomBusiness _classRoom = new ClassRoomBusiness();
        private readonly ClassRoomBusiness _material = new ClassRoomBusiness();
        private readonly ClassRoomBusiness _module = new ClassRoomBusiness();
        private readonly ClassRoomBusiness _product = new ClassRoomBusiness();
        private readonly ClassRoomBusiness _skill = new ClassRoomBusiness();
        private readonly ClassRoomBusiness _practice = new ClassRoomBusiness();
        private readonly ClassRoomBusiness _roomType = new ClassRoomBusiness();

        [Route("SearchClassRoom")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050300")]
        public HttpResponseMessage SearchClassRoom(ClassRoomSearchModel modelSearch)
        {
                var result = _classRoom.SearchClassRoom(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchMaterial")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050304")]
        public HttpResponseMessage SearchMaterial(ClassRoomMaterialSearch modelSearch)
        {
            var result = _material.SearchMaterial(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchModule")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050304")]
        public HttpResponseMessage SearchModule(ClassRoomModuleSearchModel modelSearch)
        {
            var result = _module.SearchModule(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchProduct")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050304")]
        public HttpResponseMessage SearchProduct(ClassRoomProductSearchModel modelSearch)
        {
            var result = _product.SearchProduct(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchPractice")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050304")]
        public HttpResponseMessage SearchPractice(ClassRoomPracticeSearchModel modelSearch)
        {
            var result = _practice.SearchPractice(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchSkill")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050304")]
        public HttpResponseMessage SearchSkill(SkillsSearchModel modelSearch)
        {
            var result = _skill.SearchSkill(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("SearchRoomType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050304")]
        public HttpResponseMessage SearchRoomType(RoomTypeSearchModel modelSearch)
        {
            var result = _roomType.SearchRoomType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddClassRoom")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050301")]
        public HttpResponseMessage AddClassRoom(ClassRoomModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _classRoom.AddClassRoom(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("GetClassRoom")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050302;F050304")]
        public HttpResponseMessage GetClassRoom(ClassRoomModel model)
        {
            var result = _classRoom.GetIdClassRoom(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("UpdateClassRoom")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050302")]
        public HttpResponseMessage UpdateClassRoom(ClassRoomModel model)
        {
            model.UpdateBy = GetUserIdByRequest();
            _classRoom.UpdateClassRoom(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteClassRoom")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050303")]
        public HttpResponseMessage DeleteClassRoom(ClassRoomModel model)
        {
            model.CreateBy = GetUserIdByRequest();
            _classRoom.DeleteClassRoom(model,GetUserIdByRequest());
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("ExportExcel")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050305")]
        public HttpResponseMessage ExportExcel(ClassRoomSearchModel model)
        {
            try
            {
                string path = _classRoom.ExportExcel(model);
                return Request.CreateResponse(HttpStatusCode.OK, path);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetListFolderClassRoom")]
        [HttpGet]
        public HttpResponseMessage GetListFolderClassRoom(string classRoomId)
        {
            var result = _classRoom.GetListFolderClassRoom(classRoomId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetListFileClassRoom")]
        [HttpGet]
        public HttpResponseMessage GetListFileClassRoom(string folderId)
        {
            var result = _classRoom.GetListFileClassRoom(folderId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Import file DMVT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UploadDesignDocument")]
        [HttpPost]
        public HttpResponseMessage UploadDesignDocument(UploadFolderClassRoomDesignDocumentModel model)
        {
            var userId = GetUserIdByRequest();
            _classRoom.UploadDesignDocument(model, userId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Lấy giá vật tư theo danh sách truyền lên
        /// </summary>
        /// <param name="listModule"></param>
        /// <returns></returns>
        [Route("GetPriceModule")]
        [HttpPost]
        public HttpResponseMessage GetPriceModule(List<ClassRoomModuleResultModel> listModule)
        {
            var result = _classRoom.GetPriceModule(listModule);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy giá bài thực hành theo danh sách truyền lên
        /// </summary>
        /// <param name="listPractic"></param>
        /// <returns></returns>
        [Route("GetPricePractice")]
        [HttpPost]
        public HttpResponseMessage GetPricePractice(List<ClassRoomPracticeResultModel> listPractic)
        {
            var result = _classRoom.GetPricePractice(listPractic);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Lấy giá thiết bị theo danh sách truyền lên
        /// </summary>
        /// <param name="listProduct"></param>
        /// <returns></returns>
        [Route("GetPriceProduct")]
        [HttpPost]
        public HttpResponseMessage GetPriceProduct(List<ClassRoomResultProductModel> listProduct)
        {
            var result = _classRoom.GetPriceProduct(listProduct);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [Route("UpdatePriceClassRoom")]
        [HttpPost]
        public HttpResponseMessage UpdatePriceClassRoom()
        {
            _classRoom.UpdatePriceClassRoom();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }

}

