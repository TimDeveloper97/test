using NTS.Common;
using NTS.Model.Project;
using NTS.Model.ReportApplicationPresent;
using NTS.Model.Repositories;
using NTS.Model.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ReportApplicationPresent
{
    public class ReportApplicationPresentBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public ReportApplicationPresentModel ReportApplicationPresent(ReportApplicationPresentSearchModel searchModel)
        {
            ReportApplicationPresentModel model = new ReportApplicationPresentModel(); 
            // Dự án đã hoàn thành
            model.TotalProjectFinish = db.Projects.AsNoTracking().Where(a => a.Status.Equals(Constants.Prooject_Status_Finish) && a.KickOffDate.Value.Year == searchModel.Year).Count();
            //Dự án dự án đang triển khai
            model.TotalProjectToNotFinish = db.Projects.AsNoTracking().Where(a => !a.Status.Equals(Constants.Prooject_Status_NotStartedYet) && !a.Status.Equals(Constants.Prooject_Status_Finish) && a.KickOffDate.Value.Year == searchModel.Year).Count();
            //Giải pháp đang triển khai 
            model.TotalSolutionUse = db.Solutions.AsNoTracking().Where(a => a.Status.Equals(Constants.Solution_Status_Use) && a.StartDate.Value.Year == searchModel.Year).Count();
            // Giải pháp thành dự án
            model.TotalSolutionToProject = db.Solutions.AsNoTracking().Where(a => a.Status.Equals(Constants.Solution_Status_To_Project) && a.StartDate.Value.Year == searchModel.Year).Count();
            // Giải pháp không thành dự án
            model.TotalSolutionNotToProject = db.Solutions.AsNoTracking().Where(a => a.Status.Equals(Constants.Solution_Status_Not_To_Project) && a.StartDate.Value.Year == searchModel.Year).Count();
            // Giải pháp tạm dừng
            model.TotalSolutionStop = db.Solutions.AsNoTracking().Where(a => a.Status.Equals(Constants.Solution_Status_Stop) && a.StartDate.Value.Year == searchModel.Year).Count();
            // Giải pháp Hủy
            model.TotalSolutionCancel = db.Solutions.AsNoTracking().Where(a => a.Status.Equals(Constants.Solution_Status_Cancel) && a.StartDate.Value.Year == searchModel.Year).Count();

            //  tổng tiền bán thực tế không VAT của tất cả dự án
            var totalProject = (from a in db.Projects.AsNoTracking()
                                 where a.KickOffDate.Value.Year == searchModel.Year
                                 select new ProjectResultModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     KickOffDate = a.KickOffDate,
                                     SaleNoVAT = a.SaleNoVAT,
                                     Status = a.Status
                                 }).ToList();

            var groupedProject = totalProject.GroupBy(m => new { m.Status }).Select((n) => new ProjectReport { Status = n.Key.Status, ListProject = n.ToList() }).ToList();

            foreach (var item in groupedProject)
            {
                foreach (var ite in item.ListProject)
                {
                    if (item.Status == ite.Status)
                    {
                        item.TotalCostProject = item.TotalCostProject + ite.SaleNoVAT;
                    }
                }
            }

            model.Projects = groupedProject;

            // tổng tiền bán thực tế không VAT của tất cả giải pháp
            var totalSolution = (from a in db.Solutions.AsNoTracking()
                                 where a.StartDate.Value.Year == searchModel.Year
                                 select new SolutionModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     StartDate = a.StartDate,
                                     SaleNoVat = a.SaleNoVAT,
                                     Status = a.Status
                                 }).ToList();

            var grouped = totalSolution.GroupBy(m => new { m.Status }).Select((n) => new SolutionReport { Status = n.Key.Status, ListSolution = n.ToList() }).ToList();

            foreach (var item in grouped)
            {
                foreach (var ite in item.ListSolution)
                {
                    if (item.Status == ite.Status)
                    {
                        item.TotalCostSolution = item.TotalCostSolution + ite.SaleNoVat;
                    }
                }
            }

            model.Solutions = grouped;

            return model;
        }
    }
}
