using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.CustomerContact;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Projects
{
    public class CustomerContactBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<CustomerContactModel> SearchCustomerContact(CustomerContactSearchModel modelSearch)
        {
            SearchResultModel<CustomerContactModel> searchResult = new SearchResultModel<CustomerContactModel>();

            var dataQuery = (from a in db.CustomerContacts.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                             where !modelSearch.ListCustomerContactId.Contains(a.Id)
                             select new CustomerContactModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 CustomerId = a.CustomerId,
                                 CustomerName = b.Name,
                                 CustomerCode = b.Code,
                                 Address = a.Address,
                                 PhoneNumber = a.PhoneNumber,
                                 Email = a.Email,
                                 Position = a.Position,
                                 Avatar = a.Avatar
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.CustomerId))
            {
                dataQuery = dataQuery.Where(t => t.CustomerId.Equals(modelSearch.CustomerId));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.OrderBy(t => t.CustomerName).ToList();
            return searchResult;
        }

        /// <summary>
        /// Thực hiện add bản ghi Người liên hệ của khách hàng vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="NTSLogException"></exception>
        public void Create(CustomerContactModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        CustomerContact customerContact = new CustomerContact()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerId = model.CustomerId,
                            Address = model.Address == null ? "" : model.Address,
                            Name = model.Name,
                            PhoneNumber = model.PhoneNumber,
                            DateOfBirth = model.DateOfBirth,
                            Email = model.Email,
                            Gender = model.Gender == null ? "" : model.Gender,
                            Note = model.Note,
                            Position = model.Position,
                            Status = model.Status,
                            Avatar = model.Avatar,
                        };
                        db.CustomerContacts.Add(customerContact);

                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new NTSLogException(model.Name, ex);
                    }
                }
            }
            else
            {
                var customerContact = db.CustomerContacts.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                customerContact.CustomerId = model.CustomerId;
                customerContact.Address = model.Address;
                customerContact.Name = model.Name;
                customerContact.PhoneNumber = model.PhoneNumber;
                customerContact.DateOfBirth = model.DateOfBirth;
                customerContact.Email = model.Email;
                customerContact.Gender = model.Gender;
                customerContact.Note = model.Note;
                customerContact.Position = model.Position;
                customerContact.Status = model.Status;
                customerContact.Avatar = model.Avatar;

                db.SaveChanges();

            }
        }
    }
}
