using Newtonsoft.Json;
using NTS.Common.Logs;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RabbitMQ.Client;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLTK.Business.RabbitMQServers
{
    public class RabbitMQServer<T>
    {
        public Boolean Started { get; private set; }

        private RabbitMQ.Client.IConnection connection = null;
        private string consumerTag = string.Empty;
        private QueueDeclareOk result { get; set; }
        private ConnectionFactory _factory;

        string q_Host = string.Empty,
            q_Post = string.Empty,
            q_User = string.Empty,
            q_Pass = string.Empty,
            q_Name = string.Empty,
            q_ExpiryTime = string.Empty;

        public RabbitMQServer()
        {
            q_Host = ConfigurationManager.AppSettings["RabbitMQNotifyHost"];
            q_Post = ConfigurationManager.AppSettings["RabbitMQNotifyPost"];
            q_User = ConfigurationManager.AppSettings["RabbitMQNotifyUsername"];
            q_Pass = ConfigurationManager.AppSettings["RabbitMQNotifyPassword"];
            q_Name = ConfigurationManager.AppSettings["RabbitMQNotifyQueuename"];

            try
            {
                if (!string.IsNullOrEmpty(q_Host) && !string.IsNullOrEmpty(q_User) && !string.IsNullOrEmpty(q_Pass))
                {
                    _factory = new ConnectionFactory()
                    {
                        HostName = q_Host,
                        UserName = q_User,
                        Password = q_Pass,
                        Port = Convert.ToInt32(q_Post),
                        DispatchConsumersAsync = true
                    };
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
        }

        public Boolean PushToQueue(T data)
        {
            return Task.Run(async () =>
              await Task<bool>.Factory.StartNew(() => ProcessPushToQueue(data), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
              ).Result;
        }

        private bool ProcessPushToQueue(T document)
        {
            Boolean isSuccess = false;
            try
            {
                connection = _factory.CreateConnection();

                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: q_Name,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var message = JsonConvert.SerializeObject(document);
                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;
                    if (!string.IsNullOrEmpty(q_ExpiryTime))
                    {
                        properties.Expiration = q_ExpiryTime;
                    }
                    channel.BasicPublish(exchange: "", routingKey: q_Name, basicProperties: properties, body: body);
                }

                connection.Close();
                connection.Abort();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            return isSuccess;
        }
    }
}
