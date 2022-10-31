using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Holiday;
using NTS.Model.ImportPR;
using NTS.Model.ImportProfile;
using NTS.Model.ImportProfileDocumentConfigs;
using NTS.Model.ImportProfileProblemExist;
using NTS.Model.ImportProfileTransportSupplier;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
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

namespace QLTK.Business.ImportProfiles
{
    public class ImportProfileBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public ImportProfileSearchResultModel<ImportProfileReusltModel> SearchImportProfile(ImportProfileSearchModel searchModel, bool isAll)
        {
            ImportProfileSearchResultModel<ImportProfileReusltModel> searchResult = new ImportProfileSearchResultModel<ImportProfileReusltModel>();

            DateTime payDeadline = DateTime.Now.Date.AddDays(7);

            var dateNow = DateTime.Now.Date;
            var importProfiles = (from d in db.ImportProfiles.AsNoTracking()
                                  join s in db.Suppliers.AsNoTracking() on d.SupplierId equals s.Id into ds
                                  from dsn in ds.DefaultIfEmpty()
                                  join e in db.Employees.AsNoTracking() on d.EmployeeId equals e.Id
                                  select new ImportProfileReusltModel
                                  {
                                      Id = d.Id,
                                      Amount = d.Amount,
                                      AmountVND = d.CurrencyUnit == Constants.CurrencyUnit_VNĐ ? d.Amount : d.Amount * d.SupplierExchangeRate,
                                      Code = d.Code,
                                      PRDueDate = d.PRDueDate,
                                      EstimatedDeliveryDate = d.EstimatedDeliveryDate,
                                      Name = d.Name,
                                      PayDueDate = d.PayDueDate,
                                      PayStatus = d.PayStatus,
                                      PayIndex = d.PayIndex,
                                      Status = d.Step == Constants.ImportProfile_Step_Finish ? d.Status : !d.PRDueDate.HasValue || d.PRDueDate >= dateNow ? 1 : 2,
                                      ProductProgress = (d.Step == Constants.ImportProfile_Step_Finish) ? d.Status : (!d.EstimatedDeliveryDate.HasValue || d.EstimatedDeliveryDate >= dateNow ? 1 : 2),
                                      Step = d.Step,
                                      SupplierCode = dsn != null ? dsn.Code : string.Empty,
                                      SupplierName = dsn != null ? dsn.Name : string.Empty,
                                      SupplierId = d.SupplierId,
                                      EmployeeId = e.Id,
                                      EmployeeName = e.Name,
                                      ProjectCode = d.ProjectCode,
                                      ProjectName = d.ProjectName,
                                      ManufacturerCode = d.ManufacturerCode,
                                      PRCode = d.PRCode,
                                      CurrentExpected = d.CurrentExpected,
                                      CurrencyUnit = d.CurrencyUnit,
                                      PayWarning = d.PayDueDate.HasValue && d.PayDueDate <= payDeadline && d.PayStatus != Constants.ImportProfilePayment_Status_Pay ? (d.PayDueDate < dateNow ? 2 : 1) : 0,
                                      SupplierFinishStatus = d.SupplierFinishDate.HasValue && d.SupplierExpectedDate.HasValue ? d.SupplierFinishStatus : !d.SupplierExpectedDate.HasValue ? 3 : d.SupplierExpectedDate.Value >= dateNow ? 1 : 2,
                                      SupplierExpectedDate = d.SupplierExpectedDate,
                                      ContractFinishStatus = d.ContractFinishDate.HasValue && d.ContractExpectedDate.HasValue ? d.ContractFinishStatus : !d.ContractExpectedDate.HasValue ? 3 : d.ContractExpectedDate.Value >= dateNow ? 1 : 2,
                                      ContractExpectedDate = d.ContractExpectedDate,
                                      PayFinishStatus = d.PayFinishDate.HasValue && d.PayExpectedDate.HasValue ? d.PayFinishStatus : !d.PayExpectedDate.HasValue ? 3 : d.PayExpectedDate.Value >= dateNow ? 1 : 2,
                                      PayExpectedDate = d.PayExpectedDate,
                                      ProductionFinishStatus = d.ProductionFinishDate.HasValue && d.ProductionExpectedDate.HasValue ? d.ProductionFinishStatus : !d.ProductionExpectedDate.HasValue ? 3 : (!d.ProductionExpectedDate1.HasValue ? (d.ProductionExpectedDate.Value >= dateNow ? 1 : 2) : (!d.ProductionExpectedDate2.HasValue ? (d.ProductionExpectedDate1.Value >= dateNow ? 1 : 2) : (d.ProductionExpectedDate2.Value >= dateNow ? 1 : 2))),
                                      ProductionExpectedDate = d.ProductionExpectedDate2.HasValue ? d.ProductionExpectedDate2 : d.ProductionExpectedDate1.HasValue ? d.ProductionExpectedDate1 : d.ProductionExpectedDate,
                                      TransportFinishStatus = d.TransportFinishDate.HasValue && d.TransportExpectedDate.HasValue ? d.TransportFinishStatus : !d.TransportExpectedDate.HasValue ? 3 : d.TransportExpectedDate.Value >= dateNow ? 1 : 2,
                                      TransportExpectedDate = d.TransportExpectedDate,
                                      CustomsFinishStatus = d.CustomsFinishDate.HasValue && d.CustomExpectedDate.HasValue ? d.CustomsFinishStatus : !d.CustomExpectedDate.HasValue ? 3 : d.CustomExpectedDate.Value >= dateNow ? 1 : 2,
                                      CustomExpectedDate = d.CustomExpectedDate,
                                      WarehouseFinishStatus = d.WarehouseFinishDate.HasValue && d.WarehouseExpectedDate.HasValue ? d.WarehouseFinishStatus : !d.WarehouseExpectedDate.HasValue ? 3 : d.WarehouseExpectedDate.Value >= dateNow ? 1 : 2,
                                      WarehouseExpectedDate = d.WarehouseExpectedDate,
                                      ProblemExistQuantity = db.ImportProfileProblemExists.Where(t => t.ImportProfileId.Equals(d.Id)).Count()
                                  }).AsQueryable();

            //var problemQuantity = db.ImportProfileProblemExists.Where(t=>t.ImportProfileId.Equals())

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                importProfiles = importProfiles.Where(r => r.Code.ToLower().Contains(searchModel.Code.ToLower()) || r.Name.ToLower().Contains(searchModel.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                importProfiles = importProfiles.Where(r => r.EmployeeId.Equals(searchModel.EmployeeId));
            }

            if (searchModel.Step.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.Step == searchModel.Step.Value);
            }

