using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Repositories;
using NTS.Model.SupplierGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SupplierGroups
{
    public class SupplierGroupBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public List<SupplierGroupModel> SearchSupplierGroup(SupplierGroupModel model)
        {
            List<SupplierGroupModel> list = new List<SupplierGroupModel>();
            var dataQuery = (from a in db.SupplierGroups.AsNoTracking()
                             orderby a.Name
                             select new SupplierGroupModel
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

        public void AddSupplierGroup(SupplierGroupModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    SupplierGroup supplierGroup = new SupplierGroup
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

                    db.SupplierGroups.Add(supplierGroup);
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
        public void UpdateSupplierGroup(SupplierGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var supplierGroup = db.SupplierGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    supplierGroup.Name = model.Name;
                    supplierGroup.Code = model.Code;
                    supplierGroup.Note = model.Note;
                    supplierGroup.UpdateBy = model.UpdateBy;
                    supplierGroup.UpdateDate = DateTime.Now;

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

        public void DeletesupplierGroup(SupplierGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var supplierInGroup = db.SupplierInGroups.AsNoTracking().Where(m => m.SupplierGroupId.Equals(model.Id)).FirstOrDefault();
                if (supplierInGroup != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SupplierGroup);
                }

                try
                {
                    var supplierGroup = db.SupplierGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (supplierGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SupplierGroup);
                    }

                    db.SupplierGroups.Remove(supplierGroup);
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
        public object GetSupplierGroupInfo(SupplierGroupModel model)
        {
            var resultInfo = db.SupplierGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SupplierGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SupplierGroup);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(SupplierGroupModel model)
        {
            if (db.SupplierGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SupplierGroup);
            }

            if (db.SupplierGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SupplierGroup);
            }
        }

        public void CheckExistedForUpdate(SupplierGroupModel model)
        {
            if (db.SupplierGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SupplierGroup);
            }

            if (db.SupplierGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SupplierGroup);
            }
        }
    }
}
