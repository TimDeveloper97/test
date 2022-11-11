using NTS.Common;
using NTS.Common.Helpers;
using NTS.Model;
using NTS.Model.Error;
using NTS.Model.ProjectProducts;
using NTS.Model.ReportQualityPresent;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ReportQualityPresent
{
    public class ReportQualityPresentBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public ReportQualityPresentModel searchEmployees(ReportQualityPresentSearchModel modelSearch)
        {
            ReportQualityPresentModel model = new ReportQualityPresentModel();

            // Tổng số dự án đã triển khai
            model.Total_Project_Use = db.Projects.AsNoTracking().Where(a => !a.Status.Contains(Constants.Prooject_Status_NotStartedYet)).Count();

            // SỐ lượng tồn đọng đã xảy ra 

            var dataProblemExit = (from a in db.Errors.AsNoTracking()
                                   join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
                                   join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                                   from c in ac.DefaultIfEmpty()
                                   join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                                   from d in ad.DefaultIfEmpty()
                                   join e in db.Modules.AsNoTracking() on a.ObjectId equals e.Id into ae
                                   from ea in ae.DefaultIfEmpty()
                                   join f in db.ModuleGroups.AsNoTracking() on ea.ModuleGroupId equals f.Id into af
                                   from fa in af.DefaultIfEmpty()
                                   select new ErrorModel
                                   {
                                       Id = a.Id,
                                       Subject = a.Subject,
                                       Code = a.Code,
                                       ErrorGroupId = a.ErrorGroupId,
                                       ErrorGroupName = b.Name,
                                       AuthorId = a.AuthorId,
                                       AuthorName = c.Name,
                                       ProjectId = a.ProjectId,
                                       ProjectName = d.Name,
                                       ModuleErrorVisualCode = fa.Code,
                                       Status = a.Status,
                                       FixBy = a.FixBy,
                                       Type = a.Type,
                                       TypeName = a.Type == 1 ? "Lỗi" : "Vấn đề",
                                       KickOffDate = d.KickOffDate,
                                   }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.ProjectId))
            {
                dataProblemExit = dataProblemExit.Where(u => u.ProjectId.Equals(modelSearch.ProjectId));
            }

            if (modelSearch.DateFrom.HasValue)
            {
                dataProblemExit = dataProblemExit.Where(u => u.KickOffDate >= modelSearch.DateFrom.Value);
            }

            if (modelSearch.DateTo.HasValue)
            {
                dataProblemExit = dataProblemExit.Where(u => u.KickOffDate <= modelSearch.DateTo.Value);
            }

            var listProblemExit = dataProblemExit.ToList();
            model.Total_Problem_Project = dataProblemExit.Count();
            foreach (var item in listProblemExit)
            {
                if (!string.IsNullOrEmpty(item.FixBy))
                {
                    item.FixByName = db.Employees.AsNoTracking().FirstOrDefault(b => b.Id.Equals(item.FixBy)).Name;
                }
            }
            model.List_Problem_Project = SQLHelpper.OrderBy(dataProblemExit, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();


            //Hiển thị tổng lỗi theo từng dự án

            var error_projct = (
                                from b in db.ProjectErrors.AsNoTracking()
                                join c in db.Errors.AsNoTracking() on b.ErrorId equals c.Id
                                where modelSearch.ProjectId.Equals(b.ProjectId) && c.Type == 1
                                select new ProjectProductErrorModel
                                {
                                    ErrorId = c.Id,
                                    ErrorName = c.Subject,
                                }).AsQueryable();
            var list_error_projet = error_projct.ToList();

            var group_error = list_error_projet.GroupBy(a => new { a.Id, a.ErrorName }).Select(gr => new ProjectProductErrorModel() { Id = gr.Key.Id, ErrorName = gr.Key.ErrorName });

            model.Total_Error_Project = group_error.Count();
            model.List_Error_Project = group_error.ToList();


            //Hiên thị tổng lỗi théo nhóm sản phẩm

            var error_project_product = (from d in db.Errors.AsNoTracking()
                                         join c in db.Modules.AsNoTracking() on d.ObjectId equals c.Id into dac
                                         from acd in dac.DefaultIfEmpty()
                                         join e in db.ModuleGroups.AsNoTracking() on acd.ModuleGroupId equals e.Id
                                         where modelSearch.ProjectId.Equals(d.ProjectId) && d.Type == 1
                                         select new ProjectProductErrorModel
                                         {
                                             Id = e.Id,
                                             ModuleGroupName = e.Code,
                                             ErrorName = d.Subject,
                                         }).AsQueryable();

            var listError = error_project_product.ToList();

            var grouped = listError.GroupBy(a => new { a.ModuleGroupName }).Select(gr => new ProjectProductErrorModel() { ModuleGroupName = gr.Key.ModuleGroupName, TotalError = gr.Count() });

            model.List_Error_ProjectProduct = grouped.ToList();

            model.Total_Error_ProjectProduct = grouped.Count();
            return model;


        }

        public object ErrorWithLineProduct(ReportQualityPresentSearchModel searchModel, string deparmentId)
        {
            List<string> listName = new List<string>();
            List<double> listTotalModule = new List<double>();
            List<double> listError = new List<double>();
            List<double> listProblem = new List<double>();

            // Lấy nhóm module con
            var moduleGroups = db.ModuleGroups.AsNoTracking().Where(i => i.ParentId.Equals(searchModel.ModuleGroupId)).ToList();

            // Lấy thông tin db
            var modules = db.Modules.AsNoTracking().Where(i => i.DepartmentId.Equals(deparmentId)).ToList();
            var projectProducts = db.ProjectProducts.AsNoTracking().ToList();
            var projects = db.Projects.AsNoTracking().ToList();
            double totalError = 0;
            double totalProblem = 0;

            foreach (var item in moduleGroups)
            {
                var totalModule = modules.Where(i => i.ModuleGroupId.Equals(item.Id)).Count();

                var data = (from a in modules
                            join b in projectProducts on a.Id equals b.ModuleId
                            join c in projects on b.ProjectId equals c.Id
                            where b.DataType == Constants.ProjectProduct_DataType_Module && a.ModuleGroupId.Equals(item.Id) || b.DataType == Constants.ProjectProduct_DataType_Paradigm && a.ModuleGroupId.Equals(item.Id)
                            select new
                            {
                                a.Id
                            });

                var listModuleId = (from a in data
                                    group a by new { a.Id } into g
                                    select new
                                    {
                                        g.Key.Id,
                                    }).ToList();

                var error = (from a in db.Errors.AsEnumerable()
                             join b in listModuleId.AsEnumerable() on a.ObjectId equals b.Id
                             select new
                             {
                                 a.Id,
                                 a.Type,
                                 a.PlanStartDate
                             }).ToList();

                if (searchModel.DateFrom.HasValue)
                {
                    error = error.Where(i => i.PlanStartDate >= searchModel.DateFrom).ToList();
                }

                if (searchModel.DateTo.HasValue)
                {
                    error = error.Where(i => i.PlanStartDate <= searchModel.DateTo).ToList();
                }
                totalError = error.Where(i => i.Type == Constants.Error_Type_Error).Count();
                totalProblem = error.Where(i => i.Type == Constants.Error_Type_Issue).Count();
                if (error.Count() > 0)
                {
                    listName.Add(item.Code);
                    listTotalModule.Add(totalModule);
                    listError.Add(totalError);
                    listProblem.Add(totalProblem);
                }
            }

            return new
            {
                listName,
                listTotalModule,
                listError,
                listProblem
            };
        }

        public object ErrorGroup(SearchCommonModel model)
        {
            List<string> listName = new List<string>();
            List<double> listError = new List<double>();
            List<double> listProblem = new List<double>();
            double totalError = 0;
            double totalProblem = 0;

            // Danh sách nhóm vấn đề
            var errorGroups = db.ErrorGroups.AsNoTracking().ToList();
            var errors = db.Errors.AsNoTracking().ToList();

            foreach (var item in errorGroups)
            {
                var data = (from a in errors
                            join b in errorGroups on a.ErrorGroupId equals b.Id
                            where a.ErrorGroupId.Equals(item.Id)
                            select new
                            {
                                a.Id,
                                a.Type,
                                a.PlanStartDate
                            }).ToList();

                if (model.DateFrom.HasValue)
                {
                    data = data.Where(i => i.PlanStartDate >= model.DateFrom).ToList();
                }

                if (model.DateTo.HasValue)
                {
                    data = data.Where(i => i.PlanStartDate <= model.DateTo).ToList();
                }

                if (data.Count > 0)
                {
                    totalError = data.Where(i => i.Type == Constants.Error_Type_Error).Count();
                    totalProblem = data.Where(i => i.Type == Constants.Error_Type_Issue).Count();

                    listName.Add(item.Name);
                    listError.Add(totalError);
                    listProblem.Add(totalProblem);
                }
            }

            return new
            {
                listName,
                listError,
                listProblem
            };
        }

        public object ErrorRatio(ReportQualityPresentSearchModel model)
        {
            List<string> listName = new List<string>();
            List<double> listError = new List<double>();
            List<double> listProblem = new List<double>();
            double error = 0;
            double problem = 0;
            double radioError = 0;
            double radioProblem = 0;
            double total = 0;

            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo);
            }
            else
            {
                if (!model.DateFrom.HasValue || !model.DateTo.HasValue)
                {
                    return null;
                }

                dateFrom = model.DateFrom.Value.ToStartDate();
                dateTo = model.DateTo.Value.ToEndDate();
            }

            var errors = (from a in db.Errors.AsNoTracking()
                          join b in db.Modules.AsNoTracking() on a.ObjectId equals b.Id
                          select new
                          {
                              a.Id,
                              a.ErrorGroupId,
                              b.ModuleGroupId,
                              a.Type,
                              a.PlanStartDate
                          }).ToList();

            if (!string.IsNullOrEmpty(model.ModuleGroupId))
            {
                errors = errors.Where(i => i.ModuleGroupId.Equals(model.ModuleGroupId)).ToList();
            }

            if (!string.IsNullOrEmpty(model.ErrorGroupId))
            {
                errors = errors.Where(i => i.ErrorGroupId.Equals(model.ErrorGroupId)).ToList();
            }

            errors = errors.Where(i => i.PlanStartDate >= dateFrom && i.PlanStartDate <= dateTo).ToList();

            if (model.TimeType == Constants.TimeType_ThisMonth || model.TimeType == Constants.TimeType_LastMonth || model.TimeType == Constants.TimeType_Month)
            {
                int day = DateTime.DaysInMonth(dateFrom.Year, dateFrom.Month);
                for (int a = 1; a <= day; a++)
                {
                    listName.Add(a.ToString());
                    error = errors.Where(i => i.PlanStartDate.Value.Day == a && i.PlanStartDate.Value.Month == dateFrom.Month && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Error).Count();
                    problem = errors.Where(i => i.PlanStartDate.Value.Day == a && i.PlanStartDate.Value.Month == dateFrom.Month && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Issue).Count();
                    total = error + problem;
                    if (total > 0)
                    {
                        radioError = Math.Round((error / total * 100), 1);
                        radioProblem = Math.Round((problem / total * 100), 1);
                    }
                    else
                    {
                        radioError = 0;
                        radioProblem = 0;
                    }

                    listError.Add(radioError);
                    listProblem.Add(radioProblem);
                }
            }
            else if (model.TimeType == Constants.TimeType_Quarter)
            {
                var month = dateFrom.Month;
                if (1 <= month && month <= 3)
                {
                    for (int a = 1; a <= 3; a++)
                    {
                        listName.Add(a.ToString());
                        error = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Error).Count();
                        problem = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Issue).Count();
                        total = error + problem;
                        if (total > 0)
                        {
                            radioError = Math.Round((error / total * 100), 1);
                            radioProblem = Math.Round((problem / total * 100), 1);
                        }
                        else
                        {
                            radioError = 0;
                            radioProblem = 0;
                        }

                        listError.Add(radioError);
                        listProblem.Add(radioProblem);
                    }
                }
                else if (4 <= month && month <= 6)
                {
                    for (int a = 4; a <= 6; a++)
                    {
                        listName.Add(a.ToString());
                        error = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Error).Count();
                        problem = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Issue).Count();
                        total = error + problem;
                        if (total > 0)
                        {
                            radioError = Math.Round((error / total * 100), 1);
                            radioProblem = Math.Round((problem / total * 100), 1);
                        }
                        else
                        {
                            radioError = 0;
                            radioProblem = 0;
                        }

                        listError.Add(radioError);
                        listProblem.Add(radioProblem);
                    }
                }
                else if (7 <= month && month <= 9)
                {
                    for (int a = 7; a <= 9; a++)
                    {
                        listName.Add(a.ToString());
                        error = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Error).Count();
                        problem = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Issue).Count();
                        total = error + problem;
                        if (total > 0)
                        {
                            radioError = Math.Round((error / total * 100), 1);
                            radioProblem = Math.Round((problem / total * 100), 1);
                        }
                        else
                        {
                            radioError = 0;
                            radioProblem = 0;
                        }

                        listError.Add(radioError);
                        listProblem.Add(radioProblem);
                    }
                }
                else if (10 <= month && month <= 12)
                {
                    for (int a = 10; a <= 12; a++)
                    {
                        listName.Add(a.ToString());
                        error = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Error).Count();
                        problem = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Issue).Count();
                        total = error + problem;
                        if (total > 0)
                        {
                            radioError = Math.Round((error / total * 100), 1);
                            radioProblem = Math.Round((problem / total * 100), 1);
                        }
                        else
                        {
                            radioError = 0;
                            radioProblem = 0;
                        }

                        listError.Add(radioError);
                        listProblem.Add(radioProblem);
                    }
                }
            }
            else if (model.TimeType == Constants.TimeType_ThisYear || model.TimeType == Constants.TimeType_LastYear || model.TimeType == Constants.TimeType_Year)
            {
                for (int a = 1; a <= 12; a++)
                {
                    listName.Add(a.ToString());
                    error = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Error).Count();
                    problem = errors.Where(i => i.PlanStartDate.Value.Month == a && i.PlanStartDate.Value.Year == dateFrom.Year && i.Type == Constants.Error_Type_Issue).Count();
                    total = error + problem;
                    if (total > 0)
                    {
                        radioError = Math.Round((error / total * 100), 1);
                        radioProblem = Math.Round((problem / total * 100), 1);
                    }
                    else
                    {
                        radioError = 0;
                        radioProblem = 0;
                    }

                    listError.Add(radioError);
                    listProblem.Add(radioProblem);
                }
            }

            return new
            {
                listName,
                listError,
                listProblem
            };
        }
    }
}
