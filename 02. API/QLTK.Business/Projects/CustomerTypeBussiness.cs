using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.CustomerType;
using NTS.Model.CustomerTypeHistory;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Linq;

namespace QLTK.Business.CustomerTypes
{
    public class CustomerTypeBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<CustomerTypeModel> SearchCustomerType(CustomerTypeSearchModel modelSearch)
        {
            SearchResultModel<CustomerTypeModel> searchResult = new SearchResultModel<CustomerTypeModel>();

            var dataQuery = (from a in db.CustomerTypes.AsNoTracking()
                             select new CustomerTypeModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public CustomerTypeModel AddCustomerType(CustomerTypeModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                string id;
                if (model.ParentId == "")
                {
                    model.ParentId = null;
                }
                try
                {
                    CustomerType customerType = new CustomerType
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        ParentId = !string.IsNullOrEmpty(model.ParentId) ? model.ParentId.Trim() : null,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };

                    var customerTypeP = db.CustomerTypes.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ParentId));

                    if (customerTypeP != null)
                    {
                        customerType.Type = customerTypeP.Type;
                    }

                    db.CustomerTypes.Add(customerType);
                    id = customerType.Id;

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, customerType.Code, customerType.Id, Constants.LOG_CustomerType);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
                var resultInfo = db.CustomerTypes.Where(u => u.Id.Equals(id)).Select(p => new CustomerTypeModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                }).FirstOrDefault();

                return resultInfo;
            }
        }
        public void UpdateCustomerType(CustomerTypeModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var customerType = db.CustomerTypes.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<CustomerTypeHistoryModel>(customerType);

                    customerType.Name = model.Name.Trim();
                    customerType.Code = model.Code.Trim();
                    customerType.ParentId = !string.IsNullOrEmpty(model.ParentId) ? model.ParentId.Trim() : null;
                    customerType.Description = model.Description.Trim();
                    customerType.UpdateBy = model.UpdateBy;
                    customerType.UpdateDate = DateTime.Now;

                    var customerTypeP = db.CustomerTypes.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ParentId));

                    if (customerTypeP != null)
                    {
                        customerType.Type = customerTypeP.Type;
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<CustomerTypeHistoryModel>(customerType);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_CustomerType, customerType.Id, customerType.Code, jsonBefor, jsonApter);
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

        public void DeleteCustomerType(CustomerTypeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var customer = db.Customers.AsNoTracking().Where(m => m.CustomerTypeId.Equals(model.Id)).FirstOrDefault();
                if (customer != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.CustomerType);
                }

                try
                {
                    var customerType = db.CustomerTypes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (customerType == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerType);
                    }

                    db.CustomerTypes.Remove(customerType);


                    var NameOrCode = customerType.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<CustomerTypeHistoryModel>(customerType);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_CustomerType, customerType.Id, NameOrCode, jsonApter);

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
        public object GetCustomerTypeInfo(CustomerTypeModel model)
        {
            var resultInfo = db.CustomerTypes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new CustomerTypeModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                ParentId = p.ParentId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerType);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(CustomerTypeModel model)
        {
            if (db.CustomerTypes.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.CustomerType);
            }

            if (db.CustomerTypes.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.CustomerType);
            }
        }

        public void CheckExistedForUpdate(CustomerTypeModel model)
        {
            if (db.CustomerTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.CustomerType);
            }

            if (db.CustomerTypes.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.CustomerType);
            }
        }
    }
}
