
using NTS.Model;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using NTS.Model.NSMaterialGroup;
using NTS.Model.NSMaterialParameter;
using NTS.Model.NSMaterialGroupAttach;
using NTS.Model.NSMaterialParameterValue;
using QLTK.Business.Users;
using NTS.Common;
using QLTK.Business.AutoMappers;
using NTS.Model.UnitHistory;
using NTS.Model.UserHistory;

namespace QLTK.Business.NSMaterialGroup
{
    public class NSMaterialGroupBusiness
    {
        QLTKEntities db = new QLTKEntities();
        string SecretKey = System.Configuration.ConfigurationManager.AppSettings["keyAuthorize"];
        string ApiFile = System.Configuration.ConfigurationManager.AppSettings["ApiFile"];
        public NSMaterialGroupResultModel SearchNSMaterialGroup(NSMaterialGroupSearchModel modelSearch)
        {
            NSMaterialGroupResultModel rs = new NSMaterialGroupResultModel();
            try
            {
                var data = (from a in db.NSMaterialGroups.AsNoTracking()
                            join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                            orderby a.Code
                            select new NSMaterialGroupModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                ManufactureId = a.ManufactureId,
                                ManufactureName = b.Code,
                                ListParameter = db.NSMaterialParameters.Where(t => t.NSMaterialGroupId.Equals(a.Id)).Select(m => new NSMaterialParameterModel
                                {
                                    Code = m.Code,
                                    Name = m.Name,
                                    Unit = m.Unit
                                }).OrderBy(t => t.Code).ToList()

                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    data = data.Where(t => t.Code.Contains(modelSearch.Code));
                }
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    data = data.Where(t => t.Name.Contains(modelSearch.Name));
                }
                if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
                {
                    data = data.Where(t => t.ManufactureId.Contains(modelSearch.ManufactureId));
                }
                var ret = data.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                rs.ListNSMaterialGroup = ret.ToList();
                rs.TotalItem = data.ToList().Count;
            }
            catch (Exception ex)
            {
                throw;
            }
            return rs;
        }

        public NSMaterialGroupResultModel GetNSMaterialGroup(NSMaterialGroupSearchModel modelSearch)
        {
            NSMaterialGroupResultModel rs = new NSMaterialGroupResultModel();
            try
            {
                var data = (from a in db.NSMaterialGroups.AsNoTracking()
                            join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                            orderby a.Name
                            select new NSMaterialGroupModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                ManufactureId = a.ManufactureId,
                                ManufactureName = b.Name,


                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    data = data.Where(t => t.Code.Contains(modelSearch.Code));
                }
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    data = data.Where(t => t.Name.Contains(modelSearch.Name));
                }
                if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
                {
                    data = data.Where(t => t.ManufactureId.Contains(modelSearch.ManufactureId));
                }

                rs.ListNSMaterialGroup = SQLHelpper.OrderBy(data, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

                foreach (var item in rs.ListNSMaterialGroup)
                {
                    item.ListParameter = db.NSMaterialParameters.AsNoTracking().Where(t => t.NSMaterialGroupId.Equals(item.Id)).Select(m => new NSMaterialParameterModel
                    {
                        Code = m.Code,
                        Name = m.Name,
                        Unit = m.Unit,
                        ListValue = db.NSMaterialParameterValues.AsNoTracking().Where(t => t.NSMaterialParameterId.Equals(m.Id)).Select(n => new NSMaterialParameterValueModel
                        {
                            Id = n.Id,
                            Value = n.Value
                        }).ToList()
                    }).OrderBy(t => t.Code).ToList();
                }

                rs.TotalItem = data.ToList().Count;
            }
            catch (Exception ex)
            {
                throw;
            }
            return rs;
        }

