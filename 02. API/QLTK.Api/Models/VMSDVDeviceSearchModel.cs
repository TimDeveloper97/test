using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendingMachinesSystem.Api.Models
{
    public class VMSDVDeviceSearchModel:Common.CommonModel
    {
        public string DeviceTypeId { get; set; }
        public string DeviceSearchName { get; set; }
        public string DeviceSearchCode { get; set; }
        public string SearchPhoneNumber { get; set; }
        public string SearchStatus { get; set; }
        public string SearchBuildingId { get; set; }
        public string SearchWarningTypeId { get; set; }
        public string SearchWarningResult { get; set; }
    }
}