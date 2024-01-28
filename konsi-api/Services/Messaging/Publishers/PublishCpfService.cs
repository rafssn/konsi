using konsi_api.Models.Events;
using konsi_api.Models.Interfaces;
using System.Text;
using System.Text.Json;

namespace konsi_api.Services.Messaging.Publishers
{
    public class PublishCpfService : IPublishCpfService
    {
        private readonly RabbitService _rabbitService;
        private const string Exchange = "input-data-service";

        public PublishCpfService() {
            _rabbitService = new RabbitService();    
        }

        public void Publish(CpfSearchedEvent @event)
        {
            var payload = JsonSerializer.Serialize(@event);
            var byteArray = Encoding.UTF8.GetBytes(payload);

            _rabbitService.BasicPublish(nameof(CpfSearchedEvent), byteArray);
        }
    }
}
