namespace Abstract.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IBookingProducer
    {
        Task ProduceAsync(BookModel message, CancellationToken cancellationToken);
    }
}
