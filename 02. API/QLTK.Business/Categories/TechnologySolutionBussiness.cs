using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Categories;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Categories
{
    public class TechnologySolutionBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ResultSearchTechnologySolutionModel> SearchTech(SearchTechnologySolutionModel search)
        {
            SearchResultModel<ResultSearchTechnologySolutionModel> searchResult = new SearchResultModel<ResultSearchTechnologySolutionModel>();
            var dataQuerys = (from a in db.SolutionTechnologies.AsNoTracking()
                              orderby a.Index
                              select new ResultSearchTechnologySolutionModel
                              {
                                  Id = a.Id,
                                  Code = a.Code,
                                  Name = a.Name,
                                  Index = a.Index,
                                  Description = a.Description,
                                  IsEnable = a.IsEnable,
                                  Note = a.Note,
                              }).AsQueryable();
            if (!string.IsNullOrEmpty(search.Code))
            {
                dataQuerys = dataQuerys.Where(u => u.Code.ToUpper().Contains(search.Code.ToUpper()) || u.Name.ToUpper().Contains(search.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(search.SupplierId))
            {
                dataQuerys = (from a in dataQuerys
                              join b in db.TechnologySuppliers.AsNoTracking() on a.Id equals b.TechnologyId into ab
                              from b in ab.DefaultIfEmpty()
                              where b.SupplierId.Contains(search.SupplierId)
                              orderby a.Index
                              select new ResultSearchTechnologySolutionModel
                              {
                                  Id = a.Id,
                                  Code = a.Code,
                                  Name = a.Name,
                                  Index = a.Index,
                                  Description = a.Description,
                                  IsEnable = a.IsEnable,
                                  Note = a.Note,
                              }).AsQueryable();
            }

            if (!string.IsNullOrEmpty(search.ManufactureId))
            {
                dataQuerys = (from a in dataQuerys
                              join b in db.TechnologyManufactures.AsNoTracking() on a.Id equals b.TechnologyId into ab
                              from b in ab.DefaultIfEmpty()
                              where b.ManufactureId.Contains(search.ManufactureId)
                              orderby a.Index
                              select new ResultSearchTechnologySolutionModel
                              {
                                  Id = a.Id,
                                  Code = a.Code,
                                  Name = a.Name,
                                  Index = a.Index,
                                  Description = a.Description,
                                  IsEnable = a.IsEnable,
                                  Note = a.Note,
                              }).AsQueryable();
            }
            searchResult.TotalItem = dataQuerys.Count();
            var listResult = SQLHelpper.OrderBy(dataQuerys, search.OrderBy, search.OrderType).Skip((search.PageNumber - 1) * search.PageSize).Take(search.PageSize).ToList();
            if (listResult.Count > 0)
            {
                List<string> manuS;
                List<string> manuM;
                foreach (var item in listResult)
                {

                    manuS = (from s in db.TechnologySuppliers.AsNoTracking()
                             where s.TechnologyId.Equals(item.Id)
                             join m in db.Suppliers.AsNoTracking() on s.SupplierId equals m.Id
                             select m.Code).ToList();

                    item.SupplierName = string.Join(", ", manuS);

                    manuM = (from s in db.TechnologyManufactures.AsNoTracking()
                             where s.TechnologyId.Equals(item.Id)
                             join m in db.Manufactures.AsNoTracking() on s.ManufactureId equals m.Id
                             select m.Name).ToList();

                    item.ManufactureName = string.Join(", ", manuM);

                }
            }
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void DeleteTech(SearchTechnologySolutionModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var maxIndex = db.SolutionTechnologies.AsNoTracking().Select(a => a.Index).Max();

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listTech = db.SolutionTechnologies.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listTech.Count() > 0 && listTech != null)
                    {
                        foreach (var items in listTech)
                        {
                            if (!items.Id.Equals(model.Id))
                            {
                                var updateTech = db.SolutionTechnologies.Where(r => r.Id.Equals(items.Id)).FirstOrDefault();
                                updateTech.Index--;
                            }

                        }
                    }
                }

                try
                {
                    var tech1 = db.SolutionTechnologies.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (tech1 == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TechnologySolution);
                    }
                    db.SolutionTechnologies.Remove(tech1);

                    var manTech = db.TechnologyManufactures.Where(u => u.TechnologyId.Equals(model.Id)).ToList();
                    foreach (var man in manTech)
                    {
                        db.TechnologyManufactures.Remove(man);

                    }
                    var supTech = db.TechnologySuppliers.Where(u => u.TechnologyId.Equals(model.Id)).ToList();
                    foreach (var sup in supTech)
                    {
                        db.TechnologySuppliers.Remove(sup);
                    }

                    //var NameOrCode = quote1.Name;

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

        public object GetTechIndex()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.SolutionTechnologies.AsNoTracking()
                                 orderby a.Index ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Index.ToString(),
                                     Index = a.Index
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxResult addFirstIndex = new ComboboxResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Exten = "1";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.SolutionTechnologies.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxResult addIndex = new ComboboxResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Exten = (maxIndex + 1).ToString();
                    addIndex.Index = maxIndex + 1;
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }


        public void CreateTech(TechnologySolutionModel model)
        {
            if (db.SolutionTechnologies.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TechnologySolution);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                var indexs = db.SolutionTechnologies.ToList();
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listTech = db.SolutionTechnologies.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listTech.Count() > 0 && listTech != null)
                    {
                        foreach (var item in listTech)
                        {
                            var updateTech = db.SolutionTechnologies.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateTech.Index++;
                        }
                    }
                }
                try
                {
                    SolutionTechnology TechNew = new SolutionTechnology
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        Index = model.Index,
                        Description = model.Description.Trim(),
                        IsEnable = model.IsEnable,
                        Note = model.Note.Trim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.SolutionTechnologies.Add(TechNew);

                    List<TechnologySupplier> TechSuppliers = new List<TechnologySupplier>();
                    foreach (var sup in model.ListSupplierGroupId)
                    {
                        TechSuppliers.Add(new TechnologySupplier()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TechnologyId = TechNew.Id,
                            SupplierId = sup
                        });
                    }
                    db.TechnologySuppliers.AddRange(TechSuppliers);

                    List<TechnologyManufacture> TechManufactures = new List<TechnologyManufacture>();
                    foreach (var man in model.ListManufactureGroupId)
                    {
                        TechManufactures.Add(new TechnologyManufacture()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TechnologyId = TechNew.Id,
                            ManufactureId = man
                        });
                    }
                    db.TechnologyManufactures.AddRange(TechManufactures);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, TechNew.Name, TechNew.Id, Constants.LOG_Unit);

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


        public void UpdateTech(TechnologySolutionModel model)
        {
            var tech = db.SolutionTechnologies.Where(a => a.Id != model.Id).ToList();
            foreach (var q in tech)
            {
                if (q.Code == model.Code)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TechnologySolution);
                }
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var checkTech = db.SolutionTechnologies.Where(b => b.Index == model.Index).FirstOrDefault();
                    if (checkTech != null)
                    {
                        var newTech = db.SolutionTechnologies.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        int oldInext = newTech.Index;

                        if (checkTech.Index < newTech.Index)
                        {
                            var listQuoteChange1 = db.SolutionTechnologies.Where(a => a.Index > checkTech.Index && a.Index < newTech.Index);
                            if (listQuoteChange1.Count() > 0)
                            {
                                foreach (var item in listQuoteChange1)
                                {
                                    item.Index++;
                                }

                            }
                            checkTech.Index++;
                        }

                        if (checkTech.Index > newTech.Index)
                        {
                            var listQuoteChange = db.SolutionTechnologies.Where(a => a.Index > newTech.Index && a.Index < checkTech.Index);
                            if (listQuoteChange.ToList().Count() > 0)
                            {
                                foreach (var item in listQuoteChange)
                                {
                                    item.Index--;
                                }
                            }
                            checkTech.Index = checkTech.Index - 1;
                        }
                        newTech.Code = model.Code.Trim();
                        newTech.Index = model.Index;
                        newTech.IsEnable = model.IsEnable;
                        newTech.Name = model.Name.Trim();
                        newTech.Description = model.Description.Trim();
                        newTech.Note = model.Note.Trim();
                        newTech.UpdateBy = model.UpdateBy;
                        newTech.UpdateDate = DateTime.Now;

                        var supTech = db.TechnologySuppliers.Where(u => u.TechnologyId.Equals(model.Id)).ToList();
                        foreach (var sup in supTech)
                        {
                            db.TechnologySuppliers.Remove(sup);
                            db.SaveChanges();
                        }
                        List<TechnologySupplier> TechSuppliers = new List<TechnologySupplier>();
                        foreach (var sup in model.ListSupplierGroupId)
                        {
                            TechSuppliers.Add(new TechnologySupplier()
                            {
                                Id = Guid.NewGuid().ToString(),
                                TechnologyId = model.Id,
                                SupplierId = sup
                            });
                        }
                        db.TechnologySuppliers.AddRange(TechSuppliers);

                        var manTech = db.TechnologyManufactures.Where(u => u.TechnologyId.Equals(model.Id)).ToList();
                        foreach (var man in manTech)
                        {
                            db.TechnologyManufactures.Remove(man);
                            db.SaveChanges();
                        }
                        List<TechnologyManufacture> TechManufactures = new List<TechnologyManufacture>();
                        foreach (var man in model.ListManufactureGroupId)
                        {
                            TechManufactures.Add(new TechnologyManufacture()
                            {
                                Id = Guid.NewGuid().ToString(),
                                TechnologyId = model.Id,
                                ManufactureId = man
                            });
                        }
                        db.TechnologyManufactures.AddRange(TechManufactures);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Id))
                        {
                            var techData = db.SolutionTechnologies.FirstOrDefault(x => x.Id.Equals(model.Id));
                            if (tech != null)
                            {
                                techData.Code = model.Code.Trim();
                                techData.Name = model.Name.Trim();
                                techData.Description = model.Description.Trim();
                                techData.IsEnable = model.IsEnable;
                                techData.Note = model.Note.Trim();
                                techData.UpdateBy = model.UpdateBy;
                                techData.UpdateDate = DateTime.Now;



                            }
                        }
                    }

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


        public void CreateTechIndex(SearchTechnologySolutionModel model)
        {
            for (var i = 0; i < model.ListTech.Count; i++)
            {
                string id = model.ListTech[i].Id;
                var Tech = db.SolutionTechnologies.Where(r => r.Id.Equals(id)).FirstOrDefault();
                if (Tech != null)
                {
                    Tech.Index = i + 1;
                }
            }

            db.SaveChanges();

        }

        public object GetTechInfo(TechnologySolutionModel model)
        {
            var resultInfo = db.SolutionTechnologies.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new TechnologySolutionModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                Note = p.Note,
                IsEnable = p.IsEnable,
                Index = p.Index,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate,
            }).FirstOrDefault();
            resultInfo.ListSupplierGroupId = db.TechnologySuppliers.AsNoTracking().Where(a => a.TechnologyId.Equals(model.Id)).Select(a => a.SupplierId).ToList();
            resultInfo.ListManufactureGroupId = db.TechnologyManufactures.AsNoTracking().Where(a => a.TechnologyId.Equals(model.Id)).Select(a => a.ManufactureId).ToList();

            return resultInfo;
        }

    }
}
