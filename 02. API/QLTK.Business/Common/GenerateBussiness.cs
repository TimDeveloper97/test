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

namespace NTS.Business
{

    public class GenerateBussiness
    {
        public static void DeleteCache(string key)
        {
            string _RedisConnection = ConfigurationManager.AppSettings["RedisConnection"];
            var cacheRadar = RedisService<string>.GetInstance(_RedisConnection);
            cacheRadar.Remove(key);
        }
        public static string LockUser(string Account)
        {
            string rs = "";
            var db = new QLTKEntities();
            try
            {
                var data = db.Users.FirstOrDefault(u => u.UserName.Equals(Account));
                if (data.IsDisable==Constants.Active)
                {
                    data.IsDisable = Constants.NotActive;
                    rs = "Mở khóa tài khoản thành công";
                }
                else
                {
                    data.IsDisable = Constants.Active;
                    rs = "Khóa tài khoản thành công";
                }
                db.SaveChanges();
            }
            catch (Exception)
            {
            }
            return rs;
        }
    }
}
