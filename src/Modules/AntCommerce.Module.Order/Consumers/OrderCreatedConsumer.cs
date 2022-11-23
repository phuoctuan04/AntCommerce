using AntCommerce.Module.Message.OrderEvent;
using MassTransit;

namespace AntCommerce.Module.Order.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation($"OrderId {context.Message.OrderId} is created");
        }
    }
}
