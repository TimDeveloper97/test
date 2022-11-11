using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.ErrorGroup;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ErrorGroups
{
    public class ErrorGroupBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public object SearchErrorGroup(ErrorGroupModel modelSearch)
        {
            var dataQuery = (from a in db.ErrorGroups.AsNoTracking()
                             orderby a.Code
                             select new ErrorGroupModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 ParentId = a.ParentId,
                                 Description = a.Description,
                                 Type = a.Type,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();
            var searchResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddErrorGroup(ErrorGroupModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ErrorGroup errorGroup = new ErrorGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        ParentId = model.ParentId,
                        Description = model.Description,
                        Type = model.Type,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    db.ErrorGroups.Add(errorGroup);
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
        public void UpdateErrorGroup(ErrorGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var errorGroup = db.ErrorGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    errorGroup.Name = model.Name;
                    errorGroup.Code = model.Code;
                    errorGroup.ParentId = model.ParentId;
                    errorGroup.Description = model.Description;
                    errorGroup.Type = model.Type;
                    errorGroup.UpdateBy = model.UpdateBy;
                    errorGroup.UpdateDate = DateTime.Now;

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

        public void DeleteErrorGroup(ErrorGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var error = db.Errors.AsNoTracking().Where(m => m.ErrorGroupId.Equals(model.Id)).FirstOrDefault();
                if (error != null)
                {
                    if (model.Check != 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProblemExistGroup);
                    }
                    else
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ErrorGroup);
                    }
                }            

                try
                {
                    var errorGroup = db.ErrorGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (errorGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProblemExistGroup);
                    }

                    db.ErrorGroups.Remove(errorGroup);
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
        public ErrorGroupModel GetErrorGroupInfo(ErrorGroupModel model)
        {
            var resultInfo = db.ErrorGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ErrorGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
                Description = p.Description,
                Type = p.Type,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ErrorGroup);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(ErrorGroupModel model)
        {
            //if (db.ErrorGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ErrorGroup);
            //}

            if (db.ErrorGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ErrorGroup);
            }
        }

        public void CheckExistedForUpdate(ErrorGroupModel model)
        {
            if (db.ErrorGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ErrorGroup);
            }

            if (db.ErrorGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ErrorGroup);
            }
        }
    }
}
