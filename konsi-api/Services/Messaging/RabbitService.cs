using konsi_api.Models.Events;
using konsi_api.Models.Interfaces;
using RabbitMQ.Client;

namespace konsi_api.Services.Messaging
{
    public class RabbitService : IRabbitService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queue;
        private readonly string _exchange;
        
        public RabbitService(IConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = configuration.GetValue<string>("RabbitMQ:HostName"),
                UserName = configuration.GetValue<string>("RabbitMQ:UserName"),
                Password = configuration.GetValue<string>("RabbitMQ:Password"),
            };

            _queue = configuration.GetValue<string>("RabbitMQ:Queue");
            _exchange = configuration.GetValue<string>("RabbitMQ:Queue");

            connectionFactory.DispatchConsumersAsync = true;
            _connection = connectionFactory.CreateConnection("customers-service-publisher");

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchange,
                "fanout",
                true,
                false);

            _channel.QueueDeclare(queue: _queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(_queue,
                _exchange,
                nameof(CpfSearchedEvent));
        }

        public void BasicPublish(string key, byte[] body)
        {
            _channel.BasicPublish(_exchange, key, null, body);
        }

        public IModel CreateChannel()
        {
            return _channel;
        }
    }
}
