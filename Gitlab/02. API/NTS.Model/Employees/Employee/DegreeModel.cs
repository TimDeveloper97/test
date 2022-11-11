using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employees
{
  public  class DegreeModel
    {
        public string Id { get; set; }
        public string QualificationId { get; set; }
        public string QualificationName { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public string National { get; set; }
        public string School { get; set; }
        public string Type { get; set; }
        public string Rank { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        
    }
}
