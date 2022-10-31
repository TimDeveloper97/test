using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.QLTKMG;
using NTS.Model.Repositories;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.MaterialGroupTPA;
using NTS.Common;
using NTS.Model;
using NTS.Common.Resource;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.MaterialGroup;

namespace QLTK.Business.QLTKMG
{
    public class QLTKMGBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public SearchResultModel<QLTKMGModel> GetListMaterialGroup(QLTKMGSearchModel modelSearch)
        {
            SearchResultModel<QLTKMGModel> searchResult = new SearchResultModel<QLTKMGModel>();
            try
            {
                List<QLTKMGModel> list = new List<QLTKMGModel>();
                var listMaterialGroup = (from o in db.MaterialGroups.AsNoTracking() 
                                         join b in db.MaterialGroupTPAs.AsNoTracking() on o.MaterialGroupTPAId equals b.Id
                                         orderby o.Code
                                         select new QLTKMGModel
                                         {
                                             Id = o.Id,
                                             Code = o.Code,
                                             //Description = o.Description,
                                             Name = o.Name,
                                             MaterialGroupTPAId = o.MaterialGroupTPAId,
                                             MaterialGroupTPACode = b.Code,
                                             MaterialGroupTPAName = b.Name,
                                             ParentId = o.ParentId,
                                             //CreateBy = o.CreateBy,
                                             //CreateDate = o.CreateDate,
                                             //UpdateBy = o.UpdateBy,
                                             //UpdateDate = o.UpdateDate,
                                         }).AsQueryable();

                if(!string.IsNullOrEmpty(modelSearch.Name) || !string.IsNullOrEmpty(modelSearch.Code) || !string.IsNullOrEmpty(modelSearch.MaterialGroupTPAId))
                {
                    list = listMaterialGroup.ToList();
                }

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    list = list.Where(t => t.Code.Contains(modelSearch.Code)).ToList();
                }

                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    list = list.Where(t => t.Name.Contains(modelSearch.Name)).ToList();
                }

                if (!string.IsNullOrEmpty(modelSearch.MaterialGroupTPAId))
                {
                    list = list.Where(t => modelSearch.MaterialGroupTPAId.Equals(t.MaterialGroupTPAId)).ToList();
                }

