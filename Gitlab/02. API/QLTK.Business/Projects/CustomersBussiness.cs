using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.CustomerContact;
using NTS.Model.CustomerHistory;
using NTS.Model.CustomerRequirement;
using NTS.Model.Customers;
using NTS.Model.Meeting;
using NTS.Model.Project;
using NTS.Model.Repositories;
using NTS.Model.Survey;
using NTS.Utils;
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
using NTSModel = NTS.Model.Repositories;

namespace QLTK.Business.Customers
{
    public class CustomersBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<CustomersModel> SearchCustomer(CustomersSearchModel modelSearch)
        {
            SearchResultModel<CustomersModel> searchResult = new SearchResultModel<CustomersModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.Customers.AsNoTracking()
                             join b in db.CustomerTypes.AsNoTracking() on a.CustomerTypeId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join d in db.SBUs.AsNoTracking() on a.SBUId equals d.Id into ad
                             from adn in ad.DefaultIfEmpty()
                             select new CustomersModel
                             {
                                 Id = a.Id,
                                 CustomerTypeId = a.CustomerTypeId,
                                 CustomerTypeName = b.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Acreage = a.Acreage,
                                 EmployeeQuantity = a.EmployeeQuantity,
                                 Capital = a.Capital,
                                 Field = a.Field,
                                 SBUId = a.SBUId,
                                 SBUCode = adn != null ? adn.Code : string.Empty,
                                 SBUName = adn != null ? adn.Name : string.Empty,
                                 Type = b.Type
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(a => a.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.CustomerTypeId))
            {
                var customerType = db.CustomerTypes.AsNoTracking().FirstOrDefault(i => i.Id.Equals(modelSearch.CustomerTypeId));
                if (customerType != null)
                {
                    listParentId.Add(customerType.Id);
                }
                listParentId.AddRange(GetListParent(modelSearch.CustomerTypeId));
                dataQuery = dataQuery.Where(t => listParentId.Contains(t.CustomerTypeId));
            }

            searchResult.TotalItem = dataQuery.Count();
            if (modelSearch.PageNumber >= 1)
            {
                searchResult.ListResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }
            else
            {
                searchResult.ListResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).ToList();
            }

