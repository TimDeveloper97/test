using NTS.Model.ProjectProducts;
using NTS.Model.TestCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProductTestCriteria
{
    public class ProjectProductTestCriteriaResuldModel
    {
        public string Id { get; set; }
        public string TestCriteriaId { get; set; }
        public string ProjectProductId { get; set; }
        public bool ResultStatus { get; set; }
        public string Note { get; set; }

        public virtual ProjectProductsModel ProjectProduct { get; set; }
        public virtual TestCriteriaModel TestCriteria { get; set; }
    }
}
