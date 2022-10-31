using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Employees;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using NTS.Utils;
using System.Web;
using NTS.Model.GroupUser;
using Syncfusion.XlsIO;
using NTS.Common.Resource;
using System.Web.Hosting;
using NTS.Model.Families;
using System.IO;
using System.Globalization;
using NTS.Model.Course;
using NTS.Model.EmployeeTraining;
using NTS.Model.EmployeeCourseTraining;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.UserHistory;
using NTS.Model.Employee;
using NTS.Model.Employees.Employee;
using NTS.Model.Document;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorkType;

namespace QLTK.Business.Employees
{
    public class EmployeeBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<EmployeeResultModel> SearchEmployee(EmployeeSearchModel modelSearch)
        {
            SearchResultModel<EmployeeResultModel> searchResult = new SearchResultModel<EmployeeResultModel>();

            var dataQuery = (from a in db.Employees.AsNoTracking()
                             join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.JobPositions.AsNoTracking() on a.JobPositionId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.SBUs.AsNoTracking() on d.SBUId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             orderby a.Code
                             select new EmployeeResultModel
                             {
                                 Id = a.Id,
                                 Status = a.Status,
                                 ImagePath = a.ImagePath,
                                 Name = a.Name,
                                 Code = a.Code,
                                 UserNameId = b == null ? "" : b.Id,
                                 UserName = b == null ? "" : b.UserName,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = d.Name,
                                 JobPositionId = c.Id,
                                 JobPositionName = c.Name,
                                 Address = a.Address,
                                 SBUId = e.Id,
                                 DateOfBirth = a.DateOfBirth,
                                 Email = a.Email,
                                 IsDisable = b == null ? 0 : b.IsDisable,
                                 WorkTypeName = f.Name,
                                 WorkTypeId = f.Id,
                                 StartWorking = a.StartWorking,
                                 TaxCode = a.TaxCode,
                                 ContractExpirationDate = a.ContractExpirationDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.UserName))
            {
                dataQuery = dataQuery.Where(u => u.UserName.ToUpper().Contains(modelSearch.UserName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.TaxCode))
            {
                dataQuery = dataQuery.Where(u => u.TaxCode.ToUpper().Contains(modelSearch.TaxCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }


            if (!string.IsNullOrEmpty(modelSearch.WorkTypeId))
            {
                dataQuery = dataQuery.Where(u => u.WorkTypeId.Equals(modelSearch.WorkTypeId));
            }


            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.Status.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.Status.ToString().ToUpper().Contains(modelSearch.Status.ToString().ToUpper()));
            }

            if (modelSearch.StartWorkingFrom.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.StartWorking.HasValue && a.StartWorking >= modelSearch.StartWorkingFrom);
            }

            if (modelSearch.StartWorkingTo.HasValue)
            {
                modelSearch.StartWorkingTo = modelSearch.StartWorkingTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.StartWorking.HasValue && a.StartWorking <= modelSearch.StartWorkingTo);
            }

