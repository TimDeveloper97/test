using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.ManufactureGroup;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ManufactureGroups
{
    public class ManufactureGroupBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public List<ManufactureGroupModel> SearchManufactureGroup(ManufactureGroupModel model)
        {
            List<ManufactureGroupModel> list = new List<ManufactureGroupModel>();
            var dataQuery = (from a in db.ManufactureGroups.AsNoTracking()
                             orderby a.Name
                             select new ManufactureGroupModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).ToList();
            list = dataQuery;
            return list;
        }

        public void AddManufactureGroup(ManufactureGroupModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ManufactureGroup manufactureGroup = new ManufactureGroup
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

                    db.ManufactureGroups.Add(manufactureGroup);
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
        public void UpdateManufactureGroup(ManufactureGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var manufactureGroup = db.ManufactureGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    manufactureGroup.Name = model.Name;
                    manufactureGroup.Code = model.Code;
                    manufactureGroup.Note = model.Note;
                    manufactureGroup.UpdateBy = model.UpdateBy;
                    manufactureGroup.UpdateDate = DateTime.Now;

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

        public void DeleteManufactureGroup(ManufactureGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var manufactureInGroup = db.ManufactureInGroups.AsNoTracking().Where(m => m.ManufactureGroupId.Equals(model.Id)).FirstOrDefault();
                if (manufactureInGroup != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ManufactureGroup);
                }

                try
                {
                    var manufactureGroup = db.ManufactureGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (manufactureGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ManufactureGroup);
                    }

                    db.ManufactureGroups.Remove(manufactureGroup);
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
        public object GetManufactureGroupInfo(ManufactureGroupModel model)
        {
            var resultInfo = db.ManufactureGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ManufactureGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ManufactureGroup);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(ManufactureGroupModel model)
        {
            if (db.ManufactureGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ManufactureGroup);
            }

            if (db.ManufactureGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ManufactureGroup);
            }
        }

        public void CheckExistedForUpdate(ManufactureGroupModel model)
        {
            if (db.ManufactureGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ManufactureGroup);
            }

            if (db.ManufactureGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ManufactureGroup);
            }
        }
    }
}
