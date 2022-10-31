using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Specialize;
using NTS.Model.SurveyMaterial;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;

namespace QLTK.Business.SurveyMaterial
{
    public class SurveyMaterialBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        //public SearchResultModel<SurveyMaterialCreateModel> SearchSurveyMaterial(SurveyMaterialSearchModel modelSearch)
        //{
        //    SearchResultModel<SurveyMaterialCreateModel> searchResult = new SearchResultModel<SurveyMaterialCreateModel>();

        //    var dataQuery = (from a in db.SurveyTools.AsNoTracking()
        //                     orderby a.Name
        //        select new SurveyMaterialCreateModel()
        //        {
        //            Id = a.Id,
        //            Name = a.Name,
        //            Code = a.Code,
        //            Note = a.Note,
        //            Quantity = a.Quantity,
        //        }).AsQueryable();
        //    if (!string.IsNullOrEmpty(modelSearch.Name))
        //    {
        //        dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
        //    }

        //    if (!string.IsNullOrEmpty(modelSearch.Code))
        //    {
        //        dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
        //    }
        //    searchResult.TotalItem = dataQuery.Count();

        //    var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
        //        .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
        //    searchResult.ListResult = listResult;
        //    return searchResult;
        //}

        public SurveyMaterialCreateModel GetSurveyMaterial(SurveyMaterialCreateModel model)
        {
            var resultInfo = db.SurveyTools.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new SurveyMaterialCreateModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
                Quantity= p.Quantity
              
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SurveyMaterial);
            }
            return resultInfo;
        }

        public SurveyMaterialCreateModel AddSurveyMaterial(SurveyMaterialCreateModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);

            using (var trans = db.Database.BeginTransaction())
            {
                string id;
                try
                {
                    NTS.Model.Repositories.SurveyTool newSurveyMaterial = new NTS.Model.Repositories.SurveyTool()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Note = model.Note,
                        Quantity = model.Quantity,
                        SurveyId = model.SurvayId,
                    };

                    db.SurveyTools.Add(newSurveyMaterial);
                    id = newSurveyMaterial.Id;

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newSurveyMaterial.Code, newSurveyMaterial.Id, Constants.LOG_SurveyMaterial);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
                var resultInfo = db.SurveyTools.Where(u => u.Id.Equals(id)).Select(p => new SurveyMaterialCreateModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Note = p.Note,
                    SurvayId = p.SurveyId,
                    Quantity = p.Quantity,
                }).FirstOrDefault();
                return resultInfo; 
            }
            
        }

        public void UpdateSurveyMaterial(SurveyMaterialCreateModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSurveyMaterial = db.SurveyTools.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SurveyMaterialCreateModel>(newSurveyMaterial);

                    newSurveyMaterial.Name = model.Name.Trim();
                    newSurveyMaterial.Code = model.Code.Trim();
                    newSurveyMaterial.Quantity = model.Quantity;
                    newSurveyMaterial.Note = model.Note;
                    newSurveyMaterial.SurveyId = model.SurvayId;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SurveyMaterialCreateModel>(newSurveyMaterial);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_SurveyMaterial, newSurveyMaterial.Id, newSurveyMaterial.Code, jsonBefor, jsonApter);
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

        public void DeleteSurveyMaterial(SurveyMaterialCreateModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var _SurveyMaterial = db.SurveyTools.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_SurveyMaterial == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SurveyMaterial);
                    }

                    db.SurveyTools.Remove(_SurveyMaterial);

                    var NameOrCode = _SurveyMaterial.Name;

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

        private void CheckExistedForAdd(SurveyMaterialCreateModel model)
        {
            if (db.SurveyTools.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SurveyMaterial);
            }

            if (db.SurveyTools.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SurveyMaterial);
            }
        }
        public void CheckExistedForUpdate(SurveyMaterialCreateModel model)
        {
            if (db.SurveyTools.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SurveyMaterial);
            }

            if (db.SurveyTools.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SurveyMaterial);
            }

        }
    }



}
