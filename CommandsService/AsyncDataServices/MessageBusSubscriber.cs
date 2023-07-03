using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor, ILogger<MessageBusSubscriber> logger)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _logger = logger;
            InitializeRabbitMQ();
        }
        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetValue<string>("RabbitMQHost"),
                Port = _configuration.GetValue<int>("RabbitMQPort")
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, "Trigger", string.Empty);

            _logger.LogInformation("RabbitMQ connection opened");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation($"Event received with message {message}");

                    _eventProcessor.ProcessEvent(message);
                };
            _channel.BasicConsume(_queueName, true, consumer);
            return Task.CompletedTask;
        }
        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            _logger.LogInformation("RabbitMQ connection closed");

            base.Dispose();
        }
    }
}
