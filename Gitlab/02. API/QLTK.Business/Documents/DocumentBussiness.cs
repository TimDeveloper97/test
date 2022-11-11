using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.ClassRoom;
using NTS.Model.Combobox;
using NTS.Model.Document;
using NTS.Model.DocumentPromulgate;
using NTS.Model.HistoryVersion;
using NTS.Model.NTSDepartment;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorkType;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Document
{
    public class DocumentBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<DocumentSearchResultModel> SearchManageDocument(DocumentSearchModel searchModel)
        {
            SearchResultModel<DocumentSearchResultModel> searchResult = new SearchResultModel<DocumentSearchResultModel>();

            var dataQuery = (from a in db.Documents.AsNoTracking()
                             join b in db.DocumentTypes.AsNoTracking() on a.DocumentTypeId equals b.Id
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from acv in ac.DefaultIfEmpty()
                             select new DocumentSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 DocumentGroupId = a.DocumentGroupId,
                                 Code = a.Code,
                                 DocumentTypeName = b.Name,
                                 Status = a.Status,
                                 Description = a.Description,
                                 Keyword = a.Keyword,
                                 PromulgateDate = a.PromulgateDate,
                                 PromulgateLastDate = a.PromulgateLastDate,
                                 Version = a.Version,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = acv != null ? acv.Name : "",
                                 DocumentTypeId = a.DocumentTypeId,
                                 ReviewDateFrom = a.ReviewDateFrom,
                                 ReviewDateTo = a.ReviewDateTo,
                                 CompilationType = a.CompilationType,
                                 CompilationSuppliserId = a.CompilationSuppliserId

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.DocumentGroupId))
            {
                var documentGroupIds = GetListParent(searchModel.DocumentGroupId, db.DocumentGroups.AsNoTracking().ToList());
                documentGroupIds.Add(searchModel.DocumentGroupId);
                dataQuery = dataQuery.Where(u => documentGroupIds.Contains(u.DocumentGroupId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentUseId))
            {
                var docuemtnIds = db.DocumentObjects.AsNoTracking().Where(r => r.ObjectId.Equals(searchModel.DepartmentUseId) && r.ObjectType.Equals(Constants.ObjectType_Department)).Select(s => s.DocumentId).ToList();
                dataQuery = dataQuery.Where(u => docuemtnIds.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(searchModel.Keyword))
            {
                dataQuery = dataQuery.Where(u => !string.IsNullOrEmpty(u.Keyword) && u.Keyword.ToLower().Contains(searchModel.Keyword.ToLower()));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status);
            }

            if (searchModel.CompilationType.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.CompilationType == searchModel.CompilationType);
            }

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()) || u.Code.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.DocumentTypeId))
            {
                dataQuery = dataQuery.Where(u => u.DocumentTypeId.ToUpper().Contains(searchModel.DocumentTypeId.ToUpper()));
            }

            if (searchModel.PromulgateDateFrom.HasValue)
            {
                searchModel.PromulgateDateFrom = searchModel.PromulgateDateFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.PromulgateDate.HasValue && a.PromulgateDate >= searchModel.PromulgateDateFrom);
            }

            if (searchModel.PromulgateDateTo.HasValue)
            {
                searchModel.PromulgateDateTo = searchModel.PromulgateDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.PromulgateDate.HasValue && a.PromulgateDate <= searchModel.PromulgateDateTo);
            }

            if (searchModel.PromulgateLastDateFrom.HasValue)
            {
                searchModel.PromulgateLastDateFrom = searchModel.PromulgateLastDateFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.PromulgateLastDate.HasValue && a.PromulgateLastDate >= searchModel.PromulgateLastDateFrom);
            }

            if (searchModel.PromulgateLastDateTo.HasValue)
            {
                searchModel.PromulgateLastDateTo = searchModel.PromulgateLastDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.PromulgateLastDate.HasValue && a.PromulgateLastDate <= searchModel.PromulgateLastDateTo);
            }

            if (searchModel.EffectiveDateFrom.HasValue)
            {
                searchModel.EffectiveDateFrom = searchModel.EffectiveDateFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.ReviewDateTo.HasValue && a.ReviewDateTo >= searchModel.EffectiveDateFrom);
            }

            if (searchModel.EffectiveDateTo.HasValue)
            {
                searchModel.EffectiveDateTo = searchModel.EffectiveDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.ReviewDateTo.HasValue && a.ReviewDateTo <= searchModel.EffectiveDateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.CompilationSuppliserId))
            {
                dataQuery = dataQuery.Where(u => u.CompilationSuppliserId.ToUpper().Contains(searchModel.CompilationSuppliserId.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            foreach (var item in searchResult.ListResult)
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

            return searchResult;
        }
        private List<string> GetListParent(string id, List<DocumentGroup> docuemntGroups)
        {
            List<string> listChild = new List<string>();

            var docuemntGroupIds = docuemntGroups.Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(docuemntGroupIds);
            if (docuemntGroupIds.Count > 0)
            {
                foreach (var item in docuemntGroupIds)
                {
                    listChild.AddRange(GetListParent(item, docuemntGroups));
                }
            }
            return listChild;
        }

        public SearchResultModel<DocumentSearchResultModel> SearchDocument(DocumentSearchModel searchModel)
        {
            searchModel.Status = Constants.Document_Status_Used;

            return SearchManageDocument(searchModel);
        }

        public string CreateDocument(DocumentModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();

            var documentNameExists = db.Documents.AsNoTracking().FirstOrDefault(a => a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (documentNameExists != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Document);
            }

            var documentCodeExists = db.Documents.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (documentCodeExists != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Document);
            }

            try
            {
                NTS.Model.Repositories.Document document = new NTS.Model.Repositories.Document
                {
                    Id = Guid.NewGuid().ToString(),
                    DocumentGroupId = model.DocumentGroupId,
                    DocumentTypeId = model.DocumentTypeId,
                    Code = model.Code.ToUpper(),
                    Name = model.Name.ToUpper(),
                    Version = model.Version,
                    CompilationType = model.CompilationType,
                    CompilationEmployeeId = model.CompilationEmployeeId,
                    CompilationSuppliserId = model.CompilationSuppliserId,
                    PromulgateDate = model.PromulgateDate,
                    PromulgateLastDate = model.PromulgateLastDate,
                    DepartmentId = model.DepartmentId,
                    EmployeeId = model.EmployeeId,
                    ReviewDateFrom = model.ReviewDateFrom,
                    ReviewDateTo = model.ReviewDateTo,
                    Price = model.Price,
                    //Keyword = model.Keyword,
                    Description = model.Description,
                    ApproveWorkTypeId = model.ApproveWorkTypeId,
                    Status = Constants.Document_Status_Cancel,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now,
                };

                document.Keyword = string.Empty;
                if (model.Keywords.Count > 0)
                {
                    document.Keyword = string.Join(";", model.Keywords);

                    DocumentTag documentTagExist;
                    foreach (var item in model.DocumentTags)
                    {
                        documentTagExist = db.DocumentTags.FirstOrDefault(r => r.Name.ToLower().Equals(item.ToLower()));

                        if (documentTagExist == null)
                        {
                            db.DocumentTags.Add(new DocumentTag { Name = item });
                        }
                    }
                }

                db.Documents.Add(document);

                DocumentObject documentObject;
                foreach (var item in model.Devices)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = document.Id,
                        ObjectId = item.Id,
                        ObjectType = Constants.ObjectType_Devide
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.Modules)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = document.Id,
                        ObjectId = item.Id,
                        ObjectType = Constants.ObjectType_Module
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.WorkTypes)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = document.Id,
                        ObjectId = item.Id.ToString(),
                        ObjectType = Constants.ObjectType_WorkType
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.Departments)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = document.Id,
                        ObjectId = item.Id.ToString(),
                        ObjectType = Constants.ObjectType_Department
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.Works)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = document.Id,
                        ObjectId = item.Id,
                        ObjectType = Constants.ObjectType_Work
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, document.Name, document.Id, Constants.LOG_Document);

                db.SaveChanges();

                return document.Id;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public SearchResultModel<DepartmentResultModel> SearchDepartment(DepartmentSearchModel modelSearch)
        {
            SearchResultModel<DepartmentResultModel> search = new SearchResultModel<DepartmentResultModel>();

            var dataQuery = (from a in db.Departments.AsNoTracking()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new DepartmentResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 SBUId = a.SBUId,
                                 SBUName = c.Name,
                                 Description = a.Description,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(a => a.SBUId.Equals(modelSearch.SBUId));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        public List<DocumentTagModel> GetDocumentTags()
        {
            var tags = (from a in db.DocumentTags.AsNoTracking()
                        orderby a.Name
                        select new DocumentTagModel { Name = a.Name }).ToList();

            return tags;
        }


        public SearchResultModel<WorkTypeModel> SearchWorkType(WorkTypeSearchModel modelSearch)
        {
            SearchResultModel<WorkTypeModel> search = new SearchResultModel<WorkTypeModel>();

            var dataQuery = (from a in db.WorkTypes.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id.ToString())
                             orderby a.Name
                             select new WorkTypeModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Quantity = a.Quantity,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToLower().Contains(modelSearch.Name.ToLower()));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        public void DeleteDocument(DocumentModel model)
        {
            var documentExist = db.Documents.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (documentExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            var documentPromulgate = db.DocumentPromulgates.AsNoTracking().FirstOrDefault(a => a.DocumentId.Equals(model.Id));
            if (documentPromulgate != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Document);
            }

            try
            {
                var documentObject = db.DocumentObjects.Where(a => a.DocumentId.Equals(model.Id)).ToList();
                db.DocumentObjects.RemoveRange(documentObject);

                var documentFile = db.DocumentFiles.Where(a => a.DocumentId.Equals(model.Id)).ToList();
                db.DocumentFiles.RemoveRange(documentFile);

                var documentReference = db.DocumentReferences.Where(a => a.DocumentId.Equals(model.Id)).ToList();
                db.DocumentReferences.RemoveRange(documentReference);

                var documentProblem = db.DocumentProblems.Where(a => a.DocumentId.Equals(model.Id)).ToList();
                db.DocumentProblems.RemoveRange(documentProblem);

                var documentMeeting = db.DocumentMeetings.Where(a => a.DocumentId.Equals(model.Id)).ToList();
                db.DocumentMeetings.RemoveRange(documentMeeting);

                db.Documents.Remove(documentExist);

                var NameOrCode = documentExist.Name;
                //var jsonBefor = AutoMapperConfig.Mapper.Map<DocumentHistoryModel>(documentExist);
                //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_DocumentGroup, documentExist.Id, NameOrCode, jsonBefor);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public DocumentModel GetDocumentInfo(DocumentModel model)
        {
            var resultInfo = db.Documents.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DocumentModel
            {
                Id = p.Id,
                DocumentGroupId = p.DocumentGroupId,
                DocumentTypeId = p.DocumentTypeId,
                Code = p.Code,
                Name = p.Name,
                Version = p.Version,
                CompilationType = p.CompilationType,
                CompilationEmployeeId = p.CompilationEmployeeId,
                CompilationSuppliserId = p.CompilationSuppliserId,
                PromulgateDate = p.PromulgateDate,
                PromulgateLastDate = p.PromulgateLastDate,
                DepartmentId = p.DepartmentId,
                EmployeeId = p.EmployeeId,
                ReviewDateFrom = p.ReviewDateFrom,
                ReviewDateTo = p.ReviewDateTo,
                Price = p.Price,
                Keyword = p.Keyword,
                Description = p.Description,
                ApproveWorkTypeId = p.ApproveWorkTypeId,
                Status = p.Status,
            }).FirstOrDefault();

            if (!string.IsNullOrEmpty(resultInfo.Keyword))
            {
                resultInfo.Keywords = resultInfo.Keyword.Split(';').ToList();
            }

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            resultInfo.Devices = (from a in db.DocumentObjects.AsNoTracking()
                                  where a.DocumentId.Equals(model.Id) && a.ObjectType == Constants.ObjectType_Devide
                                  join b in db.Products.AsNoTracking() on a.ObjectId equals b.Id
                                  select new ClassRoomResultProductModel
                                  {
                                      Id = a.ObjectId,
                                      Code = b.Code,
                                      Name = b.Name
                                  }).ToList();

            resultInfo.Modules = (from a in db.DocumentObjects.AsNoTracking()
                                  where a.DocumentId.Equals(model.Id) && a.ObjectType == Constants.ObjectType_Module
                                  join b in db.Modules.AsNoTracking() on a.ObjectId equals b.Id
                                  select new ModuleModel
                                  {
                                      Id = a.ObjectId,
                                      Code = b.Code,
                                      Name = b.Name
                                  }).ToList();

            resultInfo.Departments = (from a in db.DocumentObjects.AsNoTracking()
                                      where a.DocumentId.Equals(model.Id) && a.ObjectType == Constants.ObjectType_Department
                                      join b in db.Departments.AsNoTracking() on a.ObjectId equals b.Id
                                      join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                      select new DepartmentResultModel
                                      {
                                          Id = a.ObjectId,
                                          Code = b.Code,
                                          Name = b.Name,
                                          SBUId = c.Id,
                                          SBUName = c.Name
                                      }).ToList();

            resultInfo.WorkTypes = (from a in db.DocumentObjects.AsNoTracking()
                                    where a.DocumentId.Equals(model.Id) && a.ObjectType == Constants.ObjectType_WorkType
                                    join b in db.WorkTypes.AsNoTracking() on a.ObjectId equals b.Id.ToString()
                                    select new WorkTypeModel
                                    {
                                        Id = b.Id,
                                        Code = b.Code,
                                        Name = b.Name
                                    }).ToList();

            resultInfo.Works = (from a in db.DocumentObjects.AsNoTracking()
                                where a.DocumentId.Equals(model.Id) && a.ObjectType == Constants.ObjectType_Work
                                join b in db.Tasks.AsNoTracking() on a.ObjectId equals b.Id.ToString()
                                join c in db.FlowStages.AsNoTracking() on b.FlowStageId equals c.Id into bc
                                from bcv in bc.DefaultIfEmpty()
                                join d in db.SBUs.AsNoTracking() on b.SBUId equals d.Id into bd
                                from bdv in bd.DefaultIfEmpty()
                                join e in db.Departments.AsNoTracking() on b.DepartmentId equals e.Id into be
                                from bev in be.DefaultIfEmpty()
                                select new TaskFlowStageSearchResultModel
                                {
                                    Id = a.Id,
                                    Name = b.Name,
                                    Code = b.Code,
                                    IsDesignModule = b.IsDesignModule.HasValue ? b.IsDesignModule.Value : false,
                                    FlowStageId = b.FlowStageId,
                                    FlowStageName = bcv != null ? bcv.Name : "",
                                    Type = b.Type,
                                    CreateDate = b.CreateDate,
                                    SBUId = b.SBUId,
                                    SBUName = bdv != null ? bdv.Name : "",
                                    DepartmentId = b.DepartmentId,
                                    DepartmentName = bev != null ? bev.Name : ""
                                }).ToList();



            return resultInfo;
        }

        public void UpdateDocument(DocumentModel model)
        {
            model.Name = model.Name.NTSTrim();
            model.Code = model.Code.NTSTrim();

            var documentUpdate = db.Documents.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (documentUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            var documentNameExist = db.Documents.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (documentNameExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Document);
            }

            var documentCodeExist = db.Documents.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper()));
            if (documentCodeExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Document);
            }

            //var jsonBefor = AutoMapperConfig.Mapper.Map<DocumentHistoryModel>(documentUpdate);
            try
            {
                documentUpdate.DocumentGroupId = model.DocumentGroupId;
                documentUpdate.DocumentTypeId = model.DocumentTypeId;
                documentUpdate.Code = model.Code.ToUpper();
                documentUpdate.Name = model.Name.ToUpper();
                documentUpdate.Version = model.Version;
                documentUpdate.CompilationType = model.CompilationType;
                documentUpdate.CompilationEmployeeId = model.CompilationEmployeeId;
                documentUpdate.CompilationSuppliserId = model.CompilationSuppliserId;
                documentUpdate.PromulgateDate = model.PromulgateDate;
                documentUpdate.PromulgateLastDate = model.PromulgateLastDate;
                documentUpdate.DepartmentId = model.DepartmentId;
                documentUpdate.EmployeeId = model.EmployeeId;
                documentUpdate.ReviewDateFrom = model.ReviewDateFrom;
                documentUpdate.ReviewDateTo = model.ReviewDateTo;
                documentUpdate.Price = model.Price;
                documentUpdate.Keyword = model.Keyword;
                documentUpdate.Description = model.Description;
                documentUpdate.ApproveWorkTypeId = model.ApproveWorkTypeId;
                documentUpdate.UpdateBy = model.UpdateBy;
                documentUpdate.UpdateDate = DateTime.Now;

                documentUpdate.Keyword = string.Empty;
                if (model.Keywords.Count > 0)
                {
                    documentUpdate.Keyword = string.Join(";", model.Keywords);

                    DocumentTag documentTagExist;
                    foreach (var item in model.DocumentTags)
                    {
                        documentTagExist = db.DocumentTags.FirstOrDefault(r => r.Name.ToLower().Equals(item.ToLower()));

                        if (documentTagExist == null)
                        {
                            db.DocumentTags.Add(new DocumentTag { Name = item });
                        }
                    }
                }

                var documentObjectExist = db.DocumentObjects.Where(a => a.DocumentId.Equals(model.Id)).ToList();
                db.DocumentObjects.RemoveRange(documentObjectExist);
                DocumentObject documentObject;
                foreach (var item in model.Devices)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = documentUpdate.Id,
                        ObjectId = item.Id,
                        ObjectType = Constants.ObjectType_Devide
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.Modules)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = documentUpdate.Id,
                        ObjectId = item.Id,
                        ObjectType = Constants.ObjectType_Module
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.WorkTypes)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = documentUpdate.Id,
                        ObjectId = item.Id.ToString(),
                        ObjectType = Constants.ObjectType_WorkType
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.Departments)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = documentUpdate.Id,
                        ObjectId = item.Id.ToString(),
                        ObjectType = Constants.ObjectType_Department
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                foreach (var item in model.Works)
                {
                    documentObject = new DocumentObject()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = documentUpdate.Id,
                        ObjectId = item.Id,
                        ObjectType = Constants.ObjectType_Work
                    };
                    db.DocumentObjects.Add(documentObject);
                }

                //var jsonApter = AutoMapperConfig.Mapper.Map<DocumentHistoryModel>(documentUpdate);

                //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Document, documentUpdate.Id, documentUpdate.Name, jsonBefor, jsonApter);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public void UpdateDocumentFile(DocumentUploadModel model, string userId)
        {

            var documentUpdate = db.Documents.FirstOrDefault(a => a.Id.Equals(model.DocumentId));
            if (documentUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }
            try
            {
                //var documentFiles = db.DocumentFiles.Where(a => a.DocumentId.Equals(model.DocumentId)).ToList();
                //db.DocumentFiles.RemoveRange(documentFiles);

                var documentReference = db.DocumentReferences.Where(a => a.DocumentId.Equals(model.DocumentId)).ToList();
                db.DocumentReferences.RemoveRange(documentReference);

                DocumentFile documentFile;
                foreach (var item in model.DocumentFiles)
                {
                    if (item.Path.Contains(".pdf"))
                    {
                        item.PDFPath = item.Path;
                    }

                    if (string.IsNullOrEmpty(item.Id))
                    {
                        documentFile = new DocumentFile()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DocumentId = model.DocumentId,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            Path = item.Path,
                            PDFPath = item.PDFPath,
                            Note = item.Note,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        db.DocumentFiles.Add(documentFile);
                    }
                    else
                    {
                        documentFile = db.DocumentFiles.FirstOrDefault(a => a.Id.Equals(item.Id));
                        if (documentFile == null)
                        {
                            continue;
                        }

                        if (item.IsDelete)
                        {
                            db.DocumentFiles.Remove(documentFile);
                        }
                        else
                        {
                            if (string.Compare(item.Path, documentFile.Path) != 0)
                            {
                                documentFile.UpdateBy = userId;
                                documentFile.UpdateDate = DateTime.Now;
                                documentFile.Path = item.Path;
                                documentFile.PDFPath = item.PDFPath;
                                documentFile.FileName = item.FileName;
                                documentFile.FileSize = item.FileSize;
                            }
                            documentFile.Note = item.Note;
                        }
                    }
                }

                DocumentReference documentReference1;
                foreach (var item in model.DocumentReferences)
                {
                    documentReference1 = new DocumentReference()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = model.DocumentId,
                        ReferenceId = item.Id
                    };
                    db.DocumentReferences.Add(documentReference1);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public SearchResultModel<DocumentSearchResultModel> SearchChooseDocument(DocumentSearchModel searchModel)
        {
            SearchResultModel<DocumentSearchResultModel> search = new SearchResultModel<DocumentSearchResultModel>();

            var dataQuery = (from a in db.Documents.AsNoTracking()
                             join b in db.DocumentTypes.AsNoTracking() on a.DocumentTypeId equals b.Id
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from acv in ac.DefaultIfEmpty()
                             where !searchModel.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new DocumentSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 DocumentGroupId = a.DocumentGroupId,
                                 Code = a.Code,
                                 DocumentTypeName = b.Name,
                                 Status = a.Status,
                                 Description = a.Description,
                                 Keyword = a.Keyword,
                                 PromulgateDate = a.PromulgateDate,
                                 PromulgateLastDate = a.PromulgateLastDate,
                                 Version = a.Version,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = acv != null ? acv.Name : "",
                                 DocumentTypeId = a.DocumentTypeId,
                                 ReviewDateFrom = a.ReviewDateFrom,
                                 ReviewDateTo = a.ReviewDateTo,
                                 CompilationType = a.CompilationType,
                                 CompilationSuppliserId = a.CompilationSuppliserId
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.DocumentGroupId))
            {
                var documentGroupIds = GetListParent(searchModel.DocumentGroupId, db.DocumentGroups.AsNoTracking().ToList());
                documentGroupIds.Add(searchModel.DocumentGroupId);
                dataQuery = dataQuery.Where(u => documentGroupIds.Contains(u.DocumentGroupId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentUseId))
            {
                var docuemtnIds = db.DocumentObjects.AsNoTracking().Where(r => r.ObjectId.Equals(searchModel.DepartmentUseId) && r.ObjectType.Equals(Constants.ObjectType_Department)).Select(s => s.DocumentId).ToList();
                dataQuery = dataQuery.Where(u => docuemtnIds.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(searchModel.Keyword))
            {
                dataQuery = dataQuery.Where(u => !string.IsNullOrEmpty(u.Keyword) && u.Keyword.ToLower().Contains(searchModel.Keyword.ToLower()));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status);
            }

            if (searchModel.CompilationType.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.CompilationType == searchModel.CompilationType);
            }

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()) || u.Code.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.DocumentTypeId))
            {
                dataQuery = dataQuery.Where(u => u.DocumentTypeId.ToUpper().Contains(searchModel.DocumentTypeId.ToUpper()));
            }

            if (searchModel.PromulgateDateFrom.HasValue)
            {
                searchModel.PromulgateDateFrom = searchModel.PromulgateDateFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.PromulgateDate.HasValue && a.PromulgateDate >= searchModel.PromulgateDateFrom);
            }

            if (searchModel.PromulgateDateTo.HasValue)
            {
                searchModel.PromulgateDateTo = searchModel.PromulgateDateFrom.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.PromulgateDate.HasValue && a.PromulgateDate <= searchModel.PromulgateDateTo);
            }

            if (searchModel.PromulgateLastDateFrom.HasValue)
            {
                searchModel.PromulgateLastDateFrom = searchModel.PromulgateLastDateFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.PromulgateLastDate.HasValue && a.PromulgateLastDate >= searchModel.PromulgateLastDateFrom);
            }

            if (searchModel.PromulgateLastDateTo.HasValue)
            {
                searchModel.PromulgateLastDateTo = searchModel.PromulgateLastDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.PromulgateLastDate.HasValue && a.PromulgateLastDate <= searchModel.PromulgateLastDateTo);
            }

            if (searchModel.EffectiveDateFrom.HasValue)
            {
                searchModel.EffectiveDateFrom = searchModel.EffectiveDateFrom.Value.ToStartDate();
                dataQuery = dataQuery.Where(a => a.ReviewDateTo.HasValue && a.ReviewDateTo >= searchModel.EffectiveDateFrom);
            }

            if (searchModel.EffectiveDateTo.HasValue)
            {
                searchModel.EffectiveDateTo = searchModel.EffectiveDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.ReviewDateFrom.HasValue && a.ReviewDateFrom <= searchModel.EffectiveDateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.CompilationSuppliserId))
            {
                dataQuery = dataQuery.Where(u => u.CompilationSuppliserId.ToUpper().Contains(searchModel.CompilationSuppliserId.ToUpper()));
            }

            search.TotalItem = dataQuery.Count();
            search.ListResult = dataQuery.ToList();
            return search;
        }

        public DocumentUploadModel GetDocumentFile(DocumentUploadModel model)
        {
            var documentUpdate = db.Documents.FirstOrDefault(a => a.Id.Equals(model.DocumentId));
            if (documentUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            model.DocumentFiles = (from a in db.DocumentFiles.AsNoTracking()
                                   where a.DocumentId.Equals(model.DocumentId)
                                   join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                   select new DocumentFileModel
                                   {
                                       Id = a.Id,
                                       DocumentId = a.DocumentId,
                                       FileName = a.FileName,
                                       FileSize = a.FileSize,
                                       Path = a.Path,
                                       PDFPath = a.PDFPath,
                                       CreateBy = a.CreateBy,
                                       CreateDate = a.CreateDate,
                                       CreateByName = b.UserName
                                   }).ToList();

            model.DocumentReferences = (from a in db.DocumentReferences.AsNoTracking()
                                        where a.DocumentId.Equals(model.DocumentId)
                                        join b in db.Documents.AsNoTracking() on a.ReferenceId equals b.Id
                                        join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                        select new DocumentSearchResultModel
                                        {
                                            Id = a.ReferenceId,
                                            Code = b.Code,
                                            Name = b.Name,
                                            DocumentGroupName = c.Name
                                        }).ToList();

            return model;
        }

        public SearchResultModel<DocumentPromulgateSearchResultModel> SearchPromulgate(DocumentPromulgateSearchModel searchModel)
        {
            SearchResultModel<DocumentPromulgateSearchResultModel> searchResult = new SearchResultModel<DocumentPromulgateSearchResultModel>();

            var dataQuery = (from a in db.DocumentPromulgates.AsNoTracking()
                             where a.DocumentId.Equals(searchModel.DocumentId)
                             select new DocumentPromulgateSearchResultModel
                             {
                                 Id = a.Id,
                                 Reason = a.Reason,
                                 Content = a.Content,
                                 PromulgateDate = a.PromulgateDate
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Reason))
            {
                dataQuery = dataQuery.Where(u => u.Reason.ToUpper().Contains(searchModel.Reason.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Content))
            {
                dataQuery = dataQuery.Where(u => u.Content.ToUpper().Contains(searchModel.Content.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            foreach (var item in searchResult.ListResult)
            {
                item.ListFile = (from a in db.DocumentPromulgateFiles.AsNoTracking()
                                 where a.DocumentPromulgateId.Equals(item.Id)
                                 select new DocumentPromulgateFileModel
                                 {
                                     Id = a.Id,
                                     DocumentPromulgateId = a.DocumentPromulgateId,
                                     DocumentId = a.DocumentId,
                                     FileName = a.FileName,
                                     FileSize = a.FileSize,
                                     Path = a.Path
                                 }).ToList();
            }

            return searchResult;
        }

        public void DeleteDocumentPromulgate(DocumentPromulgateModel model)
        {
            var documentPromulgateExist = db.DocumentPromulgates.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (documentPromulgateExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentPromulgate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var documentPromulgateFile = db.DocumentPromulgateFiles.Where(a => a.DocumentPromulgateId.Equals(model.Id)).ToList();
                    db.DocumentPromulgateFiles.RemoveRange(documentPromulgateFile);

                    db.DocumentPromulgates.Remove(documentPromulgateExist);
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

        public DocumentPromulgateModel GetDocumentPromulgateInfo(DocumentPromulgateModel model)
        {
            var resultInfo = db.DocumentPromulgates.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new DocumentPromulgateModel
            {
                Id = p.Id,
                DocumentId = p.Id,
                Reason = p.Reason,
                Content = p.Content,
                PromulgateDate = p.PromulgateDate
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentPromulgate);
            }

            return resultInfo;
        }

        public void UpdateDocumentPromulgate(DocumentPromulgateModel model)
        {

            var documentPromulgateUpdate = db.DocumentPromulgates.FirstOrDefault(a => a.Id.Equals(model.DocumentId));
            if (documentPromulgateUpdate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.DocumentPromulgate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    documentPromulgateUpdate.Reason = model.Reason;
                    documentPromulgateUpdate.Content = model.Content;
                    documentPromulgateUpdate.PromulgateDate = model.PromulgateDate;

                    var promulgateFile = db.DocumentPromulgateFiles.Where(a => a.DocumentPromulgateId.Equals(model.Id)).ToList();
                    db.DocumentPromulgateFiles.RemoveRange(promulgateFile);

                    var documentFile = db.DocumentFiles.AsNoTracking().Where(a => a.DocumentId.Equals(model.DocumentId)).ToList();
                    DocumentPromulgateFile documentPromulgateFile;
                    foreach (var item in documentFile)
                    {
                        documentPromulgateFile = new DocumentPromulgateFile()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DocumentPromulgateId = model.Id,
                            DocumentId = model.DocumentId,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            Path = item.Path,
                            CreateBy = item.CreateBy,
                            CreateDate = item.CreateDate,
                            UpdateBy = item.UpdateBy,
                            UpdateDate = item.UpdateDate
                        };
                        db.DocumentPromulgateFiles.Add(documentPromulgateFile);
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex); ;
                }
            }
        }

        public void CreateDocumentPromulgate(DocumentPromulgateModel model)
        {
            var document = db.Documents.FirstOrDefault(a => a.Id.Equals(model.DocumentId));
            if (document.Status == 1)
            {
                throw NTSException.CreateInstance("Tài liệu đã ban hành");
            }

            var documentFile = db.DocumentFiles.AsNoTracking().FirstOrDefault(r => r.DocumentId.Equals(model.DocumentId));
            if (documentFile == null)
            {
                throw NTSException.CreateInstance("Tài liệu chưa có file đính kèm. Không thể ban hành");
            }

            var documentProblem = db.DocumentProblems.AsNoTracking().FirstOrDefault(a => a.DocumentId.Equals(model.DocumentId) && a.Status != 2);
            if (documentProblem != null)
            {
                throw NTSException.CreateInstance("Tài liệu chưa hoàn thành review. Không thể ban hành");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    int index = 1;
                    var documentPromulgateLast = db.DocumentPromulgates.Where(r => r.DocumentId.Equals(model.DocumentId)).OrderByDescending(o => o.PromulgateDate).FirstOrDefault();
                    if (documentPromulgateLast != null)
                    {
                        if (documentPromulgateLast.PromulgateDate > model.PromulgateDate)
                        {
                            throw NTSException.CreateInstance("Ngày ban hành phải lớn hơn ngày ban hành trước!");
                        }
                        index = documentPromulgateLast.Index + 1;
                    }

                    string version = $"V.{string.Format("{0:00}", index)}";

                    NTS.Model.Repositories.DocumentPromulgate promulgate = new NTS.Model.Repositories.DocumentPromulgate
                    {
                        Id = Guid.NewGuid().ToString(),
                        DocumentId = model.DocumentId,
                        Reason = model.Reason,
                        Content = model.Content,
                        PromulgateDate = model.PromulgateDate,
                        Version = version,
                        Index = index
                    };

                    db.DocumentPromulgates.Add(promulgate);

                    // Update version và trạng thái document sau khi ban hành
                    document.Status = Constants.Document_Status_Used;
                    document.PromulgateLastDate = model.PromulgateDate;
                    document.Version = version;
                    document.PromulgateLastDate = model.PromulgateDate;
                    document.ReviewDateFrom = model.ReviewDateFrom;
                    document.ReviewDateTo = model.ReviewDateTo;
                    document.ReviewDateTo = model.ReviewDateTo;
                    document.PromulgateDate = model.ApproveDate;

                    var documentFiles = db.DocumentFiles.AsNoTracking().Where(a => a.DocumentId.Equals(model.DocumentId)).ToList();
                    DocumentPromulgateFile documentPromulgateFile;
                    foreach (var item in documentFiles)
                    {
                        documentPromulgateFile = new DocumentPromulgateFile()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DocumentPromulgateId = promulgate.Id,
                            DocumentId = model.DocumentId,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            Path = item.Path,
                            PDFPath = item.PDFPath,
                            CreateBy = item.CreateBy,
                            CreateDate = item.CreateDate,
                            UpdateBy = item.UpdateBy,
                            UpdateDate = item.UpdateDate
                        };

                        db.DocumentPromulgateFiles.Add(documentPromulgateFile);
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

        public SearchResultModel<DocumentFileModel> SearchDocumentFile(DocumentFileModel searchModel)
        {
            SearchResultModel<DocumentFileModel> searchResult = new SearchResultModel<DocumentFileModel>();

            var dataQuery = (from a in db.DocumentFiles.AsNoTracking()
                             where a.DocumentId.Equals(searchModel.DocumentId)
                             select new DocumentFileModel
                             {
                                 Id = a.Id,
                                 DocumentId = a.DocumentId,
                                 FileName = a.FileName,
                                 PDFPath = a.PDFPath,
                                 Path = a.Path
                             }).AsQueryable();

            var documentFileReference = (from a in db.DocumentReferences.AsNoTracking()
                                         where a.DocumentId.Equals(searchModel.DocumentId)
                                         join b in db.DocumentFiles.AsNoTracking() on a.ReferenceId equals b.DocumentId
                                         select new DocumentFileModel
                                         {
                                             Id = a.Id,
                                             DocumentId = a.DocumentId,
                                             FileName = b.FileName,
                                             PDFPath = b.PDFPath,
                                             Path = b.Path
                                         }).AsQueryable();

            var listDocumenFile = dataQuery.ToList();
            var listDocumentFileReference = documentFileReference.ToList();

            searchResult.ListResult.AddRange(listDocumenFile);
            searchResult.ListResult.AddRange(listDocumentFileReference);


            searchResult.TotalItem = searchResult.ListResult.Count();
            return searchResult;
        }

        public void CancelPromulgate(DocumentModel model)
        {
            var document = db.Documents.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            try
            {
                document.Status = Constants.Document_Status_Cancel;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void ReviewDocument(DocumentModel model)
        {
            var document = db.Documents.FirstOrDefault(a => a.Id.Equals(model.Id));
            if (document == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Document);
            }

            try
            {
                document.Status = Constants.Document_Status_Review;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public SearchResultModel<TaskFlowStageSearchResultModel> SearchTask(TaskFlowStageSearchModel searchModel)
        {
            SearchResultModel<TaskFlowStageSearchResultModel> searchResult = new SearchResultModel<TaskFlowStageSearchResultModel>();
            var dataQuery = (from a in db.Tasks.AsNoTracking()
                             join b in db.FlowStages.AsNoTracking() on a.FlowStageId equals b.Id into ab
                             from abv in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from acv in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from adv in ad.DefaultIfEmpty()

                             select new TaskFlowStageSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 IsDesignModule = a.IsDesignModule.HasValue ? a.IsDesignModule.Value : false,
                                 FlowStageId = a.FlowStageId,
                                 FlowStageName = abv != null ? abv.Name : "",
                                 Type = a.Type,
                                 CreateDate = a.CreateDate,
                                 SBUId = a.SBUId,
                                 SBUName = acv != null ? acv.Name : "",
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = adv != null ? adv.Name : ""

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.FlowStageId))
            {
                dataQuery = dataQuery.Where(u => u.FlowStageId.Equals(searchModel.FlowStageId));
            }

            if (searchModel.IsDesignModule.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.IsDesignModule == searchModel.IsDesignModule.Value);
            }

            if (searchModel.Type.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Type == searchModel.Type.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(searchModel.DepartmentId));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

    }
}