            if (searchModel.FinishStatus.HasValue)
            {
                if (searchModel.StepSearch.HasValue)
                {
                    if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_ConfirmSupplier)
                    {
                        importProfiles = importProfiles.Where(t => t.SupplierFinishStatus == searchModel.FinishStatus);
                    }
                    else if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_Contract)
                    {
                        importProfiles = importProfiles.Where(t => t.ContractFinishStatus == searchModel.FinishStatus);
                    }
                    else if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_Payment)
                    {
                        importProfiles = importProfiles.Where(t => t.PayFinishStatus == searchModel.FinishStatus);
                    }
                    else if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_Production)
                    {
                        importProfiles = importProfiles.Where(t => t.ProductionFinishStatus == searchModel.FinishStatus);
                    }
                    else if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_Transport)
                    {
                        importProfiles = importProfiles.Where(t => t.TransportFinishStatus == searchModel.FinishStatus);
                    }
                    else if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_Customs)
                    {
                        importProfiles = importProfiles.Where(t => t.CustomsFinishStatus == searchModel.FinishStatus);
                    }
                    else if (searchModel.StepSearch.Value == Constants.ImportProfile_Step_Import)
                    {
                        importProfiles = importProfiles.Where(t => t.WarehouseFinishStatus == searchModel.FinishStatus);
                    }
                }
                else
                {
                    importProfiles = importProfiles.Where(r => r.SupplierFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_ConfirmSupplier || r.ContractFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_Contract || r.PayFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_Payment || r.ProductionFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_Production || r.TransportFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_Transport || r.CustomsFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_Customs || r.WarehouseFinishStatus == searchModel.FinishStatus.Value && r.Step == Constants.ImportProfile_Step_Import);
                }
            }

            if (searchModel.Status.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.Status == searchModel.Status.Value);
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectCode))
            {
                importProfiles = importProfiles.Where(r => !string.IsNullOrEmpty(r.ProjectCode) && r.ProjectCode.ToLower().Contains(searchModel.ProjectCode));
            }

            if (!string.IsNullOrEmpty(searchModel.PRCode))
            {
                importProfiles = importProfiles.Where(r => !string.IsNullOrEmpty(r.PRCode) && r.PRCode.ToLower().Contains(searchModel.PRCode));
            }

            if (!string.IsNullOrEmpty(searchModel.SupplierId))
            {
                importProfiles = importProfiles.Where(r => r.SupplierId.ToLower().Equals(searchModel.SupplierId));
            }

            if (searchModel.DueDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.PRDueDate >= searchModel.DueDateFrom.Value);
            }

            if (searchModel.DueDateTo.HasValue)
            {
                var dueDateTo = searchModel.DueDateTo.Value.ToEndDate();
                importProfiles = importProfiles.Where(r => r.PRDueDate <= dueDateTo);
            }

            if (searchModel.PayDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.PayDueDate >= searchModel.PayDateFrom.Value);
            }

            if (searchModel.PayDateTo.HasValue)
            {
                var payDateTo = searchModel.PayDateTo.Value.ToEndDate();
                importProfiles = importProfiles.Where(r => r.PayDueDate <= payDateTo);
            }

            if (searchModel.DeliveryDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(r => r.EstimatedDeliveryDate >= searchModel.DeliveryDateFrom.Value);
            }

            if (searchModel.DeliveryDateTo.HasValue)
            {
                var deliveryDateTo = searchModel.DeliveryDateTo.Value.ToEndDate();
                importProfiles = importProfiles.Where(r => r.EstimatedDeliveryDate <= deliveryDateTo);
            }

            var startWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday).ToStartDate();
            var endWeek = startWeek.AddDays(7).ToEndDate();

            if (!searchModel.IsProductionExpiredOnWeek && !searchModel.IsProductionExpired)
            {
                if (searchModel.ProductionExpectedDateFrom.HasValue)
                {
                    importProfiles = importProfiles.Where(r => r.ProductionExpectedDate >= searchModel.ProductionExpectedDateFrom.Value);
                }

                if (searchModel.ProductionExpectedDateTo.HasValue)
                {
                    var productionExpectedDateTo = searchModel.ProductionExpectedDateTo.Value.ToEndDate();
                    importProfiles = importProfiles.Where(r => r.ProductionExpectedDate <= productionExpectedDateTo);
                }
            }
            else if (searchModel.IsProductionExpiredOnWeek)
            {
                importProfiles = importProfiles.Where(r => r.ProductionExpectedDate >= startWeek && r.ProductionExpectedDate <= endWeek);
            }
            else if (searchModel.IsProductionExpired)
            {
                importProfiles = importProfiles.Where(r => r.ProductionExpectedDate < dateNow);
            }

            if (searchModel.PayStatus.HasValue)
            {
                if (searchModel.PayStatus.Value == Constants.ImportProfilePayment_Status_Warning)
                {
                    importProfiles = importProfiles.Where(r => r.PayWarning == 1);
                }
                else if (searchModel.PayStatus.Value == Constants.ImportProfilePayment_Status_Expired)
                {
                    importProfiles = importProfiles.Where(r => r.PayWarning == 2);
                }
                else
                {
                    importProfiles = importProfiles.Where(r => r.PayStatus == searchModel.PayStatus.Value);
                }
            }

            if (searchModel.WorkStatus.HasValue)
            {
                // Đã kết thúc
                if (searchModel.WorkStatus == 1)
                {
                    importProfiles = importProfiles.Where(r => r.Step == Constants.ImportProfile_Step_Finish);
                }
                // Đang thực hiện
                else
                {
                    importProfiles = importProfiles.Where(r => r.Step < Constants.ImportProfile_Step_Finish);
                }
            }

            if (searchModel.IsSearch7Day)
            {
                var dateAfter = dateNow.AddDays(6);
                importProfiles = importProfiles.Where(t => t.CurrentExpected.HasValue && t.CurrentExpected <= dateAfter && t.CurrentExpected >= dateNow);
            }

            if (searchModel.SearchDateFrom.HasValue)
            {
                importProfiles = importProfiles.Where(t => t.CurrentExpected >= searchModel.SearchDateFrom.Value);
            }

            if (searchModel.SearchDateTo.HasValue)
            {
                importProfiles = importProfiles.Where(t => t.CurrentExpected <= searchModel.SearchDateTo.Value);
            }

            if (searchModel.IsPayExpired)
            {
                importProfiles = importProfiles.Where(r => r.PayWarning > 1);
            }

            if (searchModel.IsPayWarning)
            {
                importProfiles = importProfiles.Where(r => r.PayWarning == 1);
            }

            searchResult.TotalItem = importProfiles.Count();
            searchResult.PayWarningTotal = importProfiles.Count(r => r.PayWarning == 1);
            searchResult.PayExpiredTotal = importProfiles.Count(r => r.PayWarning > 1);


            searchResult.ProductionExpiredTotal = importProfiles.Count(r => r.ProductionExpectedDate < dateNow);
            searchResult.ProductionExpiredWeekTotal = importProfiles.Count(r => r.ProductionExpectedDate >= startWeek && r.ProductionExpectedDate <= endWeek);


            if (isAll)
            {
                searchResult.ListResult = SQLHelpper.OrderBy(importProfiles, searchModel.OrderBy, searchModel.OrderType).ToList();
            }
            else
            {
                searchResult.ListResult = SQLHelpper.OrderBy(importProfiles, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

                if (searchModel.WorkStatus != Constants.ImportProfile_WorkStatus_Finish)
                {

                    foreach (var item in searchResult.ListResult)
                    {


                        item.SupplierExpectedDateDay = item.SupplierExpectedDate.HasValue ? item.SupplierExpectedDate.Value.GetDayOfWWeek() : string.Empty;
                        item.ContractExpectedDateDay = item.ContractExpectedDate.HasValue ? item.ContractExpectedDate.Value.GetDayOfWWeek() : string.Empty;
                        item.PayExpectedDateDay = item.PayExpectedDate.HasValue ? item.PayExpectedDate.Value.GetDayOfWWeek() : string.Empty;
                        item.ProductionExpectedDateDay = item.ProductionExpectedDate.HasValue ? item.ProductionExpectedDate.Value.GetDayOfWWeek() : string.Empty;
                        item.TransportExpectedDateDay = item.TransportExpectedDate.HasValue ? item.TransportExpectedDate.Value.GetDayOfWWeek() : string.Empty;
                        item.CustomExpectedDateDay = item.CustomExpectedDate.HasValue ? item.CustomExpectedDate.Value.GetDayOfWWeek() : string.Empty;
                        item.WarehouseExpectedDateDay = item.WarehouseExpectedDate.HasValue ? item.WarehouseExpectedDate.Value.GetDayOfWWeek() : string.Empty;

                        switch (item.Step)
                        {
                            case Constants.ImportProfile_Step_ConfirmSupplier:
                                if (item.SupplierFinishStatus != 2)
                                {
                                    item.StepStatus = !item.SupplierExpectedDate.HasValue ? 4 : (item.SupplierExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                            case Constants.ImportProfile_Step_Contract:
                                if (item.ContractFinishStatus != 2)
                                {
                                    item.StepStatus = !item.ContractExpectedDate.HasValue ? 4 : (item.ContractExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                            case Constants.ImportProfile_Step_Payment:
                                if (item.PayFinishStatus != 2)
                                {
                                    item.StepStatus = !item.PayExpectedDate.HasValue ? 4 : (item.PayExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                            case Constants.ImportProfile_Step_Production:
                                if (item.ProductionFinishStatus != 2)
                                {
                                    item.StepStatus = !item.ProductionExpectedDate.HasValue ? 4 : (item.ProductionExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                            case Constants.ImportProfile_Step_Transport:
                                if (item.TransportFinishStatus != 2)
                                {
                                    item.StepStatus = !item.TransportExpectedDate.HasValue ? 4 : (item.TransportExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                            case Constants.ImportProfile_Step_Customs:
                                if (item.CustomsFinishStatus != 2)
                                {
                                    item.StepStatus = !item.CustomExpectedDate.HasValue ? 4 : (item.CustomExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                            case Constants.ImportProfile_Step_Import:
                                if (item.WarehouseFinishStatus != 2)
                                {
                                    item.StepStatus = !item.WarehouseExpectedDate.HasValue ? 4 : (item.WarehouseExpectedDate.Value - DateTime.Now.Date).Days > 3 ? 1 : 2;
                                }
                                else
                                {
                                    item.StepStatus = 3;
                                }
                                break;
                        }


                    }
                }
            }

            return searchResult;
        }

        public ImportProfileSearchResultModel<ImportProfileKanbanReusltModel> SearchImportProfileKanban(ImportProfileSearchModel searchModel)
        {
            ImportProfileSearchResultModel<ImportProfileKanbanReusltModel> searchResult = new ImportProfileSearchResultModel<ImportProfileKanbanReusltModel>();

            var searchImporProfileResult = SearchImportProfile(searchModel, true);

            searchResult.PayExpiredTotal = searchImporProfileResult.PayExpiredTotal;
            searchResult.PayWarningTotal = searchImporProfileResult.PayWarningTotal;

            // Xác định nhà cung cấp
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_ConfirmSupplier).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_ConfirmSupplier),
                Step = Constants.ImportProfile_Step_ConfirmSupplier,
                IsShow = true
            });

            // Làm hợp đồng
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_Contract).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_Contract),
                Step = Constants.ImportProfile_Step_Contract,
                IsShow = true
            });

            // Thanh toán
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_Payment).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_Payment),
                Step = Constants.ImportProfile_Step_Payment,
                IsShow = true
            });

            // Theo dõi tiến độ
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_Production).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_Production),
                Step = Constants.ImportProfile_Step_Production,
                IsShow = true
            });

            // Lựa chọn vận chuyển
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_Transport).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_Transport),
                Step = Constants.ImportProfile_Step_Transport,
                IsShow = true
            });

            // Thủ tục hải quan
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_Customs).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_Customs),
                Step = Constants.ImportProfile_Step_Customs,
                IsShow = true
            });

            // Nhập kho
            searchResult.ListResult.Add(new ImportProfileKanbanReusltModel
            {
                ImportProfiles = searchImporProfileResult.ListResult.Where(r => r.Step == Constants.ImportProfile_Step_Import).ToList(),
                Name = GetTyName(Constants.ImportProfile_Step_Import),
                Step = Constants.ImportProfile_Step_Transport,
                IsShow = true
            });

            searchResult.TotalItem = searchImporProfileResult.TotalItem;

            return searchResult;
        }

        public ImportCodeModel GetImportProfileCode()
        {
            var dateNow = DateTime.Now;
            string code = string.Empty;
            var maxIndex = db.ImportProfiles.AsNoTracking().Select(r => r.Index).DefaultIfEmpty(0).Max();
            maxIndex++;
            code = $"HSNK.{dateNow.ToString("yyyy")}.{string.Format("{0:00000}", maxIndex)}";

            var documentConfigs = db.ImportProfileDocumentConfigs.AsNoTracking().Where(t => !t.IsRequired).Select(t => new ListFileConfigModel
            {
                Id = t.Id,
                Name = t.Name,
                Step = t.Step
            }).OrderBy(t => t.Step).ToList();

            return new ImportCodeModel
            {
                Code = code,
                Index = maxIndex,
                ListFile = documentConfigs
            };
        }

        public void CreateImportProfile(ImportProfileCreateModel model, string userId)
        {
            List<ImportProfileProduct> listPR = new List<ImportProfileProduct>();
            List<ImportProfileDocument> listDocument = new List<ImportProfileDocument>();
            ImportProfileProduct profileProduct = null;
            ImportProfileDocument profileDocument = null;
            // kiểm tra mã hồ sơ
            var checkImportProfile = db.ImportProfiles.AsNoTracking().FirstOrDefault(t => model.Code.Equals(t.Code));
            if (checkImportProfile != null)
            {
                var importCode = GetImportProfileCode();
                model.Code = importCode.Code;
                model.Index = importCode.Index;
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.AsNoTracking().FirstOrDefault(t => t.Id.Equals(userId));

                    ImportProfile profile = new ImportProfile()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code,
                        Name = model.Name,
                        PRDueDate = model.DueDatePR,
                        ProjectCode = model.ProjectCode,
                        ProjectName = model.ProjectName,
                        ManufacturerCode = model.ManufactureCode,
                        PRCode = model.PRCode,
                        EmployeeId = user.EmployeeId,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                        Step = 1,
                        Index = model.Index,
                        SupplierExpectedDate = model.SupplierExpectedDate,
                        ContractExpectedDate = model.ContractExpectedDate,
                        PayExpectedDate = model.PayExpectedDate,
                        ProductionExpectedDate = model.ProductionExpectedDate,
                        TransportExpectedDate = model.TransportExpectedDate,
                        CustomExpectedDate = model.CustomExpectedDate,
                        WarehouseExpectedDate = model.WarehouseExpectedDate
                    };

                    db.ImportProfiles.Add(profile);

                    var documentConfig = db.ImportProfileDocumentConfigs.AsNoTracking().ToList();
                    if (documentConfig.Count > 0)
                    {
                        foreach (var item in documentConfig)
                        {
                            profileDocument = new ImportProfileDocument()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ImportProfileId = profile.Id,
                                Step = item.Step,
                                Name = item.Name,
                                IsRequired = item.IsRequired,
                                Note = item.Note,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };
                            if (model.ListFileId.Contains(item.Id))
                            {
                                profileDocument.IsRequired = true;
                            }
                            listDocument.Add(profileDocument);
                        }
                        db.ImportProfileDocuments.AddRange(listDocument);
                    }

                    if (model.ListMaterial.Count > 0)
                    {
                        PurchaseRequestProduct purchaseRequestProduct;
                        foreach (var item in model.ListMaterial)
                        {
                            purchaseRequestProduct = db.PurchaseRequestProducts.FirstOrDefault(r => r.Id.Equals(item.Id));

                            var materialInfo = (from a in db.PurchaseRequestProducts.AsNoTracking()
                                                join b in db.ImportProfileProducts.AsNoTracking() on a.Id equals b.PurchaseRequestProductId
                                                join c in db.ImportProfiles.AsNoTracking() on b.ImportProfileId equals c.Id
                                                where a.ManufactureId == item.ManufactureId && a.Code == item.Code
                                                select new
                                                {
                                                    b.HSCode,
                                                    b.ProductionDescription,
                                                    c.CreateDate
                                                }).OrderByDescending(t => t.CreateDate).FirstOrDefault();

                            if (purchaseRequestProduct != null)
                            {
                                if (purchaseRequestProduct.Status)
                                {
                                    throw NTSException.CreateInstance(MessageResourceKey.MSG0079, purchaseRequestProduct.Code);
                                }

                                purchaseRequestProduct.Status = true;
                            }

                            profileProduct = new ImportProfileProduct()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ImportProfileId = profile.Id,
                                PurchaseRequestProductId = item.Id
                            };

                            if (materialInfo != null)
                            {
                                profileProduct.HSCode = materialInfo.HSCode;
                                profileProduct.ProductionDescription = materialInfo.ProductionDescription;
                            }
                            listPR.Add(profileProduct);
                        }
                        db.ImportProfileProducts.AddRange(listPR);
                    }

                    UserLogUtil.LogHistotyAdd(db, userId, profile.Code, profile.Id, Constants.LOG_ImportProfile);

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

        public ImportProfileUpdateModel GetImportProfileById(string id, bool isView)
        {
            ImportProfileUpdateModel model = null;
            if (!isView)
            {
                var dateNow = DateTime.Now;
                model = (from a in db.ImportProfiles.AsNoTracking()
                         where a.Id.Equals(id)
                         select new ImportProfileUpdateModel
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Code = a.Code,
                             Step = a.Step,
                             Status = a.Step == Constants.ImportProfile_Step_Finish ? a.Status : !a.PRDueDate.HasValue || a.PRDueDate >= dateNow ? 1 : 2,
                             Note = a.Note,
                             PRCode = a.PRCode,
                             PRDueDate = a.PRDueDate,
                             ProjectCode = a.ProjectCode,
                             ProjectName = a.ProjectName,
                             ManufacturerCode = a.ManufacturerCode,
                             Amount = a.Amount,
                             ProcessingTime = a.ProcessingTime,
                             QuoteNumber = a.QuoteNumber,
                             QuoteDate = a.QuoteDate,
                             SupplierId = a.SupplierId,
                             IsSupplier = a.IsSupplier,
                             SupplierFinishDate = a.SupplierFinishDate,
                             IsContract = a.IsContract,
                             PONumber = a.PONumber,
                             ContractFinishDate = a.ContractFinishDate,
                             PayStatus = a.PayStatus,
                             PayIndex = a.PayIndex,
                             PayDueDate = a.PayDueDate,
                             PayFinishDate = a.PayFinishDate,
                             EstimatedDeliveryDate = a.EstimatedDeliveryDate,
                             IsWarning = a.IsWarning,
                             WarningDay = a.WarningDay,
                             ProductionFinishDate = a.ProductionFinishDate,
                             TransportSupplierId = a.TransportSupplierId,
                             TransportLeadtime = a.TransportLeadtime,
                             ShippingCost = a.ShippingCost,
                             IsInsurrance = a.IsInsurrance,
                             TransportationCosts = a.TransportationCosts,
                             TransportFinishDate = a.TransportFinishDate,
                             CustomsName = a.CustomsName,
                             CustomsElearanceStatus = a.CustomsElearanceStatus,
                             CustomsElearanceValue = a.CustomsElearanceValue,
                             CustomsType = a.CustomsType,
                             CustomsNote = a.CustomsNote,
                             VAT = a.VAT,
                             HSCode = a.HSCode,
                             ImportPercent = a.ImportPercent,
                             CustomsDeclarationFormCode = a.CustomsDeclarationFormCode,
                             CustomsDeclarationFormDate = a.CustomsDeclarationFormDate,
                             CustomsFinishDate = a.CustomsFinishDate,
                             WarehouseStatus = a.WarehouseStatus,
                             WarehouseCode = a.WarehouseCode,
                             WarehouseDate = a.WarehouseDate,
                             WarehouseFinishDate = a.WarehouseFinishDate,
                             EmployeeId = a.EmployeeId,
                             CreateBy = a.CreateBy,
                             CreateDate = a.CreateDate,
                             UpdateBy = a.UpdateBy,
                             UpdateDate = a.UpdateDate,
                             NetWeight = a.NetWeight,
                             PackageQuantity = a.PackageQuantity,
                             PackingSize = a.PackingSize,
                             TransportationInternationalCosts = a.TransportationInternationalCosts,
                             TransportationInternationalCostsUnit = a.TransportationInternationalCostsUnit,
                             CustomsClearanceFromDate = a.CustomsClearanceFromDate,
                             CustomsInlandCosts = a.CustomsInlandCosts,
                             CustomsSupplierId = a.CustomsSupplierId,
                             CustomsTypeCode = a.CustomsTypeCode,
                             ExportTax = a.ExportTax,
                             ContractExpectedDate = a.ContractExpectedDate,
                             SupplierExpectedDate = a.SupplierExpectedDate,
                             PayExpectedDate = a.PayExpectedDate,
                             ProductionExpectedDate = a.ProductionExpectedDate,
                             ProductionExpectedDate1 = a.ProductionExpectedDate1,
                             ProductionExpectedDate2 = a.ProductionExpectedDate2,
                             TransportExpectedDate = a.TransportExpectedDate,
                             CustomExpectedDate = a.CustomExpectedDate,
                             WarehouseExpectedDate = a.WarehouseExpectedDate,
                             SupplierExchangeRate = a.SupplierExchangeRate,
                             TransportExchangeRate = a.TransportExchangeRate,
                             SupplierFinishStatus = a.SupplierFinishDate.HasValue && a.SupplierExpectedDate.HasValue ? a.SupplierFinishStatus : !a.SupplierExpectedDate.HasValue ? 3 : a.SupplierExpectedDate.Value >= dateNow ? 1 : 2,
                             ContractFinishStatus = a.ContractFinishDate.HasValue && a.ContractExpectedDate.HasValue ? a.ContractFinishStatus : !a.ContractExpectedDate.HasValue ? 3 : a.ContractExpectedDate.Value >= dateNow ? 1 : 2,
                             PayFinishStatus = a.PayFinishDate.HasValue && a.PayExpectedDate.HasValue ? a.PayFinishStatus : !a.PayExpectedDate.HasValue ? 3 : a.PayExpectedDate.Value >= dateNow ? 1 : 2,
                             ProductionFinishStatus = a.ProductionFinishDate.HasValue && a.ProductionExpectedDate.HasValue ? a.ProductionFinishStatus : (!a.ProductionExpectedDate.HasValue ? 3 : !a.ProductionExpectedDate1.HasValue ? (a.ProductionExpectedDate.Value >= dateNow ? 1 : 2) : !a.ProductionExpectedDate2.HasValue ? (a.ProductionExpectedDate1.Value >= dateNow ? 1 : 2) : (a.ProductionExpectedDate2.Value >= dateNow ? 1 : 2)),
                             TransportFinishStatus = a.TransportFinishDate.HasValue && a.TransportExpectedDate.HasValue ? a.TransportFinishStatus : !a.TransportExpectedDate.HasValue ? 3 : a.TransportExpectedDate.Value >= dateNow ? 1 : 2,
                             CustomFinishStatus = a.CustomsFinishDate.HasValue && a.CustomExpectedDate.HasValue ? a.CustomsFinishStatus : !a.CustomExpectedDate.HasValue ? 3 : a.CustomExpectedDate.Value >= dateNow ? 1 : 2,
                             WarehouseFinishStatus = a.WarehouseFinishDate.HasValue && a.WarehouseExpectedDate.HasValue ? a.WarehouseFinishStatus : !a.WarehouseExpectedDate.HasValue ? 3 : a.WarehouseExpectedDate.Value >= dateNow ? 1 : 2,
                             CurrencyUnit = a.CurrencyUnit,
                             OtherCosts = a.OtherCosts,
                             DeliveryConditions = a.DeliveryConditions,
                             ChargingWeight = a.ChargingWeight
                         }).FirstOrDefault();
            }
            else
            {
                model = GetImportProfileViewById(id);
            }

            if (model == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ImportProfile);
            }

            var listPayment = (from t in db.ImportProfilePayments.AsNoTracking()
                               where t.ImportProfileId.Equals(id)
                               orderby t.Index
                               select new ImportProfilePaymentModel
                               {
                                   Id = t.Id,
                                   ImportProfileId = t.ImportProfileId,
                                   Index = t.Index,
                                   PercentPayment = t.PercentPayment,
                                   Money = t.Money,
                                   Duedate = t.Duedate,
                                   MoneyTransferName = t.MoneyTransferName,
                                   MoneyTransferPath = t.MoneyTransferPath,
                                   Note = t.Note,
                                   Status = t.Status,
                                   Content = t.Content,
                                   CurrencyUnit = t.CurrencyUnit
                               }).ToList();

            model.ListPayment = listPayment;

            var listMaterial = (from a in db.ImportProfileProducts.AsNoTracking()
                                join b in db.PurchaseRequestProducts.AsNoTracking() on a.PurchaseRequestProductId equals b.Id
                                join c in db.PurchaseRequests.AsNoTracking() on b.PurchaseRequestId equals c.Id
                                join d in db.Employees.AsNoTracking() on b.SalesBy equals d.Id
                                join m in db.Manufactures.AsNoTracking() on b.ManufactureId equals m.Id
                                where a.ImportProfileId.Equals(id)
                                orderby b.Code
                                select new ImportPRUpdateModel
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                    Code = b.Code,
                                    ManufactureCode = m.Code,
                                    ParentCode = b.ParentCode,
                                    Quantity = b.Quantity,
                                    RequireDate = b.RequireDate,
                                    ProjectCode = b.ProjectCode,
                                    SalesBy = b.SalesBy,
                                    SalesName = d.Name,
                                    PurchaseRequestCode = b.PRCode,
                                    Status = b.Status,
                                    UnitName = b.UnitName,
                                    Price = a.Price,
                                    LeadTime = a.Leadtime,
                                    CurrencyUnit = a.CurrencyUnit,
                                    Amount = a.Amount,
                                    HSCode = a.HSCode,
                                    ImportTax = a.ImportTax,
                                    OtherTax = a.OtherTax,
                                    VATTax = a.VATTax,
                                    ImportTaxValue = a.ImportTaxValue,
                                    OtherTaxValue = a.OtherTaxValue,
                                    VATTaxValue = a.VATTaxValue,
                                    QuotaPrice = b.QuotaPrice,
                                    InlandShippingCost = a.InlandShippingCost,
                                    InternationalShippingCost = a.InternationalShippingCost,
                                    OtherCosts = a.OtherCosts,
                                    RealPrice = a.RealPrice,
                                    ProductionDescription = a.ProductionDescription
                                }).ToList();

            model.ListMaterial = listMaterial;

            model.ListTransportSupplier = (from t in db.ImportProfileTransportSuppliers.AsNoTracking()
                                           where t.ImportProfileId.Equals(id)
                                           join u in db.Users.AsNoTracking() on t.UpdateBy equals u.Id
                                           join s in db.Suppliers.AsNoTracking() on t.TransportSupplierId equals s.Id into ts
                                           from tsn in ts.DefaultIfEmpty()
                                           select new ImportProfileTransportSupplierModel()
                                           {
                                               Id = t.Id,
                                               ImportProfileId = t.ImportProfileId,
                                               Code = t.Code,
                                               Note = t.Note,
                                               Effect = t.Effect,
                                               QuoteDate = t.QuoteDate,
                                               ShippingCost = t.ShippingCost,
                                               TransportSupplierId = t.TransportSupplierId,
                                               TransportLeadtime = t.TransportLeadtime,
                                               FilePath = t.FilePath,
                                               FileName = t.FileName,
                                               FileSize = t.FileSize,
                                               CreateBy = t.CreateBy,
                                               CreateDate = t.CreateDate,
                                               UpdateBy = t.UpdateBy,
                                               UpdateDate = t.UpdateDate,
                                               UploadDate = t.UpdateDate,
                                               UploadName = u.UserName
                                           }).ToList();

            model.ListDocumentStep1 = (from t in db.ImportProfileQuotes.AsNoTracking()
                                       where t.ImportProfileId.Equals(id)
                                       join u in db.Users.AsNoTracking() on t.UpdateBy equals u.Id
                                       join s in db.Suppliers.AsNoTracking() on t.SupplierId equals s.Id into ts
                                       from tsn in ts.DefaultIfEmpty()
                                       select new ImportProfileQuoteModel()
                                       {
                                           Id = t.Id,
                                           ImportProfileId = t.ImportProfileId,
                                           Code = t.Code,
                                           Note = t.Note,
                                           Effect = t.Effect,
                                           QuoteDate = t.QuoteDate,
                                           SupplierId = t.SupplierId,
                                           FilePath = t.FilePath,
                                           FileName = t.FileName,
                                           FileSize = t.FileSize,
                                           CreateBy = t.CreateBy,
                                           CreateDate = t.CreateDate,
                                           UpdateBy = t.UpdateBy,
                                           UpdateDate = t.UpdateDate,
                                           UploadDate = t.UpdateDate,
                                           UploadName = u.UserName,
                                           SupplierCode = tsn != null ? tsn.Code : string.Empty
                                       }).ToList();

            var listDocument = (from t in db.ImportProfileDocuments.AsNoTracking()
                                where t.ImportProfileId.Equals(id)
                                join u in db.Users.AsNoTracking() on t.UpdateBy equals u.Id
                                select new ImportProfileDocumentModel()
                                {
                                    Id = t.Id,
                                    ImportProfileId = t.ImportProfileId,
                                    Step = t.Step,
                                    Name = t.Name,
                                    IsRequired = t.IsRequired,
                                    Note = t.Note,
                                    FilePath = t.FilePath,
                                    FileName = t.FileName,
                                    FileSize = t.FileSize,
                                    CreateBy = t.CreateBy,
                                    CreateDate = t.CreateDate,
                                    UpdateBy = t.UpdateBy,
                                    UpdateDate = t.UpdateDate,
                                    UploadDate = t.UpdateDate,
                                    UploadName = u.UserName,
                                    SupplierName = t.SupplierName
                                }).ToList();

            model.ListDocumentStep2 = listDocument.Where(t => t.Step == Constants.ImportProfile_Step_Contract).ToList();
            model.ListDocumentStep3 = listDocument.Where(t => t.Step == Constants.ImportProfile_Step_Payment).ToList();
            model.ListDocumentStep4 = listDocument.Where(t => t.Step == Constants.ImportProfile_Step_Production).ToList();
            model.ListDocumentStep5 = listDocument.Where(t => t.Step == Constants.ImportProfile_Step_Transport).ToList();
            model.ListDocumentStep6 = listDocument.Where(t => t.Step == Constants.ImportProfile_Step_Customs).ToList();
            model.ListDocumentStep7 = listDocument.Where(t => t.Step == Constants.ImportProfile_Step_Import).ToList();

            var listDocumentOther = (from t in db.ImportProfileDocumentOthers.AsNoTracking()
                                     where t.ImportProfileId.Equals(id)
                                     join u in db.Users.AsNoTracking() on t.UpdateBy equals u.Id
                                     select new ImportProfileDocumentOtherModel()
                                     {
                                         Id = t.Id,
                                         ImportProfileId = t.ImportProfileId,
                                         Step = t.Step,
                                         Note = t.Note,
                                         FilePath = t.FilePath,
                                         FileName = t.FileName,
                                         FileSize = t.FileSize,
                                         CreateBy = t.CreateBy,
                                         CreateDate = t.CreateDate,
                                         UpdateBy = t.UpdateBy,
                                         UpdateDate = t.UpdateDate,
                                         UploadDate = t.UpdateDate,
                                         UploadName = u.UserName
                                     }).ToList();

            model.ListDocumentOtherStep1 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_ConfirmSupplier).ToList();
            model.ListDocumentOtherStep2 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_Contract).ToList();
            model.ListDocumentOtherStep3 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_Payment).ToList();
            model.ListDocumentOtherStep4 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_Production).ToList();
            model.ListDocumentOtherStep5 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_Transport).ToList();
            model.ListDocumentOtherStep6 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_Customs).ToList();
            model.ListDocumentOtherStep7 = listDocumentOther.Where(t => t.Step == Constants.ImportProfile_Step_Import).ToList();

            model.ListProblem = (from a in db.ImportProfileProblemExists.AsNoTracking()
                                 where a.ImportProfileId.Equals(id)
                                 join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                 join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                 orderby a.CreateDate descending
                                 select new ImportProfileProblemExistCreateModel
                                 {
                                     Id = a.Id,
                                     ImportProfileId = a.ImportProfileId,
                                     Note = a.Note,
                                     Plan = a.Plan,
                                     Step = a.Step,
                                     Status = a.Status,
                                     CreateBy = c.Name,
                                     CreateDate = a.CreateDate.Value
                                 }).ToList();

            return model;
        }

        private ImportProfileUpdateModel GetImportProfileViewById(string profileId)
        {
            var dateNow = DateTime.Now;
            var profileModel = (from a in db.ImportProfiles.AsNoTracking()
                                join b in db.Suppliers.AsNoTracking() on a.SupplierId equals b.Id into ab
                                from abx in ab.DefaultIfEmpty()
                                join t in db.Suppliers.AsNoTracking() on a.TransportSupplierId equals t.Id into at
                                from atn in at.DefaultIfEmpty()
                                join c in db.Suppliers.AsNoTracking() on a.CustomsSupplierId equals c.Id into ac
                                from acn in ac.DefaultIfEmpty()
                                where a.Id.Equals(profileId)
                                select new ImportProfileUpdateModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Code = a.Code,
                                    Step = a.Step,
                                    Status = a.Step == Constants.ImportProfile_Step_Finish ? a.Status : !a.PRDueDate.HasValue || a.PRDueDate >= dateNow ? 1 : 2,
                                    Note = a.Note,
                                    PRCode = a.PRCode,
                                    PRDueDate = a.PRDueDate,
                                    ProjectCode = a.ProjectCode,
                                    ProjectName = a.ProjectName,
                                    ManufacturerCode = a.ManufacturerCode,
                                    Amount = a.Amount,
                                    ProcessingTime = a.ProcessingTime,
                                    QuoteNumber = a.QuoteNumber,
                                    QuoteDate = a.QuoteDate,
                                    SupplierId = a.SupplierId,
                                    SupplierName = abx.Name,
                                    IsSupplier = a.IsSupplier,
                                    SupplierFinishDate = a.SupplierFinishDate,
                                    IsContract = a.IsContract,
                                    PONumber = a.PONumber,
                                    ContractFinishDate = a.ContractFinishDate,
                                    PayStatus = a.PayStatus,
                                    PayIndex = a.PayIndex,
                                    PayDueDate = a.PayDueDate,
                                    PayFinishDate = a.PayFinishDate,
                                    EstimatedDeliveryDate = a.EstimatedDeliveryDate,
                                    IsWarning = a.IsWarning,
                                    WarningDay = a.WarningDay,
                                    ProductionFinishDate = a.ProductionFinishDate,
                                    TransportSupplierId = a.TransportSupplierId,
                                    TransportLeadtime = a.TransportLeadtime,
                                    ShippingCost = a.ShippingCost,
                                    IsInsurrance = a.IsInsurrance,
                                    TransportationCosts = a.TransportationCosts,
                                    TransportFinishDate = a.TransportFinishDate,
                                    CustomsName = a.CustomsName,
                                    CustomsElearanceStatus = a.CustomsElearanceStatus,
                                    CustomsElearanceValue = a.CustomsElearanceValue,
                                    CustomsType = a.CustomsType,
                                    CustomsNote = a.CustomsNote,
                                    VAT = a.VAT,
                                    HSCode = a.HSCode,
                                    ImportPercent = a.ImportPercent,
                                    CustomsDeclarationFormCode = a.CustomsDeclarationFormCode,
                                    CustomsDeclarationFormDate = a.CustomsDeclarationFormDate,
                                    CustomsFinishDate = a.CustomsFinishDate,
                                    WarehouseStatus = a.WarehouseStatus,
                                    WarehouseCode = a.WarehouseCode,
                                    WarehouseDate = a.WarehouseDate,
                                    WarehouseFinishDate = a.WarehouseFinishDate,
                                    EmployeeId = a.EmployeeId,
                                    CreateBy = a.CreateBy,
                                    CreateDate = a.CreateDate,
                                    UpdateBy = a.UpdateBy,
                                    UpdateDate = a.UpdateDate,
                                    NetWeight = a.NetWeight,
                                    PackageQuantity = a.PackageQuantity,
                                    PackingSize = a.PackingSize,
                                    TransportationInternationalCosts = a.TransportationInternationalCosts,
                                    TransportationInternationalCostsUnit = a.TransportationInternationalCostsUnit,
                                    CustomsClearanceFromDate = a.CustomsClearanceFromDate,
                                    CustomsInlandCosts = a.CustomsInlandCosts,
                                    CustomsSupplierId = a.CustomsSupplierId,
                                    CustomsTypeCode = a.CustomsTypeCode,
                                    ExportTax = a.ExportTax,
                                    TransportSupplierName = atn != null ? atn.Name : string.Empty,
                                    CustomsSupplierName = acn != null ? acn.Name : string.Empty,
                                    ContractExpectedDate = a.ContractExpectedDate,
                                    SupplierExpectedDate = a.SupplierExpectedDate,
                                    PayExpectedDate = a.PayExpectedDate,
                                    ProductionExpectedDate = a.ProductionExpectedDate,
                                    ProductionExpectedDate1 = a.ProductionExpectedDate1,
                                    ProductionExpectedDate2 = a.ProductionExpectedDate2,
                                    TransportExpectedDate = a.TransportExpectedDate,
                                    CustomExpectedDate = a.CustomExpectedDate,
                                    WarehouseExpectedDate = a.WarehouseExpectedDate,
                                    SupplierExchangeRate = a.SupplierExchangeRate,
                                    TransportExchangeRate = a.TransportExchangeRate,
                                    SupplierFinishStatus = a.SupplierFinishDate.HasValue && a.SupplierExpectedDate.HasValue ? a.SupplierFinishStatus : !a.SupplierExpectedDate.HasValue || a.SupplierExpectedDate.Value >= dateNow ? 1 : 2,
                                    ContractFinishStatus = a.ContractFinishDate.HasValue && a.ContractExpectedDate.HasValue ? a.ContractFinishStatus : !a.ContractExpectedDate.HasValue || a.ContractExpectedDate.Value >= dateNow ? 1 : 2,
                                    PayFinishStatus = a.PayFinishDate.HasValue && a.PayExpectedDate.HasValue ? a.PayFinishStatus : !a.PayExpectedDate.HasValue || a.PayExpectedDate.Value >= dateNow ? 1 : 2,
                                    ProductionFinishStatus = a.ProductionFinishDate.HasValue && a.ProductionExpectedDate.HasValue ? a.ProductionFinishStatus : !a.ProductionExpectedDate.HasValue ? 1 : (!a.ProductionExpectedDate1.HasValue ? (a.ProductionExpectedDate.Value >= dateNow ? 1 : 2) : (!a.ProductionExpectedDate2.HasValue ? (a.ProductionExpectedDate1.Value >= dateNow ? 1 : 2) : (a.ProductionExpectedDate2.Value >= dateNow ? 1 : 2))),
                                    TransportFinishStatus = a.TransportFinishDate.HasValue && a.TransportExpectedDate.HasValue ? a.TransportFinishStatus : !a.TransportExpectedDate.HasValue || a.TransportExpectedDate.Value >= dateNow ? 1 : 2,
                                    CustomFinishStatus = a.CustomsFinishDate.HasValue && a.CustomExpectedDate.HasValue ? a.CustomsFinishStatus : !a.CustomExpectedDate.HasValue || a.CustomExpectedDate.Value >= dateNow ? 1 : 2,
                                    WarehouseFinishStatus = a.WarehouseFinishDate.HasValue && a.WarehouseExpectedDate.HasValue ? a.WarehouseFinishStatus : !a.WarehouseExpectedDate.HasValue || a.WarehouseExpectedDate.Value >= dateNow ? 1 : 2,
                                    CurrencyUnit = a.CurrencyUnit,
                                    OtherCosts = a.OtherCosts,
                                    DeliveryConditions = a.DeliveryConditions,
                                    ChargingWeight = a.ChargingWeight

                                }).FirstOrDefault();

            return profileModel;
        }

        public void UpdateImportProfile(ImportProfileUpdateModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var profile = db.ImportProfiles.FirstOrDefault(t => model.Id.Equals(t.Id));
                    if (profile == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ImportProfile);
                    }

                    SetInfo(profile, model, userId);

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_ImportProfile, model.Id, model.Code, "Cập nhật");

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

        public void DeleteImportProfile(ImportProfileReusltModel importProfileModel, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var importProfile = db.ImportProfiles.FirstOrDefault(r => r.Id.Equals(importProfileModel.Id));
                    if (importProfile == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ImportProfile);
                    }

                    if (importProfile.Step == Constants.ImportProfile_Step_Finish)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0077, TextResourceKey.ImportProfile);
                    }

                    var importDocumentOther = db.ImportProfileDocumentOthers.Where(r => r.ImportProfileId.Equals(importProfileModel.Id));
                    db.ImportProfileDocumentOthers.RemoveRange(importDocumentOther);

                    var importDocuments = db.ImportProfileDocuments.Where(r => r.ImportProfileId.Equals(importProfileModel.Id));
                    db.ImportProfileDocuments.RemoveRange(importDocuments);

                    var importProducts = db.ImportProfileProducts.Where(r => r.ImportProfileId.Equals(importProfileModel.Id));

                    var importTransportSuppliers = db.ImportProfileTransportSuppliers.Where(r => r.ImportProfileId.Equals(importProfileModel.Id));
                    db.ImportProfileTransportSuppliers.RemoveRange(importTransportSuppliers);

                    PurchaseRequestProduct purchaseRequestProduct;
                    foreach (var item in importProducts)
                    {
                        purchaseRequestProduct = db.PurchaseRequestProducts.FirstOrDefault(r => r.Id.Equals(item.PurchaseRequestProductId));
                        if (purchaseRequestProduct != null)
                        {
                            purchaseRequestProduct.Status = false;
                        }
                    }

                    db.ImportProfileProducts.RemoveRange(importProducts);

                    var importProfilePayments = db.ImportProfilePayments.Where(r => r.ImportProfileId.Equals(importProfileModel.Id));
                    db.ImportProfilePayments.RemoveRange(importProfilePayments);

                    var importProfileQuotes = db.ImportProfileQuotes.Where(r => r.ImportProfileId.Equals(importProfileModel.Id));
                    db.ImportProfileQuotes.RemoveRange(importProfileQuotes);

                    db.ImportProfiles.Remove(importProfile);

                    UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_ImportProfile, importProfileModel.Id, importProfile.Code, importProfile);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(importProfileModel, ex);
                }
            }
        }

        public void BackStep(string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var step = 0;
                    var importProfile = db.ImportProfiles.FirstOrDefault(r => r.Id.Equals(id));
                    if (importProfile == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ImportProfile);
                    }

                    step = importProfile.Step;
                    importProfile.Step = importProfile.Step - 1;
                    importProfile.UpdateBy = userId;
                    importProfile.UpdateDate = DateTime.Now;

                    if (step == Constants.ImportProfile_Step_Finish)
                    {
                        importProfile.Status = Constants.ImportProfile_Status_None;
                    }

                    UpdateFinishDateByStep(importProfile, null, importProfile.Step);
                    UpdateFinishStatusByStep(importProfile, null, importProfile.Step);

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_ImportProfile, importProfile.Id, importProfile.Code, $"Quay lại quy trình: {GetTyName(step)}");

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

        public void NextStep(ImportProfileUpdateModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var step = 0;
                    var importProfile = db.ImportProfiles.FirstOrDefault(r => r.Id.Equals(model.Id));
                    if (importProfile == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ImportProfile);
                    }

                    SetInfo(importProfile, model, userId);

                    step = importProfile.Step;
                    importProfile.Step = importProfile.Step + 1;
                    importProfile.UpdateBy = userId;
                    importProfile.UpdateDate = DateTime.Now;

                    if (importProfile.Step == Constants.ImportProfile_Step_Finish)
                    {
                        if (importProfile.PRDueDate.HasValue)
                        {
                            if (importProfile.PRDueDate.Value >= DateTime.Now.Date)
                            {
                                importProfile.Status = Constants.ImportProfile_Status_Ongoing;
                            }
                            else
                            {
                                importProfile.Status = Constants.ImportProfile_Status_Slow;
                            }
                        }
                    }

                    UpdateFinishDateByStep(importProfile, DateTime.Now, step);
                    UpdateFinishStatusByStep(importProfile, DateTime.Now, step);

                    UserLogUtil.LogHistotyUpdateOther(db, userId, Constants.LOG_ImportProfile, importProfile.Id, importProfile.Code, $"Kết thúc quy trình: {GetTyName(step)}");

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

        public List<ListFileModel> GetListFile(ExportListFileModel model)
        {
            List<ListFileModel> list = new List<ListFileModel>();
            var profile = db.ImportProfiles.AsNoTracking().FirstOrDefault(t => t.Id.Equals(model.Id));
            if (profile == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ImportProfile);
            }

            var listFileConfig = db.ImportProfileDocuments.AsNoTracking().Where(t => t.ImportProfileId.Equals(model.Id) && !string.IsNullOrEmpty(t.FilePath)).Select(t => new ListFileModel()
            {
                Id = t.Id,
                FileName = t.Name,
                Path = t.FilePath,
                Step = t.Step
            }).OrderBy(t => t.Step).ToList();

            var quotation = db.ImportProfileQuotes.AsNoTracking().Where(t => t.ImportProfileId.Equals(model.Id) && t.Code.Equals(profile.QuoteNumber)).Select(t => new ListFileModel()
            {
                Id = t.Id,
                FileName = t.FileName,
                Path = t.FilePath,
                Step = Constants.ImportProfile_Step_ConfirmSupplier
            }).FirstOrDefault();

            var extension = string.Empty;
            if (model.Step == 0)
            {
                int count = 1;
                if (quotation != null)
                {
                    extension = Path.GetExtension(quotation.Path);
                    quotation.FileName = count + "-Quotation-" + profile.Code + extension;
                    list.Add(quotation);
                    count++;
                }

                if (listFileConfig.Count > 0)
                {
                    foreach (var item in listFileConfig)
                    {
                        extension = Path.GetExtension(item.Path);
                        item.FileName = count + "-" + item.FileName + "-" + profile.Code + extension;
                        count++;
                    }
                    list.AddRange(listFileConfig);
                }
            }
            else
            {
                listFileConfig = listFileConfig.Where(t => t.Step == model.Step).ToList();

                if (model.Step == Constants.ImportProfile_Step_ConfirmSupplier)
                {
                    int count = 1;
                    if (quotation != null)
                    {
                        extension = Path.GetExtension(quotation.Path);
                        quotation.FileName = count + "-Quotation-" + profile.Code + extension;
                        list.Add(quotation);
                        count++;
                    }
                }
                else
                {
                    int count = 1;
                    if (listFileConfig.Count > 0)
                    {
                        foreach (var item in listFileConfig)
                        {
                            extension = Path.GetExtension(item.Path);
                            item.FileName = count + "-" + item.FileName + "-" + profile.Code + extension;
                            count++;
                        }
                        list.AddRange(listFileConfig);
                    }
                }
            }
            return list;
        }

        private void UpdateFinishDateByStep(ImportProfile importProfile, DateTime? date, int step)
        {
            switch (step)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    importProfile.SupplierFinishDate = date;
                    return;
                case Constants.ImportProfile_Step_Contract:
                    importProfile.ContractFinishDate = date;
                    return;
                case Constants.ImportProfile_Step_Customs:
                    importProfile.CustomsFinishDate = date;
                    return;
                case Constants.ImportProfile_Step_Import:
                    importProfile.WarehouseFinishDate = date;
                    return;
                case Constants.ImportProfile_Step_Payment:
                    importProfile.PayFinishDate = date;
                    return;
                case Constants.ImportProfile_Step_Production:
                    importProfile.ProductionFinishDate = date;
                    return;
                case Constants.ImportProfile_Step_Transport:
                    importProfile.TransportFinishDate = date;
                    return;
                default:
                    return;
            }
        }

        private void UpdateFinishStatusByStep(ImportProfile importProfile, DateTime? date, int step)
        {
            switch (step)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    if (date.HasValue)
                    {
                        if (!importProfile.SupplierExpectedDate.HasValue || date.Value.Date <= importProfile.SupplierExpectedDate.Value.Date)
                        {
                            importProfile.SupplierFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.SupplierFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.SupplierFinishStatus = 0;
                    }
                    return;
                case Constants.ImportProfile_Step_Contract:
                    if (date.HasValue)
                    {
                        if (!importProfile.ContractExpectedDate.HasValue || date.Value.Date <= importProfile.ContractExpectedDate.Value.Date)
                        {
                            importProfile.ContractFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.ContractFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.ContractFinishStatus = 0;
                    }
                    return;
                case Constants.ImportProfile_Step_Customs:
                    if (date.HasValue)
                    {
                        if (!importProfile.CustomExpectedDate.HasValue || date.Value.Date <= importProfile.CustomExpectedDate.Value.Date)
                        {
                            importProfile.CustomsFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.CustomsFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.CustomsFinishStatus = 0;
                    }
                    return;
                case Constants.ImportProfile_Step_Import:
                    if (date.HasValue)
                    {
                        if (!importProfile.WarehouseExpectedDate.HasValue || date.Value.Date <= importProfile.WarehouseExpectedDate.Value.Date)
                        {
                            importProfile.WarehouseFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.WarehouseFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.WarehouseFinishStatus = 0;
                    }
                    return;
                case Constants.ImportProfile_Step_Payment:
                    if (date.HasValue)
                    {
                        if (!importProfile.PayExpectedDate.HasValue || date.Value.Date <= importProfile.PayExpectedDate.Value.Date)
                        {
                            importProfile.PayFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.PayFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.PayFinishStatus = 0;
                    }
                    return;
                case Constants.ImportProfile_Step_Production:
                    if (date.HasValue)
                    {
                        if (!importProfile.ProductionExpectedDate.HasValue || date.Value.Date <= importProfile.ProductionExpectedDate.Value.Date)
                        {
                            importProfile.ProductionFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.ProductionFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.ProductionFinishStatus = 0;
                    }
                    return;
                case Constants.ImportProfile_Step_Transport:
                    if (date.HasValue)
                    {
                        if (!importProfile.TransportExpectedDate.HasValue || date.Value.Date <= importProfile.TransportExpectedDate.Value.Date)
                        {
                            importProfile.TransportFinishStatus = 1;
                        }
                        else
                        {
                            importProfile.TransportFinishStatus = 2;
                        }
                    }
                    else
                    {
                        importProfile.TransportFinishStatus = 0;
                    }
                    return;
                default:
                    return;
            }
        }

        private void UpdateCurrentDateByStep(ImportProfile importProfile, int step)
        {
            switch (step)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    importProfile.CurrentExpected = importProfile.SupplierExpectedDate;
                    return;
                case Constants.ImportProfile_Step_Contract:
                    importProfile.CurrentExpected = importProfile.ContractExpectedDate;
                    return;
                case Constants.ImportProfile_Step_Customs:
                    importProfile.CurrentExpected = importProfile.CustomExpectedDate;
                    return;
                case Constants.ImportProfile_Step_Import:
                    importProfile.CurrentExpected = importProfile.WarehouseExpectedDate;
                    return;
                case Constants.ImportProfile_Step_Payment:
                    importProfile.CurrentExpected = importProfile.PayExpectedDate;
                    return;
                case Constants.ImportProfile_Step_Production:
                    importProfile.CurrentExpected = importProfile.ProductionExpectedDate;
                    return;
                case Constants.ImportProfile_Step_Transport:
                    importProfile.CurrentExpected = importProfile.TransportExpectedDate;
                    return;
                default:
                    return;
            }
        }

        private string GetTyName(int type)
        {
            switch (type)
            {
                case Constants.ImportProfile_Step_ConfirmSupplier:
                    return "Xác định nhà cung cấp";
                case Constants.ImportProfile_Step_Contract:
                    return "Làm hợp đồng";
                case Constants.ImportProfile_Step_Customs:
                    return "Thủ tục thông quan";
                case Constants.ImportProfile_Step_Import:
                    return "Nhập kho";
                case Constants.ImportProfile_Step_Payment:
                    return "Thanh toán";
                case Constants.ImportProfile_Step_Production:
                    return "Theo dõi tiến độ sản xuất";
                case Constants.ImportProfile_Step_Transport:
                    return "Lựa chọn nhà vận chuyển";
                default:
                    return string.Empty;
            }
        }

        private void SetInfo(ImportProfile profile, ImportProfileUpdateModel model, string userId)
        {
            profile.Id = model.Id;
            profile.Name = model.Name;
            profile.Amount = model.Amount;
            profile.Code = model.Code;
            profile.Step = model.Step;
            profile.Status = model.Status;
            profile.Note = model.Note;
            profile.PRCode = model.PRCode;
            profile.PRDueDate = model.PRDueDate;
            profile.ProjectCode = model.ProjectCode;
            profile.ProjectName = model.ProjectName;
            profile.ManufacturerCode = model.ManufacturerCode;
            profile.ProcessingTime = model.ProcessingTime;
            profile.QuoteNumber = model.QuoteNumber;
            profile.QuoteDate = model.QuoteDate;
            profile.SupplierId = model.SupplierId;
            profile.IsSupplier = model.IsSupplier;
            profile.SupplierFinishDate = model.SupplierFinishDate;
            profile.IsContract = model.IsContract;
            profile.PONumber = model.PONumber;
            profile.ContractFinishDate = model.ContractFinishDate;
            profile.PayStatus = model.PayStatus;
            profile.PayIndex = model.PayIndex;
            profile.PayDueDate = model.PayDueDate;
            profile.PayFinishDate = model.PayFinishDate;
            profile.EstimatedDeliveryDate = model.EstimatedDeliveryDate;
            profile.IsWarning = model.IsWarning;
            profile.WarningDay = model.WarningDay;
            profile.ProductionFinishDate = model.ProductionFinishDate;
            profile.TransportSupplierId = model.TransportSupplierId;
            profile.TransportLeadtime = model.TransportLeadtime;
            profile.ShippingCost = model.ShippingCost;
            profile.IsInsurrance = model.IsInsurrance;
            profile.TransportationCosts = model.TransportationCosts;
            profile.TransportFinishDate = model.TransportFinishDate;
            profile.CustomsName = model.CustomsName;
            profile.CustomsElearanceStatus = model.CustomsElearanceStatus;
            profile.CustomsElearanceValue = model.CustomsElearanceValue;
            profile.CustomsType = model.CustomsType;
            profile.CustomsNote = model.CustomsNote;
            profile.VAT = model.VAT;
            profile.HSCode = model.HSCode;
            profile.ImportPercent = model.ImportPercent;
            profile.CustomsDeclarationFormCode = model.CustomsDeclarationFormCode;
            profile.CustomsDeclarationFormDate = model.CustomsDeclarationFormDate;
            profile.CustomsFinishDate = model.CustomsFinishDate;
            profile.WarehouseStatus = model.WarehouseStatus;
            profile.WarehouseCode = model.WarehouseCode;
            profile.WarehouseDate = model.WarehouseDate;
            profile.WarehouseFinishDate = model.WarehouseFinishDate;
            profile.EmployeeId = model.EmployeeId;
            profile.UpdateBy = userId;
            profile.UpdateDate = DateTime.Now;
            profile.TransportationInternationalCosts = model.TransportationInternationalCosts;
            profile.TransportationInternationalCostsUnit = model.TransportationInternationalCostsUnit;
            profile.PackingSize = model.PackingSize;
            profile.PackageQuantity = model.PackageQuantity;
            profile.NetWeight = model.NetWeight;
            profile.CustomsClearanceFromDate = model.CustomsClearanceFromDate;
            profile.CustomsInlandCosts = model.CustomsInlandCosts;
            profile.CustomsSupplierId = model.CustomsSupplierId;
            profile.CustomsTypeCode = model.CustomsTypeCode;
            profile.ExportTax = model.ExportTax;
            profile.SupplierExpectedDate = model.SupplierExpectedDate;
            profile.ContractExpectedDate = model.ContractExpectedDate;
            profile.PayExpectedDate = model.PayExpectedDate;
            profile.ProductionExpectedDate = model.ProductionExpectedDate;
            profile.ProductionExpectedDate1 = model.ProductionExpectedDate1;
            profile.ProductionExpectedDate2 = model.ProductionExpectedDate2;
            profile.TransportExpectedDate = model.TransportExpectedDate;
            profile.CustomExpectedDate = model.CustomExpectedDate;
            profile.WarehouseExpectedDate = model.WarehouseExpectedDate;
            profile.SupplierExchangeRate = model.SupplierExchangeRate;
            profile.TransportExchangeRate = model.TransportExchangeRate;
            profile.OtherCosts = model.OtherCosts;
            profile.DeliveryConditions = model.DeliveryConditions;
            profile.CurrencyUnit = model.CurrencyUnit;
            profile.ChargingWeight = model.ChargingWeight;

            List<ImportProfileDocument> lstDocument = new List<ImportProfileDocument>();
            List<ImportProfileQuote> lstQuote = new List<ImportProfileQuote>();
            List<ImportProfileTransportSupplier> lstTransportSupplier = new List<ImportProfileTransportSupplier>();

            var listDocumentTransportSupplier = db.ImportProfileTransportSuppliers.Where(t => t.ImportProfileId.Equals(model.Id)).ToList();


            ImportProfileTransportSupplierModel importProfileTransportSupplierModel;
            foreach (var item in listDocumentTransportSupplier)
            {
                importProfileTransportSupplierModel = model.ListTransportSupplier.FirstOrDefault(r => item.Id.Equals(r.Id));

                if (importProfileTransportSupplierModel != null)
                {
                    item.Code = importProfileTransportSupplierModel.Code;
                    item.Effect = importProfileTransportSupplierModel.Effect;
                    item.QuoteDate = importProfileTransportSupplierModel.QuoteDate;
                    item.TransportSupplierId = importProfileTransportSupplierModel.TransportSupplierId;
                    item.TransportLeadtime = importProfileTransportSupplierModel.TransportLeadtime;
                    item.ShippingCost = importProfileTransportSupplierModel.ShippingCost;
                    item.FileName = importProfileTransportSupplierModel.FileName;
                    item.FileSize = importProfileTransportSupplierModel.FileSize;
                    item.FilePath = importProfileTransportSupplierModel.FilePath;
                    item.Note = importProfileTransportSupplierModel.Note;
                    item.UpdateBy = userId;
                    item.UpdateDate = DateTime.Now;
                }
                else
                {
                    db.ImportProfileTransportSuppliers.Remove(item);
                }
            }

            ImportProfileTransportSupplier documentTransportSupplier;
            foreach (var item in model.ListTransportSupplier)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    documentTransportSupplier = new ImportProfileTransportSupplier()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImportProfileId = model.Id,
                        Code = item.Code,
                        Effect = item.Effect,
                        QuoteDate = item.QuoteDate,
                        TransportSupplierId = item.TransportSupplierId,
                        TransportLeadtime = item.TransportLeadtime,
                        ShippingCost = item.ShippingCost,
                        FilePath = item.FilePath,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        Note = item.Note,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    lstTransportSupplier.Add(documentTransportSupplier);
                }
            }

            db.ImportProfileTransportSuppliers.AddRange(lstTransportSupplier);

            var listDocumentQuote = db.ImportProfileQuotes.Where(t => t.ImportProfileId.Equals(model.Id)).ToList();
            ImportProfileQuoteModel importProfileQuoteModel;
            foreach (var item in listDocumentQuote)
            {
                importProfileQuoteModel = model.ListDocumentStep1.FirstOrDefault(r => item.Id.Equals(r.Id));

                if (importProfileQuoteModel != null)
                {
                    item.Code = importProfileQuoteModel.Code;
                    item.Effect = importProfileQuoteModel.Effect;
                    item.QuoteDate = importProfileQuoteModel.QuoteDate;
                    item.SupplierId = importProfileQuoteModel.SupplierId;
                    item.FileName = importProfileQuoteModel.FileName;
                    item.FileSize = importProfileQuoteModel.FileSize;
                    item.FilePath = importProfileQuoteModel.FilePath;
                    item.Note = importProfileQuoteModel.Note;
                    item.UpdateBy = userId;
                    item.UpdateDate = DateTime.Now;
                }
                else
                {
                    db.ImportProfileQuotes.Remove(item);
                }
            }

            ImportProfileQuote documentQuote;
            foreach (var item in model.ListDocumentStep1)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    documentQuote = new ImportProfileQuote()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImportProfileId = model.Id,
                        Code = item.Code,
                        Note = item.Note,
                        Effect = item.Effect,
                        QuoteDate = item.QuoteDate,
                        SupplierId = item.SupplierId,
                        FilePath = item.FilePath,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    lstQuote.Add(documentQuote);
                }
            }

            db.ImportProfileQuotes.AddRange(lstQuote);


            UpdateDocument(model.ListDocumentStep2, Constants.ImportProfile_Step_Contract, model.Id, userId, lstDocument);
            UpdateDocument(model.ListDocumentStep3, Constants.ImportProfile_Step_Payment, model.Id, userId, lstDocument);
            UpdateDocument(model.ListDocumentStep4, Constants.ImportProfile_Step_Production, model.Id, userId, lstDocument);
            UpdateDocument(model.ListDocumentStep5, Constants.ImportProfile_Step_Transport, model.Id, userId, lstDocument);
            UpdateDocument(model.ListDocumentStep6, Constants.ImportProfile_Step_Customs, model.Id, userId, lstDocument);
            UpdateDocument(model.ListDocumentStep7, Constants.ImportProfile_Step_Import, model.Id, userId, lstDocument);

            if (lstDocument.Count > 0)
            {
                db.ImportProfileDocuments.AddRange(lstDocument);
            }

            //Tài liệu khác
            //var listDocumentOther = db.ImportProfileDocumentOthers.Where(t => t.ImportProfileId.Equals(model.Id)).ToList();
            //if (listDocumentOther.Count > 0)
            //{
            //    db.ImportProfileDocumentOthers.RemoveRange(listDocumentOther);
            //}

            List<ImportProfileDocumentOther> lstDocumentOther = new List<ImportProfileDocumentOther>();

            UpdateDocumentOther(model.ListDocumentOtherStep1, Constants.ImportProfile_Step_ConfirmSupplier, model.Id, userId, lstDocumentOther);
            UpdateDocumentOther(model.ListDocumentOtherStep2, Constants.ImportProfile_Step_Contract, model.Id, userId, lstDocumentOther);
            UpdateDocumentOther(model.ListDocumentOtherStep3, Constants.ImportProfile_Step_Payment, model.Id, userId, lstDocumentOther);
            UpdateDocumentOther(model.ListDocumentOtherStep4, Constants.ImportProfile_Step_Production, model.Id, userId, lstDocumentOther);
            UpdateDocumentOther(model.ListDocumentOtherStep5, Constants.ImportProfile_Step_Transport, model.Id, userId, lstDocumentOther);
            UpdateDocumentOther(model.ListDocumentOtherStep6, Constants.ImportProfile_Step_Customs, model.Id, userId, lstDocumentOther);
            UpdateDocumentOther(model.ListDocumentOtherStep7, Constants.ImportProfile_Step_Import, model.Id, userId, lstDocumentOther);

            db.ImportProfileDocumentOthers.AddRange(lstDocumentOther);

            if (model.ListPayment.Count > 0)
            {
                List<ImportProfilePayment> listPayment = new List<ImportProfilePayment>();
                var listPay = db.ImportProfilePayments.Where(t => t.ImportProfileId.Equals(model.Id)).ToList();
                if (listPay.Count > 0)
                {
                    db.ImportProfilePayments.RemoveRange(listPay);
                }

                ImportProfilePayment pay;
                int index = 1;
                bool isUnPay = false;
                foreach (var item in model.ListPayment)
                {
                    pay = new ImportProfilePayment()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImportProfileId = model.Id,
                        PercentPayment = item.PercentPayment,
                        Money = item.Money,
                        Content = item.Content,
                        Index = index,
                        Duedate = item.Duedate,
                        Note = item.Note,
                        Status = item.Status,
                        MoneyTransferName = item.MoneyTransferName,
                        MoneyTransferPath = item.MoneyTransferPath,
                        CurrencyUnit = item.CurrencyUnit
                    };
                    listPayment.Add(pay);

                    if (item.Status == Constants.ImportProfilePayment_Status_UnPay && !isUnPay)
                    {
                        isUnPay = true;
                        profile.PayIndex = index;
                        profile.PayDueDate = pay.Duedate;
                        profile.PayStatus = Constants.ImportProfilePayment_Status_UnPay;
                    }

                    index++;
                }

                db.ImportProfilePayments.AddRange(listPayment);

                if (!isUnPay)
                {
                    profile.PayStatus = Constants.ImportProfilePayment_Status_Pay;
                }
            }

            decimal total = 0;
            if (model.ListMaterial.Count > 0)
            {
                List<ImportProfileProduct> materials = new List<ImportProfileProduct>();
                var listMaterial = db.ImportProfileProducts.Where(t => t.ImportProfileId.Equals(model.Id)).ToList();
                if (listMaterial.Count > 0)
                {
                    foreach (var item in listMaterial)
                    {
                        var prProduct = db.PurchaseRequestProducts.FirstOrDefault(t => t.Id.Equals(item.PurchaseRequestProductId));
                        if (prProduct != null)
                        {
                            prProduct.Status = false;
                        }
                    }
                    db.ImportProfileProducts.RemoveRange(listMaterial);
                }
                foreach (var item in model.ListMaterial)
                {
                    var purchaseRequestProduct = db.PurchaseRequestProducts.FirstOrDefault(r => r.Id.Equals(item.Id));
                    if (purchaseRequestProduct != null)
                    {
                        if (purchaseRequestProduct.Status)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0079, purchaseRequestProduct.Code);
                        }

                        purchaseRequestProduct.Status = true;
                    }

                    ImportProfileProduct profileProduct = new ImportProfileProduct()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImportProfileId = model.Id,
                        PurchaseRequestProductId = item.Id,
                        Price = item.Price,
                        Leadtime = item.LeadTime,
                        CurrencyUnit = item.CurrencyUnit,
                        VATTax = item.VATTax,
                        Amount = item.Amount,
                        HSCode = item.HSCode,
                        OtherTax = item.OtherTax,
                        ImportTax = item.ImportTax,
                        ImportTaxValue = item.ImportTaxValue,
                        VATTaxValue = item.VATTaxValue,
                        OtherTaxValue = item.OtherTaxValue,
                        InternationalShippingCost = item.InternationalShippingCost,
                        InlandShippingCost = item.InlandShippingCost,
                        OtherCosts = item.OtherCosts,
                        RealPrice = item.RealPrice,
                        ProductionDescription = item.ProductionDescription
                    };

                    total += item.Price * item.Quantity;
                    materials.Add(profileProduct);
                }
                db.ImportProfileProducts.AddRange(materials);
            }

            // Danh sách vấn đề tồn đọng
            var listCreate = model.ListProblem.Where(i => string.IsNullOrEmpty(i.Id)).ToList();
            var list = db.ImportProfileProblemExists.Where(i => i.ImportProfileId.Equals(profile.Id)).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var delete = model.ListProblem.FirstOrDefault(i => item.Id.Equals(i.Id));
                    if (delete == null)
                    {
                        db.ImportProfileProblemExists.Remove(item);
                    }
                    else
                    {
                        item.Note = delete.Note;
                        item.Plan = delete.Plan;
                        item.Status = delete.Status;
                    }
                }
            }

            ImportProfileProblemExist import;
            List<ImportProfileProblemExist> listProblem = new List<ImportProfileProblemExist>();
            foreach (var item in listCreate)
            {
                import = new ImportProfileProblemExist()
                {
                    Id = Guid.NewGuid().ToString(),
                    ImportProfileId = profile.Id,
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

            profile.Amount = total;
            UpdateCurrentDateByStep(profile, profile.Step);
        }

        private void UpdateDocument(List<ImportProfileDocumentModel> documentSteps, int step, string importProfileId, string userId, List<ImportProfileDocument> documentCreates)
        {
            var listDocumentOtherStep1 = db.ImportProfileDocuments.Where(t => t.ImportProfileId.Equals(importProfileId) && t.Step == step).ToList();

            ImportProfileDocumentModel importProfileDocumentModel;
            foreach (var item in listDocumentOtherStep1)
            {
                importProfileDocumentModel = documentSteps.FirstOrDefault(r => item.Id.Equals(r.Id));

                if (importProfileDocumentModel != null)
                {
                    item.FileName = importProfileDocumentModel.FileName;
                    item.FileSize = importProfileDocumentModel.FileSize;
                    item.FilePath = importProfileDocumentModel.FilePath;
                    item.Note = importProfileDocumentModel.Note;
                    item.SupplierName = importProfileDocumentModel.SupplierName;
                    item.UpdateBy = userId;
                    item.UpdateDate = DateTime.Now;
                }
                else
                {
                    db.ImportProfileDocuments.Remove(item);
                }
            }

            ImportProfileDocument document;
            foreach (var item in documentSteps)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    document = new ImportProfileDocument()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImportProfileId = importProfileId,
                        Step = step,
                        Name = item.Name,
                        IsRequired = item.IsRequired,
                        Note = item.Note,
                        SupplierName = item.SupplierName,
                        FilePath = item.FilePath,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    documentCreates.Add(document);
                }
            }
        }

        private void UpdateDocumentOther(List<ImportProfileDocumentOtherModel> documentOtherSteps, int step, string importProfileId, string userId, List<ImportProfileDocumentOther> documentCreates)
        {
            var listDocumentOtherStep1 = db.ImportProfileDocumentOthers.Where(t => t.ImportProfileId.Equals(importProfileId) && t.Step == step).ToList();

            ImportProfileDocumentOtherModel importProfileDocumentOtherModel;
            foreach (var item in listDocumentOtherStep1)
            {
                importProfileDocumentOtherModel = documentOtherSteps.FirstOrDefault(r => item.Id.Equals(r.Id));

                if (importProfileDocumentOtherModel != null)
                {
                    item.FileName = importProfileDocumentOtherModel.FileName;
                    item.FileSize = importProfileDocumentOtherModel.FileSize;
                    item.FilePath = importProfileDocumentOtherModel.FilePath;
                    item.Note = importProfileDocumentOtherModel.Note;
                    item.UpdateBy = userId;
                    item.UpdateDate = DateTime.Now;
                }
                else
                {
                    db.ImportProfileDocumentOthers.Remove(item);
                }
            }

            ImportProfileDocumentOther documentOther;
            foreach (var item in documentOtherSteps)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    documentOther = new ImportProfileDocumentOther()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImportProfileId = importProfileId,
                        Step = step,
                        Note = item.Note,
                        FilePath = item.FilePath,
                        FileName = item.FileName,
                        FileSize = item.FileSize,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                    };

                    documentCreates.Add(documentOther);
                }
            }
        }
    }
}
