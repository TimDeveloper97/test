using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTSModel = NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Utils;
using SQLHelpper = NTS.Model.SQLHelpper;
using System.Web;
using NTS.Model.GroupUser;
using NTS.Common.Resource;
using QLTK.Business.Users;
using NTS.Model.HistoryVersion;
using QLTK.Business.AutoMappers;
using NTS.Caching;
using NTS.Model.User;
using System.Configuration;

namespace QLTK.Business.GroupUser
{
    public class GroupUserBusiness
    {
        QLTKEntities db = new QLTKEntities();

        public SearchResultModel<GroupUserResultModel> SearchGroupUser(GroupUserSearchModel modelSearch)
        {
            SearchResultModel<GroupUserResultModel> searchResult = new SearchResultModel<GroupUserResultModel>();
            var dataQuery = (from a in db.GroupUsers.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                             orderby a.Name
                             select new GroupUserResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 SBUId = c.Id,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = b.Name,
                                 IsDisable = a.IsDisable,
                                 Description = a.Description
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }
            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.IsDisable.ToString()))
            {
                dataQuery = dataQuery.Where(u => u.IsDisable == modelSearch.IsDisable);
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void DeleteGroupUser(GroupUserModel model, string departmentId, string userLoginId, bool isEditOther)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var checkUser = db.Users.Where(u => u.GroupUserId.Equals(model.Id)).Count();
                if (checkUser > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.GroupUser);
                }

                try
                {
                    var groupUser = db.GroupUsers.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (groupUser == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.GroupUser);
                    }

