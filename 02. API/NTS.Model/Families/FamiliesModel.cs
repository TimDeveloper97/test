using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Families
{
    public class FamiliesModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
        public string Job { get; set; }
        public string Workplace { get; set; }
        public string PhoneNumber { get; set; }
    }
}
