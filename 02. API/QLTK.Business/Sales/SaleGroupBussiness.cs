using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SaleGroups;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SaleGroups
{
    /// <summary>
    /// Nhóm kinh doanh
    /// </summary>
    public class SaleGroupBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Lấy danh sách nhóm kinh doanh
        /// </summary>
        /// <param name="modelSearch">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public SearchResultModel<SaleGroupResultModel> SearchSaleGroup(SearchSaleGroupModel modelSearch)
        {
            SearchResultModel<SaleGroupResultModel> searchResult = new SearchResultModel<SaleGroupResultModel>();

            var dataQuery = (from a in db.SaleGroups.AsNoTracking()
                             orderby a.Name
                             select new SaleGroupResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Note))
            {
                dataQuery = dataQuery.Where(u => u.Note.ToUpper().Contains(modelSearch.Note.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();

            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();

            foreach (var item in listResult)
            {
                item.TotalEmployee = db.SaleGroupUsers.AsNoTracking().Where(a => a.SaleGroupId.Equals(item.Id)).Count();
                item.TotalProduct = db.SaleGroupProducts.AsNoTracking().Where(a => a.SaleGroupId.Equals(item.Id)).Count();
            }

            searchResult.ListResult = listResult;

            return searchResult;

        }

        /// <summary>
        /// Thêm danh sách nhân viên kinh doanh trong nhóm
        /// </summary>
        /// <param name="listEmployee"></param>
        /// <param name="saleGroupId"></param>
        public void CreateSaleGroupUser(List<SaleGroupUserModel> listEmployee, string saleGroupId)
        {
            if (listEmployee.Count > 0)
            {
                List<SaleGroupUser> employees = new List<SaleGroupUser>();

                foreach (var item in listEmployee)
                {
                    SaleGroupUser saleGroupUser = new SaleGroupUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        SaleGroupId = saleGroupId,
                        UserId = item.Id,
                    };

                    employees.Add(saleGroupUser);
                }

                db.SaleGroupUsers.AddRange(employees);
            }
        }

        /// <summary>
        /// Thêm danh sách sản phẩm kinh doanh trong nhóm
        /// </summary>
        /// <param name="listEmployee"></param>
        /// <param name="saleGroupId"></param>
        public void CreateSaleGroupProduct(List<SaleGroupProductModel> listEmployee, string saleGroupId)
        {
            if (listEmployee.Count > 0)
            {
                List<SaleGroupProduct> products = new List<SaleGroupProduct>();

                foreach (var item in listEmployee)
                {
                    SaleGroupProduct saleGroupProduct = new SaleGroupProduct
                    {
                        Id = Guid.NewGuid().ToString(),
                        SaleGroupId = saleGroupId,
                        SaleProductId = item.Id,
                    };

                    products.Add(saleGroupProduct);
                }

                db.SaleGroupProducts.AddRange(products);
            }
        }

        /// <summary>
        /// Thêm nhóm kinh doanh
        /// </summary>
        /// <param name="model">Dữ liệu thêm vào</param>
        public void CreateSaleGroup(SaleGroupCreateModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var checkNameExits = db.SaleGroups.Where(a => a.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower())).FirstOrDefault();
                    if (checkNameExits != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SaleGroup);
                    }

                    if (model.Name.Trim() == "")
                    {
                        throw NTSException.CreateInstance("Bạn không được để trống tên!");
                    }

                    NTS.Model.Repositories.SaleGroup saleGroup = new NTS.Model.Repositories.SaleGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.Trim(),
                        Note = model.Note,
                    };

                    db.SaleGroups.Add(saleGroup);

                    // Thêm danh sách nhân viên
                    CreateSaleGroupUser(model.ListEmployee, saleGroup.Id);

                    // Thêm danh sách sản phẩm
                    CreateSaleGroupProduct(model.ListGroupProduct, saleGroup.Id);
                    UserLogUtil.LogHistotyAdd(db, userId, saleGroup.Name, saleGroup.Id, Constants.LOG_SaleGroup);
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
        /// Lấy thông tin nhóm kinh doanh
        /// </summary>
        /// <param name="id">id nhóm kinh doanh</param>
        /// <returns></returns>
        public SaleGroupModel GetInfoSaleGroup(string id)
        {
            var resultInfo = db.SaleGroups.AsNoTracking().Where(u => u.Id.Equals(id)).Select(p => new SaleGroupModel()
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleGroup);
            }

            resultInfo.ListEmployee = (from a in db.SaleGroupUsers.AsNoTracking()
                                       where a.SaleGroupId.Equals(id)
                                       join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                       join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                       join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                                       orderby c.Name
                                       select new EmployeeModel
                                       {
                                           Id = b.Id,
                                           Name = c.Name,
                                           ImagePath = c.ImagePath,
                                           Code = c.Code,
                                           Email = c.Email,
                                           PhoneNumber = c.PhoneNumber,
                                           DepartmentName = d.Name,
                                           Status = c.Status
                                       }).ToList();

            resultInfo.ListGroupProduct = (from a in db.SaleGroupProducts.AsNoTracking()
                                           where a.SaleGroupId.Equals(id)
                                           join b in db.SaleProducts.AsNoTracking() on a.SaleProductId equals b.Id
                                           orderby b.EName
                                           select new SaleProductModel
                                           {
                                               Id = b.Id,
                                               Name = b.EName,
                                               Code = b.Model,
                                           }).ToList();

            return resultInfo;
        }

        /// <summary>
        /// Xóa nhóm kinh doanh
        /// </summary>
        /// <param name="id">Nhóm kinh doanh</param>
        public void DeleteSaleGroup(string id, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                //var saleProduct = db.SaleProducts.AsNoTracking().Where(u => u.Id.Equals(id)).FirstOrDefault();

                //if (saleProduct != null)
                //{
                //    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SaleGroup);
                //}

                try
                {
                    var saleGroupUser = db.SaleGroupUsers.Where(u => u.SaleGroupId.Equals(id)).ToList();

                    if (saleGroupUser.Count > 0)
                    {
                        db.SaleGroupUsers.RemoveRange(saleGroupUser);
                    }

                    var saleGroupProduct = db.SaleGroupProducts.Where(u => u.SaleGroupId.Equals(id)).ToList();

                    if (saleGroupProduct.Count > 0)
                    {
                        db.SaleGroupProducts.RemoveRange(saleGroupProduct);
                    }

                    var saleProduct = db.SaleGroups.Where(u => u.Id.Equals(id)).FirstOrDefault();

                    if (saleProduct == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SaleGroup);
                    }

                    db.SaleGroups.Remove(saleProduct);
                    //var jsonBefore = AutoMapperConfig.Mapper.Map<SaleGroupModel>(saleProduct);
                    //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SaleGroup
                    //    , saleProduct.Id, saleProduct.Name, jsonBefore);
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
        /// Cập nhật nhóm kinh doanh
        /// </summary>
        /// <param name="id">id nhóm</param>
        /// <param name="model">dữ liệu cập nhật</param>
        public void UpdateSaleGroup(string id, SaleGroupCreateModel model, string userId)
        {

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var checkNameExits = db.SaleGroups.Where(a => a.Name.Trim().ToLower().Equals(model.Name.Trim().ToLower()) && a.Id != id).FirstOrDefault();
                    if (checkNameExits != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SaleGroup);
                    }

                    var saleGroup = db.SaleGroups.Where(u => u.Id.Equals(id)).FirstOrDefault();
                    //var jsonBefore = AutoMapperConfig.Mapper.Map<SaleGroupModel>(saleGroup);
                    saleGroup.Name = model.Name;
                    saleGroup.Note = model.Note;

                    // Check danh sách nhân viên
                    var saleGroupUser = db.SaleGroupUsers.Where(u => u.SaleGroupId.Equals(id)).ToList();

                    if (saleGroupUser.Count > 0)
                    {
                        db.SaleGroupUsers.RemoveRange(saleGroupUser);
                    }

                    if (model.ListEmployee.Count > 0)
                    {
                        CreateSaleGroupUser(model.ListEmployee, id);
                    }

                    // check danh sách sản phẩm
                    var saleGroupProduct = db.SaleGroupProducts.Where(u => u.SaleGroupId.Equals(id)).ToList();

                    if (saleGroupProduct.Count > 0)
                    {
                        db.SaleGroupProducts.RemoveRange(saleGroupProduct);
                    }

                    if (model.ListGroupProduct.Count > 0)
                    {
                        CreateSaleGroupProduct(model.ListGroupProduct, id);
                    }
                    //var jsonAfter = AutoMapperConfig.Mapper.Map<SaleGroupModel>(saleGroup);
                    //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_SaleGroup, saleGroup.Name, saleGroup.Id, jsonBefore, jsonAfter);
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
        /// Lấy ra danh sách nhân viên khác nhân viên được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id nhân viên được chọn lúc đầu.</param>
        /// <returns></returns>
        public List<EmployeeModel> GetListEmployee(EmployeeSearchModel searchModel)
        {
            var listEmployee = (from a in db.Users.AsNoTracking()
                                join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                                join c in db.Departments.AsNoTracking() on b.DepartmentId equals c.Id
                                join d in db.JobPositions.AsNoTracking() on b.JobPositionId equals d.Id
                                where !searchModel.ListEmployeeId.Contains(a.Id) && b.Status == 1
                                select new EmployeeModel
                                {
                                    Id = a.Id,
                                    Code = b.Code,
                                    ImagePath = b.ImagePath,
                                    Name = b.Name,
                                    DepartmentName = c.Name,
                                    Email = b.Email,
                                    PhoneNumber = b.PhoneNumber,
                                    DepartermentId = c.Id,
                                    Status = b.Status,
                                    Avatar = b.ImagePath,
                                    Position = d.Name
                                }).AsQueryable();

            if(searchModel.Status.HasValue)
            {
                listEmployee = listEmployee.Where(t => t.Status == searchModel.Status.Value);
            }    

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                listEmployee = listEmployee.Where(a => a.Name.ToLower().Contains(searchModel.Name.ToLower()) || a.Code.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                listEmployee = listEmployee.Where(a => a.Email.ToLower().Contains(searchModel.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartermentId))
            {
                listEmployee = listEmployee.Where(a => a.DepartermentId.Equals(searchModel.DepartermentId));
            }
            if (searchModel.ListEmployeeId.Count > 0)
            {
                foreach (var item in searchModel.ListEmployeeId)
                {
                    var user = db.InterviewUsers.Where(g => g.Id.Equals(item)).FirstOrDefault();
                    if (user != null)
                    {
                        listEmployee = listEmployee.Where(r => r.Id != user.UserId);
                    }
                }
            }

            return listEmployee.ToList();
        }

        /// <summary>
        /// Danh sách sản phẩm khác sản phẩm đã được chọn.
        /// </summary>
        /// <param name="searchModel">Danh sách id sản phẩm đã chọn từ trước.</param>
        /// <returns></returns>
        public List<SaleProductModel> GetListProduct(ProductSearchModel searchModel)
        {
            var listProduct = (from a in db.SaleProducts.AsNoTracking()
                                join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                                from da in ad.DefaultIfEmpty()
                                join c in db.SaleProductTypes.AsNoTracking() on a.SaleProductTypeId equals c.Id into ac
                                from acn in ac.DefaultIfEmpty()
                                where !searchModel.ListProductId.Contains(a.Id)
                                select new SaleProductModel
                               {
                                   Id = a.Id,
                                   Code = a.Model,
                                   Name = a.EName,
                                   SaleProductTypeId = acn.Id,
                                   SaleProductTypeName = acn != null ? acn.Name : string.Empty,
                                   IsChoose = a.IsChoose
                               }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                listProduct = listProduct.Where(a => a.Name.ToLower().Contains(searchModel.Name.ToLower()) || a.Code.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.SaleProductTypeId))
            {
                listProduct = listProduct.Where(a => a.SaleProductTypeId.Equals(searchModel.SaleProductTypeId));
            }

            if (searchModel.IsChoose.HasValue)
            {
                listProduct = listProduct.Where(a => a.IsChoose.Equals(searchModel.IsChoose.Value));
            }

            return listProduct.ToList();
        }

        public List<string> GetListParent(string id)
        {
            List<string> listChild = new List<string>();
            var moduleGroup = db.ProductStandardTPATypes.AsNoTracking().Where(i => id.Equals(i.ParentId)).Select(i => i.Id).ToList();
            listChild.AddRange(moduleGroup);
            if (moduleGroup.Count > 0)
            {
                foreach (var item in moduleGroup)
                {
                    listChild.AddRange(GetListParent(item));
                }
            }
            return listChild;
        }


    }
}
