using NTS.Business.Combobox;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.Manufacture;
using NTS.Model.MaterialGroup;
using NTS.Model.Repositories;
using NTS.Model.Supplier;
using NTS.Model.SupplierHistory;
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

namespace QLTK.Business.Suppliers
{
    public class SupplierBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public object SearchSupplier(SupplierSearchModel modelSearch)
        {
            SearchResultModel<SupplierResultModel> searchResult = new SearchResultModel<SupplierResultModel>();
            List<string> listParentId = new List<string>();

            List<string> supplierIds = new List<string>();
            if (!string.IsNullOrEmpty(modelSearch.SupplierGroupId))
            {
                var materialGroups = db.MaterialGroups.AsNoTracking().ToList();

                var materialGroup = materialGroups.FirstOrDefault(i => i.Id.Equals(modelSearch.SupplierGroupId));

                if (materialGroup != null)
                {
                    listParentId.Add(materialGroup.Id);
                }

                listParentId.AddRange(GetListParent(modelSearch.SupplierGroupId, materialGroups));
                supplierIds = (from s in db.SupplierInGroups.AsNoTracking()
                               where listParentId.Contains(s.SupplierGroupId)
                               group s by s.SupplierId into g
                               select g.Key).ToList();
            }

            if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
            {
                var supplierManuIds = (from m in db.SupplierManufactures.AsNoTracking()
                                       where m.ManufactureId.Equals(modelSearch.ManufactureId)
                                       group m by m.SupplierId into g
                                       select g.Key).ToList();

                supplierIds.AddRange(supplierManuIds);
            }

            supplierIds = supplierIds.Distinct().ToList();

            var dataQuery = (from b in db.Suppliers.AsNoTracking()
                             select new SupplierResultModel
                             {
                                 Id = b.Id,
                                 Code = b.Code,
                                 Name = b.Name,
                                 Alias = b.Alias,
                                 Note = b.Note,
                                 PhoneNumber = b.PhoneNumber,
                                 Email = b.Email,
                                 Status = b.Status,
                                 BankPayment = b.BankPayment,
                                 Address = b.Address,
                                 Country = b.Country,
                                 DeliveryTime = b.DeliveryTime,
                                 TypePayment = b.TypePayment,
                                 RulesDelivery = b.RulesDelivery,
                                 RulesPayment = b.RulesPayment,
                                 Website = b.Website
                             });

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.Trim().ToUpper().Contains(modelSearch.Name.Trim().ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Email))
            {
                dataQuery = dataQuery.Where(u => u.Email.ToUpper().Contains(modelSearch.Email.ToUpper()));
            }

            if (supplierIds.Count > 0)
            {
                dataQuery = dataQuery.Where(u => supplierIds.Contains(u.Id));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            if (listResult.Count > 0)
            {
                List<string> manuS;
                foreach (var item in listResult)
                {

                    manuS = (from s in db.SupplierManufactures.AsNoTracking()
                             where s.SupplierId.Equals(item.Id)
                             join m in db.Manufactures.AsNoTracking() on s.ManufactureId equals m.Id
                             select m.Code).ToList();

                    item.ManufactureName = string.Join(", ", manuS);

                    item.ListSupplierContact = db.SupplierContacts.AsNoTracking().Where(t => t.SupplierId.Equals(item.Id)).Select(m => new SupplierContactModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Location = m.Location,
                        PhoneNumber = m.PhoneNumber,
                        Email = m.Email,
                    }).ToList();
                }
            }

            searchResult.ListResult = listResult;

