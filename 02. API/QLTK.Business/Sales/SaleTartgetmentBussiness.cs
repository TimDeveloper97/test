using Microsoft.VisualBasic.ApplicationServices;
using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Employees;
using NTS.Model.Meeting;
using NTS.Model.Repositories;
using NTS.Model.Sale.SaleProduct;
using NTS.Model.Sale.SaleTartgetment;
using NTS.Model.ScheduleProject;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Sales
{
    public class SaleTartgetmentBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public void CreateUpdateSaleTartgetment(SaleTartgetmentModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var employee = (from a in db.Users.AsNoTracking()
                                    join e in db.Employees.AsNoTracking() on a.EmployeeId equals e.Id
                                    where model.UpdateBy.Equals(a.Id)
                                    select new NTS.Model.Employees.EmployeeModel
                                    {
                                        Id = e.Id,
                                        DepartmentId = e.DepartmentId
                                    }).FirstOrDefault();
                    if (string.IsNullOrEmpty(model.Id))
                    {
                        SaleTargetment target = new SaleTargetment();
                        target.Id = Guid.NewGuid().ToString();
                        target.CustomerCode = model.CustomerCode;
                        target.CustomerName = model.CustomerName;
                        if (!string.IsNullOrEmpty(model.CustomerId))
                        {
                            target.CustomerId = model.CustomerId;
                        }
                        target.DomainId = model.DomainId;
                        target.IndustryId = model.IndustryId;
                        target.ApplicationTypeId = model.ApplicationId;
                        target.SaleTarget = model.SaleTarget;
                        target.PlanContractDate = model.PlanContractDate;
                        target.Year = model.PlanContractDate.Year;
                        target.EmployeeId = employee.Id;
                        target.DepartmentId = employee.DepartmentId;
                        target.CreateBy = model.UpdateBy;
                        target.UpdateBy = model.UpdateBy;
                        target.CreateDate =DateTime.Now;
                        target.UpdateDate = DateTime.Now;
                        db.SaleTargetments.Add(target);
                    }
                    else
                    {
                        var target = db.SaleTargetments.FirstOrDefault(a => a.Id.Equals(model.Id));
                        if (!string.IsNullOrEmpty(model.CustomerId))
                        {
                            target.CustomerId = model.CustomerId;
                        }
                        else
                        {
                            target.CustomerId = null;
                        }
                        target.CustomerCode = model.CustomerCode;
                        target.CustomerName = model.CustomerName;
                        target.DomainId = model.DomainId;
                        target.IndustryId = model.IndustryId;
                        target.ApplicationTypeId = model.ApplicationId;
                        target.SaleTarget = model.SaleTarget;
                        target.PlanContractDate = model.PlanContractDate;
                        target.Year = model.PlanContractDate.Year;
                        target.EmployeeId = employee.Id;
                        target.DepartmentId = employee.DepartmentId;
                        target.UpdateBy = model.UpdateBy;
                        target.UpdateDate = DateTime.Now;
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

        public void DeleteSaleTartgetment(SaleTartgetmentModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var target = db.SaleTargetments.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (target == null)
                    {
                        throw new Exception("Không tìm được đăng ký!");
                    }
                    db.SaleTargetments.Remove(target);
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

        public SearchResultModel<SaleTartgetmentModel> GetAllSaleTartgetments(SaleTartgetmentModel model)
        {
            SearchResultModel<SaleTartgetmentModel> searchResultModel = new SearchResultModel<SaleTartgetmentModel>();
            List<SaleTartgetmentModel> listResult = new List<SaleTartgetmentModel>();

            var employee = (from a in db.Users.AsNoTracking()
                            join e in db.Employees.AsNoTracking() on a.EmployeeId equals e.Id
                            where model.UpdateBy.Equals(a.Id)
                            select new NTS.Model.Employees.EmployeeModel
                            {
                                Id = e.Id,
                                DepartmentId = e.DepartmentId,
                                JobPositionId = e.JobPositionId,
                            }).FirstOrDefault();

            var dataQuery = (from a in db.SaleTargetments.AsNoTracking()
                             join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                             select new SaleTartgetmentModel()
                             {
                                 Id = a.Id,
                                 EmployeeCode = b.Code,
                                 EmployeeName = b.Name,
                                 DepartmentId = b.DepartmentId,
                                 CustomerCode = a.CustomerCode,
                                 CustomerName = a.CustomerName,
                                 Year = a.Year,
                                 SaleTarget = a.SaleTarget,
                                 DomainId = a.DomainId,
                                 IndustryId = a.IndustryId,
                                 ApplicationId = a.ApplicationTypeId,
                                 PlanContractDate = (DateTime)a.PlanContractDate,
                                 CreateDate = a.CreateDate,
                                 CustomerId = a.CustomerId,
                                 EmployeeId = a.EmployeeId,
                             } ).AsQueryable();
            //lấy ra vị trí trưởng
            var jobPositions = db.JobPositions.AsNoTracking().Where(a => a.Code.Equals("G05")).ToList();
            //check nếu nhân viên là trưởng nhóm thì
            if( jobPositions.FirstOrDefault(a =>a.Id.Equals(employee.JobPositionId)) != null)
            {
                dataQuery = dataQuery.Where(a => a.EmployeeId.Equals(employee.Id));
            }
            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                dataQuery = dataQuery.Where(a => a.CustomerName.Contains(model.CustomerName));
            }
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuery = dataQuery.Where(a => a.DepartmentId.Equals(model.DepartmentId));
            }
            if (model.DateFrom != null)
            {
                dataQuery = dataQuery.Where(a => a.PlanContractDate >= model.DateFrom);
            }
            if (model.DateTo != null)
            {
                dataQuery = dataQuery.Where(a => a.PlanContractDate <= model.DateTo);
            }
            listResult = dataQuery.OrderByDescending(t => t.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            var departments = db.Departments.AsNoTracking().ToList();
            var domains = db.Jobs.AsNoTracking().ToList();
            var applications = db.Applications.AsNoTracking().ToList();
            var industries = db.Industries.AsNoTracking().ToList();

            foreach (var item in listResult)
            {
                item.DepartmentName = departments.FirstOrDefault(a => a.Id.Equals(item.DepartmentId)).Name;
                item.DomainName = domains.FirstOrDefault(a => a.Id.Equals(item.DomainId)).Name;
                item.ApplicationName = applications.FirstOrDefault(a => a.Id.Equals(item.ApplicationId)).Name;
                item.IndustryName = industries.FirstOrDefault(a => a.Id.Equals(item.IndustryId)).Name;
            }
            searchResultModel.ListResult = listResult;
            searchResultModel.TotalItem = dataQuery.Count();
            return searchResultModel;
        }

        public object GetSaleTartgetmentInfo(SaleTartgetmentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
