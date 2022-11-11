using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.ClassRoom;
using NTS.Model.ClassRoomProduct;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.ModuleMaterials;
using NTS.Model.Repositories;
using NTS.Model.Skills;
using NTS.Model.Subjects;
using NTS.Model.SubjectsAttach;
using QLTK.Business.AutoMappers;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Practices;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Subjects
{
    public class SubjectsBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private PracticeBussiness practiceBussiness = new PracticeBussiness();

        public SearchResultModel<SubjectsResultModel> SearchSubjects(SubjectsSearchModel modelSearch)
        {
            SearchResultModel<SubjectsResultModel> searchResult = new SearchResultModel<SubjectsResultModel>();

            var dataQuery = (from a in db.Subjects.AsNoTracking()
                             join b in db.Degrees.AsNoTracking() on a.DegreeId equals b.Id
                             join c in db.SubjectsClassRooms.AsNoTracking() on a.Id equals c.SubjectsId into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.ClassRooms.AsNoTracking() on c.ClassRoomId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.SubjectsAttaches.AsNoTracking() on a.Id equals e.SubjectsId into ae
                             from e in ae.DefaultIfEmpty()
                             select new SubjectsResultModel
                             {
                                 Id = a.Id,
                                 DegreeId = b.Id,
                                 DegreeName = b.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 ClassRoomId = d.Id,
                                 ClassRoomName = d.Name,
                                 LearningPracticeTime = a.LearningPracticeTime,
                                 TotalLearningTime = a.TotalLearningTime,
                                 LearningTheoryTime = a.LearningTheoryTime,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            //if (!string.IsNullOrEmpty(modelSearch.Code))
            //{
            //    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            //}

            if (!string.IsNullOrEmpty(modelSearch.DegreeId))
            {
                dataQuery = dataQuery.Where(u => u.DegreeId.ToUpper().Contains(modelSearch.DegreeId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.ClassRoomId))
            {
                dataQuery = dataQuery.Where(u => u.ClassRoomId.ToUpper().Contains(modelSearch.ClassRoomId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.LearningPracticeTime.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.LearningPracticeTime.ToString().Contains(modelSearch.LearningPracticeTime.ToString()));
            }

            if (!string.IsNullOrEmpty(modelSearch.LearningTheoryTime.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.LearningTheoryTime.ToString().Contains(modelSearch.LearningTheoryTime.ToString()));
            }

            if (!string.IsNullOrEmpty(modelSearch.TotalLearningTime.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.TotalLearningTime.ToString().Contains(modelSearch.TotalLearningTime.ToString()));
            }

            List<SubjectsResultModel> listRs = new List<SubjectsResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Code, t.Name, t.TotalLearningTime, t.LearningTheoryTime, t.LearningPracticeTime, t.Description, t.DegreeName }).ToList();
            foreach (var item in lstRs)
            {
                SubjectsResultModel rs = new SubjectsResultModel();
                rs.Id = item.Key.Id;
                rs.Code = item.Key.Code;
                rs.Name = item.Key.Name;
                rs.TotalLearningTime = item.Key.TotalLearningTime;
                rs.LearningTheoryTime = item.Key.LearningTheoryTime;
                rs.LearningPracticeTime = item.Key.LearningPracticeTime;
                rs.Description = item.Key.Description;
                rs.DegreeName = item.Key.DegreeName;
                List<string> lstClasTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstClasTemp.Count > 0)
                    {
                        if (!lstClasTemp.Contains(ite.ClassRoomName))
                        {
                            rs.ClassRoomName += ", " + ite.ClassRoomName;
                            lstClasTemp.Add(ite.ClassRoomName);
                        }

                    }
                    else
                    {
                        rs.ClassRoomName += ite.ClassRoomName;
                        lstClasTemp.Add(ite.ClassRoomName);
                    }
                }
                listRs.Add(rs);
            }

            searchResult.TotalItem = listRs.Count();

            var listResult = SQLHelpper.OrderBy(listRs.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType)
               .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            //searchResult.TotalItem = listResult.Count();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<ClassRoomResultModel> SearchClassRoom(ClassRoomSearchModel modelSearch)
        {
            SearchResultModel<ClassRoomResultModel> searchResult = new SearchResultModel<ClassRoomResultModel>();
            var dataQuery = (from a in db.ClassRooms.AsNoTracking()
                             join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new ClassRoomResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 RoomTypeName = b.Name
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.Contains(modelSearch.Code));
            }
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.Contains(modelSearch.Name));
            }

            var listTemp = dataQuery.ToList();

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var classroom in listTemp)
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
            }
            searchResult.TotalItem = listTemp.Count;
            searchResult.ListResult = listTemp;
            return searchResult;
        }

        public SearchResultModel<SubjectSkillModel> SearchSkill(SkillsSearchModel modelSearch)
        {
            SearchResultModel<SubjectSkillModel> searchResult = new SearchResultModel<SubjectSkillModel>();
            var dataQuery = (from a in db.Skills.AsNoTracking()
                             join b in db.SkillGroups.AsNoTracking() on a.SkillGroupId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new SubjectSkillModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 SkillGroupName = b.Name
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }
            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddSubjects(SubjectsModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Subject newSubject = new Subject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Description = model.Description.NTSTrim(),
                        DegreeId = model.DegreeId,
                        TotalLearningTime = model.LearningPracticeTime + model.LearningTheoryTime,
                        LearningPracticeTime = model.LearningPracticeTime,
                        LearningTheoryTime = model.LearningTheoryTime,
                        Documents = model.Documents.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    foreach (var item in model.ListClassRoom)
                    {
                        SubjectsClassRoom classRoom = new SubjectsClassRoom()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SubjectsId = newSubject.Id,
                            ClassRoomId = item.Id,

                        };
                        db.SubjectsClassRooms.Add(classRoom);
                    }

                    foreach (var item in model.ListSkill)
                    {
                        SubjectSkill subjectSkill = new SubjectSkill()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SubjectId = newSubject.Id,
                            SkillId = item.Id,
                        };
                        db.SubjectSkills.Add(subjectSkill);
                    }

                    //ADD FILE
                    if (model.ListFile.Count > 0)
                    {
                        List<SubjectsAttach> listFileEntity = new List<SubjectsAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                SubjectsAttach fileEntity = new SubjectsAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SubjectsId = newSubject.Id,
                                    FileName = item.FileName,
                                    Path = item.Path,
                                    FileSize = item.FileSize,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = model.CreateBy,
                                    UpdateDate = DateTime.Now,
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.SubjectsAttaches.AddRange(listFileEntity);
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, newSubject.Id, Constants.LOG_Subject);

                    db.Subjects.Add(newSubject);
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

        public SubjectsModel GetIdSubjects(SubjectsModel model)
        {
            var resultInfo = db.Subjects.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new SubjectsModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Documents = p.Documents,
                Description = p.Description,
                DegreeId = p.DegreeId,
                TotalLearningTime = p.TotalLearningTime,
                LearningPracticeTime = p.LearningPracticeTime,
                LearningTheoryTime = p.LearningTheoryTime,


            }).FirstOrDefault();

            var ListClassRoom = (from a in db.SubjectsClassRooms.AsNoTracking()
                                 where a.SubjectsId.Equals(model.Id)
                                 join b in db.ClassRooms.AsNoTracking() on a.ClassRoomId equals b.Id
                                 orderby b.Name
                                 select new ClassRoomModel()
                                 {
                                     Id = b.Id,
                                     Code = b.Code,
                                     Name = b.Name,
                                     Description = b.Description,
                                     RoomTypeId = b.RoomTypeId,
                                 }).ToList();

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var classroom in ListClassRoom)
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
            }



            resultInfo.ListClassRoom = ListClassRoom;

            var ListSkill = (from a in db.SubjectSkills.AsNoTracking()
                             where a.SubjectId.Equals(model.Id)
                             join b in db.Skills.AsNoTracking() on a.SkillId equals b.Id
                             join c in db.SkillGroups.AsNoTracking() on b.SkillGroupId equals c.Id into bc
                             from cb in bc.DefaultIfEmpty()
                             orderby b.Name
                             select new SubjectSkillModel()
                             {
                                 Id = b.Id,
                                 Code = b.Code,
                                 Name = b.Name,
                                 SkillGroupName = cb.Name,
                                 Description = b.Description,
                             }).ToList();

            resultInfo.ListSkill = ListSkill;

            var ListFile = (from a in db.SubjectsAttaches.AsNoTracking()
                            where a.SubjectsId.Equals(model.Id)
                            orderby a.FileName
                            select new SubjectsAttachModel()
                            {
                                Id = a.Id,
                                Path = a.Path,
                                FileName = a.FileName,
                                FileSize = a.FileSize,
                                UpdateDate = a.UpdateDate,
                            }).ToList();

            resultInfo.ListFile = ListFile;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Subjects);
            }
            return resultInfo;
        }

        public void UpdateSubject(SubjectsModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSubject = db.Subjects.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                 

                        //var jsonApter = AutoMapperConfig.Mapper.Map<SubjectHistoryModel>(newSubject);

                        newSubject.Id = model.Id;
                        newSubject.Code = model.Code.Trim();
                        newSubject.Description = model.Description.NTSTrim();
                        newSubject.DegreeId = model.DegreeId;
                        newSubject.Name = model.Name.Trim();
                        newSubject.Documents = model.Documents.NTSTrim();
                        newSubject.TotalLearningTime = model.LearningPracticeTime + model.LearningTheoryTime;
                        newSubject.LearningPracticeTime = model.LearningPracticeTime;
                        newSubject.LearningTheoryTime = model.LearningTheoryTime;
                        newSubject.CreateBy = model.UpdateBy;
                        newSubject.UpdateBy = model.UpdateBy;
                        newSubject.UpdateDate = DateTime.Now;

                        var listClassRoom = db.SubjectsClassRooms.Where(u => u.SubjectsId.Equals(model.Id)).ToList();
                        if (listClassRoom.Count > 0)
                        {
                            db.SubjectsClassRooms.RemoveRange(listClassRoom);
                        }
                        if (model.ListClassRoom.Count > 0)
                        {
                            foreach (var item in model.ListClassRoom)
                            {
                                SubjectsClassRoom classRoom = new SubjectsClassRoom()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SubjectsId = newSubject.Id,
                                    ClassRoomId = item.Id,
                                };
                                db.SubjectsClassRooms.Add(classRoom);
                            }
                        }

                        var listskill = db.SubjectSkills.Where(u => u.SubjectId.Equals(model.Id)).ToList();
                        if (listskill.Count > 0)
                        {
                            db.SubjectSkills.RemoveRange(listskill);
                        }
                        if (model.ListSkill.Count > 0)
                        {
                            foreach (var item in model.ListSkill)
                            {
                                SubjectSkill subjectSkill = new SubjectSkill()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SubjectId = newSubject.Id,
                                    SkillId = item.Id,
                                };
                                db.SubjectSkills.Add(subjectSkill);
                            }
                        }


                        var fileEntities = db.SubjectsAttaches.Where(t => t.SubjectsId.Equals(model.Id)).ToList();
                        if (fileEntities.Count > 0)
                        {
                            db.SubjectsAttaches.RemoveRange(fileEntities);
                        }
                        if (model.ListFile.Count > 0)
                        {
                            List<SubjectsAttach> listFileEntity = new List<SubjectsAttach>();
                            foreach (var item in model.ListFile)
                            {
                                if (item.Path != null && item.Path != "")
                                {
                                    SubjectsAttach fileEntity = new SubjectsAttach()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        SubjectsId = newSubject.Id,
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
                            db.SubjectsAttaches.AddRange(listFileEntity);
                        }

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<SubjectHistoryModel>(newSubject);

                        //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Subject, newSubject.Id, newSubject.Code, jsonBefor, jsonApter);

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

        public void DeleteSubjects(SubjectsModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var subjects = db.Subjects.AsNoTracking().Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                {
                    var jobSubject = db.JobSubjects.AsNoTracking().Where(m => m.SubjectsId.Equals(model.Id)).FirstOrDefault();
                    if (jobSubject != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Subjects);
                    }

                    try
                    {
                        var _subjects = db.Subjects.FirstOrDefault(u => u.Id.Equals(model.Id));
                        {
                            if (_subjects == null)
                            {
                                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResource.Subjects);
                            }
                            var _subjectSkill = db.SubjectSkills.Where(a => a.SubjectId.Equals(model.Id)).ToList();
                            db.SubjectSkills.RemoveRange(_subjectSkill);

                            var _classRoom = db.SubjectsClassRooms.Where(u => u.SubjectsId.Equals(model.Id)).ToList();
                            db.SubjectsClassRooms.RemoveRange(_classRoom);

                            var _material = db.ClassRoomMaterials.Where(u => u.ClassRoomId.Equals(model.Id)).ToList();
                            db.ClassRoomMaterials.RemoveRange(_material);

                            var file = db.SubjectsAttaches.Where(u => u.SubjectsId.Equals(model.Id)).ToList();
                            if (file != null)
                            {
                                db.SubjectsAttaches.RemoveRange(file);
                            }

                            db.Subjects.Remove(_subjects);

                            //var jsonApter = AutoMapperConfig.Mapper.Map<SubjectHistoryModel>(_subjects);
                            //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Subject, _subjects.Id, _subjects.Code, jsonApter);

                            db.SaveChanges();
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new NTSLogException(model, ex);
                    }

                }
            }
        }

        public string ExportExcel(SubjectsSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.Subjects.AsNoTracking()
                             join b in db.Degrees.AsNoTracking() on a.DegreeId equals b.Id

                             join c in db.SubjectsClassRooms.AsNoTracking() on a.Id equals c.SubjectsId into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.ClassRooms.AsNoTracking() on c.ClassRoomId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()

                             join e in db.SubjectsAttaches.AsNoTracking() on a.Id equals e.SubjectsId into ae
                             from e in ae.DefaultIfEmpty()
                             select new SubjectsResultModel
                             {
                                 Id = a.Id,
                                 DegreeId = b.Id,
                                 DegreeName = b.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 ClassRoomId = d.Id,
                                 ClassRoomName = d.Name,
                                 LearningPracticeTime = a.LearningPracticeTime,
                                 TotalLearningTime = a.TotalLearningTime,
                                 LearningTheoryTime = a.LearningTheoryTime,
                             }).AsQueryable();

            List<SubjectsResultModel> listRs = new List<SubjectsResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Code, t.Name, t.TotalLearningTime, t.LearningTheoryTime, t.LearningPracticeTime, t.Description, t.DegreeName }).ToList();
            foreach (var item in lstRs)
            {
                SubjectsResultModel rs = new SubjectsResultModel();
                rs.Id = item.Key.Id;
                rs.Code = item.Key.Code;
                rs.Name = item.Key.Name;
                rs.TotalLearningTime = item.Key.TotalLearningTime;
                rs.LearningTheoryTime = item.Key.LearningTheoryTime;
                rs.LearningPracticeTime = item.Key.LearningPracticeTime;
                rs.Description = item.Key.Description;
                rs.DegreeName = item.Key.DegreeName;
                List<string> lstClasTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstClasTemp.Count > 0)
                    {
                        if (!lstClasTemp.Contains(ite.ClassRoomName))
                        {
                            rs.ClassRoomName += ", " + ite.ClassRoomName;
                            lstClasTemp.Add(ite.ClassRoomName);
                        }

                    }
                    else
                    {
                        rs.ClassRoomName += ite.ClassRoomName;
                        lstClasTemp.Add(ite.ClassRoomName);
                    }
                }
                listRs.Add(rs);
            }
            listRs = listRs.OrderBy(t => t.Code).ToList();
            //searchResult.TotalItem = listRs.Count();
            if (listRs.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Subjects.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = lstRs.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listRs.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.DegreeName,
                    a.TotalLearningTime,
                    a.LearningTheoryTime,
                    a.LearningPracticeTime,
                    a.ClassRoomName,
                    a.Description,
                });
                if (listExport.ToList().Count > 0)
                {
                    listExport = listExport.OrderBy(t => t.Code);
                }
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách môn học" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách môn học" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        private void CheckExistedForAdd(SubjectsModel model)
        {
            if (db.Subjects.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Subjects);
            }

            if (db.Subjects.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Subjects);
            }
        }

        public void CheckExistedForUpdate(SubjectsModel model)
        {
            if (db.Subjects.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Subjects);
            }

            if (db.Subjects.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Subjects);
            }
        }
    }
}
