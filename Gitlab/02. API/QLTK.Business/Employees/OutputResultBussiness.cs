using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Document;
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

namespace QLTK.Business.OutputResult
{
    public class OutputResultBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm kết quả đầu ra
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<OutputResultModel> SearchOutputResult(OutputResultSearchModel searchModel)
        {
            SearchResultModel<OutputResultModel> searchResult = new SearchResultModel<OutputResultModel>();
            var dataQuery = (from a in db.OutputRessults.AsNoTracking()
                             join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id into ab
                             from abn in ab.DefaultIfEmpty()
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from acn in ac.DefaultIfEmpty()
                             orderby a.Code
                             select new OutputResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 SBUId = a.SBUId,
                                 SBUName = abn != null ? abn.Name : string.Empty,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = acn != null ? acn.Name : string.Empty,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(searchModel.Name.ToLower()) || t.Code.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                dataQuery = dataQuery.Where(t => t.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(t => t.DepartmentId.Equals(searchModel.DepartmentId));
            }

            //var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            var listResult = dataQuery.ToList();

            foreach (var item in listResult)
            {
                item.FlowStages = (from a in db.FlowStageOutputResults.AsNoTracking()
                                   where a.OutputResultId.Equals(item.Id)
                                   join b in db.FlowStages.AsNoTracking() on a.FlowStageId equals b.Id
                                   select new FlowStageModel
                                   {
                                       Id = a.FlowStageId,
                                       Name = b.Name,
                                       Code = b.Code
                                   }).ToList();

                if (item.FlowStages != null && item.FlowStages.Count > 0)
                {
                    item.FlowStagesCode = string.Join(", ", item.FlowStages.Select(a => a.Code));
                }
            }

            if (!string.IsNullOrEmpty(searchModel.FlowStageId))
            {
                listResult = listResult.Where(i => i.FlowStages.FirstOrDefault(a => a.Id.Equals(searchModel.FlowStageId)) != null).ToList();
            }

            searchResult.TotalItem = listResult.Count();

            searchResult.ListResult = listResult.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            return searchResult;
        }

        /// <summary>
        /// Xóa kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        public void DeleteOutputResult(OutputResultModel model)
        {
            var outputResultExist = db.OutputRessults.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (outputResultExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.OutputResult);
            }

            try
            {
                var flowStageOutputResult = db.FlowStageOutputResults.Where(a => a.OutputResultId.Equals(model.Id)).ToList();
                db.FlowStageOutputResults.RemoveRange(flowStageOutputResult);

                var outputDocuments = db.OutputRessultDocuments.Where(a => a.OutputRessultId.Equals(model.Id)).ToList();
                db.OutputRessultDocuments.RemoveRange(outputDocuments);

                db.OutputRessults.Remove(outputResultExist);
                var NameOrCode = outputResultExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<OutputResultHistoryModel>(outputResultExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_OutputResult, outputResultExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Thêm mới kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        public void CreateOutputResult(OutputResultModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim(); ;
            var outputResultNameExist = db.OutputRessults.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (outputResultNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.OutputResult);
            }

            var outputResultCodeExist = db.OutputRessults.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (outputResultCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.OutputResult);
            }

            try
            {
                NTS.Model.Repositories.OutputRessult outputRessult = new NTS.Model.Repositories.OutputRessult
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = model.Code,
                    Name = model.Name.NTSTrim(),
                    Note = model.Note,
                    SBUId = model.SBUId,
                    DepartmentId = model.DepartmentId,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.OutputRessults.Add(outputRessult);

                foreach (var item in model.FlowStages)
                {
                    FlowStageOutputResult flowStageOutputResult = new FlowStageOutputResult()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FlowStageId = item.Id,
                        OutputResultId = outputRessult.Id
                    };
                    db.FlowStageOutputResults.Add(flowStageOutputResult);
                }

                foreach (var item in model.Documents)
                {
                    OutputRessultDocument outputRessultDocument = new OutputRessultDocument()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = item.Id,
                        OutputRessultId = outputRessult.Id
                    };
                    db.OutputRessultDocuments.Add(outputRessultDocument);
                }

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, outputRessult.Name, outputRessult.Id, Constants.LOG_FlowStage);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OutputResultModel GetOutputResult(OutputResultModel model)
        {
            var resultInfo = db.OutputRessults.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new OutputResultModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Note = p.Note,
                SBUId = p.SBUId,
                DepartmentId = p.DepartmentId
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.OutputResult);
            }

            resultInfo.FlowStages = (from a in db.FlowStageOutputResults.AsNoTracking()
                                     where a.OutputResultId.Equals(model.Id)
                                     join b in db.FlowStages.AsNoTracking() on a.FlowStageId equals b.Id
                                     select new FlowStageModel
                                     {
                                         Id = b.Id,
                                         Name = b.Name,
                                         Code = b.Code,
                                     }).ToList();

            resultInfo.Documents = (from a in db.OutputRessultDocuments.AsNoTracking()
                                    where a.OutputRessultId.Equals(model.Id)
                                    join c in db.Documents.AsNoTracking() on a.DocumentId equals c.Id
                                    join b in db.DocumentGroups on c.DocumentGroupId equals b.Id
                                    select new DocumentSearchResultModel
                                    {
                                        Id = a.DocumentId,
                                        Name = c.Name,
                                        Code = c.Code,
                                        DocumentGroupId = b.Id,
                                        DocumentGroupName = b.Name
                                    }).ToList();

            return resultInfo;
        }

        /// <summary>
        /// Cập nhật kết quả đầu ra
        /// </summary>
        /// <param name="model"></param>
        public void UpdateOutputResult(OutputResultModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim(); ;

            var outputResultUpdate = db.OutputRessults.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (outputResultUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.OutputResult);
            }

            var outputResultNameExist = db.OutputRessults.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (outputResultNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.OutputResult);
            }

            var flowResultCodeExist = db.OutputRessults.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (flowResultCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.OutputResult);
            }

            try
            {
                //var jsonBefor = AutoMapperConfig.Mapper.Map<OutputResultHistoryModel>(outputResultUpdate);

                outputResultUpdate.Name = model.Name;
                outputResultUpdate.Code = model.Code;
                outputResultUpdate.SBUId = model.SBUId;
                outputResultUpdate.DepartmentId = model.DepartmentId;
                outputResultUpdate.Note = model.Note;
                outputResultUpdate.UpdateBy = model.UpdateBy;
                outputResultUpdate.UpdateDate = DateTime.Now;

                var flowStageOutputResults = db.FlowStageOutputResults.Where(a => a.OutputResultId.Equals(outputResultUpdate.Id)).ToList();
                db.FlowStageOutputResults.RemoveRange(flowStageOutputResults);

                foreach (var item in model.FlowStages)
                {
                    FlowStageOutputResult flowStageOutputResult = new FlowStageOutputResult()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FlowStageId = item.Id,
                        OutputResultId = outputResultUpdate.Id
                    };
                    db.FlowStageOutputResults.Add(flowStageOutputResult);
                }

                var outputDocuments = db.OutputRessultDocuments.Where(a => a.OutputRessultId.Equals(model.Id)).ToList();
                db.OutputRessultDocuments.RemoveRange(outputDocuments);

                foreach (var item in model.Documents)
                {
                    OutputRessultDocument outputRessultDocument = new OutputRessultDocument()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = item.Id,
                        OutputRessultId = outputResultUpdate.Id
                    };
                    db.OutputRessultDocuments.Add(outputRessultDocument);
                }

                //var jsonApter = AutoMapperConfig.Mapper.Map<OutputResultHistoryModel>(outputResultUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_FlowStage, outputResultUpdate.Id, outputResultUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public SearchResultModel<FlowStageSearchResultModel> SearchFlowStage(FlowStageSearchModel modelSearch)
        {
            SearchResultModel<FlowStageSearchResultModel> search = new SearchResultModel<FlowStageSearchResultModel>();

            var dataQuery = (from a in db.FlowStages.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new FlowStageSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        public SearchResultModel<DocumentSearchResultModel> SearchChooseDocument(DocumentSearchModel modelSearch)
        {
            SearchResultModel<DocumentSearchResultModel> search = new SearchResultModel<DocumentSearchResultModel>();

            var dataQuery = (from a in db.Documents.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id) && a.Status == Constants.Document_Status_Used
                             orderby a.Name
                             join b in db.DocumentGroups.AsNoTracking() on a.DocumentGroupId equals b.Id
                             join c in db.DocumentTypes.AsNoTracking() on a.DocumentTypeId equals c.Id
                             select new DocumentSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 DocumentGroupId = a.DocumentGroupId,
                                 DocumentGroupName = b.Name,
                                 DocumentTypeName = c.Name
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.DocumentGroupId))
            {
                dataQuery = dataQuery.Where(t => t.DocumentGroupId.Equals(modelSearch.DocumentGroupId));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();

            foreach (var item in search.ListResult)
            {
                item.ListFile = (from a in db.DocumentFiles.AsNoTracking()
                                 where a.DocumentId.Equals(item.Id)
                                 select new DocumentFileModel
                                 {
                                     Id = a.Id,
                                     FileName = a.FileName,
                                     FileSize = a.FileSize,
                                     Path = a.Path
                                 }).ToList();
            }

            return search;
        }
    }
}
