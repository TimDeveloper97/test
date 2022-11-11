using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.DataDistribution;
using NTS.Model.DataDistributionFile;
using NTS.Model.ModuleDesignDocument;
using NTS.Model.ModuleMaterials;
using NTS.Model.ProjectProductDocument;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business
{
    public class DowloadFileModuleBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public List<DataDistributionModel> GetDataDistributions(DataDistributionModel model)
        {
            var data = (from a in db.DataDistributions.AsNoTracking()
                        where a.DepartmentId.Equals(model.DepartmentId) && a.Type == model.Type
                        select new DataDistributionModel
                        {
                            Id = a.Id,
                            DepartmentId = a.DepartmentId,
                            Name = a.Name,
                            ParentId = a.ParentId,
                            Description = a.Description,
                            Type = a.Type,
                            Path = a.Path,
                            IsExportMaterial = a.IsExportMaterial
                        }).ToList();

            var code = db.Modules.AsNoTracking().FirstOrDefault(i => model.ModuleId.Equals(i.Id)).Code;

            foreach (var item in data)
            {
                var dataQuery = (from a in db.DataDistributionFileLinks.AsNoTracking()
                                 join b in db.DataDistributionFiles.AsNoTracking() on a.DataDistributionFileId equals b.Id into ab
                                 from b in ab.DefaultIfEmpty()
                                 where a.DataDistributionId.Equals(item.Id)
                                 select new DataDistributionFileModel
                                 {
                                     Id = b.Id,
                                     Name = b.Name,
                                     FolderContain = b.FolderContain,
                                     GetType = b.GetType,
                                     FilterThongSo = b.FilterThongSo,
                                     FilterMaVatLieu = b.FilterMaVatLieu,
                                     FilterDonVi = b.FilterDonVi,
                                     Extension = b.Extension
                                 }).ToList();
                foreach (var items in dataQuery)
                {
                    items.FolderContain = items.FolderContain.Replace("code", code);
                }
                item.ListFile = dataQuery;
                item.ModuleId = model.ModuleId;
            }
            return data;
        }

        public List<ModuleDesignDocumentModel> GetListModuleDesignDocument()
        {
            var data = (from a in db.ModuleDesignDocuments.AsNoTracking()
                        select new ModuleDesignDocumentModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            ParentId = a.ParentId,
                            Name = a.Name,
                            Path = a.Path,
                            ServerPath = a.ServerPath,
                            FileSize = a.FileSize,
                            FileType = a.FileType,
                            DesignType = a.DesignType
                        }).ToList();
            return data;
        }

        public List<ModuleMaterialModel> GetListModuleMaterial(DataDistributionModel model)
        {
            var data = (from a in db.ModuleMaterials.AsNoTracking()
                        where model.ModuleId.Equals(a.ModuleId)
                        select new ModuleMaterialModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            MaterialId = a.MaterialId,
                            MaterialCode = a.MaterialCode,
                            MaterialName = a.MaterialName,
                            Specification = a.Specification,
                            RawMaterialCode = a.RawMaterialCode,
                            RawMaterial = a.RawMaterial,
                            //Price = a.Price.ToString(),
                            //Quantity = a.Quantity.tr,
                            //Amount = a.Amount,
                            //Weight = a.Weight,
                            ManufacturerId = a.ManufacturerId,
                            Note = a.Note,
                            UnitName = db.Units.FirstOrDefault(t => a.UnitName.Equals(t.Id)).Name,
                        }).ToList();
            return data;
        }

        public List<ModuleMaterialModel> GetListModuleMaterials(DataDistributionModel model)
        {
            var data = (from a in db.ModuleMaterials.AsNoTracking()
                        select new ModuleMaterialModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            MaterialId = a.MaterialId,
                            MaterialCode = a.MaterialCode,
                            MaterialName = a.MaterialName,
                            Specification = a.Specification,
                            RawMaterialCode = a.RawMaterialCode,
                            RawMaterial = a.RawMaterial,
                            //Price = a.Price.ToString(),
                            //Quantity = a.Quantity.tr,
                            //Amount = a.Amount,
                            //Weight = a.Weight,
                            ManufacturerId = a.ManufacturerId,
                            Note = a.Note,
                            UnitName = db.Units.AsNoTracking().FirstOrDefault(t => a.UnitName.Equals(t.Id)).Name,
                        }).ToList();
            return data;
        }

        public List<DataDistributionModel> GetListFileByModuleId(DataDistributionModel model)
        {
            List<DataDistributionModel> listRe = new List<DataDistributionModel>();
            if (model.ListSelect.Count > 0)
            {
                foreach (var item in model.ListSelect)
                {
                    model.ModuleId = item.Id;
                    var listFile = GetDataDistributions(model);
                    listRe.AddRange(listFile);
                }
            }
            return listRe;
        }

    }
}