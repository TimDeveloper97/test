using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ProjectRole;
using NTS.Model.ProjectRole.ExportExcelModel;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Projects
{
    public class ProjectRoleBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<ProjectSearchRoleModel> Search(SearchRoleModel search)
        {
            SearchResultModel<ProjectSearchRoleModel> searchResult = new SearchResultModel<ProjectSearchRoleModel>();
            var dataQuerys = (from a in db.Roles.AsNoTracking()
                              orderby a.Index
                              select new ProjectSearchRoleModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Index = a.Index,
                                  Descipton = a.Descipton,
                                  IsDisable = a.IsDisable,
                              }).AsQueryable();
            if (!string.IsNullOrEmpty(search.Name))
            {
                dataQuerys = dataQuerys.Where(u => u.Name.ToUpper().Contains(search.Name.ToUpper()));
            }
            if (search.IsDisable != null)
            {
                if (search.IsDisable == 0)
                {
                    dataQuerys = dataQuerys.Where(u => !u.IsDisable);
                }

                if (search.IsDisable == 1)
                {
                    dataQuerys = dataQuerys.Where(u => u.IsDisable);
                }
            }
            searchResult.TotalItem = dataQuerys.Count();
            var listResult = SQLHelpper.OrderBy(dataQuerys, search.OrderBy, search.OrderType).Skip((search.PageNumber - 1) * search.PageSize).Take(search.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }
        public List<ProjectRoleModel> GetAll()
        {
            var roles = db.Roles.Select(x => new ProjectRoleModel
            {
                Id = x.Id,
                Name = x.Name,
                Index = x.Index,
                Descipton = x.Descipton,
                IsDisable = x.IsDisable,
                CreateBy = x.CreateBy,
                CreateDate = x.CreateDate,
                UpdateBy = x.UpdateBy,
                UpdateDate = x.UpdateDate,
            }).ToList();
            var returnRoles = roles.OrderBy(x => x.Index).ToList();
            return returnRoles;
        }
        public List<PlanFunctionResultModel> GetPlanFunctions()
        {
            return db.PlanFunctions.Select(x => new PlanFunctionResultModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Checked = false,
            }).ToList();
        }
        public SearchRoleResultModel SearchById(string id)
        {
            SearchRoleResultModel roleResult = new SearchRoleResultModel();
            List<PlanFunctionResultModel> planFunctions = new List<PlanFunctionResultModel>();
            if (!string.IsNullOrEmpty(id))
            {
                var role = db.Roles.Where(x => x.Id.Equals(id)).Select(y => new SearchRoleResultModel
                {
                    Id = y.Id,
                    Name = y.Name,
                    Index = y.Index,
                    Descipton = y.Descipton,
                    IsDisable = y.IsDisable,
                    CreateBy = y.CreateBy,
                    CreateDate = y.CreateDate,
                    UpdateBy = y.UpdateBy,
                    UpdateDate = y.UpdateDate,
                }).FirstOrDefault();

                var planFunction = (from a in db.PlanFunctions.AsNoTracking()
                                    select new PlanFunctionResultModel
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        Code = a.Code,
                                        Checked = db.PlanPermissions.Where(x => x.RoleId.Equals(id) && x.PlanFunctionId.Equals(a.Id)).Any()
                                    }).AsQueryable();
                foreach (var item in planFunction)
                {
                    planFunctions.Add(item);
                }
                roleResult = role;
                roleResult.PlanFunctions = planFunctions;
                return roleResult;
            }
            else
            {
                return null;
            }
        }

        public ProjectRoleModel GetById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var result = db.Roles.Where(y => y.Id.Equals(id)).Select(x => new ProjectRoleModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Index = x.Index,
                    Descipton = x.Descipton,
                    IsDisable = x.IsDisable,
                    CreateBy = x.CreateBy,
                    CreateDate = x.CreateDate,
                    UpdateBy = x.UpdateBy,
                    UpdateDate = x.UpdateDate,
                }).FirstOrDefault();
                return result;
            }
            else
            {
                return null;
            }
        }

        public string ExportExcel(SearchRoleModel model)
        {
            List<ExportExcelRoleResultModel> roles = new List<ExportExcelRoleResultModel>();
            List<string> planFunctions = new List<string>();

            var projectRoleModels = db.Roles.AsNoTracking().Select(x => new ExportExcelRoleResultModel
            {
                Id = x.Id,
                Name = x.Name,
                Index = x.Index,
                Descipton = x.Descipton,
                IsDisable = x.IsDisable,
            }).ToList();

            foreach (var role in projectRoleModels)
            {
                var planpers = db.PlanPermissions.Where(x => x.RoleId.Equals(role.Id)).Select(y => y.PlanFunctionId).ToList();
                foreach (var planper in planpers)
                {
                    var planf = db.PlanFunctions.Where(x => x.Id.Equals(planper)).Select(y => new PlanFunctionExportExcelResultModel { Name = y.Name, Code = y.Code }).FirstOrDefault();
                    planFunctions.Add(planf.Name);
                }
                role.PlanfunctionNames = planFunctions;
                planFunctions = new List<string>();
            }

            roles = projectRoleModels;

            if (!string.IsNullOrEmpty(model.Code))// tìm kiếm theo mã quyền
            {
                foreach(var role in roles)
                {
                    foreach(var plan in role.PlanFunctions)
                    {
                        if (!plan.Code.Equals(model.Code))
                        {
                            roles.Remove(role);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(model.Name)) // tìm kiếm theo tên quyền
            {
                 roles = roles.Where(x => x.PlanfunctionNames.Contains(model.Name)).ToList();
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProjectRole.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = roles.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = roles.OrderBy(i => i.Index).Select((a, i) => new
                {
                    Indexs = i + 1,
                    a.Name,
                    a.Descipton,
                    a = (a.IsDisable) ? "Đang sử dụng" : "Không dùng",
                    PlanFunction = String.Join(@" - ", a.PlanfunctionNames.ToArray()),
                });
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tổng hợp dữ liệu vị trí" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tổng hợp dữ liệu vị trí" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public void CreateRole(ProjectRoleModel model)
        {
            if (db.Roles.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Role);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                var indexs = db.Roles.ToList();
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listRole = db.Roles.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listRole.Count() > 0 && listRole != null)
                    {
                        foreach (var item in listRole)
                        {
                            var updateRole = db.Roles.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateRole.Index++;
                        }
                    }
                }
                try
                {
                    Role RoleNew = new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Index = model.Index,
                        Descipton = model.Descipton.Trim(),
                        IsDisable = model.IsDisable,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.Roles.Add(RoleNew);

                    foreach (var item in model.PlanFunctions)
                    {
                        if (item.Checked)
                        {
                            var PlanPermission = new PlanPermission
                            {
                                Id = Guid.NewGuid().ToString(),
                                RoleId = RoleNew.Id,
                                PlanFunctionId = item.Id,
                            };

                            db.PlanPermissions.Add(PlanPermission);
                        }
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, RoleNew.Name, RoleNew.Id, Constants.LOG_Unit);

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

        public void UpdateRole(ProjectRoleModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var checkRole = db.Roles.Where(b => b.Index == model.Index).FirstOrDefault();
                    if (checkRole != null)
                    {
                        var newRole = db.Roles.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        int oldInext = newRole.Index;

                        if (checkRole.Index < newRole.Index)
                        {
                            var listRoleChange1 = db.Roles.Where(a => a.Index > checkRole.Index && a.Index < newRole.Index);
                            if (listRoleChange1.Count() > 0)
                            {
                                foreach (var item in listRoleChange1)
                                {
                                    item.Index++;
                                }

                            }
                            checkRole.Index++;
                        }

                        if (checkRole.Index > newRole.Index)
                        {
                            var listRoleChange = db.Roles.Where(a => a.Index > newRole.Index && a.Index < checkRole.Index);
                            if (listRoleChange.ToList().Count() > 0)
                            {
                                foreach (var item in listRoleChange)
                                {
                                    item.Index--;
                                }
                            }
                            checkRole.Index = checkRole.Index - 1;
                        }
                        newRole.Index = model.Index;
                        newRole.IsDisable = model.IsDisable;
                        newRole.Name = model.Name.Trim();
                        newRole.Descipton = model.Descipton.Trim();
                        newRole.UpdateBy = model.UpdateBy;
                        newRole.UpdateDate = DateTime.Now;
                        var planPermission = db.PlanPermissions.Where(x => x.RoleId.Equals(model.Id)).ToList();
                        if (planPermission.Count > 0)
                        {
                            db.PlanPermissions.RemoveRange(planPermission);
                        }
                        foreach (var item in model.PlanFunctions)
                        {

                            if (item.Checked)
                            {
                                var PlanPermission = new PlanPermission
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    RoleId = model.Id,
                                    PlanFunctionId = item.Id,
                                };

                                db.PlanPermissions.Add(PlanPermission);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Id))
                        {
                            var role = db.Roles.FirstOrDefault(x => x.Id.Equals(model.Id));
                            if (role != null)
                            {
                                role.Name = model.Name;
                                role.Descipton = model.Descipton;
                                role.IsDisable = model.IsDisable;
                                role.CreateBy = model.CreateBy;
                                role.UpdateBy = model.UpdateBy;
                                role.UpdateDate = DateTime.Now;
                            }
                        }
                        var planPermission = db.PlanPermissions.Where(x => x.RoleId.Equals(model.Id)).ToList();
                        if (planPermission.Count > 0)
                        {
                            db.PlanPermissions.RemoveRange(planPermission);
                        }
                        foreach (var item in model.PlanFunctions)
                        {

                            if (item.Checked)
                            {
                                var PlanPermission = new PlanPermission
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    RoleId = model.Id,
                                    PlanFunctionId = item.Id,
                                };

                                db.PlanPermissions.Add(PlanPermission);
                            }
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
        public void DeleteRole(ProjectRoleModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var checkRole = (from a in db.ProjectEmployees.AsNoTracking()
                                 select a.RoleId)
                                 .Distinct()
                                 .ToList();

                var role = db.Roles.FirstOrDefault(x => x.Id.Equals(model.Id) && x.IsDisable.Equals(model.IsDisable));

                foreach (var item in checkRole)
                {
                    if(item == role.Id)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Role);
                    }
                    else
                    {
                        var rolePermission = db.PlanPermissions.Where(x => x.RoleId.Equals(role.Id)).ToList();
                        if (rolePermission.Count > 0)
                        {
                            db.PlanPermissions.RemoveRange(rolePermission);
                        }

                        var maxIndex = db.Roles.AsNoTracking().Select(a => a.Index).Max();

                        if (model.Index <= maxIndex)
                        {
                            int modelIndex = model.Index;
                            var listRole = db.Roles.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                            if (listRole.Count() > 0 && listRole != null)
                            {
                                foreach (var items in listRole)
                                {
                                    if (!items.Id.Equals(model.Id))
                                    {
                                        var updateRole = db.Roles.Where(r => r.Id.Equals(items.Id)).FirstOrDefault();
                                        updateRole.Index--;
                                    }

                                }
                            }
                        }
                    }
                }    

                try
                {
                    var role1 = db.Roles.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (role1 == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Role);
                    }
                    db.Roles.Remove(role1);

                    var NameOrCode = role1.Name;

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

        public object GetCbbRole()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Roles.AsNoTracking()
                                 orderby a.Index ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Index.ToString(),
                                     Index = a.Index
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxResult addFirstIndex = new ComboboxResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Exten = "1";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.Roles.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxResult addIndex = new ComboboxResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Exten = (maxIndex + 1).ToString();
                    addIndex.Index = maxIndex + 1;
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
    }
}
