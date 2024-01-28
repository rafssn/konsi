using RabbitMQ.Client;

namespace konsi_api.Models.Interfaces
{
    public interface IRabbitService
    {
        public void BasicPublish(string key, byte[] body);

        public IModel CreateChannel();
    }
}
