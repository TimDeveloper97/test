using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.ConverUnit;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ConverUnits
{
    public class ConverUnitBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public void AddConverUnit(ConverUnitModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var material = db.Materials.Where(a => a.Id.Equals(model.MaterialId)).FirstOrDefault();
                    if (material == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Material);
                    }

                    var converUnits = db.ConverUnits.Where(a => a.MaterialId.Equals(model.MaterialId)).ToList();
                    if (converUnits.Count > 0)
                    {
                        db.ConverUnits.RemoveRange(converUnits);
                    }

                    if (model.ListConverUnit != null && model.ListConverUnit.Count > 0)
                    {
                        foreach (var item in model.ListConverUnit)
                        {
                            ConverUnit converUnit = new ConverUnit()
                            {
                                Id = Guid.NewGuid().ToString(),
                                UnitId = item.UnitId,
                                MaterialId = model.MaterialId,
                                Quantity = item.Quantity,
                                ConvertQuantity = item.ConvertQuantity,
                                LossRate = item.LossRate,
                            };
                            db.ConverUnits.Add(converUnit);
                        }
                        UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_MaterialGroupTPa, model.MaterialId, material.Code, "Chuyển đổi đơn vị");                    }
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

        public ConverUnitModel GetListConverUnit(ConverUnitModel model)
        {
            var data = (from a in db.ConverUnits.AsNoTracking()
                        join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                        join c in db.Units.AsNoTracking() on b.UnitId equals c.Id
                        where a.MaterialId.Equals(model.MaterialId)
                        orderby a.Quantity
                        select new ConverUnitModel
                        {
                            Id = a.Id,
                            UnitId = a.UnitId,
                            UnitName = c.Name,
                            MaterialId = a.MaterialId,
                            Quantity = a.Quantity,
                            ConvertQuantity = a.ConvertQuantity,
                            LossRate = a.LossRate,
                        }).ToList();
            model.ListConverUnit = data;
            return model;
        }

        public ConverUnitModel GetUnitName(ConverUnitModel model)
        {
            var name = (from a in db.Materials.AsNoTracking()
                        join b in db.Units.AsNoTracking() on a.UnitId equals b.Id
                        where a.Id.Equals(model.MaterialId)
                        select new ConverUnitModel
                        {
                            UnitName = b.Name,
                        }).FirstOrDefault();
            model.UnitName = name.UnitName;
            return model;
        }
    }
}
