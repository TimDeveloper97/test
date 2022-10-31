using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Survey;
using NTS.Model.Customers;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.CustomerRequirement;

namespace QLTK.Business.Solutions
{
    public class SolutionAnalysisProductBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public object SearchSolutionProduct(SolutionAnalysisProductSearchModel modelSearch)
        {
            SearchResultModel<SolutionAnalysisProductSearchModel> searchResult = new SearchResultModel<SolutionAnalysisProductSearchModel>();

            var dataQuery = (from a in db.SolutionAnalysisProducts.AsNoTracking()
                             join b in db.Solutions.AsNoTracking() on a.CustomerRequirementId equals b.Id
                             join c in db.Modules.AsNoTracking() on a.ObjectId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join p in db.Products.AsNoTracking() on a.ObjectId equals p.Id into ap
                             from apn in ap.DefaultIfEmpty()
                             where a.CustomerRequirementId.Equals(modelSearch.CustomerRequirementId)
                             select new SolutionAnalysisProductSearchModel()
                             {
                                 Id = a.Id,
                                 ObjectName = a.Type == Constants.Solution_ObjectType_Module ? ca.Name : apn.Name,
                                 ObjectCode = a.Type == Constants.Solution_ObjectType_Module ? ca.Code : apn.Code,
                                 CustomerRequirementId = a.CustomerRequirementId,
                             }).AsQueryable();

            decimal sumTotalPrice;

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            return new { searchResult };
        }

        public SolutionAnalysisProductModel GetByIdSolutionProduct(string solutionAnalysisProductId)
        {
            var resultInfo = db.SolutionAnalysisProducts.AsNoTracking().Where(u => u.Id.Equals(solutionAnalysisProductId)).Select(p => new SolutionAnalysisProductModel()
            {
                Id = p.Id,
                ObjectId = p.ObjectId,
                Type = p.Type,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SolutionProduct);
            }

            return resultInfo;
        }

        public void CreateSolutionProduct (SolutionAnalysisProductModel model, string userId)
        {
          
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.SolutionAnalysisProduct solutionAnalysisProduct = new NTS.Model.Repositories.SolutionAnalysisProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerRequirementId = model.CustomerRequirementId,
                        Type = model.Type,
                        ObjectId = model.ObjectId,
                        Name = model.Name,
                        Code = model.Code,
                        Link = model.Link == null ? "" : model.Link,
                        ManufactureName = model.ManufactureName == null ? "" : model.ManufactureName,
                    };

                    db.SolutionAnalysisProducts.Add(solutionAnalysisProduct);
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

        public void UpdateSolutionProduct(SolutionAnalysisProductModel model)
        {

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSolutionAnalysisProducts = db.SolutionAnalysisProducts.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    newSolutionAnalysisProducts.CustomerRequirementId = model.CustomerRequirementId;
                    newSolutionAnalysisProducts.ObjectId = model.ObjectId;
                    newSolutionAnalysisProducts.Type = model.Type;
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

        public void DeleteSolutionProduct(string solutionAnalysisProductId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var solutinProduct = db.SolutionAnalysisProducts.FirstOrDefault(a => a.Id.Equals(solutionAnalysisProductId));
                    if (solutinProduct == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SolutionAnalysisProduct);
                    }

                    db.SolutionAnalysisProducts.Remove(solutinProduct);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }
        }

    }
}
