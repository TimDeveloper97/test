using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Customers;
using NTS.Model.Product;
using NTS.Model.Project;
using NTS.Model.ProjectHistory;
using NTS.Model.Projects.Project;
using NTS.Model.Report;
using NTS.Model.Repositories;
using NTS.Model.TestCriteria;
using QLTK.Business.AutoMappers;
using QLTK.Business.Materials;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using static NTS.Model.Projects.Project.MoneyCollectionProjectReportResultModel;

namespace QLTK.Business.Projects
{
    public class ProductNeedPublicationsBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        

        /// <summary>
        /// danh sách dự án
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public SearchResultProjectModel<ProjectResultModel> SearchProject(string id)
        {
            SearchResultProjectModel<ProjectResultModel> searchResultObject = new SearchResultProjectModel<ProjectResultModel>();

            var listProjectProducts = db.ProjectProducts.Where(r=>r.ProductId.Equals(id)).GroupBy(r =>r.ProjectId).Select(r =>r.Key).ToList();
            string idProject = string.Join(";", listProjectProducts);
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join e in db.CustomerTypes.AsNoTracking() on b.CustomerTypeId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             where idProject.Contains(a.Id)
                             orderby a.Code
                             select new ProjectResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 DateFrom = a.DateFrom,
                                 SBUId = a.SBUId,
                                 DepartmentId = a.DepartmentId,
                                 CustomerTypeId = e.Id,
                                 CustomerType = e.Name,
                                 CustomerId = b.Id,
                                 CustomerName = b.Name,
                                 DateTo = a.DateTo,
                                 Status = a.Status,
                                 Parameter = a.Parameter,
                                 SaleNoVAT = a.SaleNoVAT,
                                 WarehouseCode = a.WarehouseCode,
                                 KickOffDate = a.KickOffDate,
                                 CreateDate = a.CreateDate,
                                 FCMPrice = a.FCMPrice,
                                 //DocumentStatus = a.DocumentStatus,

                                 Type = a.Type,
                                 PaymentStatus = a.PaymentStatus,
                                 Priority = a.Priority,
                             }).AsQueryable();
            var listProjectProduct = db.ProjectProducts.AsNoTracking().ToList();


            var list = dataQuery.ToList();

            searchResultObject.Status1 = list.Where(i => i.Status.Equals(Constants.Prooject_Status_NotStartedYet)).Count();
            searchResultObject.Status2 = list.Where(i => i.Status.Equals(Constants.Prooject_Status_Processing)).Count();
            searchResultObject.Status3 = list.Where(i => i.Status.Equals(Constants.Prooject_Status_Finish)).Count();
            searchResultObject.TotalItem = list.Count();

            //var listResult = list.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResultObject.ListResult = list;

            foreach (var item in list)
            {
                var projectProduct = db.ProjectProducts.Where(r => r.ProjectId.Equals(item.Id));
                if(projectProduct.ToList().Count >0)
                {
                    foreach(var itm in projectProduct)
                    {
                        if (itm.IsCatalogRequire)
                        {
                            item.IsCatalog = true;
                        }
                        

                        if (itm.IsUserGuideRequire)
                        {
                            item.IsPracticeExist = true;
                        }
                        
                        if (itm.IsMaintenaineGuideRequire)
                        {
                            item.IsManualMaintenance = true;
                        }
                       
                        if (itm.IsPracticeGuideRequire)
                        {
                            item.IsManualExist = true;
                        }
                        



                    }
                }


                var projectSolution = db.ProjectSolutions.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectId));
                if (projectSolution != null)
                {
                    item.IsSolution = true;
                }
                var projectTransferAttach = db.ProjectTransferAttaches.AsNoTracking().FirstOrDefault(i => item.Id.Equals(i.ProjectId));
                if (projectTransferAttach != null)
                {
                    item.IsTransfer = true;
                }

                var listprojectAttach = db.ProjectAttaches.Where(m => m.ProjectId.Equals(item.Id)).ToList();
                if (listprojectAttach != null)
                {
                    int a = 0;
                    foreach (var ite in listprojectAttach)
                    {

                        if (ite.Path != null && ite.IsRequired)
                        {
                            a++;
                        }
                        if (a == listprojectAttach.Count)
                        {
                            item.DocumentStatus = 1;
                        }
                        else if (a == 0)
                        {
                            item.DocumentStatus = 3;
                        }
                        else
                        {
                            item.DocumentStatus = 2;
                        }
                    }
                }
                else
                {
                    item.DocumentStatus = 0;
                }

            }
            return searchResultObject;

        }

        public SearchResultModel<ProductResultModel> SearchProduct(ProductSearchModel modelSearch, bool isExport)
        {
            SearchResultModel<ProductResultModel> searchResult = new SearchResultModel<ProductResultModel>();
            List<string> listParentId = new List<string>();

            var dataQuery = (from a in db.Products.AsNoTracking()
                             join b in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals b.Id
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.SBUs.AsNoTracking() on ca.SBUId equals d.Id into cad
                             from cnd in cad.DefaultIfEmpty()
                             orderby a.Code
                             select new ProductResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ProductGroupId = a.ProductGroupId,
                                 ProductGroupName = b.Name,
                                 ProductGroupCode = b.Code,
                                 Pricing = a.Price,
                                 IsManualExist = a.IsManualExist,
                                 IsQuoteExist = a.IsQuoteExist,
                                 IsPracticeExist = a.IsPracticeExist,
                                 IsLayoutExist = a.IsLayoutExist,
                                 IsMaterialExist = a.IsMaterialExist,
                                 IsManualMaintenance = a.IsManualMaintenance,
                                 IsCatalog = a.IsCatalog,
                                 ProcedureTime = a.ProcedureTime,
                                 CurentVersion = a.CurentVersion,
                                 Status = a.Status,
                                 IsEnoughtSearch = ((!a.IsManualExist) || (!a.IsQuoteExist) || (!a.IsPracticeExist) || (!a.IsLayoutExist) || (!a.IsMaterialExist)),
                                 DepartmentId = ca != null ? ca.Id : string.Empty,
                                 DepartmentName = ca != null ? ca.Name : string.Empty,
                                 SBUId = cnd != null ? cnd.Id : string.Empty,
                                 SBUName = cnd != null ? cnd.Name : string.Empty,
                                 IsTestResult = a.IsTestResult,
                                 IsSendSale = a.IsSendSale,
                                 SyncDate = a.SyncDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || r.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.TypeGuideMaintenance))
            {
                if (modelSearch.TypeGuideMaintenance.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsManualMaintenance == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsManualMaintenance == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.TypeDMBTH))
            {
                if (modelSearch.TypeDMBTH.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsManualExist == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsManualExist == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.TypeGuidePractice))
            {
                if (modelSearch.TypeGuidePractice.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsPracticeExist == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsPracticeExist == false);
                }
            }
            if (!string.IsNullOrEmpty(modelSearch.TypeCatalogs))
            {
                if (modelSearch.TypeCatalogs.Equals("1"))
                {
                    dataQuery = dataQuery.Where(a => a.IsCatalog == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(a => a.IsCatalog == false);
                }
            }

            var listProject = db.Projects.AsNoTracking();
            var listProjectProduct = db.ProjectProducts.AsNoTracking();
            List<string> listIdProduct = new List<string>();
            foreach(var item in listProject)
            {
                var projectProduct = listProjectProduct.Where(r => r.ProjectId.Equals(item.Id) &&( r.IsCatalogRequire || r.IsUserGuideRequire || r.IsMaintenaineGuideRequire || r.IsPracticeGuideRequire)).Select(r =>r.ProductId).ToList();
                if(projectProduct.Count > 0)
                {
                    listIdProduct.AddRange(projectProduct);
                }
            }
            string idProduct = string.Join(";", listIdProduct);
            if (listIdProduct.Count > 0)
            {
                dataQuery = dataQuery.Where(r => idProduct.Contains(r.Id));
            }

            searchResult.Status2 = dataQuery.Where(u => u.IsEnoughtSearch.Value).Count();
            searchResult.Status1 = dataQuery.Where(u => !u.IsEnoughtSearch.Value).Count();

            searchResult.TotalItem = dataQuery.Count();
            if (isExport)
            {
                searchResult.ListResult = dataQuery.ToList();
            }
            else
            {
                searchResult.Date = dataQuery.Max(i => i.SyncDate);
                searchResult.ListResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }
            return searchResult;
        }


    }
}
