using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.FlowStage;
using NTS.Model.HistoryVersion;
using NTS.Model.OutputResult;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.FlowStage
{
    public class FlowStageBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm dòng chảy
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<FlowStageSearchResultModel> SearchFlowStage(FlowStageSearchModel searchModel)
        {
            SearchResultModel<FlowStageSearchResultModel> searchResult = new SearchResultModel<FlowStageSearchResultModel>();
            var dataQuery = (from a in db.FlowStages.AsNoTracking()
                             orderby a.Code
                             select new FlowStageSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Code = a.Code
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        /// <summary>
        /// Xóa dòng chảy
        /// </summary>
        /// <param name="model"></param>
        public void DeleteFlowStage(FlowStageModel model)
        {
            var flowStageExist = db.FlowStages.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (flowStageExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FlowStage);
            }

            var flowStageChildExist = db.FlowStages.FirstOrDefault(a => a.ParentId.Equals(model.Id));
            if (flowStageChildExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.FlowStage);
            }

            //var flowStageUsed = db.FlowStageOutputResults.AsNoTracking().FirstOrDefault(a => a.FlowStageId.Equals(model.Id));
            //if (flowStageUsed != null)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.FlowStage);
            //}
            var tasks = db.Tasks.Where(a => a.FlowStageId.Equals(flowStageExist.Id)).ToList();
            if (tasks.Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.FlowStage);
            }

            try
            {
                var flowStageOutputResult = db.FlowStageOutputResults.Where(a => a.FlowStageId.Equals(model.Id)).ToList();
                db.FlowStageOutputResults.RemoveRange(flowStageOutputResult);
                db.FlowStages.Remove(flowStageExist);
                var NameOrCode = flowStageExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<FlowStageHistoryModel>(flowStageExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_FlowStage, flowStageExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới dòng chảy
        /// </summary>
        /// <param name="model"></param>
        public void CreateFlowStage(FlowStageModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim(); ;
            var flowStageNameExist = db.FlowStages.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (flowStageNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.FlowStage);
            }

            var flowStageCodeExist = db.FlowStages.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (flowStageCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.FlowStage);
            }

            try
            {
                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                NTS.Model.Repositories.FlowStage flowStage = new NTS.Model.Repositories.FlowStage
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = model.Code,
                    ParentId = model.ParentId,
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.FlowStages.Add(flowStage);

                foreach (var item in model.OutputResults)
                {
                    FlowStageOutputResult flowStageOutputResult = new FlowStageOutputResult()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FlowStageId = flowStage.Id,
                        OutputResultId = item.Id
                    };
                    db.FlowStageOutputResults.Add(flowStageOutputResult);
                }

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, flowStage.Name, flowStage.Id, Constants.LOG_FlowStage);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin dòng chảy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FlowStageModel GetFlowStage(FlowStageModel model)
        {
            var resultInfo = db.FlowStages.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new FlowStageModel
            {
                Id = p.Id,
                ParentId = p.ParentId,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FlowStage);
            }

            resultInfo.OutputResults = (from a in db.FlowStageOutputResults.AsNoTracking()
                                        where a.FlowStageId.Equals(model.Id)
                                        join b in db.OutputRessults.AsNoTracking() on a.OutputResultId equals b.Id
                                        join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                        join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
                                        select new OutputChooseModel
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            Code = b.Code,
                                            SBUId = b.SBUId,
                                            SBUName = c.Name,
                                            DepartmentId = b.DepartmentId,
                                            DepartmentName = d.Name
                                        }).ToList();

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật dòng chảy
        /// </summary>
        /// <param name="model"></param>
        public void UpdateFlowStage(FlowStageModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim(); ;

            var flowStageUpdate = db.FlowStages.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (flowStageUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.FlowStage);
            }

            var flowStageNameExist = db.FlowStages.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (flowStageNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.FlowStage);
            }

            var flowStageCodeExist = db.FlowStages.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (flowStageCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.FlowStage);
            }

            try
            {

                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                //var jsonBefor = AutoMapperConfig.Mapper.Map<FlowStageHistoryModel>(flowStageUpdate);

                flowStageUpdate.Name = model.Name;
                flowStageUpdate.Code = model.Code;
                flowStageUpdate.ParentId = model.ParentId;
                flowStageUpdate.Note = model.Note;
                flowStageUpdate.UpdateBy = model.UpdateBy;
                flowStageUpdate.UpdateDate = DateTime.Now;

                var flowStageOutputResults = db.FlowStageOutputResults.Where(a => a.FlowStageId.Equals(flowStageUpdate.Id)).ToList();
                db.FlowStageOutputResults.RemoveRange(flowStageOutputResults);

                foreach (var item in model.OutputResults)
                {
                    FlowStageOutputResult flowStageOutputResult = new FlowStageOutputResult()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FlowStageId = flowStageUpdate.Id,
                        OutputResultId = item.Id
                    };
                    db.FlowStageOutputResults.Add(flowStageOutputResult);
                }

                //var jsonApter = AutoMapperConfig.Mapper.Map<FlowStageHistoryModel>(flowStageUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_FlowStage, flowStageUpdate.Id, flowStageUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public SearchResultModel<OutputResultModel> SearchOutputResult(OutputResultSearchModel modelSearch)
        {
            SearchResultModel<OutputResultModel> search = new SearchResultModel<OutputResultModel>();

            var dataQuery = (from a in db.OutputRessults.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id into ab
                             from abn in ab.DefaultIfEmpty()
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from acn in ac.DefaultIfEmpty()
                             join d in db.FlowStageOutputResults.AsNoTracking() on a.Id equals d.OutputResultId into ad
                             from adv in ad.DefaultIfEmpty()
                             join e in db.FlowStages.AsNoTracking() on adv.FlowStageId equals e.Id into ade
                             from adev in ade.DefaultIfEmpty()
                             select new OutputResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 SBUId = a.SBUId,
                                 SBUName = abn!=null? abn.Name:string.Empty,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = acn != null ? acn.Name : string.Empty,
                                 FlowStageId = adv != null ? adv.FlowStageId : "",
                                 FlowStageName = adev != null ? adev.Name : "",
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(t => t.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(t => t.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.FlowStageId))
            {
                dataQuery = dataQuery.Where(t => t.FlowStageId.Equals(modelSearch.FlowStageId));
            }

            var list = dataQuery.ToList();

            search.ListResult = list.GroupBy(g => new { g.Name, g.Id, g.Code, g.SBUId, g.SBUName, g.DepartmentId, g.DepartmentName }).Select(s => new OutputResultModel
            {
                Id = s.Key.Id,
                Name = s.Key.Name,
                Code = s.Key.Code,
                SBUId = s.Key.SBUId,
                SBUName = s.Key.SBUName,
                DepartmentId = s.Key.DepartmentId,
                DepartmentName = s.Key.DepartmentName,
                FlowStageName = string.Join("; ", s.Select(a => a.FlowStageName).ToList()),
            }).ToList();

            search.TotalItem = search.ListResult.Count();

            return search;
        }
    }
}
