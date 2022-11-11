using NTS.Model.Combobox;
using NTS.Model.ClassRoom;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using NTS.Model;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Skills;
using NTS.Model.Materials;
using Syncfusion.XlsIO;
using System.Web.Hosting;
using System.Web;
using NTS.Model.ClassRoomAttach;
using NTS.Model.ModuleRoomType;
using NTS.Model.ClassRoomMaterial;
using NTS.Model.ClassRoomProduct;
using QLTK.Business.ModuleMaterials;
using NTS.Model.ModuleMaterials;
using QLTK.Business.Practices;
using NTS.Model.DesignDocuments;
using NTS.Common.Logs;
using NTS.Model.ClassRoomDesignDocument;
using NTS.Caching;
using System.Configuration;
using NTS.Model.Config;
using QLTK.Business.Materials;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.HistoryVersion;

namespace QLTK.Business.ClassRoom
{
    public class ClassRoomBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private PracticeBussiness practiceBussiness = new PracticeBussiness();

        /// <summary>
        /// Tìm kiếm phòng học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ClassRoomResultModel> SearchClassRoom(ClassRoomSearchModel modelSearch)
        {
            SearchResultModel<ClassRoomResultModel> searchResult = new SearchResultModel<ClassRoomResultModel>();

            var dataQuery = (from a in db.ClassRooms.AsNoTracking()
                             join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id into ab
                             from ba in ab.DefaultIfEmpty()
                             select new ClassRoomResultModel
                             {
                                 Id = a.Id,
                                 RoomTypeId = ba.Id,
                                 RoomTypeName = ba.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Address = a.Address,
                                 Description = a.Description,
                                 Pricing = a.Price
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.RoomTypeId))
            {
                dataQuery = dataQuery.Where(u => u.RoomTypeId.ToUpper().Equals(modelSearch.RoomTypeId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Address))
            {
                dataQuery = dataQuery.Where(u => u.Address.ToUpper().Contains(modelSearch.Address.ToUpper()));
            }

            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
               .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.TotalItem = dataQuery.Count();
            //searchResult.TotalItem = listResult.Count();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void UpdatePriceClassRoom()
        {
            var dataQuery = (from a in db.ClassRooms.AsNoTracking()
                             join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id into ab
                             from ba in ab.DefaultIfEmpty()
                             select new ClassRoomResultModel
                             {
                                 Id = a.Id,
                                 RoomTypeId = ba.Id,
                                 RoomTypeName = ba.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Address = a.Address,
                                 Description = a.Description,
                             }).AsQueryable();
            foreach (var classroom in dataQuery)
            {
                decimal pricingMaterial = 0;
                decimal pricingModule = 0;
                decimal pricingProduct = 0;
                decimal pricingPractice = 0;

                // Lấy giá bài thực hành
                var pricePractice = (from b in db.ClassRoomPractices.AsNoTracking()
                                     where b.ClassRoomId.Equals(classroom.Id)
                                     join c in db.Practices.AsNoTracking() on b.PracticeId equals c.Id
                                     select new NTS.Model.ClassRoomPractice.PricePracticeModel()
                                     {
                                         PracticeId = b.PracticeId,
                                         LessonPrice = c.LessonPrice,
                                         Quantity = b.Quantity,

                                     }).ToList();

                foreach (var item in pricePractice)
                {
                    item.HardwarePrice = practiceBussiness.GetHardwarePrice(item.PracticeId);

                    pricingPractice += item.HardwarePrice + item.LessonPrice * item.Quantity;

                }
                // Lấy giá module

                var pricemodule = (from b in db.ClassRoomModules.AsNoTracking()
                                   where b.ClassRoomId.Equals(classroom.Id)
                                   select new NTS.Model.ClassRoomModule.PriceModuleModel()
                                   {
                                       ModuleId = b.ModuleId,
                                       Quantity = b.Quantity
                                   }).ToList();

                if (pricemodule.Count() > 0)
                {
                    foreach (var module in pricemodule)
                    {
                        pricingModule += moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                    }
                }

                // Lấy giá thiết bị

                var priceProduct = (from b in db.ClassRoomProducts.AsNoTracking()
                                    where b.ClassRoomId.Equals(classroom.Id)
                                    select new PriceProductModel()
                                    {
                                        Id = b.ProductId,
                                        Quantity = b.Quantity
                                    }).ToList();

                decimal moduleAmount = 0;
                foreach (var item in priceProduct)
                {
                    var modules = (from b in db.ProductModules.AsNoTracking()
                                   where b.ProductId.Equals(item.Id)
                                   select new
                                   {
                                       b.ModuleId,
                                       b.Quantity
                                   }).ToList();

                    moduleAmount = 0;
                    foreach (var module in modules)
                    {
                        moduleAmount += item.Quantity * module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                    }

                    pricingProduct += moduleAmount;
                }

                // Lấy giá vật tư phụ

                decimal PricingMaterial = 0;
                decimal PricingModule = 0;
                decimal PricingProduct = 0;

                // Trường hợp id là vật tư trong vật tư phụ
                var priceMaterialMaterial = (from a in db.ClassRoomMaterials.AsNoTracking()
                                             join b in db.Materials.AsNoTracking() on a.ObjectId equals b.Id
                                             where a.ClassRoomId.Equals(classroom.Id) && a.Type == 1
                                             select new
                                             {
                                                 b.Pricing,
                                                 b.LastBuyDate,
                                                 b.PriceHistory,
                                                 b.InputPriceDate,
                                                 a.Quantity
                                             }).ToList();
                MaterialBusiness materialBusiness = new MaterialBusiness();
                var day = materialBusiness.GetConfigMaterialLastByDate();

                foreach (var it in priceMaterialMaterial)
                {
                    if (it.LastBuyDate.HasValue)
                    {
                        TimeSpan timeSpan = DateTime.Now.Subtract(it.LastBuyDate.Value);

                        if (timeSpan.Days <= day)
                        {
                            PricingMaterial += it.PriceHistory * it.Quantity;
                        }
                        else if (!it.InputPriceDate.HasValue || it.InputPriceDate.Value.Date < it.LastBuyDate.Value.Date)
                        {
                            PricingMaterial += 0;
                        }

                    }
                }

                // Trường hợp id là module trong vật tư phụ
                var priceMaterialModule = (from a in db.ClassRoomMaterials.AsNoTracking()
                                           join b in db.Modules.AsNoTracking() on a.ObjectId equals b.Id
                                           where a.ClassRoomId.Equals(classroom.Id) && a.Type == 2
                                           select new NTS.Model.ClassRoomModule.PriceModuleModel()
                                           {
                                               ModuleId = b.Id,
                                               Pricing = b.Pricing,
                                               Quantity = a.Quantity,
                                           }).ToList();

                if (priceMaterialModule.Count() > 0)
                {
                    foreach (var module in priceMaterialModule)
                    {
                        PricingModule += moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0) * module.Quantity;
                    }
                }

                // Trường hợp id là thiết bị trong vật tư phụ

                decimal materialProductAmount = 0;
                var product = (from a in db.ClassRoomMaterials.AsNoTracking()
                               join b in db.ProductModules.AsNoTracking() on a.ObjectId equals b.ProductId
                               where a.ClassRoomId.Equals(classroom.Id) && a.Type == 3
                               select new
                               {
                                   b.ModuleId,
                                   b.Quantity,
                               }).ToList();

                materialProductAmount = 0;
                foreach (var module in product)
                {
                    materialProductAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                }

                PricingProduct = materialProductAmount;


                pricingMaterial += PricingMaterial + PricingModule + PricingProduct;


                classroom.Pricing = pricingProduct + pricingMaterial;
                var cl = db.ClassRooms.Where(clas => clas.Id.Equals(classroom.Id)).FirstOrDefault();
                cl.Price = classroom.Pricing;
            }
            db.SaveChanges();

        }

        /// <summary>
        /// Tìm kiếm vật tư
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<MaterialResultModel> SearchMaterial(ClassRoomMaterialSearch modelSearch)
        {
            SearchResultModel<MaterialResultModel> search = new SearchResultModel<MaterialResultModel>();

            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                             from adv in ad.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new MaterialResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 MaterialGroupId = c.Id,
                                 MaterialGroupName = c.Name,
                                 Note = a.Note,
                                 Pricing = a.Pricing,
                                 ManufactureCode = adv != null ? adv.Code : "",
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.MaterialGroupId))
            {
                dataQuery = dataQuery.Where(a => a.MaterialGroupId.Equals(modelSearch.MaterialGroupId));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in search.ListResult)
            {
                int i = 1;
                item.Index = i;
                i++;

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }

            }
            return search;
        }

        /// <summary>
        /// Tìm kiếm module
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ClassRoomModuleResultModel> SearchModule(ClassRoomModuleSearchModel modelSearch)
        {
            SearchResultModel<ClassRoomModuleResultModel> search = new SearchResultModel<ClassRoomModuleResultModel>();

            var dataQuery = (from a in db.Modules.AsNoTracking()
                             join c in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new ClassRoomModuleResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ModuleGroupId = ca.Id,
                                 ModuleGroupName = ca.Name,
                                 Description = a.Description,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.ModuleGroupId))
            {
                dataQuery = dataQuery.Where(a => a.ModuleGroupId.Equals(modelSearch.ModuleGroupId));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        /// <summary>
        /// Tìm kiếm bài thực hành
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ClassRoomPracticeResultModel> SearchPractice(ClassRoomPracticeSearchModel modelSearch)
        {
            SearchResultModel<ClassRoomPracticeResultModel> search = new SearchResultModel<ClassRoomPracticeResultModel>();

            var dataQuery = (from a in db.Practices.AsNoTracking()
                             join c in db.PracticeGroups.AsNoTracking() on a.PracticeGroupId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new ClassRoomPracticeResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 PracticeGroupId = ca.Id,
                                 PracticeGroupName = ca.Name,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.PracticeGroupId))
            {
                dataQuery = dataQuery.Where(a => a.PracticeGroupId.Equals(modelSearch.PracticeGroupId));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        /// <summary>
        /// Tìm kiếm thiết bị
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ClassRoomResultProductModel> SearchProduct(ClassRoomProductSearchModel modelSearch)
        {
            SearchResultModel<ClassRoomResultProductModel> search = new SearchResultModel<ClassRoomResultProductModel>();

            var dataQuery = (from a in db.Products.AsNoTracking()
                             join c in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new ClassRoomResultProductModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ProductGroupId = c.Id,
                                 ProductGroupName = c.Name,
                                 Description = a.Description,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.ProductGroupId))
            {
                dataQuery = dataQuery.Where(a => a.ProductGroupId.Equals(modelSearch.ProductGroupId));
            }

            search.TotalItem = dataQuery.Count();
            //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        /// <summary>
        /// Tìm kiếm kĩ năng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<SkillResultModel> SearchSkill(SkillsSearchModel modelSearch)
        {
            SearchResultModel<SkillResultModel> searchResult = new SearchResultModel<SkillResultModel>();
            var dataQuey = (from a in db.Skills.AsNoTracking()
                            join b in db.SkillGroups.AsNoTracking() on a.SkillGroupId equals b.Id
                            join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
                            where !modelSearch.ListIdSelect.Contains(a.Id)
                            orderby a.Code
                            select new SkillResultModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                Note = a.Note,
                                SkillGroupId = b.Id,
                                SkillGroupName1 = b.Code,
                                ParentId = b.ParentId
                            }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuey = dataQuey.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuey = dataQuey.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(modelSearch.SkillGroupId))
            {
                dataQuey = dataQuey.Where(a => a.SkillGroupId.Equals(modelSearch.SkillGroupId));
            }
            searchResult.TotalItem = dataQuey.Count();
            var list = dataQuey.ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.ParentId))
                    {
                        GetGroupParent(item);
                    }

                }
            }
            searchResult.ListResult = list;
            return searchResult;
        }

        public void GetGroupParent(SkillResultModel model)
        {
            var skillGroup = db.SkillGroups.AsNoTracking().FirstOrDefault(t => t.Id.Equals(model.ParentId));
            if (skillGroup != null)
            {
                model.SkillGroupName6 = model.SkillGroupName5;
                model.SkillGroupName5 = model.SkillGroupName4;
                model.SkillGroupName4 = model.SkillGroupName3;
                model.SkillGroupName3 = model.SkillGroupName2;
                model.SkillGroupName2 = model.SkillGroupName1;
                model.SkillGroupName1 = skillGroup.Code;
                model.ParentId = skillGroup.ParentId;
                if (!string.IsNullOrEmpty(skillGroup.ParentId))
                {
                    GetGroupParent(model);
                }
            }
        }

        /// <summary>
        /// Tìm kiếm loại phòng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<RoomTypeResultModel> SearchRoomType(RoomTypeSearchModel modelSearch)
        {
            SearchResultModel<RoomTypeResultModel> searchResult = new SearchResultModel<RoomTypeResultModel>();

            var dataQuery = (from a in db.RoomTypes.AsNoTracking()
                             orderby a.Code
                             select new RoomTypeResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        /// <summary>
        /// Thêm phòng học
        /// </summary>
        /// <param name="model"></param>
        public void AddClassRoom(ClassRoomModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.ClassRoom newClassRoom = new NTS.Model.Repositories.ClassRoom()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Address = model.Address.Trim(),
                        RoomTypeId = model.RoomTypeId,
                        Description = model.Description.Trim(),
                    };

                    // Thêm list module
                    foreach (var item in model.ListModule)
                    {
                        ClassRoomModule module = new ClassRoomModule()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            ModuleId = item.Id,
                            Quantity = item.Quantity,
                        };

                        db.ClassRoomModules.Add(module);

                    }
                    // Thêm list thiết bị
                    foreach (var item in model.ListProduct)
                    {
                        ClassRoomProduct product = new ClassRoomProduct()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            ProductId = item.Id,
                            Quantity = item.Quantity,
                        };

                        db.ClassRoomProducts.Add(product);

                    }
                    // Thêm list bài thực hành
                    foreach (var item in model.ListPractice)
                    {
                        ClassRoomPractice practice = new ClassRoomPractice()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            PracticeId = item.Id,
                            Quantity = item.Quantity,
                        };

                        db.ClassRoomPractices.Add(practice);
                    }
                    // Thêm list vật tư phụ

                    foreach (var item in model.ListMaterial)
                    {
                        ClassRoomMaterial material = new ClassRoomMaterial()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            ObjectId = item.Id,
                            Quantity = item.Quantity,
                            Type = item.Type,
                        };

                        db.ClassRoomMaterials.Add(material);

                    }

                    //ADD FILE


                    List<ClassRoomAttach> listFileEntity = new List<ClassRoomAttach>();

                    foreach (var item in model.ListFile)
                    {
                        if (!string.IsNullOrEmpty(item.Path))
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                ClassRoomAttach fileEntity = new ClassRoomAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ClassRoomId = newClassRoom.Id,
                                    Path = item.Path,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    UpdateBy = model.CreateBy,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                };

                                listFileEntity.Add(fileEntity);
                            }

                            //db.ClassRoomAttaches.Add(fileEntity);
                        }
                    }

                    db.ClassRoomAttaches.AddRange(listFileEntity);

                    db.ClassRooms.Add(newClassRoom);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newClassRoom.Code, newClassRoom.Id, Constants.LOG_ClassRoom);
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

        /// <summary>
        /// Lấy dữ liệu phòng học theo id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ClassRoomModel GetIdClassRoom(ClassRoomModel model)
        {
            decimal pricingPractice = 0;
            decimal PricingModule = 0;
            decimal PricingProduct = 0;

            var resultInfo = db.ClassRooms.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new ClassRoomModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                RoomTypeId = p.RoomTypeId,
                Address = p.Address,

            }).FirstOrDefault();


            // Danh sách module

            var listModule = (from a in db.ClassRoomModules.AsNoTracking()
                              where a.ClassRoomId.Equals(model.Id)
                              join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                              join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                              orderby b.Code
                              select new ClassRoomModuleModel()
                              {
                                  Id = b.Id,
                                  Code = b.Code,
                                  Name = b.Name,
                                  ClassRoomId = a.ClassRoomId,
                                  ModuleId = a.ModuleId,
                                  Quantity = a.Quantity,
                                  ModuleGroupName = c.Name,
                              }).ToList();

            ModulePriceInfoModel modulePriceInfoModel;
            if (listModule.Count() > 0)
            {
                foreach (var module in listModule)
                {
                    modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(module.Id);
                    module.Pricing += modulePriceInfoModel.Price;
                }
            }

            // Danh sách thiết bị

            var listProduct = (from a in db.ClassRoomProducts.AsNoTracking()
                               where a.ClassRoomId.Equals(model.Id)
                               join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                               join c in db.ProductGroups.AsNoTracking() on b.ProductGroupId equals c.Id
                               orderby b.Code
                               select new ClassRoomProductModel()
                               {
                                   Id = b.Id,
                                   Code = b.Code,
                                   Name = b.Name,
                                   ClassRoomId = a.ClassRoomId,
                                   ProductId = a.ProductId,
                                   Quantity = a.Quantity,
                                   PracticeGroupName = c.Name
                               }).ToList();

            decimal moduleAmount = 0;
            foreach (var item in listProduct)
            {
                var modules = (from b in db.ProductModules.AsNoTracking()
                               where b.ProductId.Equals(item.Id)
                               select new
                               {
                                   b.ModuleId,
                                   b.Quantity
                               }).ToList();

                moduleAmount = 0;
                foreach (var module in modules)
                {
                    moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                }

                item.Pricing = moduleAmount;
            }

            var totalPricing = listProduct.Sum(a => a.Pricing * a.Quantity);

            // Danh sách bài thực hành

            var listPractice = (from a in db.ClassRoomPractices.AsNoTracking()
                                where a.ClassRoomId.Equals(model.Id)
                                join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                                join c in db.PracticeGroups.AsNoTracking() on b.PracticeGroupId equals c.Id
                                orderby b.Code
                                select new ClassRoomPracticeModel()
                                {
                                    Id = b.Id,
                                    Code = b.Code,
                                    Name = b.Name,
                                    ClassRoomId = a.ClassRoomId,
                                    PracticeId = a.PracticeId,
                                    Quantity = a.Quantity,
                                    PracticeGroupName = c.Name,
                                }).ToList();

            foreach (var item in listPractice)
            {
                item.HardwarePrice = practiceBussiness.GetHardwarePrice(item.Id);

                item.TotalPrice += item.HardwarePrice + item.LessonPrice * item.Quantity;
                pricingPractice += item.TotalPrice;
            }
            // Danh sách vật tư phụ

            var ListMaterial = (from a in db.ClassRoomMaterials.AsNoTracking()
                                where a.ClassRoomId.Equals(model.Id) && a.Type == 1
                                join b in db.Materials.AsNoTracking() on a.ObjectId equals b.Id
                                orderby b.Code
                                select new ClassRoomMaterialResultModel()
                                {
                                    Id = b.Id,
                                    Code = b.Code,
                                    Name = b.Name,
                                    ClassRoomId = a.ClassRoomId,
                                    ObjectId = a.ObjectId,
                                    Type = a.Type,
                                    Quantity = a.Quantity,
                                    LastBuyDate = b.LastBuyDate,
                                    PriceHistory = b.PriceHistory,
                                    InputPriceDate = b.InputPriceDate,
                                    Pricing = b.Pricing
                                }).ToList();

            MaterialBusiness materialBusiness = new MaterialBusiness();
            // giá vâtj tư phụ
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var it in ListMaterial)
            {
                if (it.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(it.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        it.Pricing = it.PriceHistory;
                    }
                    else if (!it.InputPriceDate.HasValue || it.InputPriceDate.Value.Date < it.LastBuyDate.Value.Date)
                    {
                        it.Pricing = 0;
                    }
                }
            }

            // danh sách module trong vật tư phụ
            var ListModule = (from a in db.ClassRoomMaterials.AsNoTracking()
                              where a.ClassRoomId.Equals(model.Id) && a.Type == 2
                              join b in db.Modules.AsNoTracking() on a.ObjectId equals b.Id
                              orderby b.Code
                              select new ClassRoomMaterialResultModel()
                              {
                                  Id = b.Id,
                                  Code = b.Code,
                                  Name = b.Name,
                                  ClassRoomId = a.ClassRoomId,
                                  ObjectId = a.ObjectId,
                                  Type = a.Type,
                                  Quantity = a.Quantity,
                                  Pricing = b.Pricing,
                              }).ToList();

            decimal modulePriceInfoMaterial = 0;

            foreach (var module in ListModule)
            {
                modulePriceInfoMaterial = moduleMaterialBusiness.GetPriceModuleByModuleId(module.Id, 0);
                PricingModule += modulePriceInfoMaterial;
                module.Pricing = PricingModule;
            }


            // Danh sách thiết bị trong vật tư phụ
            var ListProduct = (from a in db.ClassRoomMaterials.AsNoTracking()
                               where a.ClassRoomId.Equals(model.Id) && a.Type == 3
                               join b in db.Products.AsNoTracking() on a.ObjectId equals b.Id
                               orderby b.Code
                               select new ClassRoomMaterialResultModel()
                               {
                                   Id = b.Id,
                                   Code = b.Code,
                                   Name = b.Name,
                                   ClassRoomId = a.ClassRoomId,
                                   ObjectId = a.ObjectId,
                                   Type = a.Type,
                                   Quantity = a.Quantity,
                               }).ToList();

            decimal materialProductAmount = 0;
            foreach (var it in ListProduct)
            {
                var product = (from b in db.ProductModules.AsNoTracking()
                               where b.ProductId.Equals(it.Id)
                               select new
                               {
                                   b.ModuleId,
                                   b.Quantity
                               }).ToList();

                materialProductAmount = 0;
                foreach (var module in product)
                {
                    materialProductAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                }

                PricingProduct += materialProductAmount;
                it.Pricing += PricingProduct;
            }
            List<ClassRoomMaterialResultModel> list = new List<ClassRoomMaterialResultModel>();
            list.AddRange(ListMaterial);
            list.AddRange(ListModule);
            list.AddRange(ListProduct);

            var totalPriceMarterial = list.Sum(a => a.Pricing * a.Quantity);

            var ListFile = (from a in db.ClassRoomAttaches.AsNoTracking()
                            where a.ClassRoomId.Equals(model.Id)
                            orderby a.FileName
                            select new ClassRoomAttachModel()
                            {
                                Id = a.Id,
                                Path = a.Path,
                                FileName = a.FileName,
                                FileSize = a.FileSize.Value,
                            }).ToList();

            // Trả lại list
            resultInfo.Pricing = totalPriceMarterial + totalPricing;
            resultInfo.ListModule = listModule;
            resultInfo.ListPractice = listPractice;
            resultInfo.ListProduct = listProduct;
            resultInfo.ListMaterial = list;
            resultInfo.ListFile = ListFile;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ClassRoom);
            }
            return resultInfo;
        }

        /// <summary>
        /// Cập nhật phòng học
        /// </summary>
        /// <param name="model"></param>
        public void UpdateClassRoom(ClassRoomModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newClassRoom = db.ClassRooms.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ClassRoomHistoryModel>(newClassRoom);

                    newClassRoom.Id = model.Id;
                    newClassRoom.Code = model.Code.Trim();
                    newClassRoom.Description = model.Description.Trim();
                    newClassRoom.Address = model.Address.Trim();
                    newClassRoom.Name = model.Name.Trim();
                    newClassRoom.RoomTypeId = model.RoomTypeId;

                    // Thêm list module

                    var listModule = db.ClassRoomModules.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                    if (listModule.Count > 0)
                    {
                        db.ClassRoomModules.RemoveRange(listModule);
                    }

                    foreach (var item in model.ListModule)
                    {
                        ClassRoomModule module = new ClassRoomModule()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            ModuleId = item.Id,
                            Quantity = item.Quantity,
                        };

                        db.ClassRoomModules.Add(module);

                    }
                    // Thêm list thiết bị

                    var listProduct = db.ClassRoomProducts.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                    if (listProduct.Count > 0)
                    {
                        db.ClassRoomProducts.RemoveRange(listProduct);
                    }

                    foreach (var item in model.ListProduct)
                    {
                        ClassRoomProduct product = new ClassRoomProduct()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            ProductId = item.Id,
                            Quantity = item.Quantity,
                        };

                        db.ClassRoomProducts.Add(product);

                    }
                    // Thêm list bài thực hành

                    var listPractice = db.ClassRoomPractices.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                    if (listPractice.Count > 0)
                    {
                        db.ClassRoomPractices.RemoveRange(listPractice);
                    }

                    foreach (var item in model.ListPractice)
                    {
                        ClassRoomPractice practice = new ClassRoomPractice()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomId = newClassRoom.Id,
                            PracticeId = item.Id,
                            Quantity = item.Quantity,
                        };

                        db.ClassRoomPractices.Add(practice);
                    }

                    // Thêm list vật tư phụ
                    var listMaterial = db.ClassRoomMaterials.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                    if (listMaterial.Count > 0)
                    {
                        db.ClassRoomMaterials.RemoveRange(listMaterial);
                    }

                    if (model.ListMaterial.Count > 0)
                    {
                        foreach (var item in model.ListMaterial)
                        {
                            ClassRoomMaterial material = new ClassRoomMaterial
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClassRoomId = newClassRoom.Id,
                                ObjectId = item.Id,
                                Type = item.Type,
                                Quantity = item.Quantity
                            };
                            db.ClassRoomMaterials.Add(material);
                        }
                    }

                    // Thêm file
                    var fileEntities = db.ClassRoomAttaches.Where(t => t.ClassRoomId.Equals(model.Id)).ToList();
                    if (fileEntities.Count > 0)
                    {
                        db.ClassRoomAttaches.RemoveRange(fileEntities);
                    }

                    if (model.ListFile.Count > 0)
                    {
                        List<ClassRoomAttach> listFileEntity = new List<ClassRoomAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                ClassRoomAttach fileEntity = new ClassRoomAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ClassRoomId = newClassRoom.Id,
                                    FileName = item.FileName,
                                    Path = item.Path,
                                    FileSize = item.FileSize,
                                    CreateBy = model.UpdateBy,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = model.UpdateBy,
                                    UpdateDate = DateTime.Now,
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.ClassRoomAttaches.AddRange(listFileEntity);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ClassRoomHistoryModel>(newClassRoom);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_ClassRoom, newClassRoom.Id, newClassRoom.Code, jsonBefor, jsonApter);

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

        public void DeleteClassRoom(ClassRoomModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var classRoom = db.SubjectsClassRooms.Where(u => u.ClassRoomId.Equals(model.Id)).FirstOrDefault();
                if (classRoom != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ClassRoom);
                }
                {
                    try
                    {
                        var _classRoom = db.ClassRooms.FirstOrDefault(u => u.Id.Equals(model.Id));

                        if (_classRoom == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResource.ClassRoom);
                        }

                        var _skill = db.ClassSkills.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                        db.ClassSkills.RemoveRange(_skill);

                        var _material = db.ClassRoomMaterials.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                        db.ClassRoomMaterials.RemoveRange(_material);

                        var modules = db.ClassRoomModules.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                        db.ClassRoomModules.RemoveRange(modules);

                        var products = db.ClassRoomProducts.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                        db.ClassRoomProducts.RemoveRange(products);

                        var practices = db.ClassRoomPractices.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                        db.ClassRoomPractices.RemoveRange(practices);

                        var file = db.ClassRoomAttaches.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                        if (file != null)
                        {
                            db.ClassRoomAttaches.RemoveRange(file);
                        }

                        db.ClassRooms.Remove(_classRoom);

                        //var jsonApter = AutoMapperConfig.Mapper.Map<ClassRoomHistoryModel>(_classRoom);

                        //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_ClassRoom, _classRoom.Id, _classRoom.Code, jsonApter);
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
        }

        public string ExportExcel(ClassRoomSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.ClassRooms.AsNoTracking()
                             join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id
                             join c in db.ClassSkills.AsNoTracking() on a.Id equals c.ClassRoomId into ac
                             from c in ac.DefaultIfEmpty()
                             select new ClassRoomResultModel
                             {
                                 Id = a.Id,
                                 RoomTypeId = b.Id,
                                 RoomTypeName = b.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Address = a.Address,
                                 Description = a.Description,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.RoomTypeId))
            {
                dataQuery = dataQuery.Where(u => u.RoomTypeId.ToUpper().Equals(model.RoomTypeId.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.Address))
            {
                dataQuery = dataQuery.Where(u => u.Address.ToUpper().Contains(model.Address.ToUpper()));
            }
            //List<ClassRoomResultModel> lstRs = dataQuery.ToList();
            if (dataQuery.Count() == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ClassRoom.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = dataQuery.Count();

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = dataQuery.OrderBy(i => i.Code).Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.RoomTypeName,
                    a.Address,
                    a.Description,
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách phòng học" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách phòng học" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        private void CheckExistedForAdd(ClassRoomModel model)
        {
            if (db.ClassRooms.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ClassRoom);
            }

            if (db.ClassRooms.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ClassRoom);
            }
        }

        public void CheckExistedForUpdate(ClassRoomModel model)
        {
            if (db.ClassRooms.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ClassRoom);
            }

            if (db.ClassRooms.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ClassRoom);
            }
        }

        public List<ClassRoomDesignDocumentModel> GetListFolderClassRoom(string classRoomId)
        {
            List<ClassRoomDesignDocumentModel> list = new List<ClassRoomDesignDocumentModel>();
            list = db.ClassRoomDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_Folder) && t.ClassRoomId.Equals(classRoomId))
                .OrderBy(o => o.Path)
                .Select(m => new ClassRoomDesignDocumentModel
                {
                    Id = m.Id,
                    ClassRoomId = m.ClassRoomId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

            var root = db.ClassRoomDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder) && string.IsNullOrEmpty(t.ClassRoomId))
                .OrderBy(o => o.Path).Select(m => new ClassRoomDesignDocumentModel
                {
                    Id = m.Id,
                    ClassRoomId = m.ClassRoomId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    Path = m.Path,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

            list.AddRange(root);
            return list;
        }

        public List<ClassRoomDesignDocumentModel> GetListFileClassRoom(string folderId)
        {
            var list = db.ClassRoomDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_File) && t.ParentId.Equals(folderId))
                 .OrderBy(o => o.Path)
                .Select(m => new ClassRoomDesignDocumentModel
                {
                    Id = m.Id,
                    ClassRoomId = m.ClassRoomId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    FileSize = m.FileSize,
                    CreateDate = m.CreateDate,
                    UpdateDate = m.UpdateDate,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

            return list;
        }

        /// <summary>
        /// Upload tài liệu thiết kế
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UploadDesignDocument(UploadFolderClassRoomDesignDocumentModel model, string userId)
        {
            var classRoom = db.ClassRooms.FirstOrDefault(t => t.Id.Equals(model.ClassRoomId));

            if (classRoom == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ClassRoom);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    // Có tài liệu thiết kế
                    if (model.DesignDocuments.Count > 0)
                    {
                        List<ClassRoomDesignDocument> designDocuments = new List<ClassRoomDesignDocument>();
                        ClassRoomDesignDocument designDocument;
                        ClassRoomDesignDocument designDocumentFile;

                        var documents = db.ClassRoomDesignDocuments.Where(r => r.DesignType == model.DesignType && r.ClassRoomId.Equals(classRoom.Id)).ToList();

                        bool isDelete;
                        foreach (var document in documents)
                        {
                            isDelete = true;
                            foreach (var item in model.DesignDocuments)
                            {
                                if (item.LocalPath.Equals(document.Path))
                                {
                                    isDelete = false;
                                    break;
                                }

                                foreach (var file in item.Files)
                                {
                                    if (file.LocalPath.Equals(document.Path))
                                    {
                                        isDelete = false;
                                        break;
                                    }
                                }

                                if (!isDelete)
                                {
                                    break;
                                }
                            }

                            if (isDelete)
                            {
                                db.ClassRoomDesignDocuments.Remove(document);
                            }
                        }

                        var folderRoor = db.ClassRoomDesignDocuments.Where(r => r.Id.Equals(model.DesignType.ToString())).FirstOrDefault();
                        if (folderRoor == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0026);
                        }

                        FolderUploadModel parent;
                        foreach (var item in model.DesignDocuments)
                        {
                            designDocument = db.ClassRoomDesignDocuments.FirstOrDefault(r => r.ClassRoomId.Equals(model.ClassRoomId) && r.Path.Equals(item.LocalPath));
                            if (designDocument == null)
                            {
                                designDocument = new ClassRoomDesignDocument()
                                {
                                    Id = item.Id,
                                    ClassRoomId = classRoom.Id,
                                    ParentId = item.ParentId,
                                    ServerPath = item.ServerPath,
                                    Path = item.LocalPath,
                                    Name = item.Name,
                                    FileType = Constants.ModuleDesignDocument_FileType_Folder,
                                    DesignType = model.DesignType,
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now
                                };

                                if (string.IsNullOrEmpty(designDocument.ParentId))
                                {
                                    designDocument.ParentId = folderRoor.Id;
                                }
                                else
                                {
                                    parent = model.DesignDocuments.FirstOrDefault(r => r.Id.Equals(item.ParentId));
                                    if (parent != null && !string.IsNullOrEmpty(parent.DBId))
                                    {
                                        designDocument.ParentId = parent.DBId;
                                    }
                                }

                                designDocuments.Add(designDocument);
                            }
                            else
                            {
                                item.DBId = designDocument.Id;
                                designDocument.UpdateBy = userId;
                                designDocument.UpdateDate = DateTime.Now;
                                designDocument.ServerPath = item.ServerPath;
                            }

                            foreach (var file in item.Files)
                            {
                                designDocumentFile = db.ClassRoomDesignDocuments.FirstOrDefault(r => r.ClassRoomId.Equals(model.ClassRoomId) && r.Path.Equals(item.LocalPath));
                                if (designDocumentFile == null)
                                {
                                    designDocumentFile = new ClassRoomDesignDocument()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ClassRoomId = classRoom.Id,
                                        ParentId = item.Id,
                                        ServerPath = file.ServerPath,
                                        Path = file.LocalPath,
                                        Name = file.Name,
                                        FileType = Constants.ModuleDesignDocument_FileType_File,
                                        FileSize = file.Size,
                                        DesignType = model.DesignType,
                                        CreateBy = userId,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = userId,
                                        UpdateDate = DateTime.Now
                                    };

                                    if (designDocument != null)
                                    {
                                        designDocumentFile.ParentId = designDocument.Id;
                                    }

                                    designDocuments.Add(designDocumentFile);
                                }
                                else
                                {
                                    designDocumentFile.UpdateBy = userId;
                                    designDocumentFile.UpdateDate = DateTime.Now;
                                    designDocumentFile.FileSize = file.Size;
                                    designDocumentFile.ServerPath = file.ServerPath;
                                }
                            }
                        }

                        db.ClassRoomDesignDocuments.AddRange(designDocuments);
                    }


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    NtsLog.LogError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Tính giá list module từ list chọn truyền lên
        /// </summary>
        /// <param name="listModule"></param>
        /// <returns></returns>
        public List<ClassRoomModuleResultModel> GetPriceModule(List<ClassRoomModuleResultModel> listModule)
        {
            foreach (var item in listModule)
            {
                item.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
            }
            return listModule;
        }

        /// <summary>
        /// Tính giá bài thực hành từ list chọn truyền lên
        /// </summary>
        /// <param name="listPractic"></param>
        /// <returns></returns>
        public List<ClassRoomPracticeResultModel> GetPricePractice(List<ClassRoomPracticeResultModel> listPractic)
        {
            foreach (var item in listPractic)
            {
                item.HardwarePrice = practiceBussiness.GetHardwarePrice(item.Id);

                item.TotalPrice = item.HardwarePrice + item.LessonPrice * item.Quantity;
            }

            return listPractic;
        }

        /// <summary>
        /// Tính giá thiết bị theo danh sách chọn truyền lên
        /// </summary>
        /// <param name="listProduct"></param>
        /// <returns></returns>
        public List<ClassRoomResultProductModel> GetPriceProduct(List<ClassRoomResultProductModel> listProduct)
        {
            foreach (var item in listProduct)
            {
                var modules = (from b in db.ProductModules.AsNoTracking()
                               where b.ProductId.Equals(item.Id)
                               select new
                               {
                                   b.ModuleId,
                                   b.Quantity
                               }).ToList();

                decimal moduleAmount = 0;
                foreach (var module in modules)
                {
                    moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                }

                item.Pricing = moduleAmount;
            }

            return listProduct;
        }



    }
}
