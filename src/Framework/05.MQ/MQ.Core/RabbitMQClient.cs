using System;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Wyn.MQ.Abstractions;
using Wyn.MQ.Abstractions.Constant;
using Wyn.MQ.Abstractions.Options;
using Wyn.Utils.Extensions;
using Wyn.Utils.Helpers;

namespace Wyn.MQ.Core
{
    /// <summary>
    /// RabbitMQ客户端
    /// </summary>
    public class RabbitMQClient : IDisposable
    {
        // 发送连接
        private IConnection _sendConnection;

        // 接收连接
        private IConnection _receiveConnection;

        private readonly RabbitMQOptions _options;

        public RabbitMQClient(RabbitMQOptions options)
        {
            _options = options;

            CreateConnection();
        }

        internal void CreateConnection()
        {
            if (_options.UserName.IsNull())
                throw new ArgumentNullException("用户名不能为空","用户名不能为空");

            if (_options.Password.IsNull())
                throw new ArgumentNullException("密码不能为空", "密码不能为空");

            if (_options.HostName.IsNull())
                _options.HostName = "localhost";

            if (_options.Port < 1 || _options.Port > 65535)
                _options.Port = 5672;

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            if (_options.VirtualHost.NotNull())
                factory.VirtualHost = _options.VirtualHost;

            _sendConnection = factory.CreateConnection();

            _receiveConnection = factory.CreateConnection();
        }

        /// <summary>
        /// 发送连接
        /// </summary>
        public IConnection SendConnection => _sendConnection;

        /// <summary>
        /// 接收连接
        /// </summary>
        public IConnection ReceiveConnection => _receiveConnection;

        /// <summary>
        /// 发送单条消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名</param>
        /// <param name="routingKey">路由建</param>
        /// <param name="message">消息体</param>
        /// <param name="settings">配置</param>
        public void Send<T>(string queue, T message, string routingKey = "", RabbitMQDeclareSettings settings = null)
        {
            if (queue.IsNull())
                throw new ArgumentNullException(nameof(queue), "queue is null");

            queue = GetQueueName(queue);

            if (routingKey.IsNull())
                routingKey = queue;

            using var channel = _sendConnection.CreateModel();

            settings = GetSettings(settings);
            var exchange = settings.Exchange;
            channel.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete, exchange.Arguments);
            channel.QueueDeclare(queue, settings.Queue.Durable, settings.Queue.Exclusive, settings.Queue.AutoDelete, settings.Queue.Arguments);
            channel.QueueBind(queue, exchange.Name, routingKey);

            var body = Encoding.UTF8.GetBytes(JsonHelper.Serialize(message));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange.Name, routingKey, properties, body);
        }

        /// <summary>
        /// 使用事件接收消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue">队列名称</param>
        /// <param name="func">回调函数</param>
        /// <param name="settings">定义设置</param>
        public Consumer Receive<T>(string queue, Func<T, bool> func, RabbitMQDeclareSettings settings = null)
        {
            if (queue.IsNull())
                throw new ArgumentNullException(nameof(queue), "queue is null");

            if (func.IsNull())
                throw new ArgumentNullException(nameof(func), "func is null");

            queue = GetQueueName(queue);

            var channel = _receiveConnection.CreateModel();
            channel.BasicQos(0, 1, false);
            settings = GetSettings(settings);
            channel.QueueDeclare(queue, settings.Queue.Durable, settings.Queue.Exclusive, settings.Queue.AutoDelete, settings.Queue.Arguments);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var message = JsonHelper.Deserialize<T>(Encoding.UTF8.GetString(eventArgs.Body.ToArray()));
                if (func(message))
                {
                    channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                else
                {
                    channel.BasicNack(eventArgs.DeliveryTag, false, true);
                }
            };

            var tag = channel.BasicConsume(queue, false, consumer);
            return new Consumer
            {
                Channel = channel,
                Tag = tag
            };
        }

        public void Dispose()
        {
            _sendConnection?.Dispose();
            _receiveConnection?.Dispose();
        }

        private string GetQueueName(string queue)
        {
            return _options.Prefix.NotNull() ? $"{_options.Prefix}.{queue}" : queue;
        }

        private RabbitMQDeclareSettings GetSettings(RabbitMQDeclareSettings settings)
        {
            settings ??= new RabbitMQDeclareSettings();

            settings.Exchange ??= new ExchangeDeclareSettings();

            settings.Queue ??= new QueueDeclareSettings();

            var exchange = settings.Exchange;

            if (exchange.Name.IsNull())
            {
                switch (exchange.Type)
                {
                    case ExchangeType.Fanout:
                        exchange.Name = DefaultExchange.Fanout;
                        break;
                    case ExchangeType.Topic:
                        exchange.Name = DefaultExchange.Topic;
                        break;
                    case ExchangeType.Headers:
                        exchange.Name = DefaultExchange.Headers;
                        break;
                    default:
                        exchange.Name = DefaultExchange.Direct;
                        break;
                }
            }

            return settings;
        }
    }
}
