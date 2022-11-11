using System;
using System.Linq;

namespace NTS.Model.NTSDepartment
{
    public class DepartmentResultModel : BaseModel
    {
        public string Id { get; set; }
        public string SBUId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
        public string PhoneNumber { get; set; }

        public string ManagerId { get; set; }

        public string ManagerName { get; set; }
        public string JobPositionId { get; set; }

        public string Description { get; set; }
        public string SBUName { get; set; }
        public bool IsDesign { get; set; }
    }
}