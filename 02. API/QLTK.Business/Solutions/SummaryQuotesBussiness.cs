using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Quotation;
using NTS.Model.Repositories;
using NTS.Model.Solution;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Solutions
{
    public class SummaryQuotesBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<QuotationSearchModel> GetAllQuotationInfo(QuotationSearchModel modelSearch)
        {
            SearchResultModel<QuotationSearchModel> searchResult = new SearchResultModel<QuotationSearchModel>();

            try
            {
                var dataQuery = (from a in db.Quotations.AsNoTracking()
                                 join t in db.CustomerRequirements.AsNoTracking() on a.CustomerRequirementId equals t.Id into ta
                                 from t in ta.DefaultIfEmpty()
                                 join b in db.Customers.AsNoTracking() on t.CustomerId equals b.Id into ab
                                 from b in ab.DefaultIfEmpty()
                                 join c in db.CustomerTypes.AsNoTracking() on b.CustomerTypeId equals c.Id into bc
                                 from c in bc.DefaultIfEmpty()
                                 join d in db.CustomerDomains.AsNoTracking() on t.DomainId equals d.Id into td
                                 from d in td.DefaultIfEmpty()
                                 join e in db.Jobs.AsNoTracking() on d.JobId equals e.Id into ed
                                 from e in ed.DefaultIfEmpty()
                                 join f in db.SBUs.AsNoTracking() on a.SBUId equals f.Id into fa
                                 from f in fa.DefaultIfEmpty()
                                 join g in db.Employees.AsNoTracking() on t.Reciever equals g.Id into tg
                                 from g in tg.DefaultIfEmpty()
                                 join i in db.Departments.AsNoTracking() on t.DepartmentRequest equals i.Id into ti
                                 from i in ti.DefaultIfEmpty()
                                 join h in db.Employees.AsNoTracking() on t.Petitioner equals h.Id into th
                                 from h in th.DefaultIfEmpty()
                                 select new QuotationSearchModel
                                 {
                                     Code = t.Code,
                                     CustomerName = b.Name,
                                     CustomerCode = b.Code,
                                     CustomerType = c.Name,
                                     JobName = e.Name,
                                     Province = b.ProvinceId == null ? b.ProvinceId : db.Provinces.FirstOrDefault(t => t.Id.Equals(b.ProvinceId)).Name,
                                     NumberQuotation = a.Code,
                                     QuotationDate = a.QuotationDate,
                                     QuotationPrice = a.QuotationPrice,
                                     ExpectedPrice = a.ExpectedPrice,
                                     ImplementationDate = a.ImplementationDate,
                                     AdvanceRate = a.AdvanceRate,
                                     SuccessRate = a.SuccessRate,
                                     QuotationStatus = a.Status,
                                     QuotationId = a.Id,
                                     SBUName = f.Name,
                                     SBUId = f.Id,
                                     PetitionerName = h.Name,//NV phụ trách BG => YCKH
                                     EmployeeId = h.Id,
                                     DepartmentRequestName = i.Name,
                                     DepartmentId = i.Id,
                                     RecieverName = g.Name,//NV tiếp nhận BG =>
                                     CreateDate = a.CreateDate,
                                     DateFrom = a.QuotationDate,
                                     DateTo = a.QuotationDate,
                                     Status = a.Status,
                                     IsNotFullSchedule = db.QuotationPlans.Where(r => !r.PlanDueDate.HasValue && r.QuotationId.Equals(a.Id)).Any(),
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Code))
                {
                    dataQuery = dataQuery.Where(r => r.NumberQuotation.ToUpper().Contains(modelSearch.Code.ToUpper()));
                }
                if (!string.IsNullOrEmpty(modelSearch.CustomerName))
                {
                    dataQuery = dataQuery.Where(r => r.CustomerName.ToUpper().Contains(modelSearch.CustomerName.ToUpper()) || r.CustomerCode.ToUpper().Contains(modelSearch.CustomerName.ToUpper()));
                }
                if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
                {
                    dataQuery = dataQuery.Where(r => r.DepartmentId.Equals(modelSearch.DepartmentId));
                }
                if (!string.IsNullOrEmpty(modelSearch.SBUId))
                {
                    dataQuery = dataQuery.Where(r => r.SBUId.Equals(modelSearch.SBUId));
                }
                if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
                {
                    dataQuery = dataQuery.Where(r => r.EmployeeId.Equals(modelSearch.EmployeeId));
                }
                if (modelSearch.DateFrom.HasValue)
                {
                    dataQuery = dataQuery.Where(u => u.QuotationDate >= modelSearch.DateFrom);
                }
                if (modelSearch.DateTo.HasValue)
                {
                    dataQuery = dataQuery.Where(u => u.QuotationDate <= modelSearch.DateTo);
                }
                if (modelSearch.Status != -1)
                {
                    if(modelSearch.Status == 2)
                    {
                        dataQuery = dataQuery.Where(r => r.IsNotFullSchedule.Equals(true));
                    }
                    else
                    {
                        dataQuery = dataQuery.Where(r => r.Status.Equals(modelSearch.Status));
                    }
                }
                if (modelSearch.QuotationPrice != null)
                {
                    if (modelSearch.QuotationPriceType == 1)
                    {
                        dataQuery = dataQuery.Where(u => u.QuotationPrice == modelSearch.QuotationPrice);
                    }
                    else if (modelSearch.QuotationPriceType == 2)
                    {
                        dataQuery = dataQuery.Where(u => u.QuotationPrice > modelSearch.QuotationPrice);
                    }
                    else if (modelSearch.QuotationPriceType == 3)
                    {
                        dataQuery = dataQuery.Where(u => u.QuotationPrice >= modelSearch.QuotationPrice);
                    }
                    else if (modelSearch.QuotationPriceType == 4)
                    {
                        dataQuery = dataQuery.Where(u => u.QuotationPrice < modelSearch.QuotationPrice);
                    }
                    else if (modelSearch.QuotationPriceType == 5)
                    {
                        dataQuery = dataQuery.Where(u => u.QuotationPrice <= modelSearch.QuotationPrice);
                    }
                }

                var maxData = (from a in db.QuotationSteps.AsNoTracking()
                               join e in db.SBUs.AsNoTracking() on a.SBUId equals e.Id into ae
                               from e in ae.DefaultIfEmpty()
                               where a.IsEnable == true
                               group e by a.SBUId into g
                               select g.Count()).ToList();
                int max = 0;
                if (maxData.Count > 0)
                {
                    max = maxData.Max();
                }

                searchResult.TotalItem = dataQuery.Count();

                var listResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

                searchResult.ListResult = listResult;

                ListQuotationStep itemQuotationStep = new ListQuotationStep();
                List<ListQuotationStep> listData = new List<ListQuotationStep>();
                var listQuotationPlan = db.QuotationPlans.ToList();
                ListStatusQuotationProcess itemStatusQuotationProcess = new ListStatusQuotationProcess();
                var listStep = db.StepInQuotations.ToList();
                int TotalWork = 0; //Tổng lượng CV phải làm
                int WorkInProgress = 0; //SL còn phải làm
                int WorkLate = 0; //SL trễ

                int countSuccessRate = 0;

                foreach (var parent in listResult)
                {
                    foreach(var plan in listQuotationPlan)
                    {
                        if(parent.QuotationId == plan.QuotationId && plan.Status != 4)
                        {
                            TotalWork = TotalWork + 1;
                        }
                        if (parent.QuotationId == plan.QuotationId && (plan.Status == 1 || plan.Status == 2))
                        {
                            WorkInProgress = WorkInProgress + 1;
                        }
                        if (parent.QuotationId == plan.QuotationId && plan.Status == 4)
                        {
                            WorkLate = WorkLate + 1;
                        }
                    }

                    itemStatusQuotationProcess = new ListStatusQuotationProcess();
                    itemStatusQuotationProcess = new ListStatusQuotationProcess
                    {
                        TotalWork = TotalWork,
                        WorkInProgress = WorkInProgress,
                        WorkLate = WorkLate,
                    };
                    if (itemStatusQuotationProcess != null)
                    {
                        parent.ListStatusQuotationProcess.Add(itemStatusQuotationProcess);
                    }

                    TotalWork = 0;
                    WorkInProgress = 0;
                    WorkLate = 0;

                    var list = db.QuotationSteps.Where(a => a.SBUId.Equals(parent.SBUId) && a.IsEnable == true).OrderBy(b => b.Index).ToList();

                    listData = new List<ListQuotationStep>();
                    listData = (from d in db.StepInQuotations.AsNoTracking()
                                    join f in db.QuotationSteps.AsNoTracking() on d.QuotationStepId equals f.Id into df
                                    from f in df.DefaultIfEmpty()
                                    join e in db.Quotations.AsNoTracking() on d.QuotationId equals e.Id into de
                                    from e in de.DefaultIfEmpty()
                                    where e.Id.Equals(parent.QuotationId) && f.IsEnable == true
                                    orderby d.Index
                                    select new ListQuotationStep
                                    {
                                        QuotesId = d.Id,
                                        Name = f.Name,
                                        Code = e.Code,
                                        QuotationStepId = f.Id,
                                        Status = d.Status,
                                    }).ToList();

                    foreach (var x in list)
                    {
                        itemQuotationStep = new ListQuotationStep();
                        itemQuotationStep = listData.FirstOrDefault(a => a.QuotationStepId.Equals(x.Id));
                        if (itemQuotationStep != null)
                        {
                            parent.ListQuotationStep.Add(itemQuotationStep);
                        }
                        else
                        {
                            parent.ListQuotationStep.Add(new ListQuotationStep());
                        }                 

                    }

                    if (parent.ListQuotationStep.Count() < max)
                    {
                        for (var i = 0; i < max - parent.ListQuotationStep.Count();)
                        {
                            parent.ListQuotationStep.Add(new ListQuotationStep());
                        }
                    }

                    foreach(var s in listStep)
                    {
                        if(s.QuotationId == parent.QuotationId && s.Status == 3)
                        {
                            countSuccessRate = countSuccessRate + 1;
                        }    
                    }
                    if(countSuccessRate > 0)
                    {
                        int CountStepInQuotation = listStep.Where(a => a.QuotationId.Equals(parent.QuotationId)).Count();
                        parent.SuccessRate = 100 / CountStepInQuotation * countSuccessRate;

                        var addSuccessRate = db.Quotations.FirstOrDefault(a => a.Id.Equals(parent.QuotationId));
                        addSuccessRate.SuccessRate = 100 / CountStepInQuotation * countSuccessRate;

                        db.SaveChanges();
                    }    
                    
                    countSuccessRate = 0;
                }
                return searchResult;
            }

            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }

        }

        public SearchResultModel<QuotationSearchModel> GetQuotationByCustomerId(QuotationSearchModel modelSearch, string CustomerId)
        {
            SearchResultModel<QuotationSearchModel> searchResult = new SearchResultModel<QuotationSearchModel>();

            try
            {
                var dataQuery = (from a in db.Quotations.AsNoTracking()
                                 join t in db.CustomerRequirements.AsNoTracking() on a.CustomerRequirementId equals t.Id into ta
                                 from t in ta.DefaultIfEmpty()
                                 join f in db.SBUs.AsNoTracking() on a.SBUId equals f.Id into fa
                                 from f in fa.DefaultIfEmpty()
                                 join o in db.Departments.AsNoTracking() on t.DepartmentRequest equals o.Id into tf
                                 from o in tf.DefaultIfEmpty()
                                 join b in db.Employees.AsNoTracking() on t.Petitioner equals b.Id
                                 where a.CustomerId.Equals(CustomerId)
                                 select new QuotationSearchModel
                                 {
                                     NumberQuotation = a.Code,
                                     QuotationDate = a.QuotationDate,
                                     QuotationPrice = a.QuotationPrice,
                                     ExpectedPrice = a.ExpectedPrice,
                                     ImplementationDate = a.ImplementationDate,
                                     AdvanceRate = a.AdvanceRate,
                                     SuccessRate = a.SuccessRate,
                                     QuotationStatus = a.Status,
                                     QuotationId = a.Id,
                                     CreateDate = a.CreateDate,
                                     SBUId = f.Id,
                                     EmployeeChargeName = b.Name,
                                     DepartmentName = o.Name,
                                 }).AsQueryable();

                var maxData = (from a in db.QuotationSteps.AsNoTracking()
                               join e in db.SBUs.AsNoTracking() on a.SBUId equals e.Id into ae
                               from e in ae.DefaultIfEmpty()
                               where a.IsEnable == true
                               group e by a.SBUId into g
                               select g.Count()).ToList();
                int max = 0;
                if (maxData.Count > 0)
                {
                    max = maxData.Max();
                }

                searchResult.TotalItem = dataQuery.Count();

                var listResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

                searchResult.ListResult = listResult;

                ListQuotationStep itemQuotationStep = new ListQuotationStep();
                List<ListQuotationStep> listData = new List<ListQuotationStep>();
                var listQuotationPlan = db.QuotationPlans.ToList();
                ListStatusQuotationProcess itemStatusQuotationProcess = new ListStatusQuotationProcess();
                var listStep = db.StepInQuotations.ToList();
                int TotalWork = 0; //Tổng lượng CV phải làm
                int WorkInProgress = 0; //SL còn phải làm
                int WorkLate = 0; //SL trễ

                //int countSuccessRate = 0;

                foreach (var parent in listResult)
                {
                    foreach (var plan in listQuotationPlan)
                    {
                        if (parent.QuotationId == plan.QuotationId && plan.Status != 4)
                        {
                            TotalWork = TotalWork + 1;
                        }
                        if (parent.QuotationId == plan.QuotationId && (plan.Status == 1 || plan.Status == 2))
                        {
                            WorkInProgress = WorkInProgress + 1;
                        }
                        if (parent.QuotationId == plan.QuotationId && plan.Status == 4)
                        {
                            WorkLate = WorkLate + 1;
                        }
                    }

                    itemStatusQuotationProcess = new ListStatusQuotationProcess();
                    itemStatusQuotationProcess = new ListStatusQuotationProcess
                    {
                        TotalWork = TotalWork,
                        WorkInProgress = WorkInProgress,
                        WorkLate = WorkLate,
                    };
                    if (itemStatusQuotationProcess != null)
                    {
                        parent.ListStatusQuotationProcess.Add(itemStatusQuotationProcess);
                    }

                    TotalWork = 0;
                    WorkInProgress = 0;
                    WorkLate = 0;

                    var list = db.QuotationSteps.Where(a => a.SBUId.Equals(parent.SBUId) && a.IsEnable == true).OrderBy(b => b.Index).ToList();

                    listData = new List<ListQuotationStep>();
                    listData = (from d in db.StepInQuotations.AsNoTracking()
                                join f in db.QuotationSteps.AsNoTracking() on d.QuotationStepId equals f.Id into df
                                from f in df.DefaultIfEmpty()
                                join e in db.Quotations.AsNoTracking() on d.QuotationId equals e.Id into de
                                from e in de.DefaultIfEmpty()
                                where e.Id.Equals(parent.QuotationId) && f.IsEnable == true
                                orderby d.Index
                                select new ListQuotationStep
                                {
                                    QuotesId = d.Id,
                                    Name = f.Name,
                                    Code = e.Code,
                                    QuotationStepId = f.Id,
                                    Status = d.Status,
                                }).ToList();

                    foreach (var x in list)
                    {
                        itemQuotationStep = new ListQuotationStep();
                        itemQuotationStep = listData.FirstOrDefault(a => a.QuotationStepId.Equals(x.Id));
                        if (itemQuotationStep != null)
                        {
                            parent.ListQuotationStep.Add(itemQuotationStep);
                        }
                        else
                        {
                            parent.ListQuotationStep.Add(new ListQuotationStep());
                        }
                    }

                    if (parent.ListQuotationStep.Count() < max)
                    {
                        for (var i = 0; i < max - parent.ListQuotationStep.Count();)
                        {
                            parent.ListQuotationStep.Add(new ListQuotationStep());
                        }
                    }
                }
                return searchResult;
            }

            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }

        }

        public List<QuoteStepModel> GetQuotationPlan(string Id)
        {
            var listData = (from c in db.QuotationPlans.AsNoTracking()
                        join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id into cd
                        from d in cd.DefaultIfEmpty()
                        join e in db.Departments.AsNoTracking() on d.DepartmentId equals e.Id into de
                        from e in de.DefaultIfEmpty()
                        where c.QuotationId.Equals(Id)
                        orderby c.Name
                        select new QuotationPlanModel
                        {
                            Id = c.Id,
                            ParentId = c.StepInQuotationId,
                            QuotationPlanName = c.Name,
                            EmployeeName = d.Name,
                            DepartmentName = e.Name,
                            PlanDueDate = c.PlanDueDate,
                            ActualEndDate = c.ActualEndDate,
                            Status = c.Status,
                            Description = c.Descripton,
                            DoneRatio = c.DoneRatio,
                        }).ToList();

            var data = (from a in db.StepInQuotations.AsNoTracking()
                        join b in db.QuotationSteps.AsNoTracking() on a.QuotationStepId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        where a.QuotationId.Equals(Id) && b.IsEnable == true
                        orderby a.Index
                        select new QuoteStepModel
                        {
                            StepInQuotationId = a.Id,
                            ParentId = null,
                            Name = b.Name,

                        }).ToList();

            foreach (var parent in data)
            {
                parent.Listchild = listData.Where(a => a.ParentId.Equals(parent.StepInQuotationId)).ToList();
            }
            foreach (var parent in data)
            {
                parent.ListchildInProgress = listData.Where(a => a.ParentId.Equals(parent.StepInQuotationId) && (a.Status != 3)).ToList();
            }

            return data;
        }

        public object GetCustomerRequire()
        {
            var data = (from a in db.Customers.AsNoTracking()
                        join b in db.CustomerRequirements.AsNoTracking() on a.Id equals b.CustomerId
                        select new 
                        {
                            CustomerId = a.Id,
                            CustomerName = a.Name,
                            CustomerCode = a.Code,
                        }).Distinct().ToList();
            return data;
        }

        public object GetQuotationById(string quotationId)
        {
            var data = db.Quotations.AsNoTracking().Where(x => x.Id == quotationId).FirstOrDefault();

            var departmentName = (from a in db.Departments.AsNoTracking()
                                  join b in db.SBUs.AsNoTracking() on a.SBUId equals b.Id into ab
                                  from b in ab.DefaultIfEmpty()
                                  join c in db.Quotations.AsNoTracking() on b.Id equals c.SBUId into bc
                                  from c in bc.DefaultIfEmpty()
                                  where c.Id.Equals(quotationId)
                                  select new
                                  {
                                      a.Name,
                                  }).FirstOrDefault();

            var ListQuotationPlan = (from a in db.StepInQuotations.AsNoTracking()
                                     join b in db.Quotations.AsNoTracking() on a.QuotationId equals b.Id into ab
                                     from b in ab.DefaultIfEmpty()
                                     join d in db.QuotationSteps.AsNoTracking() on a.QuotationStepId equals d.Id into ad
                                     from d in ad.DefaultIfEmpty()
                                     where b.Id.Equals(quotationId) && d.IsEnable == true
                                     orderby a.Index
                                     select new QuotationStepByPlanModel
                                     {
                                         QuotesId = a.Id,
                                         QuotesName = d.Name,
                                         QuotesCode = d.Code,
                                         SuccessRatio = a.Rate,
                                         PlanDueDate = null,
                                         ActualEndDate = null,
                                     }).ToList();

            var count = 0;

            foreach(var item in ListQuotationPlan)
            {
                var maxDate = db.QuotationPlans.Where(a => a.QuotationId.Equals(quotationId) && a.StepInQuotationId.Equals(item.QuotesId)).Select(b => b.PlanDueDate).Max();

                item.PlanDueDate = maxDate;

                var listActualDate = db.QuotationPlans.Where(a => a.QuotationId.Equals(quotationId) && a.StepInQuotationId.Equals(item.QuotesId) && a.Status == 3).ToList();
                var CountList = listActualDate.Count();
                foreach (var i in listActualDate)
                {
                    if(i.ActualEndDate.HasValue)
                    {
                        count = count + 1;
                    }    
                }   
                if(CountList == count && CountList != 0)
                {
                    var maxActualDate = db.QuotationPlans.Where(a => a.QuotationId.Equals(quotationId) && a.StepInQuotationId.Equals(item.QuotesId)).Select(b => b.ActualEndDate).Max();

                    item.ActualEndDate = maxActualDate;

                    count = 0;
                }
                else
                {
                    count = 0;
                }
            }

            var dataDocument = (from a in db.Attachments.AsNoTracking()
                                join b in db.Quotations.AsNoTracking() on a.ObjectId equals b.Id into ab
                                from b in ab.DefaultIfEmpty()
                                where b.Id.Equals(quotationId)
                                select new ListQuotationDocument
                                {
                                    Id = a.Id,
                                    ObjectId = b.Id,
                                    Name = a.Name,
                                    FilePath = a.FilePath,
                                    FileName = a.FileName,
                                    Size = a.Size,
                                    Extention = a.Extention,
                                    Thumbnail = a.Thumbnail,
                                    HashValue = a.HashValue,
                                    Description = a.Description, 
                                    CreateBy = a.CreateBy,
                                    UpdateBy = a.UpdateBy,
                                    CreateDate = a.CreateDate,
                                    UpdateDate = a.UpdateDate
                                }).ToList();

            var dataStepInQuotation = (from a in db.StepInQuotations.AsNoTracking()
                                       join b in db.Quotations.AsNoTracking() on a.QuotationId equals b.Id into ab
                                       where a.QuotationId.Equals(quotationId)
                                       select a).ToList();

            return new
            {
                data,
                departmentName,
                ListQuotationPlan,
                dataDocument,
                dataStepInQuotation
            };
        }

        public object GetFileInfor(string quotationId)
        {
            var data = (from a in db.Attachments.AsNoTracking()
                        where a.ObjectId.Equals(quotationId)
                        select new
                        {
                            Id = a.Id,
                            FileName = a.FileName,
                            Description = a.Description,
                        }).ToList();
            return data;
        }

        public object GetCustomerById(string customerId)
        {
            var data = (from a in db.CustomerRequirements.AsNoTracking()
                        where a.CustomerId.Equals(customerId)
                        select new 
                        {
                            CustomerRequirementId = a.Id,
                            NumberRequire = a.Code,
                        }).ToList();

            var EmployeeCharge = (from a in db.Customers.AsNoTracking()
                                  where a.Id.Equals(customerId)
                                  join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                                  select new
                                  {
                                      CustomerCode = a.Code,
                                      EmployeeChargeName = b.Name,
                                      EmployeeChargeCode = b.Code,
                                      EmployeeChargePhone = b.PhoneNumber,
                                      EmployeeChargeEmail = b.Email,
                                      EmployeeChargeId = b.Id,
                                  }).FirstOrDefault();

            var CusContact = (from a in db.Customers.AsNoTracking()
                              where a.Id.Equals(customerId)
                              join b in db.CustomerContacts.AsNoTracking() on a.Id equals b.CustomerId
                              select new
                              {
                                  CusContactId = a.Id,
                              }).FirstOrDefault();
            return new
            {
                data,
                EmployeeCharge,
                CusContact
            };
        }

        public object GetCbbEmployee()
        {
                var ListModel = (from a in db.Employees.AsNoTracking()
                                 orderby a.Name ascending
                                 select new 
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Code = a.Code,
                                 }).ToList();
            return ListModel;

        }

        public object GetEmployeeCharge(string quotationId)
        {
            var EmployeeCharge =  (from a in db.Quotations.AsNoTracking()
                                   join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                                   from b in ab.DefaultIfEmpty()
                                   join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id into bc
                                   from c in bc.DefaultIfEmpty()
                                   where a.Id.Equals(quotationId)
                                  select new
                                  {
                                      Code = c.Code,
                                      Name = c.Name,
                                      Id = c.Id,
                                  }).FirstOrDefault();
            return EmployeeCharge;
        }

        public object GetRequireByNumberYCKH(string requireId)
        {
            var data = (from a in db.CustomerRequirements.AsNoTracking()
                        join c in db.CustomerRequirementContents.AsNoTracking() on a.Id equals c.CustomerRequirementId into bc
                        from c in bc.DefaultIfEmpty()
                        where a.Id.Equals(requireId)
                        select new
                        {
                            ContentRequire = c.Request,
                        }).FirstOrDefault();

            var Petitioner = (from a in db.CustomerRequirements.AsNoTracking()
                              where a.Id.Equals(requireId)
                              join b in db.Employees.AsNoTracking() on a.Petitioner equals b.Id
                              select new
                              {
                                  EmployeeName = b.Name,
                                  EmployeePhone = b.PhoneNumber,
                                  EmployeeEmail = b.Email,
                              }).FirstOrDefault();
            return new
            {
                data,
                Petitioner
            };
        }

        public object GetQuotesBySBU(string SBUid)
        {
            var data = (from a in db.QuotationSteps.AsNoTracking()
                        where a.SBUId.Equals(SBUid) && a.IsEnable == true
                        orderby a.Index
                        select new 
                        {
                            QuotesId = a.Id,
                            QuotesCode = a.Code,
                            QuotesName = a.Name,
                            SuccessRatio = a.SuccessRatio,
                            Index = a.Index,
                        }).ToList();
            return data;

        }
        public object CheckSoldQuotation(string quotationId)
        {
            var data = db.Quotations.Where(a => a.Id.Equals(quotationId)).Select(b => b.CustomerRequirementId).FirstOrDefault();
            var CheckYCKH = (from a in db.Quotations.AsNoTracking()
                             join b in db.CustomerRequirements.AsNoTracking() on a.CustomerRequirementId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             where a.CustomerRequirementId.Equals(data) && a.Status.Equals(1)
                             select new
                             {
                                 QuotationName = a.Code,
                                 YCKH = b.Code,
                                 Status = 1,
                                 QuotationId = a.Id
                             }).FirstOrDefault();
            if (CheckYCKH != null)
            {
                return CheckYCKH;
            }
            else return null;
        }

        public void ChangeStatusQuotation(string quotationId, string CreateBy)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    var newQ = db.Quotations.Where(r => r.Id.Equals(quotationId)).FirstOrDefault();
                    newQ.Status = 0;
                    newQ.UpdateDate = dateNow;
                    newQ.CreateBy = CreateBy;

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(quotationId, ex);
                }
            }
        }

        public void CreateQuotationPlan(QuotationPlanModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //  2 => "Đang triển khai" ? 3 => "Đã xong" ? 4 => "Trễ kế hoạch"
                    var data = db.StepInQuotations.Where(a => a.Id.Equals(model.StepInQuotationId)).FirstOrDefault();
                    int Status = 1;
                    if (model.PlanStartDate == null && model.PlanDueDate == null && model.ActualEndDate == null)
                    {
                        Status = 1;
                        data.Status = 2; //Đang triển khai
                    }
                    DateTime now = DateTime.Now;
                    if (model.PlanDueDate.HasValue)
                    {
                        DateTime PlanDueDatePlus = model.PlanDueDate.Value.AddDays(1);
                        if (model.PlanDueDate.HasValue && PlanDueDatePlus >= now && model.ActualEndDate == null)
                        {
                            Status = 2;
                            data.Status = 2; //Đang triển khai
                        }
                        if (model.PlanDueDate.HasValue && PlanDueDatePlus < now && model.ActualEndDate == null)
                        {
                            Status = 4;
                            data.Status = 4; //trễ
                        }
                    }    
                    
                    var dateNow = DateTime.Now;

                    QuotationPlan newQuotaionPlan = new QuotationPlan()
                    {
                        Id = Guid.NewGuid().ToString(),
                        QuotationId = model.QuotationId,
                        StepInQuotationId = model.StepInQuotationId,
                        EmployeeId = model.EmployeeId,
                        Name = model.Name,
                        Descripton = model.Description,
                        EstimateTime = model.EstimateTime,
                        DoneRatio = 0,
                        PlanDueDate = model.PlanDueDate,
                        PlanStartDate = model.PlanStartDate,
                        Status = Status,
                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = model.CreateBy
                    };

                    db.QuotationPlans.Add(newQuotaionPlan);
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

        public void UpdateQuotationPlan(QuotationPlanModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    var newQP = db.QuotationPlans.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    
                    var dateNow = DateTime.Now;
                    if (dateNow <= model.PlanDueDate && model.ActualEndDate == null)
                    {
                        newQP.Status = 2; //Đang triển khai
                    }
                    if (model.PlanDueDate == null && model.ActualEndDate == null)
                    {
                        newQP.Status = 1; //Chưa có kế hoạch
                    }
                    if (dateNow > model.PlanDueDate && model.ActualEndDate == null)
                    {
                        newQP.Status = 4; //Trễ kế hoạch
                    }    
                    newQP.Name = model.Name;
                    newQP.EmployeeId = model.EmployeeId;
                    newQP.EstimateTime = model.EstimateTime;
                    newQP.PlanDueDate = model.PlanDueDate;
                    newQP.PlanStartDate = model.PlanStartDate;
                    newQP.Descripton = model.Description;
                    newQP.UpdateDate = dateNow;
                    newQP.UpdateBy = model.CreateBy;

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

        public void AddQuote(QuotationModel model)
        {
            int AutoGenerateIndex;
            var Year = DateTime.Now.Year;
            var CustomerCode = model.CustomerCode;

            string QuoteId = Guid.NewGuid().ToString();

            var ResetYear = db.Quotations.AsNoTracking().FirstOrDefault() != null ? db.Quotations.AsNoTracking().Select(a => a.Year).Max() : 0;
            if (Year > ResetYear)
            {
                AutoGenerateIndex = 1;
            }
            else
            {
                var Index = db.Quotations.AsNoTracking().Where(a => a.Year.Equals(Year)).FirstOrDefault() != null ? db.Quotations.AsNoTracking().Where(a => a.Year.Equals(Year)).Select(a => a.AutoGenerateIndex).Max() : 0;
                AutoGenerateIndex = Index + 1;
            }


            var QuatationCode = CustomerCode + "." + (Year - 2000) + "." + AutoGenerateIndex;


            using (var trans = db.Database.BeginTransaction())
            {
                if (model.EffectiveLength < 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0089, TextResourceKey.EffectiveLength);
                }
                try
                {
                    var dateNow = DateTime.Now;
                    if (model.ListQuoteDocument.Count > 0)
                    {
                        CreatFile(model.ListQuoteDocument, model.CreateBy, QuoteId);
                    }

                    if (model.ListQuotesStep.Count() > 0)
                    {
                        foreach (var item in model.ListQuotesStep)
                        {

                            StepInQuotation quotationStep = new StepInQuotation()
                            {
                                Id = Guid.NewGuid().ToString(),
                                QuotationId = QuoteId,
                                QuotationStepId = item.QuotesId,
                                Index = item.Index,
                                Rate = item.SuccessRatio,
                                Status = 1, //Chưa có kế hoạch
                            };
                            db.StepInQuotations.Add(quotationStep);
                        }
                    }



                    Quotation newQuotaion = new Quotation()
                    {
                        Id = QuoteId,
                        CustomerRequirementId = model.CustomerRequirementId,
                        Code = QuatationCode,
                        QuotationDate = model.QuotationDate,
                        EffectiveLength = model.EffectiveLength,
                        EmployeeId = model.EmployeeId,
                        CustomerId = model.CustomerId,
                        CustomerContactId = model.CustomerContactId,
                        Warranty = model.Warranty,
                        Delivery = model.Delivery,
                        QuotationPrice = 0,
                        AdvanceRate = model.AdvanceRate,
                        SuccessRate = 0,
                        ExpectedPrice = model.ExpectedPrice,
                        ImplementationDate = model.ImplementationDate,
                        Status = model.Status,
                        SBUId = model.SBUid,
                        AutoGenerateIndex = AutoGenerateIndex,
                        //Attachments
                        PaymentMethod = model.PaymentMethod,
                        Description = model.Description,
                        Year = Year,

                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = model.CreateBy
                    };

                    db.Quotations.Add(newQuotaion);

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

        public void CreatFile(List<QuoteDocumentModel> model, string CreateBy, string Id)
        {
            var dateNow = DateTime.Now;

            if (model.Count() > 0)
            {
                foreach (var item in model)
                {

                    Attachment quotationDocument = new Attachment()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateBy = CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = CreateBy,
                        ObjectId = Id,
                        Name = item.Name,
                        FilePath = item.FilePath,
                        FileName = item.FileName,
                        Size = item.Size,
                        Extention = item.Extention,
                        Thumbnail = item.Thumbnail,
                        HashValue = item.HashValue,
                        Description = item.Description,
                    };
                    db.Attachments.Add(quotationDocument);

                }
            }
            db.SaveChanges();
        }

        public void UpdateQuote(QuotationModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (model.EffectiveLength < 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0089, TextResourceKey.EffectiveLength);
                }
                try
                {
                    var dateNow = DateTime.Now;
                    var newQ = db.Quotations.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newQ.QuotationDate = model.QuotationDate;
                    newQ.EffectiveLength = model.EffectiveLength;
                    newQ.EmployeeId = model.EmployeeId;
                    newQ.CustomerId = model.CustomerId;
                    newQ.CustomerContactId = model.CustomerContactId;
                    newQ.Warranty = model.Warranty;
                    newQ.Delivery = model.Delivery;
                    newQ.AdvanceRate = model.AdvanceRate;
                    newQ.ExpectedPrice = model.ExpectedPrice;
                    newQ.ImplementationDate = model.ImplementationDate;
                    newQ.Status = model.Status;
                    newQ.SBUId = model.SBUid;
                    newQ.PaymentMethod = model.PaymentMethod;
                    newQ.Description = model.Description;
                    newQ.CustomerRequirementId = model.CustomerRequirementId;
                    newQ.UpdateDate = dateNow;
                    newQ.UpdateBy = model.CreateBy;

                    if (model.ListQuoteDocument.Count > 0)
                    {
                        var proAt = db.Attachments.Where(a => a.ObjectId.Equals(model.Id)).ToList();

                        db.Attachments.RemoveRange(proAt);

                        Attachment attachment = new Attachment();
                        foreach (var item in model.ListQuoteDocument)
                        {
                            Attachment data = new Attachment()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CreateBy = model.CreateBy,
                                CreateDate = dateNow,
                                UpdateDate = dateNow,
                                UpdateBy = model.CreateBy,
                                ObjectId = model.Id,
                                Name = item.Name,
                                FilePath = item.FilePath,
                                FileName = item.FileName,
                                Size = item.Size,
                                Extention = item.Extention,
                                Thumbnail = item.Thumbnail,
                                HashValue = item.HashValue,
                                Description = item.Description,
                            };
                            db.Attachments.Add(data);
                        }
                    }

                    if(model.isShowChosse == true)
                    {
                        var stepQuotation = db.StepInQuotations.Where(x => x.QuotationId.Equals(model.Id)).ToList();
                        var listRemove = stepQuotation.ToList();
                        foreach (var item in stepQuotation)
                        {
                            if (model.ListQuotesStep.FirstOrDefault(a => item.QuotationStepId.Equals(a.QuotesId)) != null)
                            {
                                listRemove.Remove(item);

                            }
                            else
                            {
                                var delPlan = db.QuotationPlans.Where(a => a.StepInQuotationId.Equals(item.Id)).ToList();
                                if (delPlan.Count > 0)
                                {
                                    foreach (var a in delPlan)
                                    {
                                        db.QuotationPlans.Remove(a);
                                    }
                                }
                            }
                        }
                        if (listRemove.Count != stepQuotation.Count)
                        {
                            db.StepInQuotations.RemoveRange(listRemove);
                        }

                        StepInQuotation stepInQuotation = new StepInQuotation();
                        foreach (QuoteStepModel item in model.ListQuotesStep)
                        {
                            stepInQuotation = new StepInQuotation();
                            stepInQuotation = db.StepInQuotations.Where(a => a.QuotationId.Equals(model.Id) && a.QuotationStepId.Equals(item.QuotesId)).FirstOrDefault();
                            if (stepInQuotation != null)// Sửa
                            {
                                stepInQuotation.Rate = item.SuccessRatio;
                            }
                            else // Thêm
                            {
                                stepInQuotation = new StepInQuotation()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    QuotationId = model.Id,
                                    QuotationStepId = item.QuotesId,
                                    Index = item.Index,
                                    Rate = item.SuccessRatio,
                                    Status = 1,
                                };
                                db.StepInQuotations.Add(stepInQuotation);
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

        public void AddProduct(QuotationProductModel model)
        {
            var quote = db.QuotationProducts.Where(a => a.Id != model.Id && a.QuotationId == model.QuotationId).ToList();
            foreach (var q in quote)
            {
                if (q.Code == model.Code)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.QuotationProduct);
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    QuotationProduct newQP = new QuotationProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        QuotationId = model.QuotationId,
                        Code = model.Code,
                        Name = model.Name,
                        ObjectId = model.ObjectId,
                        ObjectType = model.ObjectType,
                        IndustryId = model.IndustryId,
                        ManufactureId = model.ManufactureId,
                        UnitId = model.UnitId,
                        Price = model.Price,
                        Quantity = model.Quantity,
                        Amount = model.Price * model.Quantity,
                        Description = model.Description,

                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = model.CreateBy
                    };

                    db.QuotationProducts.Add(newQP);

                    var data = db.Quotations.Where(a => a.Id.Equals(model.QuotationId)).FirstOrDefault();
                    data.QuotationPrice = data.QuotationPrice + newQP.Amount;

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

        public void UpdateProduct(QuotationProductModel model)
        {
            var quote = db.QuotationProducts.Where(a => a.Id != model.Id && a.QuotationId == model.QuotationId).ToList();
            foreach (var q in quote)
            {
                if (q.Code == model.Code)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.QuotationProduct);
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newQP = db.QuotationProducts.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    var dateNow = DateTime.Now;

                    var data = db.Quotations.FirstOrDefault(a => a.Id.Equals(model.QuotationId));
                    if(data != null)
                    {
                        var upQP = db.QuotationProducts.Where(a => a.QuotationId.Equals(data.Id)).ToList();
                        
                        if (model.Price != newQP.Price || model.Quantity != newQP.Quantity)
                        {
                            data.QuotationPrice = 0;
                            newQP.Price = model.Price;
                            newQP.Quantity = model.Quantity;
                            newQP.Amount = model.Price * model.Quantity;
                            foreach (var x in upQP)
                            {
                                data.QuotationPrice = data.QuotationPrice + x.Amount;
                            }
                        }
                    }

                    newQP.Code = model.Code;
                    newQP.Name = model.Name;
                    newQP.ObjectId = model.ObjectId;
                    newQP.ObjectType = model.ObjectType;
                    newQP.IndustryId = model.IndustryId;
                    newQP.ManufactureId = model.ManufactureId;
                    newQP.UnitId = model.UnitId;
                    newQP.Description = model.Description;
                    newQP.UpdateDate = dateNow;
                    newQP.UpdateBy = model.CreateBy;

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

        public void DeleteProduct(string Id)
        {
            var pro = db.QuotationProducts.FirstOrDefault(x => x.Id.Equals(Id));
            try
            {
                if (pro != null)
                {
                    db.QuotationProducts.Remove(pro);
                }

               
            }
            catch (Exception)
            {
                throw;
            }
            db.SaveChanges();
            var data = db.Quotations.FirstOrDefault(a => a.Id.Equals(pro.QuotationId));
            if (data != null)
            {
                var upQP = db.QuotationProducts.Where(a => a.QuotationId.Equals(data.Id)).ToList();
                decimal total = 0;
                foreach (var x in upQP)
                {
                    total = total + x.Amount;
                }
                data.QuotationPrice = total;
            }
            db.SaveChanges();
        }

        public void DeleteQuotation(string Id)
        {
            var pro = db.QuotationPlans.FirstOrDefault(x => x.Id.Equals(Id));
            
            try
            {
                if (pro != null)
                {
                    db.QuotationPlans.Remove(pro);
                }
            }
            catch (Exception)
            {
                throw;
            }
            db.SaveChanges();
        }

        public void DeleteQuotationByQuotationId(string Id)
        {
            var pro = db.Quotations.FirstOrDefault(x => x.Id.Equals(Id));
            var delStep = db.StepInQuotations.Where(a => a.QuotationId.Equals(Id)).ToList();
            var delPlan = db.QuotationPlans.Where(a => a.QuotationId.Equals(Id)).ToList();
            var delAttachment = db.Attachments.Where(a => a.ObjectId.Equals(Id)).ToList();
            try
            {
                if (pro != null)
                {
                    db.Quotations.Remove(pro);
                }
                if (delStep.Count > 0)
                {
                    foreach(var a in delStep)
                    {
                        db.StepInQuotations.Remove(a);
                    }    
                }
                if (delAttachment.Count > 0)
                {
                    foreach (var a in delAttachment)
                    {
                        db.Attachments.Remove(a);
                    }
                    
                }
                if (delPlan.Count > 0)
                {
                    foreach (var a in delPlan)
                    {
                        db.QuotationPlans.Remove(a);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            db.SaveChanges();
        }

        public object ChangeIndustry(string IndustryId)
        {
            var data = (from a in db.Industries.AsNoTracking()
                        where a.Id.Equals(IndustryId)
                        select new
                        {
                            IndustryName = a.Name,
                        }).FirstOrDefault();
            return data;
        }

        public object ChangeModule(string ObjectId)
        {
            var data = (from a in db.Modules.AsNoTracking()
                        where a.Id.Equals(ObjectId)
                        select new
                        {
                           Name = a.Name,
                           Code = a.Code
                        }).FirstOrDefault();
            return data;
        }

        public object ChangeProduct(string ObjectId)
        {
            var data = (from a in db.Products.AsNoTracking()
                        where a.Id.Equals(ObjectId)
                        select new
                        {
                            Name = a.Name,
                            Code = a.Code
                        }).FirstOrDefault();
            return data;
        }

        public object ChangeSaleProduct(string ObjectId)
        {
            var data = (from a in db.SaleProducts.AsNoTracking()
                        where a.Id.Equals(ObjectId)
                        select new
                        {
                            Name = a.VName,
                            Code = a.Model
                        }).FirstOrDefault();
            return data;
        }

        public object ChangeMaterial(string ObjectId)
        {
            var data = (from a in db.Materials.AsNoTracking()
                        where a.Id.Equals(ObjectId)
                        select new
                        {
                            Name = a.Name,
                            Code = a.Code
                        }).FirstOrDefault();
            return data;
        }

        public object GetQuotationProduct(string Id)
        {
            var data = (from a in db.QuotationProducts.AsNoTracking()
                        join c in db.Industries.AsNoTracking() on a.IndustryId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        join d in db.Units.AsNoTracking() on a.UnitId equals d.Id into ad
                        from d in ad.DefaultIfEmpty()
                        where a.QuotationId.Equals(Id)
                        orderby a.Code
                        select new QuotationProductModel
                        {
                            QuotationId = Id,
                            Id = a.Id,
                            Code = a.Code,
                            Name = a.Name,
                            ManufactureName = b.Name,
                            UnitName = d.Name,
                            Quantity = a.Quantity,
                            Price = a.Price,
                            Amount = a.Amount,
                            Description = a.Description,
                            IndustryName = c.Name,
                            ObjectType = a.ObjectType
                        }).ToList();

            decimal TotalAmount = 0;
            foreach (var x in data)
            {
                TotalAmount = TotalAmount + x.Amount;
            }
            return new
            {
                data,
                TotalAmount
            };
        }

        public object GetQuotationProductInfor(string Id)
        {
            var data = (from a in db.QuotationProducts.AsNoTracking()
                        join c in db.Industries.AsNoTracking() on a.IndustryId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        where a.Id.Equals(Id)
                        select new QuotationProductModel
                        {
                            QuotationId = a.QuotationId,
                            Id = a.Id,
                            Code = a.Code,
                            Name = a.Name,
                            ManufactureId = a.ManufactureId,
                            UnitId = a.UnitId,
                            Quantity = a.Quantity,
                            Price = a.Price,
                            Description = a.Description,
                            IndustryId = c.Id,
                            IndustryName = c.Name,
                            ObjectId = a.ObjectId,
                            ObjectType = a.ObjectType
                        }).FirstOrDefault();
            return new
            {
                data,
            };
        }

        public object GetQuotationPlanById(string Id)
        {
            var data = db.QuotationPlans.Where(a => a.Id.Equals(Id)).FirstOrDefault();
            return data;
        }

        public object GetListQuotationStep(string QuotationId)
        {
            var data = (from a in db.QuotationSteps.AsNoTracking()
                        join b in db.StepInQuotations.AsNoTracking() on a.Id equals b.QuotationId into ab
                        from b in ab.DefaultIfEmpty()
                        join c in db.Quotations.AsNoTracking() on b.QuotationId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        where c.Id.Equals(QuotationId) && a.IsEnable == true
                        select new
                        {
                            QuotationStepId = a.Id,
                            QuotationStepName = a.Name,
                        }).ToList();
            return data;
        }

        public string GetGroupInTemplate()
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BaoGiaThietBi_Import_Template.xlsx"));
            IWorksheet sheet0 = workbook.Worksheets[0];
            IWorksheet sheet1 = workbook.Worksheets[1];
            IWorksheet sheet2 = workbook.Worksheets[2];
            IWorksheet sheet3 = workbook.Worksheets[3];

            var listIndustry = db.Industries.AsNoTracking().Select(i => i.Code).ToList();
            var listManufacture = db.Manufactures.AsNoTracking().Select(i => i.Code).ToList();
            var listUnit = db.Units.AsNoTracking().Select(i => i.Name).ToList();

            sheet0.Range["E3:E1000"].DataValidation.DataRange = sheet1.Range["A1:A1000"];
            sheet0.Range["F3:F1000"].DataValidation.DataRange = sheet2.Range["A1:A1000"];
            sheet0.Range["G3:G1000"].DataValidation.DataRange = sheet3.Range["A1:A1000"];

            IRange iRangeData1 = sheet1.FindFirst("<industryCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData1.Text = iRangeData1.Text.Replace("<industryCode>", string.Empty);
            IRange iRangeData2 = sheet2.FindFirst("<manufactureCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData2.Text = iRangeData2.Text.Replace("<manufactureCode>", string.Empty);
            IRange iRangeData3 = sheet3.FindFirst("<unitCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData3.Text = iRangeData3.Text.Replace("<unitCode>", string.Empty);

            var listExportD = listIndustry.OrderBy(a => a).Select((o, i) => new
            {
                o,
            }); ;

            var listExportE = listManufacture.OrderBy(a => a).Select((o, i) => new
            {
                o,
            }); ;

            var listExportF = listUnit.OrderBy(a => a).Select((o, i) => new
            {
                o,
            }); ;

            sheet1.ImportData(listExportD, iRangeData1.Row, iRangeData1.Column, false);
            sheet2.ImportData(listExportE, iRangeData2.Row, iRangeData2.Column, false);
            sheet3.ImportData(listExportF, iRangeData3.Row, iRangeData3.Column, false);
            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BaoGiaThietBi_Import_Template" + ".xlsx");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "BaoGiaThietBi_Import_Template" + ".xlsx";

            return resultPathClient;
        }

        public void ImportFile(string userId, string QuotationId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }
            string ObjectType, ObjectId, Code, Name, IndustryId, ManufactureId, UnitId, Description, Price, Quantity;
            decimal Amout;
            string[] arrListQuotationProductGroup = { };
            var QuotationProducts = db.QuotationProducts.AsNoTracking();
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.EnableSheetCalculations();
            int rowCount = sheet.Rows.Count();
            List<QuotationProduct> list = new List<QuotationProduct>();
            // List<ManufactureInGroup> manufactureInGroups = new List<ManufactureInGroup>();
            QuotationProduct itemC;
            // ManufactureInGroup itemD;
            List<int> rowObjectType = new List<int>();// check có phải bắt buộc k
            List<int> rowName = new List<int>();
            List<int> rowIndustry = new List<int>();
            List<int> rowManufacture = new List<int>();
            List<int> rowUnit = new List<int>();
            List<int> rowAmount = new List<int>();
            List<int> rowPrice = new List<int>();
            List<int> rowQuantity = new List<int>();

            List<int> rowCheckName = new List<int>();// check tồn tại
            List<int> rowCheckQuantity = new List<int>();

            if (rowCount < 3)
            {
                throw NTSException.CreateInstance("File import không đúng. Chọn file khác");
            }

            try
            {
                for (int i = 3; i <= rowCount; i++)
                {

                    itemC = new QuotationProduct();
                    itemC.Id = Guid.NewGuid().ToString();
                    itemC.QuotationId = QuotationId;
                    ObjectType = sheet[i, 2].Value;
                    Name = sheet[i, 3].Value;
                    Code = sheet[i, 4].Value;
                    IndustryId = sheet[i, 5].Value;
                    ManufactureId = sheet[i, 6].Value;
                    UnitId = sheet[i, 7].Value;
                    Price = sheet[i, 8].Value;
                    Quantity = sheet[i, 9].Value;
                    Description = sheet[i, 10].Value;

                    if (string.IsNullOrEmpty(ObjectType) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Code) 
                        && string.IsNullOrEmpty(IndustryId) && string.IsNullOrEmpty(ManufactureId) && string.IsNullOrEmpty(UnitId)
                        && string.IsNullOrEmpty(Price) && string.IsNullOrEmpty(Quantity) && string.IsNullOrEmpty(Description))
                    {
                        continue;
                    }    

                    //ObjectType
                    try
                    {
                        if (!string.IsNullOrEmpty(ObjectType))
                        {
                            if (ObjectType.Equals("Nhập tay"))
                            {
                                itemC.ObjectType = -1;
                            }
                            if (ObjectType.Equals("Module"))
                            {
                                itemC.ObjectType = 1;
                            }
                            if (ObjectType.Equals("Thiết bị"))
                            {
                                itemC.ObjectType = 2;
                            }
                            if (ObjectType.Equals("Thư viện sản phẩm kinh doanh"))
                            {
                                itemC.ObjectType = 3;
                            }
                            if (ObjectType.Equals("Vật tư"))
                            {
                                itemC.ObjectType = 4;
                            }
                        }
                        else
                        {
                            rowObjectType.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowObjectType.Add(i);
                        continue;
                    }

                    //Industry
                    try
                    {
                        if (!string.IsNullOrEmpty(IndustryId))
                        {
                            itemC.IndustryId = db.Industries.Where(u => u.Code.Equals(IndustryId.Trim())).FirstOrDefault().Id;
                        }
                        else
                        {
                            rowIndustry.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowIndustry.Add(i);
                        continue;
                    }

                    //Manufacture
                    try
                    {
                        if (!string.IsNullOrEmpty(ManufactureId))
                        {
                            itemC.ManufactureId = db.Manufactures.Where(u => u.Code.Equals(ManufactureId.Trim())).FirstOrDefault().Id;
                        }
                        else
                        {
                            rowManufacture.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowManufacture.Add(i);
                        continue;
                    }

                    //Unit
                    try
                    {
                        if (!string.IsNullOrEmpty(UnitId))
                        {
                            itemC.UnitId = db.Units.Where(u => u.Name.Equals(UnitId.Trim())).FirstOrDefault().Id;
                        }
                        else
                        {
                            rowUnit.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowUnit.Add(i);
                        continue;
                    }

                    //Name
                    try
                    {
                        if (!string.IsNullOrEmpty(Name))
                        {
                            if (db.QuotationProducts.AsNoTracking().Where(o => o.Name.Equals(Name)).Count() > 0)
                            {
                                rowCheckName.Add(i);
                            }
                            else
                            {
                                itemC.Name = Name;
                            }
                        }
                        else
                        {
                            rowName.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowName.Add(i);
                        continue;
                    }


                    //Quantity
                    try
                    {
                        if (!string.IsNullOrEmpty(Quantity))
                        {
                            var check = int.Parse(Quantity);
                            if (check != null)
                            {
                                itemC.Quantity = check;
                            }
                            else
                            {
                                rowCheckQuantity.Add(i);
                            }
                        }
                        else
                        {
                            rowQuantity.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        if (itemC.Quantity == null)
                        {
                            rowQuantity.Add(i);
                        }
                        else
                        {
                            rowCheckQuantity.Add(i);
                        }

                        continue;
                    }

                    //Price
                    try
                    {
                        if (!string.IsNullOrEmpty(Price))
                        {
                            itemC.Price = Convert.ToDecimal(Price);
                        }
                        else
                        {
                            rowPrice.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowPrice.Add(i);
                        continue;
                    }

                    //Amout
                    try
                    {
                        if (!string.IsNullOrEmpty(Quantity) && !string.IsNullOrEmpty(Price))
                        {
                            itemC.Amount = itemC.Quantity * itemC.Price;
                        }
                        else
                        {
                            rowAmount.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowAmount.Add(i);
                        continue;
                    }

                    //Code
                    if (!string.IsNullOrEmpty(Code))
                    {
                        itemC.Code = Code;
                    }

                    //Description
                    if (!string.IsNullOrEmpty(Description))
                    {
                        itemC.Description = Description;
                    }

                    //
                    itemC.CreateBy = userId;
                    itemC.UpdateBy = userId;
                    itemC.CreateDate = DateTime.Now;
                    itemC.UpdateDate = DateTime.Now;

                    list.Add(itemC);

                }


                if (rowObjectType.Count > 0)
                {
                    throw NTSException.CreateInstance("Nhóm thiết bị dòng <" + string.Join(", ", rowObjectType) + "> không được phép để trống!");
                }
                if (rowName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên thiết bị dòng <" + string.Join(", ", rowName) + "> không được phép để trống!");
                }
                if (rowIndustry.Count > 0)
                {
                    throw NTSException.CreateInstance("Ngành hàng dòng <" + string.Join(", ", rowIndustry) + "> không được phép để trống!");
                }
                if (rowManufacture.Count > 0)
                {
                    throw NTSException.CreateInstance("Hãng sản xuất dòng <" + string.Join(", ", rowManufacture) + "> không được phép để trống!");
                }
                if (rowUnit.Count > 0)
                {
                    throw NTSException.CreateInstance("Đơn vị dòng <" + string.Join(", ", rowUnit) + "> không được phép để trống!");
                }
                if (rowPrice.Count > 0)
                {
                    throw NTSException.CreateInstance("Giá dòng <" + string.Join(", ", rowPrice) + "> không được phép để trống!");
                }
                if (rowQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", rowQuantity) + "> không được phép để trống!");
                }
                if (rowAmount.Count > 0)
                {
                    throw NTSException.CreateInstance("Thành tiền dòng <" + string.Join(", ", rowQuantity) + "> không thể tính toán được! Dữ liệu giá và số lượng không được phép để trống!");
                }
                if (rowCheckName.Count > 0)
                {
                    throw NTSException.CreateInstance("Tên thiết bị dòng <" + string.Join(", ", rowCheckName) + "> đã tồn tại. Nhập tên khác!");
                }

                if (rowCheckQuantity.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng thiết bị dòng <" + string.Join(", ", rowCheckQuantity) + "> không đúng định dạng. Nhập số lượng khác!");
                }

                #endregion
                db.QuotationProducts.AddRange(list);

                var data = db.Quotations.Where(a => a.Id.Equals(QuotationId)).FirstOrDefault();
                decimal TotalAmount = 0;
                foreach (var x in list)
                {
                    data.QuotationPrice = data.QuotationPrice + x.Amount;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ex;
            }
            workbook.Close();
            excelEngine.Dispose();
        }

        public string ExportExcel(QuotationSearchModel model, string QuoationId)
        {
            var dataQuery = (from a in db.Quotations.AsNoTracking()
                             join t in db.CustomerRequirements.AsNoTracking() on a.CustomerRequirementId equals t.Id into ta
                             from t in ta.DefaultIfEmpty()
                             join b in db.Customers.AsNoTracking() on t.CustomerId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join h in db.Employees.AsNoTracking() on t.Petitioner equals h.Id into th
                             from h in th.DefaultIfEmpty()
                             join g in db.Employees.AsNoTracking() on t.Reciever equals g.Id into tg
                             from g in tg.DefaultIfEmpty()
                             where a.Id.Equals(QuoationId)
                             select new QuotationSearchModel
                             {
                                 Code = t.Code, //Yêu cầu
                                 NumberQuotation = a.Code, //Số BG
                                 QuotationDate = a.QuotationDate, //Ngày BG
                                 PetitionerName = h.Name, //Người BG
                                 Phone = h.PhoneNumber, //SĐT người BG
                                 Email = h.Email, //Email người BG
                                 Delivery = a.Delivery, //Thời gian cung cấp
                                 Warranty = a.Warranty, //TG bảo hành
                                 PaymentMethod = a.PaymentMethod, //PT thanh toán
                                 EffectiveLength = a.EffectiveLength, //Hiệu lực báo giá
                                 RecieverName = g.Name, //Người nhận BG
                                 CustomerName = b.Name,//Tên người gửi
                                 CustomerAddress = b.Address,//Đchi
                                 CustomerTel = b.PhoneNumber,//Sđt
                                 CustomerTax = b.TaxCode//Tax
                             }).FirstOrDefault();
            if (dataQuery == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Quotation);
            }

            var dataQuotationProduct = (from a in db.QuotationProducts.AsNoTracking()
                        join c in db.Industries.AsNoTracking() on a.IndustryId equals c.Id into bc
                        from c in bc.DefaultIfEmpty()
                        join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        join d in db.Units.AsNoTracking() on a.UnitId equals d.Id into ad
                        from d in ad.DefaultIfEmpty()
                        where a.QuotationId.Equals(QuoationId)
                        orderby a.Code
                        select new QuotationProductModel
                        {
                            QuotationId = QuoationId,
                            Id = a.Id,
                            Code = a.Code,
                            Name = a.Name,
                            ManufactureName = b.Name,
                            UnitName = d.Name,
                            Quantity = a.Quantity,
                            Price = a.Price,
                            Amount = a.Amount,
                            Description = a.Description,
                            IndustryName = c.Name,
                            ObjectType = a.ObjectType,
                        }).ToList();

            decimal TotalAmount = 0;
            if (dataQuotationProduct.Count > 0)
            {
                foreach (var x in dataQuotationProduct)
                {
                    TotalAmount = TotalAmount + x.Amount;
                }
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Quotation_Template.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                IRange rangeCustomerName = sheet.FindFirst("<dataCustomerName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeCustomerName.Text = rangeCustomerName.Text.Replace("<dataCustomerName>", dataQuery.CustomerName);

                IRange rangeCustomerAddress = sheet.FindFirst("<dataCustomerAddress>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeCustomerAddress.Text = rangeCustomerAddress.Text.Replace("<dataCustomerAddress>", dataQuery.CustomerAddress);

                IRange rangeCustomerTel = sheet.FindFirst("<dataCustomerTel>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeCustomerTel.Text = rangeCustomerTel.Text.Replace("<dataCustomerTel>", dataQuery.CustomerTel);

                IRange rangeCustomerTax = sheet.FindFirst("<dataCustomerTax>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeCustomerTax.Text = rangeCustomerTax.Text.Replace("<dataCustomerTax>", dataQuery.CustomerTax);

                IRange rangeRecieverName = sheet.FindFirst("<dataRecieverName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeRecieverName.Text = rangeRecieverName.Text.Replace("<dataRecieverName>", dataQuery.RecieverName);

                IRange rangeNumberQuotation = sheet.FindFirst("<dataNumberQuotation>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeNumberQuotation.Text = rangeNumberQuotation.Text.Replace("<dataNumberQuotation>", dataQuery.NumberQuotation);

                IRange rangeQuotationDate = sheet.FindFirst("<dataQuotationDate>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeQuotationDate.Text = rangeQuotationDate.Text.Replace("<dataQuotationDate>", (dataQuery.QuotationDate.ToString("dd/MM/yyy")));

                IRange rangePetitionerNameQuotation = sheet.FindFirst("<dataPetitionerNameQuotation>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangePetitionerNameQuotation.Text = rangePetitionerNameQuotation.Text.Replace("<dataPetitionerNameQuotation>", dataQuery.PetitionerName);

                IRange rangePhoneQuotation = sheet.FindFirst("<dataPhoneQuotation>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangePhoneQuotation.Text = rangePhoneQuotation.Text.Replace("<dataPhoneQuotation>", dataQuery.Phone);

                IRange rangeEmailQuotation = sheet.FindFirst("<dataEmailQuotation>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeEmailQuotation.Text = rangeEmailQuotation.Text.Replace("<dataEmailQuotation>", dataQuery.Email);

                IRange rangeDelivery = sheet.FindFirst("<dataDelivery>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeDelivery.Text = rangeDelivery.Text.Replace("<dataDelivery>", dataQuery.Delivery);

                IRange rangeWarranty = sheet.FindFirst("<dataWarranty>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeWarranty.Text = rangeWarranty.Text.Replace("<dataWarranty>", dataQuery.Warranty);

                IRange rangePaymentMethod = sheet.FindFirst("<dataPaymentMethod>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangePaymentMethod.Text = rangePaymentMethod.Text.Replace("<dataPaymentMethod>", dataQuery.PaymentMethod);

                IRange rangeTotalProduct = sheet.FindFirst("<dataTotalProduct>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeTotalProduct.Text = rangeTotalProduct.Text.Replace("<dataTotalProduct>", TotalAmount.ToString("#,##"));

                IRange rangeEffectiveLength = sheet.FindFirst("<dataEffectiveLength>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeEffectiveLength.Text = rangeEffectiveLength.Text.Replace("<dataEffectiveLength>", dataQuery.EffectiveLength.ToString());

                IRange iRangeDataProduct = sheet.FindFirst("<dataProduct>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataProduct.Text = iRangeDataProduct.Text.Replace("<dataProduct>", string.Empty);
                
                var total = dataQuotationProduct.Count;
                var productExport = dataQuotationProduct.Select((o, i) => new
                {
                    Index = i + 1,
                    Type = o.ObjectType == 1 ? "Module" : o.ObjectType == 2 ? "Thiết bị" : o.ObjectType == 3 ? "Thư viện sản phẩm kinh doanh" : "",
                    o.Name,
                    o.Code,
                    o.ManufactureName,
                    o.UnitName,
                    o.Quantity,
                    o.Price,
                    o.Amount
                });

                if(total > 1)
                {
                    sheet.InsertRow(iRangeDataProduct.Row + 1, productExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                if (total > 0)
                {
                    sheet.ImportData(productExport, iRangeDataProduct.Row, iRangeDataProduct.Column, false);
                    sheet.Range[iRangeDataProduct.Row, 1, iRangeDataProduct.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDataProduct.Row, 1, iRangeDataProduct.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDataProduct.Row, 1, iRangeDataProduct.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDataProduct.Row, 1, iRangeDataProduct.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDataProduct.Row, 1, iRangeDataProduct.Row + total - 1, 8].Borders.Color = ExcelKnownColors.Black;
                }    
                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + dataQuery.NumberQuotation + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + dataQuery.NumberQuotation + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

    }
}
