using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.TestCriteria;
using NTS.Model.TestCriteriaHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
namespace QLTK.Business.TestCriterias
{
    public class TestCriteriaBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm tiêu chí
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<TestCriteriaResultModel> SearchModel(TestCriterSearchModel modelSearch)
        {
            SearchResultModel<TestCriteriaResultModel> searchResult = new SearchResultModel<TestCriteriaResultModel>();
            try
            {
                var dataQuey = (from a in db.TestCriterias.AsNoTracking()
                                join b in db.TestCriteriaGroups.AsNoTracking() on a.TestCriteriaGroupId equals b.Id
                                orderby a.Code
                                select new TestCriteriaResultModel
                                {
                                    Id = a.Id,
                                    TestCriteriaGroupId = a.TestCriteriaGroupId,
                                    TestCriteriaGroupName = b.Name,
                                    Code = a.Code,
                                    Name = a.Name,
                                    Type = a.DataType,
                                    TechnicalRequirements = a.TechnicalRequirements,
                                    Note = a.Note,
                                    DataType = a.DataType,
                                    CreateBy = a.CreateBy,
                                    CreateDate = a.CreateDate,
                                    UpdateBy = a.UpdateBy,
                                    UpdateDate = a.UpdateDate,
                                }).AsQueryable();
                // Mã nhóm
                if (!string.IsNullOrEmpty(modelSearch.TestCriteriaGroupId))
                {
                    dataQuey = dataQuey.Where(r => r.TestCriteriaGroupId.Contains(modelSearch.TestCriteriaGroupId));
                }
                // Tên
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }
                // Mã
                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuey = dataQuey.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
                }
                // tính năng
                if (!string.IsNullOrEmpty(modelSearch.TechnicalRequirements))
                {
                    dataQuey = dataQuey.Where(r => r.TechnicalRequirements.ToUpper().Contains(modelSearch.TechnicalRequirements.ToUpper()));
                }

                if (modelSearch.DataType.HasValue)
                {
                    dataQuey = dataQuey.Where(r => r.DataType == modelSearch.DataType.Value);
                }

                searchResult.TotalItem = dataQuey.Count();
                var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// Xóa tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void DeleteTestCriteria(TestCriteriaModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.ModuleTestCriterias.AsNoTracking().Where(r => r.TestCriteriaId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.TestCriteria);
                }
                if (db.ProjectProductTestCriterias.AsNoTracking().Where(r => r.TestCriteriaId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.TestCriteria);
                }

                try
                {
                    var testcriteria = db.TestCriterias.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (testcriteria == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TestCriteria);
                    }
                    db.TestCriterias.Remove(testcriteria);

                    var NameOrCode = testcriteria.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<TestCriteriasHistoryModel>(testcriteria);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_TestCriteria, testcriteria.Id, NameOrCode, jsonApter);
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
        /// Thêm mới nhóm tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void AddCriteria(TestCriteriaModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                // Check tên nhóm tiêu chí đã tồn tại chưa
                if (db.TestCriterias.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.TestCriteria);
                }

                // check mã nhóm tiêu chí đã tồn tại chưa
                if (db.TestCriterias.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TestCriteria);
                }
                try
                {
                    TestCriteria newtestCriteria = new TestCriteria()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TestCriteriaGroupId = model.TestCriteriaGroupId,
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        TechnicalRequirements = model.TechnicalRequirements.NTSTrim(),
                        Note = model.Note.NTSTrim(),
                        DataType = model.DataType,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.TestCriterias.Add(newtestCriteria);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Name, newtestCriteria.Id, Constants.LOG_TestCriteria);

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
        /// Get tiêu chí
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public TestCriteriaModel GetTestCriteriaInfo(TestCriteriaModel model)
        {
            try
            {
                var resuldInfor = db.TestCriterias.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new TestCriteriaModel
                {
                    Id = p.Id,
                    TestCriteriaGroupId = p.TestCriteriaGroupId,
                    Code = p.Code,
                    Name = p.Name,
                    TechnicalRequirements = p.TechnicalRequirements,
                    Note = p.Note,
                    DataType = p.DataType
                }).FirstOrDefault();

                if (resuldInfor == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TestCriteria);
                }

                return resuldInfor;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Update tiêu chí
        /// </summary>
        /// <param name="model"></param>
        public void UpdateTestCriteria(TestCriteriaModel model)
        {
            string nameOld = "";
            using (var trans = db.Database.BeginTransaction())
            {
                model.Code = Util.RemoveSpecialCharacter(model.Code);
                if (db.TestCriterias.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.TestCriteria);
                }
                if (db.TestCriterias.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Code.Equals(o.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TestCriteria);
                }
                try
                {
                    var groupEdit = db.TestCriterias.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<TestCriteriasHistoryModel>(groupEdit);

                    nameOld = groupEdit.Name.NTSTrim();
                    groupEdit.Code = model.Code.NTSTrim();
                    groupEdit.TestCriteriaGroupId = model.TestCriteriaGroupId;
                    groupEdit.Name = model.Name.NTSTrim();
                    groupEdit.Note = model.Note.NTSTrim();
                    groupEdit.DataType = model.DataType;
                    groupEdit.TechnicalRequirements = model.TechnicalRequirements.NTSTrim();
                    groupEdit.UpdateBy = model.CreateBy;
                    groupEdit.UpdateDate = DateTime.Now;
                    groupEdit.CreateBy = model.CreateBy;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<TestCriteriasHistoryModel>(groupEdit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_TestCriteria, groupEdit.Id, groupEdit.Code, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            try
            {
                string decription = String.Empty;
                if (nameOld.ToLower() == model.Name.ToLower())
                {
                    decription = "Cập nhật tiêu chí tên là: " + nameOld;
                }
                else
                {
                    decription = "Cập nhật tiêu chí có tên ban đầu là:  " + nameOld + " thành " + model.Name; ;
                }
                LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
            }
            catch (Exception) { }
        }

        public string ExcelTestCriteria(TestCriteriaModel model)
        {

            var dataQuey = (from a in db.TestCriterias.AsNoTracking()
                            join b in db.TestCriteriaGroups.AsNoTracking() on a.TestCriteriaGroupId equals b.Id
                            orderby a.Code
                            select new TestCriteriaResultModel
                            {
                                Id = a.Id,
                                TestCriteriaGroupId = a.TestCriteriaGroupId,
                                TestCriteriaGroupName = b.Name,
                                Code = a.Code,
                                Name = a.Name,
                                TechnicalRequirements = a.TechnicalRequirements,
                                Note = a.Note,
                            }).AsQueryable();
            if (!string.IsNullOrEmpty(model.TestCriteriaGroupId))
            {
                dataQuey = dataQuey.Where(r => r.TestCriteriaGroupId.Contains(model.TestCriteriaGroupId));
            }
            // Tên
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(model.Name.ToUpper()));
            }
            // Mã
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuey = dataQuey.Where(r => r.Code.ToUpper().Contains(model.Code.ToUpper()));
            }
            // tính năng
            if (!string.IsNullOrEmpty(model.TechnicalRequirements))
            {
                dataQuey = dataQuey.Where(r => r.TechnicalRequirements.ToUpper().Contains(model.TechnicalRequirements.ToUpper()));
            }
            List<TestCriteriaResultModel> listModel = dataQuey.ToList();
            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TestCri.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.OrderBy(i => i.Name).Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.TestCriteriaGroupName,
                    a.TechnicalRequirements,
                    a.Note,
                });


                if (listExport.Count() > 1)
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách tiêu chí" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách tiêu chí" + ".xls";

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
