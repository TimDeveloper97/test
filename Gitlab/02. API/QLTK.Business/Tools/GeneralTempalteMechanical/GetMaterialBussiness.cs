using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Materials;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialModel = NTS.Model.GeneralTemplate.MaterialModel;
using MaterialSearchModel = NTS.Model.GeneralTemplate.MaterialSearchModel;

namespace QLTK.Business.GeneralTempalteMechanical
{
    public class GetMaterialBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public List<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            List<MaterialModel> materials = new List<MaterialModel>();
            try
            {
                var dataQuery = (from a in db.Materials.AsNoTracking()
                                 join b in db.Units.AsNoTracking() on a.UnitId equals b.Id into ab
                                 from ba in ab.DefaultIfEmpty()
                                 join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id into ac
                                 from ca in ac.DefaultIfEmpty()
                                 join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                                 from da in ad.DefaultIfEmpty()
                                 //join e in db.RawMaterials.AsNoTracking() on a.RawMaterialId equals e.Id into ae
                                 //from ea in ae.DefaultIfEmpty()
                                 orderby a.Name
                                 select new MaterialModel
                                 {
                                     Id = a.Id,
                                     MaterialName = a.Name,
                                     RawMaterialName =a.RawMaterial,
                                     MaterialCode = a.Code,
                                     UnitName = ba == null ? "" : ba.Name,
                                     Quantity = 1,
                                     DeliveryDays = a.DeliveryDays,
                                     ManufacturerName = da == null ? "" : da.Name,
                                     Pricing = a.Pricing,

                                     Note = a.Note,

                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                {
                    dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
                }

                if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                {
                    dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
                }

                materials = dataQuery.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return materials;
        }
    }
}
