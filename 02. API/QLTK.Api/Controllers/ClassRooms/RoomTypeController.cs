using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using NTS.Model.ModuleRoomType;
using QLTK.Api.Attributes;
using QLTK.Business.RoomType;


namespace QLTK.Api.Controllers.RoomType
{
    [RoutePrefix(prefix: "api/Roomtype")]
    [ApiHandleExceptionSystem]
    [NTSIPAuthorize(AllowFeature = "F050204")]
    public class RoomTypeController : ApiController
    {
        private readonly Business.RoomType.RoomType _roomType = new Business.RoomType.RoomType();

        [Route("SearchRoomType")]
        [HttpPost]
        public HttpResponseMessage SearchRoomType(RoomTypeSearchModel modelSearch)
        {
            var result = _roomType.SearchRoomType(modelSearch);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetRoomType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050202")]
        public HttpResponseMessage GetRoomType(RoomTypeModel model)
        {
            var result = _roomType.GetRoomTypeInfo(model);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("AddRoomType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050201")]
        public HttpResponseMessage AddRoomType(RoomTypeModel model)
        {
            _roomType.AddRoomType(model);
            return Request.CreateResponse(HttpStatusCode.OK, value: string.Empty);
        }

        [Route("UpdateRoomType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050202")]
        public HttpResponseMessage UpdateRoomType(RoomTypeModel model)
        {
            _roomType.UpdateRoomType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }

        [Route("DeleteRoomType")]
        [HttpPost]
        [NTSAuthorize(AllowFeature = "F050203")]
        public HttpResponseMessage DeleteRoomType(RoomTypeModel model)
        {
            _roomType.deleteRoomType(model);
            return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        }
    }
}