            if (modelSearch.ContractExpirationDateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.ContractExpirationDate.HasValue && a.ContractExpirationDate >= modelSearch.ContractExpirationDateFrom);
            }

            if (modelSearch.ContractExpirationDateTo.HasValue)
            {
                modelSearch.ContractExpirationDateTo = modelSearch.ContractExpirationDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.ContractExpirationDate.HasValue && a.ContractExpirationDate <= modelSearch.ContractExpirationDateTo);
            }

            searchResult.Status2 = dataQuery.Where(u => u.Status == 0).Count();
            searchResult.Status1 = dataQuery.Where(u => u.Status == 1).Count();
            searchResult.TotalItem = dataQuery.Count();
            var listResult = NTS.Model.SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            foreach (var item in listResult)
            {
                var courses = (from e in db.EmployeeCourseTrainings.AsNoTracking()
                               join a in db.CourseTrainings.AsNoTracking() on e.CourseTrainingId equals a.Id
                               where e.EmployeeId.Equals(item.Id)
                               select new EmployeeTrainingCourseModel
                               {
                                   Status = a.Status,
                               }).ToList();
                item.TotalCourse = courses.Count();
                item.CourseNumber = courses.Where(c => c.Status == 0).Count();
            }
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcel(EmployeeSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.Employees.AsNoTracking()
                             join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.JobPositions.AsNoTracking() on a.JobPositionId equals c.Id into ac
                             from acv in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.SBUs.AsNoTracking() on d.SBUId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Provinces.AsNoTracking() on a.AddressProvinceId equals f.Id into af
                             from afv in af.DefaultIfEmpty()
                             join g in db.Districts.AsNoTracking() on a.AddressDistrictId equals g.Id into ag
                             from agv in ag.DefaultIfEmpty()
                             join h in db.Wards.AsNoTracking() on a.AddressWardId equals h.Id into ah
                             from ahv in ah.DefaultIfEmpty()
                             join i in db.Provinces.AsNoTracking() on a.PermanentAddressProvinceId equals i.Id into ai
                             from aiv in ai.DefaultIfEmpty()
                             join k in db.Districts.AsNoTracking() on a.PermanentAddressDistrictId equals k.Id into ak
                             from akv in ak.DefaultIfEmpty()
                             join l in db.Wards.AsNoTracking() on a.PermanentAddressWardId equals l.Id into al
                             from alv in al.DefaultIfEmpty()
                             join m in db.Provinces.AsNoTracking() on a.CurrentAddressProvinceId equals m.Id into am
                             from amv in am.DefaultIfEmpty()
                             join n in db.Districts.AsNoTracking() on a.CurrentAddressDistrictId equals n.Id into an
                             from anv in an.DefaultIfEmpty()
                             join u in db.Wards.AsNoTracking() on a.CurrentAddressWardId equals u.Id into au
                             from auv in au.DefaultIfEmpty()
                             join t in db.InsuranceLevels.AsNoTracking() on a.InsuranceLevelId equals t.Id into at
                             from atv in at.DefaultIfEmpty()
                             join o in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals o.Id into ao
                             from aov in ao.DefaultIfEmpty()
                             join x in db.WorkLocations.AsNoTracking() on a.WorkLocationId equals x.Id into ax
                             from axv in ax.DefaultIfEmpty()
                             join y in db.WorkTypes.AsNoTracking() on a.AppliedPositionId equals y.Id into ay
                             from ayv in ay.DefaultIfEmpty()
                             join s in db.EmployeeGroups.AsNoTracking() on a.EmployeeGroupId equals s.EmployeeGroupId into sa
                             from asv in sa.DefaultIfEmpty()
                             orderby a.Code
                             select new EmployeeExportModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 EmailCompany = a.Email,
                                 Email = a.PersonalEmail,
                                 IdentifyNum = a.IdentifyNum,
                                 IdentifyDate = a.IdentifyDate,
                                 IdentifyAddress = a.IdentifyAddress,
                                 Gender = a.Gender,
                                 DateOfBirth = a.DateOfBirth,
                                 PhoneNumber = a.PhoneNumber,
                                 MaritalStatus = a.MaritalStatus,
                                 Address = a.Address,
                                 AddressProvinceName = afv != null ? afv.Name : "",
                                 AddressDistrictName = agv != null ? agv.Name : "",
                                 AddressWardName = ahv != null ? ahv.Name : "",
                                 PermanentAddress = a.PermanentAddress,
                                 PermanentAddressProvinceName = aiv != null ? aiv.Name : "",
                                 PermanentAddressDistrictName = akv != null ? akv.Name : "",
                                 PermanentAddressWardName = alv != null ? alv.Name : "",
                                 CurrentAddress = a.CurrentAddress,
                                 CurrentAddressProvinceName = amv != null ? amv.Name : "",
                                 CurrentAddressDistrictName = anv != null ? anv.Name : "",
                                 CurrentAddressWardName = auv != null ? auv.Name : "",
                                 TaxCode = a.TaxCode,
                                 StartInsurance = a.StartInsurance,
                                 BookNumberInsurance = a.BookNumberInsurance,
                                 CardNumberInsurance = a.CardNumberInsurance,
                                 Kcb = a.Kcb,
                                 InsuranceLevelName = atv != null ? atv.Name : "",
                                 InsuranceMoney = atv != null ? atv.Money.ToString() : "",
                                 Forte = a.Forte,
                                 SBUId = e.Id,
                                 SBUName = e.Name,
                                 SBUCode = e.Code,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = d.Name,
                                 DepartmentCode = d.Code,
                                 StartWorking = a.StartWorking,
                                 Seniority = a.Seniority,
                                 IsOfficial = a.IsOfficial,
                                 IsOfficialName = a.IsOfficial == 1 ? "Thử việc" : "Chính thức",
                                 JobPositionId = a.JobPositionId,
                                 JobPositionName = acv != null ? acv.Name : "",
                                 JobPositionCode = acv != null ? acv.Code : "",
                                 WorkTypeId = a.WorkTypeId,
                                 WorkTypeName = aov != null ? aov.Name : "",
                                 WorkTypeCode = aov != null ? aov.Code : "",
                                 WorkLocationName = axv != null ? axv.Name : "",
                                 AppliedPositionName = ayv != null ? ayv.Name : "",
                                 EmployeeGroupName = asv != null ? asv.Name : "",
                                 Status = a.Status,
                                 StatusName = a.Status == 1 ? "Đang làm việc" : "Đã nghỉ việc",
                                 UserNameId = b == null ? "" : b.Id,
                                 UserName = b == null ? "" : b.UserName,

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()) || u.Name.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                dataQuery = dataQuery.Where(u => u.UserName.ToUpper().Contains(model.UserName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.TaxCode))
            {
                dataQuery = dataQuery.Where(u => u.TaxCode.ToUpper().Contains(model.TaxCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(model.SBUId));
            }

            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(model.DepartmentId));
            }

            if (!string.IsNullOrEmpty(model.Status.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.Status.ToString().ToUpper().Contains(model.Status.ToString().ToUpper()));
            }

            if (model.StartWorkingFrom.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.StartWorking.HasValue && a.StartWorking >= model.StartWorkingFrom);
            }

            if (model.StartWorkingTo.HasValue)
            {
                model.StartWorkingTo = model.StartWorkingTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.StartWorking.HasValue && a.StartWorking <= model.StartWorkingTo);
            }

            List<EmployeeExportModel> listModel = dataQuery.ToList();

            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Employee.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    View9 = a.Status == 1 ? "Đang làm việc" : a.Status == 0 ? "Đã nghỉ việc" : "",
                    a.Name,
                    a.Code,
                    a.UserName,
                    a.EmployeeGroupName,
                    a.DateOfBirth,
                    a.TaxCode,
                    StartInsurance = a.StartInsurance.HasValue ? a.StartInsurance.Value.ToString("dd/MM/yyyy") : "",
                    a.BookNumberInsurance,
                    a.CardNumberInsurance,
                    a.Kcb,
                    a.InsuranceLevelName,
                    a.InsuranceMoney,
                    a.Forte,
                    a.SBUName,
                    a.DepartmentName,
                    a.JobPositionName,
                    a.WorkTypeName,
                    StartWorking = a.StartWorking.HasValue ? a.StartWorking.Value.ToString("dd/MM/yyyy") : "",
                    a.Seniority,
                    a.IsOfficialName,
                    a.WorkLocationName,
                    a.EmailCompany,
                    a.Email,
                    a.IdentifyNum,
                    IdentifyDate = a.IdentifyDate.HasValue ? a.IdentifyDate.Value.ToString("dd/MM/yyyy") : "",
                    a.IdentifyAddress,
                    Gender = a.Gender == 1 ? "Name" : (a.Gender == 2 ? "Nữ" : "Khác"),
                    a.PhoneNumber,
                    MaritalStatus = a.MaritalStatus == 1 ? "Độc thân" : (a.MaritalStatus == 2 ? "Đã có gia đình" : "Ly hôn"),
                    a.Address,
                    a.AddressProvinceName,
                    a.AddressDistrictName,
                    a.AddressWardName,
                    a.PermanentAddress,
                    a.PermanentAddressProvinceName,
                    a.PermanentAddressDistrictName,
                    a.PermanentAddressWardName,
                    a.CurrentAddress,
                    a.CurrentAddressProvinceName,
                    a.CurrentAddressDistrictName,
                    a.CurrentAddressWardName
                });


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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách nhân viên" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách nhân viên" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public List<TaskFlowStageModel> GetWorkList(EmployeeModel model)
        {
            var workTypeId = db.Employees.Where(e => e.Id.Equals(model.Id)).FirstOrDefault().WorkTypeId;

            var tasks = (from a in db.Tasks.AsNoTracking()
                         join b in db.TaskWorkTypes.AsNoTracking() on a.Id equals b.TaskId
                         where b.WorkTypeRId.Equals(workTypeId) || b.WorkTypeAId.Equals(workTypeId) || b.WorkTypeSId.Equals(workTypeId)
                         || b.WorkTypeCId.Equals(workTypeId) | b.WorlTypeIId.Equals(workTypeId)
                         select new TaskFlowStageModel
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Code = a.Code,
                             WorkTypeSId = b.WorkTypeSId,
                             WorkTypeAId = b.WorkTypeAId,
                             WorkTypeCId = b.WorkTypeCId,
                             WorkTypeIId = b.WorlTypeIId,
                             WorkTypeRId = b.WorkTypeRId,
                         }).ToList();
            foreach(var item in tasks)
            {
                item.WorkTypeId = workTypeId;
            }
            
            return tasks;
        }

        public List<WorkTypeDocumentModel> GetProcedure(EmployeeModel model)
        {
            var workTypeId = db.Employees.Where(e => e.Id.Equals(model.Id)).FirstOrDefault().WorkTypeId;
            var tasks = (from a in db.Tasks.AsNoTracking()
                         join b in db.TaskWorkTypes on a.Id equals b.TaskId
                         where b.WorkTypeRId.Equals(workTypeId) || b.WorkTypeAId.Equals(workTypeId) || b.WorkTypeSId.Equals(workTypeId)
                         || b.WorkTypeCId.Equals(workTypeId) | b.WorlTypeIId.Equals(workTypeId)
                         select new TaskFlowStageModel
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Code = a.Code,
                             WorkTypeSId = b.WorkTypeSId,
                             WorkTypeAId = b.WorkTypeAId,
                             WorkTypeCId = b.WorkTypeCId,
                             WorkTypeIId = b.WorlTypeIId,
                             WorkTypeRId = b.WorkTypeRId,
                         }).ToList();

            var documents = (from a in db.DocumentObjects.AsNoTracking()
                             join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                             join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                             join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                             where a.ObjectId.Equals(workTypeId) && d.Code.ToUpper().Equals("QUYTRINH")
                             select new WorkTypeDocumentModel
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                                 Code = b.Code,
                                 DocumentGroupName = c.Name,
                                 DocumentTypeName = d.Name,
                                 IsDocumentOfTask = a.ObjectType == Constants.ObjectType_Work ? true : false
                             }).ToList();

            foreach (var item in tasks)
            {
                //Lấy danh sách tài liệu từ task
                var documentOfTask = (from a in db.DocumentObjects.AsNoTracking()
                                      join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                      join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                      join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                                      where (a.ObjectId.Equals(item.Id) || a.ObjectId.Equals(workTypeId)) && d.Code.ToUpper().Equals("QUYTRINH")
                                      select new WorkTypeDocumentModel
                                      {
                                          Id = b.Id,
                                          Name = b.Name,
                                          Code = b.Code,
                                          DocumentGroupName = c.Name,
                                          DocumentTypeName = d.Name,
                                          IsDocumentOfTask = a.ObjectType == Constants.ObjectType_Work ? true : false
                                      }).ToList();

                foreach (var document in documentOfTask)
                {
                    var documentExist = documents.FirstOrDefault(a => a.Id.Equals(document.Id));
                    if (documentExist == null)
                    {
                        documents.Add(document);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(documentExist.TaskName))
                        {
                            documentExist.TaskName += string.IsNullOrEmpty(documentExist.TaskName) ? item.Name : ";" + item.Name;

                        }
                        else
                        {
                            var taskNames = documentExist.TaskName.Split(';');
                            if (taskNames.Where(t => t.ToUpper().Equals(item.Name.ToUpper())).Count() == 0)
                            {
                                documentExist.TaskName += string.IsNullOrEmpty(documentExist.TaskName) ? item.Name : ";" + item.Name;
                            }
                        }
                    }
                }
            }

            return documents;
        }

        public List<WorkTypeDocumentModel> GetRegulation(EmployeeModel model)
        {
            var workTypeId = db.Employees.Where(e => e.Id.Equals(model.Id)).FirstOrDefault().WorkTypeId;
            var tasks = (from a in db.Tasks.AsNoTracking()
                         where a.WorkTypeRId.Equals(workTypeId) || a.WorkTypeAId.Equals(workTypeId) || a.WorkTypeSId.Equals(workTypeId)
                         || a.WorkTypeCId.Equals(workTypeId) | a.WorkTypeIId.Equals(workTypeId)
                         select new TaskFlowStageModel
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Code = a.Code,
                             WorkTypeSId = a.WorkTypeSId,
                             WorkTypeAId = a.WorkTypeAId,
                             WorkTypeCId = a.WorkTypeCId,
                             WorkTypeIId = a.WorkTypeIId,
                             WorkTypeRId = a.WorkTypeRId,
                         }).ToList();

            var documents = (from a in db.DocumentObjects.AsNoTracking()
                             join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                             join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                             join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                             where a.ObjectId.Equals(workTypeId) && d.Code.ToUpper().Equals("QUYDINH")
                             select new WorkTypeDocumentModel
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                                 Code = b.Code,
                                 DocumentGroupName = c.Name,
                                 DocumentTypeName = d.Name,
                                 IsDocumentOfTask = a.ObjectType == Constants.ObjectType_Work ? true : false
                             }).ToList();

            foreach (var item in tasks)
            {
                //Lấy danh sách tài liệu từ task
                var documentOfTask = (from a in db.DocumentObjects.AsNoTracking()
                                      join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                      join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                      join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                                      where a.ObjectId.Equals(workTypeId) && d.Code.ToUpper().Equals("QUYDINH")
                                      select new WorkTypeDocumentModel
                                      {
                                          Id = b.Id,
                                          Name = b.Name,
                                          Code = b.Code,
                                          DocumentGroupName = c.Name,
                                          DocumentTypeName = d.Name,
                                          IsDocumentOfTask = a.ObjectType == Constants.ObjectType_Work ? true : false
                                      }).ToList();

                foreach (var document in documentOfTask)
                {
                    var documentExist = documents.FirstOrDefault(a => a.Id.Equals(document.Id));
                    if (documentExist == null)
                    {
                        documents.Add(document);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(documentExist.TaskName))
                        {
                            documentExist.TaskName += string.IsNullOrEmpty(documentExist.TaskName) ? item.Name : ";" + item.Name;

                        }
                        else
                        {
                            var taskNames = documentExist.TaskName.Split(';');
                            if (taskNames.Where(t => t.ToUpper().Equals(item.Name.ToUpper())).Count() == 0)
                            {
                                documentExist.TaskName += string.IsNullOrEmpty(documentExist.TaskName) ? item.Name : ";" + item.Name;
                            }
                        }

                    }
                }
            }
            return documents;
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="model"></param>
        public void CreateEmployee(EmployeeModel model)
        {
            // kiểm tra mã nhân viên
            var checkEmployees = db.Employees.AsNoTracking().Where(u => u.Code.ToLower().Equals(model.Code.ToLower()) || u.Email.ToLower().Equals(model.Email.ToLower()) || u.Name.ToLower().Equals(model.Name.ToLower())).ToList();
            if (checkEmployees.FirstOrDefault(u => u.Code.ToLower().Equals(model.Code.ToLower())) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Employee);
            }
            if (checkEmployees.FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower())) != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Employee);
            }

            // check vị trí cv 
            // ds NV của phòng ban
            var ListEmployeDepartment = db.Employees.AsNoTracking().Where(a => a.DepartmentId.Equals(model.DepartmentId)).ToList();
            // check xem có phải là only ko 
            var job_position_only = db.JobPositions.AsNoTracking().FirstOrDefault(p => p.Id.Equals(model.JobPositionId)).IsOnlyOne;
            if (job_position_only == true)
            {
                var jobposition_Id = ListEmployeDepartment.FirstOrDefault(p => p.Id.Equals(model.JobPositionId));
                if (jobposition_Id != null)
                {
                    throw NTSException.CreateInstance("Chức vụ đã có người đảm nhận");
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    Employee employee = new Employee()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                        Email = model.Email.NTSTrim(),
                        ImagePath = model.ImagePath,
                        IdentifyNum = model.IdentifyNum.NTSTrim(),
                        DepartmentId = model.DepartmentId,
                        DateOfBirth = model.DateOfBirth,
                        StartWorking = model.StartWorking,
                        PhoneNumber = model.PhoneNumber.NTSTrim(),
                        Address = model.Address.NTSTrim(),
                        Gender = model.Gender,
                        Status = model.Status,
                        EndWorking = model.EndWorking,
                        JobPositionId = model.JobPositionId.NTSTrim(),
                        WorkTypeId = model.WorkType,
                        Index = model.Index,
                        TaxCode = model.TaxCode,

                        IsOfficial = model.IsOfficial,
                        PersonalEmail = model.PersonalEmail,
                        IdentifyDate = model.IdentifyDate,
                        IdentifyAddress = model.IdentifyAddress,
                        AddressProvinceId = model.AddressProvinceId,
                        AddressDistrictId = model.AddressDistrictId,
                        AddressWardId = model.AddressWardId,
                        PermanentAddress = model.PermanentAddress,
                        PermanentAddressProvinceId = model.PermanentAddressProvinceId,
                        PermanentAddressDistrictId = model.PermanentAddressDistrictId,
                        PermanentAddressWardId = model.PermanentAddressWardId,
                        CurrentAddress = model.CurrentAddress,
                        CurrentAddressProvinceId = model.CurrentAddressProvinceId,
                        CurrentAddressDistrictId = model.CurrentAddressDistrictId,
                        CurrentAddressWardId = model.CurrentAddressWardId,
                        StartInsurance = model.StartInsurance,
                        BookNumberInsurance = model.BookNumberInsurance,
                        CardNumberInsurance = model.CardNumberInsurance,
                        Kcb = model.Kcb,
                        InsuranceLevelId = model.InsuranceLevelId,
                        Forte = model.Forte,
                        MaritalStatus = model.MaritalStatus,
                        Seniority = model.Seniority,
                        ReasonEndWorkingId = model.ReasonEndWorkingId,
                        WorkLocationId = model.WorkLocationId,
                        AppliedPositionId = model.AppliedPositionId,
                        EmployeeGroupId = model.EmployeeGroupId,


                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = dateNow,
                    };
                    db.Employees.Add(employee);

                    // thêm người thân
                    foreach (var item in model.ListFamilies)
                    {
                        Family family = new Family
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            Name = item.Name.Trim(),
                            DateOfBirth = item.DateOfBirth,
                            Relationship = item.Relationship,
                            Gender = item.Gender,
                            Job = item.Job,
                            Workplace = item.Workplace,
                            PhoneNumber = item.PhoneNumber
                        };
                        db.Families.Add(family);
                    }

                    //Thêm ngân hàng
                    foreach (var item in model.BankAccounts)
                    {
                        NTS.Model.Repositories.EmployeeBankAccount bankAccount = new NTS.Model.Repositories.EmployeeBankAccount()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            BankAccountId = item.BankAccountId,
                            AccountNumber = item.AccountNumber
                        };
                        db.EmployeeBankAccounts.Add(bankAccount);
                    }

                    //Thêm lịch sử công tác
                    foreach (var item in model.WorkHistories)
                    {
                        EmployeeWorkHistory employeeWorkHistory = new EmployeeWorkHistory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            CompanyName = item.CompanyName,
                            Position = item.Position,
                            TotalTime = item.TotalTime,
                            WorkTypeId = item.WorkTypeId,
                            ReferencePerson = item.ReferencePerson,
                            ReferencePersonPhone = item.ReferencePersonPhone,
                            NumberOfManage = item.NumberOfManage,
                            Income = item.Income
                        };
                        db.EmployeeWorkHistories.Add(employeeWorkHistory);
                    }

                    //Thêm điều chuyển
                    foreach (var item in model.HistoryJobTranfer)
                    {
                        EmployeeHistoryJobTranfer employeeHistoryJobTranfer = new EmployeeHistoryJobTranfer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            DateJobTranfer = item.DateJobTranfer.Value,
                            WorkTypeId = item.WorkTypeId,
                            DepartmentId = item.DepartmentId,
                            FileName = item.FileName,
                            FilePath = item.FileSize,
                            FileSize = item.FileSize
                        };
                        db.EmployeeHistoryJobTranfers.Add(employeeHistoryJobTranfer);
                    }

                    //Thêm bổ nhiệm
                    foreach (var item in model.HistoryAppoint)
                    {
                        EmployeeHistoryAppoint employeeHistoryAppoint = new EmployeeHistoryAppoint()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            DateAppoint = item.DateAppoint.Value,
                            PositionId = item.PositionId,
                            DepartmentId = item.DepartmentId,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            FilePath = item.FilePath,
                        };
                        db.EmployeeHistoryAppoints.Add(employeeHistoryAppoint);
                    }

                    //Thêm điều chỉnh thu nhập
                    foreach (var item in model.HistoriesIncome)
                    {
                        EmployeeChangeIncome employeeChangeIncome = new EmployeeChangeIncome()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            ReasonChangeIncomeId = item.ReasonChangeIncomeId,
                            NewIncome = item.NewIncome,
                            DateChange = item.DateChange.Value,
                        };
                        db.EmployeeChangeIncomes.Add(employeeChangeIncome);
                    }

                    //Thêm mức bảo hiểm
                    foreach (var item in model.HistoriesInsurance)
                    {
                        EmployeeChangeInsurance employeeChangeInsurance = new EmployeeChangeInsurance()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            ReasonChangeInsuranceId = item.ReasonChangeInsuranceId,
                            InsuranceMoney = item.InsuranceMoney,
                            InsuranceLevelId = item.InsuranceLevelId,
                            DateChange = item.DateChange,
                        };
                        db.EmployeeChangeInsurances.Add(employeeChangeInsurance);
                    }

                    //Thêm mới họp đồng lao động
                    if (model.HistoriesLaborContract.Count > 0)
                    {
                        foreach (var item in model.HistoriesLaborContract)
                        {
                            EmployeeContract employeeContract = new EmployeeContract()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeId = employee.Id,
                                LaborContractId = item.LaborContractId,
                                ContractFrom = item.ContractFrom,
                                ContractTo = item.ContractTo,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                FilePath = item.FilePath
                            };
                            db.EmployeeContracts.Add(employeeContract);
                        }

                        var maxContractTo = model.HistoriesLaborContract.Max(a => a.ContractTo.Value);
                        if (maxContractTo != null)
                        {
                            employee.ContractExpirationDate = maxContractTo;
                        }
                    }



                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, employee.Code, employee.Id, Constants.LOG_Employee);

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

        public void LockEmployee(EmployeeModel model)
        {
            var user = db.Users.Where(o => o.EmployeeId.Equals(model.Id)).FirstOrDefault();
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    user.IsDisable = model.IsDisable;
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

        public void UpdateEmployee(EmployeeModel model)
        {
            // kiểm tra mã nhân viên
            if (db.Employees.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Employee);
            }
            if (db.Employees.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Email))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Employee);
            }
            if (model.StartWorking > model.EndWorking)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0013, TextResourceKey.Employee);
            }
            // kiểm tra tên tài khoản
            if (db.Users.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.UserName.Equals(model.UserName))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.User);
            }
            var employee = db.Employees.Where(o => o.Id.Equals(model.Id)).FirstOrDefault();

            //var jsonApter = AutoMapperConfig.Mapper.Map<EmployeeHistoryModel>(employee);


            if (employee == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
            }

            // check vị trí cv 
            // ds NV của phòng ban
            var ListEmployeDepartment = db.Employees.AsNoTracking().Where(a => a.DepartmentId.Equals(model.DepartmentId)).ToList();
            // check xem có phải là only ko 
            bool job_position_only;
            var job_position = db.JobPositions.AsNoTracking().FirstOrDefault(p => p.Id.Equals(model.JobPositionId));
            if (job_position != null)
            {
                job_position_only = job_position.IsOnlyOne;
                if (job_position_only == true)
                {
                    var jobposition_Id = ListEmployeDepartment.Where(p => p.JobPositionId.Equals(model.JobPositionId)).ToList();
                    if (jobposition_Id.Count() > 0)
                    {
                        throw NTSException.CreateInstance("Chức vụ đã có người đảm nhận");
                    }
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    employee.Name = model.Name.NTSTrim();
                    employee.Code = model.Code.NTSTrim();
                    employee.Email = model.Email.NTSTrim();
                    employee.ImagePath = model.ImagePath;
                    employee.IdentifyNum = model.IdentifyNum.NTSTrim();
                    employee.DepartmentId = model.DepartmentId.NTSTrim();
                    employee.DateOfBirth = model.DateOfBirth;
                    employee.StartWorking = model.StartWorking;
                    employee.PhoneNumber = model.PhoneNumber;
                    employee.Address = model.Address.NTSTrim();
                    employee.Gender = model.Gender;
                    employee.Status = model.Status;
                    employee.EndWorking = model.EndWorking;
                    employee.JobPositionId = model.JobPositionId.NTSTrim();
                    employee.WorkTypeId = model.WorkType;
                    employee.Index = model.Index;
                    employee.UpdateBy = model.UpdateBy;
                    employee.UpdateDate = DateTime.Now;
                    employee.TaxCode = model.TaxCode;

                    employee.IsOfficial = model.IsOfficial;
                    employee.PersonalEmail = model.PersonalEmail;
                    employee.IdentifyDate = model.IdentifyDate;
                    employee.IdentifyAddress = model.IdentifyAddress;
                    employee.AddressProvinceId = model.AddressProvinceId;
                    employee.AddressDistrictId = model.AddressDistrictId;
                    employee.AddressWardId = model.AddressWardId;
                    employee.PermanentAddress = model.PermanentAddress;
                    employee.PermanentAddressProvinceId = model.PermanentAddressProvinceId;
                    employee.PermanentAddressDistrictId = model.PermanentAddressDistrictId;
                    employee.PermanentAddressWardId = model.PermanentAddressWardId;
                    employee.CurrentAddress = model.CurrentAddress;
                    employee.CurrentAddressProvinceId = model.CurrentAddressProvinceId;
                    employee.CurrentAddressDistrictId = model.CurrentAddressDistrictId;
                    employee.CurrentAddressWardId = model.CurrentAddressWardId;
                    employee.StartInsurance = model.StartInsurance;
                    employee.BookNumberInsurance = model.BookNumberInsurance;
                    employee.CardNumberInsurance = model.CardNumberInsurance;
                    employee.Kcb = model.Kcb;
                    employee.InsuranceLevelId = model.InsuranceLevelId;
                    employee.Forte = model.Forte;
                    employee.MaritalStatus = model.MaritalStatus;
                    employee.Seniority = model.Seniority;
                    employee.ReasonEndWorkingId = model.ReasonEndWorkingId;
                    employee.WorkLocationId = model.WorkLocationId;
                    employee.AppliedPositionId = model.AppliedPositionId;
                    employee.EmployeeGroupId = model.EmployeeGroupId;


                    // update bảng người thân
                    var listfamily = db.Families.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    if (listfamily.Count > 0)
                    {
                        db.Families.RemoveRange(listfamily);
                    }
                    if (model.ListFamilies.Count > 0)
                    {
                        foreach (var item in model.ListFamilies)
                        {
                            Family family = new Family
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeId = employee.Id,
                                Name = item.Name.NTSTrim(),
                                DateOfBirth = item.DateOfBirth,
                                Relationship = item.Relationship,
                                Gender = item.Gender,
                                Job = item.Job,
                                Workplace = item.Workplace,
                                PhoneNumber = item.PhoneNumber
                            };
                            db.Families.Add(family);
                        }
                    }

                    //update bảng ngân hàng
                    var bankAccountExist = db.EmployeeBankAccounts.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeBankAccounts.RemoveRange(bankAccountExist);
                    foreach (var item in model.BankAccounts)
                    {
                        NTS.Model.Repositories.EmployeeBankAccount bankAccount = new NTS.Model.Repositories.EmployeeBankAccount()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            BankAccountId = item.BankAccountId,
                            AccountNumber = item.AccountNumber
                        };
                        db.EmployeeBankAccounts.Add(bankAccount);
                    }

                    //Update bảng lịch sử công tác
                    var workHistoriesExist = db.EmployeeWorkHistories.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeWorkHistories.RemoveRange(workHistoriesExist);
                    foreach (var item in model.WorkHistories)
                    {
                        EmployeeWorkHistory employeeWorkHistory = new EmployeeWorkHistory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            CompanyName = item.CompanyName,
                            Position = item.Position,
                            TotalTime = item.TotalTime,
                            WorkTypeId = item.WorkTypeId,
                            ReferencePerson = item.ReferencePerson,
                            ReferencePersonPhone = item.ReferencePersonPhone,
                            NumberOfManage = item.NumberOfManage,
                            Income = item.Income
                        };
                        db.EmployeeWorkHistories.Add(employeeWorkHistory);
                    }

                    // Update lịch sử điều chuyển
                    var jobTranferExist = db.EmployeeHistoryJobTranfers.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeHistoryJobTranfers.RemoveRange(jobTranferExist);
                    foreach (var item in model.HistoryJobTranfer)
                    {
                        EmployeeHistoryJobTranfer employeeHistoryJobTranfer = new EmployeeHistoryJobTranfer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            DateJobTranfer = item.DateJobTranfer.Value,
                            WorkTypeId = item.WorkTypeId,
                            DepartmentId = item.DepartmentId,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            FileSize = item.FileSize
                        };
                        db.EmployeeHistoryJobTranfers.Add(employeeHistoryJobTranfer);
                    }

                    // Update lịch sử bổ nhiệm
                    var appointExist = db.EmployeeHistoryAppoints.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeHistoryAppoints.RemoveRange(appointExist);
                    foreach (var item in model.HistoryAppoint)
                    {
                        EmployeeHistoryAppoint employeeHistoryAppoint = new EmployeeHistoryAppoint()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            DateAppoint = item.DateAppoint.Value,
                            PositionId = item.PositionId,
                            DepartmentId = item.DepartmentId,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            FilePath = item.FilePath,
                        };
                        db.EmployeeHistoryAppoints.Add(employeeHistoryAppoint);
                    }

                    // Update điều chỉnh thu nhập
                    var incomeExist = db.EmployeeChangeIncomes.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeChangeIncomes.RemoveRange(incomeExist);
                    foreach (var item in model.HistoriesIncome)
                    {
                        EmployeeChangeIncome employeeChangeIncome = new EmployeeChangeIncome()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            ReasonChangeIncomeId = item.ReasonChangeIncomeId,
                            NewIncome = item.NewIncome,
                            DateChange = item.DateChange.Value,
                        };
                        db.EmployeeChangeIncomes.Add(employeeChangeIncome);
                    }

                    // Update điều chỉnh mức đóng bảo hiểm
                    var historiesChangeInsuranceExist = db.EmployeeChangeInsurances.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeChangeInsurances.RemoveRange(historiesChangeInsuranceExist);
                    foreach (var item in model.HistoriesInsurance)
                    {
                        EmployeeChangeInsurance employeeChangeInsurance = new EmployeeChangeInsurance()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmployeeId = employee.Id,
                            ReasonChangeInsuranceId = item.ReasonChangeInsuranceId,
                            InsuranceMoney = item.InsuranceMoney,
                            InsuranceLevelId = item.InsuranceLevelId,
                            DateChange = item.DateChange,
                        };
                        db.EmployeeChangeInsurances.Add(employeeChangeInsurance);
                    }

                    // Update hợp đồng lao động
                    var laborContractExist = db.EmployeeContracts.Where(a => a.EmployeeId.Equals(employee.Id)).ToList();
                    db.EmployeeContracts.RemoveRange(laborContractExist);

                    if (model.HistoriesLaborContract.Count > 0)
                    {
                        foreach (var item in model.HistoriesLaborContract)
                        {
                            EmployeeContract employeeContract = new EmployeeContract()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeId = employee.Id,
                                LaborContractId = item.LaborContractId,
                                ContractFrom = item.ContractFrom,
                                ContractTo = item.ContractTo,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                FilePath = item.FilePath
                            };
                            db.EmployeeContracts.Add(employeeContract);
                        }

                        var maxContractTo = model.HistoriesLaborContract.Max(a => a.ContractTo.Value);
                        if (maxContractTo != null)
                        {
                            employee.ContractExpirationDate = maxContractTo;
                        }
                    }



                    //var jsonBefor = AutoMapperConfig.Mapper.Map<EmployeeHistoryModel>(employee);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Employee, employee.Id, employee.Code, jsonBefor, jsonApter);

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
        public EmployeeModel GetEmployeeInfo(EmployeeModel model)
        {
            if (model.UserId != null)
            {
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.UserId));
                var employee1 = db.Employees.AsNoTracking().FirstOrDefault(o => o.Id.Equals(user.EmployeeId));
                model.Name = employee1.Name;
                model.Code = employee1.Code;
                model.Email = employee1.Email;
                model.Id = employee1.Id;
                model.ImagePath = employee1.ImagePath;
                model.IdentifyNum = employee1.IdentifyNum;
                model.DepartmentId = employee1.DepartmentId;
                model.DateOfBirth = employee1.DateOfBirth;
                model.StartWorking = employee1.StartWorking;
                model.PhoneNumber = employee1.PhoneNumber;
                model.Address = employee1.Address;
                model.Gender = employee1.Gender;
                model.Status = employee1.Status;
                model.EndWorking = employee1.EndWorking;
                model.JobPositionId = employee1.JobPositionId;
                model.WorkType = employee1.WorkTypeId;
                model.CreateBy = employee1.CreateBy;
                model.CreateDate = employee1.CreateDate;
                model.UpdateBy = employee1.UpdateBy;
                model.UpdateDate = employee1.UpdateDate;
                model.TaxCode = employee1.TaxCode;

                model.IsOfficial = employee1.IsOfficial.HasValue ? employee1.IsOfficial.Value : 0;
                model.PersonalEmail = employee1.PersonalEmail;
                model.IdentifyDate = employee1.IdentifyDate;
                model.IdentifyAddress = employee1.IdentifyAddress;
                model.AddressProvinceId = employee1.AddressProvinceId;
                model.AddressDistrictId = employee1.AddressDistrictId;
                model.AddressWardId = employee1.AddressWardId;
                model.PermanentAddress = employee1.PermanentAddress;
                model.PermanentAddressProvinceId = employee1.PermanentAddressProvinceId;
                model.PermanentAddressDistrictId = employee1.PermanentAddressDistrictId;
                model.PermanentAddressWardId = employee1.PermanentAddressWardId;
                model.CurrentAddress = employee1.CurrentAddress;
                model.CurrentAddressProvinceId = employee1.CurrentAddressProvinceId;
                model.CurrentAddressDistrictId = employee1.CurrentAddressDistrictId;
                model.CurrentAddressWardId = employee1.CurrentAddressWardId;
                model.StartInsurance = employee1.StartInsurance;
                model.BookNumberInsurance = employee1.BookNumberInsurance;
                model.CardNumberInsurance = employee1.CardNumberInsurance;
                model.Kcb = employee1.Kcb;
                model.InsuranceLevelId = employee1.InsuranceLevelId;
                model.Forte = employee1.Forte;
                model.MaritalStatus = employee1.MaritalStatus;
                model.Seniority = employee1.Seniority;
                model.ReasonEndWorkingId = employee1.ReasonEndWorkingId;
                model.WorkLocationId = employee1.WorkLocationId;
                model.AppliedPositionId = employee1.AppliedPositionId;
                model.EmployeeGroupId = employee1.EmployeeGroupId;
            }

            try
            {
                var employee = db.Employees.AsNoTracking().FirstOrDefault(o => o.Id.Equals(model.Id));
                model.Name = employee.Name;
                model.Code = employee.Code;
                model.Email = employee.Email;
                model.Id = employee.Id;
                model.UserId = model.UserId;
                model.ImagePath = employee.ImagePath;
                model.IdentifyNum = employee.IdentifyNum;
                model.DepartmentId = employee.DepartmentId;
                model.DateOfBirth = employee.DateOfBirth;
                model.StartWorking = employee.StartWorking;
                model.PhoneNumber = employee.PhoneNumber;
                model.Address = employee.Address;
                model.Gender = employee.Gender;
                model.Status = employee.Status;
                model.EndWorking = employee.EndWorking;
                model.JobPositionId = employee.JobPositionId;
                model.WorkType = employee.WorkTypeId;
                model.CreateBy = employee.CreateBy;
                model.CreateDate = employee.CreateDate;
                model.UpdateBy = employee.UpdateBy;
                model.UpdateDate = employee.UpdateDate;
                model.TaxCode = employee.TaxCode;

                model.IsOfficial = employee.IsOfficial.HasValue ? employee.IsOfficial.Value : 0;
                model.PersonalEmail = employee.PersonalEmail;
                model.IdentifyDate = employee.IdentifyDate;
                model.IdentifyAddress = employee.IdentifyAddress;
                model.AddressProvinceId = employee.AddressProvinceId;
                model.AddressDistrictId = employee.AddressDistrictId;
                model.AddressWardId = employee.AddressWardId;
                model.PermanentAddress = employee.PermanentAddress;
                model.PermanentAddressProvinceId = employee.PermanentAddressProvinceId;
                model.PermanentAddressDistrictId = employee.PermanentAddressDistrictId;
                model.PermanentAddressWardId = employee.PermanentAddressWardId;
                model.CurrentAddress = employee.CurrentAddress;
                model.CurrentAddressProvinceId = employee.CurrentAddressProvinceId;
                model.CurrentAddressDistrictId = employee.CurrentAddressDistrictId;
                model.CurrentAddressWardId = employee.CurrentAddressWardId;
                model.StartInsurance = employee.StartInsurance;
                model.BookNumberInsurance = employee.BookNumberInsurance;
                model.CardNumberInsurance = employee.CardNumberInsurance;
                model.Kcb = employee.Kcb;
                model.InsuranceLevelId = employee.InsuranceLevelId;
                model.Forte = employee.Forte;
                model.MaritalStatus = employee.MaritalStatus;
                model.Seniority = employee.Seniority;
                model.ReasonEndWorkingId = employee.ReasonEndWorkingId;
                model.WorkLocationId = employee.WorkLocationId;
                model.AppliedPositionId = employee.AppliedPositionId;
                model.EmployeeGroupId = employee.EmployeeGroupId;


                var departments = db.Departments.AsNoTracking().Where(a => a.Id.Equals(model.DepartmentId)).FirstOrDefault();
                if (departments != null)
                {
                    model.DepartmentCode = departments.Code;
                    model.SBUID = departments.SBUId;

                }



                // Lấy danh sách tài khoản ngân hàng
                model.BankAccounts = (from a in db.EmployeeBankAccounts.AsNoTracking()
                                      where a.EmployeeId.Equals(model.Id)
                                      select new EmployeeBankAccountModel
                                      {
                                          Id = a.Id,
                                          EmployeeId = a.EmployeeId,
                                          BankAccountId = a.BankAccountId,
                                          AccountNumber = a.AccountNumber,
                                      }).ToList();

                // Lấy danh sách lịch sử công việc
                model.WorkHistories = (from a in db.EmployeeWorkHistories.AsNoTracking()
                                       where a.EmployeeId.Equals(model.Id)
                                       select new EmployeeWorkHistoryModel
                                       {
                                           Id = a.Id,
                                           EmployeeId = a.EmployeeId,
                                           CompanyName = a.CompanyName,
                                           Position = a.Position,
                                           TotalTime = a.TotalTime,
                                           WorkTypeId = a.WorkTypeId,
                                           ReferencePerson = a.ReferencePerson,
                                           ReferencePersonPhone = a.ReferencePersonPhone,
                                           NumberOfManage = a.NumberOfManage.HasValue ? a.NumberOfManage.Value : 0,
                                           Income = a.Income.HasValue ? a.Income.Value : 0
                                       }).ToList();

                // Lấy danh sách người thân
                var family = (from a in db.Families.AsNoTracking()
                              where a.EmployeeId.Equals(model.Id)
                              select new FamiliesModel
                              {
                                  Id = a.Id,
                                  EmployeeId = a.EmployeeId,
                                  Name = a.Name,
                                  DateOfBirth = a.DateOfBirth,
                                  Relationship = a.Relationship,
                                  Gender = a.Gender,
                                  Job = a.Job,
                                  Workplace = a.Workplace,
                                  PhoneNumber = a.PhoneNumber
                              }).ToList();

                if (family.Count > 0)
                {
                    model.ListFamilies = family;
                }

                // Lấy danh sách điều chuyển
                model.HistoryJobTranfer = (from a in db.EmployeeHistoryJobTranfers.AsNoTracking()
                                           where a.EmployeeId.Equals(model.Id)
                                           orderby a.DateJobTranfer descending
                                           select new EmployeeJobTranferModel
                                           {
                                               Id = a.Id,
                                               EmployeeId = a.EmployeeId,
                                               DateJobTranfer = a.DateJobTranfer,
                                               WorkTypeId = a.WorkTypeId,
                                               DepartmentId = a.DepartmentId,
                                               FileName = a.FileName,
                                               FileSize = a.FileSize,
                                               FilePath = a.FilePath
                                           }).ToList();

                // Lấy danh sách bổ nhiệm
                model.HistoryAppoint = (from a in db.EmployeeHistoryAppoints.AsNoTracking()
                                        where a.EmployeeId.Equals(model.Id)
                                        orderby a.DateAppoint descending
                                        select new EmployeeHistoryAppointModel
                                        {
                                            Id = a.Id,
                                            EmployeeId = a.EmployeeId,
                                            DateAppoint = a.DateAppoint,
                                            PositionId = a.PositionId,
                                            DepartmentId = a.DepartmentId,
                                            FileName = a.FileName,
                                            FilePath = a.FilePath,
                                            FileSize = a.FileSize
                                        }).ToList();

                // Lấy danh sách điều chỉnh thu nhập
                model.HistoriesIncome = (from a in db.EmployeeChangeIncomes.AsNoTracking()
                                         where a.EmployeeId.Equals(model.Id)
                                         select new EmployeeHistoryIncomeModel
                                         {
                                             Id = a.Id,
                                             EmployeeId = a.EmployeeId,
                                             ReasonChangeIncomeId = a.ReasonChangeIncomeId,
                                             NewIncome = a.NewIncome,
                                             DateChange = a.DateChange
                                         }).ToList();

                // Lấy danh sách điều chỉnh mức đóng bảo hiểm
                model.HistoriesInsurance = (from a in db.EmployeeChangeInsurances.AsNoTracking()
                                            where a.EmployeeId.Equals(model.Id)
                                            select new EmployeeHistoryInsuranceModel
                                            {
                                                Id = a.Id,
                                                EmployeeId = a.EmployeeId,
                                                ReasonChangeInsuranceId = a.ReasonChangeInsuranceId,
                                                InsuranceMoney = a.InsuranceMoney,
                                                DateChange = a.DateChange,
                                                InsuranceLevelId = a.InsuranceLevelId
                                            }).ToList();

                // Lấy danh sách hợp đồng lao động
                model.HistoriesLaborContract = (from a in db.EmployeeContracts.AsNoTracking()
                                                where a.EmployeeId.Equals(model.Id)
                                                orderby a.ContractFrom descending
                                                select new EmployeeHistoryLaborContractModel
                                                {
                                                    Id = a.Id,
                                                    EmployeeId = a.EmployeeId,
                                                    LaborContractId = a.LaborContractId,
                                                    ContractFrom = a.ContractFrom,
                                                    ContractTo = a.ContractTo,
                                                    FileName = a.FileName,
                                                    FilePath = a.FilePath,
                                                    FileSize = a.FileSize
                                                }).ToList();

            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
            return model;
        }

        public void DeleteEmployee(EmployeeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Users.AsNoTracking().Where(r => r.EmployeeId.Equals(model.Id) && r.IsLogin == true).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Employee);
                }
                if (db.EmployeeSkillDetails.AsNoTracking().Where(r => r.EmployeeId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Employee);
                }
                if (db.EmployeeEducations.AsNoTracking().Where(r => r.EmployeeId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Employee);
                }

                if (db.Responsibilities.AsNoTracking().Where(r => r.EmployeeId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Employee);
                }
                if (db.TaskTimeStandards.AsNoTracking().Where(r => r.EmployeeId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Employee);
                }

                var familis = db.Families.Where(m => m.EmployeeId.Equals(model.Id)).ToList();
                var employeedegree = db.EmployeeDegrees.Where(m => m.EmployeeId.Equals(model.Id)).ToList();
                var skill = db.EmployeeSkillDetails.Where(m => m.EmployeeId.Equals(model.Id)).ToList();
                try
                {
                    var employees = db.Employees.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (employees == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
                    }
                    if (model.Status == 0 && db.Users.AsNoTracking().Where(r => r.EmployeeId.Equals(model.Id)).Count() > 0)
                    {
                        var user = db.Users.Where(m => m.EmployeeId.Equals(model.Id)).FirstOrDefault();
                        var idUser = db.UserPermissions.Where(m => m.UserId.Equals(user.Id)).ToList();
                        db.UserPermissions.RemoveRange(idUser);
                        db.Users.Remove(user);
                    }
                    db.EmployeeSkillDetails.RemoveRange(skill);
                    db.EmployeeDegrees.RemoveRange(employeedegree);
                    db.Families.RemoveRange(familis);
                    db.Employees.Remove(employees);

                    var employeeBankAcc = db.EmployeeBankAccounts.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeBankAccounts.RemoveRange(employeeBankAcc);
                    var changeInsurance = db.EmployeeChangeInsurances.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeChangeInsurances.RemoveRange(changeInsurance);
                    var laborContract = db.EmployeeContracts.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeContracts.RemoveRange(laborContract);
                    var changeJob = db.EmployeeHistoryJobTranfers.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeHistoryJobTranfers.RemoveRange(changeJob);
                    var changeAppoint = db.EmployeeHistoryAppoints.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeHistoryAppoints.RemoveRange(changeAppoint);
                    var workHistory = db.EmployeeWorkHistories.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeWorkHistories.RemoveRange(workHistory);
                    var changeIncome = db.EmployeeChangeIncomes.Where(a => a.EmployeeId.Equals(model.Id)).ToList();
                    db.EmployeeChangeIncomes.RemoveRange(changeIncome);

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

        public List<PermissionModel> GetPermissionByGroupId(EmployeeModel model)
        {
            List<PermissionModel> lstResult = new List<PermissionModel>();
            try
            {
                if (string.IsNullOrEmpty(model.GroupUserIdOld) || !model.GroupUserIdOld.Equals(model.GroupUserId))
                {
                    model.ListPermission = new List<string>();
                }
                lstResult = (from a in db.Permissions.AsNoTracking()
                             join b in db.GroupPermissions.AsNoTracking() on a.Id equals b.PermissionId
                             join c in db.GroupUsers.AsNoTracking() on b.GroupUserId equals c.Id
                             where c.Id.Equals(model.GroupUserId)
                             select new PermissionModel
                             {
                                 FunctionId = a.Id,
                                 FunctionCode = a.Code,
                                 FunctionName = a.Name,
                                 Checked = (!string.IsNullOrEmpty(model.GroupUserIdOld) && model.GroupUserIdOld.Equals(model.GroupUserId) && model.ListPermission.Contains(a.Id)) ? true : false,
                             }).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("QLTK.ErrosProcess");
            }
            return lstResult;
        }

        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string Name, Code, DateOfBirth, Gender, Email, Phone, Status, IdentifyNum, Address, JobPositionId, DepartmentId, MaritalStatus, WorkTypeId;
            var employees = db.Employees.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<Employee> list = new List<Employee>();
            Employee itemC;
            List<int> rowName = new List<int>();
            List<int> rowCode = new List<int>();
            List<int> rowDateOfBirth = new List<int>();
            List<int> rowGender = new List<int>();
            List<int> rowEmail = new List<int>();
            List<int> rowStatus = new List<int>();
            List<int> rowJobPositionId = new List<int>();
            List<int> rowDepartmentId = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<int> rowCheckName = new List<int>();
            List<int> rowCheckDateOfBirth = new List<int>();
            List<int> rowCheckGender = new List<int>();
            List<int> rowCheckEmail = new List<int>();
            List<int> rowCheckPhone = new List<int>();
            List<int> rowCheckIdentifyNum = new List<int>();
            List<int> rowCheckJobPositionId = new List<int>();
            List<int> rowCheckDepartmentId = new List<int>();
            List<int> rowCheckWorkTypeId = new List<int>();
            List<int> rowCheckMaritalStatus = new List<int>();

            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new Employee();
                    itemC.Id = Guid.NewGuid().ToString();
                    Name = sheet[i, 2].Value;
                    Code = sheet[i, 3].Value;
                    DateOfBirth = sheet[i, 4].Value;
                    Gender = sheet[i, 5].Value;
                    MaritalStatus = sheet[i, 6].Value;



                    Email = sheet[i, 7].Value;
                    Phone = sheet[i, 8].Value;
                    Status = sheet[i, 9].Value;
                    IdentifyNum = sheet[i, 10].Value;
                    Address = sheet[i, 11].Value;
                    WorkTypeId = sheet[i, 12].Value;
                    JobPositionId = sheet[i, 13].Value;
                    DepartmentId = sheet[i, 14].Value;

                    //Code
                    try
                    {
                        if (!string.IsNullOrEmpty(Code))
                        {
                            if (db.Employees.AsNoTracking().Where(o => o.Code.Equals(Code)).Count() > 0 && db.Employees.AsNoTracking().Where(o => o.Name.Equals(Name)).Count() == 0)
                            {
                                rowCheckCode.Add(i);
                            }
                            else
                            {
                                itemC.Code = Code;
                            }
                        }
                        else
                        {
                            rowCode.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCode.Add(i);
                        continue;
                    }

                    //Name
                    try
                    {
                        if (!string.IsNullOrEmpty(Name))
                        {
                            if (db.Employees.AsNoTracking().Where(o => o.Name.Equals(Name)).Count() > 0 && db.Employees.AsNoTracking().Where(o => o.Code.Equals(Code)).Count() == 0)
                            {
                                rowCheckCode.Add(i);
                            }
                            else
                            {
                                itemC.Name = Name;
                            }
                        }
                        else
                        {
                            rowName.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowName.Add(i);
                        continue;
                    }

                    //DateOfBrith
                    try
                    {
                        if (!string.IsNullOrEmpty(DateOfBirth))
                        {
                            DateTime dt = DateTime.ParseExact(DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            itemC.DateOfBirth = dt;
                            //itemC.DateOfBirth = Convert.ToDateTime(DateOfBirth);
                        }
                        else
                        {
                            rowDateOfBirth.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckDateOfBirth.Add(i);
                        continue;
                    }

                    //Gender
                    try
                    {
                        if (!string.IsNullOrEmpty(Gender))
                        {
                            if (Gender.Equals("Nam"))
                            {
                                itemC.Gender = 1;
                            }
                            else if (Gender.Equals("Nữ"))
                            {
                                itemC.Gender = 2;
                            }
                            else if (Gender.Equals("Khác"))
                            {
                                itemC.Gender = 3;
                            }
                        }
                        else
                        {
                            rowGender.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowGender.Add(i);
                        continue;
                    }

                    //Tình trạng hôn nhân
                    try
                    {
                        if (!string.IsNullOrEmpty(MaritalStatus))
                        {
                            if (Gender.Equals("Độc thân"))
                            {
                                itemC.MaritalStatus = 1;
                            }
                            else if (Gender.Equals("Đã có gia đình"))
                            {
                                itemC.MaritalStatus = 2;
                            }
                            else if (Gender.Equals("Ly hôn"))
                            {
                                itemC.MaritalStatus = 3;
                            }
                        }
                        else
                        {
                            rowCheckMaritalStatus.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckMaritalStatus.Add(i);
                        continue;
                    }

                    //Email
                    try
                    {
                        if (!string.IsNullOrEmpty(Email))
                        {
                            var email = new System.Net.Mail.MailAddress(Email);
                            itemC.Email = Email;
                        }
                        else
                        {
                            rowEmail.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.Email == null)
                        {
                            rowCheckEmail.Add(i);
                        }
                        else
                        {
                            rowEmail.Add(i);
                        }
                        continue;
                    }

                    //Phone
                    if (!string.IsNullOrEmpty(Phone))
                    {
                        var test = 0;
                        foreach (char c in Phone)
                        {
                            if (!char.IsDigit(c))
                            {
                                test += 1;
                            }
                        }
                        if (test > 0)
                        {
                            rowCheckPhone.Add(i);
                        }
                        else
                        {
                            itemC.PhoneNumber = Phone;
                        }
                    }

                    //Status
                    try
                    {
                        if (!string.IsNullOrEmpty(Status))
                        {
                            if (Status.Equals("Đang làm việc"))
                            {
                                itemC.Status = 1;
                            }
                            else if (Gender.Equals("Đã nghỉ việc"))
                            {
                                itemC.Status = 2;
                            }
                        }
                        else
                        {
                            rowStatus.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowStatus.Add(i);
                        continue;
                    }

                    //IdentifyNum
                    if (!string.IsNullOrEmpty(IdentifyNum))
                    {
                        var test = 0;
                        foreach (char c in IdentifyNum)
                        {
                            if (!char.IsDigit(c))
                            {
                                test += 1;
                            }
                        }
                        if (test > 0)
                        {
                            rowCheckIdentifyNum.Add(i);
                        }
                        else
                        {
                            itemC.IdentifyNum = Phone;
                        }
                    }

                    //Address
                    if (!string.IsNullOrEmpty(Address))
                    {
                        itemC.Address = Address;
                    }

                    //JobPositionId
                    try
                    {
                        if (!string.IsNullOrEmpty(JobPositionId))
                        {
                            itemC.JobPositionId = db.JobPositions.AsNoTracking().FirstOrDefault(u => u.Name.Equals(JobPositionId)).Id;
                        }
                        else
                        {
                            rowJobPositionId.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.JobPositionId == null)
                        {
                            rowCheckJobPositionId.Add(i);
                        }
                        else
                        {
                            rowJobPositionId.Add(i);
                        }
                        continue;
                    }

                    //Vị trí worktype
                    try
                    {
                        if (!string.IsNullOrEmpty(WorkTypeId))
                        {
                            itemC.WorkTypeId = db.WorkTypes.AsNoTracking().FirstOrDefault(u => u.Name.Equals(WorkTypeId)).Id;
                        }
                        else
                        {
                            rowCheckWorkTypeId.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.WorkTypeId == null)
                        {
                            rowCheckWorkTypeId.Add(i);
                        }
                        else
                        {
                            rowCheckWorkTypeId.Add(i);
                        }
                        continue;
                    }

                    //DepartmentId
                    try
                    {
                        if (!string.IsNullOrEmpty(DepartmentId))
                        {
                            itemC.DepartmentId = db.Departments.AsNoTracking().FirstOrDefault(u => u.Name.Equals(DepartmentId)).Id;
                        }
                        else
                        {
                            rowDepartmentId.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.DepartmentId == null)
                        {
                            rowCheckDepartmentId.Add(i);
                        }
                        else
                        {
                            rowDepartmentId.Add(i);
                        }
                        continue;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    var check = db.Employees.FirstOrDefault(t => t.Code.ToUpper().Equals(itemC.Code.ToUpper()) && t.Name.ToUpper().Equals(itemC.Name.ToUpper()));
                    if (check != null)
                    {
                        check.Code = itemC.Code;
                        check.Name = itemC.Name;
                        check.DateOfBirth = itemC.DateOfBirth;
                        check.Gender = itemC.Gender;
                        check.Email = itemC.Email;
                        check.PhoneNumber = itemC.PhoneNumber;
                        check.Status = itemC.Status;
                        check.IdentifyNum = itemC.IdentifyNum;
                        check.Address = itemC.Address;
                        check.JobPositionId = itemC.JobPositionId;
                        check.DepartmentId = itemC.DepartmentId;
                        check.WorkTypeId = itemC.WorkTypeId;
                        check.MaritalStatus = itemC.MaritalStatus;
                        check.UpdateBy = userId;
                        check.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        list.Add(itemC);
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }

            if (rowCheckCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã khách hàng dòng <" + string.Join(", ", rowCheckCode) + "> đã tồn tại!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên khách hàng dòng <" + string.Join(", ", rowCheckName) + "> đã tồn tại!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckDateOfBirth.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày sinh dòng <" + string.Join(", ", rowCheckDateOfBirth) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckGender.Count > 0)
            {
                throw NTSException.CreateInstance("Giới tính dòng <" + string.Join(", ", rowCheckGender) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckEmail.Count > 0)
            {
                throw NTSException.CreateInstance("Email dòng <" + string.Join(", ", rowCheckEmail) + "> không đúng định dạng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckPhone.Count > 0)
            {
                throw NTSException.CreateInstance("Số điện dòng <" + string.Join(", ", rowCheckPhone) + "> chỉ được chứa số!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckIdentifyNum.Count > 0)
            {
                throw NTSException.CreateInstance("Số chứng minh dòng <" + string.Join(", ", rowCheckIdentifyNum) + "> chỉ được chứa số!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckJobPositionId.Count > 0)
            {
                throw NTSException.CreateInstance("Vị trí dòng <" + string.Join(", ", rowCheckJobPositionId) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckDepartmentId.Count > 0)
            {
                throw NTSException.CreateInstance("Phòng ban dòng <" + string.Join(", ", rowCheckJobPositionId) + "> không đúng!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            //Thiếu trường
            if (rowCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã khách hàng dòng <" + string.Join(", ", rowCode) + "> không được phép để trống!");
            }

            if (rowName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên khách hàng dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowGender.Count > 0)
            {
                throw NTSException.CreateInstance("Giới tính dòng <" + string.Join(", ", rowGender) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowEmail.Count > 0)
            {
                throw NTSException.CreateInstance("Email dòng <" + string.Join(", ", rowEmail) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowStatus.Count > 0)
            {
                throw NTSException.CreateInstance("Tình trạng dòng <" + string.Join(", ", rowStatus) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowJobPositionId.Count > 0)
            {
                throw NTSException.CreateInstance("Chức vụ dòng <" + string.Join(", ", rowStatus) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckWorkTypeId.Count > 0)
            {
                throw NTSException.CreateInstance("Vị trí dòng <" + string.Join(", ", rowStatus) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowCheckMaritalStatus.Count > 0)
            {
                throw NTSException.CreateInstance("Tình trạng hôn nhân <" + string.Join(", ", rowStatus) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            if (rowDepartmentId.Count > 0)
            {
                throw NTSException.CreateInstance("Phòng ban dòng <" + string.Join(", ", rowStatus) + "> không được phép để trống!");
                //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
            }

            db.Employees.AddRange(list);
            db.SaveChanges();

            workbook.Close();
            excelEngine.Dispose();
        }

        /// <summary>
        /// Danh sách khóa học đã tham gia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<CourseInfoModel> GetListCourse(EmployeeModel model)
        {
            var data = (from a in db.Employees.AsNoTracking()
                        join b in db.EmployeeCourseTrainings.AsNoTracking() on a.Id equals b.EmployeeId
                        join c in db.CourseTrainings.AsNoTracking() on b.CourseTrainingId equals c.Id
                        join d in db.Courses.AsNoTracking() on c.CourseId equals d.Id
                        join e in db.EmployeeTranings.AsNoTracking() on c.EmployeeTrainingId equals e.Id
                        where model.Id.Equals(a.Id)
                        orderby c.Status, c.StartDate descending
                        select new CourseInfoModel
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Code = d.Code,
                            Description = d.Description,
                            StudyTime = d.StudyTime,
                            Status = c.Status,
                            DeviceForCourse = d.DeviceForCourse,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            EmployeeTraningName = e.Name,
                            EmployeeTraningCode = e.Code,
                        }).ToList();

            return data;
        }

        // Danh sách trương trình đào tạo đã tham gia

        public List<EmployeeTrainingModel> GetListEmployeeTraining(EmployeeModel model)
        {
            var data = (from a in db.EmployeeTranings.AsNoTracking()
                        join b in db.CourseTrainings.AsNoTracking() on a.Id equals b.EmployeeTrainingId
                        join c in db.EmployeeCourseTrainings.AsNoTracking() on b.Id equals c.CourseTrainingId
                        join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                        where model.Id.Equals(d.Id)
                        orderby a.Name
                        select new EmployeeTrainingModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                        }).ToList();
            return data;
        }

        /// <summary>
        /// Xuất excel file template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcelTemplate(EmployeeSearchModel model)
        {
            model.IsExport = true;

            var listPosition = db.JobPositions.AsNoTracking().Select(a => a.Name).ToList();
            var listDepartment = db.Departments.AsNoTracking().Select(a => a.Name).ToList();
            var listWorkType = db.WorkTypes.AsNoTracking().Select(a => a.Name).ToList();

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Import_Nhân viên_Template.xlsx"));

                IWorksheet sheet1 = workbook.Worksheets[1];
                IWorksheet sheet2 = workbook.Worksheets[2];
                IWorksheet sheet3 = workbook.Worksheets[3];

                //var total = listModel.Count;

                IRange iRangeData = sheet1.FindFirst("<position>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<position>", string.Empty);
                var listExportPosition = listPosition.Select((a, i) => new
                {
                    Name = a
                });


                if (listExportPosition.Count() > 1)
                {
                    sheet1.InsertRow(iRangeData.Row + 1, listExportPosition.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet1.ImportData(listExportPosition, iRangeData.Row, iRangeData.Column, false);




                IRange iRangeData1 = sheet2.FindFirst("<department>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData1.Text = iRangeData1.Text.Replace("<department>", string.Empty);
                var listExportDepartment = listDepartment.Select((a, i) => new
                {
                    Name = a
                });


                if (listDepartment.Count() > 1)
                {
                    sheet2.InsertRow(iRangeData1.Row + 1, listExportDepartment.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet2.ImportData(listExportDepartment, iRangeData.Row, iRangeData.Column, false);

                IRange iRangeData2 = sheet3.FindFirst("<worktype>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData2.Text = iRangeData2.Text.Replace("<worktype>", string.Empty);
                var listExportWorkType = listWorkType.Select((a, i) => new
                {
                    Name = a
                });


                if (listExportWorkType.Count() > 1)
                {
                    sheet3.InsertRow(iRangeData2.Row + 1, listExportWorkType.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet3.ImportData(listExportWorkType, iRangeData.Row, iRangeData.Column, false);

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Import_Nhân viên_Template" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Import_Nhân viên_Template" + ".xlsx";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }
    }
}
