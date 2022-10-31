using NTS.Model.ProductAccessories;
using NTS.Model.ProjectProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralTemplate
{
    public class ExportGeneralDesignModel: ProjectProductsResuldModel
    {       
        public List<ProjectProductsResuldModel> ListModule { get; set; }
        public List<ProductAccessoriesModel> ListMaterial { get; set; }
        public ExportGeneralDesignModel()
        {
            ListModule = new List<ProjectProductsResuldModel>();
            ListMaterial = new List<ProductAccessoriesModel>();
        }
    }
}
