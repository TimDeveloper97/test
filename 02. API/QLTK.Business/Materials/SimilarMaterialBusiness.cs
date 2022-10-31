using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SimilarMaterial;
using NTS.Model.SimilarMaterialHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SimilarMaterials
{
    public class SimilarMaterialBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SimilarMaterialModel> SearchSimilarMaterial(SimilarMaterialSearchModel modelSearch)
        {
            SearchResultModel<SimilarMaterialModel> searchResult = new SearchResultModel<SimilarMaterialModel>();

            var dataQuery = (from a in db.SimilarMaterials.AsNoTracking()
                             orderby a.Name
                             select new SimilarMaterialModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<SimilarMaterialModel> SearchMaterialInSimilarMaterial(SimilarMaterialSearchModel modelSearch)
        {
            SearchResultModel<SimilarMaterialModel> searchResult = new SearchResultModel<SimilarMaterialModel>();

            var dataQuery = (from a in db.SimilarMaterials.AsNoTracking()
                             join b in db.SimilarMaterialConfigs.AsNoTracking() on a.Id equals b.SimilarMaterialId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Materials.AsNoTracking() on b.MaterialId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             orderby a.Name
                             select new SimilarMaterialModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 MaterialCode = c.Code,
                                 MaterialName = c.Name,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
            {
                dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.MaterialName))
            {
                dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
            }

            var listResult = dataQuery.ToList();
            List<SimilarMaterialModel> listRs = new List<SimilarMaterialModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name }).ToList();
            foreach (var item in lstRs)
            {
                SimilarMaterialModel rs = new SimilarMaterialModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                List<string> lstName = new List<string>();
                List<string> lstCode = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstCode.Count > 0)
                    {
                        if (!lstCode.Contains(ite.MaterialCode))
                        {
                            rs.MaterialCode += ", " + ite.MaterialCode;
                            lstCode.Add(ite.MaterialCode);
                        }

                    }
                    else
                    {
                        rs.MaterialCode += ite.MaterialCode;
                        lstCode.Add(ite.MaterialCode);
                    }

                    if (lstName.Count > 0)
                    {
                        if (!lstName.Contains(ite.MaterialName))
                        {
                            rs.MaterialName += ", " + ite.MaterialName;
                        }
                    }
                    else
                    {
                        rs.MaterialName += ite.MaterialName;
                        lstName.Add(ite.MaterialName);
                    }
                }
                listRs.Add(rs);
            }
            searchResult.TotalItem = listRs.Count();
            searchResult.ListResult = listRs;
            return searchResult;
        }

        public void AddSimilarMaterial(SimilarMaterialModel model)
        {
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    SimilarMaterial similarMaterial = new SimilarMaterial()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.SimilarMaterials.Add(similarMaterial);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Name, similarMaterial.Id, Constants.LOG_SimilarMaterial);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }
        public void UpdateSimilarMaterial(SimilarMaterialModel model)
        {
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var similarMaterial = db.SimilarMaterials.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SimilarMaterialHistoryModel>(similarMaterial);

                    similarMaterial.Name = model.Name.Trim();
                    similarMaterial.UpdateBy = model.UpdateBy;
                    similarMaterial.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SimilarMaterialHistoryModel>(similarMaterial);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_SimilarMaterial, similarMaterial.Id, similarMaterial.Name, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public void DeletesimilarMaterial(SimilarMaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var similarMaterialConfig = db.SimilarMaterialConfigs.AsNoTracking().Where(m => m.SimilarMaterialId.Equals(model.Id)).FirstOrDefault();
                if (similarMaterialConfig != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0016, TextResourceKey.SimilarMaterial);
                }

                try
                {
                    var similarMaterial = db.SimilarMaterials.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (similarMaterial == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SimilarMaterial);
                    }

                    db.SimilarMaterials.Remove(similarMaterial);

                    var NameOrCode = similarMaterial.Name;
                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SimilarMaterialHistoryModel>(similarMaterial);

                    //UserLogUtil.LogHistotyDelete(db, similarMaterial.CreateBy, Constants.LOG_SimilarMaterial, similarMaterial.Id, NameOrCode, jsonBefor);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }
        public SimilarMaterialModel GetSimilarMaterialInfo(SimilarMaterialModel model)
        {
            var resultInfo = db.SimilarMaterials.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SimilarMaterialModel
            {
                Id = p.Id,
                Name = p.Name,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SimilarMaterial);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(SimilarMaterialModel model)
        {
            if (db.SimilarMaterials.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SimilarMaterial);
            }
        }

        public void CheckExistedForUpdate(SimilarMaterialModel model)
        {
            if (db.SimilarMaterials.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SimilarMaterial);
            }
        }
    }
}
