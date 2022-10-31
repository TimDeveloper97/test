using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DesignDocuments;
using NTS.Model.Document;
using NTS.Model.GroupModule;
using NTS.Model.Module;
using NTS.Model.ModuleDesignDocument;
using NTS.Model.ModuleHistory;
using NTS.Model.ModuleManualDocument;
using NTS.Model.ModuleMaterials;
using NTS.Model.ModuleOldVersion;
using NTS.Model.ModulePart;
using NTS.Model.ModuleProductionTime;
using NTS.Model.ModuleProductStandard;
using NTS.Model.ProductStandards;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using NTS.Model.TestCriteria;
using OfficeOpenXml.ConditionalFormatting;
using QLTK.Business.AutoMappers;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.QLTKMODULE
{
    public class ModuleBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();

        /// <summary>
        /// Tìm kiếm module
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ModuleResultModel> SearchModule(ModuleSearchModel modelSearch)
        {
            SearchResultModel<ModuleResultModel> searchResult = new SearchResultModel<ModuleResultModel>();
            List<string> listParentId = new List<string>();

            try
            {
                var dataQuery = (from a in db.Modules.AsNoTracking()
                                 join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                                 join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                                 from ca in ac.DefaultIfEmpty()
                                 join d in db.SBUs.AsNoTracking() on ca.SBUId equals d.Id into cad
                                 from cnd in cad.DefaultIfEmpty()
                                 orderby a.Name
                                 select new ModuleResultModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ModuleGroupId = a.ModuleGroupId,
                                     ModuleGroupName = b.Name,
                                     ModuleGroupCode = b.Code,
                                     Description = a.Description,
                                     Note = a.Note,
                                     Status = a.Status,
                                     Specification = a.Specification,
                                     FileElectric = a.FileElectric,
                                     FileElectronic = a.FileElectronic,
                                     FileMechanics = a.FileMechanics,
                                     ElectricExist = a.ElectricExist,
                                     ElectronicExist = a.ElectronicExist,
                                     MechanicsExist = a.MechanicsExist,
                                     SoftwareExist = a.SoftwareExist,
                                     HMIExist = a.HMIExist,
                                     PLCExist = a.PLCExist,
                                     FilmExist = a.FilmExist,
                                     IsSoftware = a.IsSoftware,
                                     Pricing = a.Pricing.ToString(),
                                     IsUpdateFilm = a.IsFilm,
                                     IsHMI = a.IsHMI,
                                     IsPLC = a.IsPLC,
                                     IsDMTV = a.IsDMTV,
                                     CurrentVersion = a.CurrentVersion,
                                     Price = a.Pricing,
                                     Leadtime = a.Leadtime,
                                     CreateBy = a.CreateBy,
                                     CreateDate = a.CreateDate,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,
                                     IsEnought = (!a.FileElectric || (a.FileElectric && a.ElectricExist)) && (!a.FileElectronic || (a.FileElectronic && a.ElectronicExist)) && (!a.FileMechanics || (a.FileMechanics && a.MechanicsExist)) && (!a.IsHMI || (a.IsHMI && a.HMIExist)) && (!a.IsPLC || (a.IsPLC && a.PLCExist)) && (!a.IsSoftware || (a.IsSoftware && a.SoftwareExist)) && (!a.IsFilm || (a.IsFilm && a.FilmExist)),
                                     DepartmentId = ca != null ? ca.Id : string.Empty,
                                     DepartmentName = ca != null ? ca.Name : string.Empty,
                                     SBUId = cnd != null ? cnd.Id : string.Empty,
                                     SBUName = cnd != null ? cnd.Name : string.Empty,
                                     IsManual = a.IsManual,
                                     ManualExist = a.ManualExist,
                                     IsSendSale = a.IsSendSale,
                                     SyncDate = a.SyncDate
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.SBUId))
                {
                    dataQuery = dataQuery.Where(a => a.SBUId.Equals(modelSearch.SBUId));
                }

                if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
                {
                    dataQuery = dataQuery.Where(a => a.DepartmentId.Equals(modelSearch.DepartmentId));
                }

                // tìm kiếm theo mã nhóm    
                if (!string.IsNullOrEmpty(modelSearch.ModuleGroupId))
                {
                    var moduleGroups = db.ModuleGroups.AsNoTracking().ToList();
                    var moduleGroup = moduleGroups.FirstOrDefault(i => i.Id.Equals(modelSearch.ModuleGroupId));
                    if (moduleGroup != null)
                    {
                        listParentId.Add(moduleGroup.Id);
                    }

                    listParentId.AddRange(GetListParent(modelSearch.ModuleGroupId, moduleGroups));
                    var listModuleGroup = listParentId.ToList();
                    //dataQuery = (from a in dataQuery
                    //             join b in listModuleGroup.AsQueryable() on a.ModuleGroupId equals b
                    //             select a).AsQueryable();

                    dataQuery = dataQuery.Where(r => listModuleGroup.Contains(r.ModuleGroupId));
                }

                // tìm kiếm theo tên module
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }

                // tìm kiếm theo mã module
                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || r.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
                }
                //if (modelSearch.IsSendSale != null)
                //{
                //    dataQuery = dataQuery.Where(a => a.IsSendSale.Equals(modelSearch.IsSendSale));
                //}

                // tìm kiếm theo mã module
                if (!string.IsNullOrEmpty(modelSearch.Note))
                {
                    dataQuery = dataQuery.Where(r => !string.IsNullOrEmpty(r.Note) && r.Note.ToUpper().Contains(modelSearch.Note.ToUpper()));
                }

                // tìm kiếm theo tình trạng sử dụng
                if (!string.IsNullOrEmpty(modelSearch.Status.ToString()))
                {
                    dataQuery = dataQuery.Where(r => r.Status == modelSearch.Status);
                }

                // Trạng thái sử dụng
                if (modelSearch.IsEnought != null)
                {
                    if (modelSearch.IsEnought == 0)
                    {
                        dataQuery = dataQuery.Where(u => !u.IsEnought);
                    }

                    if (modelSearch.IsEnought == 1)
                    {
                        dataQuery = dataQuery.Where(u => u.IsEnought);
                    }
                }

                if (modelSearch.File != null)
                {
                    if (modelSearch.File.Equals("0"))
                    {
                        dataQuery = dataQuery.Where(r => r.FileMechanics == true /*&& r.MechanicsExist == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.MechanicsExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.MechanicsExist == true);
                        }
                    }

                    if (modelSearch.File.Equals("1"))
                    {
                        dataQuery = dataQuery.Where(r => r.FileElectric == true /*&& r.FileElectric == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.ElectricExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.ElectricExist == true);
                        }
                    }

                    if (modelSearch.File.Equals("2"))
                    {
                        dataQuery = dataQuery.Where(r => r.FileElectronic == true /*&& r.FileElectronic == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.ElectronicExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.ElectronicExist == true);
                        }
                    }

                    if (modelSearch.File.Equals("3"))
                    {
                        dataQuery = dataQuery.Where(r => r.IsUpdateFilm == true /*&& r.FilmExist == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.FilmExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.FilmExist == true);
                        }
                    }

                    if (modelSearch.File.Equals("4"))
                    {
                        dataQuery = dataQuery.Where(r => r.IsHMI == true /*&& r.HMIExist == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.HMIExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.HMIExist == true);
                        }
                    }

                    if (modelSearch.File.Equals("5"))
                    {
                        dataQuery = dataQuery.Where(r => r.IsPLC == true /*&& r.PLCExist == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.PLCExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.PLCExist == true);
                        }
                    }

                    if (modelSearch.File.Equals("6"))
                    {
                        dataQuery = dataQuery.Where(r => r.IsSoftware/* == true && r.SoftwareExist == true*/);
                        if (modelSearch.IsEnought == 0)
                        {
                            dataQuery = dataQuery.Where(u => u.SoftwareExist == false);
                        }
                        if (modelSearch.IsEnought == 1)
                        {
                            dataQuery = dataQuery.Where(u => u.SoftwareExist == true);
                        }
                    }
                }

                //var data = dataQuery.ToList();
                //foreach (var item in data)
                //{
                //    item.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
                //}

                searchResult.TotalItem = dataQuery.Count();
                searchResult.Status5 = searchResult.TotalItem > 0 ? dataQuery.Max(i => i.Price) : 0;
                searchResult.Status1 = searchResult.TotalItem > 0 ? dataQuery.Max(i => i.Leadtime) : 0;
                List<ModuleResultModel> listResult = new List<ModuleResultModel>();

                if (!modelSearch.IsExport)
                {
                    searchResult.Date = dataQuery.Max(i => i.SyncDate);
                    listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                }
                else
                {
                    listResult = dataQuery.OrderBy(i => i.Code).ToList();
                }

                var errors = db.Errors.AsNoTracking().Where(t => t.Type == Constants.Error_Type_Error && t.Status != Constants.Problem_Status_Creating && t.Status != Constants.Problem_Status_Awaiting_Confirm).Select(s => s.ObjectId).ToList();
                foreach (var item in listResult)
                {
                    item.ListModelError = errors.Count(t => item.Id.Equals(t));
                    item.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
                }

                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách list tiêu chí
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<TestCriteriaResultModel> GetListTestCriteria(TestCriterSearchModel modelSearch)
        {
            SearchResultModel<TestCriteriaResultModel> searchResult = new SearchResultModel<TestCriteriaResultModel>();
            try
            {
                var dataQuery = (from a in db.Modules.AsNoTracking()
                                 join b in db.ModuleTestCriterias.AsNoTracking() on a.Id equals b.ModuleId
                                 join c in db.TestCriterias.AsNoTracking() on b.TestCriteriaId equals c.Id
                                 join d in db.TestCriteriaGroups.AsNoTracking() on c.TestCriteriaGroupId equals d.Id
                                 where a.Id == b.ModuleId && b.TestCriteriaId == c.Id
                                 orderby a.Code
                                 select new TestCriteriaResultModel
                                 {
                                     Id = a.Id,
                                     TestCriteriaGroupId = c.TestCriteriaGroupId,
                                     TestCriteriaGroupName = d.Name,
                                     Name = a.Name,
                                     Code = a.Code,
                                     TechnicalRequirements = c.TechnicalRequirements,
                                     Note = a.Note,
                                     CreateDate = a.CreateDate,
                                     CreateBy = a.CreateBy,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,
                                 }).AsQueryable();
                searchResult.ListResult = dataQuery.ToList();
                searchResult.TotalItem = searchResult.ListResult.Count;

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return searchResult;
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string ExportExcel(ModuleSearchModel model)
        {
            model.IsExport = true;

            var data = SearchModule(model);
            List<ModuleResultModel> listModel = data.ListResult;

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Module.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    View9 = a.Status == 1 ? "Chỉ sử dụng một lần" : a.Status == 2 ? "Module chuẩn" : a.Status == 3 ? "Module ngừng sử dụng" : "",
                    a.Code,
                    a.Name,
                    a.Description,
                    a.ModuleGroupName,
                    ListModelError = db.Errors.AsNoTracking().Where(t => t.ObjectId.Equals(a.Id)).Select(m => new NTS.Model.ModuleError.ModuleErrorModel
                    {
                        Id = m.Id,
                        ObjectId = m.ObjectId,
                    }).ToList().Count,
                    view = a.IsEnought ? "Đã đủ" : "Chưa đủ",
                    viewDMVT = a.IsDMTV == true ? "Có" : "Không",
                    // TSKT
                    view1 = !string.IsNullOrEmpty(a.Specification) ? "Có - Đủ" : string.IsNullOrEmpty(a.Specification) ? "Có - Chưa đủ" : "Không",
                    // TK Cơ khí
                    view2 = a.MechanicsExist == false && a.FileMechanics == true ? "Có - Chưa đủ" : a.MechanicsExist == true && a.FileMechanics == true ? "Có - Đủ" : "Không",
                    // TK điện
                    view3 = a.ElectricExist == true && a.FileElectric == true ? "Có - Đủ" : a.ElectricExist == false && a.FileElectric == true ? "Có - Chưa đủ" : "Không",
                    // TK điện tư
                    View4 = a.ElectronicExist == true && a.FileElectronic == true ? "Có - Đủ" : a.ElectronicExist == false && a.FileElectronic == true ? "Có - Chưa đủ" : "Không",
                    // In Film
                    View5 = a.FilmExist == true && a.IsUpdateFilm == true ? "Có - Đủ" : a.FilmExist == false && a.IsUpdateFilm == true ? "Có - Chưa đủ" : "Không",
                    view6 = a.HMIExist == true && a.IsHMI == true ? "Có - Đủ" : a.HMIExist == false && a.IsHMI == true ? "Có - Chưa đủ" : "Không",
                    view7 = a.PLCExist == true && a.IsPLC == true ? "Có - Đủ" : a.PLCExist == false && a.IsPLC == true ? "Có - Chưa đủ" : "Không",
                    view8 = a.SoftwareExist == true && a.IsSoftware == true ? "Có - Đủ" : a.SoftwareExist == false && a.IsSoftware == true ? "Có - Chưa đủ" : "Không",
                    view9 = a.ManualExist == true && a.IsManual == true ? "Có - Đủ" : a.ManualExist == false && a.IsManual == true ? "Có - Chưa đủ" : "Không",
                    a.CurrentVersion,
                    a.Price,
                    a.Leadtime,
                    SheetView = DateTimeHelper.ToStringDDMMYY(a.UpdateDate)
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách module" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách module" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        /// <summary>
        /// Xóa module
        /// </summary>
        /// <param name="model"></param>
        public void DeleteModul(ModuleModel model, string departmentIdLogin)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var sketchMaterialMechanical = db.SketchMaterialMechanicals.AsNoTracking().Where(m => m.ModuleId.Equals(model.Id)).FirstOrDefault();
                if (sketchMaterialMechanical != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Module);
                }

                var error = db.Errors.AsNoTracking().Where(m => m.ObjectId.Equals(model.Id)).FirstOrDefault();
                if (error != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Module);
                }

                try
                {
                    var modulDelete = db.Modules.FirstOrDefault(o => o.Id.Equals(model.Id));
                    if (!modulDelete.DepartmentId.Equals(departmentIdLogin))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.Module);
                    }
                    if (modulDelete != null)
                    {
                        var userGuide = db.UserGuides.AsNoTracking().Where(o => modulDelete.Id.Equals(o.ModuleId)).Count();
                        var moduleInPractice = db.ModuleInPractices.AsNoTracking().Where(o => modulDelete.Id.Equals(o.ModuleId)).Count();
                        var component = db.Components.AsNoTracking().Where(o => modulDelete.Id.Equals(o.ModuleId)).Count();
                        var moduleInProduct = db.ModuleInProducts.AsNoTracking().Where(o => modulDelete.Id.Equals(o.ModuleId)).Count();
                        if (moduleInPractice > 0 || userGuide > 0 || component > 0 || moduleInProduct > 0)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Module);
                        }
                        else
                        {
                            db.Modules.Remove(modulDelete);

                            var NameOrCode = modulDelete.Code;

                            //var jsonBefor = AutoMapperConfig.Mapper.Map<ModuleHistoryModel>(modulDelete);
                            //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Module, modulDelete.Id, NameOrCode, jsonBefor);

                            db.SaveChanges();
                            trans.Commit();
                        }

                    }
                    else
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                    }
                }
                catch (NTSException ntsEx)
                {
                    trans.Rollback();
                    throw ntsEx;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            //luu Log lich su
            string decription = "Xóa nhóm module tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        /// <summary>
        /// Kiểm tra thêm mới module
        /// </summary>
        /// <param name="moduleModel"></param>
        public void CheckInputAdd(ModuleModel moduleModel)
        {
            moduleModel.Code = Util.RemoveSpecialCharacter(moduleModel.Code);
            //if (db.Modules.AsNoTracking().Where(o => o.Name.Equals(moduleModel.Name)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Module);
            //}

            if (db.Modules.AsNoTracking().Where(o => o.Code.Equals(moduleModel.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Module);
            }
        }

        /// <summary>
        /// Thêm mới module
        /// </summary>
        /// <param name="moduleModel"></param>
        public string AddModule(ModuleModel moduleModel)
        {
            CheckInputAdd(moduleModel);
            moduleModel.Code = Util.RemoveSpecialCharacter(moduleModel.Code);
            string moduleId = Guid.NewGuid().ToString();

            var moduleGroupCode = db.ModuleGroups.AsNoTracking().FirstOrDefault(a => a.Id.Equals(moduleModel.ModuleGroupId)).Code;
            if (!moduleModel.Code.StartsWith(moduleGroupCode))
            {
                throw NTSException.CreateInstance("Mã module phải trùng với Mã nhóm module");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    Module newModule = new Module()
                    {
                        Id = moduleId,
                        ModuleGroupId = moduleModel.ModuleGroupId,
                        Name = moduleModel.Name,
                        Code = moduleModel.Code,
                        Note = moduleModel.Note,
                        Status = moduleModel.Status,
                        State = moduleModel.State,
                        Specification = moduleModel.Specification,
                        Pricing = moduleModel.Pricing,
                        FileElectric = moduleModel.FileElectric,
                        FileElectronic = moduleModel.FileElectronic,
                        FileMechanics = moduleModel.FileMechanics,
                        IsSoftware = moduleModel.IsSoftware,
                        IsHMI = moduleModel.IsHMI,
                        IsPLC = moduleModel.IsPLC,
                        IsDMTV = moduleModel.IsDMTV,
                        CurrentVersion = moduleModel.CurrentVersion,
                        ThumbnailPath = moduleModel.ThumbnailPath,
                        IsFilm = moduleModel.IsFilm,
                        Description = moduleModel.Description,
                        EditContent = moduleModel.EditContent,
                        DepartmentId = moduleModel.DepartmentCreated,
                        IsManual = moduleModel.IsManual,
                        ManualExist = moduleModel.ManualExist,

                        CreateBy = moduleModel.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = moduleModel.CreateBy
                    };

                        db.Modules.Add(newModule);                  

                    if (moduleModel.ListStage.Count() > 0)
                    {
                        foreach (var item in moduleModel.ListStage)
                        {
                            ModuleProductionTime moduleProductionTime = new ModuleProductionTime()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleId = newModule.Id,
                                StageId = item.StageId,
                                ExecutionTime = item.Time
                            };
                            db.ModuleProductionTimes.Add(moduleProductionTime);
                        }
                        newModule.Leadtime = moduleModel.ListStage.Sum(a => a.ExecutionTime);
                    }


                    foreach (var item in moduleModel.ListModuleManualDocument)
                    {
                        if (!item.IsDocument)
                        {
                            ModuleManualDocument moduleManualDocument = new ModuleManualDocument()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleId = newModule.Id,
                                Path = item.Path,
                                FileName = item.FileName,
                                CreateDate = dateNow,
                                FileSize = item.FileSize,
                                Note = item.Note,
                                FileType = item.FileType,
                            };
                            db.ModuleManualDocuments.Add(moduleManualDocument);
                            newModule.ManualExist = true;
                        }
                        else
                        {
                            DocumentObject documentObject = new DocumentObject()
                            {
                                Id = Guid.NewGuid().ToString(),
                                DocumentId = item.Id,
                                ObjectId = newModule.Id,
                                ObjectType = Constants.ObjectType_Module
                            };
                            db.DocumentObjects.Add(documentObject);
                            newModule.ManualExist = true;
                        }
                    }

                    if (moduleModel.ListModuleManualDocument.Count > 0)
                    {
                        var checkExits = moduleModel.ListModuleManualDocument.Where(a => a.FileName.Contains(newModule.Code)).ToList();
                        if (checkExits.Count > 0)
                        {
                            newModule.ManualExist = true;
                        }
                        else
                        {
                            newModule.ManualExist = true;
                        }
                    }

                    foreach (var item in moduleModel.ListImage)
                    {
                        ModuleImage moduleImage = new ModuleImage()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ModuleId = newModule.Id,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            ThumbnailPath = item.ThumbnailPath,
                            Note = item.Note,
                            CreateBy = newModule.CreateBy,
                            CreateDate = DateTime.Now,
                        };
                        db.ModuleImages.Add(moduleImage);
                    }
                    //AddModulePart(moduleModel, newModule.Id);
                    UserLogUtil.LogHistotyAdd(db, moduleModel.CreateBy, moduleModel.Code, moduleModel.Id, Constants.LOG_Module);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(moduleModel, ex);
                }
            }
            try
            {
                //luu Log lich su
                string decription = String.Empty;
                decription = "Thêm module có tên là: " + moduleModel.Name;
                LogBusiness.SaveLogEvent(db, moduleModel.CreateBy, decription);
            }
            catch (Exception) { }

            return moduleId;
        }

        /// <summary>
        /// Tìm kiếm theo Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<TestCriteriaModel> GetModuleId(string Id)
        {
            List<TestCriteriaModel> search = new List<TestCriteriaModel>();
            List<TestCriteriaModel> searchResult = new List<TestCriteriaModel>();
            try
            {

                var dataQuery = (from o in db.Modules.AsNoTracking().OrderBy(r => r.Name)
                                 select new TestCriteriaModel
                                 {
                                     Id = o.Id,
                                     Note = o.Note,
                                     Name = o.Name,
                                     CreateBy = o.CreateBy,
                                     CreateDate = o.CreateDate,
                                     UpdateBy = o.UpdateBy,
                                     UpdateDate = o.UpdateDate,
                                     IsSendSale = o.IsSendSale
                                 }).AsQueryable();
                List<TestCriteriaModel> listResult = new List<TestCriteriaModel>();
                var listParent = dataQuery.ToList().Where(r => r.Id.Equals(Id)).ToList();
                search = listResult;
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return search.ToList();
        }

        //Danh sách tiêu chí
        public object GetTestCriteria(TestCriterSearchModel modelSearch)
        {
            SearchResultModel<TestCriteriaModel> searchResult = new SearchResultModel<TestCriteriaModel>();

            var dataQuery = (from a in db.TestCriterias.AsNoTracking()
                             join b in db.TestCriteriaGroups.AsNoTracking() on a.TestCriteriaGroupId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new TestCriteriaModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 TestCriteriaGroupId = a.TestCriteriaGroupId,
                                 TestCriteriaGroupName = b.Name,
                                 TechnicalRequirements = a.TechnicalRequirements,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 DataType = a.DataType
                             }).AsQueryable();

            //if (!string.IsNullOrEmpty(modelSearch.Name))
            //{
            //    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            //}

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.TestCriteriaGroupId))
            {
                dataQuery = dataQuery.Where(u => u.TestCriteriaGroupId.ToUpper().Contains(modelSearch.TestCriteriaGroupId.ToUpper()));
            }

            if (modelSearch.DataType.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.DataType == modelSearch.DataType.Value);
            }

            searchResult.TotalItem = dataQuery.Count();
            //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void AddTestCriteria(ModuleModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var oldTestCriteria = db.ModuleTestCriterias.Where(a => a.ModuleId.Equals(model.ModuleId));
                if (oldTestCriteria != null)
                {
                    db.ModuleTestCriterias.RemoveRange(oldTestCriteria);
                }
                try
                {
                    if (model.ListTestCriteriaModule != null)
                    {
                        foreach (var item in model.ListTestCriteriaModule)
                        {
                            ModuleTestCriteria newModuleTestCriteria = new ModuleTestCriteria
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleId = model.ModuleId,
                                TestCriteriaId = item,
                            };

                            db.ModuleTestCriterias.Add(newModuleTestCriteria);
                        }

                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Module, model.ModuleId, string.Empty, "Tiêu chí");

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

        public object GetTestCriteriaInfo(ModuleModel model)
        {
            var resultInfo = (from a in db.Modules.AsNoTracking()
                              select new ModuleModel
                              {
                                  Id = a.Id,
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Sketches);
            }

            return resultInfo;
        }

        public object SearchTestCriteria(ModuleModel modelSearch)
        {
            SearchResultModel<TestCriteriaModel> searchResult = new SearchResultModel<TestCriteriaModel>();
            try
            {
                var testCriteria = db.ModuleTestCriterias.AsNoTracking().Where(a => a.ModuleId.Equals(modelSearch.Id));
                List<string> listTestCriteriaId = new List<string>();
                if (testCriteria != null)
                {
                    foreach (var item in testCriteria)
                    {
                        listTestCriteriaId.Add(item.TestCriteriaId);
                    }
                }

                var dataQuery = (from a in db.TestCriterias.AsNoTracking()
                                 join b in db.TestCriteriaGroups.AsNoTracking() on a.TestCriteriaGroupId equals b.Id
                                 where listTestCriteriaId.Contains(a.Id)
                                 orderby a.Name
                                 select new TestCriteriaModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                     Note = a.Note,
                                     TestCriteriaGroupId = a.TestCriteriaGroupId,
                                     TestCriteriaGroupName = b.Name,
                                     TechnicalRequirements = a.TechnicalRequirements,
                                     CreateDate = a.CreateDate,
                                     CreateBy = a.CreateBy,
                                     UpdateBy = a.UpdateBy,
                                     UpdateDate = a.UpdateDate,

                                 }).AsQueryable();


                searchResult.TotalItem = dataQuery.Count();
                searchResult.ListResult = dataQuery.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách tiêu chuẩn
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<ProductStandardsModel> SearchProductStandard(ProductStandardsSearchModel modelSearch)
        {
            SearchResultModel<ProductStandardsModel> searchResult = new SearchResultModel<ProductStandardsModel>();
            var dataQuey = (from a in db.ProductStandards.AsNoTracking()
                            join b in db.ProductStandardGroups.AsNoTracking() on a.ProductStandardGroupId equals b.Id
                            join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id
                            join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                            orderby a.Code
                            select new ProductStandardsModel
                            {
                                Id = a.Id,
                                ProductStandardGroupId = a.ProductStandardGroupId,
                                ProductStandardGroupName = b.Name,
                                SBUId = a.SBUId,
                                SBUName = c.Name,
                                DepartmentId = a.DepartmentId,
                                DepartmentName = d.Name,
                                Code = a.Code,
                                Name = a.Name,
                                Content = a.Content,
                                Target = a.Target,
                                Note = a.Note,
                                Version = a.Version,
                                EditContent = a.EditContent,
                                CreateBy = a.CreateBy,
                                CreateDate = a.CreateDate,
                                UpdateBy = a.UpdateBy,
                                UpdateDate = a.UpdateDate,
                            }).AsQueryable();
            searchResult.TotalItem = dataQuey.Count();
            var listResult = dataQuey.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// Xuất excel danh sách tiêu chí
        /// </summary>
        /// <param name="moduleModel"></param>
        /// <param name="newmoduleId"></param>
        public string ExportExcelCriteria(TestCriterSearchModel model)
        {
            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ModuleId));

            if (module == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }

            model.IsExport = true;
            var dataQuery = (from a in db.Modules.AsNoTracking()
                             join b in db.ModuleTestCriterias.AsNoTracking() on a.Id equals b.ModuleId
                             join c in db.TestCriterias.AsNoTracking() on b.TestCriteriaId equals c.Id
                             join d in db.TestCriteriaGroups.AsNoTracking() on c.TestCriteriaGroupId equals d.Id
                             where b.ModuleId.Equals(model.ModuleId)
                             orderby c.Name
                             select new TestCriteriaResultModel
                             {
                                 Id = a.Id,
                                 ModuleCode = a.Code,
                                 TestCriteriaGroupId = c.TestCriteriaGroupId,
                                 TestCriteriaGroupName = d.Name,
                                 Name = c.Name,
                                 Code = c.Code,
                                 TechnicalRequirements = c.TechnicalRequirements,
                                 Note = c.Note,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                             }).AsQueryable();

            List<TestCriteriaResultModel> listModel = dataQuery.ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0015, TextResourceKey.TestCriteria);
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/listCriteria.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = listModel.Count;

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = listModel.Select((a, i) => new
            {
                Index = i + 1,
                a.ModuleCode,
                a.TestCriteriaGroupName,
                a.Name,
                a.Code,
                a.TechnicalRequirements,
                a.Note,
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

            string resultPathClient = "Template/" + Constants.FolderExport + module.Code + "Danh sách tiêu chí.xlsx";

            string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return resultPathClient;

        }

        /// <summary>
        /// Lấy tiêu chí còn lại.
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public List<TestCriteriaModel> GetModuleGroupExcepted(TestCriterSearchModel modelSearch)
        {
            List<TestCriteriaModel> listExpected = new List<TestCriteriaModel>();
            List<TestCriteriaModel> listAll = new List<TestCriteriaModel>();
            List<TestCriteriaModel> listById = new List<TestCriteriaModel>();
            listById = GetModuleId(modelSearch.Id);

            foreach (var itemAll in listAll)
            {
                foreach (var itemById in listById)
                {
                    if (itemAll.Id.Equals(itemById.Id))
                    {
                        listExpected.Add(itemAll);
                    }
                }
            }
            return listAll.Except(listExpected).ToList();

        }

        //Add module part
        public void AddModulePart(ModuleModel moduleModel, string newmoduleId)
        {
            moduleModel.ListModulePartModel.ToList();
            if (moduleModel.ListModulePartModel != null && moduleModel.ListModulePartModel.Count > 0)
            {
                ModulePart newModulePart;
                foreach (var item in moduleModel.ListModulePartModel)
                {
                    newModulePart = new ModulePart();
                    newModulePart.Id = Guid.NewGuid().ToString();
                    newModulePart.ModuleId = newmoduleId;
                    newModulePart.ModuleItemId = item.ModuleItemId;
                    newModulePart.ModuleName = item.ModuleName;
                    newModulePart.ModuleCode = item.ModuleCode;
                    newModulePart.Qty = item.Qty;
                    db.ModuleParts.Add(newModulePart);
                }
            }
        }

        public void AddModuleProductStandard(ModuleProductStandardModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var moduleProductStandard = db.ModuleProductStandards.Where(a => a.ModuleId.Equals(model.ModuleId));
                if (moduleProductStandard != null)
                {
                    db.ModuleProductStandards.RemoveRange(moduleProductStandard);
                }
                try
                {
                    if (model.List != null)
                    {
                        foreach (var item in model.List)
                        {
                            ModuleProductStandard productStandard = new ModuleProductStandard
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductStandardId = item,
                                ModuleId = model.ModuleId,
                                ModuleGroupId = model.ModuleGroupId,
                            };
                            db.ModuleProductStandards.Add(productStandard);
                        }
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Module, model.ModuleId, string.Empty, "Tiêu chuẩn");

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

        public object GetModuleProductStandardInfo(ModuleProductStandardModel model)
        {
            var data = (from a in db.ModuleProductStandards.AsNoTracking()
                        join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                        join c in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals c.Id
                        where a.ModuleId.Equals(model.ModuleId)
                        orderby c.Code
                        select new ModuleProductStandardModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            ProductStandardId = a.ProductStandardId
                        }).ToList();
            model.ListProductStandard = data;
            return model;
        }

        public object GetModuleTestCriteriInfo(TestCriteriModel model)
        {
            var data = (from a in db.ModuleTestCriterias.AsNoTracking()
                        join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                        join c in db.TestCriterias.AsNoTracking() on a.TestCriteriaId equals c.Id
                        where a.ModuleId.Equals(model.Id)
                        orderby c.Code
                        select new TestCriteriModel
                        {
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            TestCriteriasId = a.TestCriteriaId
                        }).ToList();

            model.ListTestCriteri = data;

            return model;
        }

        public ModuleModel GetModuleInfo(ModuleModel model)
        {
            var module = db.Modules.AsNoTracking().FirstOrDefault(o => o.Id.Equals(model.Id));
            if (string.IsNullOrEmpty(model.Id))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }
            if (model == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }
            try
            {
                model.Creator = (from a in db.Users.AsNoTracking()
                                 join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id into ab
                                 from anb in ab.DefaultIfEmpty()
                                 where a.Id.Equals(module.CreateBy)
                                 select anb.Name).FirstOrDefault();
                var department = db.Departments.AsNoTracking().Where(a => a.Id.Equals(module.DepartmentId)).FirstOrDefault();
                model.DepartmentCreated = department != null ? department.Name : string.Empty;
                model.ModuleGroupId = module.ModuleGroupId;
                model.Name = module.Name;
                model.Code = module.Code;
                model.Note = module.Note;
                model.Status = module.Status;
                model.State = module.State;
                model.Specification = module.Specification;
                model.Pricing = module.Pricing;
                model.FileElectric = module.FileElectric;
                model.ElectricExist = module.ElectricExist;
                model.FileElectronic = module.FileElectronic;
                model.ElectronicExist = module.ElectronicExist;
                model.FileMechanics = module.FileMechanics;
                model.MechanicsExist = module.MechanicsExist;
                model.IsSoftware = module.IsSoftware;
                model.SoftwareExist = module.SoftwareExist;
                model.IsHMI = module.IsHMI;
                model.HMIExist = module.HMIExist;
                model.IsPLC = module.IsPLC;
                model.PLCExist = module.PLCExist;
                model.CurrentVersion = module.CurrentVersion;
                model.ThumbnailPath = module.ThumbnailPath;
                model.IsFilm = module.IsFilm;
                model.FilmExist = module.FilmExist;
                model.Description = module.Description;
                model.EditContent = module.EditContent;
                model.CreateBy = module.CreateBy;
                model.CreateDate = module.CreateDate;
                model.UpdateBy = module.UpdateBy;
                model.UpdateDate = module.UpdateDate;
                model.ManualExist = module.ManualExist;
                model.IsManual = module.IsManual;

                var listStage = (from a in db.ModuleProductionTimes.AsNoTracking()
                                 join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                                 where a.ModuleId.Equals(module.Id)
                                 orderby b.Name
                                 select new ModuleProductionTimeModel
                                 {
                                     Id = a.Id,
                                     ModuleId = a.ModuleId,
                                     StageId = a.StageId,
                                     Time = a.ExecutionTime,
                                     Name = b.Name
                                 }
                               ).ToList();

                model.ListStage = listStage;

                var listModuleManualDocument = (from a in db.ModuleManualDocuments.AsNoTracking()
                                                where a.ModuleId.Equals(module.Id) && a.FileType.Equals(1)
                                                select new ModuleManualDocumentModel
                                                {
                                                    Id = a.Id,
                                                    ModuleId = a.ModuleId,
                                                    Path = a.Path,
                                                    FileName = a.FileName,
                                                    CreateDate = a.CreateDate,
                                                    FileSize = a.FileSize,
                                                    Note = a.Note,
                                                    FileType = a.FileType
                                                }).ToList();

                listModuleManualDocument.AddRange((from a in db.DocumentObjects.AsNoTracking()
                                                   join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                                   where a.ObjectId.Equals(module.Id) && a.ObjectType == Constants.ObjectType_Module
                                                   select new ModuleManualDocumentModel
                                                   {
                                                       Id = b.Id,
                                                       FileName = b.Name,
                                                       Note = b.Description,
                                                       IsDocument = true
                                                   }).ToList());

                model.ListModuleManualDocument = listModuleManualDocument;
                if (listModuleManualDocument.Count > 0)
                {
                    var checkExits = listModuleManualDocument.Where(a => a.FileName.Contains(model.Code)).ToList();
                    if (checkExits.Count > 0)
                    {
                        model.ManualExist = true;
                    }
                    else
                    {
                        model.ManualExist = false;
                    }
                }

                var listImage = (from a in db.ModuleImages.AsNoTracking()
                                 where a.ModuleId.Equals(module.Id)
                                 select new ModuleImageModel
                                 {
                                     FileName = a.FileName,
                                     FilePath = a.FilePath,
                                     ThumbnailPath = a.ThumbnailPath,
                                     Note = a.Note,
                                 }).ToList();

                model.ListImage = listImage;

                //Lấy List version
                var listHistory = (from a in db.ModuleOldVersions.AsNoTracking()
                                   join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                   join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                   where a.ModuleId.Equals(model.Id)
                                   orderby a.CreateDate descending
                                   select new ModuleOldVersionModel
                                   {
                                       Id = a.Id,
                                       ModuleId = a.ModuleId,
                                       Content = a.Content,
                                       Version = a.Version.ToString(),
                                       Description = a.Description,
                                       CreateBy = a.CreateBy,
                                       CreateByName = c.Name,
                                       CreateDate = a.CreateDate
                                   }).ToList();

                model.ListHistory = listHistory;

                model.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(model.Id, 0);

                if (model.FileMechanics && model.FileElectronic)
                {
                    var materialPCB = db.ModuleMaterials.AsNoTracking().Where(r => r.ModuleId.Equals(model.Id) && r.MaterialCode.StartsWith("PCB")).GroupBy(g => g.MaterialCode).Select(s => s.Key).ToList();

                    var modulePCB = (from m in db.Modules.AsEnumerable()
                                     join p in materialPCB on m.Code.ToUpper() equals p.ToUpper()
                                     where m.FileElectronic && m.ElectronicExist
                                     select m).ToList();

                    if (modulePCB.Count == materialPCB.Count)
                    {
                        model.ElectronicExist = true;
                    }

                }

                if ((!model.FileElectric || (model.FileElectric && model.ElectricExist)) &&
                    (!model.FileElectronic || (model.FileElectronic && model.ElectronicExist)) &&
                    (!model.FileMechanics || (model.FileMechanics && model.MechanicsExist)) &&
                    (!model.IsSoftware || (model.IsSoftware && model.SoftwareExist)) &&
                    (!model.IsHMI || (model.IsHMI && model.HMIExist)) &&
                    (!model.IsPLC || (model.IsPLC && model.PLCExist)) &&
                    (!model.IsFilm || (model.IsFilm && model.FilmExist)))
                {
                    model.DataStatus = true;
                }
                else
                {
                    model.DataStatus = false;
                }

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ModulePartModel> GetListModulePart(ModuleModel model)
        {
            List<ModulePartModel> listModulePart = new List<ModulePartModel>();
            try
            {
                if (!string.IsNullOrEmpty(model.Id))
                {
                    listModulePart = (from a in db.ModuleParts.AsNoTracking()
                                      where a.ModuleId.Equals(model.Id)
                                      select new ModulePartModel
                                      {
                                          Id = a.Id,
                                          ModuleItemId = a.ModuleItemId,
                                          ModuleName = a.ModuleName,
                                          ModuleCode = a.ModuleCode,
                                          Qty = a.Qty,
                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QLTK.ErrosProcess");
            }
            return listModulePart;
        }

        public void CheckInputUpdate(ModuleModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            var groupEdit = db.Modules.AsNoTracking().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
            //if (db.Modules.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Module);
            //}
            if (db.Modules.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Code.Equals(o.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Module);
            }
            //if (groupEdit.CurrentVersion > model.CurrentVersion)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0011, TextResourceKey.Module);
            //}
        }

        public void UpdateModule(ModuleModel model, string departmentIdLogin)
        {
            string NameOld = "";
            CheckInputUpdate(model);
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            var groupEdit = db.Modules.Where(o => model.Id.Equals(o.Id)).FirstOrDefault();

            //var jsonApter = AutoMapperConfig.Mapper.Map<ModuleHistoryModel>(groupEdit);

            if (groupEdit == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }
            if (!groupEdit.DepartmentId.Equals(departmentIdLogin))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0029, TextResourceKey.Module);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    NameOld = groupEdit.Name;
                    groupEdit.Code = model.Code;
                    groupEdit.Name = model.Name;
                    groupEdit.ModuleGroupId = model.ModuleGroupId;
                    groupEdit.Note = model.Note;
                    groupEdit.Status = model.Status;
                    groupEdit.State = model.State;
                    groupEdit.Specification = model.Specification;
                    groupEdit.Pricing = model.Pricing;
                    groupEdit.FileElectric = model.FileElectric;
                    groupEdit.FileElectronic = model.FileElectronic;
                    groupEdit.FileMechanics = model.FileMechanics;
                    groupEdit.IsSoftware = model.IsSoftware;
                    groupEdit.IsHMI = model.IsHMI;
                    groupEdit.IsPLC = model.IsPLC;
                    groupEdit.ThumbnailPath = model.ThumbnailPath;
                    groupEdit.IsFilm = model.IsFilm;
                    groupEdit.IsDMTV = model.IsDMTV;
                    groupEdit.Description = model.Description;
                    groupEdit.UpdateBy = model.CreateBy;
                    groupEdit.UpdateDate = DateTime.Now;
                    groupEdit.IsManual = model.IsManual;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ModuleHistoryModel>(groupEdit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, groupEdit.UpdateBy, Constants.LOG_Module, groupEdit.Id, groupEdit.Code, jsonBefor, jsonApter);

                    var listStageOld = db.ModuleProductionTimes.Where(a => a.ModuleId.Equals(model.Id)).ToList();

                    if (listStageOld.Count > 0)
                    {
                        db.ModuleProductionTimes.RemoveRange(listStageOld);
                    }
                    if (model.ListStage.Count() > 0)
                    {
                        groupEdit.Leadtime = model.ListStage.Sum(a => a.Time);
                        foreach (var item in model.ListStage)
                        {
                            ModuleProductionTime moduleProductionTime = new ModuleProductionTime()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleId = groupEdit.Id,
                                StageId = item.StageId,
                                ExecutionTime = item.Time
                            };
                            db.ModuleProductionTimes.Add(moduleProductionTime);
                        }
                    }


                    var listModuleManualDocument = db.ModuleManualDocuments.Where(a => a.ModuleId.Equals(model.Id)).ToList();
                    db.ModuleManualDocuments.RemoveRange(listModuleManualDocument);

                    var documentObjects = db.DocumentObjects.Where(i => i.ObjectId.Equals(model.Id) && i.ObjectType == Constants.ObjectType_Module).ToList();
                    db.DocumentObjects.RemoveRange(documentObjects);

                    foreach (var item in model.ListModuleManualDocument)
                    {
                        if (!item.IsDocument)
                        {
                            if (!string.IsNullOrEmpty(item.FileName))
                            {
                                ModuleManualDocument moduleManualDocument = new ModuleManualDocument()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ModuleId = model.Id,
                                    Path = item.Path,
                                    FileName = item.FileName,
                                    CreateDate = DateTime.Now,
                                    FileSize = item.FileSize,
                                    Note = item.Note,
                                    FileType = item.FileType,
                                };
                                db.ModuleManualDocuments.Add(moduleManualDocument);
                                listModuleManualDocument.Add(moduleManualDocument);
                            }
                        }
                        else
                        {
                            DocumentObject documentObject = new DocumentObject()
                            {
                                Id = Guid.NewGuid().ToString(),
                                DocumentId = item.Id,
                                ObjectId = model.Id,
                                ObjectType = Constants.ObjectType_Module
                            };
                            db.DocumentObjects.Add(documentObject);
                        }
                    }

                    if (model.ListModuleManualDocument.Count > 0)
                    {
                        var checkExits = listModuleManualDocument.Where(a => a.FileName.Contains(model.Code)).ToList();
                        if (checkExits.Count > 0)
                        {
                            groupEdit.ManualExist = true;
                        }
                        else
                        {
                            groupEdit.ManualExist = false;
                        }
                    }
                    else
                    {
                        groupEdit.ManualExist = false;
                    }

                    var listImage = db.ModuleImages.Where(a => a.ModuleId.Equals(model.Id));
                    db.ModuleImages.RemoveRange(listImage);

                    foreach (var item in model.ListImage)
                    {
                        ModuleImage moduleImage = new ModuleImage()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ModuleId = model.Id,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            ThumbnailPath = item.ThumbnailPath,
                            Note = item.Note,
                            CreateBy = model.CreateBy,
                            CreateDate = DateTime.Now,
                        };
                        db.ModuleImages.Add(moduleImage);
                    }

                    if (groupEdit.CurrentVersion != model.CurrentVersion)
                    {
                        ModuleOldVersion history = new ModuleOldVersion
                        {
                            Id = Guid.NewGuid().ToString(),
                            ModuleId = model.Id,
                            Version = model.CurrentVersion,
                            Content = model.EditContent,
                            CreateBy = model.CreateBy,
                            CreateDate = DateTime.Now
                        };
                        db.ModuleOldVersions.Add(history);

                        groupEdit.EditContent = model.EditContent;
                        groupEdit.CurrentVersion = model.CurrentVersion;

                        var moduleDesignDocument = (from r in db.ModuleDesignDocuments
                                                    where r.ModuleId == model.Id
                                                    select r);

                        ModuleDesignDocumentOld moduleDesignDocumentOld = new ModuleDesignDocumentOld();
                        foreach (var item in moduleDesignDocument)
                        {
                            moduleDesignDocumentOld = new ModuleDesignDocumentOld()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ModuleOldVersionId = history.Id,
                                Name = item.Name,
                                Path = item.Path,
                                ServerPath = item.ServerPath,
                                FileSize = item.FileSize,
                                FileType = item.FileType,
                                ParentId = item.ParentId,
                                DesignType = item.DesignType,
                                CreateBy = item.CreateBy,
                                CreateDate = item.CreateDate,
                                UpdateBy = item.UpdateBy,
                                UpdateDate = item.UpdateDate,
                                HashValue = item.HashValue
                            };

                            db.ModuleDesignDocumentOlds.Add(moduleDesignDocumentOld);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
            try
            {
                string decription = String.Empty;
                if (NameOld.ToLower() == model.Name.ToLower())
                {
                    decription = "Cập nhật nhóm module tên là: " + NameOld;
                }
                else
                {
                    decription = "Cập nhật nhóm module có tên ban đầu là:  " + NameOld + " thành " + model.Name; ;
                }
                LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
            }
            catch (Exception) { }
        }

        public void UpdateModuleIsDMTV(ModuleModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.ListModule.Count > 0)
                    {
                        foreach (var item in model.ListModule)
                        {
                            var module = db.Modules.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            module.IsDMTV = item.IsDMTV;
                            module.UpdateBy = model.UpdateBy;
                            module.UpdateDate = DateTime.Now;
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

        public void UpdateModulePart(ModuleModel moduleModel)
        {
            moduleModel.ListModulePartModel.ToList();
            DeleteModulePart(moduleModel);
            List<ModulePartModel> listNewPart = new List<ModulePartModel>();

            foreach (var item in moduleModel.ListModulePartModel)
            {
                var modulePartEdit = db.ModuleParts.AsQueryable().Where(o => item.Id.Equals(o.Id)).FirstOrDefault();
                if (modulePartEdit != null)
                {
                    modulePartEdit.Qty = item.Qty;
                    modulePartEdit.ModuleCode = item.ModuleCode;
                    modulePartEdit.ModuleName = item.ModuleName;
                }
                else
                {
                    listNewPart.Add(item);
                }
            }
            moduleModel.ListModulePartModel = listNewPart;
            AddModulePart(moduleModel, moduleModel.Id);
        }

        public void DeleteModulePart(ModuleModel moduleModel)
        {
            List<ModulePartModel> oldList = GetListModulePart(moduleModel);
            List<ModulePartModel> newList = moduleModel.ListModulePartModel;
            List<ModulePart> deleteList = new List<ModulePart>();
            for (var i = 0; i < oldList.Count; i++)
            {
                var flag = 0;
                for (var y = 0; y < newList.Count; y++)
                {

                    if (oldList[i].Id.Equals(newList[y].Id))
                    {
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    var idDelete = oldList[i].Id;
                    var modulePartDelete = db.ModuleParts.Where(o => idDelete.Equals(o.Id)).FirstOrDefault();
                    db.ModuleParts.Remove(modulePartDelete);
                    //deleteList.Add(modulePartDelete);
                }
            }
            //for (var x = 0; x < deleteList.Count; x++)
            //{
            //    db.ModuleParts.Remove(deleteList[x]);
            //}
        }

        /// <summary>
        /// Lấy danh sách folder thiết kế module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<ModuleDesignDocumentModel> GetListFolderModule(string moduleId)
        {
            List<ModuleDesignDocumentModel> list = new List<ModuleDesignDocumentModel>();

            var moduleDesignDocuments = db.ModuleDesignDocuments.AsNoTracking().Where(r => r.ModuleId.Equals(moduleId)).ToList();

            var data = moduleDesignDocuments.Where(t => t.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder)).OrderBy(o => o.Path).Select(m => new ModuleDesignDocumentModel
            {
                Id = m.Id,
                ModuleId = m.ModuleId,
                Name = m.Name,
                ParentId = m.ParentId,
                Path = m.Path,
                ServerPath = m.ServerPath,
                DesignType = m.DesignType,
                //TotalFile = moduleDesignDocuments.Count(r => m.Id.Equals(r.ParentId) && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File))
            }).ToList();

            var moduleDesignDocumentFiles = moduleDesignDocuments.Where(r => r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)).ToList();
            foreach (var item in data)
            {
                item.TotalFile = moduleDesignDocumentFiles.Count(r => item.Id.Equals(r.ParentId));
            }

            var root = db.ModuleDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder) && string.IsNullOrEmpty(t.ModuleId)).OrderBy(o => o.Path).Select(m => new ModuleDesignDocumentModel
            {
                Id = m.Id,
                ModuleId = m.ModuleId,
                Name = m.Name,
                ParentId = m.ParentId,
                Path = m.Path,
                ServerPath = m.ServerPath,
                DesignType = m.DesignType,
            }).ToList();

            // Lấy thiết kế điện tử
            var materialPCB = db.ModuleMaterials.AsNoTracking().Where(r => r.ModuleId.Equals(moduleId) && r.MaterialCode.ToUpper().StartsWith("PCB")).ToList();

            Module modulePCB;
            List<ModuleDesignDocumentModel> documentPCB;
            foreach (var item in materialPCB)
            {
                modulePCB = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(item.MaterialCode));

                if (modulePCB != null)
                {
                    moduleDesignDocuments = db.ModuleDesignDocuments.AsNoTracking().Where(t => t.ModuleId.Equals(modulePCB.Id)).ToList();

                    documentPCB = moduleDesignDocuments.Where(t => t.FileType.Equals(Constants.ModuleDesignDocument_FileType_Folder))
                        .OrderBy(o => o.Path)
                        .Select(m => new ModuleDesignDocumentModel
                        {
                            Id = m.Id,
                            ModuleId = m.ModuleId,
                            Name = m.Name,
                            ParentId = m.ParentId,
                            Path = m.Path,
                            ServerPath = m.ServerPath,
                            DesignType = m.DesignType,
                            //TotalFile = moduleDesignDocuments.Count(r => m.Id.Equals(r.ParentId) && r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File))
                        }).ToList();

                    moduleDesignDocumentFiles = moduleDesignDocuments.Where(r => r.FileType.Equals(Constants.ModuleDesignDocument_FileType_File)).ToList();

                    foreach (var itemPCB in documentPCB)
                    {
                        itemPCB.TotalFile = moduleDesignDocumentFiles.Count(r => itemPCB.Id.Equals(r.ParentId));
                    }

                    list.AddRange(documentPCB);

                }
            }

            list.AddRange(data);
            list.AddRange(root);
            return list;
        }

        public List<ModuleDesignDocumentModel> GetListFileModule(ModuleDesignDocumentModel model)
        {
            var list = db.ModuleDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ModuleDesignDocument_FileType_File) && t.ParentId.Equals(model.Id)).OrderBy(o => o.Path).Select(m => new ModuleDesignDocumentModel
            {
                Id = m.Id,
                ModuleId = m.ModuleId,
                Name = m.Name,
                ParentId = m.ParentId,
                FileSize = m.FileSize,
                CreateDate = m.CreateDate,
                UpdateDate = m.UpdateDate,
                Path = m.Path,
                ServerPath = m.ServerPath
            }).ToList();
            return list;
        }

        public void UploadDesignDocument(ImportFileModuleModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var module = db.Modules.FirstOrDefault(t => model.ModuleId.Equals(t.Id));

                    if (module == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
                    }

                    // Có tài liệu thiết kế
                    if (model.DesignDocuments.Count > 0)
                    {
                        List<ModuleDesignDocument> designDocuments = new List<ModuleDesignDocument>();
                        ModuleDesignDocument designDocument;
                        ModuleDesignDocument designDocumentFile;

                        var documents = db.ModuleDesignDocuments.Where(r => r.DesignType == model.DesignType && r.ModuleId.Equals(module.Id)).ToList();

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
                                    if (file.LocalPath.EndsWith(document.Path))
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
                                db.ModuleDesignDocuments.Remove(document);
                            }
                        }

                        var folderRoor = db.ModuleDesignDocuments.AsNoTracking().Where(r => r.Id.Equals(model.DesignType.ToString())).FirstOrDefault();
                        FolderUploadModel parent;
                        if (folderRoor == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0026);
                        }

                        var documentDesigns = db.ModuleDesignDocuments.Where(r => r.ModuleId.Equals(model.ModuleId));

                        foreach (var item in model.DesignDocuments)
                        {
                            designDocument = documentDesigns.FirstOrDefault(r => r.Path.Equals(item.LocalPath));
                            if (designDocument == null)
                            {
                                designDocument = new ModuleDesignDocument()
                                {
                                    Id = item.Id,
                                    ModuleId = module.Id,
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
                                //  Trường hợp không thay đổi thì không xử lý
                                if (file.RowState == 1)
                                {
                                    continue;
                                }

                                designDocumentFile = documentDesigns.FirstOrDefault(r => r.Path.Equals(file.LocalPath));
                                if (designDocumentFile == null)
                                {
                                    designDocumentFile = new ModuleDesignDocument()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ModuleId = module.Id,
                                        ParentId = item.Id,
                                        ServerPath = file.ServerPath,
                                        Path = file.LocalPath,
                                        Name = file.Name,
                                        FileType = Constants.ModuleDesignDocument_FileType_File,
                                        DesignType = model.DesignType,
                                        HashValue = file.HashValue,
                                        FileSize = file.Size,
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
                                    //designDocumentFile.ParentId = item.Id;
                                    designDocumentFile.UpdateBy = userId;
                                    designDocumentFile.UpdateDate = DateTime.Now;
                                    designDocumentFile.FileSize = file.Size;
                                    designDocumentFile.ServerPath = file.ServerPath;
                                }
                            }
                        }

                        db.ModuleDesignDocuments.AddRange(designDocuments);
                    }

                    if (model.DesignType == Constants.TypeFunctionDefinition_Mechanical)
                    {
                        module.MechanicsExist = true;
                        module.FilmExist = model.FilmExist;
                        module.ElectronicExist = false;
                        var modules = db.Modules.AsNoTracking().Where(r => r.Code.StartsWith("PCB")).ToList();

                        var materialPCB = db.ModuleMaterials.AsNoTracking().Where(r => r.ModuleId.Equals(model.ModuleId) && r.MaterialCode.ToUpper().StartsWith("PCB"))
                                                                                .GroupBy(g => g.MaterialCode).Select(s => s.Key).ToList();

                        var modulePCBCheck = (from m in modules
                                              join p in materialPCB on m.Code.ToUpper() equals p.ToUpper()
                                              where m.ElectronicExist
                                              select m).ToList();

                        if (modulePCBCheck.Count == materialPCB.Count && materialPCB.Count > 0)
                        {
                            module.ElectronicExist = true;
                        }

                    }
                    else if (model.DesignType == Constants.TypeFunctionDefinition_Electricity)
                    {
                        module.ElectricExist = true;
                        module.HMIExist = model.HMIExist;
                        module.PLCExist = model.PLCExist;
                        module.SoftwareExist = model.SoftwareExist;
                    }
                    else if (model.DesignType == Constants.TypeFunctionDefinition_Electronic)
                    {
                        module.ElectronicExist = true;

                        var moduleIds = (from m in db.ModuleMaterials.AsNoTracking()
                                         join d in db.Modules.AsNoTracking() on m.ModuleId equals d.Id
                                         where m.MaterialCode.ToUpper().Equals(module.Code.ToUpper()) && !d.ElectronicExist
                                         group d by d.Id into g
                                         select g.Key).ToList();

                        var modules = db.Modules.AsNoTracking().Where(r => r.Code.StartsWith("PCB") && !r.Id.Equals(model.ModuleId)).ToList();

                        foreach (var moduleId in moduleIds)
                        {
                            var modulePCB = db.Modules.FirstOrDefault(r => r.Id.Equals(moduleId));
                            modulePCB.ElectronicExist = false;

                            var materialPCB = db.ModuleMaterials.AsNoTracking().Where(r => r.ModuleId.Equals(moduleId) && r.MaterialCode.ToUpper().StartsWith("PCB"))
                                                                                .GroupBy(g => g.MaterialCode).Select(s => s.Key).ToList();

                            var modulePCBCheck = (from m in modules
                                                  join p in materialPCB on m.Code.ToUpper() equals p.ToUpper()
                                                  where m.ElectronicExist
                                                  select m).ToList();

                            if (materialPCB.Count > 0 && modulePCBCheck.Count - 1 == materialPCB.Count)
                            {
                                modulePCB.ElectronicExist = true;
                            }
                        }
                    }

                    // Danh sách người tk
                    db.ModuleDesigners.RemoveRange(db.ModuleDesigners.Where(r => r.ModuleId.Equals(module.Id) && model.DesignType == r.DesignType));

                    ModuleDesigner designer;
                    foreach (var item in model.Designers)
                    {
                        designer = new ModuleDesigner()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Designer = item.Designer,
                            DesignType = item.DesignType,
                            ModuleId = module.Id
                        };

                        db.ModuleDesigners.Add(designer);
                    }

                    // Update danh mục vật tư
                    UpdateModuleMaterials(model.Materials, module.Id, userId);

                    //Add thông tin thiết bị 
                    foreach (var item in model.ListData)
                    {
                        if (item.IsChecked)
                        {
                            var productModule = db.ProductModuleUpdates.FirstOrDefault(i => i.ModuleId.Equals(module.Id) && i.ProductId.Equals(item.Id));
                            if (productModule != null)
                            {
                                productModule.UpdateBy = userId;
                                productModule.UpdateDate = DateTime.Now;
                            }
                            else
                            {
                                ProductModuleUpdate productModuleUpdate = new ProductModuleUpdate()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ModuleId = module.Id,
                                    ProductId = item.Id,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now
                                };
                                db.ProductModuleUpdates.Add(productModuleUpdate);
                            }
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();

                }
                catch(NTSException ntsEx)
                {
                    trans.Rollback();
                    throw ntsEx;
                }
                catch (Exception ex)
                {
                    trans.Rollback(); 
                    throw new NTSLogException(model, ex);
                }
            }
        }

        /// <summary>
        /// Cập nhật danh mục vật tư module
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="moduleId"></param>
        /// <param name="userId"></param>
        private void UpdateModuleMaterials(List<ModuleMaterialModel> materials, string moduleId, string userId)
        {
            // Cập nhật danh sách vật tư module
            if (materials.Count == 0)
            {
                return;
            }
            // Kiểm tra HSX TPA
            var manu = db.Manufactures.AsNoTracking().FirstOrDefault(t => "TPA".Equals(t.Code));
            if (manu == null)
            {
                var rule = db.CodeRules.AsNoTracking().FirstOrDefault(t => "TPA".Equals(t.Code));
                if (rule != null)
                {
                    manu = new Manufacture()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = rule.Code,
                        Name = rule.Code,
                        MaterialType = Constants.Material_Type_Standard,
                        Status = Constants.Manufacture_Status_Use,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now

                    };
                    db.Manufactures.Add(manu);

                    var manufactureGroup = new ManufactureInGroup()
                    {
                        ManufactureId = manu.Id,
                        ManufactureGroupId = rule.MaterialGroupId
                    };
                    db.ManufactureInGroups.Add(manufactureGroup);
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0019, TextResourceKey.Manufacture);
                }
            }

            Material material;
            List<Material> lstMaterial = new List<Material>();
            MaterialGroup materialGroup;

            List<ModuleMaterial> lstModuleMaterial = new List<ModuleMaterial>();

            var moduleMaterials = db.ModuleMaterials.Where(t => t.ModuleId.Equals(moduleId)).ToList();

            if (moduleMaterials.Count > 0)
            {
                db.ModuleMaterials.RemoveRange(moduleMaterials);
            }

            List<string> codeExist = new List<string>();
            ModuleMaterial moduleMaterial;
            Manufacture manufacture;
            Unit unit;
            RawMaterial rawMaterial;
            foreach (var item in materials)
            {
                moduleMaterial = new ModuleMaterial()
                {
                    Id = Guid.NewGuid().ToString(),
                    Index = item.Index,
                    ModuleId = moduleId,
                    MaterialName = item.MaterialName,
                    Specification = item.Specification,
                    MaterialCode = item.MaterialCode,
                    RawMaterialCode = item.RawMaterialCode,
                    UnitName = item.UnitName,
                    Quantity = item.Quantity,
                    RawMaterial = item.RawMaterial,
                    Weight = item.Weight,
                    ManufacturerCode = item.ManufacturerCode,
                    Note = item.Note,
                };

                manufacture = db.Manufactures.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(item.ManufacturerCode.ToUpper()));

                if (manufacture != null)
                {
                    moduleMaterial.ManufacturerId = manufacture.Id;
                }

                material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(item.MaterialCode.ToLower()));

                if (material != null)
                {
                    moduleMaterial.MaterialId = material.Id;
                    moduleMaterial.Price = material.Pricing;
                    moduleMaterial.Amount = moduleMaterial.Price * moduleMaterial.Quantity;
                }
                else if (item.ManufacturerCode.ToUpper().Equals(Constants.Manufacture_TPA))
                {
                    material = new Material()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = item.MaterialName,
                        Code = item.MaterialCode,
                        ManufactureId = moduleMaterial.ManufacturerId,
                        MaterialType = manufacture.MaterialType,
                        Specification = item.Specification,
                        Note = item.Note,
                        RawMaterial = item.RawMaterial,
                        Status = Constants.Material_Status_Use,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                        DeliveryDays = 0,
                        Pricing = 0
                    };

                    moduleMaterial.MaterialId = material.Id;

                    var codeRule = db.CodeRules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(item.MaterialCode.Substring(0, r.Code.Length)));

                    if (codeRule != null && !string.IsNullOrEmpty(codeRule.MaterialGroupId))
                    {

                        material.MaterialGroupId = codeRule.MaterialGroupId;
                        material.MaterialGroupTPAId = codeRule.MaterialGroupTPAId;
                    }
                    else
                    {
                        materialGroup = db.MaterialGroups.AsNoTracking().FirstOrDefault();

                        if (materialGroup != null)
                        {
                            material.MaterialGroupId = materialGroup.Id;
                            material.MaterialGroupTPAId = materialGroup.MaterialGroupTPAId;
                        }
                    }

                    unit = db.Units.AsNoTracking().FirstOrDefault(r => r.Name.ToLower().Equals(item.UnitName.ToLower()));

                    if (unit != null)
                    {
                        material.UnitId = unit.Id;
                    }
                    else
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0073, TextResource.Unit, item.UnitName);
                    }

                    db.Materials.Add(material);
                    db.SaveChanges();
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0073, TextResource.Material, item.MaterialCode);
                }

                lstModuleMaterial.Add(moduleMaterial);

                if (!string.IsNullOrEmpty(item.RawMaterialCode))
                {
                    material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(item.RawMaterialCode.ToUpper()));

                    if (material == null)
                    {
                        continue;
                    }

                    moduleMaterial = new ModuleMaterial()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Index = item.Index + ".1",
                        ModuleId = moduleId,
                        MaterialId = material.Id,
                        MaterialName = material.Name,
                        MaterialCode = material.Code,
                        Weight = 0,
                        ManufacturerCode = item.ManufacturerCode,
                        Price = material.Pricing
                    };

                    if (manufacture != null)
                    {
                        moduleMaterial.ManufacturerCode = manufacture.Code;
                    }

                    unit = db.Units.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.UnitId));
                    moduleMaterial.UnitName = unit.Name;

                    manufacture = db.Manufactures.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.ManufactureId));
                    moduleMaterial.ManufacturerCode = manufacture.Code;
                    moduleMaterial.ManufacturerId = manufacture.Id;

                    if (!Constants.Manufacture_TPA.Equals(item.Specification.ToUpper()))
                    {
                        moduleMaterial.Quantity = item.Quantity;
                    }
                    else
                    {
                        var unitKg = db.Units.AsNoTracking().FirstOrDefault(r => r.Name.ToUpper().Equals(Constants.Unit_Kg));

                        if (unitKg != null && item.Weight > 0)
                        {
                            moduleMaterial.Quantity = GetTotalByConvertUnit(material.Id, unitKg.Id, unit.Id, item.Weight, item.Quantity);
                        }
                        else
                        {
                            moduleMaterial.Quantity = item.Quantity;
                        }
                    }

                    lstModuleMaterial.Add(moduleMaterial);
                }
                else if (string.IsNullOrEmpty(item.RawMaterialCode) && !string.IsNullOrEmpty(item.RawMaterial) && item.Weight > 0
                    && Constants.Manufacture_TPA.Equals(item.Specification.ToUpper()) && Constants.Manufacture_TPA.Equals(item.ManufacturerCode.ToUpper()) && !Constants.Unit_Bo.ToUpper().Equals(item.UnitName.ToUpper()))
                {
                    rawMaterial = db.RawMaterials.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(item.RawMaterial.ToUpper()));

                    if (rawMaterial == null || string.IsNullOrEmpty(rawMaterial.MaterialId))
                    {
                        continue;
                    }

                    material = db.Materials.AsNoTracking().FirstOrDefault(r => r.Id.Equals(rawMaterial.MaterialId));

                    if (material == null)
                    {
                        continue;
                    }

                    moduleMaterial = new ModuleMaterial()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Index = item.Index + ".1",
                        ModuleId = moduleId,
                        MaterialId = material.Id,
                        MaterialName = material.Name,
                        MaterialCode = material.Code,
                        Weight = 0,
                        ManufacturerCode = item.ManufacturerCode,
                        Quantity = item.Weight,
                        Price = material.Pricing
                    };

                    manufacture = db.Manufactures.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.ManufactureId));
                    moduleMaterial.ManufacturerCode = manufacture.Code;
                    moduleMaterial.ManufacturerId = manufacture.Id;

                    unit = db.Units.AsNoTracking().FirstOrDefault(r => r.Id.Equals(material.UnitId));
                    moduleMaterial.UnitName = unit.Name;

                    lstModuleMaterial.Add(moduleMaterial);
                }
            }

            db.ModuleMaterials.AddRange(lstModuleMaterial);
        }

        /// <summary>
        /// Lấy số lượng theo chuyển đổi đơn vị
        /// </summary>
        /// <param name="partId"></param>
        /// <param name="unitIdSrc"></param>
        /// <param name="unitIdDis"></param>
        /// <param name="totalFile"></param>
        /// <param name="isConvert"></param>
        /// <returns></returns>
        public decimal GetTotalByConvertUnit(string materialId, string unitIdDis, string unitIdSrc, decimal totalsrc, decimal quantitySrc)
        {
            try
            {
                var converUnits = db.ConverUnits.AsNoTracking().FirstOrDefault(r => r.MaterialId.Equals(materialId) && r.UnitId.Equals(unitIdDis));

                if (converUnits == null)
                {
                    return quantitySrc;
                }

                decimal total = (totalsrc * converUnits.Quantity) / converUnits.ConvertQuantity;

                return Math.Round(total + ((total * converUnits.LossRate) / 100), 4);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                return totalsrc;
            }
        }

        public TreeListDesignDocumentModel GetTreeList(ModuleDesignDocumentModel model)
        {
            TreeListDesignDocumentModel rs = new TreeListDesignDocumentModel();
            var folderParent = db.ModuleDesignDocuments.AsNoTracking().FirstOrDefault(t => t.Id.Equals(model.Id));
            var allDocument = db.ModuleDesignDocuments.AsNoTracking().ToList();

            if (folderParent != null)
            {
                rs.Id = folderParent.Id;
                rs.Name = folderParent.Name;
                rs.Path = folderParent.Name + "/";

                var checkChild = allDocument.Where(t => t.ParentId.Equals(rs.Id)).ToList();
                if (checkChild.Count > 0)
                {
                    //Lấy danh sách file trong thư mục
                    var listFile = checkChild.Where(t => t.FileType.Equals("1")).ToList();
                    if (listFile.Count > 0)
                    {
                        foreach (var item in listFile)
                        {
                            var lst = listFile.Select(m => new TreeListDesignDocumentModel
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Path = rs.Path + item.Name
                            }).ToList();
                            rs.ListFile = lst;
                        }
                    }

                    //Lấy danh sách folder trong thư mục
                    var listFolder = checkChild.Where(t => t.FileType.Equals("2")).ToList();
                    if (listFolder.Count > 0)
                    {
                        foreach (var item in listFolder)
                        {
                            TreeListDesignDocumentModel folder = new TreeListDesignDocumentModel()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Path = rs.Path + item.Name
                            };
                            rs.ListFolder.Add(folder);
                            GetChildren(item, allDocument, folder);
                        }
                    }
                }
            }
            return rs;
        }

        public void GetChildren(ModuleDesignDocument entity, List<ModuleDesignDocument> allDocument, TreeListDesignDocumentModel folder)
        {
            var child = allDocument.Where(t => t.ParentId.Equals(entity.Id)).ToList();
            if (child.Count > 0)
            {
                //Lấy danh sách file trong thư mục
                var listFile = child.Where(t => t.FileType.Equals("1")).ToList();
                if (listFile.Count > 0)
                {
                    foreach (var item in listFile)
                    {
                        var lst = listFile.Select(m => new TreeListDesignDocumentModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Path = folder.Path + "/" + item.Name
                        }).ToList();
                        folder.ListFile = lst;
                    }
                }

                //Lấy danh sách folder trong thư mục
                var listFolder = child.Where(t => t.FileType.Equals("2")).ToList();
                if (listFolder.Count > 0)
                {
                    foreach (var item in listFolder)
                    {
                        TreeListDesignDocumentModel folderChild = new TreeListDesignDocumentModel()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Path = folder.Path + "/" + item.Name
                        };
                        folder.ListFolder.Add(folderChild);
                        GetChildren(item, allDocument, folderChild);
                    }
                }
            }
        }

        public string GetChildrenFolderId(ModuleDesignDocumentModel model)
        {
            string id = string.Empty;
            var item = db.ModuleDesignDocuments.AsNoTracking().FirstOrDefault(t => t.ParentId.Equals(model.Id) && t.ModuleId.Equals(model.ModuleId));
            if (item != null)
            {
                id = item.Id;
            }
            return id;
        }

        public List<ModuleDesignerModel> GetDesigners(string moduleId)
        {
            var designers = (from m in db.ModuleDesigners.AsNoTracking()
                             where m.ModuleId.Equals(moduleId)
                             select new ModuleDesignerModel()
                             {
                                 Designer = m.Designer,
                                 DesignType = m.DesignType
                             }).ToList();

            var materials = (from m in db.ModuleMaterials.AsNoTracking()
                             where m.ModuleId.Equals(moduleId) && m.MaterialCode.ToUpper().StartsWith("PCB")
                             select new
                             {
                                 m.MaterialCode
                             });

            var designerPCB = (from d in db.ModuleDesigners.AsNoTracking()
                               join m in db.Modules.AsNoTracking() on d.ModuleId equals m.Id
                               join t in materials on m.Code.ToUpper() equals t.MaterialCode.ToUpper()
                               select new ModuleDesignerModel()
                               {
                                   Designer = d.Designer,
                                   DesignType = d.DesignType
                               }).ToList();

            designers.AddRange(designerPCB);

            return designers;
        }

        public List<string> GetListParent(string id, List<ModuleGroup> moduleGroups)
        {
            List<string> listChild = new List<string>();
            var moduleGroup = moduleGroups.Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(moduleGroup);
            if (moduleGroup.Count > 0)
            {
                foreach (var item in moduleGroup)
                {
                    listChild.AddRange(GetListParent(item, moduleGroups));
                }
            }
            return listChild;
        }

        /// <summary>
        /// Lấy danh sách công việc thiết kế chưa hoàn thành
        /// </summary>
        /// <returns></returns>
        public SearchPlanResultModel<ListPlanDesginModel> SearchListPlanDesgin(string moduleId)
        {
            SearchPlanResultModel<ListPlanDesginModel> searchResult = new SearchPlanResultModel<ListPlanDesginModel>();

            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.Equals(moduleId));
            if (module == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }

            searchResult.ElectricExist = module.ElectricExist;
            searchResult.ElectronicExist = module.ElectricExist;
            searchResult.MechanicsExist = module.MechanicsExist;
            searchResult.FileElectric = module.FileElectric;
            searchResult.FileElectronic = module.FileElectronic;
            searchResult.FileMechanics = module.FileMechanics;

            //var listPlan = (from a in db.Plans.AsNoTracking()
            //                join b in db.Tasks.AsNoTracking() on a.TaskId equals b.Id into ab
            //                from ba in ab.DefaultIfEmpty()
            //                join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id into ac
            //                from ca in ac.DefaultIfEmpty()
            //                join e in db.Employees.AsNoTracking() on a.ResponsiblePersion equals e.Id into ae
            //                from ea in ae.DefaultIfEmpty()
            //                join f in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals f.Id
            //                join g in db.Modules.AsNoTracking() on f.ModuleId equals g.Id
            //                where ba.Type == Constants.Task_Design && a.Status != Constants.Plan_Status_Done && g.Id.Equals(moduleId) && string.IsNullOrEmpty(a.ReferenceId)
            //                select new ListPlanDesginModel()
            //                {
            //                    Id = a.Id,
            //                    NameWork = ba.Name,
            //                    CodeProject = ca.Code,
            //                    UserName = ea.Name,
            //                    UserCode = ea.Code,
            //                    DateStart = a.StartDate,
            //                    DateEnd = a.EndDate,
            //                    Status = a.Status
            //                }).ToList();

            //searchResult.ListResult = listPlan;

            return searchResult;
        }

        /// <summary>
        /// Update trạng thái 1 hoặc nhiều kế hoạch thành đã hoàn thành
        /// </summary>
        /// <param name="ListChecker"></param>
        /// <param name="updateBy"></param>
        public void UpdateListCheckStatus(List<string> ListChecker, string updateBy)
        {

            foreach (var item in ListChecker)
            {
                var status = db.Plans.FirstOrDefault(r => r.Id.Equals(item));

                if (status == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
                }

                status.Status = Constants.Plan_Status_Done;
                status.UpdateDate = DateTime.Now;
                //status.RealEndDate = DateTime.Now;
                status.UpdateBy = updateBy;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Lấy danh sách công đoạn theo nhóm module 
        /// </summary>
        /// <param name="moduleGroupId"></param>
        /// <returns></returns>
        public SearchResultModel<StageModel> GetListStageByModuleGroupId(StageModel model, string departmentId)
        {
            SearchResultModel<StageModel> searchResult = new SearchResultModel<StageModel>();

            var listStage = (from a in db.ModuleGroupStages.AsNoTracking()
                             join b in db.Stages.AsNoTracking() on a.StageId equals b.Id
                             where a.ModuleGroupId.Equals(model.ModuleGroupId) && b.DepartmentId.Equals(departmentId)
                             orderby b.Name
                             select new StageModel()
                             {
                                 StageId = b.Id,
                                 Name = b.Name,
                                 Time = a.Time
                             }).ToList();

            searchResult.ListResult = listStage.ToList();

            return searchResult;
        }

        public string GetContentModule(string moduleId)
        {
            string content = string.Empty;
            var data = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(moduleId));
            if (data != null)
            {
                content = data.EditContent;
            }
            else
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }
            return content;
        }

        public void UpdateContent(ModuleContenModel model)
        {
            var module = db.Modules.FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }
            module.EditContent = model.Content;
            db.SaveChanges();
        }

        public string ImportFile(HttpPostedFile file, bool isConfirm, string userId)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            string code;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<int> rowCode = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<string> listId = new List<string>();
            string confirm = string.Empty;

            try
            {
                var list = db.Modules.AsNoTracking().ToList();
                for (int i = 3; i <= rowCount; i++)
                {
                    code = sheet[i, 2].Value;

                    if (!string.IsNullOrEmpty(code))
                    {
                        var data = list.FirstOrDefault(a => a.Code.Trim().Equals(code.Trim()));
                        if (data != null)
                        {
                            listId.Add(data.Id);
                        }
                        else
                        {
                            rowCheckCode.Add(i);
                        }
                    }
                    else
                    {
                        rowCode.Add(i);
                    }
                }

                #endregion

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã module dòng <" + string.Join(", ", rowCode) + "> không được để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã module dòng <" + string.Join(", ", rowCheckCode) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                confirm = SyncSaleModule(false, isConfirm, listId, userId);
            }
            catch (NTSException ntsEx)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ntsEx;
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();

            return confirm;
        }

        public string SyncSaleModule(bool isAll, bool isConfirm, List<string> listModuleId, string userId)
        {
            //ProductModel product;
            SaleProduct saleProduct;
            SaleProduct check;
            SaleProductMedia saleProductMedia;
            List<SaleProductMedia> saleProductMedias;
            List<SaleProduct> listSaleProduct = new List<SaleProduct>();
            List<SaleProductMedia> listSaleProductMedia = new List<SaleProductMedia>();
            SyncSaleProduct syncSaleProduct;
            List<SyncSaleProduct> syncSaleProducts = new List<SyncSaleProduct>();
            Module module;
            List<ModuleImage> listImage;
            string groupCode = string.Empty;
            var listModuleGroup = db.ModuleGroups.AsNoTracking().ToList();
            decimal price = 0;
            string conten = $"Đồng bộ sản phẩm kinh doanh";
            List<string> listCodeError = new List<string>();

            if (isAll)
            {
                listModuleId = new List<string>();
                listModuleId = db.Modules.AsNoTracking().Select(i => i.Id).ToList();
            }

            foreach (var id in listModuleId)
            {
                module = new Module();
                module = db.Modules.FirstOrDefault(i => i.Id.Equals(id));
                if (module == null)
                {
                    continue;
                }
                module.SyncDate = DateTime.Now;
                module.IsSendSale = true;
                price = 0;
                price = moduleMaterialBusiness.GetPriceModuleByModuleId(module.Id, 0);

                listImage = new List<ModuleImage>();
                listImage = db.ModuleImages.AsNoTracking().Where(i => i.ModuleId.Equals(module.Id)).ToList();

                groupCode = listModuleGroup.FirstOrDefault(i => i.Id.Equals(module.ModuleGroupId))?.Code;

                // Kiểm tra thông tin module, nếu thiếu thì cảnh báo
                if (!isConfirm && (string.IsNullOrEmpty(groupCode) ||
                    string.IsNullOrEmpty(module.Description) ||
                    price <= 0 || module.Leadtime <= 0))
                {
                    listCodeError.Add(module.Code);
                    continue;
                }

                check = new SaleProduct();
                check = db.SaleProducts.FirstOrDefault(i => i.Type == Constants.SaleProductModule && i.SourceId.Equals(module.Id));
                if (check != null)
                {
                    check.EName = string.Empty;
                    check.VName = module.Name;
                    check.Model = module.Code;
                    check.GroupCode = groupCode;
                    check.ManufactureId = Constants.ManufactureId;
                    check.CountryName = Constants.CountryName;
                    check.Specifications = module.Specification;
                    check.SpecificationDate = module.UpdateDate;
                    check.MaterialPrice = Math.Floor(price);
                    check.DeliveryTime = module.Leadtime.ToString();
                    check.SourceId = module.Id;
                    check.IsSync = true;
                    check.Status = true;
                    check.Type = Constants.SaleProductModule;
                    check.UpdateBy = userId;
                    check.UpdateDate = DateTime.Now;

                    if (listImage.Count > 0)
                    {
                        check.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProductModule,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    saleProductMedias = new List<SaleProductMedia>();
                    saleProductMedias = db.SaleProductMedias.Where(i => i.SaleProductId.Equals(check.Id) && i.Type == Constants.SaleProductMedia_Type_LibraryImage).ToList();

                    db.SaleProductMedias.RemoveRange(saleProductMedias);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = check.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncModule, check.Id, check.Model, conten);
                }
                else
                {
                    saleProduct = new SaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        EName = string.Empty,
                        VName = module.Name,
                        Model = module.Code,
                        GroupCode = groupCode,
                        ManufactureId = Constants.ManufactureId,
                        CountryName = Constants.CountryName,
                        Specifications = module.Description,
                        SpecificationDate = module.UpdateDate,
                        MaterialPrice = Math.Floor(price),
                        DeliveryTime = module.Leadtime.ToString(),
                        SourceId = module.Id,
                        IsSync = true,
                        Status = true,
                        Type = Constants.SaleProductModule,
                        CreateDate = DateTime.Now,
                        CreateBy = userId,
                        UpdateDate = DateTime.Now,
                        UpdateBy = userId,
                    };

                    if (listImage.Count > 0)
                    {
                        saleProduct.AvatarPath = listImage.FirstOrDefault().ThumbnailPath;
                    }

                    listSaleProduct.Add(saleProduct);

                    syncSaleProduct = new SyncSaleProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = Constants.SyncSaleProduct_Type_SaleProduct,
                        Date = DateTime.Now
                    };
                    syncSaleProducts.Add(syncSaleProduct);

                    foreach (var item in listImage)
                    {
                        saleProductMedia = new SaleProductMedia()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SaleProductId = saleProduct.Id,
                            Path = item.FilePath,
                            FileName = item.FileName,
                            FileSize = 0,
                            CreateDate = item.CreateDate,
                            Type = Constants.SaleProductMedia_Type_Image
                        };
                        listSaleProductMedia.Add(saleProductMedia);
                    }

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_SyncModule, saleProduct.Id, saleProduct.Model, conten);
                }
            }

            if (listCodeError.Count > 0)
            {
                return "Mã module <" + string.Join(", ", listCodeError) + "> đang thiếu thông tin đồng bộ. Bạn có muốn tiếp tục đồng bộ!";
            }

            using (var trans = db.Database.BeginTransaction())
            {
                db.SaleProducts.AddRange(listSaleProduct);
                db.SyncSaleProducts.AddRange(syncSaleProducts);
                db.SaleProductMedias.AddRange(listSaleProductMedia);
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }

            return null;
        }

        public List<ModuleManualDocumentModel> SearchDocument(ModuleDocumentSearchModel searchModel)
        {
            var dataQuery = (from a in db.Documents.AsNoTracking()
                             join b in db.DocumentGroups.AsNoTracking() on a.DocumentGroupId equals b.Id
                             where b.Code.ToUpper().Equals(searchModel.GroupCode.ToUpper()) && !searchModel.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new ModuleManualDocumentModel
                             {
                                 Id = a.Id,
                                 FileName = a.Name,
                                 Note = a.Description,
                                 IsDocument = true
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(i => i.FileName.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            var result = dataQuery.ToList();
            return result;
        }

        public List<ModuleManualDocumentModel> GetListDocumentFile(string documentId)
        {
            var result = (from a in db.Documents.AsNoTracking()
                          join b in db.DocumentFiles.AsNoTracking() on a.Id equals b.DocumentId
                          where a.Id.Equals(documentId)
                          select new ModuleManualDocumentModel
                          {
                              Id = b.Id,
                              FileName = b.FileName,
                              Path = b.Path
                          }).ToList();

            return result;
        }
    }
}
