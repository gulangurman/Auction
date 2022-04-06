using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Auction.Ordering.Models;
using Ordering.Domain.Repositories;
using Ordering.Domain.Models;
using EventBusRabbitMQ.Events;

namespace Auction.Ordering.Consumers
{
    public class EventBusOrderCreateConsumer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public EventBusOrderCreateConsumer(IRabbitMQPersistentConnection persistentConnection, IMapper mapper, IOrderRepository orderRepository)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.OrderCreateQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstants.OrderCreateQueue, autoAck: true, consumer: consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message);

            if (e.RoutingKey == EventBusConstants.OrderCreateQueue)
            {
                var dto = _mapper.Map<CreateOrderDTO>(@event);

                dto.CreatedAt = DateTime.Now;
                dto.TotalPrice = @event.Quantity * @event.Price;
                dto.UnitPrice = @event.Price;
                var order = _mapper.Map<Order>(dto);
                if (order == null)
                {
                    throw new ApplicationException("Entity could not be mapped!");
                }
                await _orderRepository.AddAsync(order);
            }
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}
