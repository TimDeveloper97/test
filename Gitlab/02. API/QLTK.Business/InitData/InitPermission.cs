using Newtonsoft.Json;
using NTS.Common.Logs;
using NTS.Model.InitData;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace QLTK.Business.InitData
{
    public static class InitPermission
    {
        public static void Init()
        {
            try
            {
                var pathGroupFunction = HostingEnvironment.MapPath("~/" + "InitData/groupfunction.json");
                if (File.Exists(pathGroupFunction))
                {
                    var groupFunctions = JsonConvert.DeserializeObject<StaticPermission>(File.ReadAllText(pathGroupFunction));


                    QLTKEntities db = new QLTKEntities();
                    var listGroup = db.GroupFunctions.ToList();
                    List<Permission> listFunction;
                    GroupFunction group;
                    StaticGroupFunction groupFunction;
                    StaticFuncfion function;
                    foreach (var groupF in listGroup)
                    {
                        listFunction = db.Permissions.Where(r => r.GroupFunctionId == groupF.Id).ToList();
                        groupFunction = groupFunctions.Groups.FirstOrDefault(r => groupF.Id.Equals(r.Id));

                        if (groupFunction != null)
                        {
                            foreach (var func in listFunction)
                            {
                                function = groupFunctions.Functions.FirstOrDefault(r => func.Id.Equals(r.Id));
                                if (function == null)
                                {
                                    db.UserPermissions.RemoveRange(db.UserPermissions.Where(r => r.PermissionId.Equals(func.Id)));
                                    db.GroupPermissions.RemoveRange(db.GroupPermissions.Where(r => r.PermissionId.Equals(func.Id)));

                                    db.Permissions.Remove(func);
                                }
                            }
                        }
                        else
                        {

                            db.UserPermissions.RemoveRange(from p in db.Permissions
                                                           join u in db.UserPermissions on p.Id equals u.PermissionId
                                                           where p.GroupFunctionId.Equals(groupF.Id)
                                                           select u);

                            db.GroupPermissions.RemoveRange(from p in db.Permissions
                                                            join u in db.GroupPermissions on p.Id equals u.PermissionId
                                                            where p.GroupFunctionId.Equals(groupF.Id)
                                                            select u);

                            db.Permissions.RemoveRange(listFunction);
                            db.GroupFunctions.Remove(groupF);
                        }
                    }

                    db.SaveChanges();

                    foreach (var groupf in groupFunctions.Groups)
                    {
                        group = db.GroupFunctions.FirstOrDefault(r => r.Id.Equals(groupf.Id));

                        if (group == null)
                        {
                            group = new GroupFunction
                            {
                                Name = groupf.Name,
                                Code = groupf.Code,
                                Index = groupf.Index,
                                Id = groupf.Id
                            };

                            db.GroupFunctions.Add(group);

                        }
                        else
                        {
                            group.Name = groupf.Name;
                            group.Code = groupf.Code;
                            group.Index = groupf.Index;
                        }
                    }

                    db.SaveChanges();

                    Permission permission;
                    foreach (var fun in groupFunctions.Functions)
                    {
                        permission = db.Permissions.FirstOrDefault(r => r.Id.Equals(fun.Id));
                        if (permission == null)
                        {
                            permission = new Permission
                            {
                                Code = fun.Code,
                                Id = fun.Id,
                                GroupFunctionId = fun.GroupId,
                                Name = fun.Name,
                                ScreenCode = fun.ScreenCode,
                                Index = fun.Index
                            };

                            db.Permissions.Add(permission);
                        }
                        else
                        {
                            permission.Name = fun.Name;
                            permission.Code = fun.Code;
                            permission.ScreenCode = fun.ScreenCode;
                            permission.GroupFunctionId = fun.GroupId;
                            permission.Index = fun.Index;
                        }
                    }

                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
        }
    }
}
