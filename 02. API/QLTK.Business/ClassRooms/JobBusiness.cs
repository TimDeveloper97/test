using NTS.Model.Repositories;
using System;
using NTS.Model.Subjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Job;
using NTS.Model.JobSupjects;
using NTS.Model;
using NTS.Model.Combobox;
using Syncfusion.XlsIO;
using System.Web.Hosting;
using System.Web;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.JobAttach;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.HistoryVersion;
using NTS.Model.ClassRoom;
using NTS.Model.ClassRoomProduct;
using QLTK.Business.ModuleMaterials;
using NTS.Model.Product;

namespace QLTK.Business.Jobs
{
    public class JobBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();


        /// <summary>
        /// Tìm kiếm nghề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public object SearchJob(JobSearchModel modelSearch)
        {
            SearchResultModel<JobModel> searchResult = new SearchResultModel<JobModel>();
            try
            {
                var data = (from a in db.Jobs.AsNoTracking()
                            join b in db.JobGroups.AsNoTracking() on a.JobGroupId equals b.Id
                            join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
                            join d in db.JobSubjects.AsNoTracking() on a.Id equals d.JobId into da
                            from d in da.DefaultIfEmpty()
                            join e in db.Subjects.AsNoTracking() on d.SubjectsId equals e.Id into ea
                            from e in ea.DefaultIfEmpty()
                            orderby a.Code
                            select new JobModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                JobGroupId = b.Id,
                                JobGroupName = b.Name,
                                DegreeId = c.Id,
                                DegreeName = c.Name,
                                Description = a.Description,
                                SubjectId = e.Id,
                                SubjectName = e.Name,
                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    data = data.Where(t => t.Code.Contains(modelSearch.Code));
                }
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    data = data.Where(t => t.Name.Contains(modelSearch.Name));
                }
                if (!string.IsNullOrEmpty(modelSearch.JobGroupId))
                {
                    data = data.Where(t => t.JobGroupId.Contains(modelSearch.JobGroupId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DegreeId))
                {
                    data = data.Where(t => t.DegreeId.Contains(modelSearch.DegreeId));
                }
                if (!string.IsNullOrEmpty(modelSearch.SubjectName))
                {
                    data = data.Where(t => t.SubjectName.Contains(modelSearch.SubjectName));
                }
                List<JobModel> listRs = new List<JobModel>();
                var lstRs = data.GroupBy(t => new { t.Id, t.Code, t.Name, t.JobGroupName, t.DegreeName, t.Description });
                foreach (var item in lstRs)
                {
                    JobModel rs = new JobModel();
                    rs.Id = item.Key.Id;
                    rs.Code = item.Key.Code;
                    rs.Name = item.Key.Name;
                    rs.JobGroupName = item.Key.JobGroupName;
                    rs.DegreeName = item.Key.DegreeName;
                    rs.Description = item.Key.Description;
                    List<string> lstSTemp = new List<string>();
                    foreach (var ite in item.ToList())
                    {
                        if (lstSTemp.Count > 0)
                        {
                            if (!lstSTemp.Contains(ite.SubjectName))
                            {
                                rs.SubjectName += ", " + ite.SubjectName;
                                lstSTemp.Add(ite.SubjectName);
                            }

                        }
                        else
                        {
                            rs.SubjectName += ite.SubjectName;
                            lstSTemp.Add(ite.SubjectName);
                        }
                    }
                    listRs.Add(rs);
                }

                searchResult.TotalItem = listRs.Count();
                var listResult = SQLHelpper.OrderBy(listRs.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw;
            }
            return searchResult;
        }

        public JobModel GetProductClassInfor(JobModel model)
        {
            JobModel jobModel = new JobModel();
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
                                   PracticeGroupName = c.Name,
                                   Pricing = b.Price
                               }).ToList();
            jobModel.ListClassRoomProductModel = listProduct;

