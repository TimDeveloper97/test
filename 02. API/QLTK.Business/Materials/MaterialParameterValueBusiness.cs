using NTS.Model.MaterialParameterValue;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Materials
{
    public class MaterialParameterValueBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public List<MaterialParameterValueModel> GetValueByParameterId(string Id)
        {
            List<MaterialParameterValueModel> rs = new List<MaterialParameterValueModel>();
            try
            {
                var data = db.MaterialParameterValues.AsNoTracking().Where(t => t.MaterialParameterId.Equals(Id)).ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        rs.Add(new MaterialParameterValueModel
                        {
                            Id = item.Id,
                            MaterialParameterId = item.MaterialParameterId,
                            Value = item.Value,
                            IsChecked = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rs;
        }

        public bool AddMaterialParameterValue(MaterialParameterValueModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                bool rs = false;
                try
                {
                    var data = db.MaterialParameterValues.AsNoTracking().FirstOrDefault(t => t.Value.Equals(model.Value) && t.MaterialParameterId.Equals(model.MaterialParameterId));
                    if (data != null)
                    {
                        throw new Exception("Giá trị đã tồn tại trong thông số, hãy kiểm tra lại");
                    }

                    db.MaterialParameterValues.Add(new MaterialParameterValue
                    {
                        Id = Guid.NewGuid().ToString(),
                        Value = model.Value,
                        MaterialParameterId = model.MaterialParameterId,
                        Index = db.MaterialParameterValues.Where(t => t.MaterialParameterId.Equals(model.MaterialParameterId)).ToList().Count + 1,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    });
                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
                return rs;
            }
        }

        public bool CheckRelationship(string Id)
        {
            try
            {
                var data = db.Specifications.AsNoTracking().Where(t => t.MaterialParameterValueId.Equals(Id)).ToList();
                if (data.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
    }

}
