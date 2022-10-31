using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Course;
using NTS.Model.Document;
using NTS.Model.HistoryVersion;
using NTS.Model.Materials;
using NTS.Model.OutputResult;
using NTS.Model.Repositories;
using NTS.Model.Skills;
using NTS.Model.Task;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorldSkill;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.TaskFlowStage
{
    public class TaskFlowStageBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm công việc
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<TaskFlowStageSearchResultModel> SearchTask(TaskFlowStageSearchModel searchModel, bool isExport)
        {
            SearchResultModel<TaskFlowStageSearchResultModel> searchResult = new SearchResultModel<TaskFlowStageSearchResultModel>();
            var dataQuery = (from a in db.Tasks.AsNoTracking()
                             join b in db.FlowStages.AsNoTracking() on a.FlowStageId equals b.Id into ab
                             from abv in ab.DefaultIfEmpty()
                             select new TaskFlowStageSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 IsDesignModule = a.IsDesignModule.HasValue ? a.IsDesignModule.Value : false,
                                 FlowStageId = a.FlowStageId,
                                 FlowStageName = abv != null ? abv.Name : "",
                                 Type = a.Type,
                                 CreateDate = a.CreateDate,
                                 SBUId = a.SBUId,
                                 DepartmentId = a.DepartmentId,
                                 IsProjectWork = a.IsProjectWork,
                                 TaskInputResult = db.TaskInputRessults.Where(r => r.TaskId.Equals(a.Id)).Select(r => r.OutputRessultId).ToList(),
                                 TaskOutPutResult = db.TaskOutputRessults.Where(r => r.TaskId.Equals(a.Id)).Select(r => r.OutputRessultId).ToList(),
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.FlowStageId))
            {
                dataQuery = dataQuery.Where(u => u.FlowStageId.Equals(searchModel.FlowStageId));
            }

            if (searchModel.IsDesignModule.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.IsDesignModule == searchModel.IsDesignModule.Value);
            }

            if (searchModel.IsProjectWork.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.IsProjectWork == searchModel.IsProjectWork.Value);
            }

            if (searchModel.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == searchModel.Type.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(searchModel.DepartmentId));
            }
            if (!string.IsNullOrEmpty(searchModel.CodeOutput))
            {
                List<string> outputResultId = db.OutputRessults.Where(r => r.Name.ToUpper().Contains(searchModel.CodeOutput.ToUpper())).Select(r => r.Id).ToList();
                dataQuery = dataQuery.Where(u => u.TaskOutPutResult.Select(x => x).Intersect(outputResultId).Any());
            }

            searchResult.TotalItem = dataQuery.Count();
            if (isExport)
            {
                searchResult.ListResult = dataQuery.ToList();
            }
            else
            {
                var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
                searchResult.ListResult = listResult;
            }

            var outputResult = db.OutputRessults.AsNoTracking();
            //get RASCI
            var taskWorkTypes = db.TaskWorkTypes.AsNoTracking();
            var workTypes = db.WorkTypes.ToList();
            var deparments = db.Departments.ToList();
            var sbus = db.SBUs.ToList();

            foreach (var item in searchResult.ListResult)
            {
                item.CodeInput = String.Join("; ", (from a in outputResult
                                                    where item.TaskInputResult.Contains(a.Id)
                                                    select a.Name).ToArray());

                item.CodeOutput = String.Join("; ", (from a in outputResult
                                                     where item.TaskOutPutResult.Contains(a.Id)
                                                     select a.Name).ToArray());

                var taskWorkType = taskWorkTypes.Where(t => t.TaskId.Equals(item.Id)).ToList();
                RASCIEntity rASCIEntity = new RASCIEntity();
                foreach (var twt in taskWorkType)
                {
                    rASCIEntity = new RASCIEntity();
                    rASCIEntity.R = workTypes.Where(w => w.Id.Equals(twt.WorkTypeRId)).FirstOrDefault() != null ? workTypes.Where(w => w.Id.Equals(twt.WorkTypeRId)).FirstOrDefault().Name : " ";
                    rASCIEntity.A = workTypes.Where(w => w.Id.Equals(twt.WorkTypeAId)).FirstOrDefault() != null ? workTypes.Where(w => w.Id.Equals(twt.WorkTypeAId)).FirstOrDefault().Name : " ";
                    rASCIEntity.S = workTypes.Where(w => w.Id.Equals(twt.WorkTypeSId)).FirstOrDefault() != null ? workTypes.Where(w => w.Id.Equals(twt.WorkTypeSId)).FirstOrDefault().Name : " ";
                    rASCIEntity.C = workTypes.Where(w => w.Id.Equals(twt.WorkTypeCId)).FirstOrDefault() != null ? workTypes.Where(w => w.Id.Equals(twt.WorkTypeCId)).FirstOrDefault().Name : " ";
                    rASCIEntity.I = workTypes.Where(w => w.Id.Equals(twt.WorlTypeIId)).FirstOrDefault() != null ? workTypes.Where(w => w.Id.Equals(twt.WorlTypeIId)).FirstOrDefault().Name : " ";
                    rASCIEntity.Department = deparments.Where(w => w.Id.Equals(twt.DepartmentId)).FirstOrDefault() != null ? deparments.Where(w => w.Id.Equals(twt.DepartmentId)).FirstOrDefault().Name : " ";
                    rASCIEntity.SBU = sbus.Where(w => w.Id.Equals(twt.SBUId)).FirstOrDefault() != null ? sbus.Where(w => w.Id.Equals(twt.SBUId)).FirstOrDefault().Name : " ";
                    item.RASCI.Add(rASCIEntity);
                }
            }

            return searchResult;
        }

        public object ExportExcel(TaskFlowStageSearchModel model)
        {
            var data = SearchTask(model, true);
            List<TaskFlowStageSearchResultModel> tasks = data.ListResult;

            if (tasks.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TaskFlowStage.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = tasks.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = tasks.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.FlowStageName,
                    a.WorkTypeR,
                    a.WorkTypeA,
                    a.WorkTypeS,
                    a.WorkTypeC,
                    a.WorkTypeI,
                    a.SBUName,
                    a.DepartmentName,
                    IsProjectWork = a.IsProjectWork == true ? "Có" : "Không",
                    a.CodeInput,
                    a.CodeOutput
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách công việc" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách công việc" + ".xlsx";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Xóa công việc
        /// </summary>
        /// <param name="model"></param>
        public void DeleteTask(TaskFlowStageModel model)
        {
            var taskExist = db.Tasks.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (taskExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Task);
            }

            var plan = db.Plans.AsNoTracking().Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
            if (plan != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Task);
            }

            var taskModule = db.TaskModuleGroups.AsNoTracking().Where(m => m.TaskId.Equals(model.Id)).FirstOrDefault();
            if (taskModule != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Task);
            }
            try
            {
                var skills = db.TaskSkills.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskSkills.RemoveRange(skills);

                var documents = db.DocumentObjects.Where(a => a.ObjectId.Equals(model.Id)).ToList();
                db.DocumentObjects.RemoveRange(documents);

                var materials = db.TaskMaterils.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskMaterils.RemoveRange(materials);

                var taskOutputRessults = db.TaskOutputRessults.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskOutputRessults.RemoveRange(taskOutputRessults);

                var taskInputRessults = db.TaskInputRessults.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskInputRessults.RemoveRange(taskInputRessults);

                var taskWorktypes = db.TaskWorkTypes.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskWorkTypes.RemoveRange(taskWorktypes);

                var NameOrCode = taskExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<TaskFlowStageHistoryModel>(taskExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Task, taskExist.Id, NameOrCode, jsonBefor);
                db.Tasks.Remove(taskExist);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CreateTask(TaskFlowStageModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var taskNameExist = db.Tasks.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (taskNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Task);
            }

            var taskCodeExist = db.Tasks.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (taskCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Task);
            }

            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    NTS.Model.Repositories.Task newTask = new NTS.Model.Repositories.Task
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                        IsDesignModule = model.IsDesignModule,
                        Type = model.Type,
                        Description = model.Description.NTSTrim(),
                        TimeStandard = model.TimeStandard,
                        DegreeId = model.DegreeId,
                        Specialization = model.Specialization,
                        SpecializeId = model.SpecializeId,
                        WorkTypeRId = model.WorkTypeRId,
                        WorkTypeAId = model.WorkTypeAId,
                        WorkTypeSId = model.WorkTypeSId,
                        WorkTypeCId = model.WorkTypeCId,
                        WorkTypeIId = model.WorkTypeIId,
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmentId,
                        FlowStageId = model.FlowStageId,
                        PercentValue = model.PercentValue,
                        CreateBy = model.CreateBy,
                        UpdateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        IsProjectWork = model.IsProjectWork,
                    };
                    db.Tasks.Add(newTask);
                    int i = 0;
                    foreach (var list in model.TaskWorkTypes)
                    {
                        i++;
                        TaskWorkType taskWorkType = new TaskWorkType
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaskId = newTask.Id,
                            WorkTypeRId = list.WorkTypeRId,
                            WorkTypeAId = list.WorkTypeAId,
                            WorkTypeSId = list.WorkTypeSId,
                            WorkTypeCId = list.WorkTypeCId,
                            WorlTypeIId = list.WorkTypeIId,
                            DepartmentId = list.DepartmentId,
                            SBUId = list.SBUId,
                            Index = i
                        };
                        db.TaskWorkTypes.Add(taskWorkType);
                    }

                    //Thêm kỹ năng
                    foreach (var item in model.Skills)
                    {
                        TaskSkill taskSkill = new TaskSkill()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaskId = newTask.Id,
                            SkillId = item.Id,
                        };
                        db.TaskSkills.Add(taskSkill);

                    }

                    //Thêm tài liệu
                    foreach (var item in model.Documents)
                    {
                        DocumentObject taskDocument = new DocumentObject()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ObjectId = newTask.Id,
                            DocumentId = item.Id,
                            ObjectType = Constants.ObjectType_Work,
                        };
                        db.DocumentObjects.Add(taskDocument);
                    }

                    //Thêm công cụ
                    foreach (var item in model.Materials)
                    {
                        TaskMateril taskMateril = new TaskMateril()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaskId = newTask.Id,
                            MaterialId = item.Id,
                            Quantity = item.Quantity
                        };
                        db.TaskMaterils.Add(taskMateril);
                    }

                    //Thêm danh sách kế quả đầu ra
                    foreach (var item in model.OutputResults)
                    {
                        TaskOutputRessult taskOutputRessult = new TaskOutputRessult()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaskId = newTask.Id,
                            OutputRessultId = item.Id
                        };
                        db.TaskOutputRessults.Add(taskOutputRessult);
                    }

                    //Thêm danh sách kế quả đầu vào
                    foreach (var item in model.InputResults)
                    {
                        TaskInputRessult taskInputRessult = new TaskInputRessult()
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaskId = newTask.Id,
                            OutputRessultId = item.Id
                        };
                        db.TaskInputRessults.Add(taskInputRessult);
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newTask.Name, newTask.Id, Constants.LOG_Task);


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
        /// Lấy thông tin công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TaskFlowStageModel GetFlowStageInfo(TaskFlowStageModel model)
        {
            var resultInfo = db.Tasks.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new TaskFlowStageModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                IsDesignModule = p.IsDesignModule,
                Type = p.Type,
                Description = p.Description,
                TimeStandard = p.TimeStandard,
                DegreeId = p.DegreeId,
                Specialization = p.Specialization,
                SpecializeId = p.SpecializeId,
                //WorkTypeRId = p.WorkTypeRId,
                //WorkTypeAId = p.WorkTypeAId,
                //WorkTypeSId = p.WorkTypeSId,
                //WorkTypeCId = p.WorkTypeCId,
                //WorkTypeIId = p.WorkTypeIId,
                //SBUId = p.SBUId,
                //DepartmentId = p.DepartmentId,
                FlowStageId = p.FlowStageId,
                PercentValue = p.PercentValue,
                IsProjectWork = p.IsProjectWork,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FlowStage);
            }

            //Lấy thông tin kỹ năng
            resultInfo.Skills = (from a in db.TaskSkills.AsNoTracking()
                                 join b in db.WorkSkills.AsNoTracking() on a.SkillId equals b.Id
                                 join c in db.WorkSkillGroups.AsNoTracking() on b.WorkSkillGroupId equals c.Id
                                 where a.TaskId.Equals(model.Id)
                                 select new WorkSkillModel
                                 {
                                     Id = b.Id,
                                     Name = b.Name,
                                     WorkSkillGroupName = c.Name,
                                     Description = b.Description
                                 }).ToList();

            //Lấy danh sách tài liệu
            resultInfo.Documents = (from a in db.DocumentObjects.AsNoTracking()
                                    join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                    join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                    join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                                    where a.ObjectId.Equals(model.Id)
                                    select new DocumentSearchResultModel
                                    {
                                        Id = b.Id,
                                        Name = b.Name,
                                        Code = b.Code,
                                        DocumentGroupId = b.DocumentGroupId,
                                        DocumentGroupName = c.Name,
                                        DocumentTypeName = d.Name
                                    }).ToList();

            foreach (var item in resultInfo.Documents)
            {
                item.ListFile = (from a in db.DocumentFiles.AsNoTracking()
                                 where a.DocumentId.Equals(item.Id)
                                 select new DocumentFileModel
                                 {
                                     Id = a.Id,
                                     FileName = a.FileName,
                                     FileSize = a.FileSize,
                                     Path = a.Path
                                 }).ToList();
            }

            //Lấy danh sách công cụ
            resultInfo.Materials = (from a in db.TaskMaterils.AsNoTracking()
                                    join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                    join c in db.MaterialGroups.AsNoTracking() on b.MaterialGroupId equals c.Id
                                    join d in db.Manufactures.AsNoTracking() on b.ManufactureId equals d.Id
                                    where a.TaskId.Equals(model.Id)
                                    select new MaterialResultModel
                                    {
                                        Id = b.Id,
                                        Name = b.Name,
                                        Code = b.Code,
                                        MaterialGroupName = c.Name,
                                        ManufactureCode = d.Name,
                                        Pricing = b.Pricing,
                                        Quantity = a.Quantity
                                    }).ToList();

            //Lấy danh sách đầu vào
            resultInfo.InputResults = (from a in db.TaskInputRessults.AsNoTracking()
                                       join b in db.OutputRessults.AsNoTracking() on a.OutputRessultId equals b.Id
                                       where a.TaskId.Equals(model.Id)
                                       join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                       join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
                                       select new OutputResultModel
                                       {
                                           Id = b.Id,
                                           Name = b.Name,
                                           Code = b.Code,
                                           SBUId = b.SBUId,
                                           DepartmentId = b.DepartmentId,
                                           SBUName = c.Name,
                                           DepartmentName = d.Name
                                       }).ToList();

            //Lấy danh sách đầu ra
            resultInfo.OutputResults = (from a in db.TaskOutputRessults.AsNoTracking()
                                        join b in db.OutputRessults.AsNoTracking() on a.OutputRessultId equals b.Id
                                        where a.TaskId.Equals(model.Id)
                                        join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                        join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
                                        select new OutputResultModel
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Code = b.Code,
                                            SBUId = b.SBUId,
                                            DepartmentId = b.DepartmentId,
                                            SBUName = c.Name,
                                            DepartmentName = d.Name
                                        }).ToList();
            resultInfo.TaskWorkTypes = (from a in db.TaskWorkTypes.AsNoTracking()
                                        where resultInfo.Id.Equals(a.TaskId)
                                        orderby a.Index ascending
                                        select new TaskWorkTypeModel
                                        {
                                            Id = a.Id,
                                            TaskId = a.TaskId,
                                            DepartmentId = a.DepartmentId,
                                            SBUId = a.SBUId,
                                            WorkTypeAId = a.WorkTypeAId,
                                            WorkTypeCId = a.WorkTypeCId,
                                            WorkTypeIId = a.WorlTypeIId,
                                            WorkTypeRId = a.WorkTypeRId,
                                            WorkTypeSId = a.WorkTypeSId,
                                            Index = a.Index
                                        }).ToList();
            foreach (var item in resultInfo.TaskWorkTypes)
            {
                item.DepartmentIds = (from a in db.Departments.AsNoTracking()
                                      where item.SBUId.Equals(a.SBUId)
                                      orderby a.Name
                                      select new ComboboxResult()
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Code = a.Code,
                                      }).ToList();
            }

            return resultInfo;
        }

        public List<CourseInfoModel> GetCourses(List<string> skillIds)
        {
            var courses = (from a in db.CourseSkills.AsNoTracking()
                           where skillIds.Contains(a.WorkSkillId)
                           join b in db.Courses.AsNoTracking() on a.CourseId equals b.Id
                           select new CourseInfoModel
                           {
                               Id = b.Id,
                               Code = b.Code,
                               Name = b.Name,
                               Description = b.Description,
                               StudyTime = b.StudyTime
                           }
                          ).ToList();
            return courses;
        }

        /// <summary>
        /// Chỉnh sửa công việc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateTask(TaskFlowStageModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();

            var taskUpdate = db.Tasks.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (taskUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Task);
            }

            var taskNameExist = db.Tasks.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (taskNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Task);
            }

            var taskCodeExist = db.Tasks.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && !string.IsNullOrEmpty(a.Code) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (taskCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Task);
            }

            try
            {
                //var jsonBefor = AutoMapperConfig.Mapper.Map<TaskFlowStageHistoryModel>(taskUpdate);

                taskUpdate.Name = model.Name.NTSTrim();
                taskUpdate.Code = model.Code.NTSTrim();
                taskUpdate.IsDesignModule = model.IsDesignModule;
                taskUpdate.Type = model.Type;
                taskUpdate.Description = model.Description.NTSTrim();
                taskUpdate.TimeStandard = model.TimeStandard;
                taskUpdate.DegreeId = model.DegreeId;
                taskUpdate.Specialization = model.Specialization;
                taskUpdate.SpecializeId = model.SpecializeId;
                //taskUpdate.WorkTypeRId = model.WorkTypeRId;
                //taskUpdate.WorkTypeAId = model.WorkTypeAId;
                //taskUpdate.WorkTypeSId = model.WorkTypeSId;
                //taskUpdate.WorkTypeCId = model.WorkTypeCId;
                //taskUpdate.WorkTypeIId = model.WorkTypeIId;
                //taskUpdate.SBUId = model.SBUId;
                //taskUpdate.DepartmentId = model.DepartmentId;
                taskUpdate.FlowStageId = model.FlowStageId;
                taskUpdate.PercentValue = model.PercentValue;
                taskUpdate.UpdateDate = DateTime.Now;
                taskUpdate.UpdateBy = model.UpdateBy;
                taskUpdate.IsProjectWork = model.IsProjectWork;

                var skills = db.TaskSkills.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskSkills.RemoveRange(skills);

                //var documents = db.TaskDocuments.Where(a => a.TaskId.Equals(model.Id)).ToList();
                //db.TaskDocuments.RemoveRange(documents);
                var taskWorkTypes = db.TaskWorkTypes.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskWorkTypes.RemoveRange(taskWorkTypes);

                var documents = db.DocumentObjects.Where(a => a.ObjectId.Equals(model.Id)).ToList();
                db.DocumentObjects.RemoveRange(documents);

                var materials = db.TaskMaterils.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskMaterils.RemoveRange(materials);

                var taskOutputRessults = db.TaskOutputRessults.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskOutputRessults.RemoveRange(taskOutputRessults);

                var taskInputRessults = db.TaskInputRessults.Where(a => a.TaskId.Equals(model.Id)).ToList();
                db.TaskInputRessults.RemoveRange(taskInputRessults);

                //Thêm kỹ năng
                foreach (var item in model.Skills)
                {
                    TaskSkill taskSkill = new TaskSkill()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaskId = taskUpdate.Id,
                        SkillId = item.Id,
                    };
                    db.TaskSkills.Add(taskSkill);

                }

                //Thêm tài liệu
                foreach (var item in model.Documents)
                {
                    DocumentObject taskDocument = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ObjectId = taskUpdate.Id,
                        DocumentId = item.Id,
                        ObjectType = Constants.ObjectType_Work,
                    };
                    db.DocumentObjects.Add(taskDocument);
                }

                //Thêm công cụ
                foreach (var item in model.Materials)
                {
                    TaskMateril taskMateril = new TaskMateril()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaskId = taskUpdate.Id,
                        MaterialId = item.Id,
                        Quantity = item.Quantity
                    };
                    db.TaskMaterils.Add(taskMateril);
                }

                //Thêm danh sách kế quả đầu ra
                foreach (var item in model.OutputResults)
                {
                    TaskOutputRessult taskOutputRessult = new TaskOutputRessult()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaskId = taskUpdate.Id,
                        OutputRessultId = item.Id
                    };
                    db.TaskOutputRessults.Add(taskOutputRessult);
                }

                //Thêm danh sách kế quả đầu vào
                foreach (var item in model.InputResults)
                {
                    TaskInputRessult taskInputRessult = new TaskInputRessult()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaskId = taskUpdate.Id,
                        OutputRessultId = item.Id
                    };
                    db.TaskInputRessults.Add(taskInputRessult);
                }
                // thêm Task work type
                int i = 0;
                foreach (var item in model.TaskWorkTypes)
                {
                    i = i + 1;
                    TaskWorkType taskWorkType = new TaskWorkType()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TaskId = taskUpdate.Id,
                        DepartmentId = item.DepartmentId,
                        SBUId = item.SBUId,
                        WorkTypeAId = item.WorkTypeAId,
                        WorkTypeCId = item.WorkTypeCId,
                        WorkTypeSId = item.WorkTypeSId,
                        WorlTypeIId = item.WorkTypeIId,
                        WorkTypeRId = item.WorkTypeRId,
                        Index = i
                    };
                    db.TaskWorkTypes.Add(taskWorkType);
                }

                //var jsonApter = AutoMapperConfig.Mapper.Map<TaskFlowStageHistoryModel>(taskUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Task, taskUpdate.Id, taskUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
