namespace Domain
{
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstract.Models;
    using Abstract.Services;
    using Confluent.Kafka;

    public class BookingProducer : IBookingProducer
    {
        private const string TopicName = "booking-topic";
        private readonly ProducerConfig _producerConfig;

        public BookingProducer(ProducerConfig producerConfig) => _producerConfig = producerConfig;

        public async Task ProduceAsync(BookModel message, CancellationToken cancellationToken)
        {
            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            await producer.ProduceAsync(TopicName, new Message<Null, string> {Value = JsonSerializer.Serialize(message)}, cancellationToken);
            producer.Flush(cancellationToken);
        }
    }
}
