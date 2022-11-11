using NTS.Caching;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.GroupFunction;
using NTS.Model.Permission;
using NTS.Model.Repositories;
using NTS.Model.User;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace QLTK.Business.Users
{
    public class UserBusiness
    {
        private QLTKEntities db = new QLTKEntities();        

        public void Create(UserModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Users.AsNoTracking().Where(o => o.UserName.Equals(model.UserName)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.User);
                }
                try
                {
                    var dateNow = DateTime.Now;
                    User adduser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName.Trim(),
                        EmployeeId = model.EmployeeId,
                        GroupUserId = model.GroupUserId,
                        IsDisable = model.IsDisable,
                        IsLogin = false,
                        SecurityStamp = PasswordUtils.CreatePasswordHash(),
                        HomeURL = string.Empty,
                        CreateDate = dateNow,
                        UpdateDate = dateNow,
                        CreateBy = model.CreateBy,
                        UpdateBy = model.UpdateBy,
                    };

                    adduser.PasswordHash = PasswordUtils.ComputeHash(Constants.DEFAULT_PASSWORD + adduser.SecurityStamp);
                    db.Users.Add(adduser);

                    List<UserPermission> userPermissions = new List<UserPermission>();
                    UserPermission addPer;
                    foreach (var item in model.ListGroupFunction)
                    {
                        foreach (var per in item.Permissions)
                        {
                            if (per.IsChecked)
                            {
                                addPer = new UserPermission
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    UserId = adduser.Id,
                                    PermissionId = per.Id,
                                };
                                userPermissions.Add(addPer);
                            }
                        }
                    }

                    db.UserPermissions.AddRange(userPermissions);

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

        public void UpdateUser(UserModel model)
        {
            // kiểm tra tên tài khoản
            if (db.Users.AsNoTracking().Where(o => !o.EmployeeId.Equals(model.EmployeeId) && (o.UserName.Equals(model.UserName))).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.User);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();
                    user.UserName = model.UserName.Trim();
                    user.IsDisable = model.IsDisable;
                    user.HomeURL = string.Empty;
                    user.GroupUserId = model.GroupUserId;
                    user.CreateBy = model.CreateBy;
                    user.CreateDate = DateTime.Now;
                    user.UpdateBy = model.UpdateBy;
                    user.UpdateDate = DateTime.Now;

                    // update bảng quyền
                    var listSpecification = db.UserPermissions.Where(a => a.UserId.Equals(user.Id)).ToList();
                    if (listSpecification.Count > 0)
                    {
                        db.UserPermissions.RemoveRange(listSpecification);
                    }

                    List<UserPermission> userPermissions = new List<UserPermission>();
                    UserPermission addPer;
                    foreach (var item in model.ListGroupFunction)
                    {
                        foreach (var per in item.Permissions)
                        {
                            if (per.IsChecked)
                            {
                                addPer = new UserPermission
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    UserId = user.Id,
                                    PermissionId = per.Id,
                                };

                                userPermissions.Add(addPer);
                            }
                        }
                    }

                    RedisService<UserModel> redisService = RedisService<UserModel>.GetInstance();
                    //tên key cache sẽ có dang 'VP:LoginInfo:123'
                    string keyLoginInfo = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheLoginKey"] + user.Id;
                    //lấy xem có cache cũ ko
                    //var cacheCheck = redisService.Get<UserEntity>(keyLoginInfo);
                    //ghi đè lại tất cả các thông tin 
                    redisService.Remove(keyLoginInfo);

                    db.UserPermissions.AddRange(userPermissions);
                  
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

        public UserModel GetUserInfo(UserModel model)
        {
            var resuldInfor = db.Users.AsNoTracking().Where(u => model.EmployeeId.Equals(u.EmployeeId)).Select(p => new UserModel
            {
                Id = p.Id,
                EmployeeId = p.EmployeeId,
                UserName = p.UserName,
                IsDisable = p.IsDisable,
                GroupUserId = p.GroupUserId,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate,
            }).FirstOrDefault();

            if (resuldInfor == null)
            {
                resuldInfor = new UserModel();
            }

            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();

            var gFunctions = db.GroupFunctions.AsNoTracking().OrderBy(o=>o.Index).ToList();

            var userPermissions = (from u in db.UserPermissions.AsNoTracking()
                                   where u.UserId.Equals(resuldInfor.Id)
                                   select u.PermissionId).ToList();

            var permissions = (from p in db.Permissions.AsEnumerable()
                               join u in userPermissions on p.Id equals u into pu
                               from pun in pu.DefaultIfEmpty()
                               orderby p.Index
                               select new PermissionsModel
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Code = p.Code,
                                   IsChecked = pun != null ? true : false,
                                   GroupFunctionId = p.GroupFunctionId,
                                   ScreenCode = p.ScreenCode
                               }).ToList();

            GroupFunctionModel paramModel = new GroupFunctionModel();
            foreach (var ite in gFunctions)
            {
                paramModel = new GroupFunctionModel();
                paramModel.Id = ite.Id;
                paramModel.Name = ite.Name;
                paramModel.Permissions = permissions.Where(t => t.GroupFunctionId.Equals(ite.Id)).ToList();
                paramModel.PermissionTotal = paramModel.Permissions.Count;
                paramModel.IsChecked = paramModel.Permissions.Count(r => !r.IsChecked) == 0;
                paramModel.CheckCount = paramModel.Permissions.Count(r => r.IsChecked);
                groupFunctions.Add(paramModel);
            }

            resuldInfor.ListGroupFunction = groupFunctions;

            return resuldInfor;
        }

        public void ChangePassword(UserModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var user = db.Users.FirstOrDefault(r => r.Id.Equals(model.Id));

                if (user == null)
                {
                    throw new Exception(ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.User));
                }

                var securityStamp = PasswordUtils.ComputeHash(model.OldPassword + user.SecurityStamp);
                if (!user.PasswordHash.Equals(securityStamp))
                {
                    throw new Exception(ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0052));
                }

                try
                {
                    user.SecurityStamp = PasswordUtils.CreatePasswordHash();
                    user.PasswordHash = PasswordUtils.ComputeHash(model.NewPassword + user.SecurityStamp);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ResourceUtil.GetResourcesNoLag(ErrorResourceKey.ERR0001));
                }
            }
        }

        public void ResetPassword(string Id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var user = db.Users.FirstOrDefault(r => r.EmployeeId.Equals(Id));

                if (user == null)
                {
                    throw new Exception(ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, TextResourceKey.User));
                }

                try
                {
                    user.SecurityStamp = PasswordUtils.CreatePasswordHash();
                    user.PasswordHash = PasswordUtils.ComputeHash(Constants.DEFAULT_PASSWORD + user.SecurityStamp);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw new Exception(ResourceUtil.GetResourcesNoLag(ErrorResourceKey.ERR0001));
                }
            }
        }

        /// <summary>
        /// Lấy quyền của nhóm
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        public List<PermissionsModel> GetGroupPermission(string groupUserId)
        {
            var result = db.GroupPermissions.AsNoTracking().Where(r => r.GroupUserId.Equals(groupUserId)).Select(
                s => new PermissionsModel
                {
                    Id = s.PermissionId
                }).ToList();

            return result;
        }
    }

}
