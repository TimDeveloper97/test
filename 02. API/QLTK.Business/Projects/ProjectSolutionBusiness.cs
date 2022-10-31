using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ProjectSolution;
using NTS.Model.Repositories;
using NTS.Model.Solution;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ProjectSolutions
{
    public class ProjectSolutionBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public SearchResultModel<SolutionModel> SearchProjectSolution(SolutionSearchModel modelSearch)
        {
            SearchResultModel<SolutionModel> searchResult = new SearchResultModel<SolutionModel>();

            var dataQuery = (from a in db.Solutions.AsNoTracking()
                             join b in db.SolutionGroups.AsNoTracking() on a.SolutionGroupId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Customers.AsNoTracking() on a.EndCustomerId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.Employees.AsNoTracking() on a.BusinessUserId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.ProjectSolutions.AsNoTracking() on a.Id equals f.SolutionId into af
                             from fa in af.DefaultIfEmpty()
                             join h in db.Employees.AsNoTracking() on a.SolutionMaker equals h.Id into ah
                             from ha in ah.DefaultIfEmpty()
                             where modelSearch.ProjectId.Equals(fa.ProjectId)
                             select new SolutionModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 CustomerId = a.CustomerId,
                                 CustomerName = c.Name,
                                 EndCustomerId = a.EndCustomerId,
                                 EndCustomerName = d.Name,
                                 BusinessUserId = a.BusinessUserId,
                                 TPAUName = e.Name,
                                 SolutionMakerName = ha.Name,
                                 Price = a.Price,
                                 FinishDate = a.FinishDate,
                                 SolutionGroupId = a.SolutionGroupId,
                                 SolutionGroupName = b.Name,
                                 ProjectSolutionId = fa.Id,
                                 DMVTExist = a.DMVTExist,
                                 Design2DExist = a.Design2DExist,
                                 FCMExist = a.FCMExist,
                                 Design3DExist = a.Design3DExist,
                                 TSTKExist = a.TSTKExist,
                                 ExplanExist = a.ExplanExist
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            foreach (var item in listResult)
            {
                List<string> listRs = new List<string>();
                item.ProjectSolution = db.ProjectSolutions.Where(i => i.SolutionId.Equals(item.Id)).ToList().Count;
                var listProject = (from a in db.ProjectSolutions.AsNoTracking()
                                   join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                                   where a.SolutionId.Equals(item.Id)
                                   select new SolutionModel
                                   {
                                       ProjectCode = b.Code,
                                       ProjectName = b.Name
                                   }).AsQueryable().ToList();
                foreach (var it in listProject)
                {
                    if (listProject.Count > 1)
                    {
                        item.ProjectCode += ", " + it.ProjectCode;
                        item.ProjectName += "," + it.ProjectName;
                        item.ProjectCode = item.ProjectCode.Substring(1);
                    }
                    else
                    {
                        item.ProjectName += it.ProjectName;
                        item.ProjectCode += it.ProjectCode;
                    }
                }
            }
            searchResult.ListResult = listResult;
            foreach (var item in listResult)
            {
                var listAttach = (from a in db.SolutionAttaches.AsNoTracking()
                                  where a.SolutionId.Equals(item.Id)
                                  orderby a.FileName
                                  select new SolutionAttachModel
                                  {
                                      Id = a.Id,
                                      SolutionId = a.SolutionId,
                                      FileName = a.FileName,
                                      FileSize = a.FileSize,
                                      Path = a.Path,
                                      Type = a.Type
                                  }).ToList();
                item.ListFile = listAttach;               
            }
            return searchResult;
        }

        public SearchResultModel<SolutionModel> SearchSolution(SolutionSearchModel modelSearch)
        {
            SearchResultModel<SolutionModel> searchResult = new SearchResultModel<SolutionModel>();

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
                                 //join i in db.ProjectSolutions.AsNoTracking() on a.Id equals i.SolutionId into ai
                                 //from ia in ai.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
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
                                 //ProjectSolutionId = ia.Id
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.SolutionGroupId))
            {
                dataQuery = dataQuery.Where(u => u.SolutionGroupId.Equals(modelSearch.SolutionGroupId));
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

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUSolutionMakerId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentSolutionMakerId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.SolutionMaker))
            {
                dataQuery = dataQuery.Where(u => u.SolutionMaker.Equals(modelSearch.SolutionMaker));
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
            var listResult = dataQuery.ToList();
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
            }
            searchResult.Status2 = data;
            searchResult.Status3 = searchResult.TotalItem - data;
            return searchResult;
        }

        /// <summary>
        /// Thêm sản phẩm vào giải pháp
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"> người đăng nhập</param>

        public void AddProjectSolution(ProjectSolutionModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    ProjectSolution projectSolution;
                    List<ProjectSolution> listSolution = new List<ProjectSolution>();
                    ProjectSolutionProduct projectSolutionProduct;
                    List<ProjectSolutionProduct> listProjectSolutionProduct = new List<ProjectSolutionProduct>();

                    if (model.ListResult.Count > 0)
                    {
                        foreach (var item in model.ListResult)
                        {
                            if (string.IsNullOrEmpty(item.ProjectSolutionId))
                            {
                                projectSolution = new ProjectSolution
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectId = model.ProjectId,
                                    SolutionId = item.Id,
                                };
                                listSolution.Add(projectSolution);
                            }
                            if (item.IsDelete == true)
                            {
                                var listDelete = db.ProjectSolutions.Where(a => a.Id.Equals(item.ProjectSolutionId)).ToList();
                                var listProjectProductSolution = db.ProjectSolutionProducts.Where(a => a.ProjectSolutionId.Equals(item.ProjectSolutionId)).ToList();
                                db.ProjectSolutionProducts.RemoveRange(listProjectProductSolution);
                                db.ProjectSolutions.RemoveRange(listDelete);
                            }
                        }


                        var listproduct = db.ProjectSolutionProducts.Where(a => a.ProjectSolutionId.Equals(model.ProjectProductSolutionId)).ToList();
                        db.ProjectSolutionProducts.RemoveRange(listproduct);

                        foreach (var item in model.ListProduct)
                        {
                            if (string.IsNullOrEmpty(model.ProjectProductSolutionId))
                            {
                                var checkName = listSolution.FirstOrDefault(a => a.SolutionId.Equals(model.SolutionId));
                                projectSolutionProduct = new ProjectSolutionProduct()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectSolutionId = checkName.Id,
                                    ProjectProductId = item.Id,
                                };
                                listProjectSolutionProduct.Add(projectSolutionProduct);
                            }
                            else
                            {
                                projectSolutionProduct = new ProjectSolutionProduct()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProjectSolutionId = model.ProjectProductSolutionId,
                                    ProjectProductId = item.Id,
                                };
                                listProjectSolutionProduct.Add(projectSolutionProduct);
                            }

                        }
                    }

                    db.ProjectSolutions.AddRange(listSolution);
                    db.ProjectSolutionProducts.AddRange(listProjectSolutionProduct);

                    UserLogUtil.LogHistotyUpdateSub(db, userId, Constants.LOG_Project, model.ProjectId, string.Empty, "Giải pháp");


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
        /// Lấy sản phẩm theo mã dự án
        /// </summary>
        /// <param name="projectId">mã dự án</param>
        /// <returns></returns>
        public List<ProjectProductToSolutionModel> GetProjectProductByProjectId(string projectId, string projectSolutionId)
        {
            List<ProjectProductToSolutionModel> listProjectProductNotParentId = new List<ProjectProductToSolutionModel>();
            var data = (from a in db.ProjectProducts.AsNoTracking()
                            where a.ProjectId.Equals(projectId) && string.IsNullOrEmpty(a.ParentId)
                            join b in db.ProjectSolutionProducts.AsNoTracking() on a.Id equals b.ProjectProductId into ab
                            from abn in ab.DefaultIfEmpty()
                            where abn == null || abn.ProjectSolutionId.Equals(projectSolutionId)
                            orderby a.ContractIndex
                            select new ProjectProductToSolutionModel
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
                                ModuleId = a.ModuleId
                            }).ToList();

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

        public object StatusSolutionProduct(string projectId)
        {
            string Status = string.Empty;
            var listProduct = (from a in db.ProjectProducts.AsNoTracking()
                               where a.ProjectId.Equals(projectId) && string.IsNullOrEmpty(a.ParentId)
                               select a.Id).ToList();
            int TotalProduct = listProduct.Count;
            int TotalProjectSolutionProduct = db.ProjectSolutionProducts.AsNoTracking().Where(a => listProduct.Contains(a.ProjectProductId)).Count();
            if (TotalProduct == TotalProjectSolutionProduct)
            {
                Status = "Đã đủ";
            }
            else
            {
                Status = "Chưa đủ";
            }
            return new { TotalProduct, TotalProjectSolutionProduct, Status };
        }
    }
}
