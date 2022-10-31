using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DashboardEmployee;
using NTS.Model.Practice;
using NTS.Model.PracticeSkill;
using NTS.Model.PracticeSkillHistory;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using NTS.Model.SkillAttach;
using NTS.Model.Skills;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Skills
{
    public class SkillsBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SkillsModel> SearchSkills(SkillsSearchModel modelSearch)
        {
            SearchResultModel<SkillsModel> searchResult = new SearchResultModel<SkillsModel>();
            List<string> listParentId = new List<string>();

            var dataQuey = (from a in db.Skills.AsNoTracking()
                            join b in db.SkillGroups.AsNoTracking() on a.SkillGroupId equals b.Id
                            join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
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
            if (!string.IsNullOrEmpty(modelSearch.SkillGroupId))
            {
                //dataQuey = dataQuey.Where(r => r.SkillGroupId.ToUpper().Contains(modelSearch.SkillGroupId.ToUpper()));
                var skillGroup = db.SkillGroups.AsNoTracking().FirstOrDefault(i => i.Id.Equals(modelSearch.SkillGroupId));
                if (skillGroup != null)
                {
                    listParentId.Add(skillGroup.Id);
                }
                listParentId.AddRange(GetListParent(modelSearch.SkillGroupId));
                var listSkillGroup = listParentId.AsQueryable();
                dataQuey = (from a in dataQuey
                             join b in listSkillGroup.AsQueryable() on a.SkillGroupId equals b
                             select new SkillsModel
                             {
                                 Id = a.Id,
                                 SkillGroupId = a.SkillGroupId,
                                 SkillGroupName = a.SkillGroupName,
                                 DegreeId = a.DegreeId,
                                 DegreeName = a.DegreeName,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Description = a.Description,
                                 Note = a.Note,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();
            }

            searchResult.TotalItem = dataQuey.Count();
            var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<PracticeModel> SearchPractice(PracticeSearchModel modelSearch)
        {
            SearchResultModel<PracticeModel> searchResult = new SearchResultModel<PracticeModel>();
            var dataQuery = (from a in db.Practices.AsNoTracking()
                             join b in db.PracticeGroups.AsNoTracking() on a.PracticeGroupId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new PracticeModel
                             {
                                 Id = a.Id,
                                 PracticeGroupId = a.PracticeGroupId,
                                 PracticeGroupName = b.Name,
                                 PracticeGroupCode = b.Code,
                                 Code = a.Code,
                                 Name = a.Name,
                                 CurentVersion = a.CurentVersion.ToString(),
                                 Note = a.Note,
                                 TrainingTime = a.TrainingTime,
                                 ListModuleInPractice = (from m in db.ModuleInPractices.AsNoTracking()
                                                         join t in db.Modules.AsNoTracking() on m.ModuleId equals t.Id
                                                         where m.PracticeId.Equals(a.Id)
                                                         orderby m.Qty
                                                         select new ModuleInPracticeModel
                                                         {
                                                             Id = m.Id,
                                                             ModuleId = m.ModuleId,
                                                             PracticeId = m.PracticeId,
                                                             Qty = m.Qty,
                                                             Version = m.Version,
                                                             ModuleName = t.Name,
                                                             Specification = t.Specification,
                                                             Note = t.Note,
                                                             Code = t.Code,
                                                         }).ToList(),
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Id))
            {
                dataQuery = dataQuery.Where(u => u.Id.Equals(modelSearch.Id));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public PracticeSkillModel GetPracticeSkillInfo(PracticeSkillModel model)
        {
            var module = db.PracticeSkills.FirstOrDefault(o => o.SkillId.Equals(model.SkillId));
            try
            {
                model.Id = module.Id;
                model.SkillId = module.SkillId;
                model.PracticeId = module.PracticeId;
                //Lấy List version
                var listPractice = (from a in db.PracticeSkills.AsNoTracking()
                                    join b in db.Skills.AsNoTracking() on a.SkillId equals b.Id
                                    join c in db.Practices.AsNoTracking() on a.PracticeId equals c.Id
                                    where a.SkillId.Equals(model.SkillId)
                                    select new PracticeModel
                                    {
                                        Id = c.Id,
                                        Code = c.Code,
                                        Name = c.Name,
                                    }).ToList();
                model.ListPractice = listPractice;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool AddSkills(SkillsModel model)
        {
            bool rs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Skill newSkill = new Skill
                    {
                        Id = Guid.NewGuid().ToString(),
                        DegreeId = model.DegreeId,
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                        Note = model.Note.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    if (!string.IsNullOrEmpty(model.SkillGroupId))
                    {
                        newSkill.SkillGroupId = model.SkillGroupId;
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newSkill.Code, newSkill.Id, Constants.LOG_Practice_Skill);


                    db.Skills.Add(newSkill);

                    //Add file
                    if (model.ListFile.Count > 0)
                    {
                        List<SkillAttach> listFileEntity = new List<SkillAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                SkillAttach fileEntity = new SkillAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SkillId = newSkill.Id,
                                    Path = item.Path,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    CreateBy = newSkill.CreateBy,
                                    CreateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.SkillAttaches.AddRange(listFileEntity);
                    }
                    //Add PracticeSkill
                    foreach (var item in model.ListData)
                    {
                        PracticeSkill newPracticeSkill = new PracticeSkill
                        {
                            Id = Guid.NewGuid().ToString(),
                            SkillId = newSkill.Id,
                            PracticeId = item.Id,
                        };
                        db.PracticeSkills.Add(newPracticeSkill);
                    }
                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }
            return rs;
        }
        public void AddPracticeSkill(PracticeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model.listSelect)
                    {
                        PracticeSkill newPracticeSkill = new PracticeSkill
                        {
                            Id = Guid.NewGuid().ToString(),
                            SkillId = item.SkillId,
                            PracticeId = item.PracticeId,
                        };
                        db.PracticeSkills.Add(newPracticeSkill);
                    }
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

        public bool UpdateSkills(SkillsModel model)
        {
            bool rs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSkill = db.Skills.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<PracticeSkillHistoryModel>(newSkill);

                    newSkill.DegreeId = model.DegreeId;
                    newSkill.Name = model.Name.NTSTrim();
                    newSkill.Code = model.Code.NTSTrim();
                    newSkill.Note = model.Note.NTSTrim();
                    newSkill.Description = model.Description.NTSTrim();
                    newSkill.UpdateBy = model.UpdateBy;
                    newSkill.UpdateDate = DateTime.Now;
                    newSkill.SkillGroupId = null;




                    if (!string.IsNullOrEmpty(model.SkillGroupId))
                    {
                        newSkill.SkillGroupId = model.SkillGroupId;
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<PracticeSkillHistoryModel>(newSkill);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Practice_Skill, newSkill.Id, newSkill.Code, jsonBefor, jsonApter);


                    var fileEntities = db.SkillAttaches.Where(t => t.SkillId.Equals(model.Id));
                    db.SkillAttaches.RemoveRange(fileEntities);
                    if (model.ListFile.Count > 0)
                    {
                        List<SkillAttach> listFileEntity = new List<SkillAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null)
                            {
                                SkillAttach fileEntity = new SkillAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SkillId = newSkill.Id,
                                    FileName = item.FileName,
                                    Path = item.Path,
                                    FileSize = item.FileSize,
                                    CreateBy = newSkill.CreateBy,
                                    CreateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.SkillAttaches.AddRange(listFileEntity);
                    }
                    var oldTestCriteria = db.PracticeSkills.Where(a => a.SkillId.Equals(model.Id));
                    if (oldTestCriteria != null)
                    {
                        db.PracticeSkills.RemoveRange(oldTestCriteria);

                        if (model.ListData != null)
                        {
                            foreach (var item in model.ListData)
                            {
                                PracticeSkill newPracticeSkill = new PracticeSkill
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SkillId = model.Id,
                                    PracticeId = item.Id,
                                };

                                db.PracticeSkills.Add(newPracticeSkill);
                            }

                        }
                    }
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

        public void DeleteSkill(SkillsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var skill = db.EmployeeSkillDetails.AsNoTracking().Where(m => m.SkillId.Equals(model.Id)).FirstOrDefault();
                if (skill != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Skills);
                }

                var classSkill = db.ClassSkills.AsNoTracking().Where(m => m.SkillId.Equals(model.Id)).FirstOrDefault();
                if (classSkill != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Skills);
                }
                try
                {
                    var skills = db.Skills.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (skills == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Skills);
                    }
                    var pratice = db.PracticeSkills.Where(u => u.SkillId.Equals(model.Id)).ToList();
                    if (pratice.Count > 0)
                    {
                        db.PracticeSkills.RemoveRange(pratice);
                    }
                    var file = db.SkillAttaches.Where(u => u.SkillId.Equals(model.Id)).ToList();
                    if (file != null)
                    {
                        db.SkillAttaches.RemoveRange(file);
                    }

                    var NameOrCode = skills.Name;


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<PracticeSkillHistoryModel>(skills);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Practice_Skill, skills.Id, NameOrCode, jsonBefor);

                    db.Skills.Remove(skills);
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

        public void DeletePracticeSkill(PracticeSkillModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var practiceSkill = db.PracticeSkills.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (practiceSkill == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.PracticeSkill);
                    }
                    db.PracticeSkills.Remove(practiceSkill);
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

        public object GetSkillInfo(SkillsModel model)
        {
            var listpracticeskill = (from a in db.PracticeSkills.AsNoTracking()
                                     join b in db.Skills.AsNoTracking() on a.SkillId equals b.Id
                                     join c in db.Practices.AsNoTracking() on a.PracticeId equals c.Id
                                     where a.SkillId.Equals(model.Id)
                                     select new PracticeModel
                                     {
                                         Id = c.Id,
                                         Name = c.Name,
                                         Code = c.Code
                                     }).ToList();
            try
            {
                var resultInfo = db.Skills.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SkillsModel
                {
                    Id = p.Id,
                    SkillGroupId = p.SkillGroupId,
                    DegreeId = p.DegreeId,
                    Code = p.Code,
                    Name = p.Name,
                    Description = p.Description,
                    Note = p.Note,
                    
                }).FirstOrDefault();
                if (resultInfo != null)
                {
                    resultInfo.ListFile = db.SkillAttaches.AsNoTracking().Where(t => t.SkillId.Equals(resultInfo.Id)).Select(m => new SkillAttachModel
                    {
                        Id = m.Id,
                        SkillId = m.SkillId,
                        FileName = m.FileName,
                        Path = m.Path,
                        Note = m.Note,
                        FileSize = m.FileSize.Value
                    }).ToList();
                }
                if (resultInfo == null)
                {
                    throw new Exception("Nhóm kỹ năng này đã bị xóa bởi người dùng khác");
                }
                resultInfo.ListData = listpracticeskill;
                return resultInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
            }
        }

        public string ExportExcel(SkillsModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.Skills.AsNoTracking()
                             join b in db.SkillGroups.AsNoTracking() on a.SkillGroupId equals b.Id
                             join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
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
                                 Note = a.Note,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(model.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(model.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.SkillGroupId))
            {
                dataQuery = dataQuery.Where(r => r.SkillGroupId.ToUpper().Contains(model.SkillGroupId.ToUpper()));
            }
            List<SkillsModel> listModel = dataQuery.ToList();

            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Skills.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.Code,
                    a.SkillGroupName,
                    a.DegreeName,
                    a.Note,
                });
                if (listExport.Count() > 0)
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách kỹ năng" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách kỹ năng" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }
        private void CheckExistedForAdd(SkillsModel model)
        {
            if (db.Skills.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Skills);
            }

            if (db.Skills.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Skills);
            }
        }

        public void CheckExistedForUpdate(SkillsModel model)
        {
            if (db.Skills.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Skills);
            }

            if (db.Skills.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Skills);
            }
        }

        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var skillGroup = db.SkillGroups.AsNoTracking().Where(i => i.ParentId.Equals(id)).Select(i => i.Id).ToList();
            listChild.AddRange(skillGroup);
            if (skillGroup.Count > 0)
            {
                foreach (var item in skillGroup)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }
    }
}
