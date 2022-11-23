using MassTransit;

namespace AntCommerce.Module.Product.Consumers
{
    public class InventoryConsumerDefinition : ConsumerDefinition<InventoryConsumer>
    {
        public InventoryConsumerDefinition()
        {
            ConcurrentMessageLimit = 5;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<InventoryConsumer> consumerConfigurator)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);

            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
