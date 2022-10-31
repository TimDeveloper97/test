using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ExportAndKeep;
using NTS.Model.Repositories;
using NTS.Model.UserHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.ExportAndKeep
{
    public class ExportAndKeepBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm xuất giữ
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns>Danh sách trả về theo điều kiện</returns>
        public SearchResultModel<ExportAndKeepResultModel> SearchExportAndKeep(ExportAndKeepSearchModel modelSearch)
        {
            SearchResultModel<ExportAndKeepResultModel> searchResult = new SearchResultModel<ExportAndKeepResultModel>();

            var dataQuery = (from a in db.SaleProductExports.AsNoTracking()
                             where a.Status == Constants.SaleProductExport_Status_danggiu
                             join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id
                             join e in db.Users.AsNoTracking() on a.CreateBy equals e.Id
                             join d in db.Employees.AsNoTracking() on e.EmployeeId equals d.Id
                             orderby a.ExpiredDate, a.Code
                             select new ExportAndKeepResultModel()
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 CreateByName = d.Name,
                                 CreateDate = a.CreateDate,
                                 CustomerId = a.CustomerId,
                                 CustomerName = c.Name,
                                 ExpiredDate = a.ExpiredDate,
                                 Quantity = a.ProductQuantity,
                                 CreateBy = a.CreateBy,
                                 EmployeeId = d.Id,
                                 PayStatus = a.PayStatus
                             }).AsQueryable();

            // Tìm kiếm theo trạng thái
            if (modelSearch.Status.HasValue)
            {
                if (modelSearch.Status == 1)
                {
                    dataQuery = dataQuery.Where(u => u.ExpiredDate > DateTime.Now);
                }
                else if (modelSearch.Status == 2)
                {
                    dataQuery = dataQuery.Where(u => u.ExpiredDate <= DateTime.Now);
                }
            }

            // Tìm kiếm theo mã xuất giữ
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(a => a.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            // Tìm kiếm theo khách hàng
            if (!string.IsNullOrEmpty(modelSearch.Customer))
            {
                dataQuery = dataQuery.Where(a => a.CustomerName.ToLower().Contains(modelSearch.Customer.ToLower()));
            }

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrEmpty(modelSearch.Product))
            {
                dataQuery = dataQuery.Where(a => a.SaleProductName.ToLower().Contains(modelSearch.Product.ToLower()));
            }

            // Tìm kiếm theo người tạo
            if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
            {
                dataQuery = dataQuery.Where(a => a.EmployeeId.Equals(modelSearch.EmployeeId));
            }

            // Ngày tạo từ
            if (modelSearch.CreateDateFrom.HasValue)
            {
                var createdate = DateTimeHelper.ToStartDate(modelSearch.CreateDateFrom.Value);
                dataQuery = dataQuery.Where(u => u.CreateDate >= createdate);
            }

            // Ngày tạo đến
            if (modelSearch.CreateDateTo.HasValue)
            {
                var createdate = DateTimeHelper.ToEndDate(modelSearch.CreateDateTo.Value);

                dataQuery = dataQuery.Where(u => u.CreateDate <= createdate);
            }

            // Hết hạn từ
            if (modelSearch.ExpiredDateFrom.HasValue)
            {
                var createdate = DateTimeHelper.ToStartDate(modelSearch.ExpiredDateFrom.Value);

                dataQuery = dataQuery.Where(u => u.ExpiredDate >= createdate);
            }

            // Hết hạn đến
            if (modelSearch.ExpiredDateTo.HasValue)
            {
                var createdate = DateTimeHelper.ToStartDate(modelSearch.CreateDateTo.Value);

                dataQuery = dataQuery.Where(u => u.ExpiredDate <= createdate);
            }

            // Tình trạng thanh toán
            if (modelSearch.PayStatus.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.PayStatus == modelSearch.PayStatus);
            }

            // Tìm kiếm theo số lượng
            if (modelSearch.Quantity.HasValue)
            {
                if (modelSearch.QuantityType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity == modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity > modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity >= modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity < modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity <= modelSearch.Quantity);
                }
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            foreach (var item in listResult)
            {
                if (item.ExpiredDate <= DateTime.Now)
                {
                    item.Status = 5;
                }
                else
                {
                    item.Status = 6;
                }
            }

            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Lịch sử xuất giữ
        /// </summary>
        /// <param name="modelSearch">Điều kiện tìm kiếm</param>
        /// <returns></returns>
        public SearchResultModel<ExportAndKeepResultModel> SearchExportAndKeepHistory(ExportAndKeepSearchModel modelSearch)
        {
            SearchResultModel<ExportAndKeepResultModel> searchResult = new SearchResultModel<ExportAndKeepResultModel>();

            var dataQuery = (from a in db.SaleProductExports.AsNoTracking()
                             where a.Status != Constants.SaleProductExport_Status_danggiu
                             join c in db.Customers.AsNoTracking() on a.CustomerId equals c.Id
                             join e in db.Users.AsNoTracking() on a.CreateBy equals e.Id
                             join d in db.Employees.AsNoTracking() on e.EmployeeId equals d.Id
                             orderby a.ExpiredDate
                             select new ExportAndKeepResultModel()
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 CreateByName = d.Name,
                                 CreateDate = a.CreateDate,
                                 CustomerId = a.CustomerId,
                                 CustomerName = c.Name,
                                 ExpiredDate = a.ExpiredDate,
                                 Quantity = a.ProductQuantity,
                                 Status = a.Status,
                                 PayStatus = a.PayStatus,
                                 EmployeeId = d.Id
                             }).AsQueryable();

            // Tìm kiếm theo trạng thái
            if (modelSearch.Status.HasValue)
            {
                dataQuery = dataQuery.Where(a => modelSearch.Status.Value.Equals(a.Status));
            }

            // Tìm kiếm theo mã xuất giữ
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(a => a.Code.ToLower().Contains(modelSearch.Code.ToLower()));
            }

            // Tìm kiếm theo khách hàng
            if (!string.IsNullOrEmpty(modelSearch.Customer))
            {
                dataQuery = dataQuery.Where(a => a.CustomerName.ToLower().Contains(modelSearch.Customer.ToLower()));
            }

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrEmpty(modelSearch.Product))
            {
                dataQuery = dataQuery.Where(a => a.SaleProductName.ToLower().Contains(modelSearch.Product.ToLower()));
            }

            // Ngày tạo từ
            if (modelSearch.CreateDateFrom.HasValue)
            {
                var createdate = DateTimeHelper.ToStartDate(modelSearch.CreateDateFrom.Value);
                dataQuery = dataQuery.Where(u => u.CreateDate >= createdate);
            }

            // Ngày tạo đến
            if (modelSearch.CreateDateTo.HasValue)
            {
                var createdate = DateTimeHelper.ToEndDate(modelSearch.CreateDateTo.Value);

                dataQuery = dataQuery.Where(u => u.CreateDate <= createdate);
            }

            // Hết hạn từ
            if (modelSearch.ExpiredDateFrom.HasValue)
            {
                var createdate = DateTimeHelper.ToStartDate(modelSearch.ExpiredDateFrom.Value);

                dataQuery = dataQuery.Where(u => u.ExpiredDate >= createdate);
            }

            // Tìm kiếm theo người tạo
            if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
            {
                dataQuery = dataQuery.Where(a => a.EmployeeId.Equals(modelSearch.EmployeeId));
            }

            // Hết hạn đến
            if (modelSearch.ExpiredDateTo.HasValue)
            {
                var createdate = DateTimeHelper.ToStartDate(modelSearch.CreateDateTo.Value);

                dataQuery = dataQuery.Where(u => u.ExpiredDate <= createdate);
            }

            // Tìm kiếm theo số lượng
            if (modelSearch.Quantity.HasValue)
            {
                if (modelSearch.QuantityType == 1)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity == modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 2)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity > modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 3)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity >= modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 4)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity < modelSearch.Quantity);
                }
                else if (modelSearch.QuantityType == 5)
                {
                    dataQuery = dataQuery.Where(u => u.Quantity <= modelSearch.Quantity);
                }
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Thêm mới xuất giữ
        /// </summary>
        /// <param name="model">Thông tin thêm mới</param>
        public void CreateExportAndKeep(ExportAndKeepCreateModel model)
        {
            var export = db.SaleProductExports.AsNoTracking().Where(t => t.CustomerId.Equals(model.CustomerId) && t.Status == Constants.SaleProductExport_Status_danggiu && !t.CreateBy.Equals(model.CreateBy)).ToList();
            if (export.Count > 0)
            {
                throw NTSException.CreateInstance("Khách hàng đang được xuất giữ bởi nhân viên khác. Không thể tạo xuất giữ!");
            }

            var date = db.ImportInventories.AsNoTracking().FirstOrDefault(t => t.Type == Constants.ImportInventory_Type_SaleProduct);
            if (date != null)
            {
                if (date.Date < DateTime.Now.Date)
                {
                    throw NTSException.CreateInstance("Số lượng tồn kho không phải mới nhất. Vui lòng cập nhật lại số lượng tồn kho!");
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                int indexMax = 0;
                var sales = db.SaleProductExports.AsNoTracking().ToList();
                if (sales.Count > 0)
                {
                    indexMax = sales.Max(x => x.Index);
                }
                try
                {
                    SaleProductExport exportAndKeep = new SaleProductExport()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        CustomerId = model.CustomerId,
                        ExpiredDate = DateTimeHelper.ToEndDate(model.ExpiredDate),
                        Status = Constants.SaleProductExport_Status_danggiu,
                        Index = indexMax + 1,
                        UpdateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        ProductQuantity = model.ListExportAndKeepDetail.Count,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        PayStatus = model.PayStatus,
                        PaymentPercent = model.PaymentPercent,
                        PaymentAmount = model.PaymentAmount
                    };

                    var codeModel = GenerateCode();
                    exportAndKeep.Code = codeModel.Code;
                    exportAndKeep.Index = codeModel.Index;

                    db.SaleProductExports.Add(exportAndKeep);

                    if (model.ListExportAndKeepDetail.Count > 0)
                    {
                        List<SaleProductExportDetail> listSaleProductDetail = new List<SaleProductExportDetail>();

                        foreach (var item in model.ListExportAndKeepDetail)
                        {
                            SaleProductExportDetail saleProductExportDetail = new SaleProductExportDetail()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Quantity = item.Quantity,
                                SaleProductId = item.Id,
                                SaleProductExportId = exportAndKeep.Id
                            };

                            listSaleProductDetail.Add(saleProductExportDetail);

                            // Update số lượng giữ và số lượng có sẵn trong bảng saleproduct

                            var saleProductInfor = db.SaleProducts.Where(a => a.Id.Equals(item.Id)).FirstOrDefault();
                            if (saleProductInfor != null)
                            {
                                if (item.Quantity > saleProductInfor.AvailableQuantity)
                                {
                                    throw NTSException.CreateInstance("Sản phẩm " + saleProductInfor.VName + " có số lượng đang giữ lớn hơn số lượng khả dụng");
                                }

                                saleProductInfor.ExportQuantity = saleProductInfor.ExportQuantity + item.Quantity;
                                saleProductInfor.AvailableQuantity = saleProductInfor.Inventory - saleProductInfor.ExportQuantity;

                                if (saleProductInfor.AvailableQuantity < 0)
                                {
                                    saleProductInfor.AvailableQuantity = 0;
                                }
                            }
                        }

                        db.SaleProductExportDetails.AddRange(listSaleProductDetail);
                    }

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, exportAndKeep.Code, exportAndKeep.Id, Constants.LOG_SaleProductExport);

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
        /// Chỉnh sửa xuất giữ
        /// </summary>
        /// <param name="model">Thông tin chỉnh sửa</param>
        public void UpdateExportAndKeep(ExportAndKeepCreateModel model, string userid, bool isUpdateOther)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var saleProductExportInfo = db.SaleProductExports.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
                    if (saleProductExportInfo == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
                    }

                    if (!saleProductExportInfo.CreateBy.Equals(userid) && !isUpdateOther)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0032, TextResourceKey.SaleProductsExport);
                    }

                    if (userid != saleProductExportInfo.CreateBy)
                    {
                        throw NTSException.CreateInstance("Bạn không phải là người tạo phiếu xuất giữ. Không được phép chỉnh sửa!");
                    }

                    var saleProductExportDetails = db.SaleProductExportDetails.Where(a => a.SaleProductExportId.Equals(model.Id)).ToList();

                    if (saleProductExportDetails.Count > 0)
                    {
                        db.SaleProductExportDetails.RemoveRange(saleProductExportDetails);
                    }

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SaleProductExportHistory>(saleProductExportInfo);

                    saleProductExportInfo.ExpiredDate = DateTimeHelper.ToEndDate(model.ExpiredDate);
                    saleProductExportInfo.CustomerId = model.CustomerId;
                    saleProductExportInfo.UpdateBy = userid;
                    saleProductExportInfo.UpdateDate = DateTime.Now;
                    saleProductExportInfo.ProductQuantity = model.ListExportAndKeepDetail.Count;
                    saleProductExportInfo.PhoneNumber = model.PhoneNumber;
                    saleProductExportInfo.Address = model.Address;
                    saleProductExportInfo.PayStatus = model.PayStatus;
                    saleProductExportInfo.PaymentPercent = model.PaymentPercent;
                    saleProductExportInfo.PaymentAmount = model.PaymentAmount;

                    // Kiểm tra tiến độ thanh toán nếu >= 100 thì đổi trạng thái thanh toán
                    if (model.PaymentPercent >= 100)
                    {
                        saleProductExportInfo.PayStatus = Constants.SaleProductExport_PayStatus_Paid;
                    }
                    else
                    {
                        saleProductExportInfo.PayStatus = Constants.SaleProductExport_PayStatus_Unpaid;
                    }

                    if (model.ListExportAndKeepDetail.Count > 0)
                    {
                        List<SaleProductExportDetail> listSaleProductDetail = new List<SaleProductExportDetail>();
                        foreach (var item in model.ListExportAndKeepDetail)
                        {
                            SaleProductExportDetail saleProductExportDetail = new SaleProductExportDetail()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Quantity = item.Quantity,
                                SaleProductId = item.Id,
                                SaleProductExportId = model.Id
                            };
                            listSaleProductDetail.Add(saleProductExportDetail);

                            // Update số lượng giữ và số lượng có sẵn trong bảng saleproduct
                            var saleProductInfor = db.SaleProducts.Where(a => a.Id.Equals(item.Id)).FirstOrDefault();
                            decimal totalExport = 0;
                            if (saleProductInfor != null)
                            {
                                totalExport = (from a in db.SaleProductExportDetails.AsNoTracking()
                                               join b in db.SaleProductExports.AsNoTracking() on a.SaleProductExportId equals b.Id
                                               where b.Status == Constants.SaleProductExport_Status_danggiu && a.SaleProductId.Equals(item.Id) && !a.SaleProductExportId.Equals(model.Id)
                                               select a.Quantity).DefaultIfEmpty(0).Sum();

                                if (item.Quantity > saleProductInfor.Inventory - totalExport)
                                {
                                    throw NTSException.CreateInstance("Sản phẩm " + saleProductInfor.VName + " có số lượng đang giữ lớn hơn số lượng khả dụng");
                                }

                                saleProductInfor.ExportQuantity = totalExport + item.Quantity;

                                saleProductInfor.AvailableQuantity = saleProductInfor.Inventory - saleProductInfor.ExportQuantity;

                                if (saleProductInfor.AvailableQuantity < 0)
                                {
                                    saleProductInfor.AvailableQuantity = 0;
                                }
                            }
                        }

                        db.SaleProductExportDetails.AddRange(listSaleProductDetail);
                    }

                    if (userid != saleProductExportInfo.CreateBy)
                    {
                        throw NTSException.CreateInstance("Bạn không có quyền chỉnh sửa!");
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<SaleProductExportHistory>(saleProductExportInfo);
                    //UserLogUtil.LogHistotyUpdateInfo(db, userid, Constants.LOG_SaleProductExport, saleProductExportInfo.Id, saleProductExportInfo.Code, jsonBefor, jsonApter);

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
        /// Lấy thông tin xuất giữ theo id
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        /// <returns></returns>
        public ExportAndKeepModel GetInfoByIdExportAndKeep(string id)
        {
            var result = (from a in db.SaleProductExports.AsNoTracking()
                          where a.Id.Equals(id)
                          join e in db.Users.AsNoTracking() on a.CreateBy equals e.Id
                          join b in db.Employees.AsNoTracking() on e.EmployeeId equals b.Id
                          join c in db.Departments.AsNoTracking() on b.DepartmentId equals c.Id
                          join d in db.SBUs.AsNoTracking() on c.SBUId equals d.Id
                          select new ExportAndKeepModel()
                          {
                              Id = a.Id,
                              Code = a.Code,
                              CreateBy = a.CreateBy,
                              CustomerId = a.CustomerId,
                              ExpiredDate = a.ExpiredDate,
                              employeeCode = b.Code,
                              employeeName = b.Name,
                              email = b.Email,
                              SbuName = d.Name,
                              departmentName = c.Name,
                              Status = a.Status,
                              PhoneNumber = a.PhoneNumber,
                              Address = a.Address,
                              PayStatus = a.PayStatus,
                              PaymentPercent = a.PaymentPercent,
                              PaymentAmount = a.PaymentAmount
                          }).FirstOrDefault();

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
            }

            if (result.Status == 2)
            {
                if (result.ExpiredDate <= DateTime.Now)
                {
                    result.Status = 5;
                }
                else
                {
                    result.Status = 6;

                }
            }

            result.ListExportAndKeepDetail = (from s in db.SaleProductExportDetails.AsNoTracking()
                                              where s.SaleProductExportId.Equals(id)
                                              join a in db.SaleProducts.AsNoTracking() on s.SaleProductId equals a.Id
                                              orderby a.Model
                                              select new SaleProductModel
                                              {
                                                  Id = a.Id,
                                                  Name = a.VName,
                                                  Code = a.Model,
                                                  Inventory = a.Inventory,
                                                  ImagePath = a.AvatarPath,
                                                  Quantity = s.Quantity,
                                                  SaleProductId = s.SaleProductId,
                                                  ExportQuantity = a.ExportQuantity,
                                                  AvailableQuantity = a.AvailableQuantity
                                              }).ToList();

            decimal availableQuantity = 0;
            foreach (var item in result.ListExportAndKeepDetail)
            {
                availableQuantity = (from p in db.SaleProductExportDetails.AsNoTracking()
                                     join e in db.SaleProductExports.AsNoTracking() on p.SaleProductExportId equals e.Id
                                     where !e.Id.Equals(id) && e.Status == Constants.SaleProductExport_Status_danggiu && p.SaleProductId.Equals(item.SaleProductId)
                                     select p.Quantity
                                     ).DefaultIfEmpty(0).Sum();

                item.AvailableQuantity = item.Inventory - availableQuantity;
            }

            return result;
        }

        /// <summary>
        /// Lấy thông tin xuất giữ theo id xem
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        /// <returns></returns>
        public ExportAndKeepViewModel GetExportAndKeepViewById(string id, string userid)
        {
            var result = (from a in db.SaleProductExports.AsNoTracking()
                          where a.Id.Equals(id)
                          join f in db.Users.AsNoTracking() on a.CreateBy equals f.Id
                          join b in db.Employees.AsNoTracking() on f.EmployeeId equals b.Id
                          join c in db.Departments.AsNoTracking() on b.DepartmentId equals c.Id
                          join d in db.SBUs.AsNoTracking() on c.SBUId equals d.Id
                          join e in db.Customers.AsNoTracking() on a.CustomerId equals e.Id
                          join t in db.CustomerTypes.AsNoTracking() on e.CustomerTypeId equals t.Id
                          select new ExportAndKeepViewModel()
                          {
                              Id = a.Id,
                              Code = a.Code,
                              CreateBy = a.CreateBy,
                              CreateDate = a.CreateDate,
                              ExpiredDate = a.ExpiredDate,
                              EmployeeCode = b.Code,
                              EmployeeName = b.Name,
                              Email = b.Email,
                              SbuName = d.Name,
                              DepartmentName = c.Name,
                              Status = a.Status,
                              CustomerName = e.Name,
                              CustomerAddress = a.Address,
                              CustomerCode = e.Code,
                              CustomerPhoneNumber = a.PhoneNumber,
                              CustomerTypeName = t.Name,
                              PayStatus = a.PayStatus
                          }).FirstOrDefault();

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
            }

            if (result.Status == 2)
            {
                if (result.ExpiredDate <= DateTime.Now)
                {
                    result.Status = 5;
                }
                else
                {
                    result.Status = 6;

                }
            }

            result.ListExportAndKeepDetail = (from s in db.SaleProductExportDetails.AsNoTracking()
                                              where s.SaleProductExportId.Equals(id)
                                              join a in db.SaleProducts.AsNoTracking() on s.SaleProductId equals a.Id
                                              orderby a.Model
                                              select new SaleProductModel
                                              {
                                                  Id = a.Id,
                                                  Name = a.VName,
                                                  Code = a.Model,
                                                  Inventory = a.Inventory,
                                                  ImagePath = a.AvatarPath,
                                                  Quantity = s.Quantity,
                                                  SaleProductId = s.SaleProductId,
                                                  ExportQuantity = a.ExportQuantity,
                                                  AvailableQuantity = a.AvailableQuantity
                                              }).ToList();

            decimal availableQuantity = 0;
            foreach (var item in result.ListExportAndKeepDetail)
            {
                availableQuantity = (from p in db.SaleProductExportDetails.AsNoTracking()
                                     join e in db.SaleProductExports.AsNoTracking() on p.SaleProductExportId equals e.Id
                                     where !e.Id.Equals(id) && e.Status == Constants.SaleProductExport_Status_danggiu && p.SaleProductId.Equals(item.SaleProductId)
                                     select p.Quantity
                                     ).DefaultIfEmpty(0).Sum();

                item.AvailableQuantity = item.Inventory - availableQuantity;
            }

            var conten = $"Xem thông tin xuất giữ";
            UserLogUtil.LogHistotyUpdateOther(db, userid, Constants.LOG_SaleProductExport, result.Id, result.Code, conten);

            return result;
        }

        /// <summary>
        /// Xóa xuất giữ theo id
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        public void DeleteExportAndKeepById(string id, string userid, bool isUpdateOther)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var saleProductExportInfo = db.SaleProductExports.Where(a => a.Id.Equals(id)).FirstOrDefault();
                    if (saleProductExportInfo == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
                    }

                    if (!saleProductExportInfo.CreateBy.Equals(userid) && !isUpdateOther)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0037, TextResourceKey.SaleProductsExport);
                    }

                    if (userid != saleProductExportInfo.CreateBy)
                    {
                        throw NTSException.CreateInstance("Bạn không phải là người tạo phiếu xuất giữ. Không được phép xóa!");
                    }

                    var saleProductExportDetails = db.SaleProductExportDetails.Where(a => a.SaleProductExportId.Equals(id)).ToList();

                    if (saleProductExportDetails.Count > 0)
                    {
                        foreach (var item in saleProductExportDetails)
                        {
                            var saleProductInfor = db.SaleProducts.Where(a => a.Id.Equals(item.SaleProductId)).FirstOrDefault();
                            if (saleProductInfor != null)
                            {
                                var totalExport = (from p in db.SaleProductExportDetails.AsNoTracking()
                                                   where p.SaleProductId.Equals(item.SaleProductId) && !p.SaleProductExportId.Equals(id)
                                                   join e in db.SaleProductExports.AsNoTracking() on p.SaleProductExportId equals e.Id
                                                   where e.Status == Constants.SaleProductExport_Status_danggiu
                                                   select p.Quantity).DefaultIfEmpty(0).Sum();

                                saleProductInfor.ExportQuantity = totalExport;

                                saleProductInfor.AvailableQuantity = saleProductInfor.Inventory - saleProductInfor.ExportQuantity;

                                if (saleProductInfor.AvailableQuantity < 0)
                                {
                                    saleProductInfor.AvailableQuantity = 0;
                                }
                            }
                        }

                        db.SaleProductExportDetails.RemoveRange(saleProductExportDetails);
                    }

                    db.SaleProductExports.Remove(saleProductExportInfo);

                    //var jsonApter = AutoMapperConfig.Mapper.Map<SaleProductExportHistory>(saleProductExportInfo);
                    //UserLogUtil.LogHistotyDelete(db, userid, Constants.LOG_Project, saleProductExportInfo.Id, saleProductExportInfo.Code, jsonApter);

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

        /// <summary>
        /// Lấy danh sách sản phẩm kinh doanh
        /// </summary>
        /// <param name="modelSearch">dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public object GetSaleProducts(SaleProductSearchModel modelSearch, bool isPermissionViewAll)
        {
            SearchResultModel<SaleProductModel> searchResult = new SearchResultModel<SaleProductModel>();

            var saleProducts = (from a in db.SaleProducts.AsNoTracking()
                                where a.Status == Constants.SaleProduct_Status_UnLock
                                orderby a.VName
                                select new SaleProductModel()
                                {
                                    Id = a.Id,
                                    Name = a.VName,
                                    Code = a.Model,
                                    Inventory = a.Inventory,
                                    ImagePath = a.AvatarPath,
                                    SaleProductTypeId = a.SaleProductTypeId,
                                    AvailableQuantity = a.AvailableQuantity,
                                    ExportQuantity = a.ExportQuantity,
                                    Specifications = a.Specifications,
                                    CountryName = a.CountryName,
                                    ManufactureId = a.ManufactureId,
                                    EXWTPAPrice = a.EXWTPAPrice,
                                }).AsQueryable().Distinct();

            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                saleProducts = saleProducts.Where(a => a.Code.ToLower().Contains(modelSearch.NameCode.ToLower()) || a.Name.ToLower().Contains(modelSearch.NameCode.ToLower()) || a.Specifications.ToLower().Contains(modelSearch.NameCode.ToLower()));
            }

            if (!isPermissionViewAll)
            {
                var saleGroupProducts = (from p in db.SaleGroupProducts.AsNoTracking()
                                         where modelSearch.SaleGroupIdRequests.Contains(p.SaleGroupId)
                                         group p by p.SaleProductId into g
                                         select g.Key).ToList();

                saleProducts = saleProducts.Where(a => saleGroupProducts.Contains(a.Id));
            }

            if (!string.IsNullOrEmpty(modelSearch.SaleProductTypeId))
            {
                List<string> listParentId = new List<string>();

                var saleProductTypes = db.SaleProductTypes.AsNoTracking().Where(r => !string.IsNullOrEmpty(r.ParentId)).ToList();

                listParentId.Add(modelSearch.SaleProductTypeId);

                listParentId.AddRange(GetListParent(modelSearch.SaleProductTypeId, saleProductTypes));

                saleProducts = saleProducts.Where(a => listParentId.Contains(a.SaleProductTypeId));
            }

            // Số lượng tồn kho
            if (modelSearch.Inventory.HasValue)
            {
                if (modelSearch.InventoryType == 1)
                {
                    saleProducts = saleProducts.Where(u => u.Inventory == modelSearch.Inventory.Value);
                }
                else if (modelSearch.InventoryType == 2)
                {
                    saleProducts = saleProducts.Where(u => u.Inventory > modelSearch.Inventory.Value);
                }
                else if (modelSearch.InventoryType == 3)
                {
                    saleProducts = saleProducts.Where(u => u.Inventory >= modelSearch.Inventory.Value);
                }
                else if (modelSearch.InventoryType == 4)
                {
                    saleProducts = saleProducts.Where(u => u.Inventory < modelSearch.Inventory.Value);
                }
                else if (modelSearch.InventoryType == 5)
                {
                    saleProducts = saleProducts.Where(u => u.Inventory <= modelSearch.Inventory.Value);
                }
            }

            // Số lượng khả dụng
            if (modelSearch.AvailableQuantity.HasValue)
            {
                if (modelSearch.AvailableQuantityType == 1)
                {
                    saleProducts = saleProducts.Where(u => u.AvailableQuantity == modelSearch.AvailableQuantity.Value);
                }
                else if (modelSearch.AvailableQuantityType == 2)
                {
                    saleProducts = saleProducts.Where(u => u.AvailableQuantity > modelSearch.AvailableQuantity.Value);
                }
                else if (modelSearch.AvailableQuantityType == 3)
                {
                    saleProducts = saleProducts.Where(u => u.AvailableQuantity >= modelSearch.AvailableQuantity.Value);
                }
                else if (modelSearch.AvailableQuantityType == 4)
                {
                    saleProducts = saleProducts.Where(u => u.AvailableQuantity < modelSearch.AvailableQuantity.Value);
                }
                else if (modelSearch.AvailableQuantityType == 5)
                {
                    saleProducts = saleProducts.Where(u => u.AvailableQuantity <= modelSearch.AvailableQuantity.Value);
                }
            }

            // Xuất xứ
            if (!string.IsNullOrEmpty(modelSearch.CountryName))
            {
                saleProducts = saleProducts.Where(u => u.CountryName.ToUpper().Contains(modelSearch.CountryName.ToUpper()));
            }

            // Hãng sản xuất
            if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
            {
                saleProducts = saleProducts.Where(u => u.ManufactureId.Equals(modelSearch.ManufactureId));
            }

            // Giá
            if (modelSearch.EXWTPAPrice.HasValue)
            {
                if (modelSearch.EXWTPAPriceType == 1)
                {
                    saleProducts = saleProducts.Where(u => u.EXWTPAPrice == modelSearch.EXWTPAPrice.Value);
                }
                else if (modelSearch.EXWTPAPriceType == 2)
                {
                    saleProducts = saleProducts.Where(u => u.EXWTPAPrice > modelSearch.EXWTPAPrice.Value);
                }
                else if (modelSearch.EXWTPAPriceType == 3)
                {
                    saleProducts = saleProducts.Where(u => u.EXWTPAPrice >= modelSearch.EXWTPAPrice.Value);
                }
                else if (modelSearch.EXWTPAPriceType == 4)
                {
                    saleProducts = saleProducts.Where(u => u.EXWTPAPrice < modelSearch.EXWTPAPrice.Value);
                }
                else if (modelSearch.EXWTPAPriceType == 5)
                {
                    saleProducts = saleProducts.Where(u => u.EXWTPAPrice <= modelSearch.EXWTPAPrice.Value);
                }
            }

            if (modelSearch.ListIdSelect.Count > 0)
            {
                saleProducts = saleProducts.Where(a => !modelSearch.ListIdSelect.Contains(a.Id));
            }

            searchResult.TotalItem = saleProducts.Count();
            var listResult = SQLHelpper.OrderBy(saleProducts, modelSearch.OrderBy, modelSearch.OrderType)
                .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            searchResult.ListResult = listResult;

            return searchResult;
        }

        public SaleProductModel GetSaleProductById(string id)
        {

            var saleProduct = (from a in db.SaleProducts.AsNoTracking()
                               where a.Id.Equals(id)
                               select new SaleProductModel()
                               {
                                   Id = a.Id,
                                   Name = a.VName,
                                   Code = a.Model,
                                   Inventory = a.Inventory,
                                   ImagePath = a.AvatarPath,
                                   ExportQuantity = a.ExportQuantity,
                                   AvailableQuantity = a.AvailableQuantity,
                               }).FirstOrDefault();

            return saleProduct;
        }


        public List<string> GetListParent(string id, List<SaleProductType> saleProductTypes)
        {
            List<string> listChild = new List<string>();

            var moduleGroup = saleProductTypes.Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(moduleGroup);
            if (moduleGroup.Count > 0)
            {
                foreach (var item in moduleGroup)
                {
                    listChild.AddRange(GetListParent(item, saleProductTypes));
                }
            }
            return listChild;
        }

        /// <summary>
        /// Tự động tạo mã xuất giữ
        /// </summary>
        /// <returns></returns>
        public ExportAndKeepCodeModel GenerateCode()
        {
            var dateNow = DateTime.Now;
            string exportAndKeepCode = "";
            var maxIndex = db.SaleProductExports.AsNoTracking().Where(r => r.CreateDate.Month == dateNow.Month && r.CreateDate.Year == dateNow.Year).Select(r => r.Index).DefaultIfEmpty(0).Max();
            maxIndex++;
            exportAndKeepCode = $"XG.{dateNow.ToString("YYMM")}.{string.Format("{0:00000}", maxIndex)}";

            return new ExportAndKeepCodeModel
            {
                Code = exportAndKeepCode,
                Index = maxIndex
            };
        }

        /// <summary>
        /// Thêm mới thông tin khách hàng 
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        public string CreateCustomer(CustomerCreateModel model, string userId, string sbuId)
        {
            //int indexMax = db.Customers.Max(a => a.Index);
            Customer customer;
            if (!string.IsNullOrEmpty(model.Id))
            {
                customer = db.Customers.FirstOrDefault(t => t.Id.Equals(model.Id));
                if (customer == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Customer);
                }

                customer.Name = model.Name;
                customer.Note = model.Note;
                customer.Code = model.PhoneNumber;
                customer.PhoneNumber = model.PhoneNumber;
                customer.CustomerTypeId = model.CustomerTypeId;
                customer.SBUId = sbuId;
                customer.UpdateDate = DateTime.Now;
                customer.UpdateBy = userId;
                customer.Address = model.Address;
            }
            else
            {
                customer = (from a in db.Customers
                            join b in db.CustomerTypes on a.CustomerTypeId equals b.Id
                            where a.PhoneNumber.Equals(model.PhoneNumber) && b.Type == Constants.CUSTOMER_SALEPRODUCT
                            select a).FirstOrDefault();

                if(customer!=null)
                {
                    customer.Name = model.Name;
                    customer.Note = model.Note;
                    customer.CustomerTypeId = model.CustomerTypeId;
                    customer.SBUId = sbuId;
                    customer.CreateDate = DateTime.Now;
                    customer.CreateBy = userId;
                    customer.Address = model.Address;
                }
                else
                {
                    customer = new Customer()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Note = model.Note,
                        Code = model.PhoneNumber,
                        PhoneNumber = model.PhoneNumber,
                        CustomerTypeId = model.CustomerTypeId,
                        SBUId = sbuId,
                        CreateDate = DateTime.Now,
                        CreateBy = userId,
                        Address = model.Address
                    };
                }
                db.Customers.Add(customer);
                UserLogUtil.LogHistotyAdd(db, userId, customer.Code, customer.Id, Constants.LOG_Customer);
            }

            //var codeModel = GenerateCodeCustomer();
            //customer.Code = codeModel.Code;
            //customer.Index = codeModel.Index;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return customer.Id;
        }

        /// <summary>
        /// Lấy danh sách nhóm khách hàng có type ==1
        /// </summary>
        /// <returns></returns>
        public List<CustomerTypeModel> GetCustomerTypes()
        {
            var result = (from a in db.CustomerTypes.AsNoTracking()
                          where a.Type == 1
                          select new CustomerTypeModel
                          {
                              Id = a.Id,
                              Code = a.Code,
                              Name = a.Name,
                              ParentId = a.ParentId
                          }).ToList();

            return result;
        }

        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public List<CustomerResultModel> GetListCustomer()
        {
            List<CustomerResultModel> searchResult = new List<CustomerResultModel>();
            try
            {
                var ListModel = (from a in db.Customers.AsNoTracking()
                                 join b in db.CustomerTypes.AsNoTracking() on a.CustomerTypeId equals b.Id
                                 orderby a.Code ascending
                                 where b.Type == Constants.CUSTOMER_SALEPRODUCT
                                 select new CustomerResultModel()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     PhoneNumber = a.PhoneNumber,
                                     Address = a.Address,
                                     CustomerTypeName = b.Name,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        /// <summary>
        /// Tự động tạo mã khách hàng
        /// </summary>
        /// <returns></returns>
        public ExportAndKeepCodeModel GenerateCodeCustomer()
        {
            string codeCustomer = string.Empty;
            int code = 0;

            var indexs = db.Customers.Select(x => x.Index).ToList();
            if (indexs != null)
            {
                code = indexs.Max();
            }
            code++;
            if (code <= 999)
            {
                codeCustomer = $"A{string.Format("{0:000}", code)}";
            }
            else if (code <= 1998)
            {
                var index = code - 1000;
                codeCustomer = $"B{string.Format("{0:000}", code)}";
            }
            else if (code <= 2997)
            {
                var index = code - 1999;
                codeCustomer = $"C{string.Format("{0:000}", code)}";
            }
            else if (code <= 3996)
            {
                var index = code - 2998;
                codeCustomer = $"D{string.Format("{0:000}", code)}";
            }
            else if (code <= 4995)
            {
                var index = code - 3997;
                codeCustomer = $"H{string.Format("{0:000}", code)}";

            }
            else if (code <= 5994)
            {
                var index = code - 4996;
                codeCustomer = $"I{string.Format("{0:000}", code)}";

            }
            else if (code <= 6993)
            {
                var index = code - 5995;
                codeCustomer = $"J{string.Format("{0:000}", code)}";

            }
            else if (code <= 7992)
            {
                var index = code - 6994;
                codeCustomer = $"K{string.Format("{0:000}", code)}";

            }

            return new ExportAndKeepCodeModel
            {
                Code = codeCustomer,
                Index = code
            };
        }

        /// <summary>
        /// Giải phóng xuất giữ
        /// </summary>
        /// <param name="id">id xuất giữ</param>
        public void ManumitExportAndKeep(string id, string userid)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var saleProductExportInfo = db.SaleProductExports.Where(a => a.Id.Equals(id)).FirstOrDefault();


                    if (saleProductExportInfo == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
                    }

                    if (userid != saleProductExportInfo.CreateBy)
                    {
                        throw NTSException.CreateInstance("Bạn không phải là người tạo phiếu xuất giữ. Không được phép giải phóng xuất giữ!");
                    }

                    saleProductExportInfo.Status = Constants.SaleProductExport_Status_travekho;

                    var saleProductIds = db.SaleProductExportDetails.AsNoTracking().Where(a => a.SaleProductExportId.Equals(id)).Select(x => x.SaleProductId);

                    decimal totalExport = 0;
                    SaleProduct saleProductInfor;
                    foreach (var saleProductId in saleProductIds)
                    {
                        totalExport = (from a in db.SaleProductExportDetails.AsNoTracking()
                                       join b in db.SaleProductExports.AsNoTracking() on a.SaleProductExportId equals b.Id
                                       where b.Status == Constants.SaleProductExport_Status_danggiu && a.SaleProductId.Equals(saleProductId) && !a.SaleProductExportId.Equals(id)
                                       select a.Quantity).DefaultIfEmpty(0).Sum();

                        saleProductInfor = db.SaleProducts.Where(a => a.Id.Equals(saleProductId)).FirstOrDefault();

                        saleProductInfor.ExportQuantity = totalExport;
                        saleProductInfor.AvailableQuantity = saleProductInfor.Inventory - totalExport;

                        if (saleProductInfor.AvailableQuantity < 0)
                        {
                            saleProductInfor.AvailableQuantity = 0;
                        }
                    }

                    var conten = $"Giải phóng xuất giữ";
                    UserLogUtil.LogHistotyUpdateOther(db, userid, Constants.LOG_SaleProductExport, saleProductExportInfo.Id, saleProductExportInfo.Code, conten);

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

        /// <summary>
        /// Đã bán xuất giữ
        /// </summary>
        /// <param name="id"></param>
        public void SoldExportAndKeep(string id, string userid)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var saleProductExportInfo = db.SaleProductExports.Where(a => a.Id.Equals(id)).FirstOrDefault();

                    if (saleProductExportInfo == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
                    }

                    if (userid != saleProductExportInfo.CreateBy)
                    {
                        throw NTSException.CreateInstance("Bạn không phải là người tạo phiếu xuất giữ. Không được phép đã bán xuất giữ!");
                    }

                    saleProductExportInfo.Status = Constants.SaleProductExport_Status_daban;

                    var saleProductIds = db.SaleProductExportDetails.AsNoTracking().Where(a => a.SaleProductExportId.Equals(id)).Select(x => x.SaleProductId);

                    decimal totalExportQuantity = 0;
                    SaleProduct saleProductInfor;
                    foreach (var saleProductId in saleProductIds)
                    {
                        totalExportQuantity = (from a in db.SaleProductExportDetails.AsNoTracking()
                                               join b in db.SaleProductExports.AsNoTracking() on a.SaleProductExportId equals b.Id
                                               where a.SaleProductId.Equals(saleProductId) && b.Id.Equals(id)
                                               select a.Quantity).Sum();

                        saleProductInfor = db.SaleProducts.Where(a => a.Id.Equals(saleProductId)).FirstOrDefault();
                        if (totalExportQuantity <= saleProductInfor.Inventory)
                        {
                            saleProductInfor.Inventory -= totalExportQuantity;
                        }
                        else
                        {
                            saleProductInfor.Inventory = 0;
                        }

                    }

                    var conten = $"Đã bán xuất giữ";
                    UserLogUtil.LogHistotyUpdateOther(db, userid, Constants.LOG_SaleProductExport, saleProductExportInfo.Id, saleProductExportInfo.Code, conten);

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

        /// <summary>
        /// Lấy danh sách chi tiết theo sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SearchResultModel<ExportDetailSaleProductModel> GetListExportDetailBySaleProductId(string id)
        {
            SearchResultModel<ExportDetailSaleProductModel> searchResult = new SearchResultModel<ExportDetailSaleProductModel>();

            var result = (from a in db.SaleProducts.AsNoTracking()
                          where a.Id.Equals(id)
                          join b in db.SaleProductExportDetails.AsNoTracking() on a.Id equals b.SaleProductId
                          join c in db.SaleProductExports.AsNoTracking() on b.SaleProductExportId equals c.Id
                          join f in db.Users.AsNoTracking() on c.CreateBy equals f.Id
                          join d in db.Employees.AsNoTracking() on f.EmployeeId equals d.Id
                          join e in db.Customers.AsNoTracking() on c.CustomerId equals e.Id
                          select new ExportDetailSaleProductModel
                          {
                              Id = b.Id,
                              Code = c.Code,
                              CreateBy = d.Name,
                              Customer = e.Name,
                              CreateDate = c.CreateDate,
                              ExpirationDate = c.ExpiredDate,
                              AvailableQuantity = b.Quantity,
                              TotalProduct = c.ProductQuantity,
                              Status = c.Status,
                          }).ToList();

            DateTime dateNow = DateTime.Now;
            foreach (var item in result)
            {
                if (item.Status == 2)
                {
                    if (item.ExpirationDate > dateNow)
                    {
                        item.Status = 5;
                    }
                    else
                    {
                        item.Status = 6;
                    }
                }
            }

            searchResult.ListResult = result;

            return searchResult;
        }

        public string PrintCustomer(string id)
        {
            var data = (from a in db.SaleProductExports.AsNoTracking()
                        join b in db.Customers.AsNoTracking() on a.CustomerId equals b.Id
                        join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                        join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                        where a.Id.Equals(id)
                        select new
                        {
                            Mobile = d.PhoneNumber,
                            CustomerName = b.Name,
                            a.PhoneNumber,
                            a.Address,
                        }).FirstOrDefault();

            if (data == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleProductsExport);
            }

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string templatePath = HttpContext.Current.Server.MapPath("~/Template/CustomerInfo.docx");
            using (WordDocument document = new WordDocument(templatePath))
            {
                var now = DateTime.Today;

                // Thông tin người gửi
                document.NTSReplaceFirst("<companyName>", Constants.TPAName);
                document.NTSReplaceFirst("<phone1>", Constants.PhoneNumber);
                document.NTSReplaceFirst("<address1>", Constants.Address);
                if (!string.IsNullOrEmpty(data.Mobile))
                {
                    document.NTSReplaceFirst("<mobile>", data.Mobile);
                }
                else
                {
                    document.NTSReplaceFirst("<mobile>", string.Empty);
                }

                // Thông tin người nhận
                document.NTSReplaceFirst("<customerName>", data.CustomerName);
                document.NTSReplaceFirst("<phone2>", data.PhoneNumber);
                document.NTSReplaceFirst("<address2>", data.Address);

                DocToPDFConverter converter = new DocToPDFConverter();
                //Converts Word document into PDF document
                PdfDocument pdfDocument = converter.ConvertToPDF(document);
                //Saves the PDF file 
                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "ThongTinKhachHang_" + specifyName + ".pdf");
                pdfDocument.Save(pathFileSave);
                //Closes the instance of document objects
                pdfDocument.Close();
                document.Close();

                return pathFileSave;
            }
        }
    }
}
