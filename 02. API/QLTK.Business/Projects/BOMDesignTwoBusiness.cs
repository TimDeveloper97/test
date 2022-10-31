using AutoMapper.Mappers;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.BOMDesignTwo;
using NTS.Model.BOMDesignTwoAttach;
using NTS.Model.BOMDesignTwoDetials;
using NTS.Model.Combobox;
using NTS.Model.DMVTImportSAP;
using NTS.Model.Materials;
using NTS.Model.ProjectProducts;
using NTS.Model.Repositories;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using QLTK.Business.DMVTimportSAP;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.BOMDesignTwos
{
    public class BOMDesignTwoBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        private readonly DMVTImportSAPBussiness _business = new DMVTImportSAPBussiness();

        public SearchResultModel<BOMDesignTwoModel> SearchBOMDesignTwo(BOMDesignTwoModel model)
        {
            SearchResultModel<BOMDesignTwoModel> searchResult = new SearchResultModel<BOMDesignTwoModel>();

            var dataQuery = (from a in db.BOMDesignTwoes.AsNoTracking()
                             join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                             join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                             where a.ProjectProductId.Equals(model.ProjectProductId)
                             orderby a.Version descending
                             select new BOMDesignTwoModel
                             {
                                 Id = a.Id,
                                 ProjectProductId = a.ProjectProductId,
                                 Version = a.Version,
                                 Index = a.Index,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 CreateByName = c.Name,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            foreach (var item in listResult)
            {
                var list = (from a in db.BOMDesignTwoAttaches.AsNoTracking()
                            where a.BOMDesignTwoId.Equals(item.Id)
                            select new BOMDesignTwoAttachModel
                            {
                                Id = a.Id,
                                BOMDesignTwoId = a.BOMDesignTwoId,
                                FileName = a.FileName,
                                Path = a.Path,
                                FileSize = a.FileSize
                            }).ToList();
                item.ListFile = list;
            }
            if (listResult.Count() > 0)
            {
                int maxLen = listResult.Where(r => !string.IsNullOrEmpty(r.Index.ToString())).Select(s => s.Index.ToString().Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                listResult = listResult
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.Index.ToString()) ? s.Index.ToString() : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void CreateBOMDesignTwo(BOMDesignTwoModel model, HttpFileCollection hfc)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                model.Index = GetIndex(model);
                try
                {
                    BOMDesignTwo bomDesignTwo = new BOMDesignTwo
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectProductId = model.ProjectProductId,
                        Version = model.Version,
                        Index = model.Index,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    if (model.ListFile.Count > 0)
                    {
                        foreach (var item in model.ListFile)
                        {
                            BOMDesignTwoAttach file = new BOMDesignTwoAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BOMDesignTwoId = bomDesignTwo.Id,
                                FileName = item.FileName,
                                Path = item.Path,
                                FileSize = item.FileSize,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now,
                                Content =model.Content,
                            };
                            db.BOMDesignTwoAttaches.Add(file);
                        }
                    }

                    for (int i = 0; i < hfc.Count; i++)
                    {
                        if (hfc.AllKeys[i].Equals(Constants.BOM_Electrics))
                        {
                            var list = ImportMaterialElectric(hfc[i], bomDesignTwo.Id, Constants.BOM_Electric, model);
                            db.BOMDesignTwoDetials.AddRange(list);
                        }
                        else if (hfc.AllKeys[i].Equals(Constants.BOM_Manufactures))
                        {
                            var list = ImportMaterialManufacture(hfc[i], bomDesignTwo.Id, Constants.BOM_Manufacture, model);
                            db.BOMDesignTwoDetials.AddRange(list);
                        }
                        else if (hfc.AllKeys[i].Equals(Constants.BOM_TPAs))
                        {
                            var data = ImportMaterialTPA(hfc[i], bomDesignTwo.Id, Constants.BOM_TPA, model);
                            db.Materials.AddRange(data.ListMaterial);
                            db.BOMDesignTwoDetials.AddRange(data.ListData);
                        }
                        else if (hfc.AllKeys[i].Equals(Constants.BOM_Bulongs))
                        {
                            var list = ImportMaterialBulong(hfc[i], bomDesignTwo.Id, Constants.BOM_Bulong, model);
                            db.BOMDesignTwoDetials.AddRange(list);
                        }
                        else if (hfc.AllKeys[i].Equals(Constants.BOM_Others))
                        {
                            var list = ImportMaterialElectric(hfc[i], bomDesignTwo.Id, Constants.BOM_Other, model);
                            db.BOMDesignTwoDetials.AddRange(list);
                        }
                    }

                    db.BOMDesignTwoes.Add(bomDesignTwo);
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
        public void UpdateBOMDesignTwo(BOMDesignTwoModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var bomDesignTwo = db.BOMDesignTwoes.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    bomDesignTwo.ProjectProductId = model.ProjectProductId;
                    bomDesignTwo.Version = model.Version;
                    bomDesignTwo.UpdateBy = model.UpdateBy;
                    bomDesignTwo.UpdateDate = DateTime.Now;

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

        public void DeleteBOMDesignTwo(BOMDesignTwoModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var versionMax = db.BOMDesignTwoes.Max(i => i.Version);
                    var bomDesignTwo = db.BOMDesignTwoes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (bomDesignTwo == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.BOMDesignTwo);
                    }

                    if (bomDesignTwo.Version < versionMax)
                    {
                        throw NTSException.CreateInstance("Bạn chỉ được xóa version mới nhất!");
                    }

                    var bOMDesignTwoDetials = db.BOMDesignTwoDetials.Where(i => i.BOMDesignTwoId.Equals(model.Id)).ToList();
                    if (bOMDesignTwoDetials.Count > 0)
                    {
                        db.BOMDesignTwoDetials.RemoveRange(bOMDesignTwoDetials);
                    }

                    var bOMDesignTwoAttaches = db.BOMDesignTwoAttaches.Where(i => i.BOMDesignTwoId.Equals(model.Id)).ToList();
                    if (bOMDesignTwoAttaches.Count > 0)
                    {
                        db.BOMDesignTwoAttaches.RemoveRange(bOMDesignTwoAttaches);
                        var data = db.MaterialImportBOMDrafts.Where(a => a.ProjectId.Equals(bomDesignTwo.ProjectProductId) && a.ModuleId.Equals(model.ModuleId));
                        db.MaterialImportBOMDrafts.RemoveRange(data);

                        var listBOMToImport = db.BOMDesignTwoes.Where(u => u.ProjectProductId.Equals(bomDesignTwo.ProjectProductId) && !u.Id.Equals(bomDesignTwo.Id)).OrderBy(a =>a.CreateDate).ToList();
                        var list =ImportExcelWhenDeleteBOM(listBOMToImport, bomDesignTwo.ProjectProductId,model.ModuleId);
                        db.MaterialImportBOMDrafts.AddRange(list);
                    }

                    db.BOMDesignTwoes.Remove(bomDesignTwo);
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
        public object GetBOMDesignTwoInfo(BOMDesignTwoModel model)
        {
            var resultInfo = db.BOMDesignTwoes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new BOMDesignTwoModel
            {
                Id = p.Id,
                ProjectProductId = p.ProjectProductId,
                Version = p.Version,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.BOMDesignTwo);
            }

            return resultInfo;
        }

        public List<BOMDesignTwoDetial> ImportMaterialElectric(HttpPostedFile file, string BOMDesignTwoId, int MaterialType, BOMDesignTwoModel model)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string index, materialCode, materialName, quantity, unit, manufacture, price;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<BOMDesignTwoDetial> list = new List<BOMDesignTwoDetial>();
            BOMDesignTwoDetial itemC;
            List<int> rowQuantity = new List<int>();
            List<int> rowCheckQuantity = new List<int>();
            List<int> rowPrice = new List<int>();
            List<int> rowCheckPrice = new List<int>();
            List<int> rowMaterialName = new List<int>();
            List<int> rowMaterialCode = new List<int>();
            List<int> rowManufactureCode = new List<int>();
            List<int> rowUnitName = new List<int>();

            itemC = new BOMDesignTwoDetial();
            itemC.Id = Guid.NewGuid().ToString();
            itemC.BOMDesignTwoId = BOMDesignTwoId;
            itemC.MaterialType = MaterialType;
            itemC.MaterialId = model.ModuleId;
            itemC.FileIndex = "0";
            itemC.SAPIndex = 0;
            var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module != null)
            {
                itemC.MaterialCode = module.Code;
                itemC.MaterialName = module.Name;
            }
            list.Add(itemC);

            int indexStart = 1;
            try
            {
                //Index
                var bom = (from a in db.BOMDesignTwoes.AsNoTracking()
                           join b in db.BOMDesignTwoDetials.AsNoTracking() on a.Id equals b.BOMDesignTwoId
                           where a.ProjectProductId.Equals(model.ProjectProductId) && b.MaterialType.Equals(Constants.BOM_Electric) && a.Version == model.Version
                           orderby a.Version descending
                           select new
                           {
                               Version = a.Version,
                               FileIndex = b.FileIndex,
                               SAPIndex = b.SAPIndex
                           }).ToList();

                if (bom.Count > 0 && list.Where(o => !o.FileIndex.Equals("0")).Count() == 0)
                {
                    var number = bom.Max(o => o.SAPIndex);
                    indexStart = number + 1;
                }

                for (int i = 9; i <= rowCount; i++)
                {
                    itemC = new BOMDesignTwoDetial();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemC.BOMDesignTwoId = BOMDesignTwoId;
                    itemC.MaterialType = MaterialType;
                    index = sheet[i, 1].Value;
                    materialName = sheet[i, 2].Value;
                    materialCode = sheet[i, 3].Value;
                    manufacture = sheet[i, 5].Value;
                    unit = sheet[i, 8].Value;
                    quantity = sheet[i, 9].Value;
                    price = sheet[i, 10].Value;

                    if (!string.IsNullOrEmpty(index) && string.IsNullOrEmpty(materialCode))
                    {
                        continue;
                    }
                    else if (string.IsNullOrEmpty(index) && string.IsNullOrEmpty(materialCode))
                    {
                        break;
                    }

                    //Specification
                    itemC.Specification = null;

                    //Quantity
                    try
                    {
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            var Quantity = Convert.ToDecimal(quantity);
                            if (Quantity > 0)
                            {
                                itemC.Quantity = Quantity;
                            }
                            else
                            {
                                rowQuantity.Add(i);
                            }
                        }
                        else { rowQuantity.Add(i); }
                    }
                    catch (Exception)
                    {
                        rowCheckQuantity.Add(i);
                        continue;
                    }

                    //Price
                    try
                    {
                        itemC.Price = 0;
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Price = Convert.ToDecimal(price);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckPrice.Add(i);
                        continue;
                    }

                    itemC.FileIndex = index;
                    //Index
                    if (list.Count > 0)
                    {
                        var number = 1 + list.Max(o => o.SAPIndex);
                        itemC.SAPIndex = number;
                    }
                    else
                    {
                        itemC.SAPIndex = indexStart;
                        indexStart++;
                    }

                    //MaterialName
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        itemC.MaterialName = materialName.Trim();
                    }
                    else
                    {
                        rowMaterialName.Add(i);
                    }

                    //MaterialCode
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        itemC.MaterialId = "";
                        var material = db.Materials.AsNoTracking().FirstOrDefault(o => materialCode.Trim().ToUpper().Equals(o.Code.ToUpper()));
                        if (material != null)
                        {
                            itemC.MaterialId = material.Id;
                        }
                        itemC.MaterialCode = materialCode.Trim();
                    }
                    else
                    {
                        rowMaterialCode.Add(i);
                    }

                    //Manufacture
                    if (!string.IsNullOrEmpty(manufacture))
                    {
                        itemC.ManufactureCode = manufacture.Trim();
                    }
                    else
                    {
                        rowManufactureCode.Add(i);
                    }

                    //Unitname
                    if (!string.IsNullOrEmpty(unit))
                    {
                        itemC.UnitName = unit.Trim();
                    }
                    else
                    {
                        rowUnitName.Add(i);
                    }

                    list.Add(itemC);
                }

                if (rowMaterialCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialCode) + "> vật tư điện không được để trống!");
                }

                if (rowMaterialName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialName) + "> vật tư điện không được để trống!");
                }

                if (rowManufactureCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCode) + "> vật tư điện không được để trống!");
                }

                if (rowUnitName.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị dòng xuất dòng <" + string.Join(", ", rowUnitName) + "> vật tư điện không được để trống!");
                }

                if (rowQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantity) + "> vật tư điện phải lớn hơn 0!");
                }

                if (rowCheckQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQuantity) + "> vật tư điện không đúng!");
                }

                if (rowCheckPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá dòng <" + string.Join(", ", rowCheckPrice) + "> vật tư điện không đúng!");
                }

                #endregion
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }
            workbook.Close();
            excelEngine.Dispose();

            return list;
        }

        public List<BOMDesignTwoDetial> ImportMaterialManufacture(HttpPostedFile file, string BOMDesignTwoId, int materialType, BOMDesignTwoModel model)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string index, materialCode, materialName, specification, quantity, unit, price, manufacture;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<BOMDesignTwoDetial> list = new List<BOMDesignTwoDetial>();
            BOMDesignTwoDetial itemC;
            List<int> rowQuantity = new List<int>();
            List<int> rowCheckQuantity = new List<int>();
            List<int> rowPrice = new List<int>();
            List<int> rowCheckPrice = new List<int>();
            List<int> rowMaterialName = new List<int>();
            List<int> rowMaterialCode = new List<int>();
            List<int> rowManufactureCode = new List<int>();
            List<int> rowUnitName = new List<int>();

            itemC = new BOMDesignTwoDetial();
            itemC.Id = Guid.NewGuid().ToString();
            itemC.BOMDesignTwoId = BOMDesignTwoId;
            itemC.MaterialType = materialType;
            itemC.MaterialId = model.ModuleId;
            itemC.FileIndex = "0";
            itemC.SAPIndex = 0;
            var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module != null)
            {
                itemC.MaterialCode = module.Code;
                itemC.MaterialName = module.Name;
            }
            list.Add(itemC);

            int indexStart = 1;
            try
            {
                var bom = (from a in db.BOMDesignTwoes.AsNoTracking()
                           join b in db.BOMDesignTwoDetials.AsNoTracking() on a.Id equals b.BOMDesignTwoId
                           where a.ProjectProductId.Equals(model.ProjectProductId) && b.MaterialType == materialType && a.Version == model.Version
                           orderby a.Version descending
                           select new
                           {
                               Version = a.Version,
                               FileIndex = b.FileIndex,
                               SAPIndex = b.SAPIndex
                           }).ToList();

                if (bom.Count > 0 && list.Where(o => !o.FileIndex.Equals("0")).Count() == 0)
                {
                    var number = bom.Max(o => o.SAPIndex);
                    indexStart = number + 1;
                }

                decimal parentQuantity = 1;
                for (int i = 7; i <= rowCount; i++)
                {
                    index = sheet[i, 1].Value;
                    materialCode = sheet[i, 4].Value;

                    if (string.IsNullOrEmpty(index) && string.IsNullOrEmpty(materialCode))
                    {
                        break;
                    }

                    quantity = sheet[i, 7].Value;

                    if (materialCode.StartsWith("PCB") || materialCode.StartsWith("TPA"))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(quantity))
                            {
                                var Qty = Convert.ToDecimal(quantity);
                                if (Qty > 0)
                                {
                                    parentQuantity = Qty;
                                }
                                else
                                {
                                    rowQuantity.Add(i);
                                }
                            }
                            else
                            {
                                rowQuantity.Add(i);
                            }
                        }
                        catch (Exception)
                        {
                            rowCheckQuantity.Add(i);
                            continue;
                        }
                        continue;
                    }

                    itemC = new BOMDesignTwoDetial();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemC.BOMDesignTwoId = BOMDesignTwoId;
                    itemC.MaterialType = materialType;

                    materialName = sheet[i, 2].Value;
                    specification = sheet[i, 3].Value;
                    unit = sheet[i, 6].Value;
                    quantity = sheet[i, 7].Value;
                    manufacture = sheet[i, 10].Value;
                    price = sheet[i, 12].Value;

                    //Specification
                    itemC.Specification = specification;

                    //Quantity
                    try
                    {
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            var Qty = Convert.ToDecimal(quantity);
                            if (Qty > 0)
                            {
                                itemC.Quantity = Qty * parentQuantity;
                            }
                            else { rowQuantity.Add(i); }
                        }
                        else
                        {
                            rowQuantity.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckQuantity.Add(i);
                        continue;
                    }

                    //Price
                    try
                    {
                        itemC.Price = 0;
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Price = Convert.ToDecimal(price);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckPrice.Add(i);
                        continue;
                    }

                    itemC.FileIndex = index;
                    //Index
                    if (list.Count > 0)
                    {
                        var number = 1 + list.Max(o => o.SAPIndex);
                        itemC.SAPIndex = number;
                    }
                    else
                    {
                        itemC.SAPIndex = indexStart;
                        indexStart++;
                    }

                    //MaterialName
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        itemC.MaterialName = materialName.Trim();
                    }
                    else
                    {
                        rowMaterialName.Add(i);
                    }

                    //MaterialCode
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        itemC.MaterialId = "";
                        var material = db.Materials.AsNoTracking().FirstOrDefault(o => materialCode.Trim().ToUpper().Equals(o.Code.ToUpper()));
                        if (material != null)
                        {
                            itemC.MaterialId = material.Id;
                        }
                        itemC.MaterialCode = materialCode.Trim();
                    }
                    else
                    {
                        rowMaterialCode.Add(i);
                    }

                    //Manufacture
                    if (!string.IsNullOrEmpty(manufacture))
                    {
                        itemC.ManufactureCode = manufacture.Trim();
                    }
                    else
                    {
                        rowManufactureCode.Add(i);
                    }

                    //Unitname
                    if (!string.IsNullOrEmpty(unit))
                    {
                        itemC.UnitName = unit.Trim();
                    }
                    else
                    {
                        rowUnitName.Add(i);
                    }

                    list.Add(itemC);

                }

                if (rowMaterialCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialCode) + "> vật tư hãng không được để trống!");
                }

                if (rowMaterialName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialName) + "> vật tư hãng không được để trống!");
                }

                if (rowManufactureCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCode) + "> vật tư hãng không được để trống!");
                }

                if (rowUnitName.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị dòng <" + string.Join(", ", rowUnitName) + "> vật tư hãng không được để trống!");
                }

                if (rowQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantity) + "> vật tư hãng phải lớn hơn 0!");
                }

                if (rowCheckQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQuantity) + "> vật tư hãng không đúng!");
                }

                if (rowCheckPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá dòng <" + string.Join(", ", rowCheckPrice) + "> vật tư hãng không đúng!");
                }

                #endregion
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
            return list;
        }

        public ReturnModel GetIndexImport(IWorksheet sheet, string specifications, int y, int rowCount)
        {
            var check = sheet[y, 1].Value;
            var name = sheet[y, 3].Value;
            int a = y;

            for (int x = a; x <= rowCount; x++)
            {
                //if (string.IsNullOrEmpty(check) && !string.IsNullOrEmpty(name))
                //{
                //    specifications += sheet[x, 3].Value + "\n";
                check = sheet[x + 1, 1].Value;
                name = sheet[x + 1, 3].Value;
                //    if (x == rowCount)
                //    {
                //        y = x;
                //    }
                //}
                //else { y = x - 1; break; }
                if (check.Substring(0, specifications.Length).Equals(specifications) && name.Equals("HÀN"))
                {
                    y = x;
                }
                else
                {
                    y = x;
                    break;
                }
            }
            string data = specifications;
            return new ReturnModel
            {
                Specification = data,
                Index = y
            };
        }
        public GetDateImportManufacture ImportMaterialTPA(HttpPostedFile file, string BOMDesignTwoId, int materialType, BOMDesignTwoModel model)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string index, materialCode, materialName, specification, quantity, unit, manufacture, rawMaterial, rawMaterialCode, weight, price;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<BOMDesignTwoDetial> list = new List<BOMDesignTwoDetial>();
            List<IndexModel> listIndex = new List<IndexModel>();
            BOMDesignTwoDetial itemC;
            //MaterialGroup materialGroup;
            //Unit units;
            List<int> rowCode = new List<int>();
            List<int> rowQuantity = new List<int>();
            List<int> rowCheckQuantity = new List<int>();
            List<int> rowPrice = new List<int>();
            List<int> rowManufacture = new List<int>();
            List<int> rowCheckManufacture = new List<int>();
            List<int> rowCheckUnit = new List<int>();
            List<int> rowCheckMaterialGroup = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<int> rowMaterialName = new List<int>();
            List<int> rowMaterialCode = new List<int>();
            List<int> rowManufactureCode = new List<int>();
            List<int> rowUnitName = new List<int>();

            itemC = new BOMDesignTwoDetial();
            itemC.Id = Guid.NewGuid().ToString();
            itemC.BOMDesignTwoId = BOMDesignTwoId;
            itemC.MaterialType = materialType;
            itemC.MaterialId = model.ModuleId;
            itemC.FileIndex = "0";
            itemC.SAPIndex = 0;
            var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module != null)
            {
                itemC.MaterialCode = module.Code;
                itemC.MaterialName = module.Name;
            }
            list.Add(itemC);

            try
            {
                string HANIndex = string.Empty;
                for (int i = 7; i <= rowCount; i++)
                {
                    itemC = new BOMDesignTwoDetial();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemC.BOMDesignTwoId = BOMDesignTwoId;
                    itemC.MaterialType = materialType;
                    index = sheet[i, 1].Value;
                    materialName = sheet[i, 2].Value;
                    specification = sheet[i, 3].Value;
                    materialCode = sheet[i, 4].Value;
                    rawMaterialCode = sheet[i,5].Value;
                    unit = sheet[i, 6].Value;
                    quantity = sheet[i, 7].Value;
                    rawMaterial = sheet[i, 8].Value;
                    weight = sheet[i,9].Value;
                    manufacture = sheet[i, 10].Value;
                    price = sheet[i, 11].Value;

                    if (string.IsNullOrEmpty(index) && string.IsNullOrEmpty(materialCode))
                    {
                        break;
                    }

                    //Specification
                    itemC.Specification = specification;

                    if (!string.IsNullOrEmpty(specification) && specification.ToUpper().Equals("HÀN") && (string.IsNullOrEmpty(HANIndex) || !index.StartsWith(HANIndex)))
                    {
                        HANIndex = index;
                    }
                    else if (!string.IsNullOrEmpty(HANIndex) && index.StartsWith(HANIndex))
                    {
                        continue;
                    }

                    //Quantity
                    try
                    {
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            var Qty = Convert.ToDecimal(quantity);
                            if (Qty > 0)
                            {
                                itemC.Quantity = Qty;
                            }
                            else { rowQuantity.Add(i); }
                        }
                        else
                        {
                            rowQuantity.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckQuantity.Add(i);
                        continue;
                    }

                    itemC.FileIndex = index;

                    //MaterialName
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        itemC.MaterialName = materialName.Trim();
                    }
                    else
                    {
                        rowMaterialName.Add(i);
                    }

                    //MaterialCode
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        itemC.MaterialId = "";
                        var material = db.Materials.AsNoTracking().FirstOrDefault(o => materialCode.Trim().ToUpper().Equals(o.Code.ToUpper()));
                        if (material != null)
                        {
                            itemC.MaterialId = material.Id;
                        }
                        itemC.MaterialCode = materialCode.Trim();
                    }
                    else
                    {
                        rowMaterialCode.Add(i);
                    }

                    // Mã vật liệu
                    if (!string.IsNullOrEmpty(rawMaterialCode))
                    {
                        itemC.RawMaterialCode = rawMaterialCode.Trim();
                    }

                    // Đơn vị
                    if (!string.IsNullOrEmpty(unit))
                    {
                        itemC.UnitName = unit.Trim();
                    }
                    else
                    {
                        rowUnitName.Add(i);
                    }

                    // Mã vật liệu
                    if (!string.IsNullOrEmpty(rawMaterial))
                    {
                        itemC.RawMaterial = rawMaterial.Trim();
                    }

                    // Khối lượng
                    try
                    {
                        if (!string.IsNullOrEmpty(weight))
                        {
                            var Qty = Convert.ToDecimal(weight);
                            if (Qty > 0)
                            {
                                itemC.Weight = Qty;
                            }
                            //else { rowQuantity.Add(i); }
                        }
                        else
                        {
                            //rowQuantity.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        //rowCheckQuantity.Add(i);
                        //continue;
                    }

                    //Manufacture
                    if (!string.IsNullOrEmpty(manufacture))
                    {
                        itemC.ManufactureCode = manufacture.Trim();
                    }
                    else
                    {
                        rowManufactureCode.Add(i);
                    }


                    //Price
                    itemC.Price = 0;
                    try
                    {
                        if (!string.IsNullOrEmpty(price))
                        {
                            var Qty = Convert.ToDecimal(price);
                            if (Qty > 0)
                            {
                                itemC.Price = Qty;
                            }
                            //else { rowQuantity.Add(i); }
                        }
                        else
                        {
                            //rowQuantity.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        //rowCheckQuantity.Add(i);
                        //continue;
                    }

                    list.Add(itemC);
                }

                if (rowMaterialCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialCode) + "> vật tư TPA không được để trống!");
                }

                if (rowMaterialName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialName) + "> vật tư TPA không được để trống!");
                }

                if (rowManufactureCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCode) + "> vật tư TPA không được để trống!");
                }

                if (rowUnitName.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị dòng <" + string.Join(", ", rowUnitName) + "> vật tư TPA không được để trống!");
                }

                if (rowQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantity) + "> vật tư TPA phải lớn hơn 0!");
                }

                if (rowCheckQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQuantity) + "> vật tư TPA không đúng!");
                }

                #endregion
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
            return new GetDateImportManufacture
            {
                ListData = list,
                //ListMaterial = listMaterial
            };
        }

        public List<BOMDesignTwoDetial> ImportMaterialBulong(HttpPostedFile file, string BOMDesignTwoId, int materialType, BOMDesignTwoModel model)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string index, materialCode, materialName, quantity, unit, manufacture, price;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<BOMDesignTwoDetial> list = new List<BOMDesignTwoDetial>();
            BOMDesignTwoDetial itemC;
            //List<int> rowCode = new List<int>();
            List<int> rowQuantity = new List<int>();
            List<int> rowCheckQuantity = new List<int>();
            List<int> rowPrice = new List<int>();
            //List<int> rowCheckUnit = new List<int>();
            List<int> rowCheckPrice = new List<int>();
            //List<int> rowCheckMaterialGroup = new List<int>();
            //List<int> rowCheckCode = new List<int>();
            //List<int> rowManufacture = new List<int>();
            //List<int> rowCheckManufacture = new List<int>();
            List<int> rowMaterialName = new List<int>();
            List<int> rowMaterialCode = new List<int>();
            List<int> rowManufactureCode = new List<int>();
            List<int> rowUnitName = new List<int>();

            itemC = new BOMDesignTwoDetial();
            itemC.Id = Guid.NewGuid().ToString();
            itemC.BOMDesignTwoId = BOMDesignTwoId;
            itemC.MaterialType = materialType;
            itemC.MaterialId = model.ModuleId;
            itemC.FileIndex = "0";
            itemC.SAPIndex = 0;
            var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module != null)
            {
                itemC.MaterialCode = module.Code;
                itemC.MaterialName = module.Name;
            }
            list.Add(itemC);
            int indexStart = 1;
            try
            {
                var bom = (from a in db.BOMDesignTwoes.AsNoTracking()
                           join b in db.BOMDesignTwoDetials.AsNoTracking() on a.Id equals b.BOMDesignTwoId
                           where a.ProjectProductId.Equals(model.ProjectProductId) && b.MaterialType == materialType && a.Version == model.Version
                           orderby a.Version descending
                           select new
                           {
                               Version = a.Version,
                               b.FileIndex,
                               b.SAPIndex
                           }).ToList();

                if (bom.Count > 0 && list.Where(o => !o.FileIndex.Equals("0")).Count() == 0)
                {
                    var number = bom.Max(o => o.SAPIndex);
                    indexStart = number + 1;
                }

                for (int i = 9; i <= rowCount; i++)
                {
                    itemC = new BOMDesignTwoDetial();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemC.BOMDesignTwoId = BOMDesignTwoId;
                    itemC.MaterialType = materialType;
                    index = sheet[i, 1].Value;
                    materialName = sheet[i, 2].Value;
                    materialCode = sheet[i, 3].Value;
                    manufacture = sheet[i, 5].Value;
                    unit = sheet[i, 8].Value;
                    quantity = sheet[i, 9].Value;
                    price = sheet[i, 10].Value;

                    //Specification
                    itemC.Specification = null;

                    //Quantity
                    try
                    {
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            var Quantity = Convert.ToDecimal(quantity);
                            if (Quantity > 0)
                            {
                                itemC.Quantity = Quantity;
                            }
                            else
                            {
                                rowQuantity.Add(i);
                            }
                        }
                        else { rowQuantity.Add(i); }
                    }
                    catch (Exception)
                    {
                        rowCheckQuantity.Add(i);
                        continue;
                    }

                    //Price
                    try
                    {
                        itemC.Price = 0;
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Price = Convert.ToDecimal(price);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckPrice.Add(i);
                        continue;
                    }

                    itemC.FileIndex = index;
                    //Index
                    if (list.Count > 0)
                    {
                        var number = 1 + list.Max(o => o.SAPIndex);
                        itemC.SAPIndex = number;
                    }
                    else
                    {
                        itemC.SAPIndex = indexStart;
                        indexStart++;
                    }

                    //MaterialName
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        itemC.MaterialName = materialName.Trim();
                    }
                    else
                    {
                        rowMaterialName.Add(i);
                    }

                    //MaterialCode
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        itemC.MaterialId = "";
                        var material = db.Materials.AsNoTracking().FirstOrDefault(o => materialCode.Trim().ToUpper().Equals(o.Code.ToUpper()));
                        if (material != null)
                        {
                            itemC.MaterialId = material.Id;
                        }
                        itemC.MaterialCode = materialCode.Trim();
                    }
                    else
                    {
                        rowMaterialCode.Add(i);
                    }

                    //Manufacture
                    if (!string.IsNullOrEmpty(manufacture))
                    {
                        itemC.ManufactureCode = manufacture.Trim();
                    }
                    else
                    {
                        rowManufactureCode.Add(i);
                    }

                    //Unitname
                    if (!string.IsNullOrEmpty(unit))
                    {
                        itemC.UnitName = unit.Trim();
                    }
                    else
                    {
                        rowUnitName.Add(i);
                    }

                    list.Add(itemC);
                }

                if (rowMaterialCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialCode) + "> vật tư khác không được để trống!");
                }

                if (rowMaterialName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialName) + "> vật tư khác không được để trống!");
                }

                if (rowManufactureCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCode) + "> vật tư khác không được để trống!");
                }

                if (rowUnitName.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị dòng xuất dòng <" + string.Join(", ", rowUnitName) + "> vật tư khác không được để trống!");
                }

                if (rowQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantity) + "> vật tư khác phải lớn hơn 0!");
                }

                if (rowCheckQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQuantity) + "> vật tư khác không đúng!");
                }

                if (rowCheckPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá dòng <" + string.Join(", ", rowCheckPrice) + "> vật tư khác không đúng!");
                }

                #endregion
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
            return list;
        }

        public List<BOMDesignTwoDetial> ImportMaterialOther(HttpPostedFile file, string BOMDesignTwoId, int materialType, BOMDesignTwoModel model)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string index, materialCode, materialName, quantity, unit, manufacture, price;
            #region [Doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<BOMDesignTwoDetial> list = new List<BOMDesignTwoDetial>();
            BOMDesignTwoDetial itemC;
            List<int> rowQuantity = new List<int>();
            List<int> rowCheckQuantity = new List<int>();
            List<int> rowPrice = new List<int>();
            List<int> rowCheckPrice = new List<int>();
            List<int> rowMaterialName = new List<int>();
            List<int> rowMaterialCode = new List<int>();
            List<int> rowManufactureCode = new List<int>();
            List<int> rowUnitName = new List<int>();

            itemC = new BOMDesignTwoDetial();
            itemC.Id = Guid.NewGuid().ToString();
            itemC.BOMDesignTwoId = BOMDesignTwoId;
            itemC.MaterialType = materialType;
            itemC.MaterialId = model.ModuleId;
            itemC.FileIndex = "0";
            var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module != null)
            {
                itemC.MaterialCode = module.Code;
                itemC.MaterialName = module.Name;
            }
            list.Add(itemC);
            //}
            int indexStart = 1;
            try
            {
                var bom = (from a in db.BOMDesignTwoes.AsNoTracking()
                           join b in db.BOMDesignTwoDetials.AsNoTracking() on a.Id equals b.BOMDesignTwoId
                           where a.ProjectProductId.Equals(model.ProjectProductId) && b.MaterialType == materialType && a.Version == model.Version
                           orderby a.Version descending
                           select new
                           {
                               Version = a.Version,
                               b.FileIndex,
                               b.SAPIndex
                           }).ToList();

                if (bom.Count > 0 && list.Where(o => !o.FileIndex.Equals("0")).Count() == 0)
                {
                    var number = bom.Max(o => o.SAPIndex);
                    indexStart = number + 1;
                }

                for (int i = 9; i <= rowCount; i++)
                {
                    itemC = new BOMDesignTwoDetial();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemC.BOMDesignTwoId = BOMDesignTwoId;
                    itemC.MaterialType = materialType;
                    index = sheet[i, 1].Value;
                    materialName = sheet[i, 2].Value;
                    materialCode = sheet[i, 3].Value;
                    manufacture = sheet[i, 5].Value;
                    unit = sheet[i, 8].Value;
                    quantity = sheet[i, 9].Value;
                    price = sheet[i, 10].Value;

                    //Specification
                    itemC.Specification = null;

                    //Quantity
                    try
                    {
                        if (!string.IsNullOrEmpty(quantity))
                        {
                            var Quantity = Convert.ToDecimal(quantity);
                            if (Quantity > 0)
                            {
                                itemC.Quantity = Quantity;
                            }
                            else
                            {
                                rowQuantity.Add(i);
                            }
                        }
                        else { rowQuantity.Add(i); }
                    }
                    catch (Exception)
                    {
                        rowCheckQuantity.Add(i);
                        continue;
                    }

                    //Price
                    try
                    {
                        itemC.Price = 0;
                        if (!string.IsNullOrEmpty(price))
                        {
                            itemC.Price = Convert.ToDecimal(price);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckPrice.Add(i);
                        continue;
                    }

                    itemC.FileIndex = index;
                    //Index
                    if (list.Count > 0)
                    {
                        var number = 1 + list.Max(o => o.SAPIndex);
                        itemC.SAPIndex = number;
                    }
                    else
                    {
                        itemC.SAPIndex = indexStart;
                        indexStart++;
                    }

                    //MaterialName
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        itemC.MaterialName = materialName.Trim();
                    }
                    else
                    {
                        rowMaterialName.Add(i);
                    }

                    //MaterialCode
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        itemC.MaterialId = "";
                        var material = db.Materials.AsNoTracking().FirstOrDefault(o => materialCode.Trim().ToUpper().Equals(o.Code.ToUpper()));
                        if (material != null)
                        {
                            itemC.MaterialId = material.Id;
                        }
                        itemC.MaterialCode = materialCode.Trim();
                    }
                    else
                    {
                        rowMaterialCode.Add(i);
                    }

                    //Manufacture
                    if (!string.IsNullOrEmpty(manufacture))
                    {
                        itemC.ManufactureCode = manufacture.Trim();
                    }
                    else
                    {
                        rowManufactureCode.Add(i);
                    }

                    //Unitname
                    if (!string.IsNullOrEmpty(unit))
                    {
                        itemC.UnitName = unit.Trim();
                    }
                    else
                    {
                        rowUnitName.Add(i);
                    }

                    list.Add(itemC);
                }

                if (rowMaterialCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowMaterialCode) + "> vật tư khác không được để trống!");
                }

                if (rowMaterialName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên vật tư dòng <" + string.Join(", ", rowMaterialName) + "> vật tư khác không được để trống!");
                }

                if (rowManufactureCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowManufactureCode) + "> vật tư khác không được để trống!");
                }

                if (rowUnitName.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị dòng xuất dòng <" + string.Join(", ", rowUnitName) + "> vật tư khác không được để trống!");
                }

                if (rowQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantity) + "> vật tư khác phải lớn hơn 0!");
                }

                if (rowCheckQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQuantity) + "> vật tư khác không đúng!");
                }

                if (rowCheckPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn giá dòng <" + string.Join(", ", rowCheckPrice) + "> vật tư khác không đúng!");
                }

                #endregion
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(model, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
            return list;
        }

        #region file khác
        //public void ImportMaterialOther(string userId, HttpPostedFile file, string BOMDesignTwoId, int MaterialType, string projectProductId)
        //{
        //    string extension = Path.GetExtension(file.FileName).ToLower();
        //    if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
        //    {
        //        throw NTSException.CreateInstance("File dữ liệu phải là excel");
        //    }
        //    string index, materialCode, quantity, price;
        //    #region[doc du lieu tu excel]
        //    ExcelEngine excelEngine = new ExcelEngine();
        //    IApplication application = excelEngine.Excel;
        //    IWorkbook workbook = application.Workbooks.Open(file.InputStream);
        //    IWorksheet sheet = workbook.Worksheets[0];
        //    sheet.EnableSheetCalculations();
        //    int rowCount = sheet.Rows.Count();
        //    List<BOMDesignTwoDetial> list = new List<BOMDesignTwoDetial>();
        //    BOMDesignTwoDetial itemC;
        //    List<int> rowCode = new List<int>();
        //    List<int> rowCheckQuantity = new List<int>();
        //    List<int> rowPrice = new List<int>();
        //    List<int> rowCheckManufacture = new List<int>();
        //    List<int> rowCheckPrice = new List<int>();
        //    List<int> rowCheckMaterialGroup = new List<int>();
        //    List<int> rowCheckCode = new List<int>();
        //    try
        //    {
        //        for (int i = 4; i <= rowCount; i++)
        //        {
        //            itemC = new BOMDesignTwoDetial();
        //            itemC.Id = Guid.NewGuid().ToString();
        //            itemC.BOMDesignTwoId = BOMDesignTwoId;
        //            itemC.MaterialType = MaterialType;
        //            index = sheet[i, 1].Value;
        //            //materialName = sheet[i, 2].Value;
        //            materialCode = sheet[i, 1].Value;
        //            //manufacture = sheet[i, 5].Value;
        //            //unit = sheet[i, 8].Value;
        //            quantity = sheet[i, 4].Value;
        //            price = sheet[i, 8].Value;

        //            //MaterialId
        //            if (!string.IsNullOrEmpty(materialCode))
        //            {
        //                var material = db.Materials.FirstOrDefault(o => materialCode.ToUpper().Equals(o.Code.ToUpper()));
        //                if (material != null)
        //                {
        //                    itemC.MaterialId = material.Id;
        //                }
        //                else
        //                {
        //                    rowCheckCode.Add(i);
        //                }
        //            }
        //            else if (!string.IsNullOrEmpty(index) && string.IsNullOrEmpty(materialCode))
        //            {
        //                continue;
        //            }
        //            else if (string.IsNullOrEmpty(index) && string.IsNullOrEmpty(materialCode))
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                rowCode.Add(i);
        //            }

        //            //Specification
        //            itemC.Specification = null;

        //            //Quantity
        //            try
        //            {
        //                itemC.Quantity = 0;
        //                if (!string.IsNullOrEmpty(quantity))
        //                {
        //                    itemC.Quantity = Convert.ToDecimal(quantity);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                rowCheckQuantity.Add(i);
        //                continue;
        //            }

        //            //Price
        //            try
        //            {
        //                itemC.Price = 0;
        //                if (!string.IsNullOrEmpty(price))
        //                {
        //                    itemC.Price = Convert.ToDecimal(price);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                rowCheckPrice.Add(i);
        //                continue;
        //            }

        //            //Index
        //            var bom = (from a in db.BOMDesignTwoes.AsNoTracking()
        //                       join b in db.BOMDesignTwoDetials.AsNoTracking() on a.Id equals b.BOMDesignTwoId
        //                       where a.ProjectProductId.Equals(projectProductId) && b.MaterialType.Equals(MaterialType)
        //                       orderby a.Version descending
        //                       select new { a.Version, b.Index }
        //                       ).ToList().Max(o => Convert.ToInt32(o.Index));
        //            if (bom > 0)
        //            {
        //                itemC.Index = "4." + bom + "." + index;
        //            }
        //            else
        //            {
        //                itemC.Index = "4." + index;
        //            }

        //            list.Add(itemC);
        //        }

        //        if (rowCode.Count > 0)
        //        {
        //            throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCode) + "> không được để trống!");
        //        }

        //        if (rowCheckCode.Count > 0)
        //        {
        //            throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", rowCheckCode) + "> không tồn tại!");
        //        }

        //        if (rowCheckQuantity.Count > 0)
        //        {
        //            throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowCheckQuantity) + "> không đúng!");
        //        }

        //        if (rowCheckPrice.Count > 0)
        //        {
        //            throw NTSException.CreateInstance("Đơn giá dòng <" + string.Join(", ", rowCheckPrice) + "> không đúng!");
        //        }

        //        #endregion
        //        db.BOMDesignTwoDetials.AddRange(list);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        workbook.Close();
        //        excelEngine.Dispose();
        //        throw new NTSLogException(model, ex);
        //    }
        //    workbook.Close();
        //    excelEngine.Dispose();
        //}
        #endregion
        public string ExportExcel(BOMDesignTwoModel model)
        {
            DesignModuleModel bOMDesignTwoe = (from b in db.BOMDesignTwoes.AsNoTracking()
                                               join pp in db.ProjectProducts.AsNoTracking() on b.ProjectProductId equals pp.Id
                                               join m in db.Modules.AsNoTracking() on pp.ModuleId equals m.Id
                                               join p in db.Projects.AsNoTracking() on pp.ProjectId equals p.Id
                                               where b.Id.Equals(model.Id)
                                               select new DesignModuleModel
                                               {
                                                   ProjectCode = p.Code,
                                                   ModuleName = m.Name,
                                                   ModuleCode = m.Code,
                                                   Quantity = pp.Quantity,
                                                   RealQuantity = pp.RealQuantity,
                                                   WarehouseCode = p.WarehouseCode
                                               }).FirstOrDefault();

            if (bOMDesignTwoe == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, "BOM");
            }

            var dataQuery = (from a in db.BOMDesignTwoes.AsNoTracking()
                             join b in db.BOMDesignTwoDetials.AsNoTracking() on a.Id equals b.BOMDesignTwoId
                             where a.Id.Equals(model.Id)
                             join c in db.Materials.AsNoTracking() on b.MaterialId equals c.Id into bc
                             from bcn in bc.DefaultIfEmpty()
                             join g in db.MaterialGroups.AsNoTracking() on bcn != null ? bcn.MaterialGroupId : string.Empty equals g.Id into cg
                             from cgn in cg.DefaultIfEmpty()
                             join t in db.MaterialGroupTPAs.AsNoTracking() on cgn != null ? cgn.MaterialGroupTPAId : string.Empty equals t.Id into gt
                             from gtn in gt.DefaultIfEmpty()
                             orderby b.MaterialCode
                             select new BOMDesignTwoDetialsModel
                             {
                                 Id = b.Id,
                                 FileIndex = b.FileIndex,
                                 SAPIndex = b.SAPIndex,
                                 MaterialType = b.MaterialType,
                                 MaterialName = b.MaterialName,
                                 Specification = b.Specification,
                                 MaterialCode = b.MaterialCode,
                                 UnitName = b.UnitName,
                                 Quantity = b.Quantity,
                                 ManufactureCode = b.ManufactureCode,
                                 Price = b.Price,
                                 Version = a.Version,
                                 GroupCode = gtn != null ? gtn.Code : string.Empty
                             }).AsQueryable();

            List<BOMDesignTwoDetialsModel> materials = dataQuery.ToList();

            if (materials.Count() > 0)
            {
                int maxLen = materials.Where(r => !string.IsNullOrEmpty(r.FileIndex)).Select(s => s.FileIndex.Length).DefaultIfEmpty(0).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                materials = materials
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(!string.IsNullOrEmpty(s.FileIndex) ? s.FileIndex : string.Empty, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }

            List<BOMDesignTwoDetialsModel> listData = new List<BOMDesignTwoDetialsModel>();
            var listModule = materials.Where(i => i.FileIndex.Equals("0")).OrderBy(i => i.MaterialType).ToList();
            foreach (var item in listModule)
            {
                if (item.MaterialType == Constants.BOM_Electric)
                {
                    var data = item;
                    if (item.Version == 0)
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ ĐIỆN)";
                        data.MaterialCode = "IF." + item.MaterialCode.ToUpper();
                    }
                    else
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ ĐIỆN) -v" + item.Version;
                        data.MaterialCode = "IF." + item.MaterialCode.ToUpper() + ".v" + item.Version;
                    }
                    data.Quantity = 1;
                    data.ModuleIndex = "1";
                    listData.Add(data);
                    var listAdd = materials.Where(i => i.MaterialType == Constants.BOM_Electric && !i.Id.Equals(item.Id)).OrderBy(i => i.SAPIndex).ToList();
                    foreach (var items in listAdd)
                    {
                        items.ModuleIndex = "1." + items.SAPIndex;
                    }

                    listData.AddRange(listAdd);
                }
                else if (item.MaterialType == Constants.BOM_Manufacture)
                {
                    var data = item;
                    if (item.Version == 0)
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ HÀNG HÃNG)";
                        data.MaterialCode = "MF." + item.MaterialCode.ToUpper();
                    }
                    else
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ HÀNG HÃNG) -v" + item.Version;
                        data.MaterialCode = "MF." + item.MaterialCode.ToUpper() + ".v" + item.Version;
                    }
                    data.Quantity = 1;
                    data.ModuleIndex = "2";
                    listData.Add(data);
                    var listAdd = materials.Where(i => i.MaterialType == Constants.BOM_Manufacture && !i.Id.Equals(item.Id)).OrderBy(i => i.SAPIndex).ToList();
                    foreach (var items in listAdd)
                    {
                        items.ModuleIndex = "2." + items.SAPIndex;
                    }

                    listData.AddRange(listAdd);
                }
                else if (item.MaterialType == Constants.BOM_TPA)
                {
                    var data = item;
                    if (item.Version == 0)
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ CƠ KHÍ)";
                        data.MaterialCode = "CK." + item.MaterialCode.ToUpper();
                    }
                    else
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ CƠ KHÍ) -v" + item.Version;
                        data.MaterialCode = "CK." + item.MaterialCode.ToUpper() + ".v" + item.Version;
                    }
                    data.Quantity = 1;
                    data.ModuleIndex = "3";
                    listData.Add(data);
                    var listAdd = materials.Where(i => i.MaterialType == Constants.BOM_TPA && !i.Id.Equals(item.Id)).ToList();
                    foreach (var items in listAdd)
                    {
                        items.ModuleIndex = "3." + items.FileIndex;
                    }

                    listData.AddRange(listAdd);
                }
                else if (item.MaterialType == Constants.BOM_Bulong)
                {
                    var data = item;
                    if (item.Version == 0)
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ BU LÔNG)";
                        data.MaterialCode = "BL." + item.MaterialCode.ToUpper();
                    }
                    else
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ BU LÔNG) -v" + item.Version;
                        data.MaterialCode = "BL." + item.MaterialCode.ToUpper() + ".v" + item.Version;
                    }
                    data.Quantity = 1;
                    data.ModuleIndex = "4";
                    listData.Add(data);
                    var listAdd = materials.Where(i => i.MaterialType == Constants.BOM_Bulong && !i.Id.Equals(item.Id)).OrderBy(i => i.SAPIndex).ToList();
                    foreach (var items in listAdd)
                    {
                        items.ModuleIndex = "4." + items.SAPIndex;
                    }

                    listData.AddRange(listAdd);
                }
                else if (item.MaterialType == Constants.BOM_Other)
                {
                    var data = item;
                    if (item.Version == 0)
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ KHÁC)";
                        data.MaterialCode = "BS." + item.MaterialCode.ToUpper();
                    }
                    else
                    {
                        data.MaterialName = item.MaterialName.ToUpper() + " (VẬT TƯ KHÁC) -v" + item.Version;
                        data.MaterialCode = "BS." + item.MaterialCode.ToUpper() + ".v" + item.Version;
                    }
                    data.Quantity = 1;
                    data.ModuleIndex = "5";
                    listData.Add(data);
                    var listAdd = materials.Where(i => i.MaterialType == Constants.BOM_Other && !i.Id.Equals(item.Id)).OrderBy(i => i.SAPIndex).ToList();
                    foreach (var items in listAdd)
                    {
                        items.ModuleIndex = "5." + items.SAPIndex;
                    }

                    listData.AddRange(listAdd);
                }
            }

            DesignMaterialModel materialModel;
            List<DesignMaterialModel> materialExports = new List<DesignMaterialModel>();
            foreach (var material in listData)
            {
                materialModel = new DesignMaterialModel()
                {
                    Index = material.FileIndex,
                    ModuleIndex = material.ModuleIndex,
                    MaterialName = material.MaterialName,
                    Parameter = material.Specification,
                    MaterialCode = material.MaterialCode,
                    Unit = material.UnitName,
                    Quantity = material.Quantity,
                    ModuleQuantity = material.Quantity * bOMDesignTwoe.RealQuantity * GetParentQuantity(listData, material.FileIndex),
                    Manufacturer = material.ManufactureCode,
                    GroupCode = material.GroupCode,
                    Price = material.Price
                };

                materialExports.Add(materialModel);

                if (material.MaterialCode.StartsWith("TPA") || material.MaterialCode.StartsWith("PCB") && material.UnitName.ToUpper().Equals("BỘ"))
                {
                    ////materials.AddRange(GetModuleMaterialChild(material.MaterialCode, material.Index));
                }
            }


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BOMTK1");

            ////Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "BOMTK1";
            bOMDesignTwoe.Materials = materialExports;
            _business.ExportModuleMaterials(bOMDesignTwoe, "1", bOMDesignTwoe.ProjectCode, pathExport, false);

            return Path.Combine(resultPathClient, $"BOM.{ bOMDesignTwoe.ModuleCode}.xlsx");
        }

        public decimal GetParentQuantity(List<BOMDesignTwoDetialsModel> materials, string indexChild)
        {
            string indexParent = _business.GetIndexParent(indexChild);

            if (string.IsNullOrEmpty(indexParent))
            {
                return 1;
            }

            var parent = materials.FirstOrDefault(r => r.FileIndex.Equals(indexParent));

            if (parent != null)
            {
                return parent.Quantity * GetParentQuantity(materials, parent.FileIndex);
            }

            return 1;
        }

        public int GetIndex(BOMDesignTwoModel model)
        {
            int index = 1;
            try
            {
                var data = db.BOMDesignTwoes.AsNoTracking().Where(i => i.ProjectProductId.Equals(model.ProjectProductId) && i.Version == model.Version).ToList();
                if (data.Count > 0)
                {
                    index = data.Max(i => i.Index) + 1;
                }
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
            return index;
        }

        
        public List<MaterialImportBOMDraft> ImportExcelWhenDeleteBOM(List<BOMDesignTwo> boms,string ProjectProductId, string moduleId)
        {
            List<MaterialImportBOMDraft> listBOMDraft = new List<MaterialImportBOMDraft>();

            foreach (var bom in boms)
            {
                var content = bom.BOMDesignTwoAttaches.ToList()[0].Content;
                if (!string.IsNullOrEmpty(content))
                {
                    var materialImportBOMDrafts = JsonConvert.DeserializeObject<List<MaterialImportBOMDraft>>(content);
                    foreach (var item in db.MaterialImportBOMDrafts)
                    {
                        item.Status = false;
                        //item.UpdateStatus = 0;
                    }
                    foreach (var item in materialImportBOMDrafts)
                    {
                        var material = listBOMDraft.FirstOrDefault(a => a.Index.Equals(item.Index));
                        if (material != null)
                        {
                            material.Index = item.Index;
                            material.Name = item.Name;
                            material.Code = item.Code;
                            material.Specification = item.Specification;
                            material.RawMaterial = item.RawMaterial;
                            material.RawMaterialCode = item.RawMaterialCode;
                            material.UnitName = item.UnitName;
                            material.Weight = item.Weight;
                            material.Quantity = item.Quantity;
                            material.Pricing = item.Pricing;
                            material.Note = item.Note;
                            material.Amount = item.Amount;
                            material.ManufactureCode = item.ManufactureCode;
                            material.Status = false;
                            material.UpdateStatus = 1;

                            //item.UpdateStatus = 2;
                            //db.MaterialImportBOMDrafts.Add(item);

                        }
                        else if (listBOMDraft.FirstOrDefault(a => !a.Index.Equals(item.Index) && a.Code.Equals(item.Code)) != null)
                        {
                            var m = listBOMDraft.FirstOrDefault(a => !a.Index.Equals(item.Index) && a.Code.Equals(item.Code));
                            m.Quantity = (m.Quantity + item.Quantity);
                            m.Status = false;
                            m.UpdateStatus = 1;

                            //item.UpdateStatus = 3;
                            listBOMDraft.Add(m);
                        }
                        else
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.UpdateStatus = 0;
                            listBOMDraft.Add(item);
                        }
                    }
                }
            }
            return listBOMDraft;

        }
    }
}