            foreach (var item in searchResult.ListResult)
            {
                item.ListCustomerContact = db.CustomerContacts.AsNoTracking().Where(t => t.CustomerId.Equals(item.Id)).Select(m => new CustomerContactModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Address = m.Address,
                    PhoneNumber = m.PhoneNumber,
                }).ToList();
            }

            return searchResult;
        }
        public void AddCustomer(CustomersModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            var _customer = db.Customers.ToList();
            foreach (var cus in _customer)
            {
                if (cus.Code == model.Code)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Customer);
                }
                if (cus.TaxCode == model.Tax)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TaxCode);
                }
            }


            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Customer customer = new Customer()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerTypeId = model.CustomerTypeId,
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Address = model.Address.NTSTrim(),
                        PhoneNumber = model.PhoneNumber.NTSTrim(),
                        Contact = model.Contact.NTSTrim(),
                        Note = model.Note.NTSTrim(),
                        Acreage = model.Acreage,
                        EmployeeQuantity = model.EmployeeQuantity,
                        Capital = model.Capital,
                        Field = model.Field,
                        SBUId = model.SBUId,
                        TaxCode = model.Tax,
                        EmployeeId = model.EmployeeId,
                        ProvinceId = model.ProvinceId,

                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        CodeChar = model.CodeChar
                    };

                    List<CustomerDomain> customerDomains = new List<CustomerDomain>();
                    foreach (var job in model.ListJobGroupId)
                    {
                        customerDomains.Add(new CustomerDomain()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerId = customer.Id,
                            JobId = job
                        });
                    }
                    db.CustomerDomains.AddRange(customerDomains);

                    foreach (var item in model.ListCustomerContact)
                    {
                        NTS.Model.Repositories.CustomerContact customerContact = new NTS.Model.Repositories.CustomerContact()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerId = customer.Id,
                            Name = item.Name,
                            Address = item.Address,
                            PhoneNumber = item.PhoneNumber,
                            DateOfBirth = item.DateOfBirth,
                            Email = item.Email,
                            Gender = item.Gender,
                            Note = item.Note,
                            Position = item.Position,
                            Status = item.Status
                        };

                        db.CustomerContacts.Add(customerContact);
                    }
                    foreach (var item in model.CustomerOfCustomer)
                    {
                        NTS.Model.Repositories.CustomerOfCustomer customerOfCustomer = new NTS.Model.Repositories.CustomerOfCustomer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerId = customer.Id,
                            Name = item.Name,
                            CountryName = item.CountryName,
                        };
                        db.CustomerOfCustomers.Add(customerOfCustomer);
                    }

                    db.Customers.Add(customer);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, customer.Code, customer.Id, Constants.LOG_Customer);


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
        public void UpdateCustomer(CustomersModel model, string sbuId)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);

            var _customer = db.Customers.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
            if (_customer == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Customer);
            }
            if (!_customer.SBUId.Equals(sbuId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0031, TextResourceKey.SBU);
            }
            var _customer2 = db.Customers.Where(a => a.Id != model.Id).ToList();
            foreach (var cus in _customer2)
            {
                if (cus.TaxCode == model.Tax)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.TaxCode);
                }
                if (cus.Code == model.Code)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Customer);
                }
            }



            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var customer = db.Customers.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    customer.CustomerTypeId = model.CustomerTypeId;
                    customer.Name = model.Name.NTSTrim();
                    customer.Address = model.Address.NTSTrim();
                    customer.PhoneNumber = model.PhoneNumber.NTSTrim();
                    customer.Contact = model.Contact.NTSTrim();
                    customer.Note = model.Note.NTSTrim();
                    customer.Acreage = model.Acreage;
                    customer.EmployeeQuantity = model.EmployeeQuantity;
                    customer.Capital = model.Capital;
                    customer.Field = model.Field;
                    customer.SBUId = model.SBUId;
                    customer.TaxCode = model.Tax;
                    customer.ProvinceId = model.ProvinceId;
                    customer.EmployeeId = model.EmployeeId;
                    customer.UpdateBy = model.UpdateBy;
                    customer.UpdateDate = DateTime.Now;

                    if (model.ListJobGroupId != null)
                    {
                        var listCustomerDomain = db.CustomerDomains.Where(a => a.CustomerId.Equals(model.Id)).ToList();

                        foreach (var domain in listCustomerDomain)
                        {
                            db.CustomerDomains.Remove(domain);
                        }
                        db.SaveChanges();

                        List<CustomerDomain> customerDomains = new List<CustomerDomain>();
                        foreach (var job in model.ListJobGroupId)
                        {
                            customerDomains.Add(new CustomerDomain()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CustomerId = model.Id,
                                JobId = job
                            });
                        }
                        db.CustomerDomains.AddRange(customerDomains);

                    }

                    var customerOfCustomer = db.CustomerOfCustomers.Where(a => a.CustomerId.Equals(model.Id)).ToList();
                    if (customerOfCustomer.Count > 0)
                    {
                        db.CustomerOfCustomers.RemoveRange(customerOfCustomer);
                    }
                    if (model.CustomerOfCustomer.Count > 0)
                    {
                        foreach (var item in model.CustomerOfCustomer)
                        {
                            NTS.Model.Repositories.CustomerOfCustomer customerOfCustomer1 = new NTS.Model.Repositories.CustomerOfCustomer()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CustomerId = customer.Id,
                                Name = item.Name,
                                CountryName = item.CountryName,
                            };
                            db.CustomerOfCustomers.Add(customerOfCustomer1);
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

        public void Checkdelete(string customerContactId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var customer = db.Meetings.AsNoTracking().Where(m => m.CustomerContactId.Equals(customerContactId)).FirstOrDefault();
                if (customer != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.CustomerContact);
                }

                try
                {
                    var customerContact = db.CustomerContacts.FirstOrDefault(u => u.Id.Equals(customerContactId));

                    db.CustomerContacts.Remove(customerContact);


                    var NameOrCode = customerContact.Name;


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(customerContactId, ex);
                }
            }
        }

        public void DeleteCustomer(CustomersModel model, string sbuId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var customer = db.Customers.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (customer == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Customer);
                }

                if (!sbuId.Equals(customer.SBUId))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0031, TextResourceKey.SBU);
                }

                var projects = db.Projects.AsNoTracking().FirstOrDefault(r => r.CustomerId.Equals(customer.Id));
                if (projects != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0081, TextResourceKey.Customer, TextResource.Project);
                }

                var saleProductExports = db.SaleProductExports.AsNoTracking().FirstOrDefault(r => r.CustomerId.Equals(customer.Id));
                if (saleProductExports != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0081, TextResourceKey.Customer, TextResource.SaleProductsExport);
                }

                var customerContact = db.CustomerContacts.Where(b => b.CustomerId.Equals(model.Id)).ToList();
                if (customerContact != null)
                {
                    var customerRequirement = (from a in customerContact
                                               join b in db.CustomerRequirements.AsNoTracking() on a.Id equals b.CustomerContactId
                                               where a.Id.Equals(b.CustomerContactId)
                                               select new CustomerRequirement()
                                               {
                                                   CustomerContactId = a.CustomerId
                                               }).FirstOrDefault();
                    if (customerRequirement != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Customer);
                    }
                }
                try
                {

                    var customerofcustomer = db.CustomerOfCustomers.Where(c => c.CustomerId.Equals(model.Id)).ToList();
                    db.CustomerContacts.RemoveRange(customerContact);
                    db.CustomerOfCustomers.RemoveRange(customerofcustomer);

                    db.Customers.Remove(customer);

                    var NameOrCode = customer.Code;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<CustomerHistoryModel>(customer);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Customer, customer.Id, NameOrCode, jsonBefor);
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


        public SearchResultModel<CustomerMeetingSearchResultModel> GetCustomerMeetings(CustomerMeetingSearchResultModel searchModel, string Id)
        {
            SearchResultModel<CustomerMeetingSearchResultModel> searchResult = new SearchResultModel<CustomerMeetingSearchResultModel>();

            var dataQuery = (from r in db.Meetings.AsNoTracking()
                             where r.CustomerId.Equals(Id)
                             join b in db.MeetingTypes.AsNoTracking() on r.MeetingTypeId equals b.Id into rb
                             from b in rb.DefaultIfEmpty()
                             join t in db.Users.AsNoTracking() on r.CreateBy equals t.Id into rt
                             from t in rt.DefaultIfEmpty()
                             join c in db.Employees.AsNoTracking() on t.EmployeeId equals c.Id into rc
                             from c in rc.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id into rd
                             from d in rd.DefaultIfEmpty()
                             select new CustomerMeetingSearchResultModel()
                             {
                                 Code = r.Code,
                                 Name = r.Name,
                                 MeetingTypeName = b.Name,
                                 TypeName = r.Type == 1 ? "Online" : "Trực tiếp",
                                 Status = r.Status,
                                 Request = r.Request,
                                 RequestDate = r.RequestDate,
                                 Address = r.Address,
                                 MeetingDate = r.MeetingDate,
                                 StartTime = r.StartTime,
                                 EndTime = r.EndTime,
                                 EmployeeName = c.Name,
                                 DepartmentName = d.Name
                             }).ToList();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.OrderBy(t => t.Code).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public CustomersModel GetCustomerInfo(CustomersModel model)
        {
            var resultInfo = db.Customers.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new CustomersModel
            {
                Id = p.Id,
                CustomerTypeId = p.CustomerTypeId,
                Name = p.Name,
                Code = p.Code,
                Address = p.Address,
                PhoneNumber = p.PhoneNumber,
                Contact = p.Contact,
                Acreage = p.Acreage,
                EmployeeQuantity = p.EmployeeQuantity,
                Capital = p.Capital,
                Field = p.Field,
                Note = p.Note,
                SBUId = p.SBUId,
                ProvinceId = p.ProvinceId,
                Tax = p.TaxCode,
                EmployeeId = p.EmployeeId,

            }).FirstOrDefault();

            resultInfo.ListJobGroupId = db.CustomerDomains.AsNoTracking().Where(a => a.CustomerId.Equals(model.Id)).Select(a => a.JobId).ToList();
            resultInfo.ListCustomerContact = this.GetListCustomerContacts(model.Id);

            var CustomerOfCustomer = (from a in db.CustomerOfCustomers.AsNoTracking()
                                      where a.CustomerId.Equals(model.Id)
                                      select new CustomerOfCustomerModel()
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          CountryName = a.CountryName,

                                      }).ToList();

            resultInfo.CustomerOfCustomer = CustomerOfCustomer;
            resultInfo.Meetings = (from r in db.Meetings.AsNoTracking()
                                   where r.CustomerId.Equals(model.Id)
                                   join b in db.MeetingTypes.AsNoTracking() on r.MeetingTypeId equals b.Id
                                   select new MeetingInfoModel()
                                   {
                                       Code = r.Code,
                                       Name = r.Name,
                                       MeetingTypeName = b.Name,
                                       TypeName = r.Type == 1 ? "Online" : "Trực tiếp",
                                       Status = r.Status,
                                       Request = r.Request,
                                       RequestDate = r.RequestDate,
                                       Address = r.Address,
                                       MeetingDate = r.MeetingDate,
                                       StartTime = r.StartTime,
                                       EndTime = r.EndTime,
                                   }).ToList();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Customer);
            }

            return resultInfo;
        }

        public string GetCustomerCode(string codeChar)
        {
            var customers = db.Customers.Select(s => s.Code).OrderBy(o => o).ToList();

            int index = 1;

            List<string> chars = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            while (true)
            {
                //    indexChar = index / 999;

                //    if (index % 999 == 0)
                //    {
                //        indexChar--;
                //    }

                if (index > 999)
                {
                    throw NTSException.CreateInstance("Đã hết số tạo mã!");
                }

                if (customers.IndexOf($"{codeChar}{string.Format("{0:000}", index)}") != -1)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }

            return $"{codeChar}{string.Format("{0:000}", index)}";
        }

        public string ExportExcel(CustomersModel model)
        {
            model.IsExport = true;
            SearchResultModel<CustomersModel> searchResult = new SearchResultModel<CustomersModel>();

            var dataQuery = (from a in db.Customers.AsNoTracking()
                             join b in db.CustomerTypes.AsNoTracking() on a.CustomerTypeId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.CustomerContacts.AsNoTracking() on a.Id equals c.CustomerId into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.SBUs.AsNoTracking() on a.SBUId equals d.Id into ad
                             from adn in ad.DefaultIfEmpty()
                             select new CustomersModel
                             {
                                 Id = a.Id,
                                 CustomerTypeId = a.CustomerTypeId,
                                 CustomerTypeName = b.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Acreage = a.Acreage,
                                 EmployeeQuantity = a.EmployeeQuantity,
                                 Capital = a.Capital,
                                 Field = a.Field,
                                 SBUId = a.SBUId,
                                 SBUCode = adn != null ? adn.Code : string.Empty,
                                 SBUName = adn != null ? adn.Name : string.Empty,
                                 Type = b.Type
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.CustomerTypeId))
            {
                dataQuery = dataQuery.Where(u => u.CustomerTypeId.Equals(model.CustomerTypeId));
            }
            List<CustomersModel> listRs = new List<CustomersModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.CustomerTypeName, t.Note, t.Field, t.SBUName, t.Acreage, t.EmployeeQuantity, t.Capital }).ToList();

            foreach (var item in lstRs)
            {
                CustomersModel rs = new CustomersModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.CustomerTypeName = item.Key.CustomerTypeName;
                rs.Note = item.Key.Note;
                rs.Field = item.Key.Field;
                rs.SBUName = item.Key.SBUName;
                rs.Acreage = item.Key.Acreage;
                rs.EmployeeQuantity = item.Key.EmployeeQuantity;
                rs.Capital = item.Key.Capital;
                listRs.Add(rs);
            }

            foreach (var item in listRs)
            {
                item.ListCustomerContact = db.CustomerContacts.AsNoTracking().Where(t => t.CustomerId.Equals(item.Id)).Select(m => new CustomerContactModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Address = m.Address,
                    PhoneNumber = m.PhoneNumber,
                    DateOfBirth = m.DateOfBirth,
                    Email = m.Email,
                    Gender = m.Gender,
                    Note = m.Note,
                    Position = m.Position,
                    Status = m.Status
                }).ToList();
            }

            List<CustomersModel> listModel = listRs.ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0039, TextResourceKey.Customer);
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Customer.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);


                int index = 1;

                List<CustomerContactExport> listExport = new List<CustomerContactExport>();
                foreach (var item in listRs)
                {

                    listExport.Add(new CustomerContactExport()
                    {
                        Index = index++.ToString(),
                        Code = item.Code,
                        Name = item.Name,
                        CustomerTypeName = item.CustomerTypeName,
                        Note = item.Note,
                        Field = item.Field,
                        SBU = item.SBUName,
                        Acreage = item.Acreage,
                        EmployeeQuantity = item.EmployeeQuantity,
                        Capital = item.Capital,

                    });
                    if (item.ListCustomerContact == null)
                    {
                        break;
                    }
                    else
                    {
                        foreach (var ite in item.ListCustomerContact)
                        {
                            listExport.Add(new CustomerContactExport()
                            {
                                NameContact = ite.Name,
                                PhoneNumberContact = ite.PhoneNumber,
                                AddressContact = ite.Address,
                            });
                        }
                    }
                }
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách khách hàng" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách khách hàng" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public string GetGroupInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Khách hàng_Template.xls"));
            IWorksheet sheet0 = workbook.Worksheets[0];
            IWorksheet sheet = workbook.Worksheets[1];
            IWorksheet sheetSBU = workbook.Worksheets[2];
            var listStaff = db.CustomerTypes.AsNoTracking().Select(i => i.Code).ToList();
            var total = listStaff.Count;
            sheet0.Range["D3:D1000"].DataValidation.DataRange = sheet.Range["A1:A1000"];
            IRange iRangeData = sheet.FindFirst("<customerType>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<customerType>", string.Empty);

            var listExport = listStaff.Select((o, i) => new
            {
                o,
            }); ;

            var listSbuCode = db.SBUs.AsNoTracking().Select(a => a.Code).ToList();
            sheet0.Range["E3:E1000"].DataValidation.DataRange = sheetSBU.Range["A1:A1000"];
            IRange iRangeDataSbu = sheetSBU.FindFirst("<sbucode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataSbu.Text = iRangeDataSbu.Text.Replace("<sbucode>", string.Empty);
            var listExportSbuCode = listSbuCode.Select((o, i) => new
            {
                o,
            }); ;

            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheetSBU.ImportData(listExportSbuCode, iRangeDataSbu.Row, iRangeDataSbu.Column, false);
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "KhachHang_Import_Template" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "KhachHang_Import_Template" + ".xls";

            return resultPathClient;
        }

        public void ImportFile(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string customerName, customerCode, customertypeId, customerAddress, customerContact, customerPhoneNumber, sbuCode;
            var customers = db.Customers.AsNoTracking();
            var customerTypes = db.CustomerTypes.AsNoTracking();
            CustomerType customerType;
            var listSbu = db.SBUs.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<Customer> list = new List<Customer>();
            List<Customer> listUpdate = new List<Customer>();
            Customer itemC;
            List<int> rowName = new List<int>();
            List<int> rowCode = new List<int>();
            List<int> rowContact = new List<int>();
            List<int> rowCustomertype = new List<int>();
            List<int> rowPhoneNumber = new List<int>();
            List<int> rowCheckCode = new List<int>();
            List<int> rowCheckName = new List<int>();
            List<int> rowCheckCustomerType = new List<int>();
            List<int> rowCheckSpecialCode = new List<int>();
            List<int> rowSbu = new List<int>();
            List<int> rowCheckSbu = new List<int>();
            SBU sbu;
            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new Customer();
                    itemC.Id = Guid.NewGuid().ToString();
                    customerName = sheet[i, 2].Value;
                    customerCode = sheet[i, 3].Value;
                    customertypeId = sheet[i, 4].Value;
                    customerAddress = sheet[i, 6].Value;
                    customerContact = sheet[i, 7].Value;
                    customerPhoneNumber = sheet[i, 8].Value;
                    sbuCode = sheet[i, 5].Value.NTSTrim();

                    try
                    {
                        if (!string.IsNullOrEmpty(sbuCode))
                        {
                            sbu = listSbu.Where(a => a.Code.ToUpper().Equals(sbuCode.ToUpper())).FirstOrDefault();
                            if (sbu != null)
                            {
                                itemC.SBUId = sbu.Id;
                            }
                            else
                            {
                                rowCheckSbu.Add(i);
                            }
                        }
                        else
                        {
                            rowSbu.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckSbu.Add(i);
                        continue;
                    }


                    //Code
                    try
                    {
                        if (!string.IsNullOrEmpty(customerCode))
                        {
                            itemC.Code = customerCode.Trim();
                            //if (Util.CheckSpecialCharacter(customerCode.Trim()))
                            //{
                            //    rowCheckSpecialCode.Add(i);
                            //}
                            //if (db.Customers.AsNoTracking().Where(o => o.Code.Equals(customerCode.Trim())).Count() > 0 && db.Customers.AsNoTracking().Where(o => o.Name.Equals(customerName.Trim())).Count() == 0)
                            //{
                            //    rowCheckCode.Add(i);
                            //}
                            //else
                            //{
                            //    itemC.Code = customerCode.Trim();
                            //}
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
                        if (!string.IsNullOrEmpty(customerName))
                        {
                            //if (db.Customers.AsNoTracking().Where(o => o.Name.Equals(customerName.Trim())).Count() > 0 && db.Customers.AsNoTracking().Where(o => o.Code.Equals(customerCode.Trim())).Count() == 0)
                            //{
                            //    rowCheckCode.Add(i);
                            //}
                            //else
                            //{
                            //    itemC.Name = customerName.Trim();
                            //}

                            itemC.Name = customerName.Trim();
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

                    //CustomerType
                    try
                    {
                        if (!string.IsNullOrEmpty(customertypeId))
                        {

                            customerType = customerTypes.FirstOrDefault(u => u.Code.Equals(customertypeId));

                            if (customerType != null)
                            {
                                itemC.CustomerTypeId = customerType.Id;
                            }
                            else
                            {
                                rowCheckCustomerType.Add(i);
                            }
                        }
                        else
                        {
                            rowCustomertype.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.CustomerTypeId == null)
                        {
                            rowCheckCustomerType.Add(i);
                        }
                        else
                        {
                            rowCustomertype.Add(i);
                        }
                        continue;
                    }

                    if (!string.IsNullOrEmpty(customerAddress))
                    {
                        itemC.Address = customerAddress;
                    }

                    if (!string.IsNullOrEmpty(customerContact))
                    {
                        itemC.Contact = customerContact;
                    }

                    if (!string.IsNullOrEmpty(customerPhoneNumber))
                    {
                        itemC.PhoneNumber = customerPhoneNumber;
                    }

                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    var check = db.Customers.AsNoTracking().FirstOrDefault(t => t.Code.ToUpper().Equals(itemC.Code.ToUpper()) && t.Name.ToUpper().Equals(itemC.Name.ToUpper()));
                    if (check != null)
                    {
                        //check.Code = itemC.Code;
                        //check.Name = itemC.Name;
                        //check.CustomerTypeId = itemC.CustomerTypeId;
                        //check.Address = itemC.Address;
                        //check.PhoneNumber = itemC.PhoneNumber;
                        //check.Contact = itemC.Contact;
                        //check.Note = itemC.Note;
                        //check.UpdateBy = userId;
                        //check.SBUId = itemC.SBUId;
                        //check.UpdateDate = DateTime.Now;
                        itemC.Id = check.Id;
                        listUpdate.Add(itemC);
                    }
                    else
                    {
                        list.Add(itemC);
                    }
                }

                #endregion

                if (rowCheckSpecialCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã khách hàng dòng <" + string.Join(", ", rowCheckSpecialCode) + "> chứa ký tự đặc biệt!");
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

                if (rowCheckCustomerType.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại khách hàng dòng <" + string.Join(", ", rowCheckCustomerType) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCode.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã khách hàng dòng <" + string.Join(", ", rowCode) + "> không được phép để trống!");
                }

                if (rowName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên khách hàng dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCustomertype.Count > 0)
                {
                    throw NTSException.CreateInstance("Loại khách hàng dòng <" + string.Join(", ", rowCustomertype) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowContact.Count > 0)
                {
                    throw NTSException.CreateInstance("Người liên hệ dòng <" + string.Join(", ", rowContact) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowPhoneNumber.Count > 0)
                {
                    throw NTSException.CreateInstance("Số điện thoại dòng <" + string.Join(", ", rowPhoneNumber) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckSbu.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã SBU dòng <" + string.Join(", ", rowCheckSbu) + "> không đúng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowSbu.Count > 0)
                {
                    throw NTSException.CreateInstance("Mã SBU dòng <" + string.Join(", ", rowSbu) + "> không được phép để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                db.Customers.AddRange(list);

                Customer customerUpdate;
                foreach (var item in listUpdate)
                {
                    customerUpdate = db.Customers.FirstOrDefault(t => t.Id.Equals(item.Id));
                    if (customerUpdate != null)
                    {
                        customerUpdate.Code = item.Code;
                        customerUpdate.Name = item.Name;
                        customerUpdate.CustomerTypeId = item.CustomerTypeId;
                        customerUpdate.Address = item.Address;
                        customerUpdate.PhoneNumber = item.PhoneNumber;
                        customerUpdate.Contact = item.Contact;
                        customerUpdate.Note = item.Note;
                        customerUpdate.Acreage = item.Acreage;
                        customerUpdate.EmployeeQuantity = item.EmployeeQuantity;
                        customerUpdate.Capital = item.Capital;
                        customerUpdate.Field = item.Field;
                        customerUpdate.UpdateBy = userId;
                        customerUpdate.SBUId = item.SBUId;
                        customerUpdate.UpdateDate = DateTime.Now;
                    }
                }
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
        public void CheckDeleteCustomerContact(string customerId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var customerContact = db.CustomerContacts.FirstOrDefault(a => a.Id.Equals(customerId));
                if (customerContact == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerContact);
                }

                var customer = db.MeetingCustomerContacts.AsNoTracking().Where(m => m.CustomerContactId.Equals(customerId)).ToList();
                if (customer.Count > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.CustomerContact);
                }

                var customers = db.CustomerRequirements.AsNoTracking().Where(m => m.CustomerContactId.Equals(customerId)).ToList();
                if (customers.Count > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.CustomerContact);
                }
                try
                {

                    db.CustomerContacts.Remove(customerContact);
                    db.SaveChanges();
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }
        }
        private void CheckExistedForAdd(CustomersModel model)
        {
            //if (db.Customers.AsNoTracking().Where(o => o.PhoneNumber.Equals(model.PhoneNumber)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG00014, TextResourceKey.Customer);
            //}

            if (db.Customers.AsNoTracking().Where(o => o.Code.ToUpper().Equals(model.Code.ToUpper())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Customer);
            }
        }
        public void CheckExistedForUpdate(CustomersModel model)
        {
            //if (db.Customers.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.PhoneNumber.Equals(model.PhoneNumber)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG00014, TextResourceKey.Customer);
            //}

            if (db.Customers.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.ToUpper().Equals(model.Code.ToUpper())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Customer);
            }
        }

        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var customerType = db.CustomerTypes.AsNoTracking().Where(i => i.ParentId.Equals(id)).Select(i => i.Id).ToList();
            listChild.AddRange(customerType);
            if (customerType.Count > 0)
            {
                foreach (var item in customerType)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }

        public CustomerContactModel GetCustomerContact(string id)
        {
            var customerContact = db.CustomerContacts.AsNoTracking().Where(i => i.Id.Equals(id)).Select(i => new CustomerContactModel()
            {
                Id = i.Id,
                PhoneNumber = i.PhoneNumber,
                Email = i.Email,
                Address = i.Address,
                Avatar = i.Avatar,
            }).FirstOrDefault();

            return customerContact;
        }

        public List<CustomerContactModel> GetListCustomerContacts(string customerId)
        {
            List<CustomerContactModel> listResult = new List<CustomerContactModel>();
            listResult = (from a in db.CustomerContacts.AsNoTracking()
                              //join c in db.Employees.AsNoTracking() on a.EmployeeId equals c.Id
                          where a.CustomerId.Equals(customerId)
                          select new CustomerContactModel()
                          {
                              Id = a.Id,
                              Name = a.Name,
                              Address = a.Address,
                              PhoneNumber = a.PhoneNumber,
                              Status = a.Status,
                              Position = a.Position,
                              Note = a.Note,
                              Gender = a.Gender,
                              Email = a.Email,
                              DateOfBirth = a.DateOfBirth,
                              Avatar = a.Avatar,
                              //EmployeeName = c.Name,
                              //EmployeeCode = c.Code,
                          }).ToList();
            return listResult;
        }
        public SearchResultProjectModel<ProjectResultModel> SearchCustomerProject(ProjectSearchModel modelSearch, string Id)
        {
            SearchResultProjectModel<ProjectResultModel> searchResultObject = new SearchResultProjectModel<ProjectResultModel>();

            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             where b.Id.Equals(Id)
                             orderby a.Code
                             select new ProjectResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 DateFrom = a.DateFrom,
                                 SBUName = c.Name,
                                 DepartmentName = d.Name,
                                 DateTo = a.DateTo,
                                 Status = a.Status,
                                 SaleNoVAT = a.SaleNoVAT,
                                 KickOffDate = a.KickOffDate,
                                 CreateDate = a.CreateDate,
                                 FCMPrice = a.FCMPrice,
                                 Type = a.Type,
                             }).AsQueryable();

            searchResultObject.TotalItem = dataQuery.Count();

            var listResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResultObject.ListResult = listResult;


            return searchResultObject;

        }


        public SearchResultModel<CustomerRequirementSearchResultModel> GetListCustomerRequirement(CustomerRequirementSearchResultModel searchModel, string Id)
        {
            SearchResultModel<CustomerRequirementSearchResultModel> searchResult = new SearchResultModel<CustomerRequirementSearchResultModel>();

            var dataQuery = (from a in db.CustomerRequirements.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                             join c in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals c.Id
                             join d in db.Employees.AsNoTracking() on a.Petitioner equals d.Id into ad
                             from adx in ad.DefaultIfEmpty()
                             join e in db.Employees.AsNoTracking() on a.Reciever equals e.Id into ae
                             from aex in ae.DefaultIfEmpty()
                             join f in db.Departments.AsNoTracking() on a.DepartmentRequest equals f.Id into af
                             from afx in af.DefaultIfEmpty()
                             join g in db.Departments.AsNoTracking() on a.DepartmentReceive equals g.Id into ag
                             from agx in ag.DefaultIfEmpty()
                             join h in db.ProjectPhases.AsNoTracking() on a.ProjectPhaseId equals h.Id into ah
                             from ahx in ah.DefaultIfEmpty()
                             where b.Id.Equals(Id)
                             select new CustomerRequirementSearchResultModel
                             {
                                 Id = a.Id,
                                 CustomerId = b.Name,
                                 CustomerContactId = c.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Status = a.Status,
                                 Step = a.Step,
                                 Version = a.Version,
                                 Budget = a.Budget,
                                 RequestType = a.RequestType,
                                 Petitioner = a.Petitioner,
                                 DepartmentRequest = a.DepartmentRequest,
                                 DepartmentRequestName = afx != null ? afx.Name : string.Empty,
                                 DepartmentReceiveName = agx != null ? agx.Name : string.Empty,
                                 PetitionerName = adx != null ? adx.Name : string.Empty,
                                 RecieverName = aex != null ? aex.Name : string.Empty,
                                 Reciever = a.Reciever,
                                 DepartmentReceive = a.DepartmentReceive,
                                 RequestSource = a.RequestSource,
                                 RealFinishDate = a.RealFinishDate,
                                 ProjectPhaseId = ahx != null ? ahx.Name : string.Empty,
                                 Competitor = a.Competitor,
                                 CustomerSupplier = a.CustomerSupplier,
                                 PriorityLevel = a.PriorityLevel,

                                 CustomerRequirementState = a.CustomerRequirementState,
                                 CustomerRequirementAnalysisState = a.CustomerRequirementAnalysisState,
                                 SurveyState = a.SurveyState,
                                 SolutionAnalysisState = a.SolutionAnalysisState,
                                 EstimateState = a.EstimateState,
                                 DoSolutionAnalysisState = a.DoSolutionAnalysisState,

                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.OrderBy(t => t.Code).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            foreach (var item in searchResult.ListResult)
            {
                item.ListSurvey = db.Surveys.AsNoTracking().Where(t => t.CustomerRequirementId.Equals(item.Id)).Select(m => new SurveyCreateModel
                {
                    Id = m.Id,
                    CustomerRequirementId = m.CustomerRequirementId,
                    //ProjectPhaseId = m.ProjectPhaseId,
                    SurveyDate = m.SurveyDate,
                    Times = m.Times,
                }).ToList();


                foreach (var user in item.ListSurvey)
                {
                    if (!string.IsNullOrEmpty(user.Id))
                    {
                        var listServeyUser = db.SurveyUsers.Where(t => t.Id.Equals(user.ListUser)).Select(t => t.UserId).ToList();

                        if (listServeyUser.Count > 0)
                        {
                            dataQuery = dataQuery.Where(t => listServeyUser.Contains(user.Id));
                        }
                    }
                    user.Time = JsonConvert.DeserializeObject<object>(user.Times);
                }

                foreach (var tool in item.ListSurvey)
                {
                    if (!string.IsNullOrEmpty(tool.Id))
                    {
                        var listServeyTools = db.SurveyTools.Where(t => t.Id.Equals(tool.ListMaterial)).Select(t => t.MaterialId).ToList();

                        if (listServeyTools.Count > 0)
                        {
                            dataQuery = dataQuery.Where(t => listServeyTools.Contains(tool.Id));
                        }
                    }
                }

            }


            return searchResult;
        }

    }
}
