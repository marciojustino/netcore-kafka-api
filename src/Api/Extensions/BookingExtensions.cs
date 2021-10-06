namespace Api.Extensions
{
    using Abstract.Services;
    using Confluent.Kafka;
    using Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class BookingExtensions
    {
        public static IServiceCollection ConfigureBooking(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new ProducerConfig
            {
                BootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers"),
                Acks = Acks.All,
                MessageSendMaxRetries = 3,
                LingerMs = 1,
            });
            services.AddScoped<IBookingProducer, BookingProducer>();
            return services;
        }
    }
}