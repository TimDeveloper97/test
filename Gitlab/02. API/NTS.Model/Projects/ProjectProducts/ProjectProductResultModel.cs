using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectProducts
{
    public class ProjectProductResultModel
    {
        public int Total { get; set; }
        public int TotalItems { get; set; }
        public decimal ToTalAmount { get; set; }
        public decimal ToTalAmountTHTK { get; set; }
        public decimal TotalAmountIncurred { get; set; }
        public bool ColorTHTK { get; set; }
        public List<ProjectProductsModel> ListResult { get; set; }
        public List<ProjectProductsModel> ListPriceVTP { get; set; }
    }
}
