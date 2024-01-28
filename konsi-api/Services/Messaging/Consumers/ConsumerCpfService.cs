using RabbitMQ.Client.Events;
using konsi_api.Models.Events;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using konsi_api.Models.Interfaces;

namespace konsi_api.Services.Messaging.Consumers
{
    public class ConsumerCpfService : BackgroundService
    {
        private readonly IRabbitService _rabbitService;
        private readonly IBenefitsCache _benefitsCache;
        private readonly IBenefitsHttpService _benefitsHttpService;
        private readonly IElasticService _elasticService;
        private readonly string _queue = "Cpf-Searched";


        public ConsumerCpfService(IRabbitService rabbitService, IBenefitsCache benefitsCache, IBenefitsHttpService benefitsHttpService, IElasticService elasticService)
        {
            _rabbitService = rabbitService;
            _benefitsCache = benefitsCache;
            _benefitsHttpService = benefitsHttpService;
            _elasticService = elasticService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = _rabbitService.CreateChannel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonSerializer.Deserialize<CpfSearchedEvent>(contentString);

                Console.WriteLine($"Message CustomerCreatedEvent received");

                var cachedBenefits = await _benefitsCache.GetAsync(message.Cpf);

                if (!cachedBenefits.Any())
                {
                    var searchBenefits = await _benefitsHttpService.GetBenefitsAsync(message.Cpf);

                    if (searchBenefits.Any())
                    {
                        await _benefitsCache.SetAsync(message.Cpf, searchBenefits);

                        await _elasticService.IndexBeneficiary(new Models.Beneficiary { Cpf = message.Cpf, Benefits = searchBenefits });
                    }
                }

                channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            channel.BasicConsume(_queue, false, consumer);

            return;
        }
    }
}
