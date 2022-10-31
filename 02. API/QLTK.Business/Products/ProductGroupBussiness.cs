using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.ProductGroup;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ProductGroups
{
    public class ProductGroupBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<TreeNode<ProductGroupModel>> SearchProductGroupModel(ProductGroupSearchModel modelSearch)
        {
            SearchResultModel<TreeNode<ProductGroupModel>> searchResult = new SearchResultModel<TreeNode<ProductGroupModel>>();
            try
            {
                var listGroup = (from a in db.ProductGroups.AsNoTracking()
                                 select new ProductGroupModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     ParentId = a.ParentId,
                                 }).ToList();

                var listParentGroup = from r in listGroup
                                      where string.IsNullOrEmpty(r.ParentId)
                                      select r;

                TreeNode<ProductGroupModel> parentNode;
                TreeNode<ProductGroupModel> childNode;
                List<TreeNode<ProductGroupModel>> listChild;
                foreach (var groupModule in listParentGroup)
                {
                    parentNode = new TreeNode<ProductGroupModel>();
                    parentNode.data = groupModule;

                    var listChilden = from r in listGroup
                                      where r.ParentId.Equals(groupModule.Id)
                                      select r;
                    listChild = new List<TreeNode<ProductGroupModel>>();
                    foreach (var item in listChilden)
                    {
                        childNode = new TreeNode<ProductGroupModel>();

                        childNode.data = item;
                        listChild.Add(childNode);
                    }

                    parentNode.children = listChild;
                    searchResult.ListResult.Add(parentNode);

                }
                searchResult.TotalItem = listGroup.Count();
            }
            catch (Exception ex)
            {
                throw new Exception("QLTK.List");
            }

            return searchResult;
        }

        public SearchResultModel<ProductGroupModel> GetListProductGroup(ProductGroupSearchModel modelSearch)
        {
            SearchResultModel<ProductGroupModel> searchResult = new SearchResultModel<ProductGroupModel>();
            try
            {
                var listModuleGroup = (from o in db.ProductGroups.AsNoTracking()
                                       orderby o.Code
                                       select new ProductGroupModel
                                       {
                                           Id = o.Id,
                                           Code = o.Code,
                                           Description = o.Description,
                                           Name = o.Name,
                                           ParentId = o.ParentId,
                                           CreateBy = o.CreateBy,
                                           CreateDate = o.CreateDate,
                                           UpdateBy = o.UpdateBy,
                                           UpdateDate = o.UpdateDate,
                                       }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Id))
                {
                    listModuleGroup = listModuleGroup.Where(u => u.Id.ToUpper().Contains(modelSearch.Id.ToUpper()));
                }

                List<ProductGroupModel> listResult = new List<ProductGroupModel>();
                var listParent = listModuleGroup.ToList().Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                bool isSearch = false;
                int index = 1;

                List<ProductGroupModel> listChild = new List<ProductGroupModel>();
                foreach (var parent in listParent)
                {
                    isSearch = true;
                    if (!string.IsNullOrEmpty(modelSearch.Name) && !parent.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                    {
                        isSearch = false;
                    }
                    if (!string.IsNullOrEmpty(modelSearch.Code) && !parent.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                    {
                        isSearch = false;
                    }

                    listChild = GetProductGroupChild(parent.Id, listModuleGroup.ToList(), modelSearch, index.ToString());
                    if (isSearch || listChild.Count > 0)
                    {
                        parent.Index = index.ToString();
                        listResult.Add(parent);
                        index++;
                    }

                    listResult.AddRange(listChild);
                }
                searchResult.TotalItem = listResult.Count();
                //listResult = listResult.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return searchResult;
        }

        private List<ProductGroupModel> GetProductGroupChild(string parentId,
          List<ProductGroupModel> listProductGroup, ProductGroupSearchModel modelSearch, string index)
        {
            List<ProductGroupModel> listResult = new List<ProductGroupModel>();
            var listChild = listProductGroup.Where(r => parentId.Equals(r.ParentId)).ToList();
            bool isSearch = false;
            int indexChild = 1;
            List<ProductGroupModel> listChildChild = new List<ProductGroupModel>();

            foreach (var child in listChild)
            {
                isSearch = true;
                if (!string.IsNullOrEmpty(modelSearch.Name) && !child.Name.ToLower().Contains(modelSearch.Name.ToLower()))
                {
                    isSearch = false;
                }
                if (!string.IsNullOrEmpty(modelSearch.Code) && !child.Code.ToLower().Contains(modelSearch.Code.ToLower()))
                {
                    isSearch = false;
                }

                listChildChild = GetProductGroupChild(child.Id, listProductGroup, modelSearch, index + "." + indexChild);
                if (isSearch || listChildChild.Count > 0)
                {
                    child.Index = index + "." + indexChild;
                    listResult.Add(child);
                    indexChild++;
                }
                listResult.AddRange(listChildChild);
            }
            return listResult;
        }

        public void DeleteProductGroup(ProductGroupModel model)
        {
            var productGroup = db.ProductGroups.Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
            if (productGroup == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductGroup);
            }

            List<ProductGroupModel> listCheck = GetProductGroupById(model.Id);
            if (listCheck.Count > 0)
            {
                foreach (var item in listCheck)
                {
                    var productGroupCheck = db.Products.AsNoTracking().Where(m => m.ProductGroupId.Equals(item.Id)).FirstOrDefault();
                    if (productGroupCheck != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.ProductGroup);
                    }
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in listCheck)
                    {
                        var deleteProductGroup = db.ProductGroups.Where(m => m.Id.Equals(item.Id)).FirstOrDefault();
                        db.ProductGroups.Remove(deleteProductGroup);
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. ");
                }
            }
            //luu Log lich su
            //string decription = "Xóa nhóm modul tên là: " + model.Name;
            //LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        public void AddProductGroup(ProductGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.ProductGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductGroup);
                }
                if (db.ProductGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductGroup);
                }
                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                try
                {
                    var dateNow = DateTime.Now;
                    ProductGroup newProductGroup = new ProductGroup()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Code = model.Code,
                        ParentId = model.ParentId,
                        Description = model.Description,
                        CreateBy = model.CreateBy,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        UpdateBy = model.CreateBy
                    };
                    db.ProductGroups.Add(newProductGroup);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
            try
            {
                //luu Log lich su
                string decription = String.Empty;
                decription = "Thêm nhóm thiết bị có tên là: " + model.Name;
                LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
            }
            catch (Exception) { }

        }

        public ProductGroupModel GetProductGroupInfo(ProductGroupModel model)
        {
            var productGroupInfo = db.ProductGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
            if (productGroupInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductGroup);
            }
            try
            {
                model.Id = productGroupInfo.Id;
                model.Name = productGroupInfo.Name;
                model.Code = productGroupInfo.Code;
                model.Description = productGroupInfo.Description;
                model.ParentId = productGroupInfo.ParentId;
                model.CreateBy = productGroupInfo.CreateBy;
                model.CreateDate = productGroupInfo.CreateDate;
                model.UpdateBy = productGroupInfo.UpdateBy;
                model.UpdateDate = productGroupInfo.UpdateDate;              

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateProductGroup(ProductGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            using (var trans = db.Database.BeginTransaction())
            {
                var moduleGroupInfo = db.ProductGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (moduleGroupInfo == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductGroup);
                }
                if (db.ProductGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Name.Equals(model.Name))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.ProductGroup);
                }

                if (db.ProductGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && (o.Code.Equals(model.Code))).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.ProductGroup);
                }
                if (string.IsNullOrEmpty(model.ParentId))
                {
                    model.ParentId = null;
                }

                try
                {
                    //cập nhật nhóm thiết bị
                    moduleGroupInfo.Code = model.Code;
                    moduleGroupInfo.Name = model.Name;
                    moduleGroupInfo.Description = model.Description;
                    moduleGroupInfo.ParentId = model.ParentId;
                    moduleGroupInfo.UpdateBy = model.CreateBy;
                    moduleGroupInfo.UpdateDate = DateTime.Now;
                    moduleGroupInfo.CreateBy = model.CreateBy; 

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý ");
                }
            }
        }

        //Tìm kiếm nhóm thiết bị theo Id
        public List<ProductGroupModel> GetProductGroupById(string Id)
        {
            List<ProductGroupModel> search = new List<ProductGroupModel>();
            List<ProductGroupModel> searchResult = new List<ProductGroupModel>();
            try
            {
                var listProductGroup = (from o in db.ProductGroups.AsNoTracking().OrderBy(r => r.Name)
                                        select new ProductGroupModel
                                        {
                                            Id = o.Id,
                                            Name = o.Name,
                                            ParentId = o.ParentId,
                                            CreateBy = o.CreateBy,
                                            CreateDate = o.CreateDate,
                                            UpdateBy = o.UpdateBy,
                                            UpdateDate = o.UpdateDate,
                                        }).AsQueryable();
                List<ProductGroupModel> listResult = new List<ProductGroupModel>();
                var listParent = listProductGroup.ToList().Where(r => r.Id.Equals(Id)).ToList();
                List<ProductGroupModel> listChild = new List<ProductGroupModel>();
                foreach (var parent in listParent)
                {
                    listChild = GetProductGroupChild(parent.Id, listProductGroup.ToList());
                    if (listChild.Count >= 0)
                    {
                        listResult.Add(parent);

                    }
                    listResult.AddRange(listChild);
                }
                search = listResult;
            }
            catch (Exception ex)
            {

                throw new Exception("Có lỗi trong quá trình xử lý");
            }
            return search.ToList();
        }
        
        private List<ProductGroupModel> GetProductGroupChild(string parentId, List<ProductGroupModel> listDocumentTemplateType)
        {
            List<ProductGroupModel> listResult = new List<ProductGroupModel>();
            var listChild = listDocumentTemplateType.Where(r => parentId.Equals(r.ParentId)).ToList();

            bool isSearch = false;
            List<ProductGroupModel> listChildChild = new List<ProductGroupModel>();
            foreach (var child in listChild)
            {
                isSearch = true;

                listChildChild = GetProductGroupChild(child.Id, listDocumentTemplateType);
                if (isSearch || listChildChild.Count > 0)
                {
                    listResult.Add(child);
                }

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }
        
        public List<ProductGroupModel> GetProductGroupExcepted(ProductGroupSearchModel modelSearch)
        {
            List<ProductGroupModel> listExpected = new List<ProductGroupModel>();
            List<ProductGroupModel> listAll = new List<ProductGroupModel>();
            ProductGroupSearchModel modelAll = new ProductGroupSearchModel();
            listAll = GetListProductGroup(modelAll).ListResult;

            List<ProductGroupModel> listById = new List<ProductGroupModel>();
            listById = GetProductGroupById(modelSearch.Id);

            foreach (var itemAll in listAll)
            {
                foreach (var itemById in listById)
                {
                    if (itemAll.Id.Equals(itemById.Id))
                    {
                        listExpected.Add(itemAll);
                    }
                }
            }
            return listAll.Except(listExpected).ToList();
        }

    }
}