        public bool CreateNSMaterialGroup(NSMaterialGroupModel model)
        {
            bool rs = false;
            var nSMaterialGroup = db.NSMaterialGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).FirstOrDefault();
            if (nSMaterialGroup != null)
            {
                throw new Exception("Mã nhóm vật tư đã tồn tại trên hệ thống, hãy kiểm tra lại.");
            }

            if (string.IsNullOrEmpty(model.Code))
            {
                throw new Exception("Bạn không được để trống Mã thông số");
            }

            if (db.NSMaterialGroups.AsNoTracking().Where(o => model.Name.Equals(o.Name)).Count() > 0)
            {
                throw new Exception("Tên nhóm vật tư đã tồn tại trên hệ thống, hãy kiểm tra lại.");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.NSMaterialGroup entity = new NTS.Model.Repositories.NSMaterialGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Code = model.Code,
                        Description = model.Description,
                        ManufactureId = model.ManufactureId,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.NSMaterialGroups.Add(entity);

                    //Thêm mới thông số theo vật tư
                    if (model.ListParameter.Count > 0)
                    {
                        List<NSMaterialParameter> listParameterEntity = new List<NSMaterialParameter>();
                        var indexParameter = 1;
                        foreach (var item in model.ListParameter)
                        {
                            NSMaterialParameter paramEntity = new NSMaterialParameter()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = item.Name,
                                Code = item.Code,
                                NSMaterialGroupId = entity.Id,
                                Index = indexParameter,
                                ConnectCharacter = item.ConnectCharacter,
                                Unit = item.Unit,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now,
                            };
                            listParameterEntity.Add(paramEntity);
                            indexParameter++;
                            //Thêm mới giá trị theo thông số vật tư
                            if (item.ListValue.Count > 0)
                            {
                                List<NSMaterialParameterValue> listValueEntity = new List<NSMaterialParameterValue>();
                                var indexValue = 1;
                                foreach (var ite in item.ListValue)
                                {
                                    NSMaterialParameterValue valueEntity = new NSMaterialParameterValue()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Value = ite.Value,
                                        NSMaterialParameterId = paramEntity.Id,
                                        Description = ite.Description,
                                        Index = indexValue,
                                        CreateBy = model.CreateBy,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = model.CreateBy,
                                        UpdateDate = DateTime.Now,
                                    };
                                    listValueEntity.Add(valueEntity);
                                    indexValue++;
                                }
                                db.NSMaterialParameterValues.AddRange(listValueEntity);
                            }
                        }
                        db.NSMaterialParameters.AddRange(listParameterEntity);
                    }

                    //Thêm mới file đính kèm theo vật tư
                    if (model.ListFile.Count > 0)
                    {
                        List<NSMaterialGroupAttach> listFileEntity = new List<NSMaterialGroupAttach>();
                        foreach (var item in model.ListFile)
                        {
                            NSMaterialGroupAttach fileEntity = new NSMaterialGroupAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                NSMaterialGroupId = entity.Id,
                                Path = item.FilePath,
                                CreateDate = DateTime.Now,
                                FileSize = item.FileSize,
                                FileName = item.FileName
                            };
                            listFileEntity.Add(fileEntity);
                        }
                        db.NSMaterialGroupAttaches.AddRange(listFileEntity);
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, entity.Id, Constants.LOG_NSMaterialGrtoup);

                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rs;
        }

        public bool UpdateNSMaterialGroup(NSMaterialGroupModel model)
        {
            bool rs = false;
            var entity = db.NSMaterialGroups.FirstOrDefault(o => model.Id.Equals(o.Id));
            if (entity == null)
            {
                throw new Exception("Nhóm vật tư không tồn tại trên hệ thống, hãy kiểm tra lại.");
            }
            if (db.NSMaterialGroups.AsNoTracking().Where(o => model.Code.Equals(o.Code) && model.Id != o.Id).Count() > 0)
            {
                throw new Exception("Mã nhóm vật tư đã tồn tại trên hệ thống, hãy kiểm tra lại.");
            }
            if (db.NSMaterialGroups.AsNoTracking().Where(o => model.Name.Equals(o.Name) && model.Id != o.Id).Count() > 0)
            {
                throw new Exception("Tên nhóm vật tư đã tồn tại trên hệ thống, hãy kiểm tra lại.");
            }

            //var jsonApter = AutoMapperConfig.Mapper.Map<NSMaterialGroupHistoryModel>(entity);

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    entity.Name = model.Name;
                    entity.Code = model.Code;
                    entity.Description = model.Description;
                    entity.ManufactureId = model.ManufactureId;
                    entity.UpdateBy = model.UpdateBy;
                    entity.UpdateDate = DateTime.Now;

                    //Thêm mới thông số theo vật tư
                    if (model.ListParameter.Count > 0)
                    {
                        List<NSMaterialParameter> listParameterEntity = new List<NSMaterialParameter>();
                        var indexParameter = 1;
                        var listParam = db.NSMaterialParameters.Where(t => t.NSMaterialGroupId.Equals(model.Id));
                        if (listParam.ToList().Count > 0)
                        {
                            if (listParam.ToList().Count != model.ListParameter.Count)
                            {
                                foreach (var item in listParam)
                                {
                                    var valueEntities = db.NSMaterialParameterValues.Where(t => t.NSMaterialParameterId.Equals(item.Id)).ToList();
                                    db.NSMaterialParameterValues.RemoveRange(valueEntities);
                                }
                                db.NSMaterialParameters.RemoveRange(listParam);

                                foreach (var item in model.ListParameter)
                                {
                                    NSMaterialParameter param = new NSMaterialParameter()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Name = item.Name,
                                        Code = item.Code,
                                        NSMaterialGroupId = entity.Id,
                                        Index = indexParameter,
                                        ConnectCharacter = item.ConnectCharacter,
                                        Unit = item.Unit,
                                        CreateBy = model.UpdateBy,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = model.UpdateBy,
                                        UpdateDate = DateTime.Now,
                                    };
                                    listParameterEntity.Add(param);
                                    indexParameter++;
                                    //Thêm mới giá trị theo thông số vật tư
                                    if (item.ListValue.Count > 0)
                                    {
                                        List<NSMaterialParameterValue> listValueEntity = new List<NSMaterialParameterValue>();
                                        var indexValue = 1;
                                        foreach (var ite in item.ListValue)
                                        {
                                            NSMaterialParameterValue valueEntity = new NSMaterialParameterValue()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Value = ite.Value,
                                                NSMaterialParameterId = param.Id,
                                                Description = ite.Description,
                                                Index = indexValue,
                                                CreateBy = model.UpdateBy,
                                                CreateDate = DateTime.Now,
                                                UpdateBy = model.UpdateBy,
                                                UpdateDate = DateTime.Now,
                                            };
                                            listValueEntity.Add(valueEntity);
                                            indexValue++;
                                        }
                                        db.NSMaterialParameterValues.AddRange(listValueEntity);
                                    }
                                }

                                db.NSMaterialParameters.AddRange(listParameterEntity);
                            }
                        }
                    }

                    var fileEntities = db.NSMaterialGroupAttaches.Where(t => t.NSMaterialGroupId.Equals(model.Id));
                    db.NSMaterialGroupAttaches.RemoveRange(fileEntities);
                    //Thêm mới file đính kèm theo vật tư
                    if (model.ListFile.Count > 0)
                    {
                        List<NSMaterialGroupAttach> listFileEntity = new List<NSMaterialGroupAttach>();
                        foreach (var item in model.ListFile)
                        {
                            NSMaterialGroupAttach fileEntity = new NSMaterialGroupAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                NSMaterialGroupId = entity.Id,
                                FileName = item.FileName,
                                Path = item.FilePath,
                                CreateDate = DateTime.Now,
                                FileSize = item.FileSize
                            };
                            listFileEntity.Add(fileEntity);
                        }
                        db.NSMaterialGroupAttaches.AddRange(listFileEntity);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<NSMaterialGroupHistoryModel>(entity);
                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_NSMaterialGrtoup, entity.Id, entity.Code, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rs;
        }
        public NSMaterialGroupModel GetById(string Id)
        {
            NSMaterialGroupModel rs = new NSMaterialGroupModel();
            if (db.NSMaterialGroups.AsNoTracking().Where(o => Id.Equals(o.Id)).ToList().Count() == 0)
            {
                throw new Exception("Nhóm vật tư phi tiêu chuẩn không tồn tại");
            }
            try
            {
                rs = (from a in db.NSMaterialGroups.AsNoTracking()
                      join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                      where a.Id.Equals(Id)
                      select new NSMaterialGroupModel
                      {
                          Id = a.Id,
                          Code = a.Code,
                          Name = a.Name,
                          ManufactureId = a.ManufactureId,
                          ManufactureName = b.Name,
                          Description = a.Description,

                      }).FirstOrDefault();

                rs.ListParameter = db.NSMaterialParameters.AsNoTracking().Where(t => t.NSMaterialGroupId.Equals(rs.Id)).Select(m => new NSMaterialParameterModel
                {
                    Id = m.Id,
                    Code = m.Code,
                    Name = m.Name,
                    Unit = m.Unit,
                    ConnectCharacter = m.ConnectCharacter,
                    ListValue = db.NSMaterialParameterValues.AsNoTracking().Where(t => t.NSMaterialParameterId.Equals(m.Id)).Select(n => new NTS.Model.NSMaterialParameterValue.NSMaterialParameterValueModel
                    {
                        Value = n.Value,
                        Description = n.Description
                    }).ToList()
                }).OrderBy(t => t.Code).ToList();


                rs.ListFile = db.NSMaterialGroupAttaches.AsNoTracking().Where(t => t.NSMaterialGroupId.Equals(rs.Id)).Select(m => new NSMaterialGroupAttachModel
                {
                    Id = m.Id,
                    NSMaterialGroupId = m.NSMaterialGroupId,
                    FileName = m.FileName,
                    FilePath = m.Path,
                    FileSize = m.FileSize,
                    CreateDate = m.CreateDate
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
            return rs;
        }

        public bool Delete(string Id, string userLoginId)
        {
            var entity = db.NSMaterialGroups.FirstOrDefault(t => t.Id.Equals(Id));
            if (entity == null)
            {
                throw new Exception("Nhóm vật tư tiêu chuẩn không tồn tại");
            }
            //if (entity.NSMaterialParameters.ToList().Count > 0)
            //{
            //    throw new Exception("Nhóm vật tư tiêu chuẩn đang được sử dụng. Không thể xóa!");
            //}
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var listAttach = db.NSMaterialGroupAttaches.Where(t => t.NSMaterialGroupId.Equals(Id));
                    if (listAttach.ToList().Count > 0)
                    {
                        db.NSMaterialGroupAttaches.RemoveRange(listAttach);
                    }

                    var listParam = db.NSMaterialParameters.Where(t => t.NSMaterialGroupId.Equals(Id));
                    if (listParam.ToList().Count > 0)
                    {
                        foreach (var item in listParam)
                        {
                            var valueEntities = db.NSMaterialParameterValues.Where(t => t.NSMaterialParameterId.Equals(item.Id)).ToList();
                            db.NSMaterialParameterValues.RemoveRange(valueEntities);
                        }
                        db.NSMaterialParameters.RemoveRange(listParam);
                    }

                    //var jsonApter = AutoMapperConfig.Mapper.Map<NSMaterialGroupModel>(entity);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_MaterialGroupTPa, entity.Id, entity.Name, jsonApter);
                    db.NSMaterialGroups.Remove(entity);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return true;

        }
    }
}
