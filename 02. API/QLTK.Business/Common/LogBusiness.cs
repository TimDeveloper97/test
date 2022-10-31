using NTS.Common;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business
{
    public class LogBusiness
    {
        /// <summary>
        /// Log đang nhập
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userName"></param>
        public static void SaveLogLogin(QLTKEntities db, string userId, string deviceID)
        {
            ActivityLog model = new ActivityLog();
            model.Id = Guid.NewGuid().ToString();
            model.UserId = userId;
            model.LogDate = DateTime.Now;
            model.LogContent = "Đăng nhập hệ thống";
            model.ObjectId = "0";
            model.LogType = 0;
            db.ActivityLogs.Add(model);
            db.SaveChanges();
        }

        public static void SaveLogLogout(QLTKEntities db, string userId)
        {
            ActivityLog model = new ActivityLog();
            model.Id = Guid.NewGuid().ToString();
            model.UserId = userId;
            model.LogDate = DateTime.Now;
            model.LogContent = "Đăng xuất hệ thống";
            model.ObjectId = "0";
            model.LogType = 0;
            db.ActivityLogs.Add(model);
            db.SaveChanges();
        }

        /// <summary>
        /// Log thao tác dữ liệu
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userName"></param>
        /// <param name="content"></param>
        /// <param name="objectId"></param>
        public static void SaveLogEvent(QLTKEntities db, string userId, string description)
        {
            try
            {
                ActivityLog model = new ActivityLog();
                model.Id = Guid.NewGuid().ToString();
                model.UserId = userId;
                model.LogDate = DateTime.Now;
                model.LogContent = description;
                model.ObjectId = "0";
                model.LogType = 0;
                db.ActivityLogs.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
