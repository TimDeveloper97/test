using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SaleGroups;
using NTS.Model.Solution.SolutionsSupplier;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SolutionSupplier
{
    /// <summary>
    /// Nhóm kinh doanh
    /// </summary>
    public class SolutionSupplierBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Lấy ra danh sách nhà cung cấp khác nhà cung cấp được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id nhà cung cấp được chọn lúc đầu.</param>
        /// <returns></returns>
        public List<SolutionsSupplierModel> GetlistSupplier(SolutionSupplierSearchModel searchModel)
        {
            var listSupplier = (from a in db.Suppliers.AsNoTracking()
                               //join b in db.SolutionAnalysisSuppliers.AsNoTracking() on a.Id equals b.SupplierId
                                where !searchModel.ListSupplierId.Contains(a.Id)
                                select new SolutionsSupplierModel
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Name = a.Name,
                                    Email = a.Email,
                                    PhoneNumber = a.PhoneNumber,
                                }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                listSupplier = listSupplier.Where(a => a.Name.ToLower().Contains(searchModel.Name.ToLower()) || a.Code.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                listSupplier = listSupplier.Where(a => a.Email.ToLower().Contains(searchModel.Email.ToLower()));
            }


            return listSupplier.ToList();
        }


    }
}
