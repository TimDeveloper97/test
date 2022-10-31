using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;
using NTS.Model.TestCriteria;
using NTS.Model.ProjectProducts;

namespace NTS.Model.ProjectProductTestCriteria
{
    public class ProjectProductTestCriteriaSearchModel
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
