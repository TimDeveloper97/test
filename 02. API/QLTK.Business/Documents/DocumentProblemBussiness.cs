using NTS.Model.Combobox;
using NTS.Model.DocumentProblem;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common.Helpers;
using NTS.Common;
using NTS.Model;
using NTS.Common.Resource;
using QLTK.Business.Users;

namespace QLTK.Business.DocumentProblem
{
    public class DocumentProblemBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<DocumentProblemSearchResultModel> SearchDocumentProblem(DocumentProblemSearchModel searchModel)
        {
            SearchResultModel<DocumentProblemSearchResultModel> searchResult = new SearchResultModel<DocumentProblemSearchResultModel>();
            var dataQuery = (from a in db.DocumentProblems.AsNoTracking()
                             where a.DocumentId.Equals(searchModel.DocumentId)
                             select new DocumentProblemSearchResultModel
                             {
                                 Id = a.Id,
                                 Problem = a.Problem,
                                 Status = a.Status,
                                 ProblemDate = a.ProblemDate,
                                 FinishExpectedDate = a.FinishExpectedDate,
                                 FinishDate = a.FinishDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Problem))
            {
                dataQuery = dataQuery.Where(u => u.Problem.ToUpper().Contains(searchModel.Problem.ToUpper()));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status);
            }

            if (searchModel.ProblemDateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.ProblemDate >= searchModel.ProblemDateFrom);
            }

            if (searchModel.ProblemDateTo.HasValue)
            {
                searchModel.ProblemDateTo = searchModel.ProblemDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(u => u.ProblemDate <= searchModel.ProblemDateTo);
            }

            searchResult.TotalItem = dataQuery.Count();
            //var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        public void DeleteDocumentProblem(DocumentProblemModel model)
        {
            var documentProblemExist = db.DocumentProblems.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (documentProblemExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentProblem);
            }

            try
            {
                db.DocumentProblems.Remove(documentProblemExist);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void CreateDocumentProblem(DocumentProblemModel model)
        {
            try
            {
                NTS.Model.Repositories.DocumentProblem documentProblem = new NTS.Model.Repositories.DocumentProblem
                {
                    Id = Guid.NewGuid().ToString(),
                    DocumentId = model.DocumentId,
                    Problem = model.Problem,
                    Status = model.Status,
                    ProblemDate = model.ProblemDate,
                    FinishExpectedDate = model.FinishExpectedDate,
                    FinishDate = model.FinishDate,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                db.DocumentProblems.Add(documentProblem);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void UpdateDocumentProblem(DocumentProblemModel model)
        {
            var documentProblemUpdate = db.DocumentProblems.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (documentProblemUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentProblem);
            }

            try
            {
                documentProblemUpdate.Problem = model.Problem;
                documentProblemUpdate.Status = model.Status;
                documentProblemUpdate.ProblemDate = model.ProblemDate;
                documentProblemUpdate.FinishExpectedDate = model.FinishExpectedDate;
                documentProblemUpdate.FinishDate = model.FinishDate;
                documentProblemUpdate.UpdateBy = model.UpdateBy;
                documentProblemUpdate.UpdateDate = DateTime.Now;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public DocumentProblemModel GetDocumentProblem(DocumentProblemModel model)
        {
            var resultInfo = db.DocumentProblems.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DocumentProblemModel
            {
                Id = p.Id,
                DocumentId = p.DocumentId,
                Problem = p.Problem,
                Status = p.Status,
                ProblemDate = p.ProblemDate,
                FinishExpectedDate = p.FinishExpectedDate,
                FinishDate = p.FinishDate
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentProblem);
            }

            return resultInfo;
        }
    }
}