            List<ModuleInProductModel> listModuleProducts = new List<ModuleInProductModel>();
            foreach (var cp in listProduct)
            {
                var queryProductModule = (from a in db.ProductModules.AsNoTracking()
                                          join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                          join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                          where cp.Id.Equals(a.ProductId)
                                          orderby b.Code
                                          select new ModuleInProductModel
                                          {
                                              Id = a.Id,
                                              ModuleId = b.Id,
                                              Qty = a.Quantity,
                                              ModuleName = b.Name,
                                              Specification = b.Specification,
                                              Note = b.Note,
                                              Code = b.Code,
                                              Price = b.Pricing,
                                              LeadTime = b.Leadtime,
                                              Status = b.Status,
                                          }).AsQueryable();
                foreach (var c in queryProductModule)
                {
                    if (listModuleProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                    {
                        listModuleProducts.Add(c);
                    }

                }
            }
            jobModel.ListModuleProduct = listModuleProducts;
            var totalPricing = listProduct.Sum(a => a.Pricing * a.Quantity);
            return jobModel;
        }

        public JobModel GetSubjectInfo(JobModel model)
        {
            try
            {
                List<ClassRoomResultModel> listClassRoom = new List<ClassRoomResultModel>();

                foreach (var item in model.ListSubject)
                {
                    var classRooms = (from a in db.ClassRooms.AsNoTracking()
                                      join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id into ab
                                      from ba in ab.DefaultIfEmpty()
                                      join cs in db.SubjectsClassRooms.AsNoTracking() on a.Id equals cs.ClassRoomId
                                      where cs.SubjectsId.Equals(item.Id)
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
                                      }).ToList();
                    foreach (var c in classRooms)
                    {
                        if (listClassRoom.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                        {
                            listClassRoom.Add(c);
                        }

                    }
                }
                model.ListClassRoom = listClassRoom;
                List<ClassRoomProductModel> classRoomProducts = new List<ClassRoomProductModel>();
                foreach (var cr in listClassRoom)
                {
                    var listProduct = (from a in db.ClassRoomProducts.AsNoTracking()
                                       where a.ClassRoomId.Equals(cr.Id)
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
                                           PracticeGroupName = c.Name,
                                           Pricing = b.Price
                                       }).ToList();
                    foreach (var c in listProduct)
                    {
                        if (classRoomProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                        {
                            classRoomProducts.Add(c);
                        }

                    }
                }
                model.ListClassRoomProductModel = classRoomProducts;
                List<ModuleInProductModel> listModuleProducts = new List<ModuleInProductModel>();
                foreach (var cp in classRoomProducts)
                {
                    var queryProductModule = (from a in db.ProductModules.AsNoTracking()
                                              join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                              join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                              where cp.Id.Equals(a.ProductId)
                                              orderby b.Code
                                              select new ModuleInProductModel
                                              {
                                                  Id = a.Id,
                                                  ModuleId = b.Id,
                                                  Qty = a.Quantity,
                                                  ModuleName = b.Name,
                                                  Specification = b.Specification,
                                                  Note = b.Note,
                                                  Code = b.Code,
                                                  Price = b.Pricing,
                                                  LeadTime = b.Leadtime,
                                              }).AsQueryable();
                    foreach (var c in queryProductModule)
                    {
                        if (listModuleProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                        {
                            listModuleProducts.Add(c);
                        }

                    }
                }
                model.ListModuleProduct = listModuleProducts;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
            return model;
        }

        public JobModel GetClassRoomByIdSubject(string Id)
        {
            JobModel jobModel = new JobModel();
            var classRooms = (from a in db.ClassRooms.AsNoTracking()
                              join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id into ab
                              from ba in ab.DefaultIfEmpty()
                              join cs in db.SubjectsClassRooms.AsNoTracking() on a.Id equals cs.ClassRoomId
                              where cs.SubjectsId.Equals(Id)
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
                              }).ToList();
            jobModel.ListClassRoom = classRooms;
            List<ClassRoomProductModel> classRoomProducts = new List<ClassRoomProductModel>();
            foreach (var cr in classRooms)
            {
                var listProduct = (from a in db.ClassRoomProducts.AsNoTracking()
                                   where a.ClassRoomId.Equals(cr.Id)
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
                                       PracticeGroupName = c.Name,
                                       Pricing = b.Price
                                   }).ToList();
                foreach (var c in listProduct)
                {
                    if (classRoomProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                    {
                        classRoomProducts.Add(c);
                    }

                }
            }
            jobModel.ListClassRoomProductModel = classRoomProducts;
            List<ModuleInProductModel> listModuleProducts = new List<ModuleInProductModel>();
            foreach (var cp in classRoomProducts)
            {
                var queryProductModule = (from a in db.ProductModules.AsNoTracking()
                                          join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                          join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                          where cp.Id.Equals(a.ProductId)
                                          orderby b.Code
                                          select new ModuleInProductModel
                                          {
                                              Id = a.Id,
                                              ModuleId = b.Id,
                                              Qty = a.Quantity,
                                              ModuleName = b.Name,
                                              Specification = b.Specification,
                                              Note = b.Note,
                                              Code = b.Code,
                                              Price = b.Pricing,
                                              LeadTime = b.Leadtime,
                                              Status = b.Status,
                                          }).AsQueryable();
                foreach (var c in queryProductModule)
                {
                    if (listModuleProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                    {
                        listModuleProducts.Add(c);
                    }

                }
            }
            jobModel.ListModuleProduct = listModuleProducts;
            return jobModel;
        }


        /// <summary>
        /// xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcel(JobSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.Jobs.AsNoTracking()
                             join b in db.JobGroups.AsNoTracking() on a.JobGroupId equals b.Id
                             join c in db.Degrees.AsNoTracking() on a.DegreeId equals c.Id
                             join d in db.JobSubjects.AsNoTracking() on a.Id equals d.JobId into da
                             from d in da.DefaultIfEmpty()
                             join e in db.Subjects.AsNoTracking() on d.SubjectsId equals e.Id into ea
                             from e in ea.DefaultIfEmpty()
                             select new JobModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 JobGroupId = b.Id,
                                 JobGroupName = b.Name,
                                 DegreeId = c.Id,
                                 DegreeName = c.Name,
                                 Description = a.Description,
                                 SubjectId = e.Id,
                                 SubjectName = e.Name,
                                 CreateBy = a.CreateBy,
                                 CreateDate = a.CreateDate,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.Contains(model.Code));
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.DegreeId))
            {
                dataQuery = dataQuery.Where(t => t.DegreeId.Contains(model.DegreeId));
            }
            if (!string.IsNullOrEmpty(model.SubjectName))
            {
                dataQuery = dataQuery.Where(t => t.SubjectName.Contains(model.SubjectName));
            }
            if (!string.IsNullOrEmpty(model.JobGroupId))
            {
                dataQuery = dataQuery.Where(t => t.JobGroupId.Contains(model.JobGroupId));
            }
            List<JobModel> listModel = new List<JobModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Code, t.Name, t.JobGroupName, t.DegreeName, t.Description });
            if (lstRs.ToList().Count > 0)
                foreach (var item in lstRs)
                {
                    JobModel rs = new JobModel();
                    rs.Id = item.Key.Id;
                    rs.Code = item.Key.Code;
                    rs.Name = item.Key.Name;
                    rs.JobGroupName = item.Key.JobGroupName;
                    rs.DegreeName = item.Key.DegreeName;
                    rs.Description = item.Key.Description;
                    List<string> lstSTemp = new List<string>();
                    foreach (var ite in item.ToList())
                    {
                        if (lstSTemp.Count > 0)
                        {
                            if (!lstSTemp.Contains(ite.SubjectName))
                            {
                                rs.SubjectName += ", " + ite.SubjectName;
                                lstSTemp.Add(ite.SubjectName);
                            }

                        }
                        else
                        {
                            rs.SubjectName += ite.SubjectName;
                            lstSTemp.Add(ite.SubjectName);
                        }
                    }
                    listModel.Add(rs);
                }

            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Job.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.OrderBy(i => i.Name).Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.SubjectName,
                    a.JobGroupName,
                    a.DegreeName,
                    a.Description,
                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách nghề" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách nghề" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        public void Create(JobModel model)
        {
            var checkJob = db.Jobs.AsNoTracking().Where(u => u.Code.ToLower().Equals(model.Code.ToLower()) || u.Name.ToLower().Equals(model.Name.ToLower())).ToList();
            if (checkJob.FirstOrDefault(u => u.Code.ToLower().Equals(model.Code.ToLower())) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Job);
            }
            if (checkJob.FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.ToLower())) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Job);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    Job job = new Job()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                        DegreeId = model.DegreeId,
                        JobGroupId = model.JobGroupId,
                        Description = model.Description.NTSTrim(),
                        Documents = model.Documents.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = dateNow,
                    };
                    db.Jobs.Add(job);

                    // thêm danh mục môn học
                    if (model.ListJobSubject.Count > 0)
                    {
                        foreach (var item in model.ListJobSubject)
                        {
                            JobSubject jobSubject = new JobSubject
                            {
                                Id = Guid.NewGuid().ToString(),
                                JobId = job.Id,
                                SubjectsId = item.Id,
                            };
                            db.JobSubjects.Add(jobSubject);
                        }
                    }
                    // thêm tài liệu

                    if (model.ListJobAttach.Count > 0)
                    {
                        foreach (var ite in model.ListJobAttach)
                        {
                            if (ite.Path != null)
                            {
                                JobAttach jobAttach = new JobAttach
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    JobId = job.Id,
                                    FileName = ite.FileName,
                                    FileSize = ite.FileSize,
                                    Path = ite.Path,
                                    CreateBy = model.CreateBy,
                                    CreateDate = dateNow,
                                    UpdateBy = model.UpdateBy,
                                    UpdateDate = dateNow,
                                };
                                db.JobAttaches.Add(jobAttach);
                            }

                        }
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, job.Code, job.Id, Constants.LOG_Job);

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
        /// chọn danh sách môn học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        /// lấy dữ liệu danh sách môn học
        public object GetSubject(SubjectsModel modelSearch)
        {
            SearchResultModel<SubjectsModel> searchResult = new SearchResultModel<SubjectsModel>();

            var dataQuery = (from a in db.Subjects.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new SubjectsModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddJob(JobModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var oldSubject = db.JobSubjects.Where(a => a.JobId.Equals(model.Id));
                if (oldSubject != null)
                {
                    db.JobSubjects.RemoveRange(oldSubject);
                }
                try
                {
                    if (model.ListJobSubject != null)
                    {
                        foreach (var item in model.ListJobSubject)
                        {
                            JobSubject newJobSubjectsModel = new JobSubject
                            {
                                Id = Guid.NewGuid().ToString(),
                                JobId = model.Id,
                                SubjectsId = item.Id,
                            };

                            db.JobSubjects.Add(newJobSubjectsModel);
                        }

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

        public object GetJobInfo(JobModel model)
        {
            var resultInfo = (from a in db.Jobs.AsNoTracking()
                              select new JobModel
                              {
                                  Id = a.Id,
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Sketches);
            }

            return resultInfo;
        }

        public object SearchSubject(JobModel modelSearch)
        {
            SearchResultModel<SubjectsModel> searchResult = new SearchResultModel<SubjectsModel>();
            try
            {
                var subject = db.JobSubjects.AsNoTracking().Where(a => a.JobId.Equals(modelSearch.Id));
                List<string> listSubjectId = new List<string>();
                if (subject != null)
                {
                    foreach (var item in subject)
                    {
                        listSubjectId.Add(item.SubjectsId);
                    }
                }

                var dataQuery = (from a in db.Subjects.AsNoTracking()
                                 where listSubjectId.Contains(a.Id)
                                 select new SubjectsModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,

                                 }).AsQueryable();


                searchResult.TotalItem = dataQuery.Count();
                //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = dataQuery.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Xóa nghề
        /// </summary>
        /// <param name="model"></param>
        public void DeleteJob(JobModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var JobDelete = db.Jobs.Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
                    if (JobDelete != null)
                    {
                        var jobsubject = db.JobSubjects.Where(o => o.JobId.Equals(model.Id)).ToList();
                        if (jobsubject != null)
                        {
                            db.JobSubjects.RemoveRange(jobsubject);
                        }

                        var jobAttach = db.JobAttaches.Where(m => m.JobId.Equals(model.Id)).ToList();
                        if (jobAttach != null)
                        {
                            db.JobAttaches.RemoveRange(jobAttach);
                        }

                        var educationProgramJobs = db.EducationProgramJobs.Where(m => m.JobId.Equals(model.Id)).ToList();
                        if (educationProgramJobs != null)
                        {
                            db.EducationProgramJobs.RemoveRange(educationProgramJobs);
                        }

                        db.Jobs.Remove(JobDelete);

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<JobHistoryModel>(JobDelete);
                        //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Job, JobDelete.Id, JobDelete.Code, jsonBefor);

                        db.SaveChanges();
                        trans.Commit();

                    }
                    else
                    {
                        throw new Exception("Bản ghi này đã bị xóa.");
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            //luu Log lich su
            string decription = "Xóa nghề tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        /// <summary>
        /// Get dữ liệu nghề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JobModel GetJobInfor(JobModel model)
        {
            try
            {
                List<ClassRoomResultModel> listClassRoom = new List<ClassRoomResultModel>();
                var job = db.Jobs.AsNoTracking().FirstOrDefault(o => o.Id.Equals(model.Id));
                model.Name = job.Name;
                model.Code = job.Code;
                model.DegreeId = job.DegreeId;
                model.JobGroupId = job.JobGroupId;
                model.Description = job.Description;
                model.Documents = job.Documents;
                model.CreateBy = job.CreateBy;
                model.CreateDate = DateTime.Now;
                model.UpdateBy = job.UpdateBy;
                model.UpdateDate = DateTime.Now;
                var listJobSubject = (from a in db.JobSubjects.AsNoTracking()
                                      join c in db.Subjects.AsNoTracking() on a.SubjectsId equals c.Id
                                      join d in db.Jobs.AsNoTracking() on a.JobId equals d.Id
                                      where a.JobId.Equals(model.Id) && a.SubjectsId.Equals(c.Id)
                                      select new SubjectsModel
                                      {
                                          Id = c.Id,
                                          Name = c.Name,
                                          Code = c.Code,
                                      }).AsQueryable();
                model.ListSubject = listJobSubject.ToList();
                var listAttach = (from b in db.JobAttaches.AsNoTracking()
                                  where b.JobId.Equals(model.Id)
                                  select new JobAttachModel
                                  {
                                      Id = b.Id,
                                      FileName = b.FileName,
                                      FileSize = b.FileSize,
                                      Path = b.Path,
                                      UpdateBy = b.UpdateBy,
                                      UpdateDate = b.UpdateDate,
                                      CreateBy = b.CreateBy,
                                      CreateDate = b.CreateDate,
                                  }).AsQueryable();
                model.ListJobAttach = listAttach.ToList();
                foreach (var item in listJobSubject)
                {
                    var classRooms = (from a in db.ClassRooms.AsNoTracking()
                                      join b in db.RoomTypes.AsNoTracking() on a.RoomTypeId equals b.Id into ab
                                      from ba in ab.DefaultIfEmpty()
                                      join cs in db.SubjectsClassRooms.AsNoTracking() on a.Id equals cs.ClassRoomId
                                      where cs.SubjectsId.Equals(item.Id)
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
                                      }).ToList();
                    foreach (var c in classRooms)
                    {
                        if (listClassRoom.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                        {
                            listClassRoom.Add(c);
                        }

                    }
                }
                model.ListClassRoom = listClassRoom;
                List<ClassRoomProductModel> classRoomProducts = new List<ClassRoomProductModel>();
                foreach (var cr in listClassRoom)
                {
                    var listProduct = (from a in db.ClassRoomProducts.AsNoTracking()
                                       where a.ClassRoomId.Equals(cr.Id)
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
                                           PracticeGroupName = c.Name,
                                           Pricing =b.Price
                                       }).ToList();
                    foreach (var c in listProduct)
                    {
                        if (classRoomProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                        {
                            classRoomProducts.Add(c);
                        }

                    }
                }
                model.ListClassRoomProductModel = classRoomProducts;
                List<ModuleInProductModel> listModuleProducts = new List<ModuleInProductModel>();
                foreach (var cp in classRoomProducts)
                {
                    var queryProductModule = (from a in db.ProductModules.AsNoTracking()
                                              join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                                              join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                              where cp.Id.Equals(a.ProductId)
                                              orderby b.Code
                                              select new ModuleInProductModel
                                              {
                                                  Id = a.Id,
                                                  ModuleId = b.Id,
                                                  Qty = a.Quantity,
                                                  ModuleName = b.Name,
                                                  Specification = b.Specification,
                                                  Note = b.Note,
                                                  Code = b.Code,
                                                  Price = b.Pricing,
                                                  LeadTime = b.Leadtime,
												  Status = b.Status,
                                              }).AsQueryable();
                    foreach (var c in queryProductModule)
                    {
                        if (listModuleProducts.FindAll(cl => cl.Id.Equals(c.Id)).Count() == 0)
                        {
                            c.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(c.ModuleId, 0);
                            listModuleProducts.Add(c);
                        }

                    }
                }
                model.ListModuleProduct = listModuleProducts;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
            return model;
        }

        public void UpdateJob(JobModel model)
        {
            // kiểm tra mã nghề
            if (db.Jobs.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Job);
            }
            // kiểm tra tên nghề
            if (db.Jobs.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Job);
            }

            var job = db.Jobs.Where(o => o.Id.Equals(model.Id)).FirstOrDefault();

            //var jsonApter = AutoMapperConfig.Mapper.Map<JobHistoryModel>(job);

            if (job == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Job);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    job.Name = model.Name.NTSTrim();
                    job.Code = model.Code.NTSTrim();
                    job.DegreeId = model.DegreeId;
                    job.JobGroupId = model.JobGroupId;
                    job.Documents = model.Documents.NTSTrim();
                    job.Description = model.Description.NTSTrim();
                    job.UpdateBy = model.UpdateBy;
                    job.UpdateDate = DateTime.Now;
                    // update danh mục môn học
                    var listJobSubject = db.JobSubjects.Where(a => a.JobId.Equals(model.Id)).ToList();
                    if (listJobSubject.Count > 0)
                    {
                        db.JobSubjects.RemoveRange(listJobSubject);
                    }
                    if (model.ListJobSubject.Count > 0)
                    {
                        foreach (var item in model.ListJobSubject)
                        {
                            JobSubject jobsubject = new JobSubject
                            {
                                Id = Guid.NewGuid().ToString(),
                                JobId = job.Id,
                                SubjectsId = item.Id,
                            };
                            db.JobSubjects.Add(jobsubject);
                        }
                    }

                    // update danh mục file
                    var listJobAttach = db.JobAttaches.Where(b => b.JobId.Equals(model.Id)).ToList();
                    if (listJobAttach.Count > 0)
                    {
                        db.JobAttaches.RemoveRange(listJobAttach);
                    }
                    if (model.ListJobAttach.Count > 0)
                    {
                        foreach (var item in model.ListJobAttach)
                        {
                            if (item.Path != null)
                            {
                                JobAttach jobattach = new JobAttach
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    JobId = job.Id,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    Path = item.Path,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = model.UpdateBy,
                                    UpdateDate = DateTime.Now,

                                };
                                db.JobAttaches.Add(jobattach);
                            }

                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<JobHistoryModel>(job);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Job, job.Id, job.Code, jsonBefor, jsonApter);

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
}
