using NTS.Common;
using NTS.Common.Helpers;
using NTS.Model.Combobox;
using NTS.Model.Cost;
using NTS.Model.CostWarning;
using NTS.Model.Project;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.CostWarning
{
    public class CostWarningBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public object SearchCost(CostModel modelSearch)
        {
            int[] listMonth = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };

            List<CostModel> ListResult = new List<CostModel>();
            List<CostModel> ListOder = new List<CostModel>();
            List<CostModel> list = new List<CostModel>();
            decimal totalEstimatedCost = 0;
            decimal totalRealCost = 0;
            decimal totalStatusCost = 0;
            decimal totalNextMonth = 0;
            var check = db.Costs.AsNoTracking().FirstOrDefault(i => i.Year == modelSearch.Year);
            if (check != null)
            {
                var dataQuery = (from a in db.Costs.AsNoTracking()
                                 where a.Year == modelSearch.Year
                                 orderby a.Month
                                 select new CostModel
                                 {
                                     Id = a.Id,
                                     Month = a.Month,
                                     Year = a.Year,
                                     EstimatedCost = a.EstimatedCost,
                                     CheckEstimatedCost = a.EstimatedCost,
                                     RealCost = a.RealCost,
                                     CheckRealCost = a.RealCost,
                                     StatusCost = a.EstimatedCost - a.RealCost
                                 }).ToList();

                ListOder = dataQuery.ToList();
                List<CostModel> listResult = new List<CostModel>();
                foreach (var item in listMonth)
                {
                    var order = ListOder.FirstOrDefault(i => i.Month == item);
                    listResult.Add(order);
                }

                totalEstimatedCost = listResult.Sum(i => i.EstimatedCost).Value;
                totalRealCost = listResult.Sum(i => i.RealCost).Value;
                totalStatusCost = totalEstimatedCost - totalRealCost;
                var month = listResult.Where(i => i.RealCost == 0).Count();
                var index = 12 - month;
                totalNextMonth = totalEstimatedCost - totalRealCost;
                double money = (double)((totalNextMonth) / month);
                for (int i = index; i < 12; i++)
                {
                    listResult[i].NextMonthCost = Math.Round(money);
                }
                ListResult = listResult;
            }
            else
            {
                foreach (var item in listMonth)
                {
                    list.Add(new CostModel
                    {
                        Month = item,
                        Year = modelSearch.Year,
                        EstimatedCost = 0,
                        CheckEstimatedCost = 0,
                        RealCost = 0,
                        CheckRealCost = 0,
                        StatusCost = 0
                    });
                }
                var listResult = list;
                ListResult = listResult;
            }

            decimal totalCost = 0;
            var projects = db.Projects.AsNoTracking().Where(i => !i.Status.Equals(Constants.Prooject_Status_Finish) && i.KickOffDate.HasValue).Where(i => i.KickOffDate.Value.Year == modelSearch.Year).ToList();
            if (projects.Count > 0)
            {
                totalCost = projects.Sum(i => i.Price + i.DesignPrice);
            }

            return new
            {
                ListResult,
                TotalEstimatedCost = totalEstimatedCost,
                TotalRealCost = totalRealCost,
                TotalNextMonth = totalNextMonth,
                TotalStatusCost = totalStatusCost,
                TotalCost = totalCost
            };
        }

        public void AddCost(CostWarningModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var costs = db.Costs.Where(a => a.Year.Equals(model.Year)).ToList();
                if (costs.Count > 0)
                {
                    db.Costs.RemoveRange(costs);
                }
                try
                {
                    if (model.ListCost.Count > 0)
                    {
                        foreach (var item in model.ListCost)
                        {
                            Cost cost = new Cost()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Month = item.Month,
                                Year = item.Year,
                                EstimatedCost = item.EstimatedCost,
                                RealCost = item.RealCost
                            };
                            db.Costs.Add(cost);
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
    }
}
