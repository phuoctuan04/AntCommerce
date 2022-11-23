using AntCommerce.Module.Message.OrderEvent;
using MassTransit;

namespace AntCommerce.Module.Product.Consumers
{
    public class InventoryConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<InventoryConsumer> _logger;
        public InventoryConsumer(ILogger<InventoryConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation($"OrderId {context.Message.OrderId} is created. Check inventory");
        }
    }
}
