using konsi_api.Models.Events;
using konsi_api.Models.Interfaces;
using RabbitMQ.Client;

namespace konsi_api.Services.Messaging
{
    public class RabbitService : IRabbitService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public readonly string _queue = "Cpf-Searched";
        private const string _exchange = "input-data-service";


        public RabbitService()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "passw123",
            };

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
