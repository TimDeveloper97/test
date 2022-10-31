using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ModuleMaterials;
using NTS.Model.Practice;
using NTS.Model.PracticeHistory;
using NTS.Model.PracticeOldVersion;
using NTS.Model.PracticeSkill;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using NTS.Model.Skills;
using QLTK.Business.AutoMappers;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Practices
{
    public class PracticeBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        public SearchResultModel<PracticeModel> SearchPractice(PracticeSearchModel modelSearch)
        {

            SearchResultModel<PracticeModel> searchResult = new SearchResultModel<PracticeModel>();
            List<string> listParentId = new List<string>();

            var ListSkillId = (from b in db.PracticeSkills.AsNoTracking()
                               join c in db.Skills.AsNoTracking() on b.SkillId equals c.Id
                               select new PracticeSkillModel
                               {
                                   SkillId = c.Id,
                                   PracticeId = b.PracticeId
                               }).AsQueryable();

            var dataQuery = (from a in db.Practices.AsNoTracking()
                             join b in db.PracticeSkills.AsNoTracking() on a.Id equals b.PracticeId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Skills.AsNoTracking() on b.SkillId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             orderby a.Code
                             select new
                             {
                                 a.Id,
                                 a.PracticeGroupId,
                                 a.Code,
                                 a.Name,
                                 CurentVersion = a.CurentVersion.ToString(),
                                 a.Note,
                                 a.TrainingTime,
                                 a.MaterialConsumableExist,
                                 a.MaterialConsumable,
                                 a.SupMaterial,
                                 a.SupMaterialExist,
                                 a.PracticeFile,
                                 a.PracticeExist,
                                 a.LessonPrice,
                                 a.Quantity,
                                 SkillName = c.Name,
                                 SkillId = c.Id
                             }).AsQueryable();

            // tìm kiếm theo mã nhóm    
            if (!string.IsNullOrEmpty(modelSearch.PracticeGroupId))
            {
                //dataQuery = dataQuery.Where(t => modelSearch.PracticeGroupId.Equals(t.PracticeGroupId));
                var practiceGroup = db.PracticeGroups.AsNoTracking().FirstOrDefault(i => i.Id.Equals(modelSearch.PracticeGroupId));
                if (practiceGroup != null)
                {
                    listParentId.Add(practiceGroup.Id);
                }
                listParentId.AddRange(GetListParent(modelSearch.PracticeGroupId));
                var listPracticeGroup = listParentId.AsQueryable();
                dataQuery = (from a in dataQuery
                             where listParentId.Contains(a.PracticeGroupId)
                             select new
                             {
                                 a.Id,
                                 a.PracticeGroupId,
                                 a.Code,
                                 a.Name,
                                 CurentVersion = a.CurentVersion.ToString(),
                                 a.Note,
                                 a.TrainingTime,
                                 a.MaterialConsumableExist,
                                 a.MaterialConsumable,
                                 a.SupMaterial,
                                 a.SupMaterialExist,
                                 a.PracticeFile,
                                 a.PracticeExist,
                                 a.LessonPrice,
                                 a.Quantity,
                                 a.SkillName,
                                 a.SkillId
                             }).AsQueryable();
            }

            //if (!string.IsNullOrEmpty(modelSearch.Name))
            //{
            //    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            //}

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (modelSearch.PracaticeId != null && modelSearch.PracaticeId.Count > 0)
            {
                dataQuery = dataQuery.Where(u => modelSearch.PracaticeId.Contains(u.Id));
            }

            if (modelSearch.SkillId != null && modelSearch.SkillId.Count > 0)
            {
                dataQuery = dataQuery.Where(u => modelSearch.SkillId.Contains(u.SkillId));
            }

            var lstRs = dataQuery.GroupBy(t => new
            {
                Id = t.Id,
                PracticeGroupId = t.PracticeGroupId,
                Name = t.Name,
                Code = t.Code,
                CurentVersion = t.CurentVersion,
                Note = t.Note,
                TrainingTime = t.TrainingTime,
                SupMaterial = t.SupMaterial,
                SupMaterialExist = t.SupMaterialExist,
                PracticeFile = t.PracticeFile,
                PracticeExist = t.PracticeExist,
                MaterialConsumable = t.MaterialConsumable,
                MaterialConsumableExist = t.MaterialConsumableExist,
                Quantity = t.Quantity,
                LessonPrice = t.LessonPrice
            }).Select(s => new PracticeModel
            {
                Id = s.Key.Id,
                PracticeGroupId = s.Key.PracticeGroupId,
                Name = s.Key.Name,
                Code = s.Key.Code,
                CurentVersion = s.Key.CurentVersion,
                Note = s.Key.Note,
                TrainingTime = s.Key.TrainingTime,
                SupMaterial = s.Key.SupMaterial,
                SupMaterialExist = s.Key.SupMaterialExist,
                PracticeFile = s.Key.PracticeFile,
                PracticeExist = s.Key.PracticeExist,
                MaterialConsumable = s.Key.MaterialConsumable,
                MaterialConsumableExist = s.Key.MaterialConsumableExist,
                Quantity = s.Key.Quantity,
                LessonPrice = s.Key.LessonPrice
            }).ToList();

            searchResult.TotalItem = lstRs.Count();
            var listResult = lstRs.OrderBy(i => i.Code).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            foreach (var item in listResult)
            {
                if (item.PracticeExist == true)
                {
                    item.IsPracticeFile = 1;
                }

                if (item.MaterialConsumableExist == true)
                {
                    item.IsMaterialConsumable = 1;
                }

                if (item.SupMaterialExist == true)
                {
                    item.IsSupMaterial = 1;
                }

                //item.ListSkillId = ListSkillId.Where(b => b.PracticeId.Equals(item.Id)).Select(c => c.SkillId).ToList();

                item.HardwarePrice = GetHardwarePrice(item.Id);

                item.TotalPrice = item.HardwarePrice + item.LessonPrice * item.Quantity;
            }

            searchResult.ListResult = listResult;
            return searchResult;
        }
        public SearchResultModel<SkillsModel> SearchSkills(SkillsSearchModel modelSearch)
        {
            SearchResultModel<SkillsModel> searchResult = new SearchResultModel<SkillsModel>();
            var dataQuey = (from a in db.Skills.AsNoTracking()
                            join b in db.SkillGroups.AsNoTracking() on a.SkillGroupId equals b.Id
                            join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
                            //where !modelSearch.ListIdSelect.Contains(a.Id)
                            orderby a.Name
                            select new SkillsModel
                            {
                                Id = a.Id,
                                SkillGroupId = a.SkillGroupId,
                                SkillGroupName = b.Name,
                                DegreeId = a.DegreeId,
                                DegreeName = c.Name,
                                Code = a.Code,
                                Name = a.Name,
                                Description = a.Description,
                                Note = a.Note,
                                CreateBy = a.CreateBy,
                                CreateDate = a.CreateDate,
                                UpdateBy = a.UpdateBy,
                                UpdateDate = a.UpdateDate,
                            }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuey = dataQuey.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            searchResult.TotalItem = dataQuey.Count();
            var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }
        public string AddPractice(PracticeModel model)
        {
            string id;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Practice newPractice = new Practice
                    {
                        Id = Guid.NewGuid().ToString(),
                        PracticeGroupId = model.PracticeGroupId,
                        Code = model.Code,
                        Name = model.Name,
                        CurentVersion = Convert.ToInt32(model.CurentVersion),
                        Note = model.Note,
                        TrainingTime = model.TrainingTime,
                        LessonPrice = model.LessonPrice,
                        HardwarePrice = model.HardwarePrice,
                        UnitId = model.UnitId,
                        Quantity = model.Quantity,
                        TotalPrice = model.TotalPrice,
                        LeadTime = model.LeadTime,
                        Content = model.Content,
                        MaterialConsumable = model.MaterialConsumable, // vật tư tiêu hao
                        PracticeFile = model.PracticeFile,
                        SupMaterial = model.SupMaterial,

                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    var moduleGroupCode = db.PracticeGroups.AsNoTracking().FirstOrDefault(a => a.Id.Equals(model.PracticeGroupId)).Code;
                    if (model.Code.StartsWith(moduleGroupCode))
                    {
                        db.Practices.Add(newPractice);
                    }
                    else
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0028, TextResourceKey.Practice);
                        //throw NTSException.CreateInstance("Mã module phải trùng với Mã nhóm module");
                    }
                    PracticeOldVersion oldVersion = new PracticeOldVersion
                    {
                        Id = Guid.NewGuid().ToString(),
                        PracticeId = newPractice.Id,
                        Content = model.Content,
                        Version = Convert.ToInt32(model.CurentVersion),
                        Description = model.Note,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                    };
                    db.PracticeOldVersions.Add(oldVersion);
                    id = newPractice.Id;

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newPractice.Code, newPractice.Id, Constants.LOG_Practice);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }

            return id;
        }
        public bool UpdatePractice(PracticeModel model)
        {
            bool rs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newPractice = db.Practices.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<PracticeHistoryModel>(newPractice);

                    newPractice.PracticeGroupId = model.PracticeGroupId;
                    newPractice.Name = model.Name;
                    newPractice.Code = model.Code;
                    newPractice.Content = model.Content;

                    if (Convert.ToInt32(newPractice.CurentVersion) > Convert.ToInt32(model.CurentVersion))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0011);
                    }

                    if (Convert.ToInt32(model.CurentVersion) != newPractice.CurentVersion)
                    {
                        newPractice.CurentVersion = Convert.ToInt32(model.CurentVersion);
                        newPractice.EditContent = model.EditContent;
                        PracticeOldVersion oldVersion = new PracticeOldVersion
                        {
                            Id = Guid.NewGuid().ToString(),
                            PracticeId = model.Id,
                            Content = model.EditContent,
                            Version = Convert.ToInt32(model.CurentVersion),
                            Description = model.Note,
                            CreateBy = model.UpdateBy,
                            CreateDate = DateTime.Now,
                        };
                        db.PracticeOldVersions.Add(oldVersion);
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.UpdateBy, Constants.LOG_Practice, model.Id, string.Empty, "Nội dung");

                    newPractice.Note = model.Note;
                    newPractice.TrainingTime = model.TrainingTime;
                    newPractice.LessonPrice = model.LessonPrice;
                    newPractice.HardwarePrice = model.HardwarePrice;
                    newPractice.UnitId = model.UnitId;
                    newPractice.Quantity = model.Quantity;
                    newPractice.TotalPrice = Convert.ToDecimal(model.LessonPrice * model.Quantity);
                    newPractice.UpdateBy = model.UpdateBy;
                    newPractice.UpdateDate = DateTime.Now;
                    newPractice.MaterialConsumable = model.MaterialConsumable;
                    newPractice.SupMaterial = model.SupMaterial;
                    newPractice.PracticeFile = model.PracticeFile;
                    newPractice.LeadTime = model.LeadTime;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<PracticeHistoryModel>(newPractice);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Practice, newPractice.Id, newPractice.Code, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return rs;
        }
        public void DeletePractice(PracticeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var practice = db.PracticeSkills.AsNoTracking().Where(m => m.PracticeId.Equals(model.Id)).FirstOrDefault();
                if (practice != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Practice);
                }

                try
                {
                    var practice1 = db.Practices.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (practice1 == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Practice);
                    }
                    var practiceOldVersion = db.PracticeOldVersions.Where(u => u.PracticeId.Equals(model.Id)).ToList();
                    if (practiceOldVersion.Count > 0)
                    {
                        db.PracticeOldVersions.RemoveRange(practiceOldVersion);
                    }
                    db.Practices.Remove(practice1);

                    var NameOrCode = practice1.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<PracticeHistoryModel>(practice1);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Practice, practice1.Id, NameOrCode, jsonApter);

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
        public object GetPracticeInfo(PracticeModel model)
        {
            var resultInfo = db.Practices.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new PracticeModel
            {
                Id = p.Id,
                PracticeGroupId = p.PracticeGroupId,
                Code = p.Code,
                Name = p.Name,
                CurentVersion = p.CurentVersion.ToString(),
                Note = p.Note,
                Content = p.Content,
                //Content = quary.Content,
                TrainingTime = p.TrainingTime,
                LessonPrice = p.LessonPrice,
                HardwarePrice = p.HardwarePrice,
                UnitId = p.UnitId,
                Quantity = p.Quantity,
                TotalPrice = p.TotalPrice,
                LeadTime = p.LeadTime,
                EditContent = p.EditContent,
                MaterialConsumable = p.MaterialConsumable, // vật tư tiêu hao
                PracticeFile = p.PracticeFile,
                SupMaterial = p.SupMaterial,
            }).FirstOrDefault();


            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Practice);
            }

            var listHistory = (from a in db.PracticeOldVersions.AsNoTracking()
                               join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                               join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                               join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                               where a.PracticeId.Equals(model.Id)
                               orderby a.CreateDate descending
                               select new PracticeOldVersionModel
                               {
                                   Id = a.Id,
                                   PracticeId = a.PracticeId,
                                   Content = a.Content,
                                   Version = a.Version,
                                   Description = a.Description,
                                   CreateBy = a.CreateBy,
                                   CreateByName = d.Name,
                                   CreateDate = a.CreateDate
                               }).ToList();

            resultInfo.ListHistory = listHistory;

            resultInfo.HardwarePrice = GetHardwarePrice(resultInfo.Id);

            return resultInfo;

        }

        public string ExportExcel(PracticeSearchModel modelSearch)
        {
            var ListSkillId = (from c in db.Skills.AsNoTracking()
                                   //where b.PracticeId.Equals(a.Id)
                               select new PracticeSkillModel
                               {
                                   SkillId = c.Id,
                                   SkillName = c.Name,
                               }).OrderBy(t => t.SkillName).ToList();

            var ListPracticeSkillId = (from b in db.PracticeSkills.AsNoTracking()
                                       join c in db.Skills.AsNoTracking() on b.SkillId equals c.Id
                                       //where b.PracticeId.Equals(a.Id)
                                       select new PracticeSkillModel
                                       {
                                           SkillId = c.Id,
                                           SkillName = c.Name,
                                           PracticeId = b.PracticeId
                                       }).OrderBy(t => t.SkillName).ToList();


            var dataQuery = (from a in db.Practices.AsNoTracking()
                             join b in db.PracticeSkills.AsNoTracking() on a.Id equals b.PracticeId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Skills.AsNoTracking() on b.SkillId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             orderby a.Code
                             select new
                             {
                                 Id = a.Id,
                                 PracticeGroupId = a.PracticeGroupId,
                                 Code = a.Code,
                                 Name = a.Name,
                                 CurentVersion = a.CurentVersion.ToString(),
                                 Note = a.Note,
                                 TrainingTime = a.TrainingTime,
                                 TotalPrice = a.TotalPrice,
                                 MaterialConsumableExist = a.MaterialConsumableExist,
                                 MaterialConsumable = a.MaterialConsumable,
                                 SupMaterial = a.SupMaterial,
                                 SupMaterialExist = a.SupMaterialExist,
                                 PracticeFile = a.PracticeFile,
                                 PracticeExist = a.PracticeExist,

                                 //ListSkillId = ListSkillId.Where(b => b.PracticeId.Equals(a.Id)).ToList(),
                                 SkillName = c.Name,
                                 SkillId = c.Id,
                                 //ListSkillId = (from b in db.PracticeSkills
                                 //               join c in db.Skills on b.SkillId equals c.Id
                                 //               where b.PracticeId.Equals(a.Id)
                                 //               select c.Id).ToList(),
                             }).AsQueryable();

            if (modelSearch.SkillId != null && modelSearch.SkillId.Count() > 0)
            {
                dataQuery = dataQuery.Where(u => modelSearch.SkillId.Contains(u.SkillId));
                ListSkillId = ListSkillId.Where(t => modelSearch.SkillId.Contains(t.SkillId)).ToList();
            }
            // tìm kiếm theo mã nhóm    
            if (!string.IsNullOrEmpty(modelSearch.PracticeGroupId))
            {
                dataQuery = dataQuery.Where(t => modelSearch.PracticeGroupId.Equals(t.PracticeGroupId));
            }

            if (modelSearch.PracaticeId != null && modelSearch.PracaticeId.Count() > 0)
            {
                dataQuery = dataQuery.Where(u => modelSearch.PracaticeId.Contains(u.Id));
            }

            List<PracticeModel> listRs = dataQuery.GroupBy(t => new
            {
                t.Id,
                t.Name,
                t.PracticeGroupId,
                t.Code,
                t.CurentVersion,
                t.TrainingTime,
                t.Note,
                t.TotalPrice,
                t.PracticeFile,
                t.PracticeExist,
                t.MaterialConsumableExist,
                t.MaterialConsumable,
                t.SupMaterial,
                t.SupMaterialExist
            })
                                    .Select(s => new PracticeModel
                                    {
                                        Code = s.Key.Code,
                                        Name = s.Key.Name,
                                        CurentVersion = s.Key.CurentVersion,
                                        PracticeGroupId = s.Key.PracticeGroupId,
                                        TotalPrice = s.Key.TotalPrice,
                                        Id = s.Key.Id,
                                        Note = s.Key.Note,
                                        TrainingTime = s.Key.TrainingTime,
                                        PracticeFile = s.Key.PracticeFile,
                                        PracticeExist = s.Key.PracticeExist,
                                        MaterialConsumableExist = s.Key.MaterialConsumableExist,
                                        MaterialConsumable = s.Key.MaterialConsumable,
                                        SupMaterial = s.Key.SupMaterial,
                                        SupMaterialExist = s.Key.SupMaterialExist,
                                    }).ToList();

            foreach (var item in listRs)
            {
                item.ListSkillId = ListPracticeSkillId.Where(b => b.PracticeId.Equals(item.Id)).Select(c => c.SkillId).ToList();
                item.HardwarePrice = GetHardwarePrice(item.Id);

                item.TotalPrice = item.HardwarePrice + item.LessonPrice * item.Quantity;
            }

            List<string> listSkillName = new List<string>();
            List<string> listSkillId = new List<string>();
            foreach (var item in ListSkillId)
            {
                if (!listSkillName.Contains(item.SkillName))
                {
                    listSkillName.Add(item.SkillName);
                }

                if (!listSkillId.Contains(item.SkillId))
                {
                    listSkillId.Add(item.SkillId);
                }
            }

            foreach (var item in listRs)
            {
                var lst = ListPracticeSkillId.Where(t => t.PracticeId.Equals(item.Id)).ToList();
                if (lst.Count > 0)
                {
                    var lstId = lst.Select(t => t.SkillId).ToList();
                    foreach (var ite in listSkillId)
                    {
                        if (lstId.Contains(ite))
                        {
                            item.ListCheckSkill.Add("X");
                        }
                        else
                        {
                            item.ListCheckSkill.Add("");
                        }
                    }
                }
                else
                {
                    foreach (var ite in listSkillId)
                    {
                        item.ListCheckSkill.Add("");
                    }
                }
            }

            if (listRs.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                List<ExportPracticeModel> listResult = new List<ExportPracticeModel>();
                var index = 1;
                foreach (var item in listRs)
                {
                    ExportPracticeModel rs = new ExportPracticeModel();
                    rs.Index = index.ToString();
                    rs.Name = item.Name;
                    rs.Code = item.Code;
                    rs.CurentVersion = Convert.ToInt32(item.CurentVersion);
                    rs.PracticeFile = (item.PracticeFile.HasValue && item.PracticeFile.Value ? "Có" : "Không") + " - " + (item.PracticeExist.HasValue && item.PracticeExist.Value ? "Đủ" : "Chưa đủ");
                    rs.SupMaterial = (item.SupMaterial.HasValue && item.SupMaterial.Value ? "Có" : "Không") + " - " + (item.SupMaterialExist.HasValue && item.SupMaterialExist.Value ? "Đủ" : "Chưa đủ");
                    rs.MaterialConsumable = (item.MaterialConsumable.HasValue && item.MaterialConsumable.Value ? "Có" : "Không") + " - " + (item.MaterialConsumableExist.HasValue && item.MaterialConsumableExist.Value ? "Đủ" : "Chưa đủ");
                    rs.TrainingTime = item.TrainingTime;
                    rs.Note = item.Note;
                    rs.TotalPrice = item.TotalPrice;
                    listResult.Add(rs);
                    index++;
                }

                DataTable dataTable = ConvertData.ToDataTable<ExportPracticeModel>(listResult);
                var count = 0;
                foreach (var item in listSkillName)
                {
                    DataColumn col = dataTable.Columns.Add(item, typeof(string));
                    for (var i = 0; i < listRs.Count; i++)
                    {
                        dataTable.Rows[i][item] = listRs[i].ListCheckSkill[count];
                    }
                    count++;
                }

                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Practice.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                IStyle style = workbook.Styles.Add("NewStyle");
                style.Color = Color.ForestGreen;
                style.Font.Bold = true;
                style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                for (int i = 1, j = 0; i <= listSkillName.Count; i++, j++)
                {
                    sheet[2, 10 + i].Text = listSkillName[j];
                }

                sheet[2, 1, 2, 10 + listSkillName.Count].CellStyle = style;
                sheet[2, 1, 2, 10 + listSkillName.Count].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet[2, 1, 2, 10 + listSkillName.Count].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

                var total = listRs.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listRs.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.Code,
                    a.CurentVersion,
                    a.TrainingTime,
                    a.TotalPrice,
                    a.Note
                });

                if (listExport.Count() > 0)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count(), ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportDataTable(dataTable, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 0, 10 + listSkillName.Count].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 0, 10 + listSkillName.Count].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 0, 10 + listSkillName.Count].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 0, 10 + listSkillName.Count].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 0, 10 + listSkillName.Count].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 0, 10 + listSkillName.Count].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách bài thực hành" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách bài thực hành" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }
        private void CheckExistedForAdd(PracticeModel model)
        {
            if (db.Practices.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Practice);
            }

            if (db.Practices.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Practice);
            }

            if (IsNumber(model.CurentVersion) == false)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0009);
            }
        }
        public void CheckExistedForUpdate(PracticeModel model)
        {
            var query = (from a in db.PracticeOldVersions.AsNoTracking()
                         join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                         where a.PracticeId.Equals(model.Id)
                         select new PracticeOldVersionModel
                         {
                             Id = a.Id,
                             PracticeId = a.PracticeId,
                             Content = a.Content,
                             Version = a.Version,
                             Description = a.Description
                         }).ToList();

            if (db.Practices.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Practice);
            }

            if (db.Practices.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Practice);
            }

            if (Convert.ToInt32(model.CurentVersion) < 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0010);
            }
        }

        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public decimal GetHardwarePrice(string practiceId)
        {
            var listModuleInPractice = (from h in db.ModuleInPractices.AsNoTracking()
                                        where h.PracticeId.Equals(practiceId)
                                        select new
                                        {
                                            h.ModuleId,
                                            h.Qty
                                        }).ToList();

            decimal moduelAmout = 0;
            ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
            foreach (var module in listModuleInPractice)
            {
                moduelAmout += moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0) * module.Qty;
            }

            var practiceMaterialConsumable = (from p in db.PracticeMaterialConsumables.AsNoTracking()
                                              join m in db.Materials.AsNoTracking() on p.MaterialId equals m.Id
                                              where p.PracticeId.Equals(practiceId)
                                              select new
                                              {
                                                  p.Quantity,
                                                  m.PriceHistory,
                                                  m.Pricing,
                                                  m.LastBuyDate,
                                                  m.InputPriceDate
                                              }).ToList();

            decimal price = 0;
            decimal materialConsumableAmount = 0;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in practiceMaterialConsumable)
            {
                price = item.Pricing;

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        price = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        price = 0;
                    }
                }

                materialConsumableAmount += price * item.Quantity;
            }

            var practiceSubMaterial = (from p in db.PracticeSubMaterials.AsNoTracking()
                                       join m in db.Materials.AsNoTracking() on p.MaterialId equals m.Id
                                       where p.PracticeId.Equals(practiceId)
                                       select new
                                       {
                                           p.Quantity,
                                           m.PriceHistory,
                                           m.Pricing,
                                           m.LastBuyDate,
                                           m.InputPriceDate
                                       }).ToList();

            var practiceSubMaterialModule = (from p in db.PracticeSubMaterials.AsNoTracking()
                                             join m in db.Modules.AsNoTracking() on p.MaterialId equals m.Id
                                             where p.PracticeId.Equals(practiceId)
                                             select new
                                             {
                                                 m.Id,
                                                 p.Quantity,
                                                 m.Pricing,
                                             }).ToList();

            decimal subMaterialAmount = 0;
            foreach (var item in practiceSubMaterial)
            {
                price = item.Pricing;

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        price = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        price = 0;
                    }
                }

                subMaterialAmount += price * item.Quantity;
            }

            foreach (var item in practiceSubMaterialModule)
            {

                price = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
                subMaterialAmount += price * item.Quantity;
            }

            return moduelAmout + materialConsumableAmount + subMaterialAmount;
        }

        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var practiceGroups = db.PracticeGroups.AsNoTracking().Where(i => i.ParentId.Equals(id)).Select(i => i.Id).ToList();
            listChild.AddRange(practiceGroups);
            if (practiceGroups.Count > 0)
            {
                foreach (var item in practiceGroups)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }

        public List<ModuleModel> ImportModuleProductSketches(string userId, HttpPostedFile file, string productId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string moduleCode, qty;

            #region
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<ModuleModel> listImports = new List<ModuleModel>();
            ModuleModel item;
            var listModule = db.Modules.AsNoTracking().ToList();
            List<int> rowModuleCode = new List<int>();
            List<int> rowCheckModuleCode = new List<int>();
            List<int> rowQty = new List<int>();
            List<int> rowCheckQty = new List<int>();
            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    item = new ModuleModel();
                    moduleCode = sheet[i, 2].Value;
                    qty = sheet[i, 3].Value;

                    //Code
                    if (!string.IsNullOrEmpty(moduleCode))
                    {
                        var data = listModule.FirstOrDefault(a => a.Code.ToUpper().Equals(moduleCode.ToUpper()));
                        if (data != null)
                        {
                            item.Id = data.Id;
                            item.Name = data.Name;
                            item.Code = data.Code;
                            item.ModuleGroupCode = db.ModuleGroups.Where(a => a.Id.Equals(data.ModuleGroupId)).Select(s => s.Code).FirstOrDefault();
                            item.LeadTime = data.Leadtime;
                            item.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(data.Id, 0);
                        }
                        else
                        {
                            rowCheckModuleCode.Add(i);
                        }
                    }
                    else
                    {
                        rowModuleCode.Add(i);
                    }

                    // Qty
                    try
                    {
                        if (!string.IsNullOrEmpty(qty))
                        {
                            item.Quantity = Convert.ToDecimal(qty);
                        }
                        else
                        {
                            rowQty.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckQty.Add(i);
                        continue;
                    }
                    listImports.Add(item);
                }

                #endregion

                if (rowModuleCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã Module dòng <" + string.Join(", ", rowModuleCode) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckModuleCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã Module dòng <" + string.Join(", ", rowCheckModuleCode) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowQty.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQty) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckQty.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQty) + "> không đúng định dạng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }
            }
            catch (Exception ex)
            {
                //fs.Close();
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }

            //fs.Close();
            workbook.Close();
            excelEngine.Dispose();
            return listImports;
        }

        public string GetContentPractice(string practiceId)
        {
            string content = string.Empty;
            var data = db.Practices.AsNoTracking().FirstOrDefault(i => i.Id.Equals(practiceId));
            if (data != null)
            {
                content = data.EditContent;
            }
            else
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Practice);
            }
            return content;
        }

        public void UpdateContent(PracticeContentModel model)
        {
            var practice = db.Practices.FirstOrDefault(i => i.Id.Equals(model.PracticeId));
            if (practice == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Practice);
            }
            practice.EditContent = model.Content;
            db.SaveChanges();
        }
    }
}
