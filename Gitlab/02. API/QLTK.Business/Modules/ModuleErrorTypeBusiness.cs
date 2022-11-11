using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ModuleError;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Utils;
using SQLHelpper = NTS.Model.SQLHelpper;
using System.Web;
using System.Net.Http;
using NTS.Model.Common;
using Newtonsoft.Json;
using NTS.Model.GroupUser;
using Syncfusion.XlsIO;
using NTSModel = NTS.Model.Repositories;

namespace QLTK.Business.ModuleErrorType
{
    public class ModuleErrorTypeBusiness
    {
        QLTKEntities db = new QLTKEntities();
        string SecretKey = System.Configuration.ConfigurationManager.AppSettings["SecretKey"];
        string ApiFile = System.Configuration.ConfigurationManager.AppSettings["ApiFile"];
        public SearchResultModel<TreeNode<ModuleErrorTypeModel>> SearchModuleErrorType(string type)
        {
            SearchResultModel<TreeNode<ModuleErrorTypeModel>> searchResult = new SearchResultModel<TreeNode<ModuleErrorTypeModel>>();
            try
            {
                var listGroup = (from a in db.ErrorGroups.AsNoTracking()
                                 where a.Type.Equals(type)
                                 select new ModuleErrorTypeModel
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     Name = a.Name,
                                     Type = a.Type,
                                     Description = a.Description,
                                     CreateBy = a.CreateBy,
                                 }).ToList();

                var listParentGroup = from r in listGroup
                                      where string.IsNullOrEmpty(r.GroupId)
                                      select r;

                TreeNode<ModuleErrorTypeModel> parentNode;
                TreeNode<ModuleErrorTypeModel> childNode;
                List<TreeNode<ModuleErrorTypeModel>> listChild;
                foreach (var groupModule in listParentGroup)
                {
                    parentNode = new TreeNode<ModuleErrorTypeModel>();
                    parentNode.data = groupModule;

                    var listChilden = from r in listGroup
                                      where r.GroupId.Equals(groupModule.Id)
                                      select r;
                    listChild = new List<TreeNode<ModuleErrorTypeModel>>();
                    foreach (var item in listChilden)
                    {
                        childNode = new TreeNode<ModuleErrorTypeModel>();

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
                throw new Exception(ErrorMessage.ERR001);
            }
            return searchResult;
        }
        public void CreateModuleErrorType(ModuleErrorTypeModel model)
        {
            if (string.IsNullOrEmpty(model.GroupId))
            {
                var checkExits = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Type == model.Type && u.Name.ToLower().Equals(model.Name.ToLower()));
                if (checkExits != null)
                {
                    throw new Exception("Tên nhóm lỗi đã tồn tại");
                }
            }
            else
            {
                var checkExits = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Type == model.Type && u.Name.ToLower().Equals(model.Name.ToLower()));
                if (checkExits != null)
                {
                    throw new Exception("Tên lỗi đã tồn tại trong nhóm");
                }
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var data = new NTSModel.ErrorGroup();
                    data.Id = Guid.NewGuid().ToString();
                    data.Code = model.Code;
                    data.Name = model.Name;
                    data.Type = model.Type;
                    data.Description = model.Description;
                    data.CreateBy = model.CreateBy;
                    data.UpdateBy = model.CreateBy;
                    data.CreateDate = DateTime.Now;
                    data.UpdateDate = data.CreateDate;
                    db.ErrorGroups.Add(data);

                    db.SaveChanges();
                    trans.Commit();
                    //xóa cache
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new ErrorException(ErrorMessage.ERR001, ex.InnerException);
                }
            }
            //luu Log lich su
            string decription = "Thêm loại lỗi tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateModuleErrorType(ModuleErrorTypeModel model)
        {
            string nameOld = "";
            var data = db.ErrorGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (data == null)
            {
                throw new Exception(ErrorMessage.ERR007);
            }
            if (!string.IsNullOrEmpty(model.GroupId))
            {
                if (model.Id.Equals(model.GroupId))
                {
                    throw new Exception("Không thể chọn chính mình làm nhóm cha");
                }
                var checkChild = db.ErrorGroups.AsNoTracking().Where(u => u.Type == model.Type).Count();
                if (checkChild > 0)
                {
                    throw new Exception("Nhóm đang chứa con không thể làm con của nhóm khác");
                }

                var checkExits = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id) && u.Type == model.Type && u.Name.ToLower().Equals(model.Name.ToLower()));
                if (checkExits != null)
                {
                    throw new Exception("Tên lỗi đã tồn tại trong nhóm");
                }
            }
            else
            {
                var checkExits = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id) && u.Type == model.Type && u.Name.ToLower().Equals(model.Name.ToLower()));
                if (checkExits != null)
                {
                    throw new Exception("Tên lỗi đã tồn tại trong nhóm");
                }
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    nameOld = data.Name;
                    data.Code = model.Code;
                    data.Name = model.Name;
                    data.Type = model.Type;
                    data.Description = model.Description;

                    data.UpdateBy = model.CreateBy;
                    data.UpdateDate = DateTime.Now;

                    db.SaveChanges();
                    trans.Commit();
                    //xóa cache
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new ErrorException(ErrorMessage.ERR001, ex.InnerException);
                }
            }
            //luu Log lich su
            string decription = "Cập nhật loại lỗi tên là: " + model.Name;
            if (!nameOld.Equals(model.Code))
            {
                decription = "Cập nhật loại lỗi tên là: " + nameOld + " thành " + model.Name;
            }
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ModuleErrorTypeModel GetModuleErrorTypeInfo(ModuleErrorTypeModel model)
        {
            ModuleErrorTypeModel data = new ModuleErrorTypeModel();
            var moduleErrors = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
            if (moduleErrors == null)
            {
                throw new Exception(ErrorMessage.ERR007);
            }
            try
            {
                data.Id = model.Id;
                data.Code = moduleErrors.Code;
                data.Name = moduleErrors.Name;
                data.Description = moduleErrors.Description;
                data.Type = moduleErrors.Type;
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001);
            }
            return data;
        }

        public void DeleteModuleErrorType(ModuleErrorTypeModel model)
        {
            var data = db.ErrorGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (data == null)
            {
                throw new Exception(ErrorMessage.ERR007);
            }
            model.Code = data.Name;
            //xóa con trước
            var ModuleErrorsOld = db.Errors.AsNoTracking().Where(u => u.ModuleErrorVisualId.Equals(model.Id) || u.ModuleErrorReasonId.Equals(model.Id) || u.ModuleErrorCostId.Equals(model.Id));
            if (ModuleErrorsOld.Count() > 0)
            {
                throw new Exception("Nhóm lỗi đã được sử dụng không thể xóa");
            }
            try
            {
                db.ErrorGroups.Remove(data);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ErrorException(ErrorMessage.ERR001, ex.InnerException);
            }
            //luu Log lich su
            string decription = "Xóa loại lỗi tên là: " + model.Code;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }
    }
}
