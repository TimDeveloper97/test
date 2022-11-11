using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Categories;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Categories
{
    public class QuoteStepBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ResultSearchQuoteStepModel> SearchQuoteStep(SearchQuoteStepModel search)
        {
            SearchResultModel<ResultSearchQuoteStepModel> searchResult = new SearchResultModel<ResultSearchQuoteStepModel>();
            var dataQuerys = (from a in db.QuotationSteps.AsNoTracking()
                              join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id
                              orderby a.Index
                              select new ResultSearchQuoteStepModel
                              {
                                  Id = a.Id,
                                  Code = a.Code,
                                  SBUCode = b.Code,
                                  SBUName = b.Name,
                                  Name = a.Name,
                                  Index = a.Index,
                                  Description = a.Description,
                                  IsEnable = a.IsEnable,
                                  SuccessRadio = a.SuccessRatio,
                                  SBUId = a.SBUId,
                              }).AsQueryable();

            if (!string.IsNullOrEmpty(search.Code))
            {
                dataQuerys = dataQuerys.Where(u => u.Code.ToUpper().Contains(search.Code.ToUpper()) || u.Name.ToUpper().Contains(search.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(search.SBUId))
            {
                dataQuerys = dataQuerys.Where(u => u.SBUId.Equals(search.SBUId));
            }
            searchResult.TotalItem = dataQuerys.Count();
            var listResult = SQLHelpper.OrderBy(dataQuerys, search.OrderBy, search.OrderType).Skip((search.PageNumber - 1) * search.PageSize).Take(search.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public object GetQuoteInfo(QuoteStepModel model)
        {
            var resultInfo = db.QuotationSteps.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new QuoteStepModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                SBUId = p.SBUId,
                IsEnable = p.IsEnable,
                SuccessRadio = p.SuccessRatio,
                Index = p.Index,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate,
            }).FirstOrDefault();

            return resultInfo;
        }

        public object GetIndex()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.QuotationSteps.AsNoTracking()
                                 orderby a.Index ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Exten = a.Index.ToString(),
                                     Index = a.Index
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    ComboboxResult addFirstIndex = new ComboboxResult();
                    addFirstIndex.Id = "";
                    addFirstIndex.Name = "";
                    addFirstIndex.Exten = "1";
                    addFirstIndex.Index = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = db.QuotationSteps.AsNoTracking().Select(b => b.Index).Max();
                    ComboboxResult addIndex = new ComboboxResult();
                    addIndex.Id = "";
                    addIndex.Name = "";
                    addIndex.Exten = (maxIndex + 1).ToString();
                    addIndex.Index = maxIndex + 1;
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public void CreateQuote(QuoteStepModel model)
        {
            if (db.QuotationSteps.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.QuoteStep);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                var indexs = db.QuotationSteps.ToList();
                var maxIndex = 1;
                if (indexs.Count > 0)
                {
                    maxIndex = indexs.Select(a => a.Index).Max();
                }

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listQuote = db.QuotationSteps.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listQuote.Count() > 0 && listQuote != null)
                    {
                        foreach (var item in listQuote)
                        {
                            var updateQuote = db.QuotationSteps.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                            updateQuote.Index++;
                        }
                    }
                }
                try
                {
                    QuotationStep QuoteNew = new QuotationStep
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        Index = model.Index,
                        Description = model.Description.Trim(),
                        IsEnable = model.IsEnable,
                        SuccessRatio = model.SuccessRadio,
                        SBUId = model.SBUId,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.UpdateBy,
                        UpdateDate = DateTime.Now,
                    };
                    db.QuotationSteps.Add(QuoteNew);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, QuoteNew.Name, QuoteNew.Id, Constants.LOG_Unit);

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

        public void UpdateQuote(QuoteStepModel model)
        {
            var quote = db.QuotationSteps.Where(a => a.Id != model.Id).ToList();
            foreach (var q in quote)
            {
                if (q.Code == model.Code)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.QuoteStep);
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var checkQuote = db.QuotationSteps.Where(b => b.Index == model.Index).FirstOrDefault();
                    if (checkQuote != null)
                    {
                        var newQuote = db.QuotationSteps.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                        int oldInext = newQuote.Index;

                        if (checkQuote.Index < newQuote.Index)
                        {
                            var listQuoteChange1 = db.QuotationSteps.Where(a => a.Index > checkQuote.Index && a.Index < newQuote.Index);
                            if (listQuoteChange1.Count() > 0)
                            {
                                foreach (var item in listQuoteChange1)
                                {
                                    item.Index++;
                                }

                            }
                            checkQuote.Index++;
                        }

                        if (checkQuote.Index > newQuote.Index)
                        {
                            var listQuoteChange = db.QuotationSteps.Where(a => a.Index > newQuote.Index && a.Index < checkQuote.Index);
                            if (listQuoteChange.ToList().Count() > 0)
                            {
                                foreach (var item in listQuoteChange)
                                {
                                    item.Index--;
                                }
                            }
                            checkQuote.Index = checkQuote.Index - 1;
                        }
                        newQuote.Code = model.Code;
                        newQuote.Index = model.Index;
                        newQuote.IsEnable = model.IsEnable;
                        newQuote.Name = model.Name.Trim();
                        newQuote.Description = model.Description.Trim();
                        newQuote.SuccessRatio = model.SuccessRadio;
                        newQuote.SBUId = model.SBUId;
                        newQuote.UpdateBy = model.UpdateBy;
                        newQuote.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.Id))
                        {
                            var quoteData = db.QuotationSteps.FirstOrDefault(x => x.Id.Equals(model.Id));
                            if (quoteData != null)
                            {
                                quoteData.Code = model.Code;
                                quoteData.Name = model.Name.Trim();
                                quoteData.Description = model.Description.Trim();
                                quoteData.IsEnable = model.IsEnable;
                                quoteData.SuccessRatio = model.SuccessRadio;
                                quoteData.SBUId = model.SBUId;
                                quoteData.UpdateBy = model.UpdateBy;
                                quoteData.UpdateDate = DateTime.Now;
                            }
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

        public void CreateIndex(SearchQuoteStepModel model)
        {
            for (var i = 0; i < model.ListQuote.Count; i++)
            {
                string id = model.ListQuote[i].Id;
                var Quote = db.QuotationSteps.Where(r => r.Id.Equals(id)).FirstOrDefault();
                if (Quote != null)
                {
                    Quote.Index = i + 1;
                }
            }

            db.SaveChanges();

        }

        public void DeleteQuote(SearchQuoteStepModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var maxIndex = db.QuotationSteps.AsNoTracking().Select(a => a.Index).Max();

                if (model.Index <= maxIndex)
                {
                    int modelIndex = model.Index;
                    var listquote = db.QuotationSteps.AsNoTracking().Where(b => b.Index >= modelIndex).ToList();
                    if (listquote.Count() > 0 && listquote != null)
                    {
                        foreach (var items in listquote)
                        {
                            if (!items.Id.Equals(model.Id))
                            {
                                var updateQuote = db.QuotationSteps.Where(r => r.Id.Equals(items.Id)).FirstOrDefault();
                                updateQuote.Index--;
                            }

                        }
                    }
                }

                try
                {
                    var quote1 = db.QuotationSteps.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (quote1 == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuoteStep);
                    }
                    db.QuotationSteps.Remove(quote1);

                    var listStepInQuotation = db.StepInQuotations.Where(a => a.QuotationStepId.Equals(model.Id)).ToList();
                    if (listStepInQuotation.Count > 0)
                    {
                        foreach(var step in listStepInQuotation)
                        {
                            var delStep = db.StepInQuotations.FirstOrDefault(a => a.Id.Equals(step.Id));
                            var listPlan = db.QuotationPlans.Where(a => a.StepInQuotationId.Equals(delStep.Id)).ToList();
                            foreach(var plan in listPlan)
                            {
                                var delPlan = db.QuotationPlans.FirstOrDefault(a => a.Id.Equals(plan.Id));
                                db.QuotationPlans.Remove(delPlan);
                            }    
                            db.StepInQuotations.Remove(delStep);

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
