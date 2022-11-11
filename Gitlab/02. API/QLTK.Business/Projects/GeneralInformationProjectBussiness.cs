using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.GeneralInfomationProject;
using NTS.Model.Project;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.GeneralInformationProject
{
    public class GeneralInformationProjectBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ProjectResultModel> GetListGeneralInformationProject(GeneralInfomationProjectSearchModel model)
        {
            SearchResultModel<ProjectResultModel> searchResult = new SearchResultModel<ProjectResultModel>();
            List<ProjectResultModel> listGeneralInformationProject = new List<ProjectResultModel>();
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             orderby a.Name
                             select new ProjectResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Status = a.Status,
                                 KickOffDate = a.KickOffDate,
                                 DateFrom = a.DateFrom,
                                 DateTo = a.DateTo,
                                 SBUId = a.SBUId,
                                 DepartmentId = a.DepartmentId
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.SBUId))
            {
                dataQuery = dataQuery.Where(a => a.SBUId.Equals(model.SBUId));
            }
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuery = dataQuery.Where(a => a.DepartmentId.Equals(model.DepartmentId));
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(a => a.Name.Contains(model.Code) || a.Code.Contains(model.Code));
            }
            if (model.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.DateFrom != null ? a.DateFrom >= model.DateFrom : a.KickOffDate >= model.DateFrom);
            }
            if (model.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(a => a.DateTo != null ? a.DateTo <= model.DateTo : a.KickOffDate <= model.DateTo);
            }

            listGeneralInformationProject = dataQuery.ToList();
            foreach (var item in listGeneralInformationProject)
            {
                var list = db.ProjectProducts.AsNoTracking().Where(i => item.Id.Equals(i.ProjectId)).ToList();
                if (list.Count > 0)
                {
                    var listHasValue = list.Where(e => e.ExpectedDesignFinishDate.HasValue).ToList();
                    if (item.KickOffDate.HasValue)
                    {
                        TimeSpan timeSpan = new TimeSpan();
                        if (listHasValue.Count > 0)
                        {
                            item.DateMax = listHasValue.Max(e => e.ExpectedDesignFinishDate.Value);
                            timeSpan = item.DateMax.Subtract(item.KickOffDate.Value);
                            //Thời gian cần thực hiện
                            item.TotalTime = timeSpan.Days * 8;
                        }
                        else
                        {
                            item.TotalTime = 0;
                        }

                        //Lấy số người trong phòng
                        var peoples = (from a in db.Projects.AsNoTracking()
                                       join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                       join c in db.Employees.AsNoTracking() on b.Id equals c.DepartmentId
                                       where a.Id.Equals(item.Id)
                                       select c.DepartmentId).Count();

                        //Lấy tổng số giờ đã lên kế hoạch
                        decimal hour = 0;
                        //if (db.Plans.AsNoTracking().Where(i => i.ProjectId.Equals(item.Id)).ToList().Count > 0)
                        //{
                        //    hour = (from a in db.Plans.AsNoTracking()
                        //            where item.Id.Equals(a.ProjectId)
                        //            select a.EsimateTime).Sum();
                        //}

                        //Thời gian có thể thực hiện
                        item.TakeTime = (item.TotalTime * peoples) - hour;

                        //Cân đối
                        item.Symmetrical = item.TotalTime - item.TakeTime;
                    }
                    else
                    {
                        item.TotalTime = 0;
                        item.TakeTime = 0;
                        item.Symmetrical = 0;
                    }
                }
                else
                {
                    item.TotalTime = 0;
                    item.TakeTime = 0;
                    item.Symmetrical = 0;
                }
            }

            searchResult.Status1 = listGeneralInformationProject.Where(i => !i.Status.Equals(Constants.Prooject_Status_NotStartedYet) && !i.Status.Equals(Constants.Prooject_Status_Finish)).Count();
            searchResult.Status2 = listGeneralInformationProject.Where(i => i.Status.Equals(Constants.Prooject_Status_Finish)).Count();
            searchResult.ListResult = listGeneralInformationProject;
            return searchResult;
        }
    }
}
