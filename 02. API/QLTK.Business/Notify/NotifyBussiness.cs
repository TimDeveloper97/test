using NTS.Caching;
using NTS.Model.Notify;
using QLTK.Business.RabbitMQServers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLTK.Business.Notify
{
    public class NotifyBussiness
    {
        private RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
        private RabbitMQServer<WebNotifyQueueModel> rabbitMQServer = new RabbitMQServer<WebNotifyQueueModel>();

        public void SendNotify(WebNotifyQueueModel webNotifyQueue)
        {
            rabbitMQServer.PushToQueue(webNotifyQueue);
        }
    }
}
