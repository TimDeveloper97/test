using NTS.Common;
using NTS.Model.MaterialGroup;
using NTS.Model.MaterialParameter;
using NTS.Model.MaterialParameterValue;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Materials
{
    public class ConfigMaterialBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public bool SaveConfig(UpdateMaterialGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                bool rs = false;
                try
                {
                    var listParameter = db.MaterialParameters.Where(t => t.MaterialGroupId.Equals(model.Id));
                    if (model.ListParameter.Count > 0)
                    {
                        List<string> listId = new List<string>();
                        if(listParameter.ToList().Count>0)
                        {
                            foreach (var item in model.ListParameter)
                            {
                                var checkNameParam = model.ListParameter.Where(t => t.Name.Equals(item.Name)).ToList();
                                if(checkNameParam.Count > 1)
                                {
                                    throw new Exception("Tên thông số đã tồn tại trên hệ thống, hãy kiểm tra lại");
                                }
                                MaterialParameter newEntityPM = new MaterialParameter();
                                if (!string.IsNullOrEmpty(item.Id))
                                {
                                    listId.Add(item.Id);
                                    var entityPM = listParameter.FirstOrDefault(t => t.Id.Equals(item.Id));
                                    entityPM.Name = item.Name;
                                    entityPM.UpdateBy = model.CreateBy;
                                    entityPM.UpdateDate = DateTime.Now;
                                }
                                else
                                {
                                    newEntityPM.Id = Guid.NewGuid().ToString();
                                    newEntityPM.Name = item.Name;
                                    newEntityPM.MaterialGroupId = model.Id;
                                    newEntityPM.Index = db.MaterialParameters.Where(t => t.MaterialGroupId.Equals(model.Id)).ToList().Count + 1;
                                    newEntityPM.CreateBy = model.CreateBy;
                                    newEntityPM.CreateDate = DateTime.Now;
                                    newEntityPM.UpdateBy = model.CreateBy;
                                    newEntityPM.UpdateDate = DateTime.Now;
                                    db.MaterialParameters.Add(newEntityPM);
                                    listId.Add(newEntityPM.Id);
                                    item.Id = newEntityPM.Id;
                                }

                                if (item.ListValue.Count > 0)
                                {
                                    List<string> listValueId = new List<string>();
                                    var listValue = db.MaterialParameterValues.Where(t => t.MaterialParameterId.Equals(item.Id));
                                    foreach (var ite in item.ListValue)
                                    {
                                        var checkValue = item.ListValue.Where(t=>t.Value.Equals(ite.Value)).ToList();
                                        if(checkValue.Count>1)
                                        {
                                            throw new Exception("Giá trị đã tồn tại trong thông số, hãy kiểm tra lại");
                                        }
                                        if (!string.IsNullOrEmpty(ite.Id))
                                        {
                                            listValueId.Add(ite.Id);
                                            var entityPMV = listValue.FirstOrDefault(t => t.Id.Equals(ite.Id));
                                            entityPMV.Value = ite.Value;
                                            entityPMV.UpdateBy = model.CreateBy;
                                            entityPMV.UpdateDate = DateTime.Now;
                                        }
                                        else
                                        {
                                            MaterialParameterValue entityPMV = new MaterialParameterValue();
                                            entityPMV.Id = Guid.NewGuid().ToString();
                                            entityPMV.Value = ite.Value;
                                            entityPMV.MaterialParameterId = item.Id;
                                            entityPMV.Index = db.MaterialParameterValues.Where(t => t.MaterialParameterId.Equals(item.Id)).ToList().Count + 1;
                                            entityPMV.CreateBy = model.CreateBy;
                                            entityPMV.CreateDate = DateTime.Now;
                                            entityPMV.UpdateBy = model.CreateBy;
                                            entityPMV.UpdateDate = DateTime.Now;
                                            db.MaterialParameterValues.Add(entityPMV);
                                            listValueId.Add(entityPMV.Id);
                                        }
                                    }
                                    foreach (var ite in listValue)
                                    {
                                        if (!listValueId.Contains(ite.Id))
                                        {
                                            db.MaterialParameterValues.Remove(ite);
                                        }
                                    }
                                }
                                else
                                {
                                    var listValue = db.MaterialParameterValues.Where(t => t.MaterialParameterId.Equals(item.Id)).ToList();
                                    if(listValue.Count > 0)
                                    {
                                        db.MaterialParameterValues.RemoveRange(listValue);
                                    }
                                }
                            }

                            foreach (var item in listParameter)
                            {
                                if(!listId.Contains(item.Id))
                                {
                                    db.MaterialParameters.Remove(item);
                                }
                            }
                        }
                        else
                        {
                            List<MaterialParameter> listMaterialParameter = new List<MaterialParameter>();
                            List<MaterialParameterValue> listParameterValue = new List<MaterialParameterValue>();
                            foreach (var item in model.ListParameter)
                            {
                                MaterialParameter newEntityPM = new MaterialParameter();
                                newEntityPM.Id = Guid.NewGuid().ToString();
                                newEntityPM.Name = item.Name;
                                newEntityPM.MaterialGroupId = model.Id;
                                newEntityPM.Index = db.MaterialParameters.Where(t => t.MaterialGroupId.Equals(model.Id)).ToList().Count + 1;
                                newEntityPM.CreateBy = model.CreateBy;
                                newEntityPM.CreateDate = DateTime.Now;
                                newEntityPM.UpdateBy = model.CreateBy;
                                newEntityPM.UpdateDate = DateTime.Now;
                                db.MaterialParameters.Add(newEntityPM);
                                
                                if (item.ListValue.Count > 0)
                                {
                                    foreach (var ite in item.ListValue)
                                    {
                                        MaterialParameterValue entityPMV = new MaterialParameterValue();
                                        entityPMV.Id = Guid.NewGuid().ToString();
                                        entityPMV.Value = ite.Value;
                                        entityPMV.MaterialParameterId = newEntityPM.Id;
                                        entityPMV.Index = db.MaterialParameterValues.Where(t => t.MaterialParameterId.Equals(item.Id)).ToList().Count + 1;
                                        entityPMV.CreateBy = model.CreateBy;
                                        entityPMV.CreateDate = DateTime.Now;
                                        entityPMV.UpdateBy = model.CreateBy;
                                        entityPMV.UpdateDate = DateTime.Now;
                                        db.MaterialParameterValues.Add(entityPMV);
                                    }
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        if(listParameter.ToList().Count>0)
                        {
                            foreach (var item in listParameter.ToList())
                            {
                                var listValue = db.MaterialParameterValues.Where(t => t.MaterialParameterId.Equals(item.Id)).ToList();
                                db.MaterialParameterValues.RemoveRange(listValue);
                            }
                            db.MaterialParameters.RemoveRange(listParameter.ToList());
                        }
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy,"Cấu hình vật tư có thay đổi", model.Id, Constants.LOG_ConfigMaterial);
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

        public List<MaterialGroupModel> GetMaterialGroup()
        {
            List<MaterialGroupModel> rs = new List<MaterialGroupModel>();
            try
            {
                var dataMG = db.MaterialGroups.AsNoTracking().ToList();
                var dataMP = db.MaterialParameters.AsNoTracking().ToList();
                var dataMPV = db.MaterialParameterValues.AsNoTracking().ToList();

                if(dataMG.Count > 0)
                {
                    foreach (var item in dataMG)
                    {
                        MaterialGroupModel groupModel = new MaterialGroupModel();
                        groupModel.Id = item.Id;
                        groupModel.Name = item.Name;
                        groupModel.ListParameter = new List<MaterialParameterModel>();
                        var listMP = dataMP.Where(t => t.MaterialGroupId.Equals(item.Id)).ToList();
                        if(listMP.Count>0)
                        {
                            foreach (var ite in listMP)
                            {
                                MaterialParameterModel paramModel = new MaterialParameterModel();
                                paramModel.Id = ite.Id;
                                paramModel.MaterialGroupId = ite.MaterialGroupId;
                                paramModel.Name = ite.Name;
                                paramModel.ListValue = new List<MaterialParameterValueModel>();
                                var listMPV = dataMPV.Where(t=>t.MaterialParameterId.Equals(ite.Id)).ToList();
                                if(listMPV.Count>0)
                                {
                                    foreach (var it in listMPV)
                                    {
                                        MaterialParameterValueModel valueModel = new MaterialParameterValueModel();
                                        valueModel.Id = it.Id;
                                        valueModel.MaterialParameterId = it.MaterialParameterId;
                                        valueModel.Value = it.Value;
                                        paramModel.ListValue.Add(valueModel);
                                    }
                                }
                                groupModel.ListParameter.Add(paramModel);
                            }
                        }
                        rs.Add(groupModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rs;
        }


        public List<MaterialGroupModel> GetListMaterialGroup()
        {
            List<MaterialGroupModel> rs = new List<MaterialGroupModel>();
            rs = db.MaterialGroups.AsNoTracking().Select(m=>new MaterialGroupModel {
                Id = m.Id,
                ParentId = m.ParentId,
                Code = m.Code,
                Name = m.Name,
                Type = m.Type,
            }).OrderBy(t=>t.Name).ToList();
            return rs;
        }
    }
}
