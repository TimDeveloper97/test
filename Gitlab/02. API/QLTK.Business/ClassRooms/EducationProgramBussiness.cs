using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.EducationProgram;
using NTS.Model.EducationProgramAttachModel;
using NTS.Model.HistoryVersion;
using NTS.Model.Job;
using NTS.Model.Repositories;
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

namespace QLTK.Business.EducationProgram
{
    public class EducationProgramBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<EducationProgramResultModel> SearchEducationProgram(EducationProgramSearchModel modelSearch)
        {
            SearchResultModel<EducationProgramResultModel> searchResult = new SearchResultModel<EducationProgramResultModel>();

            var dataQuery = (from a in db.EducationPrograms.AsNoTracking()

                             join b in db.EducationProgramJobs.AsNoTracking() on a.Id equals b.EducationProgramId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Jobs.AsNoTracking() on b.JobId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()

                             join e in db.EducationProgramAttaches.AsNoTracking() on a.Id equals e.EducationProgramId into ae
                             from e in ae.DefaultIfEmpty()
                             select new EducationProgramResultModel
                             {

                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 JobId = c.Id,
                                 JobName = c.Name,
                                 Documents = a.Documents,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.JobId))
            {
                dataQuery = dataQuery.Where(u => u.JobId.ToUpper().Equals(modelSearch.JobId.ToUpper()));
            }


