using NTS.Model.MaterialParameter;
using NTS.Model.MaterialParameterValue;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Materials
{
    public class MaterialParameterBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public List<MaterialParameterModel> GetParameterByGroupId(string Id)
        {
            List<MaterialParameterModel> rs = new List<MaterialParameterModel>();
            try
            {
                var dataMP = db.MaterialParameters.AsNoTracking().ToList().OrderBy(i=>i.Name);
                var dataMPV = db.MaterialParameterValues.AsNoTracking().ToList().OrderBy(i=>i.Value);

                var listMP = dataMP.Where(t => t.MaterialGroupId.Equals(Id)).ToList();
                if (listMP.Count > 0)
                {
                    foreach (var ite in listMP)
                    {
                        MaterialParameterModel paramModel = new MaterialParameterModel();
                        paramModel.Id = ite.Id;
                        paramModel.MaterialGroupId = ite.MaterialGroupId;
                        paramModel.Name = ite.Name;
                        paramModel.IsDelete = false;
                        var listMPV = dataMPV.Where(t => t.MaterialParameterId.Equals(ite.Id)).ToList();
                        if (listMPV.Count > 0)
                        {
                            foreach (var it in listMPV)
                            {
                                MaterialParameterValueModel valueModel = new MaterialParameterValueModel();
                                valueModel.Id = it.Id;
                                valueModel.MaterialParameterId = it.MaterialParameterId;
                                valueModel.Value = it.Value;
                                valueModel.IsDelete = false;
                                paramModel.ListValue.Add(valueModel);
                                valueModel.IsChecked = false;
                            }
                        }
                        rs.Add(paramModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rs;
        }

        public bool AddMaterialParameter(MaterialParameterModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                bool rs = false;
                try
                {
                    var data = db.MaterialParameters.AsNoTracking().FirstOrDefault(t => t.Name.Equals(model.Name) && t.MaterialGroupId.Equals(model.MaterialGroupId));
                    if (data != null)
                    {
                        throw new Exception("Tên thông số đã tồn tại trong nhóm vật tư, hãy kiểm tra lại.");
                    }

                    db.MaterialParameters.Add(new MaterialParameter
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        MaterialGroupId = model.MaterialGroupId,
                        Index = db.MaterialParameters.Where(t => t.MaterialGroupId.Equals(model.MaterialGroupId)).ToList().Count + 1,
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
                var data = db.Specifications.AsNoTracking().Where(t => t.MaterialParameterId.Equals(Id)).ToList();
                if(data.Count > 0)
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
