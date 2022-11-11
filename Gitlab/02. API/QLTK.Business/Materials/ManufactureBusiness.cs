using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Manufacture;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model;
using System.Runtime.InteropServices;
using NTS.Common;
using System.Web;
using Syncfusion.XlsIO;
using System.Web.Hosting;
using NTS.Common.Resource;
using System.IO;
using NTS.Model.Supplier;
using NTS.Model.MaterialGroup;
using QLTK.Business.Suppliers;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.HistoryVersion;

namespace QLTK.Business.Manufacturer
{
    public class ManufactureBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly SupplierBusiness supplierBusiness = new SupplierBusiness();
        public object SearchManufacture(ManufactureSearchModel modelSearch)
        {
            SearchResultModel<ManufactureResultModel> searchResult = new SearchResultModel<ManufactureResultModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.ManufactureInGroups.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             //where a.ManufactureGroupId.Equals(modelSearch.ManufactureGroupId)
                             select new ManufactureResultModel
                             {
                                 Id = b.Id,
                                 ManufactureGroupId = a.ManufactureGroupId,
                                 Code = b.Code,
                                 Name = b.Name,
                                 Description = b.Description,
                                 Address = b.Address,
                                 Phone = b.Phone,
                                 Email = b.Email,
                                 Status = b.Status,
                                 Origination = b.Origination,
                                 MaterialType = b.MaterialType,
                                 Website = b.Website
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.ManufactureGroupId))
            {
                var materialGroups = db.MaterialGroups.AsNoTracking().ToList();

                var materialGroup = materialGroups.FirstOrDefault(i => i.Id.Equals(modelSearch.ManufactureGroupId));

                if (materialGroup != null)
                {
                    listParentId.Add(materialGroup.Id);
                }

                listParentId.AddRange(supplierBusiness.GetListParent(modelSearch.ManufactureGroupId, materialGroups));
                var listManufactureGroup = listParentId.AsQueryable();
                dataQuery = (from a in dataQuery
                             join b in listManufactureGroup.AsQueryable() on a.ManufactureGroupId equals b
                             select new ManufactureResultModel
                             {
                                 Id = a.Id,
                                 ManufactureGroupId = a.ManufactureGroupId,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Description = a.Description,
                                 Address = a.Address,
                                 Phone = a.Phone,
                                 Email = a.Email,
                                 Status = a.Status,
                                 Origination = a.Origination,
                                 MaterialType = a.MaterialType,
                                 Website = a.Website
                             }).AsQueryable();
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.MaterialType))
            {
                dataQuery = dataQuery.Where(u => u.MaterialType.Equals(modelSearch.MaterialType));
            }
            var list = dataQuery.GroupBy(t => t.Code).Select(m => new ManufactureResultModel
            {
                Id = m.FirstOrDefault().Id,
                ManufactureGroupId = m.FirstOrDefault().ManufactureGroupId,
                Code = m.Key,
                Name = m.FirstOrDefault().Name,
                Description = m.FirstOrDefault().Description,
                Address = m.FirstOrDefault().Address,
                Phone = m.FirstOrDefault().Phone,
                Email = m.FirstOrDefault().Email,
                Status = m.FirstOrDefault().Status,
                Origination = m.FirstOrDefault().Origination,
                MaterialType = m.FirstOrDefault().MaterialType,
                Website = m.FirstOrDefault().Website

            }).AsQueryable();
            searchResult.TotalItem = list.Count();
            var listResult = SQLHelpper.OrderBy(list, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<SupplierModel> SearchSupplierManufacture(SupplierSearchModel modelSearch)
        {
            SearchResultModel<SupplierModel> searchResult = new SearchResultModel<SupplierModel>();
            var dataQuery = (from a in db.Suppliers.AsNoTracking()
                             join b in db.MaterialBuyHistories.AsNoTracking() on a.Id equals b.SupplierId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Materials.AsNoTracking() on b.MaterialId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             orderby a.Code
                             select new SupplierModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 PhoneNumber = a.PhoneNumber,
                                 Email = a.Email,
                                 Note = a.Note,
                                 ManufactureId = d.Id,
                             }).AsQueryable();
            dataQuery = dataQuery.Where(u => u.ManufactureId.Equals(modelSearch.ManufactureId));

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            var listResult = dataQuery.ToList();
            List<SupplierModel> listRs = new List<SupplierModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.PhoneNumber, t.Email, t.Note }).ToList();
            foreach (var item in lstRs)
            {
                SupplierModel rs = new SupplierModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.PhoneNumber = item.Key.PhoneNumber;
                rs.Email = item.Key.Email;
                rs.Note = item.Key.Note;
                listRs.Add(rs);
            }
            searchResult.ListResult = listRs.OrderBy(i => i.Code).ToList();
            return searchResult;
        }

        public void DeleteManufacture(ManufactureModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Materials.AsNoTracking().Where(r => r.ManufactureId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Manufacture);
                }
                if (db.ManufactureAvailables.AsNoTracking().Where(r => r.ManufactureId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Manufacture);
                }

                if (db.DataSheets.AsNoTracking().Where(r => r.ManufactureId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Manufacture);
                }

                if (db.NSMaterialGroups.AsNoTracking().Where(r => r.ManufactureId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Manufacture);
                }

                try
                {
                    var manufactureInGroups = db.ManufactureInGroups.Where(m => m.ManufactureId.Equals(model.Id)).ToList();
                    if (manufactureInGroups != null && manufactureInGroups.Count() > 0)
                    {
                        db.ManufactureInGroups.RemoveRange(manufactureInGroups);
                    }

                    var manufacture = db.Manufactures.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (manufacture == null)
                    {
                        throw new Exception("Hãng không tồn tại");
                    }
                    var manufactureAvailableList = db.ManufactureAvailables.Where(u => u.ManufactureId.Equals(model.Id)).ToList();
                    if (manufactureAvailableList != null && manufactureAvailableList.Count() > 0)
                    {
                        db.ManufactureAvailables.RemoveRange(manufactureAvailableList);
                    }


                    //var jsonApter = AutoMapperConfig.Mapper.Map<ManufactureHistoryModel>(manufacture);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Manufacture, manufacture.Id, manufacture.Name, jsonApter);

                    db.Manufactures.Remove(manufacture);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
                }
            }
        }

        public void AddManufacture(ManufactureModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Manufactures.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Manufacture);
                }
                if (db.Manufactures.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Manufacture);
                }

                if (!model.Email.Equals("") && model.Email != null)
                {
                    if (db.Manufactures.AsNoTracking().Where(o => o.Email.Equals(model.Email)).Count() > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Manufacture);
                    }
                }

                try
                {
                    Manufacture newManufacture = new Manufacture
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Description = model.Description.Trim(),
                        Phone = model.Phone.Trim(),
                        Origination = model.Origination.Trim(),
                        Address = model.Address.Trim(),
                        Email = model.Email.Trim(),
                        Status = model.Status,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        MaterialType = model.MaterialType,
                        Website = model.Website.Trim(),
                        LeadTime = model.Leadtime,

                    };
                    db.Manufactures.Add(newManufacture);

                    if (model.ListManufactureGroupId.Count > 0)
                    {
                        foreach (var item in model.ListManufactureGroupId)
                        {
                            ManufactureInGroup manufactureInGroup = new ManufactureInGroup()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ManufactureId = newManufacture.Id,
                                ManufactureGroupId = item,
                            };
                            db.ManufactureInGroups.Add(manufactureInGroup);
                        }
                    }


                    UserLogUtil.LogHistotyAdd(db, newManufacture.CreateBy, newManufacture.Code, newManufacture.Id, Constants.LOG_Manufacture);

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

        public void UpdateManufacture(ManufactureModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Manufactures.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Manufacture);
                }
                if (db.Manufactures.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Manufacture);
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (db.Manufactures.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Email.Equals(model.Email)).Count() > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Manufacture);
                    }
                }

                try
                {
                    var newManufacture = db.Manufactures.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ManufactureHistoryModel>(newManufacture);

                    newManufacture.Name = model.Name.Trim();
                    newManufacture.Code = model.Code.Trim();
                    newManufacture.Description = model.Description.NTSTrim();
                    newManufacture.Email = model.Email.NTSTrim();
                    newManufacture.Phone = model.Phone.NTSTrim();
                    newManufacture.Origination = model.Origination.NTSTrim();
                    newManufacture.Address = model.Address.NTSTrim();
                    newManufacture.Status = model.Status;
                    newManufacture.UpdateBy = model.UpdateBy;
                    newManufacture.UpdateDate = DateTime.Now;
                    newManufacture.MaterialType = model.MaterialType;
                    newManufacture.Website = model.Website.NTSTrim();
                    newManufacture.LeadTime = model.Leadtime;

                    var manufactureInGroups = db.ManufactureInGroups.Where(m => m.ManufactureId.Equals(model.Id)).ToList();
                    if (manufactureInGroups.Count() > 0)
                    {
                        db.ManufactureInGroups.RemoveRange(manufactureInGroups);
                    }
                    if (model.ListManufactureGroupId.Count > 0)
                    {
                        List<ManufactureInGroup> listEntity = new List<ManufactureInGroup>();
                        foreach (var item in model.ListManufactureGroupId)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                ManufactureInGroup manufactureInGroup = new ManufactureInGroup()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ManufactureId = newManufacture.Id,
                                    ManufactureGroupId = item,
                                };
                                listEntity.Add(manufactureInGroup);
                            }
                        }
                        db.ManufactureInGroups.AddRange(listEntity);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ManufactureHistoryModel>(newManufacture);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Manufacture, newManufacture.Id, newManufacture.Code, jsonBefor, jsonApter);

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

        public ManufactureModel GetManufactureInfo(ManufactureModel model)
        {
            var resultInfo = db.Manufactures.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ManufactureModel
            {
                Id = p.Id,
                Phone = p.Phone,
                Address = p.Address,
                Email = p.Email,
                Description = p.Description,
                Code = p.Code,
                Name = p.Name,
                Status = p.Status,
                Origination = p.Origination,
                MaterialType = p.MaterialType,
                Website = p.Website,
                Leadtime = p.LeadTime,
            }).FirstOrDefault();

            var listmanufactureInGroups = db.ManufactureInGroups.AsNoTracking().Where(m => m.ManufactureId.Equals(model.Id)).ToList();
            List<MaterialGroupModel> listModel = new List<MaterialGroupModel>();
            listModel = (from a in db.MaterialGroups.AsNoTracking()
                         orderby a.Code
                         select new MaterialGroupModel()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Code = a.Code,
                             ParentId = a.ParentId
                         }).ToList();
            if (listModel.Count > 0)
            {
                foreach (var item in listModel)
                {
                    var groups = db.MaterialGroups.AsNoTracking().Where(t => item.Id.Equals(t.ParentId)).ToList();
                    if (groups.Count > 0)
                    {
                        foreach (var ite in groups)
                        {
                            item.ListChildId.Add(ite.Id);
                        }
                    }
                }
            }

            List<string> listManufactureGroupId = new List<string>();
            foreach (var item in listmanufactureInGroups)
            {
                var materialGroup = listModel.FirstOrDefault(t => item.ManufactureGroupId.Equals(t.Id));
                if (materialGroup != null)
                {
                    if (!string.IsNullOrEmpty(materialGroup.ParentId))
                    {
                        var materialGroupParent = listModel.FirstOrDefault(t => materialGroup.ParentId.Equals(t.Id));
                        if (materialGroupParent != null)
                        {
                            if (materialGroupParent.ListChildId.Count > 0)
                            {
                                var count = 0;
                                foreach (var ite in materialGroupParent.ListChildId)
                                {
                                    var check = listmanufactureInGroups.FirstOrDefault(t => ite.Equals(t.ManufactureGroupId));
                                    if (check != null)
                                    {
                                        count++;
                                    }
                                }
                                if (materialGroupParent.ListChildId.Count == count)
                                {
                                    if (!string.IsNullOrEmpty(materialGroupParent.ParentId))
                                    {
                                        GetMaterialGroupParentId(materialGroupParent.Id, listManufactureGroupId, listModel, listmanufactureInGroups);
                                    }
                                    else
                                    {
                                        var exist = listManufactureGroupId.Contains(materialGroupParent.Id);
                                        if (!exist)
                                        {
                                            listManufactureGroupId.Add(materialGroupParent.Id);
                                        }
                                    }

                                }
                                else
                                {
                                    listManufactureGroupId.Add(item.ManufactureGroupId);
                                }
                            }
                        }
                    }
                    else
                    {
                        var exist = listManufactureGroupId.Contains(item.ManufactureGroupId);
                        if (!exist)
                        {
                            listManufactureGroupId.Add(item.ManufactureGroupId);
                        }

                    }
                }

            }
            resultInfo.ListManufactureGroupId = listManufactureGroupId;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Manufacture);
            }

            return resultInfo;
        }

        private void GetMaterialGroupParentId(string id, List<string> listManufactureGroupId, List<MaterialGroupModel> listModel, List<ManufactureInGroup> manufactureInGroups)
        {
            var materialGroup = listModel.FirstOrDefault(t => id.Equals(t.Id));
            if (materialGroup != null)
            {
                var materialGroupParent = listModel.FirstOrDefault(t => materialGroup.ParentId.Equals(t.Id));
                if (materialGroupParent.ListChildId.Count > 0)
                {
                    var count = 0;
                    foreach (var ite in materialGroupParent.ListChildId)
                    {
                        var check = manufactureInGroups.FirstOrDefault(t => ite.Equals(t.ManufactureGroupId));
                        if (check != null)
                        {
                            count++;
                        }
                    }
                    if (materialGroupParent.ListChildId.Count == count)
                    {
                        if (!string.IsNullOrEmpty(materialGroupParent.ParentId))
                        {
                            GetMaterialGroupParentId(materialGroupParent.ParentId, listManufactureGroupId, listModel, manufactureInGroups);
                        }
                        else
                        {
                            var exist = listManufactureGroupId.Contains(materialGroupParent.Id);
                            if (!exist)
                            {
                                listManufactureGroupId.Add(materialGroupParent.Id);
                            }
                        }

                    }
                    else
                    {
                        listManufactureGroupId.Add(id);
                    }
                }
            }
        }

        public string ExportExcel(ManufactureSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.ManufactureInGroups.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             join c in db.MaterialGroups.AsNoTracking() on a.ManufactureGroupId equals c.Id
                             //where a.ManufactureGroupId.Equals(modelSearch.ManufactureGroupId)
                             select new ManufactureResultModel
                             {
                                 Id = b.Id,
                                 ManufactureGroupId = a.ManufactureGroupId,
                                 MaterialGroupCode = c.Code,
                                 Code = b.Code,
                                 Name = b.Name,
                                 Description = b.Description,
                                 Address = b.Address,
                                 Phone = b.Phone,
                                 Email = b.Email,
                                 Status = b.Status,
                                 Origination = b.Origination,
                                 MaterialType = b.MaterialType,
                                 Website = b.Website
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ManufactureGroupId))
            {
                dataQuery = dataQuery.Where(u => u.ManufactureGroupId.Equals(model.ManufactureGroupId));
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.MaterialType))
            {
                dataQuery = dataQuery.Where(u => u.MaterialType.Equals(model.MaterialType));
            }

            List<ManufactureResultModel> listRs = new List<ManufactureResultModel>();
            var lstRs = dataQuery.GroupBy(t => new
            {
                t.Id,
                t.Code,
                t.Name,
                t.Description,
                t.Address,
                t.Phone,
                t.Email,
                t.Status,
                t.Origination,
                t.MaterialType,
                t.Website
            }).ToList();
            foreach (var item in lstRs)
            {
                ManufactureResultModel rs = new ManufactureResultModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.Description = item.Key.Description;
                rs.Address = item.Key.Address;
                rs.Phone = item.Key.Phone;
                rs.Email = item.Key.Email;
                rs.Status = item.Key.Status;
                rs.Origination = item.Key.Origination;
                rs.MaterialType = item.Key.MaterialType;
                rs.Website = item.Key.Website;
                List<string> lstSTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstSTemp.Count > 0)
                    {
                        if (!lstSTemp.Contains(ite.MaterialGroupCode))
                        {
                            rs.MaterialGroupCode += ", " + ite.MaterialGroupCode;
                            lstSTemp.Add(ite.MaterialGroupCode);

                        }
                    }
                    else
                    {
                        rs.MaterialGroupCode += ite.MaterialGroupCode;
                        lstSTemp.Add(ite.MaterialGroupCode);
                    }
                }
                listRs.Add(rs);
            }

            var listStaff = listRs.ToList();

            if (listStaff.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/HangSanXuat.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listStaff.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listStaff.Select((o, i) => new
                {
                    Index = i + 1,
                    o.MaterialGroupCode,
                    o.Code,
                    o.Name,
                    MaterialTypeView = o.MaterialType.Equals("1") ? "Vật tư tiêu chuẩn" : "Vật tư phi tiêu chuẩn",
                    StatusType = o.Status.Equals("0") ? "Còn sử dụng" : "Không sử dụng",
                    o.Origination,
                    o.Website,
                }); ;


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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách hãng sản xuất" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách hãng sản xuất" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý");
            }
        }

        public string GetGroupInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/HangSanXuat_Import_Template.xls"));
            IWorksheet sheet0 = workbook.Worksheets[0];
            IWorksheet sheet1 = workbook.Worksheets[1];
            IWorksheet sheet2 = workbook.Worksheets[2];

            var listGroup = db.MaterialGroups.AsNoTracking().Select(i => i.Code).ToList();
            var listCountrie = db.Countries.AsNoTracking().Select(i => i.CountryName).ToList();
            sheet0.Range["B3:B1000"].DataValidation.DataRange = sheet1.Range["A1:A1000"];
            sheet0.Range["G3:G1000"].DataValidation.DataRange = sheet2.Range["A1:A1000"];
            IRange iRangeData = sheet1.FindFirst("<materialGroupCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<materialGroupCode>", string.Empty);
            IRange iRangeDatas = sheet2.FindFirst("<country>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDatas.Text = iRangeDatas.Text.Replace("<country>", string.Empty);
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
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "HangSanXuat_Import_Template" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "HangSanXuat_Import_Template" + ".xls";

            return resultPathClient;
        }

        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string manufactureGroup, Name, Code, materialtype, status, country, website, note;
            string[] arrListMaterialGroup = { };
            var manufactures = db.Manufactures.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<Manufacture> list = new List<Manufacture>();
            List<ManufactureInGroup> manufactureInGroups = new List<ManufactureInGroup>();
            Manufacture itemC;
            ManufactureInGroup itemD;
            List<int> rowManufactureGroup = new List<int>();
            List<int> rowName = new List<int>(); // check có phải bắt buộc k
            List<int> rowCode = new List<int>();
            List<int> rowmaterialtype = new List<int>();
            List<int> rowstatus = new List<int>();
            List<int> rowCheckManufactureGroup = new List<int>();
            List<int> rowCheckName = new List<int>();
            List<int> rowCheckCode = new List<int>(); // check tòn tại
            List<int> rowCheckStatus = new List<int>();
            List<int> rowCheck = new List<int>();
            List<string> rowlocation = new List<string>();
            List<string> stringId = new List<string>();

            if (rowCount < 3)
            {
                throw NTSException.CreateInstance("File import không đúng. Chọn file khác");
            }

            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new Manufacture();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemD = new ManufactureInGroup();
                    itemD.Id = Guid.NewGuid().ToString();



                    manufactureGroup = sheet[i, 2].Value;
                    Code = sheet[i, 3].Value;
                    Name = sheet[i, 4].Value;
                    materialtype = sheet[i, 5].Value;
                    status = sheet[i, 6].Value;
                    country = sheet[i, 7].DisplayText; // lấy luôn tren table
                    website = sheet[i, 8].Value;
                    note = sheet[i, 9].Value;


                    //ManufactureGroup
                    try
                    {
                        if (!string.IsNullOrEmpty(manufactureGroup))
                        {
                            var Total = manufactureGroup.LastIndexOf(",");
                            if (Total > 0)
                            {
                                arrListMaterialGroup = manufactureGroup.Split(',');
                                foreach (var item in arrListMaterialGroup)
                                {
                                    var ite = db.MaterialGroups.FirstOrDefault(u => item.Equals(u.Code)).Id;
                                    if (ite != null)
                                    {
                                        stringId.Add(ite);
                                    }
                                    else
                                    {
                                        rowlocation.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                itemD.ManufactureGroupId = db.MaterialGroups.Where(u => u.Code.Equals(manufactureGroup.Trim())).FirstOrDefault().Id;
                            }
                        }
                        else
                        {
                            rowManufactureGroup.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemD.ManufactureGroupId == null)
                        {
                            rowCheckManufactureGroup.Add(i);
                        }
                        else
                        {
                            rowManufactureGroup.Add(i);
                        }
                        continue;
                    }

                    //Code
                    try
                    {
                        if (!string.IsNullOrEmpty(Code))
                        {
                            itemC.Code = Code;
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
                            if (db.Manufactures.AsNoTracking().Where(o => o.Name.Equals(Name)).Count() > 0 && db.Manufactures.AsNoTracking().Where(o => o.Code.Equals(Code)).Count() == 0)
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
                        }
                    }
                    catch (Exception)
                    {
                        rowName.Add(i);
                        continue;
                    }

                    //MaterialType
                    try
                    {
                        if (!string.IsNullOrEmpty(materialtype))
                        {
                            if (materialtype.Equals("Vật tư tiêu chuẩn"))
                            {
                                itemC.MaterialType = "1";
                            }
                            if (materialtype.Equals("Vật tư phi tiêu chuẩn"))
                            {
                                itemC.MaterialType = "2";
                            }
                        }
                        else
                        {
                            rowmaterialtype.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowmaterialtype.Add(i);
                        continue;
                    }

                    //Status
                    try
                    {
                        if (!string.IsNullOrEmpty(status))
                        {
                            if (status.Equals("Còn sử dụng"))
                            {
                                itemC.Status = "0";
                            }
                            if (status.Equals("Không sử dụng"))
                            {
                                itemC.Status = "1";
                            }
                        }
                        else
                        {
                            rowstatus.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowstatus.Add(i);
                        continue;
                    }

                    //country
                    if (!string.IsNullOrEmpty(country))
                    {
                        itemC.Origination = country;
                    }
                    //Website
                    if (!string.IsNullOrEmpty(website))
                    {
                        itemC.Website = website;
                    }
                    if (!string.IsNullOrEmpty(note))
                    {
                        itemC.Description = note;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;
                    itemD.ManufactureId = itemC.Id;
                    if (manufactureGroup.LastIndexOf(",") > 0)
                    {
                        foreach (var item in stringId)
                        {
                            var check = db.Manufactures.FirstOrDefault(t => t.Code.ToUpper().Equals(itemC.Code.ToUpper()));
                            if (check != null)
                            {
                                var group = db.ManufactureInGroups.FirstOrDefault(t => t.ManufactureId.Equals(check.Id));
                                if (group != null)
                                {
                                    group.ManufactureGroupId = itemD.ManufactureGroupId;
                                    check.Code = itemC.Code;
                                    check.Name = itemC.Name;
                                    check.MaterialType = itemC.MaterialType;
                                    check.Origination = itemC.Origination;
                                    check.Website = itemC.Website;
                                    check.Description = itemC.Description;
                                    check.UpdateBy = userId;
                                    check.UpdateDate = DateTime.Now;
                                }
                                else
                                {
                                    itemD = new ManufactureInGroup();
                                    itemD.Id = Guid.NewGuid().ToString();
                                    itemD.ManufactureGroupId = item;
                                    itemD.ManufactureId = itemC.Id;
                                    manufactureInGroups.Add(itemD);
                                }
                            }
                            else
                            {
                                itemD = new ManufactureInGroup();
                                itemD.Id = Guid.NewGuid().ToString();
                                itemD.ManufactureGroupId = item;
                                itemD.ManufactureId = itemC.Id;
                                manufactureInGroups.Add(itemD);
                            }
                        }
                        list.Add(itemC);
                    }
                    else
                    {
                        var check = db.Manufactures.FirstOrDefault(t => t.Code.ToUpper().Equals(itemC.Code.ToUpper()));
                        if (check != null)
                        {
                            var group = db.ManufactureInGroups.FirstOrDefault(t => t.ManufactureId.Equals(check.Id));
                            if (group != null)
                            {
                                group.ManufactureGroupId = itemD.ManufactureGroupId;
                            }
                            else
                            {
                                rowCheck.Add(i);
                            }

                            check.Code = itemC.Code;
                            check.Name = itemC.Name;
                            check.MaterialType = itemC.MaterialType;
                            check.Origination = itemC.Origination;
                            check.Website = itemC.Website;
                            check.Description = itemC.Description;
                            check.UpdateBy = userId;
                            check.UpdateDate = DateTime.Now;
                        }
                        else
                        {
                            list.Add(itemC);
                            manufactureInGroups.Add(itemD);
                        }
                    }
                }
                if (rowlocation.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhóm vật tư <" + string.Join(", ", rowlocation) + "> không tồn tại!");
                }
                if (rowCheck.Count > 0)
                {
                    throw NTSException.CreateInstance("Hãng sản xuất dòng <" + string.Join(", ", rowCheck) + "> đã tồn tại nhưng chưa chọn nhóm!");
                }

                if (rowManufactureGroup.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhóm vật tư <" + string.Join(", ", rowManufactureGroup) + "> không được phép để trống!");
                }

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowCode) + "> không được phép để trống!");
                }

                if (rowName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên hãng sản xuất dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                }

                if (rowmaterialtype.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại vật tư dòng <" + string.Join(", ", rowmaterialtype) + "> không được phép để trống!");
                }

                if (rowstatus.Count > 0)
                {
                    throw NTSException.CreateInstance("Tình trạng dòng <" + string.Join(", ", rowstatus) + "> không được phép để trống!");
                }

                if (rowCheckManufactureGroup.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhóm vật tư dòng <" + string.Join(", ", rowCheckManufactureGroup) + "> không tồn tại. Nhập tên khác!");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã hãng sản xuất dòng <" + string.Join(", ", rowCheckCode) + "> đã tồn tại. Nhập mã khác!");
                }

                if (rowCheckName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên hãng sản xuất dòng <" + string.Join(", ", rowCheckName) + "> đã tồn tại. Nhập tên khác!");
                }

                if (rowCheckStatus.Count > 0)
                {
                    throw NTSException.CreateInstance("Trạng thái dòng <" + string.Join(", ", rowCheckStatus) + "> không đúng. Nhập lại!");
                }
                #endregion
                db.Manufactures.AddRange(list);
                db.ManufactureInGroups.AddRange(manufactureInGroups);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
        }

        public List<ManufactureResultModel> GetListManufacture()
        {
            List<ManufactureResultModel> searchResult = new List<ManufactureResultModel>();
            searchResult = (from a in db.Manufactures.AsNoTracking()
                            orderby a.Name
                            select new ManufactureResultModel
                            {
                                Id = a.Id,
                                Code = a.Code,
                                Name = a.Name,
                                Description = a.Description,
                                Address = a.Address,
                                Phone = a.Phone,
                                Email = a.Email,
                                MaterialType = a.MaterialType,
                                Website = a.Website,
                                Status = a.Status,
                            }).ToList();
            return searchResult;
        }

        public MaterialGroupModel GetListParentId(List<string> Id)
        {
            MaterialGroupModel data = new MaterialGroupModel();
            List<string> ListId = new List<string>();
            List<string> ListIds = new List<string>();
            List<string> ListName = new List<string>();
            List<string> ListNames = new List<string>();
            foreach (var item in Id)
            {
                ListIds = db.MaterialGroups.AsNoTracking().Where(i => i.ParentId.Equals(item)).Select(i => i.Id).ToList();
                ListNames = db.MaterialGroups.AsNoTracking().Where(i => i.ParentId.Equals(item)).Select(i => i.Name).ToList();
                var ListChild = GetListParentId(ListId);
                if (ListChild.ListParentId.Count > 0)
                {
                    ListIds.AddRange(ListChild.ListParentId);
                    ListNames.AddRange(ListChild.ListParentName);
                }
                if (ListIds.Count > 0)
                {
                    ListId.AddRange(ListIds);
                    ListName.AddRange(ListNames);
                }
            }
            data.ListParentId = ListId;
            data.ListParentName = ListName;
            return data;
        }

        public MaterialGroupModel GetId(string Id)
        {
            MaterialGroupModel data = new MaterialGroupModel();
            List<string> ListId = new List<string>();
            List<string> ListIds = new List<string>();
            List<string> ListName = new List<string>();
            List<string> ListNames = new List<string>();
            ListIds = db.MaterialGroups.Where(i => i.Id.Equals(Id)).Select(i => i.ParentId).ToList();
            ListNames = db.MaterialGroups.Where(i => i.Id.Equals(Id)).Select(i => i.Name).ToList();
            if (ListIds.Count > 0)
            {
                ListId.AddRange(ListIds);
                ListName.AddRange(ListNames);
                var ListChild = GetId(ListIds[0]);
                ListIds.AddRange(ListChild.ListParentId);
                ListNames.AddRange(ListChild.ListParentName);
            }
            data.ListParentId = ListId;
            data.ListParentName = ListName;
            return data;
        }
    }
}
