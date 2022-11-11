using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using NTS.Model.Repositories;
using NTS.Model.Combobox;
using NTS.Model.ModuleError;
using NTS.Common;
using NTS.Model.Common;
using NTS.Utils;
using NTS.Common.Resource;
using NTS.Model.Error;
using NTS.Common.Helpers;

namespace QLTK.Business.ModuleError
{
    public class ModuleErrorBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ModuleErrorModel> SearchModuleErrors(ModuleErrorSearchModel model)
        {
            SearchResultModel<ModuleErrorModel> searchResult = new SearchResultModel<ModuleErrorModel>();

            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             join g in db.Modules.AsNoTracking() on a.ObjectId equals g.Id into ag
                             from g in ag.DefaultIfEmpty()
                             where a.Type == Constants.Error_Type_Error && a.Status != Constants.Problem_Status_Creating && a.Status != Constants.Problem_Status_Awaiting_Confirm && a.ObjectType == 1
                             orderby a.Code
                             select new ModuleErrorModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 ErrorGroupName = b.Name,
                                 AuthorId = a.AuthorId,
                                 AuthorName = c.Name,
                                 PlanStartDate = a.PlanStartDate,
                                 ObjectId = a.ObjectId,
                                 ProjectId = a.ProjectId,
                                 ProjectName = d.Name,
                                 Type = a.Type,
                                 Status = a.Status,
                                 ModuleErrorVisualId = a.ModuleErrorVisualId,
                                 ModuleErrorVisualName = g.Name,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = e.Name,
                                 ErrorBy = a.ErrorBy,
                                 ErrorByName = c.Name,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 DepartmentProcessName = e.Name,
                                 StageId = a.StageId,
                                 StageName = f.Name,
                                 FixBy = a.FixBy,
                                 FixByName = c.Name,
                                 Note = a.Note,
                                 ErrorCost = a.ErrorCost,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            dataQuery = dataQuery.Where(u => u.ObjectId.Equals(model.ModuleErrorVisualId));

            if (!string.IsNullOrEmpty(model.Subject))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(model.Subject.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (model.Status != null)
            {
                dataQuery = dataQuery.Where(u => u.Status == model.Status);
            }

            if (!string.IsNullOrEmpty(model.DateFrom.ToString()))
            {
                if (model.DateTo.ToString() == "")
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate >= model.DateFrom);
                }
                else
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate >= model.DateFrom && u.PlanStartDate <= model.DateTo);
                }

            }

            if (!string.IsNullOrEmpty(model.DateTo.ToString()))
            {
                if (model.DateFrom.ToString() == "")
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate <= model.DateTo);
                }
                else
                {
                    dataQuery = dataQuery.Where(u => u.PlanStartDate >= model.DateFrom && u.PlanStartDate <= model.DateTo);
                }

            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.Status4 = dataQuery.Where(r => r.Status == 5).Count();
            searchResult.Status6 = dataQuery.Where(r => r.Status == 6).Count();
            searchResult.Status7 = dataQuery.Where(r => r.Status == 7).Count();
            searchResult.Status8 = dataQuery.Where(r => r.Status == 8).Count();
            searchResult.Status9 = dataQuery.Where(r => r.Status == 9).Count();
            searchResult.MaxDeliveryDay = dataQuery.Where(r => r.Status == Constants.Error_Status_Close).Count();
            var listResult = dataQuery.ToList();
            foreach (var item in listResult)
            {
                var errorImage = db.ErrorImages.AsNoTracking().Where(i => i.ErrorId.Equals(item.Id)).Count();
                if (errorImage > 0)
                {
                    item.Image = true;
                }
            }
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public ModuleErrorModel GetErrorInfo(ModuleErrorModel model)
        {
            var resultInfo = (from a in db.Errors.AsNoTracking()
                              join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
                              join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                              from c in ac.DefaultIfEmpty()
                              join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                              from d in ad.DefaultIfEmpty()
                              join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
                              from e in ae.DefaultIfEmpty()
                              join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
                              from f in af.DefaultIfEmpty()
                              join g in db.Modules.AsNoTracking() on a.ObjectId equals g.Id into ag
                              from g in ag.DefaultIfEmpty()
                              where a.Id.Equals(model.Id)
                              select new ModuleErrorModel
                              {
                                  Id = a.Id,
                                  Subject = a.Subject,
                                  Code = a.Code,
                                  ErrorGroupId = a.ErrorGroupId,
                                  ErrorGroupName = b.Name,
                                  AuthorId = a.AuthorId,
                                  AuthorName = c.Name,
                                  PlanStartDate = a.PlanStartDate,
                                  ObjectId = a.ObjectId,
                                  ProjectId = a.ProjectId,
                                  ProjectName = d.Name,
                                  Status = a.Status,
                                  ModuleErrorVisualId = a.ModuleErrorVisualId,
                                  ModuleErrorVisualName = g.Name,
                                  DepartmentId = a.DepartmentId,
                                  DepartmentName = e.Name,
                                  ErrorBy = a.ErrorBy,
                                  ErrorByName = c.Name,
                                  DepartmentProcessId = a.DepartmentProcessId,
                                  DepartmentProcessName = e.Name,
                                  StageId = a.StageId,
                                  StageName = f.Name,
                                  FixBy = a.FixBy,
                                  FixByName = c.Name,
                                  Note = a.Note,
                                  ErrorCost = a.ErrorCost,
                                  Description = a.Description,
                                  CreateDate = a.CreateDate,
                                  CreateBy = a.CreateBy,
                                  UpdateBy = a.UpdateBy,
                                  UpdateDate = a.UpdateDate
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            return resultInfo;
        }

    }
}
