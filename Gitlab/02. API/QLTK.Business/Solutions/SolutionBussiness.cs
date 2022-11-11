using AutoMapper.Mappers;
using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.DesignDocuments;
using NTS.Model.HistoryVersion;
using NTS.Model.ProjectSolution;
using NTS.Model.Repositories;
using NTS.Model.Solution;
using NTS.Model.SolutionDesignDocuments;
using NTS.Model.SolutionGroup;
using NTS.Model.SolutionOldVersion;
using QLTK.Business.AutoMappers;
using QLTK.Business.FileDefinitions;
using QLTK.Business.FolderDefinitions;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;
using NTS.Model.FileDefinition;
using NTS.Model.FolderDefinition;
using QLTK.Business.FileDefinitions;
using QLTK.Business.FolderDefinitions;
using NTS.Model.Survey;
using NTS.Model.CustomerRequirement;
using NTS.Model.Applys;
using NTS.Model.Candidates;
using NTS.Model.Recruitments.Candidates;
using NTS.Model.SurveyContent;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.SurveyMaterial;
using NTS.Model.ProjectAttch;
using Microsoft.VisualBasic.ApplicationServices;
using System.Security;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace QLTK.Business.Solutions
{
    public class SolutionBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ModuleMaterials.ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private readonly FolderDefinitionBusiness folderBusiness = new FolderDefinitionBusiness();
        private readonly FileDefinitionBusiness fileBusiness = new FileDefinitionBusiness();
        private List<SolutionDesignDocumentModel> _documentList = new List<SolutionDesignDocumentModel>();
        public SearchResultModel<SolutionModel> SearchSolution(SolutionSearchModel modelSearch)
        {
            SearchResultModel<SolutionModel> searchResult = new SearchResultModel<SolutionModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.Solutions.AsNoTracking()
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
                             join g in db.SBUs.AsNoTracking() on a.SBUSolutionMakerId equals g.Id into ag
                             from ga in ag.DefaultIfEmpty()
                             join k in db.Departments.AsNoTracking() on a.DepartmentSolutionMakerId equals k.Id into ak
                             from ka in ak.DefaultIfEmpty()
                             orderby a.Code
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
                                 Price = 0,
                                 StartDate = a.StartDate,
                                 FinishDate = a.FinishDate,
                                 SaleNoVat = a.SaleNoVAT,
                                 Description = a.Description,
                                 SBUName = ga == null ? "" : ga.Name,
                                 SBUSolutionMakerId = ga == null ? "" : ga.Id,
                                 DepartmentSolutionMakerId = ka == null ? "" : ka.Id,
                                 DepartmentName = ka == null ? "" : ka.Name,
                                 Index = a.Index,
                                 IsFAS = a.BusinessDomain.Contains("FAS") ? true : false,
                                 IsLAS = a.BusinessDomain.Contains("LAS") ? true : false,
                                 IsEST = a.BusinessDomain.Contains("EST") ? true : false,
                                 IsEnoughDocument = a.IsEnoughDocument,
                                 BusinessDomain = a.BusinessDomain,
                                 CustomerRequirementId = a.CustomerRequirementId

                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Typedocuments))
            {
                if (modelSearch.Typedocuments.Equals("1"))
                {
                    dataQuery = dataQuery.Where(r => r.IsEnoughDocument);
                }
                if (modelSearch.Typedocuments.Equals("2"))
                {
                    dataQuery = dataQuery.Where(r => r.IsEnoughDocument == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.BusinessDomain))
            {
                dataQuery = dataQuery.Where(r => r.BusinessDomain.Equals(modelSearch.BusinessDomain));
            }

            if (!string.IsNullOrEmpty(modelSearch.SolutionGroupId))
            {
                var solutionGroup = db.SolutionGroups.AsNoTracking().FirstOrDefault(i => i.Id.Equals(modelSearch.SolutionGroupId));
                if (solutionGroup != null)
                {
                    listParentId.Add(solutionGroup.Id);
                }
                listParentId.AddRange(GetListParent(modelSearch.SolutionGroupId));
                var listSolutionGroup = listParentId.AsQueryable();
                //var listId = listParentId.ToList();
                //dataQuery = dataQuery.Where(u => u.SolutionGroupId.Equals(modelSearch.SolutionGroupId));
                dataQuery = (from a in dataQuery
                             join b in listSolutionGroup.AsQueryable() on a.SolutionGroupId equals b
                             orderby a.Code
                             select a).AsQueryable();
            }

            if (!string.IsNullOrEmpty(modelSearch.CustomerName))
            {
                dataQuery = dataQuery.Where(u => u.CustomerName.ToUpper().Contains(modelSearch.CustomerName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.EndCustomerName))
            {
                dataQuery = dataQuery.Where(u => u.EndCustomerName.ToUpper().Contains(modelSearch.EndCustomerName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (modelSearch.Status != 0)
            {
                dataQuery = dataQuery.Where(u => u.Status == modelSearch.Status);
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.DepartmentId.Equals(u.DepartmentSolutionMakerId));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.SBUId.Equals(u.SBUSolutionMakerId));
            }

            if (modelSearch.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.StartDate >= modelSearch.DateFrom);
            }

            if (modelSearch.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.StartDate <= modelSearch.DateTo);
            }

            searchResult.Status1 = dataQuery.Where(u => u.Status == 2).Count();
            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            foreach (var item in listResult)
            {
                List<string> listRs = new List<string>();
                item.ProjectSolution = db.ProjectSolutions.AsNoTracking().Where(i => i.SolutionId.Equals(item.Id)).ToList().Count;
                var listProject = (from a in db.ProjectSolutions.AsNoTracking()
                                   join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                                   where a.SolutionId.Equals(item.Id)
                                   select new ProjectSolutionModel
                                   {
                                       ProjectCode = b.Code,
                                       ProjectName = b.Name,
                                   }).AsQueryable().ToList();
                item.ListProjectSolution = listProject;
                if (!string.IsNullOrEmpty(item.CustomerRequirementId))
                {
                    item.CustomerRequirementCode = db.CustomerRequirements.AsNoTracking().Where(r => r.Id.Equals(item.CustomerRequirementId)).FirstOrDefault().Code;
                }
            }
            searchResult.ListResult = listResult;

            int data = 0;
            foreach (var item in listResult)
            {
                if (item.Design3DExist && item.Design2DExist && item.ExplanExist &&
                   item.DMVTExist && item.FCMExist && item.TSTKExist)
                {
                    data++;
                }

                var listSolutionProduct = db.SolutionProducts.AsNoTracking().Where(a => a.SolutionId.Equals(item.Id)).ToList();

                foreach (var itm in listSolutionProduct)
                {
                    decimal temp;
                    if (!string.IsNullOrEmpty(itm.ObjectId))
                    {
                        temp = moduleMaterialBusiness.GetPriceModuleByModuleId(itm.ObjectId, 0) * itm.Quantity;
                    }
                    else
                    {
                        temp = itm.Price * itm.Quantity;
                    }
                    item.Price += temp;
                }
            }

            searchResult.Status2 = data;
            searchResult.Status3 = searchResult.TotalItem - data;
            return searchResult;
        }

        public void AddSolution(SolutionModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Solution solution = new Solution()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SolutionGroupId = model.SolutionGroupId,
                        SBUBusinessId = model.SBUBusinessId,
                        DepartmentBusinessId = model.DepartmentBusinessId,
                        BusinessUserId = model.BusinessUserId,
                        SBUSolutionMakerId = model.SBUSolutionMakerId,
                        DepartmentSolutionMakerId = model.DepartmentSolutionMakerId,
                        SolutionMaker = model.SolutionMaker,
                        Name = model.Name.Trim(),
                        Code = model.Code.Trim(),
                        Description = model.Description.Trim(),
                        CustomerId = model.CustomerId,
                        EndCustomerId = model.EndCustomerId,
                        Price = model.Price,
                        FinishDate = model.FinishDate,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        Status = model.Status,
                        StartDate = model.StartDate,
                        SaleNoVAT = model.SaleNoVat,
                        DepartmentId = model.DepartmentId,
                        Index = model.Index,
                        MechanicalMaker = model.MechanicalMaker,
                        ElectricMaker = model.ElectricMaker,
                        ElectronicMaker = model.ElectronicMaker,
                        CurentVersion = model.CurentVersion,
                        BusinessDomain = model.BusinessDomain,
                        CustomerRequirementId = model.CustomerRequirementId,
                        IsEnoughDocument = model.IsEnoughDocument,
                        JobId = model.JobId,
                        ApplicationId = model.ApplicationId
                    };

                    if (!string.IsNullOrEmpty(model.ProjectId))
                    {
                        ProjectSolution projectSolution = new ProjectSolution()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = solution.Id,
                            ProjectId = model.ProjectId
                        };
                        db.ProjectSolutions.Add(projectSolution);
                    }

                    if (model.ListFile.Count > 0)
                    {
                        List<SolutionAttach> listFileEntity = new List<SolutionAttach>();
                        foreach (var item in model.ListFile)
                        {
                            SolutionAttach fileEntity = new SolutionAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SolutionId = solution.Id,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                Path = item.Path,
                                Type = item.Type,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now
                            };
                            listFileEntity.Add(fileEntity);
                        }
                        db.SolutionAttaches.AddRange(listFileEntity);
                    }

                    if (model.ListProjectSolution.Count > 0)
                    {
                        List<ProjectSolution> listProjectSolution = new List<ProjectSolution>();
                        foreach (var item in model.ListProjectSolution)
                        {
                            ProjectSolution project = new ProjectSolution()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectId = model.Id,
                                SolutionId = solution.Id,
                            };
                            listProjectSolution.Add(project);
                        }
                        db.ProjectSolutions.AddRange(listProjectSolution);
                    }
                    foreach (var item in model.ListImage)
                    {
                        SolutionImage solutionImage = new SolutionImage()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = solution.Id,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            ThumbnailPath = item.ThumbnailPath,
                            Note = item.Note,
                            Type = Constants.SolutionImage_Type_Image,
                            CreateBy = model.CreateBy,
                            CreateDate = DateTime.Now,
                        };
                        db.SolutionImages.Add(solutionImage);
                    }

                    var type = solution.BusinessDomain.Equals("LAS") ? 1 : solution.BusinessDomain.Equals("FAS") ? 2 : 3;

                    FolderDefinitionModel folderSearchModel = new FolderDefinitionModel()
                    {
                        DepartmentId = model.DepartmentId,
                        ObjectType = 4,
                        TypeDefinitionId = type
                    };
                    List<FileDefinitionModel> files = new List<FileDefinitionModel>();
                    List<SolutionDesignDocument> documents = new List<SolutionDesignDocument>();
                    var folders = folderBusiness.GetListFolderDefinition(folderSearchModel);
                    //if (folders.Count > 0)
                    //{
                    //    SolutionDesignDocument document;
                    //    var root = db.SolutionDesignDocuments.FirstOrDefault(t => t.Name.Equals("Thietke.Gp"));
                    //    var rootFolder = folders.FirstOrDefault(t => t.FolderDefinitionFirst.Equals("Thietke.Gp"));
                    //    var listChildRoot = folders.Where(t => rootFolder.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                    //    if (listChildRoot.Count > 0)
                    //    {
                    //        foreach (var item in listChildRoot)
                    //        {
                    //            document = new SolutionDesignDocument()
                    //            {
                    //                Id = Guid.NewGuid().ToString(),
                    //                SolutionId = solution.Id,
                    //                Name = item.Name.Replace("MaGiaiPhap", model.Code),
                    //                ParentId = root.Id,
                    //                FileType = "2",
                    //                CreateBy = model.CreateBy,
                    //                CreateDate = DateTime.Now,
                    //                UpdateBy = model.CreateBy,
                    //                UpdateDate = DateTime.Now,
                    //                CurentVersion = model.CurentVersion
                    //            };
                    //            documents.Add(document);
                    //            FileDefinitionModel fileSearchModel = new FileDefinitionModel()
                    //            {
                    //                FolderDefinitionId = item.FolderDefinitionId,
                    //                TypeDefinitionId = 1
                    //            };
                    //            var listFile = fileBusiness.GetListFileDefinition(fileSearchModel);
                    //            if (listFile.Count > 0)
                    //            {
                    //                foreach (var ite in listFile)
                    //                {
                    //                    var file = new SolutionDesignDocument()
                    //                    {
                    //                        Id = Guid.NewGuid().ToString(),
                    //                        SolutionId = solution.Id,
                    //                        Name = ite.Name.Replace("MaGiaiPhap", model.Code),
                    //                        ParentId = document.Id,
                    //                        FileType = "1",
                    //                        CreateBy = model.CreateBy,
                    //                        CreateDate = DateTime.Now,
                    //                        UpdateBy = model.CreateBy,
                    //                        UpdateDate = DateTime.Now,
                    //                        CurentVersion = model.CurentVersion
                    //                    };
                    //                    documents.Add(file);
                    //                }
                    //            }
                    //            var listChild = folders.Where(t => item.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                    //            if (listChild.Count > 0)
                    //            {
                    //                foreach (var ite in listChild)
                    //                {
                    //                    GetListFolderEntity(folders, documents, ite, model.Code, document.Id, model.CreateBy, solution.Id, model.CurentVersion);
                    //                }
                    //            }
                    //        }
                    //        db.SolutionDesignDocuments.AddRange(documents);
                    //    }
                    //}

                    if (folders.Count > 0)
                    {
                        SolutionDesignDocument document;
                        var root = db.SolutionDesignDocuments.FirstOrDefault(t => t.Name.Equals("Thietke.Gp"));
                        var rootFolder = folders.Where(t => t.FolderDefinitionManageId == null);
                        foreach (var itm in rootFolder)
                        {
                            document = new SolutionDesignDocument()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SolutionId = solution.Id,
                                Name = itm.Name.Replace("MaGiaiPhap", model.Code),
                                ParentId = root.Id,
                                FileType = "2",
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now,
                                CurentVersion = model.CurentVersion
                            };
                            documents.Add(document);

                            var listChildRoot = folders.Where(t => itm.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                            if (listChildRoot.Count > 0)
                            {
                                foreach (var item in listChildRoot)
                                {
                                    document = new SolutionDesignDocument()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        SolutionId = solution.Id,
                                        Name = item.Name.Replace("MaGiaiPhap", model.Code),
                                        ParentId = root.Id,
                                        FileType = "2",
                                        CreateBy = model.CreateBy,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = model.CreateBy,
                                        UpdateDate = DateTime.Now,
                                        CurentVersion = model.CurentVersion
                                    };
                                    documents.Add(document);
                                    FileDefinitionModel fileSearchModel = new FileDefinitionModel()
                                    {
                                        FolderDefinitionId = item.FolderDefinitionId,
                                        TypeDefinitionId = 1
                                    };
                                    var listFile = fileBusiness.GetListFileDefinition(fileSearchModel);
                                    if (listFile.Count > 0)
                                    {
                                        foreach (var ite in listFile)
                                        {
                                            var file = new SolutionDesignDocument()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                SolutionId = solution.Id,
                                                Name = ite.Name.Replace("MaGiaiPhap", model.Code),
                                                ParentId = document.Id,
                                                FileType = "1",
                                                CreateBy = model.CreateBy,
                                                CreateDate = DateTime.Now,
                                                UpdateBy = model.CreateBy,
                                                UpdateDate = DateTime.Now,
                                                CurentVersion = model.CurentVersion
                                            };
                                            documents.Add(file);
                                        }
                                    }
                                    var listChild = folders.Where(t => item.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                                    if (listChild.Count > 0)
                                    {
                                        foreach (var ite in listChild)
                                        {
                                            GetListFolderEntity(folders, documents, ite, model.Code, document.Id, model.CreateBy, solution.Id, model.CurentVersion);
                                        }
                                    }
                                }

                            }
                        }

                        db.SolutionDesignDocuments.AddRange(documents);

                    }

                    db.Solutions.Add(solution);
                    var listSolutionTechnologies = db.SolutionTechnologies.ToList();
                    foreach (var item in model.SolutionTechnologies)
                    {
                        var solutionTechnologies = listSolutionTechnologies.Where(r => r.Name.ToUpper().Equals(item.ToUpper())).FirstOrDefault();
                        TechInSolution techInSolution = new TechInSolution
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = solution.Id,
                            TechnologyId = solutionTechnologies.Id

                        };
                        db.TechInSolutions.Add(techInSolution);
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, solution.Code, solution.Id, Constants.LOG_Solution);
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
        public void GetListFolderEntity(List<FolderDefinitionModel> folders, List<SolutionDesignDocument> documents, FolderDefinitionModel folder, string code, string id, string userId, string solutionId, int version)
        {
            SolutionDesignDocument document = new SolutionDesignDocument()
            {
                Id = Guid.NewGuid().ToString(),
                SolutionId = solutionId,
                Name = folder.Name.Replace("MaGiaiPhap", code),
                ParentId = id,
                FileType = "2",
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
                CurentVersion = version
            };
            documents.Add(document);
            FileDefinitionModel fileSearchModel = new FileDefinitionModel()
            {
                FolderDefinitionId = folder.FolderDefinitionId,
                TypeDefinitionId = 1
            };
            var listFile = fileBusiness.GetListFileDefinition(fileSearchModel);
            if (listFile.Count > 0)
            {
                foreach (var ite in listFile)
                {
                    var file = new SolutionDesignDocument()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SolutionId = solutionId,
                        Name = ite.Name.Replace("MaGiaiPhap", code),
                        ParentId = document.Id,
                        FileType = "1",
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                        CurentVersion = version
                    };
                    documents.Add(file);
                }
            }
            var listChild = folders.Where(t => folder.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
            if (listChild.Count > 0)
            {
                foreach (var ite in listChild)
                {
                    GetListFolderEntity(folders, documents, ite, code, document.Id, userId, solutionId, version);
                }
            }
        }
        public void UpdateSolution(SolutionModel model, string userId)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            var solutions = db.Solutions.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
            bool isDomain = false;
            if (solutions != null)
            {
                if (!string.IsNullOrEmpty(solutions.BusinessDomain))
                {
                    if (!solutions.BusinessDomain.Contains(model.BusinessDomain))
                    {
                        isDomain = true;
                    }
                }
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var solution = db.Solutions.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SolutionHistoryModel>(solution);

                    solution.SolutionGroupId = model.SolutionGroupId;
                    solution.SBUBusinessId = model.SBUBusinessId;
                    solution.DepartmentBusinessId = model.DepartmentBusinessId;
                    solution.BusinessUserId = model.BusinessUserId;
                    solution.SBUSolutionMakerId = model.SBUSolutionMakerId;
                    solution.DepartmentSolutionMakerId = model.DepartmentSolutionMakerId;
                    solution.SolutionMaker = model.SolutionMaker;
                    solution.Name = model.Name.NTSTrim();
                    solution.Code = model.Code.NTSTrim();
                    solution.Description = model.Description.NTSTrim();
                    solution.CustomerId = model.CustomerId;
                    solution.EndCustomerId = model.EndCustomerId;
                    //solution.Price = model.Price;
                    solution.FinishDate = model.FinishDate;
                    solution.UpdateBy = model.UpdateBy;
                    solution.UpdateDate = DateTime.Now;
                    solution.Status = model.Status;
                    solution.StartDate = model.StartDate;
                    solution.SaleNoVAT = model.SaleNoVat;
                    solution.MechanicalMaker = model.MechanicalMaker;
                    solution.ElectricMaker = model.ElectricMaker;
                    solution.ElectronicMaker = model.ElectronicMaker;
                    solution.CurentVersion = model.CurentVersion;
                    solution.BusinessDomain = model.BusinessDomain;
                    solution.CustomerRequirementId = model.CustomerRequirementId;
                    solution.IsEnoughDocument = model.IsEnoughDocument;
                    solution.JobId = model.JobId;
                    solution.ApplicationId = model.ApplicationId;
                    //var projectSolution = db.ProjectSolutions.FirstOrDefault(r => model.Id.Equals(r.SolutionId));
                    //if (projectSolution != null)
                    //{
                    //    projectSolution.ProjectId = model.ProjectId;
                    //}
                    //else if (!string.IsNullOrEmpty(model.ProjectId))
                    //{
                    //    ProjectSolution projectSolutions = new ProjectSolution()
                    //    {
                    //        Id = Guid.NewGuid().ToString(),
                    //        SolutionId = solution.Id,
                    //        ProjectId = model.ProjectId
                    //    };
                    //    db.ProjectSolutions.Add(projectSolutions);
                    //}



                    var fileEntities = db.SolutionAttaches.Where(t => t.SolutionId.Equals(model.Id)).ToList();
                    if (fileEntities != null)
                    {
                        db.SolutionAttaches.RemoveRange(fileEntities);
                    }

                    if (model.ListFile.Count > 0)
                    {
                        List<SolutionAttach> listFileEntity = new List<SolutionAttach>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null && item.Path != "")
                            {
                                SolutionAttach fileEntity = new SolutionAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SolutionId = model.Id,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    Path = item.Path,
                                    Type = item.Type,
                                    CreateBy = model.UpdateBy,
                                    CreateDate = DateTime.Now
                                };
                                listFileEntity.Add(fileEntity);
                            }

                        }
                        db.SolutionAttaches.AddRange(listFileEntity);
                    }

                    var listSolutionProduct = db.SolutionProducts.Where(a => a.SolutionId.Equals(model.Id)).ToList();

                    foreach (var item in listSolutionProduct)
                    {
                        decimal temp;
                        if (!string.IsNullOrEmpty(item.ObjectId))
                        {
                            temp = moduleMaterialBusiness.GetPriceModuleByModuleId(item.ObjectId, 0) * item.Quantity;
                        }
                        else
                        {
                            temp = item.Price * item.Quantity;
                        }
                        solution.Price += temp;
                    }

                    var listImage = db.SolutionImages.Where(a => a.SolutionId.Equals(model.Id));
                    db.SolutionImages.RemoveRange(listImage);

                    foreach (var item in model.ListImage)
                    {
                        SolutionImage solutionImage = new SolutionImage()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = model.Id,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            ThumbnailPath = item.ThumbnailPath,
                            Note = item.Note,
                            Type = item.Type,
                            CreateBy = model.UpdateBy,
                            CreateDate = DateTime.Now,
                        };
                        db.SolutionImages.Add(solutionImage);
                    }

                    var listProjectSolution = db.ProjectSolutions.Where(a => a.SolutionId.Equals(model.Id));
                    db.ProjectSolutions.RemoveRange(listProjectSolution);
                    foreach (var item in model.ListProjectSolution)
                    {
                        NTS.Model.Repositories.ProjectSolution projectSolution1 = new NTS.Model.Repositories.ProjectSolution()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = model.Id,
                            ProjectId = item.ProjectId,
                        };
                        db.ProjectSolutions.Add(projectSolution1);

                    }

                    var listTechSolution = db.TechInSolutions.Where(a => a.SolutionId.Equals(model.Id));
                    db.TechInSolutions.RemoveRange(listTechSolution);
                    var listSolutionTechnologies = db.SolutionTechnologies.ToList();
                    foreach (var item in model.SolutionTechnologies)
                    {
                        var solutionTechnologies = listSolutionTechnologies.Where(r => r.Name.ToUpper().Equals(item.ToUpper())).FirstOrDefault();
                        TechInSolution techInSolution = new TechInSolution
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = solution.Id,
                            TechnologyId = solutionTechnologies.Id

                        };
                        db.TechInSolutions.Add(techInSolution);
                    }

                    if (!string.IsNullOrEmpty(model.CustomerRequirementId))
                    {
                        var customerRequirement = db.CustomerRequirements.Where(r => r.Id.Equals(model.CustomerRequirementId)).FirstOrDefault();
                        if (customerRequirement != null)
                        {
                            customerRequirement.SurveyState = model.SurveyState;
                            customerRequirement.DoSolutionAnalysisState = model.DoSolutionAnalysisState;
                        }
                    }

                    var customerRequirementEstimateExist = db.CustomerRequirementEstimates.FirstOrDefault(a => a.CustomerRequirementId.Equals(model.CustomerRequirementId));
                    string customerRequirementEstimateId;
                    if (customerRequirementEstimateExist == null)
                    {
                        CustomerRequirementEstimate customerRequirementEstimate = new CustomerRequirementEstimate()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CustomerRequirementId = model.CustomerRequirementId,
                            TotalPrice = model.TotalPrice,
                            TradeConditions = model.TradeConditions
                        };
                        db.CustomerRequirementEstimates.Add(customerRequirementEstimate);
                        customerRequirementEstimateId = customerRequirementEstimate.Id;
                    }
                    else
                    {
                        customerRequirementEstimateExist.TotalPrice = model.TotalPrice;
                        customerRequirementEstimateExist.TradeConditions = model.TradeConditions;
                        customerRequirementEstimateId = customerRequirementEstimateExist.Id;
                    }

                    CustomerRequirementEstimateAttach customerRequirementEstimateAttach;
                    foreach (var item in model.ListRequireEstimateMaterialAttach)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            customerRequirementEstimateAttach = new CustomerRequirementEstimateAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CustomerRequirementEstimateId = customerRequirementEstimateId,
                                Name = item.Name,
                                Type = item.Type,
                                Note = item.Note,
                                FilePath = item.FilePath,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now,
                            };

                            db.CustomerRequirementEstimateAttaches.Add(customerRequirementEstimateAttach);
                        }
                        else
                        {
                            customerRequirementEstimateAttach = db.CustomerRequirementEstimateAttaches.FirstOrDefault(r => r.Id.Equals(item.Id));

                            if (customerRequirementEstimateAttach != null)
                            {
                                if (item.IsDelete)
                                {
                                    db.CustomerRequirementEstimateAttaches.Remove(customerRequirementEstimateAttach);
                                }
                                else
                                {
                                    customerRequirementEstimateAttach.Name = item.Name;
                                    customerRequirementEstimateAttach.Note = item.Note;
                                    customerRequirementEstimateAttach.Type = item.Type;
                                    customerRequirementEstimateAttach.FileName = item.FileName;
                                    customerRequirementEstimateAttach.FilePath = item.FilePath;
                                    customerRequirementEstimateAttach.FileSize = item.FileSize;
                                    customerRequirementEstimateAttach.UpdateBy = userId;
                                    customerRequirementEstimateAttach.UpdateDate = DateTime.Now;
                                }
                            }
                        }
                    }

                    if (isDomain)
                    {
                        var listSolutionDesignDocuments = db.SolutionDesignDocuments.Where(r => r.SolutionId.Equals(solution.Id));
                        if (listSolutionDesignDocuments.ToList().Count > 0)
                        {
                            db.SolutionDesignDocuments.RemoveRange(listSolutionDesignDocuments);
                        }

                        var type = solution.BusinessDomain.Equals("LAS") ? 1 : solution.BusinessDomain.Equals("FAS") ? 2 : 3;

                        FolderDefinitionModel folderSearchModel = new FolderDefinitionModel()
                        {
                            DepartmentId = model.DepartmentId,
                            ObjectType = 4,
                            TypeDefinitionId = type
                        };
                        List<FileDefinitionModel> files = new List<FileDefinitionModel>();
                        List<SolutionDesignDocument> documents = new List<SolutionDesignDocument>();
                        var folders = folderBusiness.GetListFolderDefinition(folderSearchModel);

                        if (folders.Count > 0)
                        {
                            SolutionDesignDocument document;
                            var root = db.SolutionDesignDocuments.FirstOrDefault(t => t.Name.Equals("Thietke.Gp"));
                            var rootFolder = folders.Where(t => t.FolderDefinitionManageId == null);
                            foreach (var itm in rootFolder)
                            {
                                document = new SolutionDesignDocument()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SolutionId = solution.Id,
                                    Name = itm.Name.Replace("MaGiaiPhap", model.Code),
                                    ParentId = root.Id,
                                    FileType = "2",
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now,
                                    CurentVersion = model.CurentVersion
                                };
                                documents.Add(document);
                                var newdocument = document;
                                var listChildRoot = folders.Where(t => itm.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                                if (listChildRoot.Count > 0)
                                {
                                    foreach (var item in listChildRoot)
                                    {
                                        document = new SolutionDesignDocument()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SolutionId = solution.Id,
                                            Name = item.Name.Replace("MaGiaiPhap", model.Code),
                                            ParentId = newdocument.Id,
                                            FileType = "2",
                                            CreateBy = userId,
                                            CreateDate = DateTime.Now,
                                            UpdateBy = userId,
                                            UpdateDate = DateTime.Now,
                                            CurentVersion = model.CurentVersion
                                        };
                                        documents.Add(document);
                                        FileDefinitionModel fileSearchModel = new FileDefinitionModel()
                                        {
                                            FolderDefinitionId = item.FolderDefinitionId,
                                            TypeDefinitionId = 1
                                        };
                                        var listFile = fileBusiness.GetListFileDefinition(fileSearchModel);
                                        if (listFile.Count > 0)
                                        {
                                            foreach (var ite in listFile)
                                            {
                                                var file = new SolutionDesignDocument()
                                                {
                                                    Id = Guid.NewGuid().ToString(),
                                                    SolutionId = solution.Id,
                                                    Name = ite.Name.Replace("MaGiaiPhap", model.Code),
                                                    ParentId = document.Id,
                                                    FileType = "1",
                                                    CreateBy = userId,
                                                    CreateDate = DateTime.Now,
                                                    UpdateBy = userId,
                                                    UpdateDate = DateTime.Now,
                                                    CurentVersion = model.CurentVersion
                                                };
                                                documents.Add(file);
                                            }
                                        }
                                        var listChild = folders.Where(t => item.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                                        if (listChild.Count > 0)
                                        {
                                            foreach (var ite in listChild)
                                            {
                                                GetListFolderEntity(folders, documents, ite, model.Code, document.Id, userId, solution.Id, model.CurentVersion);
                                            }
                                        }
                                    }

                                }
                            }

                            db.SolutionDesignDocuments.AddRange(documents);

                        }

                    }
                    else if (!string.IsNullOrEmpty(solution.BusinessDomain))
                    {
                        var list = db.SolutionDesignDocuments.Where(t => t.FileType.Equals(Constants.SolutionDesignDocument_FileType_Folder) && t.SolutionId.Equals(solution.Id) && t.CurentVersion == solution.CurentVersion);
                        if (list != null)
                        {

                            foreach (var item in list)
                            {
                                var listFile = db.SolutionDesignDocuments.Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_File) && t.ParentId.Equals(item.Id));
                                db.SolutionDesignDocuments.RemoveRange(listFile);
                            }

                            db.SolutionDesignDocuments.RemoveRange(list);

                        }
                        var type = solution.BusinessDomain.Equals("LAS") ? 1 : solution.BusinessDomain.Equals("FAS") ? 2 : 3;

                        FolderDefinitionModel folderSearchModel = new FolderDefinitionModel()
                        {
                            DepartmentId = model.DepartmentId,
                            ObjectType = 4,
                            TypeDefinitionId = type
                        };
                        List<FileDefinitionModel> files = new List<FileDefinitionModel>();
                        List<SolutionDesignDocument> documents = new List<SolutionDesignDocument>();
                        var folders = folderBusiness.GetListFolderDefinition(folderSearchModel);

                        if (folders.Count > 0)
                        {
                            SolutionDesignDocument document;
                            var root = db.SolutionDesignDocuments.FirstOrDefault(t => t.Name.Equals("Thietke.Gp"));
                            var rootFolder = folders.Where(t => t.FolderDefinitionManageId == null);
                            foreach (var itm in rootFolder)
                            {
                                var listChildRoot = folders.Where(t => itm.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                                if (listChildRoot.Count > 0)
                                {
                                    foreach (var item in listChildRoot)
                                    {
                                        document = new SolutionDesignDocument()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SolutionId = solution.Id,
                                            Name = item.Name.Replace("MaGiaiPhap", model.Code),
                                            ParentId = root.Id,
                                            FileType = "2",
                                            CreateBy = userId,
                                            CreateDate = DateTime.Now,
                                            UpdateBy = userId,
                                            UpdateDate = DateTime.Now,
                                            CurentVersion = model.CurentVersion
                                        };
                                        documents.Add(document);
                                        FileDefinitionModel fileSearchModel = new FileDefinitionModel()
                                        {
                                            FolderDefinitionId = item.FolderDefinitionId,
                                            TypeDefinitionId = 1
                                        };
                                        var listFile = fileBusiness.GetListFileDefinition(fileSearchModel);
                                        if (listFile.Count > 0)
                                        {
                                            foreach (var ite in listFile)
                                            {
                                                var file = new SolutionDesignDocument()
                                                {
                                                    Id = Guid.NewGuid().ToString(),
                                                    SolutionId = solution.Id,
                                                    Name = ite.Name.Replace("MaGiaiPhap", model.Code),
                                                    ParentId = document.Id,
                                                    FileType = "1",
                                                    CreateBy = userId,
                                                    CreateDate = DateTime.Now,
                                                    UpdateBy = userId,
                                                    UpdateDate = DateTime.Now,
                                                    CurentVersion = model.CurentVersion
                                                };
                                                documents.Add(file);
                                            }
                                        }
                                        var listChild = folders.Where(t => item.FolderDefinitionId.Equals(t.FolderDefinitionManageId)).ToList();
                                        if (listChild.Count > 0)
                                        {
                                            foreach (var ite in listChild)
                                            {
                                                GetListFolderEntity(folders, documents, ite, model.Code, document.Id, userId, solution.Id, model.CurentVersion);
                                            }
                                        }
                                    }

                                }
                            }

                            db.SolutionDesignDocuments.AddRange(documents);

                        }


                    }


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SolutionHistoryModel>(solution);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Solution, solution.Id, solution.Code, jsonBefor, jsonApter);

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

        public void DeleteSolution(SolutionModel model, string departmentId, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var solution = db.Solutions.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (solution == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Solution);
                }
                if (!string.IsNullOrEmpty(solution.DepartmentId) && !solution.DepartmentId.Equals(departmentId))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0035, TextResourceKey.Solution);
                }
                try
                {
                    var techSolution = db.TechInSolutions.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (techSolution.Count() > 0)
                    {
                        db.TechInSolutions.RemoveRange(techSolution);
                    }
                    var projectSolutions = db.ProjectSolutions.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (projectSolutions.Count() > 0)
                    {
                        db.ProjectSolutions.RemoveRange(projectSolutions);
                    }

                    var requirementEstimate = db.CustomerRequirementEstimates.Where(h => h.CustomerRequirementId.Equals(model.CustomerRequirementId));
                    var requirementEstimateId = requirementEstimate.Select(r => r.Id).ToList();
                    var customerRequirementEstimateAttach = db.CustomerRequirementEstimateAttaches.Where(k => requirementEstimateId.Contains(k.CustomerRequirementEstimateId));
                    if (customerRequirementEstimateAttach.Count() > 0)
                    {
                        db.CustomerRequirementEstimateAttaches.RemoveRange(customerRequirementEstimateAttach);
                    }

                    var file = db.SolutionAttaches.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (file.Count() > 0)
                    {
                        db.SolutionAttaches.RemoveRange(file);
                    }
                    var solutionFeatures = db.SolutionFeatures.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (solutionFeatures.Count() > 0)
                    {
                        db.SolutionFeatures.RemoveRange(solutionFeatures);
                    }
                    var solutionImages = db.SolutionImages.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (solutionImages.Count() > 0)
                    {
                        db.SolutionImages.RemoveRange(solutionImages);
                    }
                    var solutionProducts = db.SolutionProducts.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (solutionProducts.Count() > 0)
                    {
                        db.SolutionProducts.RemoveRange(solutionProducts);
                    }
                    var solutionOldVersions = db.SolutionOldVersions.Where(u => u.SolutionId.Equals(model.Id)).ToList();
                    if (solutionOldVersions.Count() > 0)
                    {
                        db.SolutionOldVersions.RemoveRange(solutionOldVersions);
                    }
                    var jsonBefor = AutoMapperConfig.Mapper.Map<List<SolutionHistoryModel>>(projectSolutions);
                    UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_Solution, solution.Id, solution.Code, jsonBefor);

                    db.Solutions.Remove(solution);
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
        public SolutionModel GetSolutionInfo(SolutionModel model)
        {
            var resultInfo = db.Solutions.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SolutionModel
            {
                Id = p.Id,
                SolutionGroupId = p.SolutionGroupId,
                SBUBusinessId = p.SBUBusinessId,
                DepartmentBusinessId = p.DepartmentBusinessId,
                BusinessUserId = p.BusinessUserId,
                SBUSolutionMakerId = p.SBUSolutionMakerId,
                DepartmentSolutionMakerId = p.DepartmentSolutionMakerId,
                SolutionMaker = p.SolutionMaker,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                CustomerId = p.CustomerId,
                EndCustomerId = p.EndCustomerId,
                Price = 0,
                FinishDate = p.FinishDate,
                JobId = p.JobId,
                ApplicationId = p.ApplicationId,


                Status = p.Status,
                SaleNoVat = p.SaleNoVAT,
                StartDate = p.StartDate,
                MechanicalMaker = p.MechanicalMaker,
                ElectricMaker = p.ElectricMaker,
                ElectronicMaker = p.ElectronicMaker,
                CurentVersion = p.CurentVersion,
                IsEnoughDocument = p.IsEnoughDocument,
                CustomerRequirementId = p.CustomerRequirementId,
                BusinessDomain = p.BusinessDomain,

            }).FirstOrDefault();
            if (!string.IsNullOrEmpty(resultInfo.CustomerRequirementId))
            {
                resultInfo.SurveyState = (int?)db.CustomerRequirements.Where(r => r.Id.Equals(resultInfo.CustomerRequirementId)).FirstOrDefault().SurveyState;
                resultInfo.DoSolutionAnalysisState = (int?)db.CustomerRequirements.Where(r => r.Id.Equals(resultInfo.CustomerRequirementId)).FirstOrDefault().DoSolutionAnalysisState;
            }

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Solution);
            }

            var project = db.ProjectSolutions.AsNoTracking().FirstOrDefault(i => model.Id.Equals(i.SolutionId));

            if (project != null)
            {

                resultInfo.ProjectId = project.ProjectId;
            }

            resultInfo.ListFile = db.SolutionAttaches.AsNoTracking().Where(t => t.SolutionId.Equals(resultInfo.Id)).Select(m => new SolutionAttachModel
            {
                Id = m.Id,
                SolutionId = m.SolutionId,
                FileName = m.FileName,
                Path = m.Path,
                FileSize = m.FileSize,
                Type = m.Type
            }).ToList();

            var listSolutionProduct = db.SolutionProducts.AsNoTracking().Where(a => a.SolutionId.Equals(model.Id)).ToList();

            foreach (var item in listSolutionProduct)
            {
                decimal temp = 0;
                if (!string.IsNullOrEmpty(item.ObjectId))
                {
                    temp = moduleMaterialBusiness.GetPriceModuleByModuleId(item.ObjectId, 0) * item.Quantity;
                }
                else
                {
                    temp = item.Price * item.Quantity;
                }
                resultInfo.Price += temp;
            }



            //var data = (from a in db.ProjectSolutions.AsNoTracking()
            //            join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
            //            join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
            //            join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
            //            join e in db.Customers.AsNoTracking() on b.CustomerId equals e.Id
            //            where a.SolutionId.Equals(model.Id)
            //            select new ProjectSolutionModel()
            //            {
            //                Id = a.Id,
            //                ProjectId = b.Id,
            //                ProjectName = b.Name,
            //                ProjectCode = b.Code,
            //                Customer = e.Name,
            //                SBUName = c.Name,
            //                DepartmentName = d.Name
            //            }).ToList();

            var listImage = (from a in db.SolutionImages.AsNoTracking()
                             where a.SolutionId.Equals(model.Id)
                             select new SolutionImageModel
                             {
                                 FileName = a.FileName,
                                 FilePath = a.FilePath,
                                 ThumbnailPath = a.ThumbnailPath,
                                 Note = a.Note,
                                 Type = a.Type
                             }).ToList();

            resultInfo.ListImage = listImage;

            //resultInfo.ListProjectSolution.AddRange(data);

            resultInfo.ListSurvey = (from a in db.Surveys.AsNoTracking()
                                     where a.CustomerRequirementId.Equals(resultInfo.CustomerRequirementId)
                                     select new SurveyCreateModel
                                     {
                                         Id = a.Id,
                                         CustomerRequirementId = a.CustomerRequirementId,
                                         //ProjectPhaseId = a.ProjectPhaseId,
                                         SurveyDate = a.SurveyDate,
                                         Times = a.Times,
                                         CustomerContactId = a.CustomerContactId,
                                         Description = a.Description,
                                     }).ToList();
            var lisCustomerContact = db.CustomerContacts.AsNoTracking();
            if (resultInfo.ListSurvey.Count > 0)
            {
                foreach (var item in resultInfo.ListSurvey)
                {
                    if (!string.IsNullOrEmpty(item.Times))
                    {
                        item.Time = JsonConvert.DeserializeObject<object>(item.Times);
                    }
                    if (!string.IsNullOrEmpty(item.CustomerContactId))
                    {
                        var name = lisCustomerContact.FirstOrDefault(r => r.Id.Equals(item.CustomerContactId)).Name;
                        if (!string.IsNullOrEmpty(name))
                        {
                            item.CustomerContactName = name;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.Times))
                    {
                        item.Time = JsonConvert.DeserializeObject<object>(item.Times);
                    }
                }
            }
            var customerRequirementEstimate = db.CustomerRequirementEstimates.AsNoTracking().FirstOrDefault(a => a.CustomerRequirementId.Equals(resultInfo.CustomerRequirementId));
            if (customerRequirementEstimate != null)
            {
                resultInfo.ListRequireEstimateMaterialAttach = (from m in db.CustomerRequirementEstimateAttaches.AsNoTracking()
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

            var listechSolution = (from a in db.TechInSolutions.AsNoTracking()
                                   where a.SolutionId.Equals(model.Id)
                                   join b in db.SolutionTechnologies.AsNoTracking() on a.TechnologyId equals b.Id
                                   select new { b.Name }
                                       ).ToList();
            if (listechSolution.Count > 0)
            {
                foreach (var item in listechSolution)
                {
                    resultInfo.SolutionTechnologies.Add(item.Name);
                }
            }
            return resultInfo;
        }

        private int GetValue(SolutionModel resultInfo)
        {
            return db.CustomerRequirements.Where(r => r.Id.Equals(resultInfo.CustomerRequirementId)).FirstOrDefault().DoSolutionAnalysisState.Value;
        }

        public string ExportExcel(SolutionSearchModel model)
        {
            model.IsExport = true;

            var dataQuery = (from a in db.Solutions.AsNoTracking()
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
                             join f in db.ProjectSolutions.AsNoTracking() on a.Id equals f.SolutionId into fa
                             from af in fa.DefaultIfEmpty()
                             join m in db.Projects.AsNoTracking() on af.ProjectId equals m.Id into ma
                             from am in ma.DefaultIfEmpty()
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

                                 ProjectName = am.Name,
                                 ProjectCode = am.Code,
                                 Price = a.Price,
                                 StartDate = a.StartDate,
                                 FinishDate = a.FinishDate,
                                 SaleNoVat = a.SaleNoVAT,
                                 Description = a.Description,
                                 SBUName = ga == null ? "" : ga.Name,
                                 SBUBusinessId = ga == null ? "" : ga.Id,
                                 DepartmentBusinessId = ka == null ? "" : ka.Id,
                                 DepartmentName = ka == null ? "" : ka.Name,
                                 Index = a.Index
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.SolutionGroupId))
            {
                dataQuery = dataQuery.Where(u => u.SolutionGroupId.Equals(model.SolutionGroupId));
            }

            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                dataQuery = dataQuery.Where(u => u.CustomerName.ToUpper().Contains(model.CustomerName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.EndCustomerName))
            {
                dataQuery = dataQuery.Where(u => u.EndCustomerName.ToUpper().Contains(model.EndCustomerName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()) || u.Name.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (model.Status != 0)
            {
                dataQuery = dataQuery.Where(u => u.Status == model.Status);
            }

            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentBusinessId.ToUpper().Contains(model.DepartmentId.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUBusinessId.ToUpper().Contains(model.SBUId.ToUpper()));
            }

            if (model.DateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.StartDate >= model.DateFrom);
            }

            if (model.DateTo.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.StartDate <= model.DateTo);

            }

            //var dataQuery = (from a in db.Solutions.AsNoTracking()
            //                 join b in db.SolutionGroups.AsNoTracking() on a.SolutionGroupId equals b.Id
            //                 join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id
            //                 join d in db.Customers.AsNoTracking() on a.EndCustomerId equals d.Id
            //                 join e in db.Employees.AsNoTracking() on a.BusinessUserId equals e.Id
            //                 join f in db.ProjectSolutions.AsNoTracking() on a.Id equals f.SolutionId
            //                 join g in db.Projects.AsNoTracking() on f.ProjectId equals g.Id
            //                 orderby a.Code
            //                 select new SolutionModel
            //                 {
            //                     Id = a.Id,
            //                     Name = a.Name,
            //                     Code = a.Code,
            //                     Description = a.Description,
            //                     CustomerId = a.CustomerId,
            //                     ProjectName = g.Name,
            //                     ProjectCode = g.Code,
            //                     CustomerName = c.Name,
            //                     EndCustomerId = a.EndCustomerId,
            //                     EndCustomerName = d.Name,
            //                     BusinessUserId = a.BusinessUserId,
            //                     TPAUName = e.Name,
            //                     Price = a.Price,
            //                     FinishDate = a.FinishDate,
            //                     SolutionGroupId = a.SolutionGroupId,
            //                     SolutionGroupName = b.Name,
            //                     Status = a.Status,
            //                 }).AsQueryable();
            //if (!string.IsNullOrEmpty(model.SolutionGroupId))
            //{
            //    dataQuery = dataQuery.Where(u => u.SolutionGroupId.ToUpper().Contains(model.SolutionGroupId.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(model.Name))
            //{
            //    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(model.Code))
            //{
            //    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            //}

            List<SolutionModel> listModel = dataQuery.ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Solution.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.Code,
                    a.SolutionGroupName,
                    a.ProjectCode,
                    a.ProjectName,
                    b = a.Status == 1 ? "Đang triển khai" : a.Status == 2 ? "Tạm dừng" : a.Status == 3 ? "Hủy" : a.Status == 4 ? "Đã hoàn thành" : "",
                    a.CustomerName,
                    a.EndCustomerName,
                    a.TPAUName,
                    a.Price,
                    a.FinishDate,
                    a.Description
                });

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 13].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách giải pháp" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách giải pháp" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        private void CheckExistedForAdd(SolutionModel model)
        {
            if (db.Solutions.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Solution);
            }

            if (db.Solutions.AsNoTracking().Where(o => o.Code.ToUpper().Equals(model.Code.ToUpper())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Solution);
            }
        }

        public void CheckExistedForUpdate(SolutionModel model)
        {
            if (db.Solutions.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Solution);
            }

            if (db.Solutions.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.ToUpper().Equals(model.Code.ToUpper())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Solution);
            }
        }

        public SearchResultModel<ChooseProjectSolutionModel> SearchProjectSolution(ChooseProjectSolutionModel model)
        {
            SearchResultModel<ChooseProjectSolutionModel> searchResult = new SearchResultModel<ChooseProjectSolutionModel>();

            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                             from ba in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from da in ad.DefaultIfEmpty()
                             join e in db.ProjectProducts.AsNoTracking() on a.Id equals e.ProductId into ae
                             from ea in ae.DefaultIfEmpty()
                             where !model.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new ChooseProjectSolutionModel()
                             {
                                 ProjectId = a.Id,
                                 ProjectName = a.Name,
                                 ProjectCode = a.Code,
                                 Customer = ba.Name,
                                 SBUName = ca.Name,
                                 DepartmentName = da.Name,
                                 ProjectProductId = ea.Id,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.ProjectName))
            {
                dataQuery = dataQuery.Where(u => u.ProjectName.Equals(model.ProjectName));
            }
            if (!string.IsNullOrEmpty(model.ProjectCode))
            {
                dataQuery = dataQuery.Where(u => u.ProjectCode.Equals(model.ProjectCode));
            }
            var lstRs = dataQuery.GroupBy(t => new { t.ProjectId, t.ProjectName, t.ProjectCode, t.Customer, t.SBUName, t.DepartmentName }).ToList();
            List<ChooseProjectSolutionModel> listRs = new List<ChooseProjectSolutionModel>();
            foreach (var item in lstRs)
            {
                ChooseProjectSolutionModel rs = new ChooseProjectSolutionModel();
                rs.ProjectId = item.Key.ProjectId;
                rs.ProjectName = item.Key.ProjectName;
                rs.ProjectCode = item.Key.ProjectCode;
                rs.Customer = item.Key.Customer;
                rs.SBUName = item.Key.SBUName;
                rs.DepartmentName = item.Key.DepartmentName;
                listRs.Add(rs);
            }
            searchResult.TotalItem = listRs.Count();
            var listResult = listRs.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SolutionModel GetSolutionCode(SolutionModel model)
        {
            var solutionIndex = 0;
            var index = (from a in db.Solutions.AsNoTracking()
                         join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                         where b.SBUId.Equals(model.SBUId)
                         select new
                         {
                             a.Index
                         }).ToList();
            if (index.Count > 0)
            {
                solutionIndex = index.Max(i => i.Index);
            }
            solutionIndex++;
            model.Index = solutionIndex;
            var year = DateTime.Now.Year;
            var sBUs = db.SBUs.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.SBUId));
            if (sBUs != null)
            {
                var lengthIndex = solutionIndex.ToString().Length;
                switch (lengthIndex)
                {
                    case 1:
                        model.Code = sBUs.Code + ".GP." + year + ".000" + solutionIndex;
                        break;
                    case 2:
                        model.Code = sBUs.Code + ".GP." + year + ".00" + solutionIndex;
                        break;
                    case 3:
                        model.Code = sBUs.Code + ".GP." + year + ".0" + solutionIndex;
                        break;
                    case 4:
                        model.Code = sBUs.Code + ".GP." + year + "." + solutionIndex;
                        break;
                    default:
                        break;
                }
            }

            return model;
        }

        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var solutionGroups = db.SolutionGroups.AsNoTracking().Where(i => i.ParentId.Equals(id)).Select(i => i.Id).ToList();
            listChild.AddRange(solutionGroups);
            if (solutionGroups.Count > 0)
            {
                foreach (var item in solutionGroups)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }

        public List<SolutionDesignDocumentModel> GetListFolderSolution(string solutionId, int curentVersion)
        {
            var solution = db.Solutions.AsNoTracking().Where(r => r.Id.Equals(solutionId)).FirstOrDefault().BusinessDomain;
            List<SolutionDesignDocumentModel> list = new List<SolutionDesignDocumentModel>();
            list = db.SolutionDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.SolutionDesignDocument_FileType_Folder) && t.SolutionId.Equals(solutionId) && t.CurentVersion == curentVersion)
                .OrderBy(o => o.Path)
                .Select(m => new SolutionDesignDocumentModel
                {
                    Id = m.Id,
                    SolutionId = m.SolutionId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

            // Thông tin Node mặc định ngoài cùng
            var root = new SolutionDesignDocumentModel
            {
                Id = "4",
                Name = solution,
                DesignType = 4
            };

            list.Add(root);
            return list;
        }

        public SearchResultModel<SolutionDesignDocumentModel> GetListFileSolution(SolutionDesignDocumentModel model)
        {
            SearchResultModel<SolutionDesignDocumentModel> searchResult = new SearchResultModel<SolutionDesignDocumentModel>();
            if (model.FolderId.Equals("4"))
            {
                var list = db.SolutionDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_File) && t.SolutionId.Equals(model.SolutionId))
                    .OrderBy(o => o.Path)
                   .Select(m => new SolutionDesignDocumentModel
                   {
                       Id = m.Id,
                       SolutionId = m.SolutionId,
                       Name = m.Name,
                       ParentId = m.ParentId,
                       FileSize = m.FileSize,
                       CreateDate = m.CreateDate,
                       UpdateDate = m.UpdateDate,
                       ServerPath = m.ServerPath
                   }).AsQueryable();
                if (!string.IsNullOrEmpty(model.Name))
                {
                    list = list.Where(r => r.Name.ToUpper().Contains(model.Name.ToUpper()));
                }
                searchResult.ListResult = list.ToList();
            }
            else
            {
                List<string> groupIds = new List<string>();
                groupIds.Add(model.FolderId);

                List<SolutionDesignDocument> groupDocument = new List<SolutionDesignDocument>();
                groupDocument = db.SolutionDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_Folder) && t.ParentId.Equals(model.FolderId)).ToList();



                // Lấy danh sách nhóm thuộc chủng loại
                var group = (from t in db.SolutionDesignDocuments.AsNoTracking()
                             where t.FileType.Equals(Constants.ProductDesignDocument_FileType_Folder) && t.ParentId.Equals(model.FolderId)
                             select t.Id).ToList();

                if (group.Count > 0)
                {
                    groupIds.AddRange(group);
                }


                var listAttch = db.SolutionDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_File))
                 .OrderBy(o => o.Path)
                .Select(m => new SolutionDesignDocumentModel
                {
                    Id = m.Id,
                    SolutionId = m.SolutionId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    FileSize = m.FileSize,
                    CreateDate = m.CreateDate,
                    UpdateDate = m.UpdateDate,
                    ServerPath = m.ServerPath
                }).ToList();

                // Đệ quy để lấy tất cả danh sách các nhóm con
                for (int i = 0; i < groupIds.Count; i++)
                {
                    GetListDocument(listAttch, groupDocument, groupIds[i]);
                }



                //var list = db.SolutionDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.ProductDesignDocument_FileType_File) && t.ParentId.Equals(model.FolderId))
                //     .OrderBy(o => o.Path)
                //    .Select(m => new SolutionDesignDocumentModel
                //    {
                //        Id = m.Id,
                //        SolutionId = m.SolutionId,
                //        Name = m.Name,
                //        ParentId = m.ParentId,
                //        FileSize = m.FileSize,
                //        CreateDate = m.CreateDate,
                //        UpdateDate = m.UpdateDate,
                //        ServerPath = m.ServerPath
                //    }).AsQueryable();
                //if (!string.IsNullOrEmpty(model.Name))
                //{
                //    list = list.Where(r => r.Name.ToUpper().Contains(model.Name.ToUpper()));
                //}

                //foreach (var item in _documentList.ToList())
                //{

                //}
                searchResult.ListResult = _documentList.ToList();
            }
            return searchResult;
        }

        /// <summary>
        /// Upload tài liệu thiết kế
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UploadFileDesignDocument(UploadFileSolutionDesignDocumentModel model, string userId)
        {
            var solutionDocument = db.SolutionDesignDocuments.FirstOrDefault(t => t.Id.Equals(model.Id));

            if (solutionDocument == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Solution);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    solutionDocument.Path = model.LocalPath;
                    solutionDocument.ServerPath = model.ServerPath;
                    solutionDocument.UpdateBy = userId;
                    solutionDocument.UpdateDate = DateTime.Now;

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    NtsLog.LogError(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Upload tài liệu thiết kế
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UploadDesignDocument(UploadFolderSolutionDesignDocumentModel model, string userId)
        {
            var solution = db.Solutions.FirstOrDefault(t => t.Id.Equals(model.SolutionId));

            if (solution == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Solution);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    // Có tài liệu thiết kế
                    if (model.DesignDocuments.Count > 0)
                    {
                        List<SolutionDesignDocument> designDocuments = new List<SolutionDesignDocument>();
                        SolutionDesignDocument designDocument;
                        SolutionDesignDocument designDocumentFile;

                        var documents = db.SolutionDesignDocuments.Where(r => r.DesignType == model.DesignType && r.SolutionId.Equals(solution.Id) && r.CurentVersion == model.CurentVersion).ToList();

                        bool isDelete;
                        foreach (var document in documents)
                        {
                            isDelete = true;
                            foreach (var item in model.DesignDocuments)
                            {
                                if (item.LocalPath.Equals(document.Path))
                                {
                                    isDelete = false;
                                    break;
                                }

                                foreach (var file in item.Files)
                                {
                                    if (file.LocalPath.Equals(document.Path))
                                    {
                                        isDelete = false;
                                        break;
                                    }
                                }

                                if (!isDelete)
                                {
                                    break;
                                }
                            }

                            if (isDelete)
                            {
                                db.SolutionDesignDocuments.Remove(document);
                            }

                        }

                        var folderRoor = db.SolutionDesignDocuments.Where(r => r.Id.Equals(model.DesignType.ToString())).FirstOrDefault();
                        if (folderRoor == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0026);
                        }

                        FolderUploadModel parent;
                        foreach (var item in model.DesignDocuments)
                        {
                            designDocument = db.SolutionDesignDocuments.FirstOrDefault(r => r.SolutionId.Equals(model.SolutionId) && r.Path.Equals(item.LocalPath) && r.CurentVersion == model.CurentVersion);
                            if (designDocument == null)
                            {
                                designDocument = new SolutionDesignDocument()
                                {
                                    Id = item.Id,
                                    SolutionId = solution.Id,
                                    ParentId = item.ParentId,
                                    ServerPath = item.ServerPath,
                                    Path = item.LocalPath,
                                    Name = item.Name,
                                    FileType = Constants.ModuleDesignDocument_FileType_Folder,
                                    DesignType = model.DesignType,
                                    CurentVersion = model.CurentVersion,
                                    CreateBy = userId,
                                    CreateDate = DateTime.Now,
                                    UpdateBy = userId,
                                    UpdateDate = DateTime.Now
                                };

                                if (string.IsNullOrEmpty(designDocument.ParentId))
                                {
                                    designDocument.ParentId = folderRoor.Id;
                                }
                                else
                                {
                                    parent = model.DesignDocuments.FirstOrDefault(r => r.Id.Equals(item.ParentId));
                                    if (parent != null && !string.IsNullOrEmpty(parent.DBId))
                                    {
                                        designDocument.ParentId = parent.DBId;
                                    }
                                }

                                designDocuments.Add(designDocument);
                            }
                            else
                            {
                                item.DBId = designDocument.Id;
                                designDocument.UpdateBy = userId;
                                designDocument.UpdateDate = DateTime.Now;
                                designDocument.ServerPath = item.ServerPath;
                            }

                            foreach (var file in item.Files)
                            {
                                designDocumentFile = db.SolutionDesignDocuments.FirstOrDefault(r => r.SolutionId.Equals(model.SolutionId) && r.Path.Equals(item.LocalPath) && r.CurentVersion == model.CurentVersion);
                                if (designDocumentFile == null)
                                {
                                    designDocumentFile = new SolutionDesignDocument()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        SolutionId = solution.Id,
                                        ParentId = item.Id,
                                        ServerPath = file.ServerPath,
                                        Path = file.LocalPath,
                                        Name = file.Name,
                                        FileType = Constants.ModuleDesignDocument_FileType_File,
                                        FileSize = file.Size,
                                        DesignType = model.DesignType,
                                        CurentVersion = model.CurentVersion,
                                        CreateBy = userId,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = userId,
                                        UpdateDate = DateTime.Now
                                    };

                                    if (designDocument != null)
                                    {
                                        designDocumentFile.ParentId = designDocument.Id;
                                    }

                                    designDocuments.Add(designDocumentFile);
                                }
                                else
                                {
                                    designDocumentFile.UpdateBy = userId;
                                    designDocumentFile.UpdateDate = DateTime.Now;
                                    designDocumentFile.FileSize = file.Size;
                                    designDocumentFile.ServerPath = file.ServerPath;
                                }
                            }
                        }

                        db.SolutionDesignDocuments.AddRange(designDocuments);
                    }

                    //Tăng Version
                    if (model.CurentVersion > solution.CurentVersion)
                    {
                        //Lưu lịch sử
                        SolutionOldVersion solutionOldVersion = new SolutionOldVersion()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = solution.Id,
                            Version = model.CurentVersion - 1,
                            Description = model.EditContent,
                            CreateBy = userId,
                            CreateDate = DateTime.Now
                        };
                        db.SolutionOldVersions.Add(solutionOldVersion);
                        solution.CurentVersion = model.CurentVersion;


                        var solutionDesignDocument = (from r in db.SolutionDesignDocuments
                                                      where r.SolutionId == model.SolutionId
                                                      select r);

                        SolutionDesignDocumentOld moduleDesignDocumentOld = new SolutionDesignDocumentOld();
                        foreach (var item in solutionDesignDocument)
                        {
                            moduleDesignDocumentOld = new SolutionDesignDocumentOld()
                            {
                                Id = Guid.NewGuid().ToString(),
                                SolutionOldVersion = solutionOldVersion.Id,
                                Name = item.Name,
                                Path = item.Path,
                                ServerPath = item.ServerPath,
                                FileSize = item.FileSize,
                                FileType = item.FileType,
                                ParentId = item.ParentId,
                                DesignType = item.DesignType,
                                CreateBy = item.CreateBy,
                                CreateDate = item.CreateDate,
                                UpdateBy = item.UpdateBy,
                                UpdateDate = item.UpdateDate,
                                HashValue = item.HashValue
                            };

                            db.SolutionDesignDocumentOlds.Add(moduleDesignDocumentOld);
                        }
                    }

                    var ckeckImage = db.SolutionImages.Where(i => i.SolutionId.Equals(model.SolutionId) && i.Type == Constants.SolutionImage_Type_Thumbnail).ToList();
                    if (ckeckImage.Count() > 0)
                    {
                        db.SolutionImages.RemoveRange(ckeckImage);
                    }
                    if (!string.IsNullOrEmpty(model.PathImage))
                    {
                        SolutionImage solutionImage = new SolutionImage()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SolutionId = solution.Id,
                            FileName = model.NameImage,
                            FilePath = model.PathImage,
                            ThumbnailPath = model.PathImage,
                            Note = null,
                            Type = Constants.SolutionImage_Type_Thumbnail,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                        };
                        db.SolutionImages.Add(solutionImage);
                    }

                    solution.Design2DExist = model.Design2DExist;
                    solution.Design3DExist = model.Design3DExist;
                    solution.ExplanExist = model.ExplanExist;
                    solution.DMVTExist = model.DMVTExist;
                    solution.TSTKExist = model.TSTKExist;
                    solution.FCMExist = model.FCMExist;

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    NtsLog.LogError(ex);
                    throw;
                }
            }
        }

        public List<SolutionOldVersionModel> GetSolutionOldVersion(string solutionId)
        {
            List<SolutionOldVersionModel> listData = new List<SolutionOldVersionModel>();
            listData = (from a in db.SolutionOldVersions.AsNoTracking()
                        join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                        join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                        where a.SolutionId.Equals(solutionId)
                        orderby a.Version descending
                        select new SolutionOldVersionModel
                        {
                            Id = a.Id,
                            SolutionId = a.SolutionId,
                            Version = a.Version,
                            Description = a.Description,
                            CreateByName = c.Name,
                            CreateDate = a.CreateDate
                        }).ToList();

            foreach (var item in listData)
            {
                var root = db.SolutionDesignDocuments.AsNoTracking().Where(t => t.FileType.Equals(Constants.SolutionDesignDocument_FileType_Folder) && string.IsNullOrEmpty(t.SolutionId)).OrderBy(o => o.Path).Select(m => new SolutionDesignDocumentModel
                {
                    Id = m.Id,
                    SolutionId = m.SolutionId,
                    Name = m.Name,
                    ParentId = m.ParentId,
                    Path = m.Path,
                    ServerPath = m.ServerPath,
                    DesignType = m.DesignType
                }).ToList();

                foreach (var itemFolder in root)
                {
                    var add = db.SolutionDesignDocuments.AsNoTracking().FirstOrDefault(t => t.FileType.Equals(Constants.SolutionDesignDocument_FileType_Folder) && t.ParentId.Equals(itemFolder.Id) && t.CurentVersion == item.Version);
                    if (add != null)
                    {
                        item.ListDesign.Add(new DesignModel
                        {
                            Name = itemFolder.Name,
                            FolderName = add.Name,
                            IsDownload = true,
                            FolderId = add.Id,
                            DesignType = add.DesignType
                        });
                    }
                    else
                    {
                        item.ListDesign.Add(new DesignModel
                        {
                            Name = itemFolder.Name,
                            IsDownload = false
                        });
                    }
                }
            }

            return listData;
        }


        public ComboboxResult GetDomainById(string id)
        {
            var job = (from a in db.CustomerDomains.AsNoTracking()
                       join b in db.Jobs.AsNoTracking() on a.JobId equals b.Id
                       where a.Id.Equals(id)
                       select new ComboboxResult
                       {

                           Id = a.Id,
                           Name = b.Name,

                       }).FirstOrDefault();


            return job;
        }

        public SearchResultModel<SurveyMaterialCreateModel> GetSurveyMaterialId(string id)
        {
            SearchResultModel<SurveyMaterialCreateModel> searchResult = new SearchResultModel<SurveyMaterialCreateModel>();

            var dataQuery = (from a in db.SurveyTools.AsNoTracking()
                             select new SurveyMaterialCreateModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Note = a.Note,
                                 Quantity = a.Quantity,
                                 SurvayId = a.SurveyId,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(id))
            {
                dataQuery = dataQuery.Where(t => t.SurvayId.Equals(id));
            }
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        public SearchResultModel<SurveyContentCreateModel> GetSurveyContentId(string id)
        {
            SearchResultModel<SurveyContentCreateModel> searchResult = new SearchResultModel<SurveyContentCreateModel>();

            List<string> candidateIds = new List<string>();

            var dataQuery = (from a in db.SurveyContents.AsNoTracking()
                             select new SurveyContentCreateModel
                             {
                                 Id = a.Id,
                                 Content = a.Content,
                                 Result = a.Result,
                                 SurveyId = a.SurveyId,
                                 Level = a.Level,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(id))
            {
                dataQuery = dataQuery.Where(t => t.SurveyId.Equals(id));
            }
            var contenUser = db.SurveyContentUsers;
            var listUser = db.Users;
            var list = dataQuery.ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (contenUser.ToList().Count > 0)
                    {
                        var user = contenUser.FirstOrDefault(r => r.SurveyContentId.Equals(item.Id));
                        if (user != null)
                        {
                            var idUser = user.Id;


                            if (!string.IsNullOrEmpty(idUser))
                            {
                                var userName = listUser.FirstOrDefault(r => r.Id.Equals(idUser));
                                if (userName != null)
                                {

                                    item.Name = userName.UserName;
                                }
                            }
                        }
                    }

                }
            }
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        public void CheckDeleteSurvey(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var survey = db.Surveys.FirstOrDefault(a => a.Id.Equals(id));
                    if (survey == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Survey);
                    }
                    var surveyConten = db.SurveyContents.Where(r => r.SurveyId.Equals(id));
                    var surveyContentIds = db.SurveyContents.Where(r => r.SurveyId.Equals(id)).Select(r =>r.Id).ToList();
                    string listId = string.Join(",", surveyContentIds);
                    var surveyContentAttaches = db.SurveyContentAttaches.Where(r =>r.SurveyContentId.Contains(listId)).ToList();
                    if (surveyContentAttaches.Count > 0)
                    {
                        db.SurveyContentAttaches.RemoveRange(surveyContentAttaches);
                    }
                    var surveyContentUser = db.SurveyContentUsers.Where(r => r.SurveyContentId.Contains(listId)).ToList();
                    if (surveyContentUser.Count > 0)
                    {
                        db.SurveyContentUsers.RemoveRange(surveyContentUser);
                    }
                    var surveyMaterial = db.SurveyTools.Where(r => r.SurveyId.Equals(id));
                    db.SurveyTools.RemoveRange(surveyMaterial);
                    db.SurveyContents.RemoveRange(surveyConten);
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

        public void DeleteConten(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var Conten = db.SurveyContents.FirstOrDefault(a => a.Id.Equals(id));
                    if (Conten == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SurveyContent);
                    }
                    if (Conten != null)
                    {
                        var surveyContenAttaches = db.SurveyContentAttaches.Where(r => r.SurveyContentId.Equals(id));
                        db.SurveyContentAttaches.RemoveRange(surveyContenAttaches);
                        var conentuser = db.SurveyContentUsers.FirstOrDefault(r => r.SurveyContentId.Equals(id));
                        db.SurveyContentUsers.Remove(conentuser);
                    }
                    db.SurveyContents.Remove(Conten);
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

        public void DeleteMaterial(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var tool = db.SurveyTools.FirstOrDefault(a => a.Id.Equals(id));
                    if (tool == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SurveyMaterial);
                    }

                    db.SurveyTools.Remove(tool);
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

        public void SaveMaterial(string id, List<CustomerRequirementMaterialInfoModel> model)
        {
            List<SurveyTool> listMaterial = new List<SurveyTool>();
            foreach (var item in model)
            {
                if (item.IsNew)
                {
                    SurveyTool surveyTool = new SurveyTool()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SurveyId = id,
                        MaterialId = item.Id,
                        Note = item.Note,
                        Quantity = item.Quantity,
                    };
                    listMaterial.Add(surveyTool);
                }
                else
                {
                    SurveyTool surveyTool = new SurveyTool()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SurveyId = id,
                        MaterialId = item.MaterialId,
                        Note = item.Note,
                        Quantity = item.Quantity,
                    };
                    listMaterial.Add(surveyTool);
                }
            }
            db.SurveyTools.AddRange(listMaterial);
            db.SaveChanges();
        }


        private void GetListDocument(List<SolutionDesignDocumentModel> fileAttach, List<SolutionDesignDocument> groups, string groupId)
        {
            var list = fileAttach.Where(r => !string.IsNullOrEmpty(r.ParentId) && r.ParentId.Equals(groupId)).ToList();

            foreach (var file in list)
            {
                if (!_documentList.Any(r => r.Id.Equals(file.Id)))
                {
                    _documentList.Add(file);
                }
            }
            // Danh sách các nhóm con
            List<SolutionDesignDocument> childs = groups.Where(r => !string.IsNullOrEmpty(r.ParentId) && r.ParentId.Equals(groupId)).ToList();

            foreach (var item in childs)
            {
                GetListDocument(fileAttach, groups, item.Id);
            }
        }



    }

}
