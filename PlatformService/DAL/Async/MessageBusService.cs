using Newtonsoft.Json;
using PlatformService.Data.Dto;
using RabbitMQ.Client;
using System.Text;

namespace PlatformService.DAL.Async
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusService> _logger;
        private IConnection? _connection;
        private IModel _channel;

        public MessageBusService(IConfiguration configuration, ILogger<MessageBusService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            InitiateRabbitMQConnection();
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonConvert.SerializeObject(platformPublishedDto);
            PublishMessage(message);
        }

        private void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("Trigger", String.Empty, null, body);
            _logger.LogInformation($"Message published to message bus: {message}");
        }

        private void InitiateRabbitMQConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.GetValue<string>("RabbitMQHost"),
                    Port = _configuration.GetValue<int>("RabbitMQPort")
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare("Trigger", ExchangeType.Fanout);

                _logger.LogInformation("Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in connection with RabbitMQ {ex.Message}");
            }
        }
    }
}
