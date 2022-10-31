using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendingMachinesSystem.Model;

namespace VendingMachinesSystem.Api.Models
{
    public class VMSDVRelayStatusModel:BaseModel
    {
        public string DeviceId { get; set; }
        public string RelayStatus { get; set; }
    }
}