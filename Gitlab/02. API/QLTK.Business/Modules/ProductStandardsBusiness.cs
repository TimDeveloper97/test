using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Products.ProductStandards;
using NTS.Model.ProductStandardHistory;
using NTS.Model.ProductStandards;
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

namespace QLTK.Business.Productstandards
{
    public class ProductStandardsBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ProductStandardsModel> SearchModel(ProductStandardsSearchModel modelSearch)
        {
            SearchResultModel<ProductStandardsModel> searchResult = new SearchResultModel<ProductStandardsModel>();

            var dataQuey = (from a in db.ProductStandards.AsNoTracking()
                            join b in db.ProductStandardGroups.AsNoTracking() on a.ProductStandardGroupId equals b.Id
                            join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id
                            join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                            join e in db.Users.AsNoTracking() on a.CreateBy equals e.Id
                            join f in db.Employees.AsNoTracking() on e.EmployeeId equals f.Id
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
                                DataType = a.DataType,
                                CreateBy = a.CreateBy,
                                CreateByName = f.Name,
                                CreateDate = a.CreateDate,
                                UpdateBy = a.UpdateBy,
                                UpdateDate = a.UpdateDate
                            }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.ProductStandardGroupId))
            {
                dataQuey = dataQuey.Where(r => r.ProductStandardGroupId.Equals(modelSearch.ProductStandardGroupId));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuey = dataQuey.Where(r => r.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuey = dataQuey.Where(r => r.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            //if (!string.IsNullOrEmpty(modelSearch.Name))
            //{
            //    dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            //}

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuey = dataQuey.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || r.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.CreateByName))
            {
                dataQuey = dataQuey.Where(r => r.CreateByName.ToUpper().Contains(modelSearch.CreateByName.ToUpper()));
            }

            if (modelSearch.DataType.HasValue)
            {
                dataQuey = dataQuey.Where(r => r.DataType == modelSearch.DataType.Value);
            }

            searchResult.TotalItem = dataQuey.Count();
            var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public bool AddProductStandards(ProductStandardsModel model)
        {
            bool rs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProductStandard newProductStandard = new ProductStandard
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductStandardGroupId = model.ProductStandardGroupId,
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmentId,
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Content = model.Content,
                        Target = model.Target,
                        Note = model.Note,
                        Version = model.Version,
                        EditContent = model.EditContent,
                        DataType = model.DataType,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };
                    var listOkImage = (from s in model.ListImage
                                       select s.FilePath).ToList();
                    var lisNGImage = (from s in model.ListImageV
                                      select s.FilePath).ToList();
                    newProductStandard.OK_Images = string.Join(",", listOkImage);
                    newProductStandard.NG_Images = string.Join(",", lisNGImage);
                    db.ProductStandards.Add(newProductStandard);
                    //Add file
                    if (model.ListFile.Count > 0)
                    {
                        List<ProductStandardAttach> listFileEntity = new List<ProductStandardAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                ProductStandardAttach fileEntity = new ProductStandardAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductStandardId = newProductStandard.Id,
                                    Path = item.Path,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    CreateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.ProductStandardAttaches.AddRange(listFileEntity);
                    }

                    UserLogUtil.LogHistotyAdd(db, newProductStandard.CreateBy, newProductStandard.Code, newProductStandard.Id, Constants.LOG_ProductStandard);

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

        public bool UpdateProductStandard(ProductStandardsModel model)
        {
            bool rs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newProductStandard = db.ProductStandards.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    //var jsonApter = AutoMapperConfig.Mapper.Map<ProductStandardLogHistoryModel>(newProductStandard);


                    newProductStandard.ProductStandardGroupId = model.ProductStandardGroupId;
                    newProductStandard.SBUId = model.SBUId;
                    newProductStandard.DepartmentId = model.DepartmentId;
                    newProductStandard.Name = model.Name.Trim();
                    newProductStandard.Code = model.Code.Trim();
                    newProductStandard.Content = model.Content;
                    newProductStandard.Target = model.Target;
                    newProductStandard.Note = model.Note;
                    newProductStandard.EditContent = model.EditContent;
                    newProductStandard.DataType = model.DataType;
                    newProductStandard.UpdateBy = model.UpdateBy;
                    newProductStandard.UpdateDate = DateTime.Now;
                    var fileEntities = db.ProductStandardAttaches.Where(t => t.ProductStandardId.Equals(model.Id));

                    var listOkImage = (from s in model.ListImage
                                       select s.FilePath).ToList();
                    var lisNGImage = (from s in model.ListImageV
                                      select s.FilePath).ToList();
                    newProductStandard.OK_Images = string.Join(",", listOkImage);
                    newProductStandard.NG_Images = string.Join(",", lisNGImage);

                    db.ProductStandardAttaches.RemoveRange(fileEntities);
                    if (model.ListFile.Count > 0)
                    {
                        List<ProductStandardAttach> listFileEntity = new List<ProductStandardAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                ProductStandardAttach fileEntity = new ProductStandardAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductStandardId = newProductStandard.Id,
                                    FileName = item.FileName,
                                    Path = item.Path,
                                    FileSize = item.FileSize,
                                    CreateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.ProductStandardAttaches.AddRange(listFileEntity);
                    }
                    // UserLogUtil.LogHistotyUpdateSub(db, newProductStandard.UpdateBy, Constants.LOG_ProductStandard, newProductStandard.Id, string.Empty, "Lịch sử");

                    if (!string.IsNullOrEmpty(model.Version) && model.Version != newProductStandard.Version)
                    {
                        ProductStandardHistory history = new ProductStandardHistory
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductStandardId = newProductStandard.Id,
                            Version = model.Version,
                            EditContent = model.EditContent,
                            CreateBy = newProductStandard.CreateBy,
                            CreateDate = DateTime.Now
                        };
                        db.ProductStandardHistories.Add(history);
                    }
                    newProductStandard.Version = model.Version;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ProductStandardLogHistoryModel>(newProductStandard);

                    //UserLogUtil.LogHistotyUpdateInfo(db, newProductStandard.UpdateBy, Constants.LOG_ProductStandard, newProductStandard.Id, newProductStandard.Name, jsonBefor, jsonApter);

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

        public void DeleteProductStandard(ProductStandardsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var productStandard = db.ModuleGroupProductStandards.AsNoTracking().Where(m => m.ProductStandardId.Equals(model.Id)).FirstOrDefault();
                if (productStandard != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductStandard);
                }
                try
                {
                    var productstandard = db.ProductStandards.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (productstandard == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductStandard);
                    }
                    var file = db.ProductStandardAttaches.Where(u => u.ProductStandardId.Equals(model.Id)).ToList();
                    var history = db.ProductStandardHistories.Where(u => u.ProductStandardId.Equals(model.Id)).ToList();
                    if (file != null)
                    {
                        db.ProductStandardAttaches.RemoveRange(file);
                    }
                    db.ProductStandardHistories.RemoveRange(history);
                    db.ProductStandards.Remove(productstandard);

                    var NameOrCode = productstandard.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ProductStandardLogHistoryModel>(productstandard);
                    //UserLogUtil.LogHistotyDelete(db, productstandard.UpdateBy, Constants.LOG_ProductStandard, productstandard.Id, NameOrCode, jsonApter);
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

        public object GetSBUIdandDeepartmentId(string userId)
        {
            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        join c in db.Departments.AsNoTracking() on b.DepartmentId equals c.Id
                        join d in db.SBUs.AsNoTracking() on c.SBUId equals d.Id
                        select new { c.Id, c.SBUId }).FirstOrDefault();
            return data;
        }

        public object GetProductStandardInfo(ProductStandardsModel model)
        {
          
            try
            {
                var resultInfo = db.ProductStandards.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProductStandardsModel
                {
                    Id = p.Id,
                    ProductStandardGroupId = p.ProductStandardGroupId,
                    SBUId = p.SBUId,
                    DepartmentId = p.DepartmentId,
                    Code = p.Code,
                    Name = p.Name,
                    Content = p.Content,
                    Target = p.Target,
                    Note = p.Note,
                    Version = p.Version,
                    EditContent = p.EditContent,
                    DataType = p.DataType,
                    OK_Images = p.OK_Images,
                    NG_Images = p.NG_Images

                }).FirstOrDefault();
                if (resultInfo != null)
                {
                    if (!string.IsNullOrEmpty(resultInfo.OK_Images)) {
                        List<string> OkImage = resultInfo.OK_Images.Split(',').ToList();
                        foreach (var item in OkImage)
                        {
                            resultInfo.ListImage.Add(new ProductStandardImageModel() { FilePath = item, ThumbnailPath = item });

                        }
                    }
                    if (!string.IsNullOrEmpty(resultInfo.OK_Images))
                    {
                        List<string> NgImage = resultInfo.NG_Images.Split(',').ToList();
                        foreach (var item in NgImage)
                        {
                            resultInfo.ListImageV.Add(new ProductStandardImageModel() { FilePath = item, ThumbnailPath = item });

                        }
                    }

                }





                if (resultInfo == null)
                {
                    throw NTSException.CreateInstance("Nhóm tiêu chuẩn này đã bị xóa bởi người dùng khác");
                }

                resultInfo.ListFile = db.ProductStandardAttaches.AsNoTracking().Where(t => t.ProductStandardId.Equals(resultInfo.Id)).Select(m => new ProductStandardAttachModel
                {
                    Id = m.Id,
                    ProductStandardId = m.ProductStandardId,
                    FileName = m.FileName,
                    Path = m.Path,
                    Note = m.Note,
                    FileSize = m.FileSize.Value,
                    CreateDate = m.CreateDate
                }).ToList();

                var listHistory = (from a in db.ProductStandardHistories.AsNoTracking()
                                   join b in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals b.Id
                                   join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                                   join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                   where a.ProductStandardId.Equals(model.Id)
                                   orderby a.CreateDate descending
                                   select new ProductStandardHistoryModel
                                   {
                                       Id = a.Id,
                                       ProductStandardId = a.ProductStandardId,
                                       Version = a.Version,
                                       EditContent = a.EditContent,
                                       CreateBy = a.CreateBy,
                                       CreateByName = d.Name,
                                       CreateDate = a.CreateDate,
                                   }).ToList();

                resultInfo.ListHistory = listHistory;
                return resultInfo;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public string ExportExcel(ProductStandardsSearchModel model)
        {
            model.IsExport = true;
            var dataQuey = (from a in db.ProductStandards.AsNoTracking()
                            join b in db.ProductStandardGroups.AsNoTracking() on a.ProductStandardGroupId equals b.Id
                            join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id
                            join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                            join e in db.Users.AsNoTracking() on a.CreateBy equals e.Id
                            join f in db.Employees.AsNoTracking() on e.EmployeeId equals f.Id
                            orderby a.Code
                            select new ProductStandardsResultModel
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
                                CreateByName = f.Name,
                                CreateDate = a.CreateDate,
                                UpdateBy = a.UpdateBy,
                                UpdateDate = a.UpdateDate,
                            }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ProductStandardGroupId))
            {
                dataQuey = dataQuey.Where(r => r.ProductStandardGroupId.Equals(model.ProductStandardGroupId));
            }
            if (!string.IsNullOrEmpty(model.SBUId))
            {
                dataQuey = dataQuey.Where(r => r.SBUId.Equals(model.SBUId));
            }
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuey = dataQuey.Where(r => r.DepartmentId.Equals(model.DepartmentId));
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(model.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuey = dataQuey.Where(r => r.Code.ToUpper().Contains(model.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.CreateByName))
            {
                dataQuey = dataQuey.Where(r => r.CreateByName.ToUpper().Contains(model.CreateByName.ToUpper()));
            }

            List<ProductStandardsResultModel> listModel = dataQuey.ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProductStandards.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Code,
                    a.Name,
                    a.ProductStandardGroupName,
                    a.SBUName,
                    a.DepartmentName,
                    a.Content,
                    shet1 = DateTimeHelper.ToStringDDMMYY(a.CreateDate.Value),
                    a.Version,
                    a.EditContent,
                    a.CreateByName,
                });

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách tiêu chuẩn" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách tiêu chuẩn" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new NTSLogException(model, ex);
            }
        }
        private void CheckExistedForAdd(ProductStandardsModel model)
        {
            if (db.ProductStandards.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductStandard);
            }

            if (db.ProductStandards.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandard);
            }
        }

        public void CheckExistedForUpdate(ProductStandardsModel model)
        {
            if (db.ProductStandards.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductStandard);
            }

            if (db.ProductStandards.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandard);
            }
        }

        public object GetShowQCProductStandardInfo(ProductStandardsModel model)
        {

            try
            {
                var resultInfo = db.QCCheckLists.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ProductStandardsModel
                {
                    Id = p.Id,
                    ProductStandardGroupId = p.ProductStandardGroupId,
                    Code = p.Code,
                    Name = p.Name,
                    Content = p.Content,
                    Target = p.Target,
                    Note = p.Note,
                    DataType = p.DataType,
                    OK_Images = p.OK_Images,
                    NG_Images = p.NG_Images

                }).FirstOrDefault();
                if (resultInfo != null)
                {
                    if (!string.IsNullOrEmpty(resultInfo.OK_Images))
                    {
                        List<string> OkImage = resultInfo.OK_Images.Split(',').ToList();
                        foreach (var item in OkImage)
                        {
                            resultInfo.ListImage.Add(new ProductStandardImageModel() { FilePath = item, ThumbnailPath = item });

                        }
                    }
                    if (!string.IsNullOrEmpty(resultInfo.NG_Images))
                    {
                        List<string> NgImage = resultInfo.NG_Images.Split(',').ToList();
                        foreach (var item in NgImage)
                        {
                            resultInfo.ListImageV.Add(new ProductStandardImageModel() { FilePath = item, ThumbnailPath = item });

                        }
                    }
                }

                if (resultInfo == null)
                {
                    throw NTSException.CreateInstance("Nhóm tiêu chuẩn này đã bị xóa bởi người dùng khác");
                }

                resultInfo.ListAttach = db.Attachments.AsNoTracking().Where(t => t.ObjectId.Equals(resultInfo.Id)).Select(m => new AttachmentModel
                {
                    Id = m.Id,
                    FileName = m.FileName,
                    FilePath = m.FilePath,
                    Size = m.Size,
                    CreateDate = m.CreateDate
                }).ToList();

                var listHistory = (from a in db.ProductStandardHistories.AsNoTracking()
                                   join b in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals b.Id
                                   join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                                   join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                   where a.ProductStandardId.Equals(model.Id)
                                   orderby a.CreateDate descending
                                   select new ProductStandardHistoryModel
                                   {
                                       Id = a.Id,
                                       ProductStandardId = a.ProductStandardId,
                                       Version = a.Version,
                                       EditContent = a.EditContent,
                                       CreateBy = a.CreateBy,
                                       CreateByName = d.Name,
                                       CreateDate = a.CreateDate,
                                   }).ToList();

                resultInfo.ListHistory = listHistory;
                return resultInfo;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }


    }
}
