using Newtonsoft.Json;
using NTS.Api.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Api.Controllers.Common
{
    public class NotifyController
    {

        ///// <summary>
        ///// Gửi thông báo notify tới máy người dùng
        ///// </summary>
        ///// <param name="fcmToken">Token máy của người dùng</param>
        ///// <param name="message">Nội dung thông báo</param>
        //public static void SendNotify(string fcmToken, string message, string userId, string notificationType)
        //{
        //    var client = new RestClient(Constants.FIREBASE_URL);
        //    var request = new RestRequest();
        //    var body = string.Empty;
        //    request.Method = Method.POST;
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", Constants.FIREBASE_SERVER_KEY);


        //    PMSCBK1100FireBaseDataModel content = new PMSCBK1100FireBaseDataModel();
        //    content.title = Constants.NOTIFY_TITLE;
        //    content.body = message;


        //    ParkingManagementSystemEntities db = new ParkingManagementSystemEntities();
        //    var phoneType = db.UserNotifications.AsNoTracking().Where(r => r.NotifyToken.Equals(fcmToken)).FirstOrDefault();
        //    if (phoneType == null || phoneType.Type != Constants.PHONE_TYPE_IOS)
        //    {
        //        PMSCBK1100FireBaseSendModel model = new PMSCBK1100FireBaseSendModel();
        //        model.data = content;
        //        model.to = fcmToken;
        //        model.priority = Constants.NOTIFY_PRIORITY;

        //        body = JsonConvert.SerializeObject(model);
        //    }
        //    else
        //    {
        //        content.badge = db.Users.Find(phoneType.UserId).Badge + 1;
        //        content.sound = "default";
        //        PMSCBK1100FireBaseSendModelIOS model = new PMSCBK1100FireBaseSendModelIOS();
        //        model.notification = content;
        //        model.to = fcmToken;
        //        model.priority = Constants.NOTIFY_PRIORITY;
        //        // TODO: add data content here if need

        //        body = JsonConvert.SerializeObject(model);
        //    }

        //    request.AddParameter("application/json", body, ParameterType.RequestBody);

        //    var response = client.Execute(request);
        //    if (response.ErrorException == null)
        //    {
        //        using (var trans = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                Notification notification = new Notification
        //                {
        //                    UserId = userId,
        //                    Content = message,
        //                    NotificationDate = DateTime.Now,
        //                    Type = notificationType
        //                };

        //                db.Notifications.Add(notification);
        //                db.SaveChanges();
        //                trans.Commit();
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //            }
        //        }
        //    }
        //}
    }
}