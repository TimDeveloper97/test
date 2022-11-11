using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model;
using NTS.Model.Repositories;
using NTS.Common;
using NTS.Caching;
using System.Configuration;
using NTS.Model.Entities;
using NTS.Model.EMSST;
using NTS.Utils;
using NTS.Model.CacheModel;
using NTS.Model.User;
using NTS.Common.Logs;

namespace NTS.Business
{

    public class AuthenBussiness
    {
        string _RedisConnection = ConfigurationManager.AppSettings["RedisConnection"];
        QLTKEntities db = new QLTKEntities();
        public Client FindClients(string clientId)
        {
            try
            {
                var client = (from a in db.ApplicationClients.AsNoTracking()
                              where a.Id.Equals(clientId)
                              select new Client
                              {
                                  Id = a.Id,
                                  SecretKey = a.SecretKey,
                                  Name = a.Name,
                                  ApplicationType = a.ApplicationType,
                                  Active = a.Active,
                                  RefreshTokenLifeTime = a.RefreshTokenLifeTime,
                                  AllowedOrigin = a.AllowedOrigin
                              }).FirstOrDefault();
                return client;

            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                return null;
            }
        }

        public EMSST1200UserInfoModel GetUserLogin(string userName)
        {
            EMSST1200UserInfoModel userLogin = new EMSST1200UserInfoModel();
            try
            {
                userLogin = (from a in db.Users.AsNoTracking()
                             where a.UserName.Equals(userName)
                             join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                             join d in db.Departments.AsNoTracking() on b.DepartmentId equals d.Id
                             join s in db.SBUs.AsNoTracking() on d.SBUId equals s.Id
                             select new EMSST1200UserInfoModel
                             {
                                 UserId = a.Id,
                                 PasswordHash = a.PasswordHash,
                                 UserName = a.UserName,
                                 EmployeId = b.Id,
                                 DepartmentId = b.DepartmentId,
                                 IsDisable = a.IsDisable,
                                 SecurityStamp = a.SecurityStamp,
                                 EmployeeName = b.Name,
                                 ImagePath = b.ImagePath,
                                 Email = b.Email,
                                 SBUId = d.SBUId,
                                 SBUName = s.Name,
                                 DepartmentName = d.Name,
                                 EmployeeCode = b.Code,
                                 Status = b.Status
                             }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            return userLogin;
        }

        public LoginEntity Login(string userName, string password, string clientId, string notifyToken)
        {
            LoginEntity loginEntity = new LoginEntity();
            try
            {
                AuthenBussiness _authen = new AuthenBussiness();
                //thay mới = entity
                EMSST1200UserInfoModel userLogin = _authen.GetUserLogin(userName);
                if (userLogin != null)
                {
                    if (userLogin.IsDisable == Constants.STATUS_LOCK)
                    {
                        //Tài khoản bị khóa. Lên hệ quản trị để kích hoạt lại
                        loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_LOCK;
                    }
                    else if (userLogin.Status == Constants.STATUS_NOTWORKING)
                    {
                        loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_LOCK;
                    }
                    else
                    {
                        var securityStamp = PasswordUtils.ComputeHash(password + userLogin.SecurityStamp);
                        if (userLogin.PasswordHash.Equals(securityStamp))
                        {
                            UserEntity userEntity = new UserEntity();
                            userEntity.SecurityKey = Guid.NewGuid().ToString();
                            userEntity.FullName = userLogin.EmployeeName;
                            userEntity.ImageLink = userLogin.ImagePath;
                            userEntity.EmployeeId = userLogin.EmployeId;
                            userEntity.DepartmentId = userLogin.DepartmentId;
                            userEntity.UserId = userLogin.UserId;
                            userEntity.UserName = userLogin.UserName;
                            userEntity.Email = userLogin.Email;
                            userEntity.HomeUrl = userLogin.HomeUrl;
                            userEntity.SBUId = userLogin.SBUId;
                            userEntity.SBUName = userLogin.SBUName;
                            userEntity.DepartmentName = userLogin.DepartmentName;
                            userEntity.EmployeeName = userLogin.EmployeeName;
                            userEntity.EmployeeCode = userLogin.EmployeeCode;
                            userEntity.ListPermission = new List<string>();
                            userEntity.IsLogin = true;
                            //thay mới = entity
                            userEntity.ListPermission = GetListPermission(userLogin.UserId);
                            userEntity.ListSaleGroups = GetSaleGroup(userLogin.UserId);
                            loginEntity.UserInfor = userEntity;
                            using (var trans = db.Database.BeginTransaction())
                            {
                                try
                                {
                                    var user = db.Users.AsQueryable().Where(o => o.UserName.Equals(userName)).FirstOrDefault();
                                    user.IsLogin = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                    throw new NTSLogException(null, ex);
                                }
                            }

                            //Luu cache thong tin dang nhap
                            RedisService<UserModel> redisService = RedisService<UserModel>.GetInstance();
                            //tên key cache sẽ có dang 'VP:LoginInfo:123'
                            string keyLoginInfo = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheLoginKey"] + userLogin.UserId;
                            //lấy xem có cache cũ ko
                            var cacheCheck = redisService.Get<UserEntity>(keyLoginInfo);
                            if (cacheCheck != null)
                            {
                                //nếu có thì giữ lại key cũ để ko bị logout
                                userEntity.SecurityKey = cacheCheck.SecurityKey;
                            }

                            //ghi đè lại tất cả các thông tin 
                            redisService.Remove(keyLoginInfo);
                            redisService.Add(keyLoginInfo, userEntity);

                            if (!string.IsNullOrEmpty(notifyToken))
                            {
                                //Luu cache thong tin token notify cho nguoi dung
                                RedisService<string> redisNotifyService = RedisService<string>.GetInstance();
                                //tên key cache sẽ có dang 'VMS_Dev:Notify:123'
                                string keyNotify = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheNotifyKey"] + userLogin.UserId;
                                //ghi đè lại tất cả các thông tin 
                                redisNotifyService.Remove(keyNotify);
                                redisNotifyService.Add(keyNotify, notifyToken);
                            }
                        }
                        else
                        {
                            // Mật khẩu không đúng
                            loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_WRONG_PASSWORD;
                        }
                    }
                }
                else
                {
                    loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_NOT_EXITS_USER;
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_SERVER_ERROR;
            }
            return loginEntity;
        }

        public List<string> GetListPermission(string userId)
        {
            List<string> listPermission = new List<string>();

            try
            {
                var userPermission = (from a in db.Users.AsNoTracking()
                                      where a.Id.Equals(userId)
                                      join b in db.UserPermissions.AsNoTracking() on a.Id equals b.UserId
                                      join c in db.Permissions.AsNoTracking() on b.PermissionId equals c.Id
                                      select new { c.Code, c.ScreenCode }
                                     ).ToList();

                listPermission = userPermission.Select(s => s.Code).ToList();
                listPermission.AddRange(userPermission.Where(r => !string.IsNullOrEmpty(r.ScreenCode)).GroupBy(g => g.ScreenCode).Select(s => s.Key).ToList());
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }

            return listPermission;
        }

        public List<string> GetSaleGroup(string userId)
        {
            List<string> saleGroupIds = new List<string>();

            try
            {
                saleGroupIds = (from a in db.SaleGroupUsers.AsNoTracking()
                                where a.UserId.Equals(userId)
                                select a.SaleGroupId
                                    ).ToList();
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }

            return saleGroupIds;
        }


        public UserEntity GetLoginModelCache(string userId)
        {
            UserEntity model = new UserEntity();
            try
            {
                var keyCache = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheLoginKey"] + userId;
                var cache = RedisService<UserEntity>.GetInstance(_RedisConnection);
                model = cache.Get<UserEntity>(keyCache);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            return model;
        }

        public void AddLoginModelCache(string userId, UserModel model)
        {
            try
            {
                var keyCache = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheLoginKey"] + userId;
                var cache = RedisService<UserEntity>.GetInstance(_RedisConnection);
                cache.Add(keyCache, model, new TimeSpan(240, 0, 0));
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
        }

        public void DeleteLoginModelCache(string userId)
        {
            try
            {
                var keyCache = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheLoginKey"] + userId;
                var cache = RedisService<UserEntity>.GetInstance(_RedisConnection);
                cache.Remove(keyCache); ;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
        }
    }
}
