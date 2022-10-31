using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.CustomerRequirement;
using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.Survey;
using NTS.Model.Customers;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.SolutionAnalysisEstimate;
using NTS.Model.Solution;
using NTS.Model.Meeting;
using NTS.Model.Project;
using NTS.Model.Employees;
using NTS.Model.MeetingContent;
using Newtonsoft.Json;
using NTS.Model.CustomerContact;

namespace QLTK.Business.Solutions
{
    public class CustomerRequirementBussiness
    {
        private QLTKEntities db = new QLTKEntities();


        /// <summary>
        /// Thêm mới yêu cầu khach hang
        /// </summary>
        /// <param name="model"></param>
        public void CreateCustomerRequirement(CustomerRequirementCreateModel model)
        {
            List<SurveyUser> listEmployee = new List<SurveyUser>();
            List<SurveyTool> listMaterial = new List<SurveyTool>();
            Survey survey = new Survey();
            while (db.CustomerRequirements.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper())) != null)
            {
                var codeModel = GenerateCode();
                model.Code = codeModel.Code;
                model.Index = codeModel.Index;
            }
            {
                try
                {
                    CustomerRequirement customerRequirement = new CustomerRequirement
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerId = model.CustomerId,
                        CustomerContactId = model.CustomerContactId,
                        Name = model.Name,
                        Code = model.Code.NTSTrim(),
                        RequestType = model.RequestType,
                        Petitioner = model.Petitioner,
                        DepartmentRequest = model.DepartmentRequest,
                        Reciever = model.Reciever,
                        DepartmentReceive = model.DepartmentReceive,
                        RealFinishDate = model.RealFinishDate,
                        RequestSource = model.RequestSource,
                        Status = model.Status,
                        Note = model.Note,
                        Index = model.Index,
                        Version = model.Version,
                        Budget = model.Budget,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        PlanFinishDate = model.PlanFinishDate,
                        StartDate = model.StartDate,
                        Duration = model.Duration,
                        DomainId = model.DomainId,
                        Competitor = model.Competitor,
                        CustomerSupplier = model.CustomerSupplier,
                        ProjectPhaseId = model.ProjectPhaseId,
                        //CodeChar = model.CodeChar 
                    };
                    db.CustomerRequirements.Add(customerRequirement);
                    //add danh sach yeu cau can xu li
                    foreach (var item in model.ListContent)
                    {
                        CustomerRequirementContent customerRequirementContent = new CustomerRequirementContent
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerRequirementId = customerRequirement.Id,
                            MeetingContentId = item.MeetingContentId,
                            Request = item.Request,
                            Solution = item.Solution,
                            FinishDate = item.FinishDate,
                            Note = item.Note,
                            Code = item.Code,
                            CreateDate = item.CreateDate,
                            RequestBy = item.RequestBy,
                        };
                        db.CustomerRequirementContents.Add(customerRequirementContent);
                    }

                    //Add file