            return searchResult;
        }

        public SearchResultModel<ManufactureModel> SearchSupplierManufacture(ManufactureSearchModel modelSearch)
        {
            SearchResultModel<ManufactureModel> searchResult = new SearchResultModel<ManufactureModel>();
            var dataQuery = (from a in db.Manufactures.AsNoTracking()
                             join b in db.Materials.AsNoTracking() on a.Id equals b.ManufactureId into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.MaterialBuyHistories.AsNoTracking() on b.Id equals c.MaterialId into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Suppliers.AsNoTracking() on c.SupplierId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             orderby a.Name
                             select new ManufactureModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Description = a.Description,
                                 Address = a.Address,
                                 Origination = a.Origination,
                                 MaterialType = a.MaterialType,
                                 Website = a.Website,
                                 SupplierId = d.Id,
                             }).AsQueryable();
            dataQuery = dataQuery.Where(u => u.SupplierId.Equals(modelSearch.SupplierId));

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            var listResult = dataQuery.ToList();
            List<ManufactureModel> listRs = new List<ManufactureModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.Description, t.Address, t.Origination, t.MaterialType, t.Website }).ToList();
            foreach (var item in lstRs)
            {
                ManufactureModel rs = new ManufactureModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.Description = item.Key.Description;
                rs.Address = item.Key.Address;
                rs.Origination = item.Key.Origination;
                rs.MaterialType = item.Key.MaterialType;
                rs.Website = item.Key.Website;
                listRs.Add(rs);
            }
            searchResult.ListResult = listRs;
            return searchResult;
        }

        public string AddSupplier(SupplierModel model)
        {
            //xoá ký tự đặc biệt            
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            string suppliserId = Guid.NewGuid().ToString();
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Suppliers.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Supplier);
                }

                if (db.Suppliers.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Supplier);
                }

                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (db.Suppliers.AsNoTracking().Where(o => o.Email.Equals(model.Email)).Count() > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Supplier);
                    }
                }

                try
                {
                    Supplier newSupplier = new Supplier
                    {
                        Id = suppliserId,
                        Name = model.Name.Trim(),
                        Alias = model.Alias.Trim(),
                        Code = model.Code.Trim(),
                        Note = model.Note.Trim(),
                        PhoneNumber = model.PhoneNumber.Trim(),
                        Email = model.Email.Trim(),
                        Status = model.Status,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        BankPayment = model.BankPayment,
                        TypePayment = model.TypePayment,
                        RulesPayment = model.RulesPayment,
                        DeliveryTime = model.DeliveryTime,
                        Address = model.Address,
                        RulesDelivery = model.RulesDelivery,
                        Country = model.Country,
                        Website = model.Website,
                    };

                    db.Suppliers.Add(newSupplier);

                    if (model.ListSupplierContact.Count > 0)
                    {
                        foreach (var item in model.ListSupplierContact)
                        {
                            SupplierContact newSupplierContact = new SupplierContact
                            {
                                Id = Guid.NewGuid().ToString(),
                                SupplierId = newSupplier.Id,
                                Name = item.Name.NTSTrim(),
                                Location = item.Location.NTSTrim(),
                                PhoneNumber = item.PhoneNumber.NTSTrim(),
                                Email = item.Email.NTSTrim(),
                            };
                            db.SupplierContacts.Add(newSupplierContact);
                        }
                    }

                    if (model.ListSupplierGroupId.Count > 0)
                    {
                        foreach (var item in model.ListSupplierGroupId)
                        {
                            SupplierInGroup supplierInGroup = new SupplierInGroup()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SupplierId = newSupplier.Id,
                                SupplierGroupId = item,
                            };
                            db.SupplierInGroups.Add(supplierInGroup);
                        }
                    }

                    if (model.ListManfacture.Count > 0)
                    {
                        foreach (var item in model.ListManfacture)
                        {
                            SupplierManufacture supplierManufacture = new SupplierManufacture
                            {
                                Id = Guid.NewGuid().ToString(),
                                SupplierId = newSupplier.Id,
                                ManufactureId = item.Id,
                            };
                            db.SupplierManufactures.Add(supplierManufacture);
                        }
                    }

                    if (model.Contracts.Count > 0)
                    {
                        foreach (var item in model.Contracts)
                        {
                            SupplierContract supplierContract = new SupplierContract
                            {
                                Id = Guid.NewGuid().ToString(),
                                LaborContractId = item.LaborContractId,
                                SupplierId = newSupplier.Id,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                Path = item.Path,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now,
                            };
                            db.SupplierContracts.Add(supplierContract);
                        }
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, newSupplier.Id, Constants.LOG_Supplier);


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }

            return suppliserId;
        }

        public string GetSupplierCode()
        {
            var suppliers = db.Suppliers.Where(r => r.Code.Contains("NCC")).Select(s => s.Code).OrderBy(o => o).ToList();

            int index = 1;

            while (true)
            {
                if (suppliers.IndexOf($"NCC{string.Format("{0:0000}", index)}") != -1)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }

            return $"NCC{string.Format("{0:0000}", index)}";
        }

        public object GetSupplierInfo(SupplierModel model)
        {
            var resultInfo = db.Suppliers.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SupplierModel
            {
                Id = p.Id,
                PhoneNumber = p.PhoneNumber,
                Alias = p.Alias,
                Email = p.Email,
                Note = p.Note,
                Code = p.Code,
                Name = p.Name,
                Status = p.Status,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate,
                Website = p.Website,
                BankPayment = p.BankPayment,
                Country = p.Country,
                Address = p.Address,
                DeliveryTime = p.DeliveryTime,
                TypePayment = p.TypePayment,
                RulesDelivery = p.RulesDelivery,
                RulesPayment = p.RulesPayment,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Supplier);
            }

            var supplierInGroups = db.SupplierInGroups.AsNoTracking().Where(m => m.SupplierId.Equals(model.Id)).ToList();
            List<string> listSupplierGroupId = new List<string>();
            foreach (var item in supplierInGroups)
            {
                listSupplierGroupId.Add(item.SupplierGroupId);
            }
            resultInfo.ListSupplierGroupId = listSupplierGroupId;

            var listSupplierContact = (from a in db.SupplierContacts.AsNoTracking()
                                       where a.SupplierId.Equals(model.Id)
                                       orderby a.Name
                                       select new SupplierContactModel
                                       {
                                           Id = a.Id,
                                           Name = a.Name,
                                           Location = a.Location,
                                           PhoneNumber = a.PhoneNumber,
                                           Email = a.Email,
                                       });
            resultInfo.ListSupplierContact = listSupplierContact.ToList();

            var listManufacture = (from a in db.SupplierManufactures.AsNoTracking()
                                   join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                                   where a.SupplierId.Equals(model.Id)
                                   select new ManufactureResultModel
                                   {
                                       Id = b.Id,
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
                                   }).ToList();
            resultInfo.ListManfacture = listManufacture;

            resultInfo.Contracts = (from a in db.SupplierContracts.AsNoTracking()
                                    where a.SupplierId.Equals(model.Id)
                                    join b in db.Users.AsNoTracking() on a.UpdateBy equals b.Id
                                    join c in db.LaborContracts.AsNoTracking() on a.LaborContractId equals c.Id
                                    select new SuppilerContractModel
                                    {
                                        Id = a.Id,
                                        LaborContractName = c.Name,
                                        SupplierId = a.SupplierId,
                                        FileName = a.FileName,
                                        FileSize = a.FileSize,
                                        Path = a.Path,
                                        CreateByName = b.UserName,
                                        UpdateDate = a.UpdateDate,
                                        SignDate = a.SignDate,
                                        DueDate = a.DueDate,
                                        Name = a.Name,
                                        Code= a.Code,
                                    }).ToList();

            return resultInfo;
        }
        public object GetSupplierContractInfo(SupplierContractModel model)
        {
            var resultInfo = (from a in db.SupplierContracts.AsNoTracking()
                               where a.Id.Equals(model.Id)
                               join b in db.Users.AsNoTracking() on a.UpdateBy equals b.Id
                               join c in db.LaborContracts.AsNoTracking() on a.LaborContractId equals c.Id
                               select new SuppilerContractModel
                               {
                                   Id = a.Id,
                                   LaborContractId = c.Id,
                                   SupplierId = a.SupplierId,
                                   FileName = a.FileName,
                                   FileSize = a.FileSize,
                                   Path = a.Path,
                                   CreateByName = b.UserName,
                                   UpdateDate = a.UpdateDate,
                                   SignDate = a.SignDate,
                                   DueDate = a.DueDate,
                                   Name = a.Name,
                                   Code = a.Code,
                               }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SupplierContract);
            }
            return resultInfo;
        }

         public void UpdateSupplierContract(SupplierContractModel model)
         {  

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var supplierContract = db.SupplierContracts.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    supplierContract.FileName = model.FileName;
                    supplierContract.FileSize = model.FileSize;
                    supplierContract.LaborContractId = model.LaborContractId;
                    supplierContract.Path = model.Path;
                    supplierContract.UpdateBy = model.UpdateBy;
                    supplierContract.UpdateDate = DateTime.Now;
                    supplierContract.SignDate = model.SignDate;
                    supplierContract.DueDate = model.DueDate;
                    supplierContract.Name = model.Name.Trim();
                    supplierContract.Code = model.Code.Trim();

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

        public void UpdateSupplier(SupplierModel model)
        {  //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Suppliers.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Supplier);
                }
                var supplier = db.Suppliers.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                if (supplier == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Supplier);
                }
                //var jsonApter = AutoMapperConfig.Mapper.Map<SupplierHistoryModel>(supplier);


                if (!string.IsNullOrEmpty(model.Email))
                {
                    if (db.Suppliers.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Email.Equals(model.Email)).Count() > 0)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Supplier);
                    }
                }

                var listSupplierContact = db.SupplierContacts.Where(s => s.SupplierId.Equals(model.Id)).ToList();

                try
                {
                    if (listSupplierContact.Count() > 0)
                    {
                        db.SupplierContacts.RemoveRange(listSupplierContact);
                    }
                    supplier.Alias = model.Alias.NTSTrim();
                    supplier.Name = model.Name.Trim();
                    supplier.Code = model.Code.Trim();
                    supplier.Note = model.Note.NTSTrim();
                    supplier.Email = model.Email.NTSTrim();
                    supplier.PhoneNumber = model.PhoneNumber.NTSTrim();
                    supplier.Status = model.Status;
                    supplier.UpdateBy = model.UpdateBy;
                    supplier.UpdateDate = DateTime.Now;
                    supplier.BankPayment = model.BankPayment;
                    supplier.TypePayment = model.TypePayment;
                    supplier.RulesPayment = model.RulesPayment;
                    supplier.DeliveryTime = model.DeliveryTime;
                    supplier.Address = model.Address;
                    supplier.RulesDelivery = model.RulesDelivery;
                    supplier.Country = model.Country;
                    supplier.Website = model.Website;

                    if (model.ListSupplierContact.Count > 0)
                    {
                        foreach (var item in model.ListSupplierContact)
                        {
                            SupplierContact newSupplierContact = new SupplierContact
                            {
                                Id = Guid.NewGuid().ToString(),
                                SupplierId = model.Id,
                                Name = item.Name.Trim(),
                                Location = item.Location.Trim(),
                                PhoneNumber = item.PhoneNumber.Trim(),
                                Email = item.Email.Trim(),
                            };
                            db.SupplierContacts.Add(newSupplierContact);
                        }
                    }

                    var supplierInGroups = db.SupplierInGroups.Where(m => m.SupplierId.Equals(model.Id)).ToList();
                    if (supplierInGroups != null && supplierInGroups.Count() > 0)
                    {
                        db.SupplierInGroups.RemoveRange(supplierInGroups);
                    }
                    if (model.ListSupplierGroupId.Count > 0)
                    {
                        foreach (var item in model.ListSupplierGroupId)
                        {
                            SupplierInGroup supplierInGroup = new SupplierInGroup()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SupplierId = model.Id,
                                SupplierGroupId = item,
                            };
                            db.SupplierInGroups.Add(supplierInGroup);
                        }
                    }

                    var supplierManufactures = db.SupplierManufactures.Where(i => i.SupplierId.Equals(model.Id)).ToList();
                    if (supplierManufactures.Count > 0)
                    {
                        db.SupplierManufactures.RemoveRange(supplierManufactures);
                    }
                    if (model.ListManfacture.Count > 0)
                    {
                        foreach (var item in model.ListManfacture)
                        {
                            SupplierManufacture supplierManufacture = new SupplierManufacture
                            {
                                Id = Guid.NewGuid().ToString(),
                                SupplierId = model.Id,
                                ManufactureId = item.Id,
                            };
                            db.SupplierManufactures.Add(supplierManufacture);
                        }
                    }

                    foreach (var item in model.Contracts)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            SupplierContract supplierContract = new SupplierContract
                            {
                                Id = Guid.NewGuid().ToString(),
                                LaborContractId = item.LaborContractId,
                                SupplierId = model.Id,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                Path = item.Path,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now,
                            };
                            db.SupplierContracts.Add(supplierContract);
                        }
                        else
                        {
                            var supplierContractExist = db.SupplierContracts.FirstOrDefault(a => a.Id.Equals(item.Id));
                            if (supplierContractExist == null)
                            {
                                continue;
                            }
                            if (item.IsDelete)
                            {
                                db.SupplierContracts.Remove(supplierContractExist);
                            }
                            else
                            {
                                if (string.Compare(item.LaborContractId, supplierContractExist.LaborContractId) != 0)
                                {
                                    supplierContractExist.LaborContractId = item.LaborContractId;
                                }
                            }

                        }
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SupplierHistoryModel>(supplier);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Supplier, supplier.Id, supplier.Code, jsonBefor, jsonApter);

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

        public void DeleteSupplier(SupplierModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.MaterialBuyHistories.AsNoTracking().Where(r => r.SupplierId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Supplier);
                }
                var supplier = db.Suppliers.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (supplier == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Supplier);
                }
                var listSupplierContact = db.SupplierContacts.Where(s => s.SupplierId.Equals(model.Id)).ToList();
                try
                {
                    var listsupplierManufa = db.SupplierManufactures.Where(s => s.SupplierId.Equals(model.Id)).ToList();

                    if (listsupplierManufa.Count > 0)
                    {
                        db.SupplierManufactures.RemoveRange(listsupplierManufa);
                    }
                    if (listSupplierContact.Count > 0)
                    {
                        db.SupplierContacts.RemoveRange(listSupplierContact);
                    }

                    var supplierInGroup = db.SupplierInGroups.Where(u => u.SupplierId.Equals(model.Id)).ToList();
                    if (supplierInGroup.Count > 0)
                    {
                        db.SupplierInGroups.RemoveRange(supplierInGroup);
                    }

                    var supplierContracts = db.SupplierContracts.Where(a => a.SupplierId.Equals(model.Id)).ToList();
                    db.SupplierContracts.RemoveRange(supplierContracts);

                    db.Suppliers.Remove(supplier);

                    var NameOrCode = supplier.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SupplierHistoryModel>(supplier);
                    //UserLogUtil.LogHistotyDelete(db, supplier.UpdateBy, Constants.LOG_Supplier, supplier.Id, NameOrCode, jsonApter);

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

        public string ExportExcel(SupplierSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.SupplierInGroups.AsNoTracking()
                             join b in db.Suppliers.AsNoTracking() on a.SupplierId equals b.Id
                             join c in db.MaterialGroups.AsNoTracking() on a.SupplierGroupId equals c.Id
                             orderby b.Name
                             select new SupplierResultModel
                             {
                                 Id = b.Id,
                                 SupplierGroupName = c.Name,
                                 Code = b.Code,
                                 Name = b.Name,
                                 Note = b.Note,
                                 PhoneNumber = b.PhoneNumber,
                                 Email = b.Email,
                                 Status = b.Status,

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.SupplierGroupId))
            {
                dataQuery = dataQuery.Where(u => u.SupplierGroupId.Equals(model.SupplierGroupId));
            }

            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                dataQuery = dataQuery.Where(u => u.Email.ToUpper().Contains(model.Email.ToUpper()));
            }

            List<SupplierResultModel> listSupplier = dataQuery.ToList();


            SupplierContact supplierContact;
            foreach (var item in listSupplier)
            {
                item.ListSupplierContact = db.SupplierContacts.AsNoTracking().Where(t => t.SupplierId.Equals(item.Id)).Select(m => new SupplierContactModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Location = m.Location,
                    PhoneNumber = m.PhoneNumber,
                    Email = m.Email,
                }).ToList();

                supplierContact = db.SupplierContacts.AsNoTracking().FirstOrDefault(u => u.SupplierId.Equals(item.Id));

                if (supplierContact != null)
                {
                    item.SupplierContactName = supplierContact.Name;
                    item.SupplierContactPhoneNumber = supplierContact.PhoneNumber;
                    item.SupplierContactEmail = supplierContact.Email;
                }
            }

            if (listSupplier.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/NhaCungCap.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = listSupplier.Count;

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = listSupplier.Select((o, i) => new
            {
                Index = i + 1,
                o.SupplierGroupName,
                o.Code,
                o.Name,
                StatusType = o.Status.Equals("0") ? "Còn sử dụng" : "Không sử dụng",
                o.Email,
                o.PhoneNumber,
                o.Note,
                o.SupplierContactName,
                o.SupplierContactPhoneNumber,
                o.SupplierContactEmail

            });


            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders.Color = ExcelKnownColors.Black;


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách nhà cung cấp" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách nhà cung cấp" + ".xls";

            return resultPathClient;
        }

        public string GetGroupInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/NhaCungCap_Import_Template.xls"));
            IWorksheet sheet0 = workbook.Worksheets[0];

            IWorksheet sheet = workbook.Worksheets[1];

            var listStaff = db.MaterialGroups.Select(i => i.Code).ToList();
            var total = listStaff.Count;
            sheet0.Range["B3:B1000"].DataValidation.DataRange = sheet.Range["A1:A1000"];
            IRange iRangeData = sheet.FindFirst("<materialGroupCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<materialGroupCode>", string.Empty);

            var listExport = listStaff.Select((o, i) => new
            {
                o,
            }); ;

            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "NhaCungCap_Import_Template" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "NhaCungCap_Import_Template" + ".xls";

            return resultPathClient;
        }

        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string SupplierGroup, Name, Code, Status, Phone, Email, Note;
            var manufactures = db.Manufactures.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<Supplier> list = new List<Supplier>();
            List<SupplierInGroup> listGroup = new List<SupplierInGroup>();
            Supplier itemC;
            SupplierInGroup itemD;
            List<int> rowSupplierGroup = new List<int>();
            List<int> rowName = new List<int>();
            List<int> rowCode = new List<int>();
            List<int> rowStatus = new List<int>();
            List<int> rowCheckSupplierGroup = new List<int>();
            List<int> rowCheckName = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<int> rowCheckStatus = new List<int>();
            List<int> rowCheckPhone = new List<int>();
            List<int> rowCheckEmail = new List<int>();

            if (rowCount < 3)
            {
                throw NTSException.CreateInstance("File import không đúng. Chọn file khác");
            }

            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new Supplier();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemD = new SupplierInGroup();
                    itemD.Id = Guid.NewGuid().ToString();
                    SupplierGroup = sheet[i, 2].Value;
                    Code = sheet[i, 3].Value;
                    Name = sheet[i, 4].Value;
                    Status = sheet[i, 5].Value;
                    Phone = sheet[i, 6].Value;
                    Email = sheet[i, 7].Value;
                    Note = sheet[i, 8].Value;

                    //SupplierGroup
                    try
                    {
                        if (!string.IsNullOrEmpty(SupplierGroup))
                        {
                            itemD.SupplierGroupId = db.MaterialGroups.FirstOrDefault(u => u.Code.Equals(SupplierGroup)).Id;
                        }
                        else
                        {
                            rowSupplierGroup.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemD.SupplierGroupId == null)
                        {
                            rowCheckSupplierGroup.Add(i);
                        }
                        else
                        {
                            rowSupplierGroup.Add(i);
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
                            if (db.Suppliers.AsNoTracking().Where(o => o.Name.Equals(Name)).Count() > 0 && db.Suppliers.AsNoTracking().Where(o => o.Code.Equals(Code)).Count() == 0)
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

                    try
                    {
                        if (!string.IsNullOrEmpty(Status))
                        {
                            if (Status.Equals("Còn sử dụng"))
                            {
                                itemC.Status = "0";
                            }
                            if (Status.Equals("Không sử dụng"))
                            {
                                itemC.Status = "1";
                            }
                        }
                        else
                        {
                            rowStatus.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckStatus.Add(i);
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

                    //Email
                    try
                    {
                        if (!string.IsNullOrEmpty(Email))
                        {
                            var email = new System.Net.Mail.MailAddress(Email);
                            itemC.Email = Email;
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.Email == null)
                        {
                            rowCheckEmail.Add(i);
                        }
                        continue;
                    }


                    if (!string.IsNullOrEmpty(Note))
                    {
                        itemC.Note = Note;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;
                    itemD.SupplierId = itemC.Id;
                    var check = db.Suppliers.FirstOrDefault(t => t.Code.ToUpper().Equals(itemC.Code.ToUpper()));
                    if (check != null)
                    {
                        var group = db.SupplierInGroups.FirstOrDefault(t => t.SupplierId.Equals(check.Id));
                        group.SupplierGroupId = itemD.SupplierGroupId;
                        check.Code = itemC.Code;
                        check.Name = itemC.Name;
                        check.PhoneNumber = itemC.PhoneNumber;
                        check.Email = itemC.Email;
                        check.Note = itemC.Note;
                        check.UpdateBy = userId;
                        check.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        list.Add(itemC);
                        listGroup.Add(itemD);
                    }
                }

                if (rowSupplierGroup.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên nhóm vật tư dòng <" + string.Join(", ", rowSupplierGroup) + "> không được phép để trống!");
                }

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhà cung cấp dòng <" + string.Join(", ", rowCode) + "> không được phép để trống!");
                }

                if (rowName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên nhà cung cấp dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                }

                if (rowStatus.Count > 0)
                {
                    throw NTSException.CreateInstance("Trạng thái dòng <" + string.Join(", ", rowStatus) + "> không được phép để trống!");
                }

                if (rowCheckSupplierGroup.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên nhóm vật tư dòng <" + string.Join(", ", rowCheckSupplierGroup) + "> không tồn tại. Nhập mã khác!");
                }

                if (rowCheckCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã nhà cung cấp dòng <" + string.Join(", ", rowCheckCode) + "> đã tồn tại. Nhập mã khác!");
                }

                if (rowCheckName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên nhà cung cấp dòng <" + string.Join(", ", rowCheckName) + "> đã tồn tại. Nhập tên khác!");
                }

                if (rowCheckStatus.Count > 0)
                {
                    throw NTSException.CreateInstance("Trạng thái dòng <" + string.Join(", ", rowCheckStatus) + "> không đúng. Nhập lại!");
                }

                if (rowCheckPhone.Count > 0)
                {
                    throw NTSException.CreateInstance("Số điện thoại dòng <" + string.Join(", ", rowCheckPhone) + "> không đúng!");
                }

                if (rowCheckEmail.Count > 0)
                {
                    throw NTSException.CreateInstance("Email dòng <" + string.Join(", ", rowCheckEmail) + "> không đúng định dạng!");
                }
                #endregion
                db.Suppliers.AddRange(list);
                db.SupplierInGroups.AddRange(listGroup);
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

        public object SearchManufacture(ManufactureSearchModel modelSearch)
        {
            SearchResultModel<ManufactureResultModel> searchResult = new SearchResultModel<ManufactureResultModel>();
            var dataQuery = (from a in db.ManufactureInGroups.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(b.Id)
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

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
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
            var listResult = list.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public List<string> GetListParent(string id, List<MaterialGroup> materialGroups)
        {
            List<string> listChild = new List<string>();

            var materialGroup = materialGroups.Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(materialGroup);
            if (materialGroup.Count > 0)
            {
                foreach (var item in materialGroup)
                {
                    listChild.AddRange(GetListParent(item, materialGroups));
                }
            }
            return listChild;
        }

        public void CreateSupplierContract(SupplierContractModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var SupplierId = db.Suppliers.AsNoTracking().Where(u => model.SupplierId.Equals(u.Code)).Select(u=> u.Id).FirstOrDefault();
                    var dateNow = DateTime.Now;
                    SupplierContract data = new SupplierContract()
                    {
                        Id = Guid.NewGuid().ToString(),
                        LaborContractId = model.LaborContractId,
                        SupplierId = SupplierId,
                        FileName = model.FileName,
                        FileSize = model.FileSize,
                        Path = model.Path,
                        SignDate = model.SignDate,
                        DueDate = model.DueDate,
                        Code = model.Code,
                        Name = model.Name.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = dateNow
                    };
                    db.SupplierContracts.Add(data);

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

        public void DeleteSupplierContract(SupplierContractModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var supplierContract = db.SupplierContracts.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (supplierContract == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SupplierContract);
                }

                db.SupplierContracts.Remove(supplierContract);

                db.SaveChanges();
                trans.Commit();

            }
        }
    }
}
