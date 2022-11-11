using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Classification;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ClassIfication
{
    public class ClassIficationService
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ClassIficationResultModel> SearchClassIfication(ClassIficationSearchModel modelSearch)
        {
            SearchResultModel<ClassIficationResultModel> searchResult = new SearchResultModel<ClassIficationResultModel>();

            var dataQuery = (from a in db.Classifications.AsNoTracking()
                             orderby a.Index
                             select new ClassIficationResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Index = a.Index,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
            //    .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            //searchResult.ListResult = listResult;
            return searchResult;
        }

        public object GetClassIficationInfo(string ificationId)
        {
            var resultInfo = db.Classifications.AsNoTracking().Where(u => u.Id.Equals(ificationId)).Select(p => new ClassIficationModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Index = p.Index,
                Note = p.Note,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Classifications);
            }
            return resultInfo;
        }

        public void CreateClassIfication(ClassIficationModel model)
        {
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                var indexs = db.Classifications.AsNoTracking().ToList();
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listInfi = db.Classifications.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listInfi.Count() > 0 && listInfi != null)
                    {
                        foreach (var item in listInfi)
                        {
                            var updateInfi = db.Classifications.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateInfi.Index++;
                        }
                    }
                }
                try
                {
                    NTS.Model.Repositories.Classification newInfi = new NTS.Model.Repositories.Classification
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Code = model.Code,
                        Note = model.Note,
                        Index = model.Index,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,

                    };
                    db.Classifications.Add(newInfi);
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

        public void UpdateClassIfication(ClassIficationModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var checkInfi = db.Classifications.Where(b => b.Index == model.Index).FirstOrDefault();
                    if (checkInfi != null)
                    {
                        var newInfi = db.Classifications.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                        int olđInext = newInfi.Index;
                        if (checkInfi.Index < newInfi.Index)
                        {
                            var listInfiChange1 = db.Classifications.Where(a => a.Index > checkInfi.Index && a.Index < newInfi.Index);
                            if (listInfiChange1.Count() > 0)
                            {
                                foreach (var item in listInfiChange1)
                                {
                                    item.Index++;
                                }

                            }
                            checkInfi.Index++;
                        }

                        if (checkInfi.Index > newInfi.Index)
                        {
                            var listUnitChange = db.Units.Where(a => a.Index > newInfi.Index && a.Index < checkInfi.Index);
                            if (listUnitChange.ToList().Count() > 0)
                            {
                                foreach (var item in listUnitChange)
                                {
                                    item.Index--;
                                }
                            }
                            checkInfi.Index = checkInfi.Index - 1;
                        }
                        newInfi.Index = model.Index;
                        newInfi.Code = model.Code;
                        newInfi.Name = model.Name;
                        newInfi.Note = model.Note;
                        newInfi.UpdateBy = model.UpdateBy;
                        newInfi.UpdateDate = DateTime.Now;

                    }
                    else
                    {
                        var newInfi = db.Classifications.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                        var listInfi = (from a in db.Classifications.AsNoTracking()
                                        orderby a.Index ascending
                                        select new ClassIficationModel
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Index = a.Index,
                                            Note = a.Note
                                        }).AsQueryable();
                        if (newInfi.Index == 1 && listInfi.Count() == 1 && !model.Index.Equals("1"))
                        {
                            throw new Exception("Không được quyền sửa thứ tự ưu tiên. Vui lòng xem lại!");
                        }
                        newInfi.Name = model.Name;
                        newInfi.Note = model.Note;
                        newInfi.Index = model.Index;
                        newInfi.UpdateBy = model.UpdateBy;
                        newInfi.UpdateDate = DateTime.Now;
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

        public void DeleteClassIfication(string ificationId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.EmployeeDegrees.AsNoTracking().Where(r => r.ClassificationId.Equals(ificationId)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Unit);
                }
                var model = db.Classifications.FirstOrDefault(a => a.Id.Equals(ificationId));
                var maxIndex = db.Classifications.AsNoTracking().Select(a => a.Index).Max();
                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listClassIfi = db.Classifications.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listClassIfi.Count() > 0 && listClassIfi != null)
                    {
                        foreach (var item in listClassIfi)
                        {
                            if (!item.Id.Equals(model.Id))
                            {
                                var updateClassIfi = db.Classifications.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                                updateClassIfi.Index--;
                            }

                        }
                    }
                }

                try
                {
                    var classification = db.Classifications.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (classification == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Unit);
                    }
                    db.Classifications.Remove(classification);
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

        private void CheckExistedForAdd(ClassIficationModel model)
        {
            if (db.Classifications.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Classifications);
            }

            if (db.Classifications.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Classifications);
            }
        }

        public void CheckExistedForUpdate(ClassIficationModel model)
        {
            if (db.Classifications.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Classifications);
            }

            if (db.Classifications.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Classifications);
            }
        }
    }
}
