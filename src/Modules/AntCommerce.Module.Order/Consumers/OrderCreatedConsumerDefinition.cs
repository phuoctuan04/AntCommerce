using MassTransit;

namespace AntCommerce.Module.Order.Consumers
{
    public class OrderCreatedConsumerDefinition : ConsumerDefinition<OrderCreatedConsumer>
    {
        public OrderCreatedConsumerDefinition()
        {
            ConcurrentMessageLimit = 5;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderCreatedConsumer> consumerConfigurator)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);

            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}