namespace Abstract.Services
{
    using Models;

    public interface IBookingConsumer
    {
        void Listen(BookModel message);
    }
}
