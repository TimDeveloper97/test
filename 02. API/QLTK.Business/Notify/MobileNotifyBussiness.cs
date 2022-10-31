using NTS.Caching;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Notify;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Notify
{
    public class MobileNotifyBussiness
    {
        private QLTKEntities _db = new QLTKEntities();
        RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
        public List<NotifyModel> GetNotify(NotifySearchModel model)
        {
            List<NotifyModel> lst = new List<NotifyModel>();
            try
            {

                var notifies = (from n in _db.Notifications.AsNoTracking()
                                where n.UserId.Equals(model.UserId)
                                select new NotifyModel
                                {
                                    Content = n.Content,
                                    CreateDate = n.CreateDate,
                                    Id = n.NotificationId,
                                    Image = n.Image,
                                    Link = n.Link,
                                    ObjectId = n.ObjectId,
                                    ObjectType = n.ObjectType,
                                    Status = n.Status,
                                    Title = n.Title
                                }).AsQueryable();

                if (!string.IsNullOrEmpty(model.TimeFrom))
                {
                    var dateFrom = DateTimeHelper.ConvertDateFromStr(model.TimeFrom);
                    notifies = notifies.Where(r => r.CreateDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(model.TimeTo))
                {
                    var dateTo = DateTimeHelper.ConvertDateToStr(model.TimeTo);
                    notifies = notifies.Where(r => r.CreateDate <= dateTo);
                }

                lst = notifies.OrderByDescending(u => u.CreateDate).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ResourceUtil.GetResourcesNoLag(ErrorResourceKey.ERR0001));
            }

            return lst;
        }

        public void TickNotify(NotifySearchModel model)
        {
            try
            {
                var notify = _db.Notifications.FirstOrDefault(r => r.NotificationId == model.Id);

                notify.Status = Constants.Notify_Status_Read;

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ResourceUtil.GetResourcesNoLag(ErrorResourceKey.ERR0001));
            }
        }
    }
}
