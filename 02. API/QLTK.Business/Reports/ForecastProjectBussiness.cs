using NTS.Common;
using NTS.Model.ForecastProject;
using NTS.Model.Project;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ForecastProject
{
    public class ForecastProjectBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public object GetForecastProjects()
        {
            var data = (from a in db.Projects.AsNoTracking()
                        orderby a.Name
                        select new ProjectModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Code = a.Code,
                            Status = a.Status,
                            DateTo = a.DateTo
                        }).ToList();

            //Số lượng dự án đang triển khai hiện tại
            var total = data.Where(i => !Constants.Prooject_Status_NotStartedYet.Equals(i.Status) && !Constants.Prooject_Status_Finish.Equals(i.Status)).Count();

            double totalPaln = 0;
            double ratio = 0;
            int totalUnfinishedPlan = 0;
            int totalUnplanned = 0;
            int planSlow = 0;
            List<ForecastProjectModel> listProject = new List<ForecastProjectModel>();
            foreach (var item in data)
            {
                //Danh sách công việc từng dự án
                var listPlan = db.Plans.AsNoTracking().Where(i => item.Id.Equals(i.ProjectId)).ToList();

                //Tỷ lệ hoàn thành dự án
                var finish = listPlan.Where(i => i.Status == Constants.Plan_Status_Done).Count();
                totalPaln = listPlan.Count;
                var result = (double)finish / totalPaln * 100;
                bool check = Double.IsNaN(result);
                if (check)
                {
                    ratio = 0;
                }
                else
                {
                    ratio = Math.Round(result, 2);
                }

                //Số lượng đầu việc theo dự án chưa hoàn thành
                totalUnfinishedPlan = (int)(totalPaln - finish);

                //Số lượng đầu việc theo dự án chưa có kế hoạch
                totalUnplanned = listPlan.Where(i => !i.PlanStartDate.HasValue || !i.PlanDueDate.HasValue ).Count();

                //Số lượng đầu việc theo dự án có kế hoạch nhưng chậm so với deadline kick-off
                var plan = listPlan.Where(i => i.PlanStartDate.HasValue && i.PlanDueDate.HasValue).ToList();
                planSlow = plan.Where(i => item.DateTo < i.PlanDueDate).Count();

                //Add model
                listProject.Add(new ForecastProjectModel
                {
                    Name = item.Name,
                    FinishProject = ratio,
                    TotalUnfinishedPlan = totalUnfinishedPlan,
                    TotalUnplanned = totalUnplanned,
                    PlanSlow = planSlow
                });
            }

            //Số lượng đầu việc chưa hoàn thành
            var totalUnfinishedPlanAll = db.Plans.AsNoTracking().Where(i => Constants.Plan_Status_Done != i.Status).Count();

            //Số lượng vấn đề tồn đọng
            var totalError = db.Errors.AsNoTracking().Select(i => i.Id).Count();

            //Số lượng vấn đề tồn đọng chưa có giải pháp
            var totalErrorNoSolution = db.Errors.AsNoTracking().Where(i => string.IsNullOrEmpty(i.Solution)).Count();

            //Số lượng vấn đề tồn đọng có giải pháp nhưng deadline chậm so với deadline kick-off
            int totalErrorCompareDateline = 0;
            var errors = db.Errors.AsNoTracking().ToList();
            foreach (var item in errors)
            {
                if (!string.IsNullOrEmpty(item.ProjectId) && !string.IsNullOrEmpty(item.Solution))
                {
                    var projects = db.Projects.AsNoTracking().FirstOrDefault(i => item.ProjectId.Equals(i.Id));
                    if (projects != null)
                    {
                        if (item.PlanFinishDate > projects.DateTo)
                        {
                            totalErrorCompareDateline++;
                        }
                    }
                }
            }

            return new
            {
                Total = total,
                ListProject = listProject,
                TotalUnfinishedPlanAll = totalUnfinishedPlanAll,
                TotalError = totalError,
                TotalErrorNoSolution = totalErrorNoSolution,
                TotalErrorCompareDateline = totalErrorCompareDateline
            };
        }
    }
}