            List<EducationProgramResultModel> listRs = new List<EducationProgramResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Code, t.Name }).ToList();
            foreach (var item in lstRs)
            {
                EducationProgramResultModel rs = new EducationProgramResultModel();
                rs.Id = item.Key.Id;
                rs.Code = item.Key.Code;
                rs.Name = item.Key.Name;

                List<string> lstJobTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstJobTemp.Count > 0)
                    {
                        if (!lstJobTemp.Contains(ite.JobName))
                        {
                            rs.JobName += ", " + ite.JobName;
                            lstJobTemp.Add(ite.JobName);
                        }

                    }
                    else
                    {
                        rs.JobName += ite.JobName;
                        lstJobTemp.Add(ite.JobName);
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

        public SearchResultModel<JobResultModel> SearchJob(JobSearchModel modelSearch)
        {
            SearchResultModel<JobResultModel> searchResult = new SearchResultModel<JobResultModel>();
            var dataQuery = (from a in db.Jobs.AsNoTracking()
                             join b in db.JobGroups.AsNoTracking() on a.JobGroupId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new JobResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 JobGroupName = b.Name
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.Contains(modelSearch.Code));
            }
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.Contains(modelSearch.Name));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddEducationProgram(EducationProgramModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.EducationProgram new_educationProgram = new NTS.Model.Repositories.EducationProgram()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        Documents = model.Documents,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    foreach (var item in model.ListJob)
                    {
                        EducationProgramJob eduJob = new EducationProgramJob()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EducationProgramId = new_educationProgram.Id,
                            JobId = item.Id,

                        };
                        db.EducationProgramJobs.Add(eduJob);

                    }

                    //ADD FILE


                    List<EducationProgramAttach> listFileEntity = new List<EducationProgramAttach>();
                    foreach (var item in model.ListFile)
                    {
                        if (!string.IsNullOrEmpty(item.Path))
                        {
                            EducationProgramAttach fileEntity = new EducationProgramAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EducationProgramId = new_educationProgram.Id,
                                Path = item.Path,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                UpdateBy = new_educationProgram.CreateBy,
                                CreateBy = new_educationProgram.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                            };
                            listFileEntity.Add(fileEntity);
                            // db.EducationProgramAttaches.Add(fileEntity);
                        }
                    }
                    db.EducationProgramAttaches.AddRange(listFileEntity);

                    db.EducationPrograms.Add(new_educationProgram);

                    UserLogUtil.LogHistotyAdd(db, new_educationProgram.CreateBy, new_educationProgram.Code, new_educationProgram.Id, Constants.LOG_Education);
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

        public EducationProgramModel GetIdEducationProgram(EducationProgramModel model)
        {
            var resultInfo = db.EducationPrograms.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new EducationProgramModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Documents = p.Documents,
            }).FirstOrDefault();

            var ListJob = (from a in db.EducationProgramJobs.AsNoTracking()
                           where a.EducationProgramId.Equals(model.Id)
                           join b in db.Jobs.AsNoTracking() on a.JobId equals b.Id
                           join c in db.JobGroups.AsNoTracking() on b.JobGroupId equals c.Id
                           orderby b.Name
                           select new JobModel()
                           {
                               Id = b.Id,
                               Code = b.Code,
                               Name = b.Name,
                               Description = b.Description,
                               JobGroupName = c.Name,
                           }).ToList();

            resultInfo.ListJob = ListJob;

            var ListFile = (from a in db.EducationProgramAttaches.AsNoTracking()
                            where a.EducationProgramId.Equals(model.Id)
                            orderby a.FileName
                            select new EducationProgramAttachModel()
                            {
                                Id = a.Id,
                                Path = a.Path,
                                FileName = a.FileName,
                                FileSize = a.FileSize,
                            }).ToList();

            resultInfo.ListFile = ListFile;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EducationProgram);
            }
            return resultInfo;
        }

        public void UpdateEducationProgram(EducationProgramModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var new_Education = db.EducationPrograms.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    {
                        //var jsonApter = AutoMapperConfig.Mapper.Map<EducationHistoryModel>(new_Education);

                        new_Education.Id = model.Id;
                        new_Education.Code = model.Code;
                        new_Education.Name = model.Name;
                        new_Education.Documents = model.Documents;

                        new_Education.CreateBy = model.UpdateBy;
                        new_Education.UpdateBy = model.UpdateBy;
                        new_Education.UpdateDate = DateTime.Now;

                        var listJob = db.EducationProgramJobs.Where(u => u.EducationProgramId.Equals(model.Id)).ToList();
                        if (listJob.Count > 0)
                        {
                            db.EducationProgramJobs.RemoveRange(listJob);
                        }
                        if (model.ListJob.Count > 0)
                        {
                            foreach (var item in model.ListJob)
                            {
                                EducationProgramJob job_edu = new EducationProgramJob()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    EducationProgramId = new_Education.Id,
                                    JobId = item.Id,
                                };
                                db.EducationProgramJobs.Add(job_edu);
                            }
                        }


                        var fileEntities = db.EducationProgramAttaches.Where(t => t.EducationProgramId.Equals(model.Id)).ToList();
                        if (fileEntities.Count > 0)
                        {
                            db.EducationProgramAttaches.RemoveRange(fileEntities);
                        }

                        if (model.ListFile.Count > 0)
                        {
                            List<EducationProgramAttach> listFileEntity = new List<EducationProgramAttach>();
                            foreach (var item in model.ListFile)
                            {
                                if (item.Path != null)
                                {
                                    EducationProgramAttach fileEntity = new EducationProgramAttach()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        EducationProgramId = new_Education.Id,
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
                            db.EducationProgramAttaches.AddRange(listFileEntity);
                        }

                        //var jsonBefor = AutoMapperConfig.Mapper.Map<EducationHistoryModel>(new_Education);
                        //UserLogUtil.LogHistotyUpdateInfo(db, new_Education.CreateBy, Constants.LOG_Education, new_Education.Id, new_Education.Code, jsonBefor, jsonApter);

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

        public void DeleteEducationProgram(EducationProgramModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var educationProgram = db.EducationPrograms.AsNoTracking().Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                {
                    try
                    {
                        var _educationProgram = db.EducationPrograms.FirstOrDefault(u => u.Id.Equals(model.Id));
                        if (_educationProgram == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResource.EducationProgram);
                        }

                        var _job = db.EducationProgramJobs.Where(u => u.EducationProgramId.Equals(model.Id)).ToList();
                        db.EducationProgramJobs.RemoveRange(_job);

                        var files = db.EducationProgramAttaches.Where(u => u.EducationProgramId.Equals(model.Id)).ToList();
                        if (files.Count > 0)
                        {
                            db.EducationProgramAttaches.RemoveRange(files);
                        }

                        db.EducationPrograms.Remove(_educationProgram);

                        //var jsonApter = AutoMapperConfig.Mapper.Map<EducationHistoryModel>(_educationProgram);
                        //UserLogUtil.LogHistotyDelete(db, _educationProgram.CreateBy, Constants.LOG_Education, _educationProgram.Id, _educationProgram.Code, jsonApter);

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

        public string ExportExcel(EducationProgramSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.EducationPrograms.AsNoTracking()

                             join b in db.EducationProgramJobs.AsNoTracking() on a.Id equals b.EducationProgramId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Jobs.AsNoTracking() on b.JobId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()

                             join e in db.EducationProgramAttaches.AsNoTracking() on a.Id equals e.EducationProgramId into ae
                             from e in ae.DefaultIfEmpty()
                             select new EducationProgramResultModel
                             {

                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 JobId = c.Id,
                                 JobName = c.Name,

                             }).AsQueryable();

            List<EducationProgramResultModel> listRs = new List<EducationProgramResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Code, t.Name }).ToList();
            foreach (var item in lstRs)
            {
                EducationProgramResultModel rs = new EducationProgramResultModel();
                rs.Id = item.Key.Id;
                rs.Code = item.Key.Code;
                rs.Name = item.Key.Name;

                List<string> lstJobTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstJobTemp.Count > 0)
                    {
                        if (!lstJobTemp.Contains(ite.JobName))
                        {
                            rs.JobName += ", " + ite.JobName;
                            lstJobTemp.Add(ite.JobName);
                        }

                    }
                    else
                    {
                        rs.JobName += ite.JobName;
                        lstJobTemp.Add(ite.JobName);
                    }
                }
                listRs.Add(rs);
            }

            //searchResult.TotalItem = listRs.Count();
            if (lstRs.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/EducationProgram.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = lstRs.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listRs.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.JobName,

                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 4].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 4].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách chương trình đào tạo" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách chương trình đào tạo" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        private void CheckExistedForAdd(EducationProgramModel model)
        {
            if (db.EducationPrograms.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.EducationProgram);
            }

            if (db.EducationPrograms.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.EducationProgram);
            }
        }

        public void CheckExistedForUpdate(EducationProgramModel model)
        {
            if (db.EducationPrograms.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.EducationProgram);
            }

            if (db.EducationPrograms.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.EducationProgram);
            }
        }
    }
}
