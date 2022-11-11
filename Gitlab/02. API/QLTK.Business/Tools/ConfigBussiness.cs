using NTS.Caching;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Config;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Config
{
    public class ConfigBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ConfigResultModel> SearchConfig()
        {
            SearchResultModel<ConfigResultModel> searchResult = new SearchResultModel<ConfigResultModel>();

            var dataQuery = (from a in db.Configs.AsNoTracking()
                             join b in db.Users.AsNoTracking() on a.UpdateBy equals b.Id
                             join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                             orderby a.Name
                             select new ConfigResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Value = a.Value,
                                 ValueType = a.ValueType,
                                 Unit = a.Unit,
                                 UpdateByName = c.Name,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        public ConfigModel GetConfigInfo(ConfigModel model)
        {
            var resultInfo = db.Configs.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new ConfigModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Value = p.Value,
                ValueType = p.ValueType,
                Unit = p.Unit
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Config);
            }

            return resultInfo;
        }
        public void UpdateConfig(ConfigModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var config = db.Configs.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    config.Value = model.Value;
                    config.Unit = model.Unit;
                    config.UpdateBy = model.UpdateBy;
                    config.UpdateDate = DateTime.Now;

                    // Xóa cache lưu cấu hình
                    RedisService<ConfigModel> redisService = RedisService<ConfigModel>.GetInstance();
                    string keyConfig = ConfigurationManager.AppSettings["PrefixSystemKey"] + ConfigurationManager.AppSettings["CacheConfig"] + config.Code;
                    if (redisService.Exists(keyConfig))
                    {
                        redisService.Remove(keyConfig);
                    }

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
    }
}