                    if (model.ListAttach.Count > 0)
                    {
                        List<CustomerRequirementAttach> listFileEntity = new List<CustomerRequirementAttach>();
                        foreach (var item in model.ListAttach)
                        {
                            if (item.FilePath != null && item.FilePath != "")
                            {
                                CustomerRequirementAttach fileEntity = new CustomerRequirementAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    CustomerRequirementId = customerRequirement.Id,
                                    Name = item.Name,
                                    Note = item.Note,
                                    Type = item.Type,
                                    FileSize = item.FileSize,
                                    CreateDate = DateTime.Now,
                                    CreateBy = model.CreateBy,
                                    UpdateBy = model.CreateBy,
                                    FileName = item.FileName,
                                    FilePath = item.FilePath,
                                    UpdateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.CustomerRequirementAttaches.AddRange(listFileEntity);
                    }

                    foreach (var item in model.ListSurvey)
                    {
                        NTS.Model.Repositories.Survey survey1 = new NTS.Model.Repositories.Survey()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerRequirementId = customerRequirement.Id,
                            //ProjectPhaseId = customerRequirement.ProjectPhaseId,
                            SurveyDate = item.SurveyDate,
                            Times = JsonConvert.SerializeObject(item.Times),
                            CreateBy = item.CreateBy,
                            CreateDate = DateTime.Now,
                            UpdateBy = item.CreateBy,
                            UpdateDate = DateTime.Now,
                        };

                        db.Surveys.Add(survey1);

                        if (item.ListUser.Count > 0)
                        {
                            foreach (var user in item.ListUser)
                            {
                                SurveyUser surveyUser = new SurveyUser()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyId = survey1.Id,
                                    UserId = user.Id
                                };
                                listEmployee.Add(surveyUser);
                            }
                            db.SurveyUsers.AddRange(listEmployee);
                        }

                        if (item.ListMaterial.Count > 0)
                        {
                            foreach (var tool in item.ListMaterial)
                            {
                                SurveyTool surveyTool = new SurveyTool()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SurveyId = survey1.Id,
                                    MaterialId = tool.Id
                                };
                                listMaterial.Add(surveyTool);
                            }
                            db.SurveyTools.AddRange(listMaterial);
                        }
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, customerRequirement.Code, customerRequirement.Id, Constants.CustomerRequirement_Status);

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new NTSLogException(model, ex);
                }
            }
        }




        public List<SolutionModel> GetCustomerRequirementProductSolutionById(CustomerRequirementSolutionContentModel model)
        {
            var solutionByProducts = (from a in db.Solutions.AsNoTracking()
                                      join b in db.SolutionGroups.AsNoTracking() on a.SolutionGroupId equals b.Id into ab
                                      from ba in ab.DefaultIfEmpty()
                                      join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id into ac
                                      from ca in ac.DefaultIfEmpty()
                                      join d in db.Customers.AsNoTracking() on a.EndCustomerId equals d.Id into ad
                                      from da in ad.DefaultIfEmpty()
                                      join e in db.Employees.AsNoTracking() on a.BusinessUserId equals e.Id into ae
                                      from ea in ae.DefaultIfEmpty()
                                      join h in db.Employees.AsNoTracking() on a.SolutionMaker equals h.Id into ah
                                      from ha in ah.DefaultIfEmpty()
                                      join g in db.SBUs.AsNoTracking() on a.SBUBusinessId equals g.Id into ag
                                      from ga in ag.DefaultIfEmpty()
                                      join k in db.Departments.AsNoTracking() on a.DepartmentBusinessId equals k.Id into ak
                                      from ka in ak.DefaultIfEmpty()
                                          //join h in db.CustomerRequirementSolutions.AsNoTracking() on a.Id equals h.SolutionId into src
                                          //from csr in src
                                      join j in db.CustomerRequirementProductSolutions.AsNoTracking() on a.Id equals j.SolutionId into crps
                                      from cspr in crps
                                      join l in db.ProductNeedSolutions.AsNoTracking() on cspr.ProductNeedSolutionId equals l.Id into pns
                                      from psn in pns
                                      where psn.Id.Equals(model.ID) && psn.CustomerRequirementId.Equals(model.Content)

                                      select new SolutionModel
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Code = a.Code,
                                          SolutionGroupId = a.SolutionGroupId,
                                          SolutionGroupName = ba == null ? "" : ba.Name,
                                          Status = a.Status,
                                          CustomerName = ca == null ? "" : ca.Name,
                                          EndCustomerName = da == null ? "" : da.Name,
                                          TPAUName = da == null ? "" : ea.Name,
                                          SolutionMakerName = ha == null ? "" : ha.Name,
                                          Price = a.Price,
                                          StartDate = a.StartDate,
                                          FinishDate = a.FinishDate,
                                          SaleNoVat = a.SaleNoVAT,
                                          Description = a.Description,
                                          SBUName = ga == null ? "" : ga.Name,
                                          SBUBusinessId = ga == null ? "" : ga.Id,
                                          DepartmentBusinessId = ka == null ? "" : ka.Id,
                                          DepartmentName = ka == null ? "" : ka.Name,
                                          SBUSolutionMakerId = a.SBUSolutionMakerId,
                                          DepartmentSolutionMakerId = a.DepartmentSolutionMakerId,
                                          Index = a.Index,
                                          //StatusRequirementSolution = csr.Status == 0 ? "" : (csr.Status == 1 ? "Chốt" : "Hủy"),
                                      }).ToList();


            return solutionByProducts;
        }




        private void CheckExistedForAdd(CustomerRequirementCreateModel model)
        {
            if (db.CustomerRequirements.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductStandard);
            }

            if (db.CustomerRequirements.AsNoTracking().Where(o => o.Code.Equals(model.Code.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductStandard);
            }
        }




        /// <summary>
        /// Xoa yêu cầu khach hang
        /// </summary>

        public void DeleteCustomerRequirement(string id, string userId)
        {
            var request = db.CustomerRequirements.FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var attachs = db.CustomerRequirementAttaches.Where(a => a.CustomerRequirementId.Equals(id));
                    var content = db.CustomerRequirementContents.Where(b => b.CustomerRequirementId.Equals(id));
                    var productNeedPrice = db.ProductNeedPrices.Where(c => c.CustomerRequirementId.Equals(id));
                    var ProductNeedSolution = db.ProductNeedSolutions.Where(d => d.CustomerRequirementId.Equals(id));
                    var survey = db.Surveys.Where(e => e.CustomerRequirementId.Equals(id));
                    var analysisRisk = db.SolutionAnalysisRisks.Where(f => f.CustomerRequirementId.Equals(id));
                    var analysisEstimate = db.SolutionAnalysisEstimates.Where(g => g.CustomerRequirementId.Equals(id));
                    var requirementEstimate = db.CustomerRequirementEstimates.Where(h => h.CustomerRequirementId.Equals(id));
                    var analysisProduct = db.SolutionAnalysisProducts.Where(i => i.CustomerRequirementId.Equals(id));
                    var analysisSupplier = db.SolutionAnalysisSuppliers.Where(i => i.CustomerRequirementId.Equals(id));
                    var customerRequirementSolutions = db.CustomerRequirementSolutions.Where(k => k.CustomerRequirementId.Equals(id));

                    var requirementEstimateId = requirementEstimate.Select(r => r.Id).ToList();
                    var CustomerRequirementEstimateAttach = db.CustomerRequirementEstimateAttaches.Where(k => requirementEstimateId.Contains(k.CustomerRequirementEstimateId));


                    if (survey.Count() > 0)
                    {
                        foreach (var item in survey)
                        {
                            if (item.SurveyTools.Count() > 0)
                            {
                                db.SurveyTools.RemoveRange(item.SurveyTools);
                            }
                            if (item.SurveyContents.Count() > 0)
                            {
                                foreach (var surveyContent in item.SurveyContents)
                                {
                                    if (surveyContent.SurveyContentAttaches.Count() > 0)
                                    {
                                        db.SurveyContentAttaches.RemoveRange(surveyContent.SurveyContentAttaches);
                                    }
                                    if (surveyContent.SurveyContentUsers.Count() > 0)
                                    {
                                        db.SurveyContentUsers.RemoveRange(surveyContent.SurveyContentUsers);
                                    }
                                }
                                db.SurveyContents.RemoveRange(item.SurveyContents);
                            }
                            if (item.SurveyUsers.Count() > 0)
                            {
                                db.SurveyUsers.RemoveRange(item.SurveyUsers);
                            }
                            if (item.SurveyUsers.Count() > 0)
                            {
                                db.SurveyUsers.RemoveRange(item.SurveyUsers);
                            }
                        }
                    }
                    if (customerRequirementSolutions.Count() > 0)
                    {
                        foreach (var item in customerRequirementSolutions)
                        {
                            if (item.CustomerRequirementSolutionContents.Count() > 0)
                            {
                                db.CustomerRequirementSolutionContents.RemoveRange(item.CustomerRequirementSolutionContents);
                            }
                        }
                    }

                    db.CustomerRequirementAttaches.RemoveRange(attachs);
                    db.CustomerRequirementContents.RemoveRange(content);
                    db.ProductNeedPrices.RemoveRange(productNeedPrice);
                    db.ProductNeedSolutions.RemoveRange(ProductNeedSolution);
                    db.Surveys.RemoveRange(survey);
                    db.SolutionAnalysisRisks.RemoveRange(analysisRisk);
                    db.SolutionAnalysisEstimates.RemoveRange(analysisEstimate);
                    db.CustomerRequirementEstimateAttaches.RemoveRange(CustomerRequirementEstimateAttach);
                    db.CustomerRequirementEstimates.RemoveRange(requirementEstimate);
                    db.SolutionAnalysisProducts.RemoveRange(analysisProduct);
                    db.SolutionAnalysisSuppliers.RemoveRange(analysisSupplier);
                    db.CustomerRequirements.Remove(request);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(id, ex);
                }
            }
        }

        /// <summary>
        /// Chi tiết yêu cầu khach hang
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object getCustomerId(string id)
        {
            var data = db.CustomerRequirements.Where(a => a.Id.Equals(id)).Select(b => b.CustomerId).FirstOrDefault();
            return data;
        }

        public CustomerRequirementInfoModel GetCustomerRequirementById(string id)
        {
            var request = (from a in db.CustomerRequirements.AsNoTracking()
                           where a.Id.Equals(id)
                           join b in db.CustomerRequirementEstimates.AsNoTracking() on a.Id equals b.CustomerRequirementId into ab
                           from abv in ab.DefaultIfEmpty()
                           select new CustomerRequirementInfoModel
                           {
                               Id = a.Id,
                               Name = a.Name,
                               CustomerId = a.CustomerId,
                               CustomerContactId = a.CustomerContactId,
                               Code = a.Code,
                               RequestType = a.RequestType,
                               Petitioner = a.Petitioner,
                               DepartmentRequest = a.DepartmentRequest,
                               RequestSource = a.RequestSource,
                               Reciever = a.Reciever,
                               DepartmentReceive = a.DepartmentReceive,
                               RealFinishDate = a.RealFinishDate,
                               Note = a.Note,
                               Index = a.Index,
                               Status = a.Status,
                               Version = a.Version,
                               Budget = a.Budget,
                               UpdateBy = a.UpdateBy,
                               ProjectPhaseId = a.ProjectPhaseId,
                               Competitor = a.Competitor,
                               CustomerSupplier = a.CustomerSupplier,
                               PriorityLevel = a.PriorityLevel,
                               Step = a.Step,
                               TradeConditions = abv != null ? abv.TradeConditions : "",
                               TotalPrice = abv != null ? abv.TotalPrice : 0,
                               MeetingCode = a.MeetingCode,
                               Content1 = a.Content1,
                               Conclude1 = a.Conclude1,
                               Person1 = a.Person1,
                               Content2 = a.Content2,
                               Conclude2 = a.Conclude2,
                               Person2 = a.Person2,
                               Content3 = a.Content3,
                               Conclude3 = a.Conclude3,
                               Person3 = a.Person3,
                               PlanFinishDate = a.PlanFinishDate,
                               StartDate = a.StartDate,
                               Duration = a.Duration,
                               CustomerRequirementState = a.CustomerRequirementState,
                               CustomerRequirementAnalysisState = a.CustomerRequirementAnalysisState,
                               SurveyState = a.SurveyState,
                               SolutionAnalysisState = a.SolutionAnalysisState,
                               EstimateState = a.EstimateState,
                               DoSolutionAnalysisState = a.DoSolutionAnalysisState,
                               DomainId = a.DomainId,

                           }).FirstOrDefault();
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
            }

            request.ListAttach = (from m in db.CustomerRequirementAttaches.AsNoTracking()
                                  where m.CustomerRequirementId.Equals(id)
                                  join u in db.Users.AsNoTracking() on m.CreateBy equals u.Id
                                  join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                  select new CustomerRequirementAttachModel
                                  {
                                      Id = m.Id,
                                      Type = m.Type,
                                      Name = m.Name,
                                      Note = m.Note,
                                      FilePath = m.FilePath,
                                      FileName = m.FileName,
                                      FileSize = m.FileSize,

                                  }).ToList();

            var customerRequirementEstimate = db.CustomerRequirementEstimates.AsNoTracking().FirstOrDefault(a => a.CustomerRequirementId.Equals(request.Id));
            if (customerRequirementEstimate != null)
            {
                request.ListRequireEstimateMaterialAttach = (from m in db.CustomerRequirementEstimateAttaches.AsNoTracking()
                                                             where m.CustomerRequirementEstimateId.Equals(customerRequirementEstimate.Id)
                                                             select new CustomerRequirementAttachModel
                                                             {
                                                                 Id = m.Id,
                                                                 Type = m.Type,
                                                                 Name = m.Name,
                                                                 Note = m.Note,
                                                                 FilePath = m.FilePath,
                                                                 FileName = m.FileName,
                                                                 FileSize = m.FileSize,
                                                             }).ToList();
            }


            request.ListUser = (from a in db.SurveyUsers.AsNoTracking()
                                join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                                join e in db.Surveys.AsNoTracking() on a.SurveyId equals e.Id
                                where a.SurveyId.Equals(request.Id)
                                select new CustomerRequirementUserInfoModel
                                {
                                    SurveyId = a.SurveyId,
                                    UserId = a.UserId,
                                    Code = c.Code,
                                    Name = c.Name,
                                    PhoneNumber = c.PhoneNumber,
                                    Email = c.Email,
                                    Status = c.Status,
                                    DepartmentName = d.Name,
                                    IsNew = false
                                }).ToList();

            request.ListMaterial = (from a in db.SurveyTools.AsNoTracking()
                                    join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                    join c in db.MaterialGroups.AsNoTracking() on b.MaterialGroupId equals c.Id
                                    join d in db.Manufactures.AsNoTracking() on b.ManufactureId equals d.Id
                                    join e in db.Surveys.AsNoTracking() on a.SurveyId equals e.Id
                                    where a.SurveyId.Equals(request.Id)
                                    select new CustomerRequirementMaterialInfoModel
                                    {
                                        Id = b.Id,
                                        SurveyId = a.SurveyId,
                                        Name = b.Name,
                                        Code = b.Code,
                                        MaterialGroupName = c.Name,
                                        ManufactureCode = d.Name,
                                        Pricing = b.Pricing,
                                    }).ToList();

            List<SolutionAnalysisEstimateModel> estimate = new List<SolutionAnalysisEstimateModel>();
            var ListEstimates = db.SolutionAnalysisEstimates.Where(a => a.CustomerRequirementId.Equals(id)).ToList();
            foreach (var item in ListEstimates)
            {
                if (item.MaterialId != null)
                {
                    var listEstimate = (from s in db.SolutionAnalysisEstimates.AsNoTracking()
                                        where s.Id.Equals(item.Id)
                                        join t in db.Materials.AsNoTracking() on s.MaterialId equals t.Id
                                        join c in db.MaterialGroups.AsNoTracking() on t.MaterialGroupId equals c.Id into ac
                                        from c in ac.DefaultIfEmpty()
                                        join d in db.Manufactures.AsNoTracking() on t.ManufactureId equals d.Id into ad
                                        from adv in ad.DefaultIfEmpty()
                                        select new SolutionAnalysisEstimateModel()
                                        {
                                            Id = s.Id,
                                            MaterialId = s.MaterialId,
                                            Note = s.Note,
                                            DeliveryTime = s.DeliveryTime,
                                            MaterialGroupName = c.Name,
                                            ManufactureCode = adv != null ? adv.Code : "",
                                            Code = t.Code,
                                            Name = t.Name,
                                            Pricing = t.Pricing,
                                            Quantity = (int)s.Quantity,
                                        }).FirstOrDefault();
                    if (listEstimate != null)
                    {
                        estimate.Add(listEstimate);
                    }

                }
                if (item.MaterialId == null)
                {
                    var solutionMaterial = (from a in db.SolutionAnalysisEstimates.AsNoTracking()
                                            where a.Id.Equals(item.Id)
                                            select new SolutionAnalysisEstimateModel()
                                            {
                                                Id = a.Id,
                                                Note = a.Note,
                                                DeliveryTime = a.DeliveryTime,
                                                MaterialGroupName = a.MaterialGroupName,
                                                ManufactureCode = a.MaterialGroupCode,
                                                Code = a.Code,
                                                Name = a.Name,
                                                Pricing = (decimal)a.Pricing,
                                                Quantity = (int)a.Quantity,
                                            }).FirstOrDefault();
                    if (solutionMaterial != null)
                    {
                        estimate.Add(solutionMaterial);
                    }
                }

            }
            request.ListEstimate = estimate;

            request.ListContent = (from a in db.CustomerRequirementContents.AsNoTracking()
                                   join b in db.CustomerContacts.AsNoTracking() on a.RequestBy equals b.Id into ab
                                   from b in ab.DefaultIfEmpty()
                                   where a.CustomerRequirementId.Equals(request.Id)
                                   select new CustomerRequirementContentModel
                                   {
                                       Id = a.Id,
                                       CustomerRequirementId = a.CustomerRequirementId,
                                       MeetingContentId = a.MeetingContentId,
                                       Request = a.Request,
                                       Solution = a.Solution,
                                       FinishDate = a.FinishDate,
                                       Note = a.Note,
                                       Code = a.Code,
                                       CreateDate = a.CreateDate,
                                       RequestBy = a.RequestBy,
                                       RequestName = b.Name
                                   }).ToList();

            request.ListProductNeedPrice = db.ProductNeedPrices.AsNoTracking().Where(t => t.CustomerRequirementId.Equals(request.Id)).Select(t => new ProductNeedPriceModel
            {
                Id = t.Id,
                CustomerRequirementId = t.CustomerRequirementId,
                Name = t.Name,
                Code = t.Code,
                DeliveryDate = t.DeliveryDate,
                ManufactureName = t.ManufactureName,
                Price = t.Price,
                ProductType = t.ProductType,
                Quantity = t.Quantity,
                Unit = t.Unit,
                Specifications = t.Specifications,
                Note = t.Note
            }).ToList();

            request.ListSolution = (from a in db.Solutions.AsNoTracking()
                                    join b in db.SolutionGroups.AsNoTracking() on a.SolutionGroupId equals b.Id into ab
                                    from ba in ab.DefaultIfEmpty()
                                    join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id into ac
                                    from ca in ac.DefaultIfEmpty()
                                    join d in db.Customers.AsNoTracking() on a.EndCustomerId equals d.Id into ad
                                    from da in ad.DefaultIfEmpty()
                                    join e in db.Employees.AsNoTracking() on a.BusinessUserId equals e.Id into ae
                                    from ea in ae.DefaultIfEmpty()
                                    join h in db.Employees.AsNoTracking() on a.SolutionMaker equals h.Id into ah
                                    from ha in ah.DefaultIfEmpty()
                                    join g in db.SBUs.AsNoTracking() on a.SBUBusinessId equals g.Id into ag
                                    from ga in ag.DefaultIfEmpty()
                                    join k in db.Departments.AsNoTracking() on a.DepartmentBusinessId equals k.Id into ak
                                    from ka in ak.DefaultIfEmpty()
                                    join h in db.CustomerRequirementSolutions.AsNoTracking() on a.Id equals h.SolutionId into src
                                    from csr in src
                                    where csr.CustomerRequirementId.Equals(id)
                                    select new SolutionModel
                                    {
                                        Id = a.Id,
                                        Name = a.Name,
                                        Code = a.Code,
                                        SolutionGroupId = a.SolutionGroupId,
                                        SolutionGroupName = ba == null ? "" : ba.Name,
                                        Status = a.Status,
                                        CustomerName = ca == null ? "" : ca.Name,
                                        EndCustomerName = da == null ? "" : da.Name,
                                        TPAUName = da == null ? "" : ea.Name,
                                        SolutionMakerName = ha == null ? "" : ha.Name,
                                        Price = a.Price,
                                        StartDate = a.StartDate,
                                        FinishDate = a.FinishDate,
                                        SaleNoVat = a.SaleNoVAT,
                                        Description = a.Description,
                                        SBUName = ga == null ? "" : ga.Name,
                                        SBUBusinessId = ga == null ? "" : ga.Id,
                                        DepartmentBusinessId = ka == null ? "" : ka.Id,
                                        DepartmentName = ka == null ? "" : ka.Name,
                                        SBUSolutionMakerId = a.SBUSolutionMakerId,
                                        DepartmentSolutionMakerId = a.DepartmentSolutionMakerId,
                                        Index = a.Index,
                                        StatusRequirementSolution = csr.Status == 0 ? "" : (csr.Status == 1 ? "Chốt" : "Hủy"),
                                    }).ToList();
            request.listEmployee = (from e in db.Employees.AsNoTracking()
                                    orderby e.Name
                                    select new EmployeeModel
                                    {
                                        Id = e.Id,
                                        Name = e.Name,
                                        Code = e.Code
                                    }).ToList();

            return request;
        }


        /// <summary>
        /// cap nhat
        /// </summary>
        /// <param></param>

        public void UpdateCustomerRequirement(string id, CustomerRequirementInfoModel model, string userId)
        {
            var checkCode = db.CustomerRequirements.AsNoTracking().FirstOrDefault(r => !r.Id.Equals(id) && r.Code.ToLower().Equals(model.Code.ToLower()));

            if (checkCode != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.CustomerRequirement);
            }

            #region thaint Bug #17937
            List<string> idModel = new List<string>();

            foreach (var item in model.ListContent)
            {
                idModel.Add(item.Id);
            }

            var customerRequirementContents = db.CustomerRequirementContents.Where(t => t.CustomerRequirementId.Equals(id)).ToList();
            foreach (var item in customerRequirementContents) // xóa yêu cầu
            {
                if (!idModel.Contains(item.Id))
                {
                    var needToDelete = db.CustomerRequirementContents.FirstOrDefault(x => x.Id.Equals(item.Id));
                    db.CustomerRequirementContents.Remove(needToDelete);
                }
            }
            if (customerRequirementContents.Count > 0) // cập nhật yêu cầu 
            {
                foreach (var item in model.ListContent)
                {
                    if (!string.IsNullOrEmpty(item.Id))
                    {
                        var customerRequirementContent = db.CustomerRequirementContents.FirstOrDefault(x => x.Id.Equals(item.Id));
                        if (customerRequirementContent != null)
                        {
                            customerRequirementContent.Request = item.Request;
                            customerRequirementContent.Solution = item.Solution;
                            customerRequirementContent.CreateDate = item.CreateDate;
                            customerRequirementContent.FinishDate = item.FinishDate;
                            customerRequirementContent.Note = item.Note;
                            customerRequirementContent.Code = item.Code;
                            customerRequirementContent.RequestBy = item.RequestBy;

                        }
                    }
                }
            }
            foreach (var item in model.ListContent)
            {
                if (string.IsNullOrEmpty(item.Id)) // thêm mới yêu cầu
                {
                    var newCustomerRequirementContent = new CustomerRequirementContent
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerRequirementId = id,
                        MeetingContentId = item.MeetingContentId,
                        Request = item.Request,
                        Solution = item.Solution,
                        FinishDate = item.FinishDate,
                        Note = item.Note,
                        Code = item.Code,
                        CreateDate = item.CreateDate,
                        RequestBy = item.RequestBy,
                    };
                    db.CustomerRequirementContents.Add(newCustomerRequirementContent);
                }
            }

            //var meetingContentId = customerRequirementContent.Select(x => x.MeetingContentId).FirstOrDefault();
            //if (customerRequirementContent.Count > 0)
            //{
            //    db.CustomerRequirementContents.RemoveRange(customerRequirementContent);
            //}
            //if (meetingContentId != null)
            //{
            //    foreach (var item in model.ListContent)
            //    {
            //        var content = new CustomerRequirementContent()
            //        {
            //            Id = Guid.NewGuid().ToString(),
            //            CustomerRequirementId = id,
            //            MeetingContentId = meetingContentId,
            //            Request = item.Request,
            //            Solution = item.Solution,
            //            FinishDate = item.FinishDate,
            //            Note = item.Note,
            //            Code=item.Code,
            //            CreateDate=item.CreateDate,
            //        };
            //        db.CustomerRequirementContents.Add(content);
            //    }
            //}

            #endregion
            var request = db.CustomerRequirements.FirstOrDefault(t => t.Id.Equals(id));

            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
            }

            List<SurveyUser> listEmployee = new List<SurveyUser>();
            List<SurveyTool> listMaterial = new List<SurveyTool>();
            List<SolutionAnalysisProduct> products = new List<SolutionAnalysisProduct>();

            Survey survey = new Survey();

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (request.CustomerId != model.CustomerId)
                    {
                        request.CustomerId = model.CustomerId;

                        //var listData =  db.CustomerRequirementContents.Where(a => a.CustomerRequirementId.Equals(id)).ToList();

                        //foreach (var data in listData)
                        //{
                        //    db.CustomerRequirementContents.Remove(data);
                        //}    

                        //foreach (var item in model.ListContent)
                        //{
                        //    CustomerRequirementContent customerRequirementContent = new CustomerRequirementContent
                        //    {
                        //        Id = Guid.NewGuid().ToString(),
                        //        CustomerRequirementId = id,
                        //        MeetingContentId = item.MeetingContentId,
                        //        Request = item.Request,
                        //        Solution = item.Solution,
                        //        FinishDate = item.FinishDate,
                        //        Note = item.Note,
                        //        Code = item.Code,
                        //        CreateDate = item.CreateDate,
                        //        RequestBy = item.RequestBy,
                        //    };
                        //    db.CustomerRequirementContents.Add(customerRequirementContent);
                        //}
                    }

                    //if (request.CustomerId == model.CustomerId)
                    //{
                    //    var listData = db.CustomerRequirementContents.Where(a => a.CustomerRequirementId.Equals(id)).ToList();

                    //    foreach (var data in listData)
                    //    {
                    //        foreach (var item in model.ListContent)
                    //        {
                    //            if(item.Id == data.Id)
                    //            {
                    //                var dataCustomerRequirementContent = db.CustomerRequirementContents.FirstOrDefault(a => a.Id.Equals(data.Id));

                    //                dataCustomerRequirementContent.MeetingContentId = item.MeetingContentId;
                    //                dataCustomerRequirementContent.Request = item.Request;
                    //                dataCustomerRequirementContent.Solution = item.Solution;
                    //                dataCustomerRequirementContent.FinishDate = item.FinishDate;
                    //                dataCustomerRequirementContent.CreateDate = item.CreateDate;
                    //                dataCustomerRequirementContent.Code = item.Code;
                    //                dataCustomerRequirementContent.Note = item.Note;
                    //                dataCustomerRequirementContent.RequestBy = item.RequestBy;

                    //                db.SaveChanges();
                    //            }
                    //        }    
                    //    }
                    //}

                    request.CustomerContactId = model.CustomerContactId;
                    request.Name = model.Name;
                    request.Note = model.Note;
                    request.Code = model.Code;
                    request.Status = model.Status;
                    request.Version = model.Version;
                    request.Budget = model.Budget;
                    request.RequestType = model.RequestType;
                    request.DepartmentRequest = model.DepartmentRequest;
                    request.RealFinishDate = model.RealFinishDate;
                    request.UpdateBy = userId;
                    request.UpdateDate = DateTime.Now;
                    request.DepartmentReceive = model.DepartmentReceive;
                    request.Reciever = model.Reciever;
                    request.Petitioner = model.Petitioner;
                    request.RequestSource = model.RequestSource;
                    request.PlanFinishDate = model.PlanFinishDate;
                    request.StartDate = model.StartDate;
                    request.Duration = model.Duration;
                    request.CustomerRequirementState = model.CustomerRequirementState;
                    request.SolutionAnalysisState = model.SolutionAnalysisState;
                    request.DoSolutionAnalysisState = model.DoSolutionAnalysisState;
                    request.DomainId = model.DomainId;
                    request.Competitor = model.Competitor;
                    request.CustomerSupplier = model.CustomerSupplier;
                    request.ProjectPhaseId = model.ProjectPhaseId;

                    CustomerRequirementAttach attach;
                    foreach (var item in model.ListAttach)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            attach = new CustomerRequirementAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CustomerRequirementId = request.Id,
                                Name = item.Name,
                                Note = item.Note,
                                Type = item.Type,
                                FileSize = item.FileSize,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };

                            db.CustomerRequirementAttaches.Add(attach);
                        }
                        else
                        {
                            attach = db.CustomerRequirementAttaches.FirstOrDefault(r => r.Id.Equals(item.Id));

                            if (attach != null)
                            {
                                if (item.IsDelete)
                                {
                                    db.CustomerRequirementAttaches.Remove(attach);
                                }
                                else
                                {
                                    attach.Name = item.Name;
                                    attach.Note = item.Note;
                                    attach.Type = item.Type;
                                    attach.FileName = item.FileName;
                                    attach.FilePath = item.FilePath;
                                    attach.FileSize = item.FileSize;
                                    attach.UpdateBy = userId;
                                    attach.UpdateDate = DateTime.Now;
                                }
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


        //public void UpdateCustomerRequirementContent(string id, CustomerRequirementContentModel model)
        //{
        //    var data = db.CustomerRequirementContents.FirstOrDefault(a => a.Id.Equals(id));

        //    using (var trans = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            data.MeetingContentId = model.MeetingContentId;
        //            data.Request = model.Request;
        //            data.Solution = model.Solution;
        //            data.FinishDate = model.FinishDate;
        //            data.CreateDate = model.CreateDate;
        //            data.Code = model.Code;
        //            data.Note = model.Note;
        //            data.RequestBy = model.RequestBy;

        //            db.SaveChanges();
        //            trans.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();
        //            throw new NTSLogException(model, ex);
        //        }
        //    }
        //}

        public void CreateCustomerRequirementContent(string id, CustomerRequirementContentModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    CustomerRequirementContent customerRequirementContent = new CustomerRequirementContent
                    {
                        Id = Guid.NewGuid().ToString(),
                        CustomerRequirementId = id,
                        MeetingContentId = model.MeetingContentId,
                        Request = model.Request,
                        Solution = model.Solution,
                        FinishDate = model.FinishDate,
                        CreateDate = model.CreateDate,
                        Code = model.Code,
                        Note = model.Note,
                        RequestBy = model.RequestBy,
                    };
                    db.CustomerRequirementContents.Add(customerRequirementContent);

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

        /// <summary>
        /// Tìm kiếm yêu cầu khach hang
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>

        public SearchResultModel<CustomerRequirementSearchResultModel> SearchCustomerRequirement(CustomerRequirementSearchResultModel searchModel, string EnableEmployeeId)
        {
            SearchResultModel<CustomerRequirementSearchResultModel> searchResult = new SearchResultModel<CustomerRequirementSearchResultModel>();

            var EnableId = (from a in db.Users.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                            where a.Id.Equals(EnableEmployeeId)
                            select b.Id).FirstOrDefault();

            var dataQuery = (from a in db.CustomerRequirements.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                             join c in db.CustomerContacts.AsNoTracking() on a.CustomerContactId equals c.Id
                             join d in db.Employees.AsNoTracking() on a.Petitioner equals d.Id into ad
                             from adx in ad.DefaultIfEmpty()
                             join e in db.Employees.AsNoTracking() on a.Reciever equals e.Id into ae
                             from aex in ae.DefaultIfEmpty()
                             join f in db.Departments.AsNoTracking() on a.DepartmentRequest equals f.Id into af
                             from afx in af.DefaultIfEmpty()
                             join g in db.Departments.AsNoTracking() on a.DepartmentReceive equals g.Id into ag
                             from agx in ag.DefaultIfEmpty()
                             join h in db.ProjectPhases.AsNoTracking() on a.ProjectPhaseId equals h.Id into ah
                             from ahx in ah.DefaultIfEmpty()
                                 //where a.Petitioner.Equals(EnableId) || b.EmployeeId.Equals(EnableId)
                             select new CustomerRequirementSearchResultModel
                             {
                                 Id = a.Id,
                                 CustomerId = b.Id,
                                 CustomerName = b.Name,
                                 EmployeeId = b.EmployeeId,
                                 CustomerContactId = c.Name,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Note = a.Note,
                                 Status = a.Status,
                                 Step = a.Step,
                                 Version = a.Version,
                                 Budget = a.Budget,
                                 RequestType = a.RequestType,
                                 Petitioner = a.Petitioner,
                                 DepartmentRequest = a.DepartmentRequest,
                                 DepartmentRequestName = afx != null ? afx.Name : string.Empty,
                                 DepartmentReceiveName = agx != null ? agx.Name : string.Empty,
                                 PetitionerName = adx != null ? adx.Name : string.Empty,
                                 RecieverName = aex != null ? aex.Name : string.Empty,
                                 Reciever = a.Reciever,
                                 DepartmentReceive = a.DepartmentReceive,
                                 RequestSource = a.RequestSource,
                                 RealFinishDate = a.RealFinishDate,
                                 ProjectPhaseId = ahx != null ? ahx.Name : string.Empty,
                                 Competitor = a.Competitor,
                                 CustomerSupplier = a.CustomerSupplier,
                                 PriorityLevel = a.PriorityLevel,

                                 CustomerRequirementState = a.CustomerRequirementState,
                                 CustomerRequirementAnalysisState = a.CustomerRequirementAnalysisState,
                                 SurveyState = a.SurveyState,
                                 SolutionAnalysisState = a.SolutionAnalysisState,
                                 EstimateState = a.EstimateState,
                                 DoSolutionAnalysisState = a.DoSolutionAnalysisState,

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(searchModel.Code.ToUpper()) || r.Name.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status.Value);
            }


            if (!string.IsNullOrEmpty(searchModel.CustomerId))
            {
                dataQuery = dataQuery.Where(a => a.CustomerId.Equals(searchModel.CustomerId));
            }


            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.OrderBy(t => t.Code).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            foreach (var item in searchResult.ListResult)
            {
                item.ListSurvey = db.Surveys.AsNoTracking().Where(t => t.CustomerRequirementId.Equals(item.Id)).Select(m => new SurveyCreateModel
                {
                    Id = m.Id,
                    CustomerRequirementId = m.CustomerRequirementId,
                    //ProjectPhaseId = m.ProjectPhaseId,
                    SurveyDate = m.SurveyDate,
                    Times = m.Times,
                }).ToList();


                foreach (var user in item.ListSurvey)
                {
                    if (!string.IsNullOrEmpty(user.Id))
                    {
                        var listServeyUser = db.SurveyUsers.Where(t => t.Id.Equals(user.ListUser)).Select(t => t.UserId).ToList();

                        if (listServeyUser.Count > 0)
                        {
                            dataQuery = dataQuery.Where(t => listServeyUser.Contains(user.Id));
                        }
                    }
                    user.Time = JsonConvert.DeserializeObject<object>(user.Times);
                }

                foreach (var tool in item.ListSurvey)
                {
                    if (!string.IsNullOrEmpty(tool.Id))
                    {
                        var listServeyTools = db.SurveyTools.Where(t => t.Id.Equals(tool.ListMaterial)).Select(t => t.MaterialId).ToList();

                        if (listServeyTools.Count > 0)
                        {
                            dataQuery = dataQuery.Where(t => listServeyTools.Contains(tool.Id));
                        }
                    }
                }

            }


            return searchResult;
        }

        /// <summary>
        /// Tự động tạo mã xuất giữ
        /// </summary>
        /// <returns></returns>
        public CustomerRequirementCodeModel GenerateCode()
        {
            var dateNow = DateTime.Now;
            string code = "";
            var maxIndex = db.CustomerRequirements.AsNoTracking().Select(r => r.Index).DefaultIfEmpty(0).Max();
            maxIndex++;
            code = $"YC.{string.Format("{0:0000}", maxIndex)}";

            return new CustomerRequirementCodeModel
            {
                Code = code,
                Index = maxIndex
            };
        }


        public void NextStep(CustomerRequirementCreateModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Status = 0;
                    var customerRequirement = db.CustomerRequirements.FirstOrDefault(r => r.Id.Equals(model.Id));
                    if (customerRequirement == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
                    }


                    Status = customerRequirement.Status;
                    customerRequirement.Status = customerRequirement.Status + 1;
                    customerRequirement.UpdateBy = userId;
                    customerRequirement.UpdateDate = DateTime.Now;

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

        public void NextThreeStep(CustomerRequirementCreateModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Status = 0;
                    var customerRequirement = db.CustomerRequirements.FirstOrDefault(r => r.Id.Equals(model.Id));
                    if (customerRequirement == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
                    }


                    Status = customerRequirement.Status;
                    customerRequirement.Status = customerRequirement.Status + 5;
                    customerRequirement.UpdateBy = userId;
                    customerRequirement.UpdateDate = DateTime.Now;

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


        public void BackStep(string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Status = 0;
                    var customerRequirement = db.CustomerRequirements.FirstOrDefault(r => r.Id.Equals(id));
                    if (customerRequirement == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
                    }


                    Status = customerRequirement.Status;
                    customerRequirement.Status = customerRequirement.Status - 1;
                    customerRequirement.UpdateBy = userId;
                    customerRequirement.UpdateDate = DateTime.Now;

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


        public void BackThreeStep(string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Status = 0;
                    var customerRequirement = db.CustomerRequirements.FirstOrDefault(r => r.Id.Equals(id));
                    if (customerRequirement == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CustomerRequirement);
                    }


                    Status = customerRequirement.Status;
                    customerRequirement.Status = customerRequirement.Status - 5;
                    customerRequirement.UpdateBy = userId;
                    customerRequirement.UpdateDate = DateTime.Now;

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

        public void CheckDeleteSurvey(string customerRequirementId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var survey = db.Surveys.FirstOrDefault(a => a.Id.Equals(customerRequirementId));
                    if (survey != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Survey);
                    }

                    db.Surveys.Remove(survey);
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

        public SurveyInfoModel GetSurvey(string id)
        {
            var survey = db.Surveys.AsNoTracking().Where(i => i.Id.Equals(id)).Select(i => new SurveyInfoModel()
            {
                Id = i.Id,
                CustomerRequirementId = i.CustomerRequirementId,
                //ProjectPhaseId = i.ProjectPhaseId,
                SurveyDate = i.SurveyDate,
                Times = i.Times,
            }).FirstOrDefault();
            survey.Time = JsonConvert.DeserializeObject<object>(survey.Times);

            return survey;
        }

        public List<ComboboxResult> GetDomain(string id)
        {
            var requests = (from a in db.CustomerDomains.AsNoTracking()
                            where a.CustomerId.Equals(id)
                            join b in db.Jobs on a.JobId equals b.Id into ab
                            from b in ab.DefaultIfEmpty()
                            group b by new { a.Id, b.Name, b.Code } into g
                            select new ComboboxResult
                            {
                                Id = g.Key.Id,
                                Name = g.Key.Name,
                                Code = g.Key.Code
                            }).ToList();

            return requests;
        }

        public object GetRequestName(string id)
        {
            var requests = (from a in db.CustomerContacts.AsNoTracking()
                            where a.Id.Equals(id)
                            select new
                            {
                                Name = a.Name,
                            }).FirstOrDefault();

            return requests;
        }

        public object GetCustomerContactById(string id)
        {
            var requests = (from a in db.CustomerContacts.AsNoTracking()
                            where a.CustomerId.Equals(id)
                            select new
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Email = a.Email
                            }).ToList();

            return requests;
        }

        public ProductNeedSolutionModel GetProductNeedSolutionById(string id)
        {
            var product = db.ProductNeedSolutions.AsNoTracking().Where(i => i.Id.Equals(id)).Select(t => new ProductNeedSolutionModel()
            {
                Id = t.Id,
                CustomerRequirementId = t.CustomerRequirementId,
                Name = t.Name,
                Code = t.Code,
                DeliveryDate = t.DeliveryDate,
                ManufactureName = t.ManufactureName,
                Price = t.Price,
                ProductType = t.ProductType,
                Quantity = t.Quantity,
                Unit = t.Unit,
                Specifications = t.Specifications,
                Note = t.Note
            }).FirstOrDefault();

            return product;
        }

        public object GetCustomerContact()
        {
            var data = (from a in db.CustomerContacts.AsNoTracking()
                        select new
                        {
                            Name = a.Name,
                            Email = a.Email,
                            Id = a.Id
                        }).ToList();
            return data;
        }

        public ProductNeedSolutionModel GetProductNeedPriceById(string id)
        {
            var product = db.ProductNeedPrices.AsNoTracking().Where(i => i.Id.Equals(id)).Select(t => new ProductNeedSolutionModel()
            {
                Id = t.Id,
                CustomerRequirementId = t.CustomerRequirementId,
                Name = t.Name,
                Code = t.Code,
                DeliveryDate = t.DeliveryDate,
                ManufactureName = t.ManufactureName,
                Price = t.Price,
                ProductType = t.ProductType,
                Quantity = t.Quantity,
                Unit = t.Unit,
                Specifications = t.Specifications,
                Note = t.Note
            }).FirstOrDefault();

            return product;
        }


        public void CreateUpdateCustomerRequirementContent(CustomerRequirementContentModel model)
        {
            if (model.Id != "") // update nếu có Id
            {
                var existing = db.CustomerRequirementContents.FirstOrDefault(x => x.Id.Equals(model.Id));
                existing.Request = model.Request;
                existing.Solution = model.Solution;
                existing.FinishDate = model.FinishDate;
                existing.Note = model.Note;
                existing.Code = model.Code;
                existing.RequestBy = model.RequestBy;

            }
            else // create nếu chưa có Id
            {
                var newCustomerRequirementContent = new CustomerRequirementContent
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerRequirementId = model.CustomerRequirementId,
                    MeetingContentId = model.MeetingContentId,
                    Request = model.Request,
                    Solution = model.Solution,
                    FinishDate = model.FinishDate,
                    Note = model.Note,
                    Code = model.Code,
                    CreateDate = model.CreateDate,
                    RequestBy = model.RequestBy,
                };
                db.CustomerRequirementContents.Add(newCustomerRequirementContent);
            }
            db.SaveChanges();
        }

        public void DeleteCustomerRequirementContent(string Id)
        {
            var exist = db.CustomerRequirementContents.FirstOrDefault(x => x.Id.Equals(Id));
            if (exist != null)
            {
                db.CustomerRequirementContents.Remove(exist);
            }
            db.SaveChanges();
        }

        public CustomerRequirementContentModel SearchCustomerRequirementContentModelById(string id)
        {
            var result = db.CustomerRequirementContents.Where(x => x.Id.Equals(id)).Select(y => new CustomerRequirementContentModel
            {
                Id = y.Id,
                CustomerRequirementId = y.CustomerRequirementId,
                MeetingContentId = y.MeetingContentId,
                Request = y.Request,
                Solution = y.Solution,
                FinishDate = y.FinishDate,
                Note = y.Note,
                Code = y.Code,
                CreateDate = y.CreateDate,
                RequestBy = y.RequestBy
            }).FirstOrDefault();
            return result;
        }
    }
}