                    if (!string.IsNullOrEmpty(groupUser.DepartmentId) && !groupUser.DepartmentId.Equals(departmentId) && !isEditOther)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0035, TextResourceKey.GroupUser);
                    }

                    var groupPermissions = db.GroupPermissions.Where(u => u.GroupUserId.Equals(model.Id)).ToList();
                    if (groupPermissions.Count > 0)
                    {
                        db.GroupPermissions.RemoveRange(groupPermissions);
                    }
                    model.Name = groupUser.Name;
                    db.GroupUsers.Remove(groupUser);


                    //var jsonBefor = AutoMapperConfig.Mapper.Map<GroupUserHistoryModel>(groupUser);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_GroupUser, groupUser.Id, groupUser.Name, jsonBefor);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            //luu Log lich su
            string decription = "Xóa nhóm quyền tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        public void CreateGroupUser(GroupUserModel model)
        {
            if (db.GroupUsers.AsNoTracking().Where(o => o.Name.Equals(model.Name.Trim())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.GroupUser);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTSModel.GroupUser groupUser = new NTSModel.GroupUser();
                    groupUser.Id = Guid.NewGuid().ToString();
                    groupUser.Name = model.Name;
                    groupUser.DepartmentId = model.DepartmentId;
                    groupUser.Description = model.Description;
                    groupUser.IsDisable = int.Parse(model.IsDisable);
                    groupUser.CreateBy = model.CreateBy;
                    groupUser.UpdateBy = model.CreateBy;
                    groupUser.CreateDate = DateTime.Now;
                    groupUser.UpdateDate = groupUser.CreateDate;
                    db.GroupUsers.Add(groupUser);
                    #region[thêm vào bảng con]
                    if (model.ListPermission.Count > 0)
                    {
                        GroupPermission itemPermission;
                        foreach (var item in model.ListPermission.Where(u => u.Checked == true && string.IsNullOrEmpty(u.Index)))
                        {
                            itemPermission = new GroupPermission();
                            itemPermission.Id = Guid.NewGuid().ToString();
                            itemPermission.GroupUserId = groupUser.Id;
                            itemPermission.PermissionId = item.FunctionId;
                            db.GroupPermissions.Add(itemPermission);
                        }
                    }
                    #endregion

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, groupUser.Name, groupUser.Id, Constants.LOG_GroupUser);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            //luu Log lich su
            string decription = "Thêm nhóm quyền tên là: " + model.Name;
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateGroupUser(GroupUserModel model, string departmentId, bool isEditOther)
        {
            if (db.GroupUsers.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.GroupUser);
            }

            string nameOld = "";
            using (var trans = db.Database.BeginTransaction())
            {
                var groupUser = db.GroupUsers.FirstOrDefault(u => u.Id.Equals(model.Id));

                //var jsonApter = AutoMapperConfig.Mapper.Map<GroupUserHistoryModel>(groupUser);

                if (groupUser == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.GroupUser);
                }

                if (!string.IsNullOrEmpty(groupUser.DepartmentId) && !groupUser.DepartmentId.Equals(departmentId) && !isEditOther)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.GroupUser);
                }

                try
                {
                    nameOld = groupUser.Name;
                    groupUser.Name = model.Name;
                    groupUser.DepartmentId = model.DepartmentId;
                    groupUser.Description = model.Description;
                    groupUser.IsDisable = int.Parse(model.IsDisable);
                    groupUser.UpdateBy = model.UpdateBy;
                    groupUser.UpdateDate = DateTime.Now;
                    //xóa quyền cũ
                    var groupPermissionOld = db.GroupPermissions.Where(u => u.GroupUserId.Equals(groupUser.Id));
                    if (groupPermissionOld.Count() > 0)
                    {
                        db.GroupPermissions.RemoveRange(groupPermissionOld);
                    }

                    List<string> permissionIds = new List<string>();
                    #region[Thêm vào bảng con]
                    if (model.ListPermission.Count > 0)
                    {
                        GroupPermission itemPermission;
                        foreach (var item in model.ListPermission.Where(u => u.Checked == true && string.IsNullOrEmpty(u.Index)))
                        {
                            itemPermission = new GroupPermission();
                            itemPermission.Id = Guid.NewGuid().ToString();
                            itemPermission.GroupUserId = groupUser.Id;
                            itemPermission.PermissionId = item.FunctionId;
                            db.GroupPermissions.Add(itemPermission);

                            permissionIds.Add(item.FunctionId);
                        }
                    }
                    #endregion

                    if (model.IsUpdateUser)
                    {
                        db.UserPermissions.RemoveRange((from u in db.Users
                                                        join p in db.UserPermissions on u.Id equals p.UserId
                                                        where groupUser.Id.Equals(u.GroupUserId)
                                                        select p));

                        var users = db.Users.Where(r => groupUser.Id.Equals(r.GroupUserId)).ToList();

                        List<UserPermission> userPermissions = new List<UserPermission>();
                        RedisService<UserModel> redisService = RedisService<UserModel>.GetInstance();
                        string keyLoginInfo;
                        foreach (var user in users)
                        {
                            foreach (var permissionId in permissionIds)
                            {
                                userPermissions.Add(new UserPermission
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionId = permissionId,
                                    UserId = user.Id
                                });
                            }

                            // Key cache user đăng nhập
                            keyLoginInfo = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheLoginKey"] + user.Id;
                            // Xóa cache
                            redisService.Remove(keyLoginInfo);
                        }

                        db.UserPermissions.AddRange(userPermissions);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<GroupUserHistoryModel>(groupUser);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_GroupUser, groupUser.Id, groupUser.Name, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            //luu Log lich su
            string decription = "Cập nhật nhóm quyền tên là: " + model.Name;
            if (!nameOld.Equals(model.Name))
            {
                decription = "Cập nhật nhóm quyền tên là: " + nameOld + " thành " + model.Name;
            }
            LogBusiness.SaveLogEvent(db, model.CreateBy, decription);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public GroupUserModel GetGroupUserInfo(GroupUserModel model)
        {
            try
            {
                var groupUserModel = db.GroupUsers.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.Id));
                model = new GroupUserModel();
                var lstGroupPermissionsId = new List<string>();
                if (groupUserModel != null)
                {
                    model.Id = groupUserModel.Id;
                    model.Name = groupUserModel.Name;
                    model.Description = groupUserModel.Description;
                    model.IsDisable = groupUserModel.IsDisable.ToString();
                    model.DepartmentId = groupUserModel.DepartmentId;
                    var department = db.Departments.AsNoTracking().FirstOrDefault(a => a.Id.Equals(model.DepartmentId));

                    if (department != null)
                    {
                        model.SBUId = !string.IsNullOrEmpty(department.SBUId) ? department.SBUId : string.Empty;
                    }

                    lstGroupPermissionsId = db.GroupPermissions.AsNoTracking().Where(u => u.GroupUserId.Equals(groupUserModel.Id)).Select(u => u.PermissionId).ToList();
                }
                else
                {
                    model.Name = "";
                    model.HomeURL = "";
                    model.Description = "";
                    model.IsDisable = "1";
                    model.DepartmentId = "";
                    model.SBUId = string.Empty;
                }

                model.ListPermission = GetListGroupPermissions(lstGroupPermissionsId);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
            return model;
        }
        public List<PermissionModel> GetListGroupPermissions(List<string> lstCheck)
        {
            List<PermissionModel> lst = new List<PermissionModel>();
            try
            {
                var lstPermissions = db.Permissions.AsNoTracking().OrderBy(o => o.Index).ToList();
                List<Permission> lstPermissionsTemp;
                PermissionModel permissionModel;
                var group = db.GroupFunctions.AsNoTracking().OrderBy(u => u.Index).ToList();
                for (int i = 0; i < group.Count; i++)
                {
                    permissionModel = new PermissionModel();
                    permissionModel.Index = (i + 1).ToString();
                    permissionModel.FunctionCode = group[i].Code;
                    permissionModel.FunctionName = group[i].Name;
                    permissionModel.GroupFunctionId = "";
                    permissionModel.FunctionId = group[i].Id;
                    permissionModel.Checked = false;
                    lst.Add(permissionModel);
                    lstPermissionsTemp = lstPermissions.Where(u => u.GroupFunctionId.Equals(group[i].Id)).ToList();
                    foreach (var item in lstPermissionsTemp)
                    {
                        permissionModel = new PermissionModel();
                        permissionModel.Index = "";
                        permissionModel.FunctionCode = item.Code;
                        permissionModel.FunctionName = item.Name;
                        permissionModel.GroupFunctionId = group[i].Id;
                        permissionModel.FunctionId = item.Id;
                        permissionModel.Checked = lstCheck.Contains(item.Id);
                        lst.Add(permissionModel);
                    }
                }
            }
            catch (Exception)
            { }
            return lst;
        }

    }
}
