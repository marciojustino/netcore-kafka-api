namespace Api.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstract.Models;
    using Abstract.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("v1/books")]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingProducer _bookingProducer;

        public BookingController(
            ILogger<BookingController> logger,
            IBookingProducer bookingProducer)
        {
            _logger = logger;
            _bookingProducer = bookingProducer;
        }

        [HttpPost]
        public async Task<IActionResult> SaveBooking([FromBody] BookModel booking, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ValidationState);

            await _bookingProducer.ProduceAsync(booking, cancellationToken);
            return NoContent();
        }
    }
}
