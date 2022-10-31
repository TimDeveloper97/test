using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.EmployeeSkillDetails;
using NTS.Model.EmployeeSkillsDetails;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.EmployeeSkillDetails
{
    public class EmployeeSkillDetailsBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<EmployeeSkillDetailsModel> SearchEmployeeSkillDetails(EmployeeSkillDetailsSeaechModel model)
        {
            SearchResultModel<EmployeeSkillDetailsModel> searchResult = new SearchResultModel<EmployeeSkillDetailsModel>();
            var dataQuery = (from a in db.EmployeeSkillDetails.AsNoTracking()
                             join b in db.Skills.AsNoTracking() on a.SkillId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.EmployeeId equals c.Id
                             join d in db.SkillGroups.AsNoTracking() on b.SkillGroupId equals d.Id
                             where a.EmployeeId.Equals(model.EmployeeId)
                             orderby a.RateDate
                             select new EmployeeSkillDetailsModel
                             {
                                 Id = a.Id,
                                 EmployeeId = a.EmployeeId,
                                 SkillId = a.SkillId,
                                 SkillGroupId = d.Id,
                                 Code = b.Code,
                                 Name = b.Name,
                                 LevelMax = 10,
                                 LevelRate = a.LevelRate.ToString(),
                                 RateDate = a.RateDate,
                                 LevelRate1 = a.LevelRate1.ToString(),
                                 RateDate1 = a.RateDate1,
                                 LevelRate2 = a.LevelRate2.ToString(),
                                 RateDate2 = a.RateDate2,
                                 Note = a.Note
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void UpdateEmployeeSkillDetails(EmployeeSkillDetailsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newEmployeeSkillDetails = db.EmployeeSkillDetails.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newEmployeeSkillDetails.EmployeeId = newEmployeeSkillDetails.EmployeeId;
                    newEmployeeSkillDetails.SkillId = newEmployeeSkillDetails.SkillId;
                    newEmployeeSkillDetails.LevelRate2 = newEmployeeSkillDetails.LevelRate1;
                    newEmployeeSkillDetails.RateDate2 = newEmployeeSkillDetails.RateDate1;
                    newEmployeeSkillDetails.LevelRate1 = newEmployeeSkillDetails.LevelRate;
                    newEmployeeSkillDetails.RateDate1 = newEmployeeSkillDetails.RateDate;
                    newEmployeeSkillDetails.LevelRate = int.Parse(model.LevelRate);
                    newEmployeeSkillDetails.RateDate = model.RateDate;
                    newEmployeeSkillDetails.UpdateBy = model.UpdateBy;
                    newEmployeeSkillDetails.UpdateDate = DateTime.Now;
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
    }
}
