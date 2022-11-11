using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.ImportProfileProblemExist;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLTK.Business.ImportProfileProblemExists
{
    public class ImportProfileProblemExistBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ImportProfileProblemExistCreateModel> SearchImportProfileProblemExist(ImportProfileProblemSearchModel modelSearch)
        {
            SearchResultModel<ImportProfileProblemExistCreateModel> searchResult = new SearchResultModel<ImportProfileProblemExistCreateModel>();

            var dataQuery = (from a in db.ImportProfileProblemExists.AsNoTracking()
                             where a.ImportProfileId.Equals(modelSearch.ImportProfileId) && a.Step.Equals(modelSearch.Step)
                             orderby a.Note
                             select new ImportProfileProblemExistCreateModel
                             {
                                 Id = a.Id,
                                 ImportProfileId = a.ImportProfileId,
                                 Note = a.Note,
                                 Plan = a.Plan,
                                 Step = a.Step,
                                 Status = a.Status
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void CreateImportProblemExist(string userId, List<ImportProfileProblemExistCreateModel> ListProblem)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //var list=db.ImportProfileProblemExists.Where(i=>i.ImportProfileId.Equals())

                    ImportProfileProblemExist import;
                    List<ImportProfileProblemExist> listProblem = new List<ImportProfileProblemExist>();
                    foreach (var item in ListProblem)
                    {
                        import = new ImportProfileProblemExist()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ImportProfileId = item.ImportProfileId,
                            Note = item.Note,
                            Plan = item.Plan,
                            Step = item.Step,
                            Status = item.Status,
                            CreateBy = userId,
                            CreateDate = DateTime.Now
                        };
                        listProblem.Add(import);
                    }

                    db.ImportProfileProblemExists.AddRange(listProblem);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(ListProblem, ex);
                }
            }
        }
    }
}
