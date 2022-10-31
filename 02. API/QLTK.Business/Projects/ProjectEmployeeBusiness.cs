using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.ExternalEmployee;
using NTS.Model.Projects.ProjectEmloyee;
using NTS.Model.Repositories;
using NTS.Model.Role;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Projects
{
    public class ProjectEmployeeBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public List<ProjectProductToEmployeeModel> GetProjectEmployeeByProjectId(string projectId)
        {
            var data = (from a in db.Projects.AsNoTracking()
                        where a.Id.Equals(projectId)
                        join b in db.ProjectEmployees.AsNoTracking() on a.Id equals b.ProjectId
                        join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                        join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                        join e in db.Roles.AsNoTracking() on b.RoleId equals e.Id
                        select new ProjectProductToEmployeeModel
                        {
                            Id = b.Id,
                            Code = c.Code,
                            ImagePath = c.ImagePath,
                            EmployeeId = b.EmployeeId,
                            EmployeeName = c.Name,
                            EmployeePhone = c.PhoneNumber,
                            Email = c.Email,
                            DepartmentName = d.Name,
                            RoleName = e.Name,
                            RoleId = e.Id,
                            JobDescription = b.JobDescription,
                            StartTime = b.StartTime,
                            EndTime = b.EndTime,
                            Subsidy = b.Subsidy,
                            TimeNow = DateTime.Now,
                            SubsidyStartTime = (DateTime)b.SubsidyStartTime,
                            SubsidyEndTime = (DateTime)b.SubsidyEndTime,
                            Evaluate = b.Evaluate,
                            Status = b.Status,
                            Checked = b.HasContractPlanPermit,
                        }).ToList();
            var list = data.ToList();
            return list;
        }

        public List<ProjectProductToEmployeeModel> GetProjectExternalEmployeeByProjectId(string projectId)
        {
            var data = (from a in db.Projects.AsNoTracking()
                        where a.Id.Equals(projectId)
                        join b in db.ProjectEmployees.AsNoTracking() on a.Id equals b.ProjectId
                        join c in db.ExternalEmployees.AsNoTracking() on b.EmployeeId equals c.Id
                        join e in db.Roles.AsNoTracking() on b.RoleId equals e.Id
                        select new ProjectProductToEmployeeModel
                        {
                            Id = b.Id,
                            ImagePath = c.ImagePath,
                            ExternalEmployeeId = b.EmployeeId,
                            EmployeeName = c.Name,
                            EmployeePhone = c.PhoneNumber,
                            Email = c.Email,
                            RoleName = e.Name,
                            RoleId = e.Id,
                            JobDescription = b.JobDescription,
                            StartTime = b.StartTime,
                            EndTime = b.EndTime,
                            Subsidy = b.Subsidy,
                            TimeNow = DateTime.Now,
                            SubsidyStartTime = (DateTime)b.SubsidyStartTime,
                            SubsidyEndTime = (DateTime)b.SubsidyEndTime,
                            Evaluate = b.Evaluate,
                            Status = b.Status
                        }).ToList();

            var list = data.ToList();
            return list;
        }
        public List<SubsidyHistoryModel> GetSubsidyHistory(string projectEmployeeId)
        {
            var data = (from a in db.SubsidyHistories.AsNoTracking()
                        where a.ProjectEmployeeId.Equals(projectEmployeeId)
                        join b in db.ProjectEmployees.AsNoTracking() on a.ProjectEmployeeId equals b.Id
                        select new SubsidyHistoryModel
                        {
                            Id = a.Id,
                            SubsidyStartTime = a.StartTime,
                            SubsidyEndTime = a.EndTime,
                            Subsidy = a.Subsidy,
                        }).ToList();

            var list = data.ToList();
            return list;
        }

        public object GetDescriptionRoleById(string RoleId)
        {
            var data = (from a in db.Roles.AsNoTracking()
                        where a.Id.Equals(RoleId)
                        select new RoleModel
                        {
                            Id = a.Id,
                            Description = a.Descipton,
                        }).FirstOrDefault();

            return data;
        }


        public List<ProjectEmployeeGroupModel> SearchProjectByEmployeeId(string EmployeeId)
        {
            var data = (from a in db.ProjectEmployees.AsNoTracking()
                        where a.EmployeeId.Equals(EmployeeId)
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id into ba
                        from bac in ba.DefaultIfEmpty()
                        join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id into ac
                        from acd in ac.DefaultIfEmpty()
                        join d in db.Departments.AsNoTracking() on bac.DepartmentId equals d.Id into bacd
                        from bacde in bacd.DefaultIfEmpty()
                        join e in db.Roles.AsNoTracking() on a.RoleId equals e.Id into ae
                        from aef in ae.DefaultIfEmpty()
                        orderby a.EndTime descending
                        select new ProjectEmployeeGroupModel
                        {
                            Code = acd != null ? acd.Code : string.Empty,
                            EmployeeId = bac != null ? bac.Id : string.Empty,
                            ProjectId = a.Id,
                            ProjectName = acd != null ? acd.Name : string.Empty,
                            RoleName = aef != null ? aef.Name : string.Empty,
                            Evaluate = a.Evaluate,
                            DepartmentName = bacde != null ? bacde.Name : string.Empty,
                            JobDescription = a.JobDescription,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            Status = a.Status,
                            StatusProject = acd != null ? acd.Status : string.Empty
                        }).ToList();

            var proByEmployeeId = data.ToList();
            return proByEmployeeId;
        }

        public object GetEmployeeName(string EmployeeId)
        {
            var data = (from a in db.Employees.AsNoTracking()
                        where a.Id.Equals(EmployeeId)
                        select new EmployeeModel
                        {
                            Code = a.Code,
                            Name = a.Name,
                        }).FirstOrDefault();
            return data;
        }

        public object GetExternalEmployeeName(string EmployeeId)
        {
            var data = (from a in db.ExternalEmployees.AsNoTracking()
                        where a.Id.Equals(EmployeeId)
                        select new EmployeeModel
                        {
                            Name = a.Name,
                        }).FirstOrDefault();
            return data;
        }

        public List<ProjectEmployeeGroupModel> SearchProjectByExEmployeeId(string EmployeeId)
        {
            var data = (from a in db.ProjectEmployees.AsNoTracking()
                        where a.EmployeeId.Equals(EmployeeId)
                        join b in db.ExternalEmployees.AsNoTracking() on a.EmployeeId equals b.Id into ba
                        from bac in ba.DefaultIfEmpty()
                        join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id into ac
                        from acd in ac.DefaultIfEmpty()
                        join e in db.Roles.AsNoTracking() on a.RoleId equals e.Id into ae
                        from aef in ae.DefaultIfEmpty()
                        orderby a.EndTime descending
                        select new ProjectEmployeeGroupModel
                        {
                            Code = acd != null ? acd.Code : string.Empty,
                            EmployeeId = bac != null ? bac.Id : string.Empty,
                            ProjectId = a.Id,
                            ProjectName = acd != null ? acd.Name : string.Empty,
                            RoleName = aef != null ? aef.Name : string.Empty,
                            Evaluate = a.Evaluate,
                            JobDescription = a.JobDescription,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            Status = a.Status,
                            StatusProject = acd != null ? acd.Status : string.Empty
                        }).ToList();

            var proByEmployeeId = data.ToList();
            return proByEmployeeId;
        }
        public List<ProjectProductToEmployeeModel> GetProjectEmployeeNotByProjectId(string projectId)
        {
            var data = (from a in db.Projects.AsNoTracking()
                        where a.Id.Equals(projectId)
                        join b in db.ProjectEmployees.AsNoTracking() on a.Id equals b.ProjectId
                        join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                        join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                        join e in db.Roles.AsNoTracking() on b.RoleId equals e.Id
                        select new ProjectProductToEmployeeModel
                        {
                            Id = b.Id,
                            ImagePath = c.ImagePath,
                            EmployeeId = b.EmployeeId,
                            EmployeeName = c.Name,
                            EmployeePhone = c.PhoneNumber,
                            Email = c.Email,
                            DepartmentName = d.Name,
                            RoleName = e.Name,
                            JobDescription = b.JobDescription,
                            StartTime = b.StartTime,
                            EndTime = b.EndTime,
                            Subsidy = b.Subsidy,
                            TimeNow = DateTime.Now,
                            SubsidyStartTime = (DateTime)b.SubsidyStartTime,
                            SubsidyEndTime = (DateTime)b.SubsidyEndTime,
                            Evaluate = b.Evaluate,
                            Status = b.Status
                        }).ToList();

            var list = data.ToList();
            return list;
        }

        public ProjectEmployeeGroupModel SearchProjectEmployeeGroup(ProjectEmployeeSearchModel modelSearch)
        {
            ProjectEmployeeGroupModel rs = new ProjectEmployeeGroupModel();
            try
            {
                var data = (from a in db.Employees.AsNoTracking()
                            join b in db.ProjectEmployees.AsNoTracking() on a.Id equals b.EmployeeId
                            join c in db.Projects.AsNoTracking() on b.ProjectId equals c.Id
                            join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                            join e in db.Roles.AsNoTracking() on b.RoleId equals e.Id
                            orderby a.Id
                            select new ProjectEmployeeGroupModel
                            {
                                EmployeeId = a.Id,
                                ProjectId = c.Id,
                                ProjectName = c.Name,
                                RoleName = e.Name,
                                Evaluate = b.Evaluate,
                                DepartmentName = d.Name,
                                JobDescription = b.JobDescription,
                                StartTime = b.StartTime,
                                EndTime = b.EndTime,
                                Status = b.Status,
                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Id))
                {
                    data = data.Where(t => t.EmployeeId.Contains(modelSearch.Id));
                }
                rs.ListProjectEmployeeGroup = data.ToList();
                rs.TotalItem = data.ToList().Count;
            }
            catch (Exception ex)
            {
                throw;
            }
            return rs;
        }
        public void UpdateSubsidyPE(ProjectProductToEmployeeModel model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                var data = db.ProjectEmployees.FirstOrDefault(x => x.Id.Equals(model.Id));
                if (data != null)
                {
                    data.Evaluate = model.Evaluate;
                    data.Status = model.Status;
                    data.Subsidy = model.Subsidy;
                    data.SubsidyStartTime = model.SubsidyStartTime;
                    data.SubsidyEndTime = model.SubsidyEndTime;
                    data.RoleId = model.RoleId;
                    data.JobDescription = model.JobDescription;
                    data.StartTime = model.StartTime;
                    data.EndTime = model.EndTime;
                }
                db.SaveChanges();
            }

        }

        public void AddSubsidyHistory(SubsidyHistoryModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    SubsidyHistory subsidyHistory = new SubsidyHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectEmployeeId = model.Id,
                        Subsidy = model.Subsidy,
                        StartTime = model.SubsidyStartTime,
                        EndTime = model.SubsidyEndTime,
                    };

                    db.SubsidyHistories.Add(subsidyHistory);

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
        /// Lấy ra danh sách nhân viên khác nhân viên được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id nhân viên được chọn lúc đầu.</param>
        /// <returns></returns>
        public List<EmployeeModel> GetListEmployee(EmployeeSearchModel searchModel)
        {
            var listEmployee = (from b in db.Employees.AsNoTracking()
                                join c in db.Departments.AsNoTracking() on b.DepartmentId equals c.Id
                                where !searchModel.ListEmployeeId.Contains(b.Id)
                                where b.Status.Equals(1)
                                select new EmployeeModel
                                {
                                    Id = b.Id,
                                    Code = b.Code,
                                    ImagePath = b.ImagePath,
                                    Name = b.Name,
                                    DepartmentName = c.Name,
                                    Email = b.Email,
                                    PhoneNumber = b.PhoneNumber,
                                    DepartmentId = c.Id,
                                    Status = b.Status,
                                    EmployeeId = b.Id,
                                }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                listEmployee = listEmployee.Where(a => a.Name.ToLower().Contains(searchModel.Name.ToLower()) || a.Code.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                listEmployee = listEmployee.Where(a => a.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (searchModel.ListEmployeeId.Count > 0)
            {
                foreach (var item in searchModel.ListEmployeeId)
                {
                    var ep = db.ProjectEmployees.Where(g => g.EmployeeId.Equals(item)).FirstOrDefault();
                    if (ep != null)
                    {
                        listEmployee = listEmployee.Where(r => r.EmployeeId != ep.EmployeeId);
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchModel.RoleId))
            {
                List<string> listEmployeeId = new List<string>();
                listEmployeeId = (from a in db.ProjectEmployees.AsNoTracking()
                                  where a.RoleId.Equals(searchModel.RoleId)
                                  select a.EmployeeId).ToList();
                var emplo = string.Join(@" , ", listEmployeeId);
                //listEmployee = listEmployee.Where(a => a.EmployeeId.Equals("000251f7-32c2-405d-8b68-6149219f8afa"));
                listEmployee = listEmployee.Where(a => emplo.Contains(a.EmployeeId));
            }

            return listEmployee.ToList();
        }

        public List<EmployeeModel> GetListExEmployee(EmployeeSearchModel searchModel)
        {
            var listEmployee = (from a in db.ExternalEmployees.AsNoTracking()
                                where !searchModel.ListEmployeeId.Contains(a.Id)
                                select new EmployeeModel
                                {
                                    Id = a.Id,
                                    ImagePath = a.ImagePath,
                                    Name = a.Name,
                                    Email = a.Email,
                                    PhoneNumber = a.PhoneNumber,
                                    EmployeeId = a.Id,
                                }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                listEmployee = listEmployee.Where(a => a.Name.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (searchModel.ListEmployeeId.Count > 0)
            {
                foreach (var item in searchModel.ListEmployeeId)
                {
                    var ep = db.ProjectEmployees.Where(g => g.EmployeeId.Equals(item)).FirstOrDefault();
                    if (ep != null)
                    {
                        listEmployee = listEmployee.Where(r => r.EmployeeId != ep.EmployeeId);
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchModel.RoleId))
            {
                List<string> listEmployeeId = new List<string>();
                listEmployeeId = (from a in db.ProjectEmployees.AsNoTracking()
                                  where a.RoleId.Equals(searchModel.RoleId)
                                  select a.EmployeeId).ToList();
                var emplo = string.Join(@" , ", listEmployeeId);
                //listEmployee = listEmployee.Where(a => a.EmployeeId.Equals("000251f7-32c2-405d-8b68-6149219f8afa"));
                listEmployee = listEmployee.Where(a => emplo.Contains(a.EmployeeId));
            }
            return listEmployee.ToList();
        }
        public List<RoleModel> GetRole()
        {
            var list = (from a in db.Roles.AsNoTracking()
                        where a.IsDisable == false
                        select new RoleModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Index = a.Index,
                        }).ToList();
           
            var listRole = list.OrderBy(x => x.Index).ToList();

            return listRole;
        }

        public void AddMoreProjectEmployee(List<ProjectEmployeeModel> model)
        {
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    ProjectEmployee projectEmployee = new ProjectEmployee()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = item.EmployeeId,
                        Type = item.Type,
                        ProjectId = item.ProjectId,
                        RoleId = item.RoleId,
                        StartTime = item.startTime,
                        EndTime = item.endTime,
                        JobDescription = item.JobDescription,
                        Evaluate = item.Evaluate,
                        Status = item.Status,
                        Subsidy = item.Subsidy,
                        SubsidyStartTime = item.subsidyStartTime,
                        SubsidyEndTime = item.subsidyEndTime,
                    };

                    db.ProjectEmployees.Add(projectEmployee);
                }
                db.SaveChanges();

            }

        }

        public void CreateProjectEmployeeAndExternalEmployee(ProjectExternalEmployeeModel model)
        {
            // kiểm tra mã nhân viên
            var checkEmployees = db.ExternalEmployees.AsNoTracking().Where(u => u.Email.ToLower().Equals(model.Email.ToLower())).ToList();
            
            if (checkEmployees.FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower())) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Employee);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    ExternalEmployee externalEmployee = new ExternalEmployee()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        Email = model.Email.NTSTrim(),
                        DateOfBirth = model.DateOfBirth,
                        PhoneNumber = model.PhoneNumber.NTSTrim(),
                        ImagePath = model.ImagePath,
                        Address = model.Address.NTSTrim(),
                        Gender = model.Gender,
                        BankAccount = model.BankAccount.NTSTrim(),
                        BankName = model.BankName,
                        BankCode = model.BankCode,
                        TaxCode = model.TaxCode,
                        IdentifyAddress = model.IdentifyAddress,
                        CurrentAddress = model.CurrentAddress,
                        CurrentAddressProvinceId = model.CurrentAddressProvinceId,
                        CurrentAddressDistrictId = model.CurrentAddressDistrictId,
                        CurrentAddressWardId = model.CurrentAddressWardId,
                        IdentifyNum = model.IdentifyNum,
                        IdentifyDate = model.IdentifyDate,
                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = dateNow
                    };
                    db.ExternalEmployees.Add(externalEmployee);

                    ProjectEmployee projectEmployee = new ProjectEmployee()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = externalEmployee.Id,
                        Type = model.Type,
                        Status = model.Status,
                        ProjectId = model.ProjectId,
                        RoleId = model.RoleId,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        JobDescription = model.JobDescription,
                        Evaluate = model.Evaluate,
                        Subsidy = model.Subsidy,
                        SubsidyStartTime = model.SubsidyStartTime,
                        SubsidyEndTime = model.SubsidyEndTime,
                    };
                    db.ProjectEmployees.Add(projectEmployee);

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


        public void DeleteEmployee(ProjectEmployeeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var projectEmployee = db.ProjectEmployees.FirstOrDefault(m => m.Id.Equals(model.Id));
                    db.ProjectEmployees.Remove(projectEmployee);

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

        public string ExportExcel(ProjectEmployeeSearchModel model)
        {
            //lấy 2 cái list
            var listEmp = GetProjectEmployeeByProjectId(model.ProjectId);
            var listExEmp = GetProjectExternalEmployeeByProjectId(model.ProjectId);
            //Khởi tạo ex
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ListProjectEmployee.xlsx"));
                //khởi tạo sheet 1
                IWorksheet sheet = workbook.Worksheets[0];

                var total1 = listEmp.Count;

                //Tìm vtri đổ dữ liệu
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

                var listExport = listEmp.Select((a, i) => new
                {
                    Indexs = i + 1,
                    a.Code,
                    a.EmployeeName,
                    a.EmployeePhone,
                    a.Email,
                    a.DepartmentName,
                    a.RoleName,
                    a.JobDescription,
                    Time= a.StartTime.ToString("dd/MM/yyyy") + " - " + a.EndTime.ToString("dd/MM/yyyy"),
                    subsidy = double.Parse(a.Subsidy.ToString()).ToString("#,###", cul.NumberFormat) + "\r\n" + (a.SubsidyStartTime == null ? "" :((DateTime) a.SubsidyStartTime).ToString("dd/MM/yyyy"))
                              + ((a.SubsidyStartTime == null || a.SubsidyEndTime ==null) ? "" :" - ") + (a.SubsidyEndTime == null ? "" : ((DateTime)a.SubsidyEndTime).ToString("dd/MM/yyyy")),
                    evaluate = (a.Evaluate == 1 ? "Xuất sắc" : a.Evaluate == 2 ? "Tốt" : a.Evaluate == 3 ? "Khá" : a.Evaluate == 4 ? "Trung bình" : "Chưa có đánh giá"),
                    Status = a.Status ? "ON" : "OFF",
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total1 - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total1 - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total1 - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total1 - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total1 - 1, 12].Borders.Color = ExcelKnownColors.Black;
               
                ///

                IWorksheet sheet1 = workbook.Worksheets[1];
                var total2 = listExEmp.Count;
                IRange iRangeData1 = sheet1.FindFirst("<Data1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData1.Text = iRangeData1.Text.Replace("<Data1>", string.Empty);

                var listExExport = listExEmp.Select((a, i) => new
                {
                    Indexs = i + 1,
                    a.EmployeeName,
                    a.EmployeePhone,
                    a.Email,
                    a.RoleName,
                    a.JobDescription,
                    Time = a.StartTime.ToString("dd/MM/yyyy") + " - " + a.EndTime.ToString("dd/MM/yyyy"),
                    subsidy = double.Parse(a.Subsidy.ToString()).ToString("#,###", cul.NumberFormat) + "\r\n" + (a.SubsidyStartTime == null ? "" : ((DateTime)a.SubsidyStartTime).ToString("dd/MM/yyyy"))
                             + ((a.SubsidyStartTime == null || a.SubsidyEndTime == null) ? "" : " - ") + (a.SubsidyEndTime == null ? "" : ((DateTime)a.SubsidyEndTime).ToString("dd/MM/yyyy")),
                    evaluate = (a.Evaluate == 1 ? "Xuất sắc" : a.Evaluate == 2 ? "Tốt" : a.Evaluate == 3 ? "Khá" : a.Evaluate == 4 ? "Trung bình" : "Chưa có đánh giá"),
                    Status = a.Status ? "ON" : "OFF",
                });
                if (listExExport.Count() > 1)
                {
                    sheet1.InsertRow(iRangeData1.Row + 1, listExExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet1.ImportData(listExExport, iRangeData1.Row, iRangeData1.Column, false);
                sheet1.Range[iRangeData1.Row, 1, iRangeData1.Row + total2 - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range[iRangeData1.Row, 1, iRangeData1.Row + total2 - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range[iRangeData1.Row, 1, iRangeData1.Row + total2 - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range[iRangeData1.Row, 1, iRangeData1.Row + total2 - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet1.Range[iRangeData1.Row, 1, iRangeData1.Row + total2 - 1, 11].Borders.Color = ExcelKnownColors.Black;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tổng hợp dữ liệu nhân viên tham gia dự án" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tổng hợp dữ liệu nhân viên tham gia dự án" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public void updateHasContractPlanPermit(ProjectProductToEmployeeModel model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                var data = db.ProjectEmployees.FirstOrDefault(x => x.Id.Equals(model.Id));
                if (data != null)
                {
                    data.HasContractPlanPermit = model.Checked;
                }
                db.SaveChanges();
            }
        }
    }
}
