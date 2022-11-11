using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.FunctionGroup;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.FunctionGroups
{
    public class FunctionGroupsBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<FunctionGroupModel> SearchFunctionGroups(FunctionGroupSearchModel modelSearch)
        {
            SearchResultModel<FunctionGroupModel> searchResult = new SearchResultModel<FunctionGroupModel>();

            var dataQuery = (from a in db.FunctionGroups.AsNoTracking()
                             orderby a.Code
                             select new FunctionGroupModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).ToList();

            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddFunctionGroups(FunctionGroupModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    FunctionGroup newFunctionGroups = new FunctionGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        Note = model.Note,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.FunctionGroups.Add(newFunctionGroups);
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
        public void UpdateFunctionGroups(FunctionGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newFunctionGroup = db.FunctionGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newFunctionGroup.Name = model.Name;
                    newFunctionGroup.Code = model.Code;
                    newFunctionGroup.Note = model.Note;
                    newFunctionGroup.UpdateBy = model.UpdateBy;
                    newFunctionGroup.UpdateDate = DateTime.Now;

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

        public void DeleteFunctionGroup(FunctionGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var functionGroup = db.Functions.AsNoTracking().Where(m => m.FunctionGroupId.Equals(model.Id)).FirstOrDefault();
                if (functionGroup != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.FunctionGroups);
                }

                try
                {
                    var functiongroup = db.FunctionGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (functiongroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FunctionGroups);
                    }

                    db.FunctionGroups.Remove(functiongroup);
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
        public object GetFunctionGroupInfo(FunctionGroupModel model)
        {
            var resultInfo = db.FunctionGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new FunctionGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FunctionGroups);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(FunctionGroupModel model)
        {
            if (db.FunctionGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.FunctionGroups);
            }

            if (db.FunctionGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.FunctionGroups);
            }
        }

        public void CheckExistedForUpdate(FunctionGroupModel model)
        {
            if (db.FunctionGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.FunctionGroups);
            }

            if (db.FunctionGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.FunctionGroups);
            }
        }
    }
}
