using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.QLTKEP;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Utils;
using SQLHelpper = NTS.Model.SQLHelpper;
using System.Web;

namespace QLTK.Business.QLTKEP
{
    public class QLTKEPBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public SearchResultObject<QLTKEPResultModel> SearchEmployee(QLTKEPSearchModel modelSearch)
        {
            SearchResultObject<QLTKEPResultModel> searchResult = new SearchResultObject<QLTKEPResultModel>();
            try
            {
                //var emp = db.Employees.FirstOrDefault();
                //List<Employee> lst = new List<Employee>();
                //Employee item;
                //for (int i = 0; i < 800; i++)
                //{
                //    item = new Employee();
                //    item.Id = Guid.NewGuid().ToString();
                //    item.QualificationId = emp.QualificationId;
                //    item.JobPositionId = emp.JobPositionId;
                //    item.DepartmentId = emp.DepartmentId;
                //    item.Code = "nv5000"+i;
                //    item.Name = "Vũ Hoan"+i;
                //    item.DateOfBirth = emp.DateOfBirth;
                //    item.PhoneNumber = emp.PhoneNumber;
                //    item.Email = emp.Email;
                //    item.ImagePath = emp.ImagePath;
                //    item.Address = emp.Address;
                //    item.Gender = emp.Gender;
                //    item.BankAccount = emp.BankAccount;
                //    item.SocialInsurrance = emp.SocialInsurrance;
                //    item.StartWorking = emp.StartWorking;
                //    item.EndWorking = emp.EndWorking;
                //    item.Status = emp.Status;
                //    item.CreateBy = emp.CreateBy;
                //    item.CreateDate = emp.CreateDate;
                //    item.UpdateBy = emp.UpdateBy;
                //    item.UpdateDate = emp.UpdateDate;
                //    lst.Add(item);
                //}
                //db.Employees.AddRange(lst);
                //db.SaveChanges();
                //throw new Exception("Xong 500");
                //var emp = db.Employees.ToList();
                //for (int i = 0; i < emp.Count; i++)
                //{
                //    emp[i].Code = "nv0"+i;
                //    emp[i].Name = emp[i].Name + i;
                //}
                //db.SaveChanges();
                var dataQuery = (from a in db.Employees.AsNoTracking()
                                 join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId into ab from ab1 in ab.DefaultIfEmpty()
                                 join c in db.JobPositions.AsNoTracking() on a.JobPositionId equals c.Id
                                 orderby a.Name
                                 select new QLTKEPResultModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     PhoneNumber = a.PhoneNumber,
                                     Address = a.Address,
                                     DateOfBirth = a.DateOfBirth,
                                     UserName =ab1!=null? ab1.UserName:"",
                                     JobPositionName = c.Name,
                                     JobPositionId = a.JobPositionId,
                                     Email = a.Email,
                                     ImagePath = a.ImagePath,
                                     Gender = a.Gender,
                                     IdentifyNum = a.IdentifyNum,
                                     StartWorking = a.StartWorking,
                                     EndWorking = a.EndWorking,
                                 }).AsQueryable();

                //checks conditions
                //if (!string.IsNullOrEmpty(modelSearch.Name))
                //{
                //    dataQuery = dataQuery.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                //}
                //if (!string.IsNullOrEmpty(modelSearch.Code))
                //{
                //    dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(modelSearch.Code));
                //}
                //if (!string.IsNullOrEmpty(modelSearch.PhoneNumber))
                //{
                //    dataQuery = dataQuery.Where(r => r.PhoneNumber.ToUpper().Contains(modelSearch.PhoneNumber));
                //}
                //if (!string.IsNullOrEmpty(modelSearch.JobPositionId))
                //{
                //    dataQuery = dataQuery.Where(r => r.JobPositionId.Equals(modelSearch.JobPositionId));
                //}
                //if (modelSearch.Gender != null)
                //{
                //    dataQuery = dataQuery.Where(r => r.Gender == modelSearch.Gender);
                //}
                //searchResult.TotalItem = dataQuery.Count();
                searchResult.ListResult = dataQuery.ToList();
                searchResult.TotalItem = searchResult.ListResult.Count;
                //throw new Exception("EMP.List");
            }
            catch (Exception ex)
            {
                throw new Exception("QLTK.List");
            }
            return searchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void DeleteEmployee(QLTKEPModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var userEmployees = db.Employees.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (userEmployees == null)
                {
                    throw new Exception("Nhân viên này đã bị xóa bởi người dùng khác");
                }
                try
                {
                    model.Name = userEmployees.Name;
                    var userModel = db.Users.FirstOrDefault(u => u.Employee.Equals(userEmployees.Id));
                    if (userModel != null)
                    {
                        db.Users.Remove(userModel);
                    }
                    db.Employees.Remove(userEmployees);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
                }
            }
            //luu Log lich su
            string decription = "Xóa nhân viên tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        public void AddEmployee(QLTKEPModel model, HttpFileCollection hfc)
        {

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateEmployee(QLTKEPModel model, HttpFileCollection hfc)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public QLTKEPModel GetEmployeeInfo(QLTKEPModel model)
        {
            var userModel = db.Users.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (userModel == null)
            {
                throw new Exception("Nhân viên này đã bị xóa bởi người dùng khác");
            }
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
            }
            return null;
        }


    }
}
