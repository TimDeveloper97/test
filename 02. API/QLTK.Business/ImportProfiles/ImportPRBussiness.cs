using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ImportPR;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.ImportPR
{
    public class ImportPRBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ImportPRModel> SearchPR(ImportPRSearchModel searchModel)
        {
            SearchResultModel<ImportPRModel> searchResult = new SearchResultModel<ImportPRModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.PurchaseRequestProducts.AsNoTracking()
                             join b in db.PurchaseRequests.AsNoTracking() on a.PurchaseRequestId equals b.Id
                             join c in db.Manufactures.AsNoTracking() on a.ManufactureId equals c.Id
                             join d in db.Employees.AsNoTracking() on a.SalesBy equals d.Id
                             select new ImportPRModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 ManufactureCode = c.Code,
                                 ParentCode = a.ParentCode,
                                 Quantity = a.Quantity,
                                 RequireDate = a.RequireDate,
                                 ProjectCode = a.ProjectCode,
                                 ProjectName = a.ProjectName,
                                 SalesBy = a.SalesBy,
                                 SalesName = d.Name,
                                 PurchaseRequestCode = b.Code,
                                 Status = a.Status,
                                 UnitName = a.UnitName,
                                 ManufactureId = c.Id,
                                 QuotaPrice = a.QuotaPrice
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()) || u.Name.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectCode))
            {
                dataQuery = dataQuery.Where(u => u.ProjectCode.ToUpper().Contains(searchModel.ProjectCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.PRCode))
            {
                dataQuery = dataQuery.Where(u => u.PurchaseRequestCode.ToUpper().Contains(searchModel.PRCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ManufactureId))
            {
                dataQuery = dataQuery.Where(u => searchModel.ManufactureId.Contains(u.ManufactureId));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.SalesBy));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.SalesBy));
            }

            if (searchModel.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.RequireDate >= searchModel.DateFrom.Value);
            }

            if (searchModel.DateTo.HasValue)
            {
                var dateTo = searchModel.DateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(r => r.RequireDate <= dateTo);
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), searchModel.OrderBy, searchModel.OrderType).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<ImportPRModel> SearchChoosePR(ImportPRSearchModel searchModel)
        {
            SearchResultModel<ImportPRModel> searchResult = new SearchResultModel<ImportPRModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.PurchaseRequestProducts.AsNoTracking()
                             where !a.Status
                             join b in db.PurchaseRequests.AsNoTracking() on a.PurchaseRequestId equals b.Id
                             join c in db.Manufactures.AsNoTracking() on a.ManufactureId equals c.Id
                             join d in db.Employees.AsNoTracking() on a.SalesBy equals d.Id
                             where !searchModel.ListId.Contains(a.Id)
                             select new ImportPRModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 ManufactureCode = c.Code,
                                 ParentCode = a.ParentCode,
                                 Quantity = a.Quantity,
                                 RequireDate = a.RequireDate,
                                 ProjectCode = a.ProjectCode,
                                 ProjectName = a.ProjectName,
                                 SalesBy = a.SalesBy,
                                 SalesName = d.Name,
                                 PurchaseRequestCode = b.Code,
                                 Status = a.Status,
                                 UnitName = a.UnitName,
                                 ManufactureId = c.Id,
                                 QuotaPrice = a.QuotaPrice
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()) || u.Name.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectCode))
            {
                dataQuery = dataQuery.Where(u => u.ProjectCode.ToUpper().Contains(searchModel.ProjectCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.PRCode))
            {
                dataQuery = dataQuery.Where(u => u.PurchaseRequestCode.ToUpper().Contains(searchModel.PRCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ManufactureId))
            {
                dataQuery = dataQuery.Where(u => searchModel.ManufactureId.Contains(u.ManufactureId));
            }

            //if (searchModel.Status.HasValue)
            //{
            //    dataQuery = dataQuery.Where(u => u.Status == searchModel.Status.Value);
            //}

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.SalesBy));
            }

            if (searchModel.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.RequireDate >= searchModel.DateFrom.Value);
            }

            if (searchModel.DateTo.HasValue)
            {
                var dateTo = searchModel.DateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(r => r.RequireDate <= dateTo);
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), searchModel.OrderBy, searchModel.OrderType).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void DeleteProduct(ImportPRModel productModel)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var product = db.PurchaseRequestProducts.FirstOrDefault(r => r.Id.Equals(productModel.Id));

                    if (product == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Material);
                    }

                    if (product.Status)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0078);
                    }

                    db.PurchaseRequestProducts.Remove(product);

                    db.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new NTSLogException(productModel, ex);
                }
            }
        }

        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string itemCode, itemName, fatherCode, unitName, manufactureCode, requireQuantity, requireDate, saleEmployee, project, projectName, prCode, quotaPrice;
            string[] arrListMaterialGroup = { };
            #region[doc du lieu tu excel]


            PurchaseRequestProduct purchaseRequestProduct;
            PurchaseRequest purchaseRequest;
            PurchaseRequest purchaseRequestExit;
            List<int> rowItemCode = new List<int>();
            List<int> rowItemName = new List<int>();
            List<int> rowFatherCode = new List<int>();
            List<int> rowUnitName = new List<int>();
            List<int> rowManufactureCode = new List<int>();
            List<int> rowManufactureCodeNotExist = new List<int>();
            List<int> rowRequireQuantity = new List<int>();
            List<int> rowRequireDate = new List<int>();
            List<int> rowRequireDateError = new List<int>();
            List<int> rowSaleEmployeeNotExist = new List<int>();
            List<int> rowSaleEmployee = new List<int>();
            List<int> rowProject = new List<int>();
            List<int> rowProjectName = new List<int>();
            List<int> rowPrCode = new List<int>();
            List<int> rowQuotaPrice = new List<int>();
            List<PurchaseRequest> listPR = new List<PurchaseRequest>();
            List<PurchaseRequestProduct> listPRProduct = new List<PurchaseRequestProduct>();
            List<PurchaseRequestProduct> listPRProductUpdate = new List<PurchaseRequestProduct>();

            PurchaseRequestProduct purchaseRequestProductExist;
            Employee employeeExist;
            Manufacture manufactureExist;
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(file.InputStream);
                IWorksheet sheet = workbook.Worksheets[0];
                sheet.EnableSheetCalculations();
                int rowCount = sheet.Rows.Count();

                if (rowCount < 2)
                {
                    throw NTSException.CreateInstance("File import không đúng. Chọn file khác");
                }

                try
                {
                    for (int i = 2; i <= rowCount; i++)
                    {
                        purchaseRequestProduct = new PurchaseRequestProduct();
                        purchaseRequestProduct.Id = Guid.NewGuid().ToString();
                        purchaseRequest = new PurchaseRequest();
                        purchaseRequest.Id = Guid.NewGuid().ToString();
                        itemCode = sheet[i, 1].Value;
                        itemName = sheet[i, 2].Value;
                        fatherCode = sheet[i, 3].Value;
                        unitName = sheet[i, 4].Value;
                        manufactureCode = sheet[i, 5].Value;
                        requireQuantity = sheet[i, 6].DisplayText;
                        requireDate = sheet[i, 7].Value;
                        saleEmployee = sheet[i, 8].Value;
                        project = sheet[i, 9].Value;
                        projectName = sheet[i, 10].Value;
                        prCode = sheet[i, 11].Value;
                        quotaPrice = sheet[i, 12].Value;

                        if(string.IsNullOrEmpty(itemCode) && string.IsNullOrEmpty(itemName) && string.IsNullOrEmpty(fatherCode) && string.IsNullOrEmpty(unitName) && string.IsNullOrEmpty(manufactureCode) && string.IsNullOrEmpty(requireQuantity) && string.IsNullOrEmpty(requireDate) && string.IsNullOrEmpty(saleEmployee) && string.IsNullOrEmpty(project) && string.IsNullOrEmpty(prCode) && string.IsNullOrEmpty(quotaPrice))
                        {
                            continue;
                        }    

                        //itemName
                        if (!string.IsNullOrEmpty(itemName))
                        {
                            purchaseRequestProduct.Name = itemName;
                        }
                        else
                        {
                            rowItemCode.Add(i);
                        }

                        //fatherCode
                        purchaseRequestProduct.ParentCode = fatherCode;

                        //ManufactureCode
                        if (!string.IsNullOrEmpty(manufactureCode))
                        {
                            manufactureExist = db.Manufactures.AsNoTracking().FirstOrDefault(t => manufactureCode.Equals(t.Code));
                            if (manufactureExist != null)
                            {
                                purchaseRequestProduct.ManufactureId = manufactureExist.Id;
                            }
                            else
                            {
                                rowManufactureCodeNotExist.Add(i);
                                continue;
                            }
                        }
                        else
                        {
                            rowManufactureCode.Add(i);
                            continue;
                        }

                        //RequireDate
                        try
                        {
                            if (!string.IsNullOrEmpty(requireDate))
                            {
                                purchaseRequestProduct.RequireDate = DateTime.ParseExact(requireDate.Trim(), "dd.MM.yy", null);
                            }
                            else
                            {
                                rowRequireDate.Add(i);
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            rowRequireDateError.Add(i);
                            continue;
                        }

                        //requireQuantity
                        try
                        {
                            if (!string.IsNullOrEmpty(requireQuantity))
                            {
                                purchaseRequestProduct.Quantity = Convert.ToInt32(requireQuantity.Trim());
                            }
                            else
                            {
                                rowRequireQuantity.Add(i);
                                continue;
                            }
                        }
                        catch (Exception)
                        {
                            rowRequireQuantity.Add(i);
                            continue;
                        }

                        //saleEmployee
                        if (!string.IsNullOrEmpty(saleEmployee))
                        {
                            employeeExist = db.Employees.AsNoTracking().FirstOrDefault(t => saleEmployee.Trim().ToUpper().Equals(t.Name.ToUpper()));
                            if (employeeExist != null)
                            {
                                purchaseRequestProduct.SalesBy = employeeExist.Id;
                            }
                            else
                            {
                                rowSaleEmployeeNotExist.Add(i);
                                continue;
                            }
                        }
                        else
                        {
                            rowSaleEmployee.Add(i);
                            continue;
                        }

                        //project
                        if (!string.IsNullOrEmpty(project))
                        {
                            purchaseRequestProduct.ProjectCode = project.Trim(); ;
                        }
                        else
                        {
                            rowProject.Add(i);
                            continue;
                        }

                        //project
                        if (!string.IsNullOrEmpty(projectName))
                        {
                            purchaseRequestProduct.ProjectName = projectName.Trim(); ;
                        }
                        else
                        {
                            rowProjectName.Add(i);
                            continue;
                        }

                        //unitName
                        purchaseRequestProduct.UnitName = unitName;

                        //PRCode
                        if (!string.IsNullOrEmpty(prCode))
                        {
                            purchaseRequestProduct.PRCode = prCode;
                            purchaseRequestExit = db.PurchaseRequests.AsNoTracking().FirstOrDefault(t => prCode.Equals(t.Code));
                            if (purchaseRequestExit != null)
                            {
                                purchaseRequestProduct.PurchaseRequestId = purchaseRequestExit.Id;
                            }
                            else
                            {
                                purchaseRequestProduct.PurchaseRequestId = purchaseRequest.Id;
                                purchaseRequest.Code = prCode;
                                purchaseRequest.CreateBy = userId;
                                purchaseRequest.UpdateBy = userId;
                                purchaseRequest.CreateDate = DateTime.Now;
                                purchaseRequest.UpdateDate = DateTime.Now;

                                listPR.Add(purchaseRequest);
                            }
                        }
                        else
                        {
                            rowPrCode.Add(i);
                            continue;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(quotaPrice))
                            {
                                purchaseRequestProduct.QuotaPrice = decimal.Parse(quotaPrice);
                            }
                        }
                        catch
                        {
                            rowQuotaPrice.Add(i);
                            continue;
                        }


                        //itemCode
                        if (!string.IsNullOrEmpty(itemCode))
                        {
                            purchaseRequestProductExist = db.PurchaseRequestProducts.AsNoTracking().FirstOrDefault(t => itemCode.Equals(t.Code) && t.PRCode.Equals(prCode));
                            if (purchaseRequestProductExist != null)
                            {
                                listPRProductUpdate.Add(new PurchaseRequestProduct
                                {
                                    Id = purchaseRequestProductExist.Id,
                                    Name = purchaseRequestProduct.Name,
                                    ParentCode = purchaseRequestProduct.ParentCode,
                                    ManufactureId = purchaseRequestProduct.ManufactureId,
                                    ProjectCode = purchaseRequestProduct.ProjectCode,
                                    ProjectName = purchaseRequestProduct.ProjectName,
                                    Quantity = purchaseRequestProduct.Quantity,
                                    RequireDate = purchaseRequestProduct.RequireDate,
                                    UnitName = purchaseRequestProduct.UnitName,
                                    SalesBy = purchaseRequestProduct.SalesBy
                                }); ;
                                continue;
                            }
                            else
                            {
                                purchaseRequestProduct.Code = itemCode.Trim();
                            }
                        }
                        else
                        {
                            rowItemCode.Add(i);
                        }

                        listPRProduct.Add(purchaseRequestProduct);
                    }

                    #endregion

                    workbook.Close();
                    excelEngine.Dispose();
                }
                catch (Exception ex)
                {
                    workbook.Close();
                    excelEngine.Dispose();
                    throw new NTSLogException(null, ex);
                }
            }


            if (rowItemCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã vật tư <" + string.Join(", ", rowItemCode) + "> không được phép để trống!");
            }

            if (rowItemName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên vật tư <" + string.Join(", ", rowItemName) + "> không được phép để trống!");
            }

            if (rowManufactureCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCode) + "> không được phép để trống!");
            }

            if (rowRequireQuantity.Count > 0)
            {
                throw NTSException.CreateInstance("Số lượng yêu cầu dòng <" + string.Join(", ", rowRequireQuantity) + "> không được phép để trống!");
            }

            if (rowRequireDate.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày yêu cầu dòng <" + string.Join(", ", rowRequireDate) + "> không được phép để trống!");
            }

            if (rowRequireDateError.Count > 0)
            {
                throw NTSException.CreateInstance("Ngày yêu cầu dòng <" + string.Join(", ", rowRequireDateError) + "> không đúng định dạng!");
            }

            if (rowProject.Count > 0)
            {
                throw NTSException.CreateInstance("Mã dự án dòng <" + string.Join(", ", rowProject) + "> không được phép để trống!");
            }

            if (rowProjectName.Count > 0)
            {
                throw NTSException.CreateInstance("Tên dự án dòng <" + string.Join(", ", rowProjectName) + "> không được phép để trống!");
            }

            if (rowPrCode.Count > 0)
            {
                throw NTSException.CreateInstance("Mã PR dòng <" + string.Join(", ", rowPrCode) + "> không được phép để trống!");
            }

            if (rowSaleEmployee.Count > 0)
            {
                throw NTSException.CreateInstance("Nhân viên bán hàng dòng <" + string.Join(", ", rowSaleEmployee) + "> không được phép để trống!");
            }

            if (rowQuotaPrice.Count > 0)
            {
                throw NTSException.CreateInstance("Giá định mức dòng <" + string.Join(", ", rowSaleEmployee) + "> không đúng!");
            }

            if (rowManufactureCodeNotExist.Count > 0)
            {
                throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCodeNotExist) + "> không tồn tại!");
            }

            if (rowSaleEmployeeNotExist.Count > 0)
            {
                throw NTSException.CreateInstance("Nhân viên dòng <" + string.Join(", ", rowSaleEmployeeNotExist) + "> không tồn tại!");
            }

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    db.PurchaseRequests.AddRange(listPR);
                    db.PurchaseRequestProducts.AddRange(listPRProduct);

                    foreach (var item in listPRProductUpdate)
                    {
                        purchaseRequestProductExist = db.PurchaseRequestProducts.FirstOrDefault(r => r.Id.Equals(item.Id));

                        purchaseRequestProductExist.Name = item.Name;
                        purchaseRequestProductExist.ParentCode = item.ParentCode;
                        purchaseRequestProductExist.Quantity = item.Quantity;
                        purchaseRequestProductExist.RequireDate = item.RequireDate;
                        purchaseRequestProductExist.SalesBy = item.SalesBy;
                        purchaseRequestProductExist.ParentCode = item.ParentCode;
                        purchaseRequestProductExist.ManufactureId = item.ManufactureId;
                        purchaseRequestProductExist.UnitName = item.UnitName;
                    }

                    db.SaveChanges();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw (ex);
                }
            }
        }
    }
}
