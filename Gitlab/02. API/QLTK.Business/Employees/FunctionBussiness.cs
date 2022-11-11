using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Function;
using NTS.Model.FunctionHistory;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using SQLHelpper = NTS.Utils.SQLHelpper;

namespace QLTK.Business.Function
{
    public class Function
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<FunctionResultModel> SearchFunction(FunctionSearchModel modelSearch)
        {
            SearchResultModel<FunctionResultModel> searchResult = new SearchResultModel<FunctionResultModel>();

            var dataQuery = (from a in db.Functions.AsNoTracking()
                             join b in db.FunctionGroups.AsNoTracking() on a.FunctionGroupId equals b.Id
                             orderby a.Code
                             select new FunctionResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 TechnicalRequire = a.TechnicalRequire,
                                 FunctionGroupId = a.FunctionGroupId,
                                 FunctionGroupName = b.Name,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.FunctionGroupId))
            {
                dataQuery = dataQuery.Where(t => modelSearch.FunctionGroupId.Equals(t.FunctionGroupId));
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.FunctionGroupName))
            {
                dataQuery = dataQuery.Where(u => u.FunctionGroupName.ToUpper().Contains(modelSearch.FunctionGroupName.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
                .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            searchResult.ListResult = listResult;

            return searchResult;
        }

        public FunctionModel GetIdFunction(FunctionModel model)
        {
            var resultInfo = db.Functions.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new FunctionModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
                TechnicalRequire = p.TechnicalRequire,
                FunctionGroupId = p.FunctionGroupId,
                FunctionGroupName = p.FunctionGroupId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Function);
            }
            return resultInfo;
        }

        public void AddFunction(FunctionModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.Function newFunction = new NTS.Model.Repositories.Function()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Note = model.Note.Trim(),
                        TechnicalRequire = model.TechnicalRequire.Trim(),
                        FunctionGroupId = model.FunctionGroupId,

                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    db.Functions.Add(newFunction);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newFunction.Code, newFunction.Id, Constants.LOG_Function);

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

        public void UpdateFunction(FunctionModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newFunction = db.Functions.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<FunctionHistoryModel>(newFunction);


                    newFunction.Name = model.Name.Trim();
                    newFunction.Code = model.Code.Trim();
                    newFunction.Note = model.Note.Trim();
                    newFunction.TechnicalRequire = model.TechnicalRequire.Trim();
                    newFunction.FunctionGroupId = model.FunctionGroupId;
                    newFunction.UpdateBy = model.UpdateBy;
                    newFunction.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<FunctionHistoryModel>(newFunction);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Function, newFunction.Id, newFunction.Code, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public void DeleteFunction(FunctionModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var _function = db.Functions.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_function == null)
                    {
                        //ko tồn tại
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Function);
                    }
                    var NameOrCode = _function.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<FunctionHistoryModel>(_function);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Function, _function.Id, NameOrCode, jsonBefor);
                    db.Functions.Remove(_function);
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

        private void CheckExistedForAdd(FunctionModel model)
        {
            if (db.Functions.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Function);
            }

            if (db.Functions.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Function);
            }
        }

        public void CheckExistedForUpdate(FunctionModel model)
        {
            //if (db.Functions.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && model.Id != o.Id).Count() > 0)
            if (db.Functions.AsNoTracking().Where(o => o.Name.Equals(model.Name) && o.Id != model.Id).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Function);
            }

            //if (db.Functions.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            if (db.Functions.AsNoTracking().Where(o => o.Code.Equals(model.Code) && o.Id != model.Id).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Function);
            }


        }
    }
}
