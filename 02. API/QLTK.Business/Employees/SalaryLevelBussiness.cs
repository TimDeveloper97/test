using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using NTS.Model.SalaryLevel;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.SalaryLevel
{
    public class SalaryLevelBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm mức lương
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<SalaryLevelResultModel> SearchSalaryLevel(SalaryLevelSearchModel searchModel)
        {
            SearchResultModel<SalaryLevelResultModel> searchResult = new SearchResultModel<SalaryLevelResultModel>();
            var dataQuery = (from a in db.SalaryLevels.AsNoTracking()
                             join b in db.SalaryGroups.AsNoTracking() on a.SalaryGroupId equals b.Id into ab
                             from abn in ab.DefaultIfEmpty()
                             join c in db.SalaryTypes.AsNoTracking() on a.SalaryTypeId equals c.Id into ac
                             from acn in ac.DefaultIfEmpty()
                             select new SalaryLevelResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 Code = a.Code,
                                 Salary = a.Salary,
                                 SalaryGroupId = a.SalaryGroupId,
                                 SalaryTypeId = a.SalaryTypeId,
                                 SalaryGroupName = abn != null ? abn.Name : string.Empty,
                                 SalaryTypeName = acn != null ? acn.Name : string.Empty
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.SalaryGroupId))
            {
                dataQuery = dataQuery.Where(r => searchModel.SalaryGroupId.Equals(r.SalaryGroupId));
            }

            if (!string.IsNullOrEmpty(searchModel.SalaryTypeId))
            {
                dataQuery = dataQuery.Where(r => searchModel.SalaryTypeId.Equals(r.SalaryTypeId));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Xóa mức lương
        /// </summary>
        /// <param name="model"></param>
        public void DeleteSalaryLevel(SalaryLevelModel model)
        {
            var salaryLevelExist = db.SalaryLevels.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (salaryLevelExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryLevel);
            }

            var salaryLevelUsed = db.WorkTypes.AsNoTracking().FirstOrDefault(a => a.SalaryLevelMinId.Equals(model.Id) || a.SalaryLevelMaxId.Equals(model.Id));
            if (salaryLevelUsed != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SalaryLevel);
            }

            try
            {
                db.SalaryLevels.Remove(salaryLevelExist);

                var NameOrCode = salaryLevelExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<SalaryLevelHistoryModel>(salaryLevelExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_SalaryLevel, salaryLevelExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới mức lương
        /// </summary>
        /// <param name="model"></param>
        public void CreateSalaryLevel(SalaryLevelModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var salaryLevelNameExits = db.SalaryLevels.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()) && a.SalaryTypeId.Equals(model.SalaryTypeId) && a.SalaryGroupId.Equals(model.SalaryGroupId));
            if (salaryLevelNameExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SalaryLevel);
            }

            var salaryLevelCodeExits = db.SalaryLevels.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()) && a.SalaryTypeId.Equals(model.SalaryTypeId) && a.SalaryGroupId.Equals(model.SalaryGroupId));
            if (salaryLevelCodeExits != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SalaryLevel);
            }

            try
            {
                NTS.Model.Repositories.SalaryLevel salaryLevel = new NTS.Model.Repositories.SalaryLevel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Code = model.Code,
                    Note = model.Note,
                    Salary = model.Salary,
                    SalaryTypeId = model.SalaryTypeId,
                    SalaryGroupId = model.SalaryGroupId,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.SalaryLevels.Add(salaryLevel);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, salaryLevel.Name, salaryLevel.Id, Constants.LOG_SalaryLevel);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin mức lương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SalaryLevelModel GetInfo(SalaryLevelModel model)
        {
            var resultInfo = db.SalaryLevels.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SalaryLevelModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
                Salary = p.Salary,
                SalaryGroupId = p.SalaryGroupId,
                SalaryTypeId = p.SalaryTypeId
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryLevel);
            }

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật mức lương
        /// </summary>
        /// <param name="model"></param>
        public void Update(SalaryLevelModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();
            var salaryLevelUpdate = db.SalaryLevels.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (salaryLevelUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SalaryLevel);
            }

            var salaryLevelNameExist = db.SalaryLevels.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()) && a.SalaryGroupId.ToUpper().Equals(model.SalaryGroupId.ToUpper()));
            if (salaryLevelNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SalaryLevel);
            }

            var salaryLevelCodeExist = db.SalaryLevels.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()) && a.SalaryGroupId.ToUpper().Equals(model.SalaryGroupId.ToUpper()));
            if (salaryLevelCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SalaryLevel);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<SalaryLevelHistoryModel>(salaryLevelUpdate);

            try
            {
                salaryLevelUpdate.Name = model.Name;
                salaryLevelUpdate.Code = model.Code;
                salaryLevelUpdate.Note = model.Note;
                salaryLevelUpdate.Salary = model.Salary;
                salaryLevelUpdate.SalaryGroupId = model.SalaryGroupId;
                salaryLevelUpdate.SalaryTypeId = model.SalaryTypeId;
                salaryLevelUpdate.UpdateBy = model.UpdateBy;
                salaryLevelUpdate.UpdateDate = DateTime.Now;

                //var jsonApter = AutoMapperConfig.Mapper.Map<SalaryLevelHistoryModel>(salaryLevelUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_SalaryLevel, salaryLevelUpdate.Id, salaryLevelUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public string GetGroupInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/MucLuong_Import_Template.xls"));
            IWorksheet sheet0 = workbook.Worksheets[0];
            IWorksheet sheet1 = workbook.Worksheets[1];
            IWorksheet sheet2 = workbook.Worksheets[2];

            var listGroup = db.SalaryGroups.AsNoTracking().Select(i => i.Name).ToList();
            var listCountrie = db.SalaryTypes.AsNoTracking().Select(i => i.Name).ToList();
            sheet0.Range["F3:F1000"].DataValidation.DataRange = sheet1.Range["A1:A1000"];
            sheet0.Range["G3:G1000"].DataValidation.DataRange = sheet2.Range["A1:A1000"];
            IRange iRangeData = sheet1.FindFirst("<salaryGroupName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<salaryGroupName>", string.Empty);
            IRange iRangeDatas = sheet2.FindFirst("<salaryTypeName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDatas.Text = iRangeDatas.Text.Replace("<salaryTypeName>", string.Empty);
            var listExport = listGroup.OrderBy(a => a).Select((o, i) => new
            {
                o,
            }); ;

            var listExports = listCountrie.OrderBy(a => a).Select((o, i) => new
            {
                o,
            }); ;

            sheet1.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet2.ImportData(listExports, iRangeDatas.Row, iRangeDatas.Column, false);
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "MucLuong_Import_Template" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "MucLuong_Import_Template" + ".xls";

            return resultPathClient;
        }


        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string Name, Code, note, salary, SalaryGroupName, SalaryTypeName;
            //string[] arrListMaterialGroup = { };
            var salaryLeves = db.SalaryLevels.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();

            List<NTS.Model.Repositories.SalaryLevel> list = new List<NTS.Model.Repositories.SalaryLevel>();

            NTS.Model.Repositories.SalaryLevel itemC;

            List<int> rowName = new List<int>();
            List<int> rowCode = new List<int>();
            List<int> rowsalaryFormat = new List<int>();
            List<int> rowsalaryEmpty = new List<int>();
            List<int> rownote = new List<int>();
            List<int> rowCheckName = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<int> rowChecksalary = new List<int>();
            List<int> rowChecknote = new List<int>();
            List<int> rowCheck = new List<int>();
            List<string> rowlocation = new List<string>();
            List<string> stringId = new List<string>();
            List<int> rowsalaryGroupName = new List<int>();
            List<int> rowsalaryTypeName = new List<int>();
            List<int> rowsalaryTypeNames = new List<int>();
            List<int> rowsalaryGroupNames = new List<int>();
            if (rowCount < 3)
            {
                throw NTSException.CreateInstance("File import không đúng. Chọn file khác");
            }

            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new NTS.Model.Repositories.SalaryLevel();
                    itemC.Id = Guid.NewGuid().ToString();

                    Code = sheet[i, 2].Value;
                    Name = sheet[i, 3].Value;
                    salary = sheet[i, 4].Value;
                    note = sheet[i, 5].Value;
                    SalaryGroupName = sheet[i, 6].Value;
                    SalaryTypeName = sheet[i, 7].Value;
                    list.Add(itemC);



                    //Salary
                    try
                    {

                        if (!string.IsNullOrEmpty(salary))
                        {
                            itemC.Salary = Convert.ToDecimal(salary.Trim());
                        }
                        else
                        {
                            rowsalaryEmpty.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowsalaryFormat.Add(i);
                        if (itemC != null)
                        {
                            list.Remove(itemC);
                        }
                        continue;
                    }

                    //note
                    if (!string.IsNullOrEmpty(note))
                    {
                        itemC.Note = note;
                    }
                    //salaryGroupName
                    try
                    {
                        if (!string.IsNullOrEmpty(SalaryGroupName))
                        {
                            var salaryGroup = db.SalaryGroups.AsNoTracking().Where(t => t.Name.ToUpper().Contains(SalaryGroupName.ToUpper())).FirstOrDefault();
                            itemC.SalaryGroupId = salaryGroup.Id;
                            if (salaryGroup == null)
                            {
                                rowsalaryGroupNames.Add(i);
                            }
                        }
                        else
                        {
                            rowsalaryGroupName.Add(i);
                            if (itemC != null)
                            {
                                list.Remove(itemC);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        rowsalaryGroupName.Add(i);
                        if (itemC != null)
                        {
                            list.Remove(itemC);
                        }
                        continue;
                    }
                    //salaryTypeName
                    try
                    {
                        if (!string.IsNullOrEmpty(SalaryTypeName))
                        {
                            var salaryType = db.SalaryTypes.AsNoTracking().Where(t => t.Name.ToUpper().Contains(SalaryTypeName.ToUpper())).FirstOrDefault();
                            itemC.SalaryTypeId = salaryType.Id;
                            if (salaryType == null)
                            {
                                rowsalaryTypeNames.Add(i);
                            }
                        }
                        else
                        {
                            rowsalaryTypeName.Add(i);
                            if (itemC != null)
                            {
                                list.Remove(itemC);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        rowsalaryTypeName.Add(i);
                        if (itemC != null)
                        {
                            list.Remove(itemC);
                        }
                        continue;
                    }

                    //Name
                    try
                    {
                        if (!string.IsNullOrEmpty(Name))
                        {
                            if (db.SalaryLevels.AsNoTracking().Where(o => o.Code.Equals(Code) && o.SalaryTypeId != itemC.SalaryTypeId && o.SalaryGroupId != itemC.SalaryGroupId && !o.Code.Equals(Code)).Count() > 0)
                            {
                                rowCheckName.Add(i);
                            }
                            else
                            {
                                itemC.Name = Name;
                            }
                        }
                        else
                        {
                            rowName.Add(i);
                            if (itemC != null)
                            {
                                list.Remove(itemC);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        rowName.Add(i);
                        if (itemC != null)
                        {
                            list.Remove(itemC);
                        }
                        continue;
                    }

                    //Code
                    try
                    {
                        if (!string.IsNullOrEmpty(Code))
                        {
                            if (db.SalaryLevels.AsNoTracking().Where(o => o.Code.Equals(Code) && o.SalaryTypeId != itemC.SalaryTypeId && o.SalaryGroupId != itemC.SalaryGroupId && !o.Name.Equals(itemC.Name)).Count() > 0)
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
                            if (itemC != null)
                            {
                                list.Remove(itemC);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        rowCode.Add(i);
                        if (itemC != null)
                        {
                            list.Remove(itemC);
                        }
                        continue;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    var salarylevels = db.SalaryLevels.FirstOrDefault(r => r.SalaryGroupId.Equals(itemC.SalaryGroupId) && r.Code.Equals(Code) && r.Name.Equals(Name) && r.SalaryTypeId == itemC.SalaryTypeId);
                    if (salarylevels != null)
                    {
                        db.SalaryLevels.Remove(salarylevels);
                        db.SaveChanges();

                        foreach (var item in list)
                        {
                            if (item.SalaryGroupId.Equals(salarylevels.SalaryGroupId) && item.SalaryTypeId.Equals(salarylevels.SalaryTypeId) && item.Name.Equals(salarylevels.Name) && item.Code.Equals(salarylevels.Code))
                            {
                                item.Id = salarylevels.Id;
                            }


                        }

                    }
                    var salarylevelr = list.Where(r => r.SalaryGroupId.Equals(itemC.SalaryGroupId) && r.Code.Equals(Code) && r.Name.Equals(Name) && r.SalaryTypeId == itemC.SalaryTypeId).ToList();
                    if (salarylevelr.Count >1)
                    {
                       list.Remove(itemC);

                        foreach (var item in list)
                        {
                            if (item.SalaryGroupId.Equals(itemC.SalaryGroupId) && item.SalaryTypeId.Equals(itemC.SalaryTypeId) && item.Name.Equals(itemC.Name) && item.Code.Equals(itemC.Code))
                            {
                                item.Salary = itemC.Salary;
                                item.Note = itemC.Note;
                            }


                        }

                    }


                    #endregion
                }
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }

            db.SalaryLevels.AddRange(list);
            db.SaveChanges();
            workbook.Close();
            excelEngine.Dispose();


            if (rowCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã mức lương dòng <" + string.Join(", ", rowCode) + "> không được phép để trống!");
            }

            if (rowName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên mức lương dòng<" + string.Join(", ", rowName) + "> không được phép để trống!");
            }

            if (rowsalaryEmpty.Count > 0)
            {
                throw NTSException.CreateInstance("Mức lương dòng <" + string.Join(", ", rowsalaryEmpty) + "> không được phép để trống!");
            }

            if (rowsalaryFormat.Count > 0)
            {
                throw NTSException.CreateInstance("Mức lương dòng <" + string.Join(", ", rowsalaryFormat) + "> sai định dạng!");
            }

            if (rowsalaryGroupName.Count > 0)
            {
                throw NTSException.CreateInstance("Nhóm lương dòng <" + string.Join(", ", rowsalaryGroupName) + "> không được phép để trống!");
            }

            if (rowsalaryTypeName.Count > 0)
            {
                throw NTSException.CreateInstance("Ngạch lương dòng <" + string.Join(", ", rowsalaryTypeName) + "> không được phép để trống!");
            }

            if (rowsalaryTypeNames.Count > 0)
            {
                throw NTSException.CreateInstance("Ngạch lương dòng <" + string.Join(", ", rowsalaryGroupName) + "> sai thông tin!");
            }

            if (rowsalaryGroupNames.Count > 0)
            {
                throw NTSException.CreateInstance("Nhóm lương dòng <" + string.Join(", ", rowsalaryGroupNames) + "> sai thông tin!");
            }

            if (rowCheckCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã mức lương dòng <" + string.Join(", ", rowCheckCode) + "> đã tồn tại. Nhập mã khác!");
            }

            if (rowCheckName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên mức lương dòng <" + string.Join(", ", rowCheckName) + "> đã tồn tại. Nhập tên khác!");
            }

        }
    }
}
