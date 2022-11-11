using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.Project;
using NTS.Model.ProjectTransferAttach;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ProjectTransferAttachs
{
    public class ProjectTransferAttachBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ProjectTransferAttachModel> SearchProjectTransferAttachs(ProjectTransferAttachAddModel modelSearch)
        {
            SearchResultModel<ProjectTransferAttachModel> searchResult = new SearchResultModel<ProjectTransferAttachModel>();
            var dataQuey = (from b in db.ProjectTransferAttaches.AsNoTracking()
                            join c in db.Users.AsNoTracking() on b.CreateBy equals c.Id
                            join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                            where b.ProjectId.Equals(modelSearch.ProjectId)
                            orderby b.FileName
                            select new ProjectTransferAttachModel
                            {
                                Id = b.Id,
                                ProjectId = b.ProjectId,
                                FileName = b.FileName,
                                FileSize = b.FileSize,
                                FilePath = b.Path,
                                Note = b.Note,
                                CreateBy = b.CreateBy,
                                CreateByName = d.Name,
                                CreateDate = b.CreateDate,
                                SignDate = b.SignDate,
                                NumberOfReport = b.NumberOfReport
                            }).AsQueryable();
            searchResult.ListResult = dataQuey.ToList();
            return searchResult;
        }

        public void AddProjectTransferAttach(ProjectTransferAttachAddModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProjectTransferAttach fileEntity;
                    ProjectTransferProduct projectTranferProduct;
                    List<ProjectTransferProduct> listProjectTransferPro = new List<ProjectTransferProduct>();
                    List<ProjectTransferAttach> listFileEntity = new List<ProjectTransferAttach>();
                    List<ProjectTransferAttach> listFileRemove = new List<ProjectTransferAttach>();

                    if (model.ListFile.Count > 0)
                    {

                        foreach (var item in model.ListFile)
                        {
                            if (string.IsNullOrEmpty(item.FilePath))
                            {
                                break;
                            }
                            if (!string.IsNullOrEmpty(item.FilePath) && string.IsNullOrEmpty(item.Id) && !string.IsNullOrEmpty(item.FileName))
                            {
                                fileEntity = new ProjectTransferAttach()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectId = model.ProjectId,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    Path = item.FilePath,
                                    CreateBy = model.CreateBy,
                                    CreateDate = DateTime.Now,
                                    SignDate = item.SignDate,
                                    NumberOfReport = item.NumberOfReport,
                                };
                                listFileEntity.Add(fileEntity);
                            }

                            if (item.IsDelete == true)
                            {
                                var File = db.ProjectTransferAttaches.FirstOrDefault(a => a.Id.Equals(item.Id));
                                var ProjectTranferProduct = db.ProjectTransferProducts.Where(a => a.ProjectTransferAttachId.Equals(item.Id)).ToList();
                                db.ProjectTransferProducts.RemoveRange(ProjectTranferProduct);
                                db.ProjectTransferAttaches.Remove(File);
                            }
                            if (!string.IsNullOrEmpty(item.Id))
                            {
                                var File = db.ProjectTransferAttaches.FirstOrDefault(a => a.Id.Equals(model.FileId));
                                if (File != null)
                                {
                                    var newData = db.ProjectTransferAttaches.FirstOrDefault(u => u.Id.Equals(item.Id));
                                    newData.ProjectId = item.ProjectId;
                                    newData.FileName = item.FileName;
                                    newData.FileSize = item.FileSize;
                                    newData.Path = item.FilePath;
                                    newData.CreateBy = item.CreateBy;
                                    newData.CreateDate = DateTime.Now;
                                    newData.SignDate = item.SignDate;
                                    newData.NumberOfReport = item.NumberOfReport;
                                }
                            }
                        }

                        db.ProjectTransferAttaches.AddRange(listFileEntity);


                        db.SaveChanges();


                        var listproduct = db.ProjectTransferProducts.Where(a => a.ProjectTransferAttachId.Equals(model.FileId)).ToList();
                        db.ProjectTransferProducts.RemoveRange(listproduct);


                        if (model.ListProductTranfer.Count > 0)
                        {
                            foreach (var item in model.ListProductTranfer)
                            {

                                if (string.IsNullOrEmpty(model.FileId) && !string.IsNullOrEmpty(model.FileName))
                                {
                                    var checkName = listFileEntity.FirstOrDefault(a => a.FileName.Equals(model.FileName));
                                    projectTranferProduct = new ProjectTransferProduct()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProjectTransferAttachId = checkName.Id,
                                        ProjectProductId = item.Id,
                                        Quantity = 0,
                                    };
                                    listProjectTransferPro.Add(projectTranferProduct);
                                }

                                else
                                {
                                    projectTranferProduct = new ProjectTransferProduct()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ProjectTransferAttachId = model.FileId,
                                        ProjectProductId = item.Id,
                                        Quantity = 0,
                                    };
                                    listProjectTransferPro.Add(projectTranferProduct);
                                }
                                //}
                            }
                            db.ProjectTransferProducts.AddRange(listProjectTransferPro);
                        }

                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Project, model.ProjectId, string.Empty, "Biên bản chuyển giao");

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
        /// Lấy list sản phẩm không có nhóm cho 
        /// </summary>
        /// <param name="projectId">tên dự án</param>
        /// <param name="fileId">tên file select</param>
        /// <returns></returns>
        public List<ProjectProductToTranferModel> GetProjectProductToTranfer(string projectId, string fileId)
        {
            List<ProjectProductToTranferModel> listProjectProductNotParentId = new List<ProjectProductToTranferModel>();
            var data = (from a in db.ProjectProducts.AsNoTracking()
                        where a.ProjectId.Equals(projectId) && string.IsNullOrEmpty(a.ParentId)
                        join b in db.ProjectTransferProducts.AsNoTracking() on a.Id equals b.ProjectProductId into ab
                        from abn in ab.DefaultIfEmpty()
                            //where abn == null || abn.ProjectTransferAttachId.Equals(fileId)
                        orderby a.ContractIndex
                        join c in db.ProjectTransferAttaches.AsNoTracking() on abn.ProjectTransferAttachId equals c.Id into bc
                        from bcv in bc.DefaultIfEmpty()
                        select new ProjectProductToTranferModel
                        {
                            Id = a.Id,
                            ProjectId = a.ProjectId,
                            ParentId = a.ParentId,
                            ContractCode = a.ContractCode,
                            ContractName = a.ContractName,
                            Note = a.Note,
                            Price = a.Price,
                            Amount = a.RealQuantity * a.Price,
                            Checked = abn != null ? true : false,
                            ProductId = a.ProductId,
                            DataType = a.DataType,
                            ModuleId = a.ModuleId,
                            ProjectTransferAttachId = abn != null ? abn.ProjectTransferAttachId : null,
                            SignDate = bcv != null ? bcv.SignDate : null,
                            NumberOfReport = bcv != null ? bcv.NumberOfReport : ""
                        }).AsEnumerable();
            var list = data.ToList();
            foreach (var item in list)
            {
                if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
                {
                    var product = db.Products.AsNoTracking().FirstOrDefault(i => item.ProductId.Equals(i.Id));
                    if (product != null)
                    {
                        item.Code = product.Code;
                        item.Name = product.Name;
                    }
                }
                else if (item.DataType == Constants.ProjectProduct_DataType_Module || item.DataType == Constants.ProjectProduct_DataType_Paradigm)
                {
                    var module = db.Modules.AsNoTracking().FirstOrDefault(i => item.ModuleId.Equals(i.Id));
                    if (module != null)
                    {
                        item.Code = module.Code;
                        item.Name = module.Name;
                    }
                }
            }
            var listData = list.ToList();
            return listData;
        }

        public object StatusTranferProduct(string projectId)
        {
            string Status = string.Empty;
            var listProduct = (from a in db.ProjectProducts.AsNoTracking()
                               where a.ProjectId.Equals(projectId) && string.IsNullOrEmpty(a.ParentId)
                               select a.Id).ToList();
            int TotalProduct = listProduct.Count;
            int TotalProjectTranferProduct = db.ProjectTransferProducts.AsNoTracking().Where(a => listProduct.Contains(a.ProjectProductId)).Count();
            if (TotalProduct == TotalProjectTranferProduct)
            {
                Status = "Đã đủ";
            }
            else
            {
                Status = "Chưa đủ";
            }
            return new { TotalProduct, TotalProjectTranferProduct, Status };
        }


        public object getListPlanTransferByProjectId(string projectId)
        {
            object listPlan = new object();
            //var listPlan = (from a in db.Plans.AsNoTracking()
            //                join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
            //                join b in db.Projects.AsNoTracking() on d.ProjectId equals b.Id
            //                join c in db.Tasks.AsNoTracking() on a.TaskId equals c.Id
            //                join e in db.Employees.AsNoTracking() on a.ResponsiblePersion equals e.Id into ae
            //                from ea in ae.DefaultIfEmpty()
            //                where a.ProjectId.Equals(projectId) && c.Type == Constants.Task_Transfer && a.Status != Constants.Plan_Status_Done && string.IsNullOrEmpty(a.ReferenceId)
            //                select new PlanTransferByProjectModel
            //                {
            //                    Id = a.Id,
            //                    TaskId = c.Id,
            //                    TaskName = c.Name,
            //                    ProjectId = a.ProjectId,
            //                    ExecutionTime = a.ExecutionTime,
            //                    ProjectName = b.Name,
            //                    projectCode = b.Code,
            //                    ProjectProductId = a.ProjectProductId,
            //                    Type = c.Type,
            //                    RealStartDate = a.RealStartDate,
            //                    RealEndDate = a.RealEndDate,
            //                    StartDate = a.StartDate,
            //                    ResponsiblePersion = ea != null ? ea.Name : string.Empty,
            //                    EndDate = a.EndDate,
            //                    EsimateTime = a.EsimateTime,
            //                    Status = a.Status,
            //                    ExpectedDesignFinishDate = d.ExpectedDesignFinishDate,
            //                    ExpectedMakeFinishDate = d.ExpectedMakeFinishDate,
            //                    ExpectedTransferDate = d.ExpectedTransferDate,
            //                    DataType = d.DataType,
            //                    ProductId = d.ProductId,
            //                    ModuleId = d.ModuleId,
            //                    ContractName = d.ContractName,
            //                    ContractCode = d.ContractCode,
            //                }).ToList();

            //foreach (var item in listPlan)
            //{
            //    if (item.DataType == Constants.ProjectProduct_DataType_ProjectProduct)
            //    {
            //        var product = db.Products.AsNoTracking().FirstOrDefault(i => item.ProductId.Equals(i.Id));
            //        if (product != null)
            //        {
            //            item.Code = product.Code;
            //            item.Name = product.Name;
            //        }
            //    }
            //    else if (item.DataType == Constants.ProjectProduct_DataType_Module || item.DataType == Constants.ProjectProduct_DataType_Paradigm)
            //    {
            //        var module = db.Modules.AsNoTracking().FirstOrDefault(i => item.ModuleId.Equals(i.Id));
            //        if (module != null)
            //        {
            //            item.Code = module.Code;
            //            item.Name = module.Name;
            //        }
            //    }
            //}

            return listPlan;
        }

        public void updatePlanStatusByProjectId(List<string> plansId)
        {
            var listPlan = db.Plans.Where(a => plansId.Contains(a.Id)).ToList();

            foreach (var item in listPlan)
            {
                item.Status = Constants.Plan_Status_Done;
            }
            db.SaveChanges();
        }
    }
}