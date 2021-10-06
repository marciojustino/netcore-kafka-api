namespace Domain
{
    using System;
    using System.Threading;
    using Abstract.Models;
    using Abstract.Services;
    using Confluent.Kafka;

    public class BookingConsumer : IBookingConsumer
    {
        private const string TopicName = "booking-topic";
        private readonly ConsumerConfig _consumerConfig;

        public BookingConsumer(ConsumerConfig consumerConfig) => _consumerConfig = consumerConfig;

        public void Listen(BookModel message)
        {
            using var consumer = new ConsumerBuilder<Ignore, BookModel>(_consumerConfig).Build();
            consumer.Subscribe(TopicName);

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                    try
                    {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"Consumed message '{cr.Message.Value}' at '{cr.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }
    }
}