                List<QLTKMGModel> listRs = new List<QLTKMGModel>();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        var checkExist = listRs.FirstOrDefault(t => item.Id.Equals(t.Id));
                        if (checkExist == null)
                        {
                            listRs.Add(item);
                        }
                        if (!string.IsNullOrEmpty(item.ParentId))
                        {
                            var entity = listMaterialGroup.FirstOrDefault(t => item.ParentId.Equals(t.Id));
                            if(entity!=null)
                            {
                                var check = listRs.FirstOrDefault(t => entity.Id.Equals(t.Id));
                                if (check == null)
                                {
                                    listRs.Add(entity);
                                }
                                if (!string.IsNullOrEmpty(entity.ParentId))
                                {
                                    listRs = GetMaterialGroupParent(entity.ParentId, listRs, listMaterialGroup.ToList());
                                }
                            }
                        }
                    }
                }
                else if(string.IsNullOrEmpty(modelSearch.Name) && string.IsNullOrEmpty(modelSearch.Code)&& string.IsNullOrEmpty(modelSearch.MaterialGroupTPAId))
                {
                    listRs = listMaterialGroup.ToList();
                }
                else
                {
                    listRs = list.ToList();
                }
                var listResult = listRs.OrderBy(t => t.Code).ToList();
                searchResult.TotalItem = listResult.Count;
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return searchResult;
        }

        private List<QLTKMGModel> GetMaterialGroupParent(string parentId, List<QLTKMGModel> listRs, List<QLTKMGModel> data)
        {
            var entity = data.FirstOrDefault(t => parentId.Equals(t.Id));
            if(entity!=null)
            {
                var check = listRs.FirstOrDefault(t => entity.Id.Equals(t.Id));
                if (check == null)
                {
                    listRs.Add(entity);
                }
                if (!string.IsNullOrEmpty(entity.ParentId))
                {
                    GetMaterialGroupParent(entity.ParentId, listRs, data);
                }
            }
            return listRs;
        }

        private List<QLTKMGModel> GetMaterialGroupChild(string parentId,
          List<QLTKMGModel> listDocumentTemplateType, QLTKMGSearchModel modelSearch, string index)
        {
            List<QLTKMGModel> listResult = new List<QLTKMGModel>();
            var listChild = listDocumentTemplateType.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            int indexChild = 1;
            List<QLTKMGModel> listChildChild = new List<QLTKMGModel>();
            foreach (var child in listChild)
            {
                isSearch = true;
                if (!string.IsNullOrEmpty(modelSearch.Name) && !child.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                {
                    isSearch = false;
                }

                if (!string.IsNullOrEmpty(modelSearch.Code) && !child.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                {
                    isSearch = false;
                }

                if (!string.IsNullOrEmpty(modelSearch.MaterialGroupTPAId) && !child.MaterialGroupTPAId.ToLower().Contains(modelSearch.MaterialGroupTPAId.ToLower()))
                {
                    isSearch = false;
                }

                listChildChild = GetMaterialGroupChild(child.Id, listDocumentTemplateType, modelSearch, index + "." + indexChild);
                if (isSearch || listChildChild.Count > 0)
                {
                    child.Index = index + "." + indexChild;
                    listResult.Add(child);
                    indexChild++;
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }


        public void AddMaterialGroup(QLTKMGModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                //xoá ký tự đặc biệt
                model.Code = Util.RemoveSpecialCharacter(model.Code);

                if (db.MaterialGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.MaterialGroup);
                }

                if (db.MaterialGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.MaterialGroup);
                }

                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                try
                {
                    MaterialGroup newMaterialGroup = new MaterialGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Type = model.Type,
                        MaterialGroupTPAId = model.MaterialGroupTPAId,
                        Description = model.Description,
                        ParentId = model.ParentId,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };
                    db.MaterialGroups.Add(newMaterialGroup);

                    if (model.SupplierList != null)
                    {
                        foreach (var supplier in model.SupplierList)
                        {
                            ManufactureAvailable newManufactureAvailable = new ManufactureAvailable
                            {
                                Id = Guid.NewGuid().ToString(),
                                ManufactureId = supplier.Id,
                                MaterialGroupId = newMaterialGroup.Id
                            };
                            db.ManufactureAvailables.Add(newManufactureAvailable);
                        }
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, model.Id, Constants.LOG_MaterialGroup);

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

        public void UpdateMaterialGroup(QLTKMGModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            if (db.MaterialGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.MaterialGroup);
            }

            if (db.MaterialGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.MaterialGroup);
            }
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var materialGroup = db.MaterialGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<MaterialGroupHistoryModel>(materialGroup);

                    materialGroup.Name = model.Name.Trim();
                    materialGroup.Code = model.Code.Trim();
                    materialGroup.MaterialGroupTPAId = model.MaterialGroupTPAId;
                    materialGroup.Type = model.Type;
                    materialGroup.Description = model.Description;
                    materialGroup.ParentId = model.ParentId;
                    materialGroup.UpdateBy = model.UpdateBy;
                    materialGroup.UpdateDate = DateTime.Now;

                    var supplierAvailableList = db.ManufactureAvailables.Where(r => r.MaterialGroupId.Equals(model.Id)).ToList();
                    db.ManufactureAvailables.RemoveRange(supplierAvailableList);

                    if (model.SupplierList != null)
                    {
                        foreach (var supplier in model.SupplierList)
                        {
                            ManufactureAvailable newManufactureAvailables = new ManufactureAvailable
                            {
                                Id = Guid.NewGuid().ToString(),
                                ManufactureId = supplier.Id,
                                MaterialGroupId = model.Id
                            };
                            db.ManufactureAvailables.Add(newManufactureAvailables);
                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<MaterialGroupHistoryModel>(materialGroup);
                    //UserLogUtil.LogHistotyUpdateInfo(db, model.CreateBy, Constants.LOG_MaterialGroup, materialGroup.Id, materialGroup.Code, jsonBefor, jsonApter);

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

        public void DeleteMaterialGroup(QLTKMGModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Materials.AsNoTracking().Where(r => r.MaterialGroupId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MaterialGroup);
                }
                if (db.MaterialGroups.AsNoTracking().Where(r => r.ParentId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MaterialGroup);
                }
                if (db.CodeRules.AsNoTracking().Where(r => r.MaterialGroupId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MaterialGroup);
                }
                if (db.ManufactureInGroups.AsNoTracking().Where(r => r.ManufactureGroupId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.MaterialGroup);
                }

                try
                {
                    var materialGroup = db.MaterialGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (materialGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialGroup);
                    }
                    var manufactureAvailableList = db.ManufactureAvailables.Where(u => u.MaterialGroupId.Equals(model.Id)).ToList();
                    if (manufactureAvailableList != null && manufactureAvailableList.Count() > 0)
                    {
                        db.ManufactureAvailables.RemoveRange(manufactureAvailableList);
                    }


                    //var jsonApter = AutoMapperConfig.Mapper.Map<MaterialGroupHistoryModel>(materialGroup);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_MaterialGroup, materialGroup.Id, materialGroup.Code, jsonApter);

                    db.MaterialGroups.Remove(materialGroup);
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

        public QLTKMGModel GetMaterialGroupInfo(QLTKMGModel model)
        {
            var resultInfo = db.MaterialGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new QLTKMGModel
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Description = p.Description,
                Type = p.Type,
                MaterialGroupTPAId = p.MaterialGroupTPAId,
                Code = p.Code,
                Name = p.Name
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialGroup);
            }

            var supplierList = (from a in db.ManufactureAvailables.AsNoTracking()
                                join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                                where a.MaterialGroupId.Equals(model.Id)
                                select new QLTKMGModel
                                {
                                    Id = a.ManufactureId,
                                    Name = b.Name
                                });

            resultInfo.SupplierList = supplierList.ToList();

            return resultInfo;
        }



        //lấy cbb loại trừ theo Id
        public object GetCbbMaterialGroupFullChild(string Id)
        {

            List<ComboboxMultilevelResult> searchResult = new List<ComboboxMultilevelResult>();
            try
            {
                var listMaterialGroup = (from a in db.MaterialGroups.AsNoTracking()
                                         orderby a.Code ascending
                                         select a).ToList();

                int index = 1;
                StringBuilder nameBuild;
                foreach (var materialGroup in listMaterialGroup)
                {

                    if (string.IsNullOrEmpty(materialGroup.ParentId))
                    {
                        nameBuild = new StringBuilder();
                        nameBuild.Append(index);
                        nameBuild.Append(". ");
                        nameBuild.Append(materialGroup.Name);

                        searchResult.Add(new ComboboxMultilevelResult()
                        {
                            Id = materialGroup.Id,
                            Name = materialGroup.Name,
                            Code = materialGroup.Code,
                            ParentId = materialGroup.ParentId
                        });

                        searchResult.AddRange(GetMaterialGroupChild1(materialGroup.Id, index.ToString(), listMaterialGroup));

                        index++;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }

            List<ComboboxMultilevelResult> listCopare = new List<ComboboxMultilevelResult>();
            if (Id != null)
            {
                List<ComboboxResult> listById = GetCbbMaterialGroupById(Id);
                foreach (var item in searchResult)
                {
                    foreach (var itemId in listById)
                    {
                        if (item.Id.Equals(itemId.Id))
                        {
                            listCopare.Add(item);
                        }
                    }
                }
                return listCopare;
            }
            else
            {
                return searchResult;
            }

        }

        private List<ComboboxMultilevelResult> GetMaterialGroupChild1(string parentId, string index, List<MaterialGroup> listDeviceType)
        {
            List<ComboboxMultilevelResult> listChild = new List<ComboboxMultilevelResult>();
            var listDeviceTypeChild = listDeviceType.Where(r => parentId.Equals(r.ParentId)).OrderBy(r => r.Name).ToList();

            int indexChild = 1;
            foreach (var deviceType in listDeviceTypeChild)
            {

                listChild.Add(new ComboboxMultilevelResult()
                {
                    Id = deviceType.Id,
                    Name = deviceType.Name,
                    Code = deviceType.Code,
                    ParentId = deviceType.ParentId

                });

                listChild.AddRange(GetMaterialGroupChild1(deviceType.Id, index + "." + indexChild, listDeviceType));

                indexChild++;
            }

            return listChild;
        }


        public List<ComboboxResult> GetCbbMaterialGroupById(string Id)
        {
            List<ComboboxResult> search = new List<ComboboxResult>();
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.MaterialGroups.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code
                                 }
                               ).AsQueryable();

                var listMaterialGroup = (from o in db.MaterialGroups.AsNoTracking().OrderBy(r => r.Name)
                                         select new QLTKMGModel
                                         {
                                             Id = o.Id,
                                             Description = o.Description,
                                             Name = o.Name,
                                             Code = o.Code,
                                             ParentId = o.ParentId,
                                             CreateBy = o.CreateBy,
                                             CreateDate = o.CreateDate,
                                             UpdateBy = o.UpdateBy,
                                             UpdateDate = o.UpdateDate,
                                         }).AsQueryable();
                List<QLTKMGModel> listResult = new List<QLTKMGModel>();
                var listParent = listMaterialGroup.ToList().Where(r => r.Id.Equals(Id)).ToList();
                List<QLTKMGModel> listChild = new List<QLTKMGModel>();

                foreach (var parent in listParent)
                {

                    listChild = GetMaterialGroupChild(parent.Id, listMaterialGroup.ToList());
                    if (listChild.Count >= 0)
                    {
                        listResult.Add(parent);

                    }

                    listResult.AddRange(listChild);
                }
                searchResult = ListModel.ToList();

                for (int i = 0; i < listResult.Count(); i++)
                {
                    for (int u = 0; u < searchResult.Count(); u++)
                    {
                        if (searchResult[u].Id.Equals(listResult[i].Id))
                        {
                            search.Add(searchResult[u]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý");
            }
            return searchResult.Except(search).ToList();
        }

        private List<QLTKMGModel> GetMaterialGroupChild(string parentId, List<QLTKMGModel> listMaterialGroup)
        {
            List<QLTKMGModel> listResult = new List<QLTKMGModel>();
            var listChild = listMaterialGroup.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            List<QLTKMGModel> listChildChild = new List<QLTKMGModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetMaterialGroupChild(child.Id, listMaterialGroup);
                if (isSearch || listChildChild.Count > 0)
                {
                    listResult.Add(child);
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }
    }
}
