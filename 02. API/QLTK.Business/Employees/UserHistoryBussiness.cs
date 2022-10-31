using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.UserHistoryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.UserHistoryManage
{
    public class UserHistoryBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<UserHistoryModel> SearchUserHistory(UserHistorySearchModel searchModel)
        {
            SearchResultModel<UserHistoryModel> result = new SearchResultModel<UserHistoryModel>();

            var dataQuery = (from a in db.Employees.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                             join d in db.Users.AsNoTracking() on a.Id equals d.EmployeeId
                             join e in db.UserHistories.AsNoTracking() on d.Id equals e.UserId
                             orderby a.Name
                             select new UserHistoryModel
                             {
                                 SBUId = c.Id,
                                 DepartmentId = b.Id,
                                 Date = e.CreateDate,
                                 EmployeeName = a.Name,
                                 ObjectId = e.ObjectId,
                                 Content = e.Content,
                                 EmployeeId = a.Id,
                                 SBUName = c.Name,
                                 DepartmentName = b.Name
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                dataQuery = dataQuery.Where(a => a.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(a => a.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeName))
            {
                dataQuery = dataQuery.Where(a => a.EmployeeName.ToLower().Contains(searchModel.EmployeeName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.Content))
            {
                dataQuery = dataQuery.Where(a => a.Content.ToLower().Contains(searchModel.Content.ToLower()));
            }

            if (searchModel.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.Date >= searchModel.DateFrom);
            }

            if (searchModel.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.Date <= searchModel.DateTo);
            }

            result.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            result.ListResult = listResult;

            return result;
        }
    }
}
