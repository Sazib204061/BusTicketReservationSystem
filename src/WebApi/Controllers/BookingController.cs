using Application.Contracts.DTOs.Booking;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("seat-plan/{busScheduleId}")]
        public async Task<ActionResult<SeatPlanDto>> GetSeatPlan(Guid busScheduleId)
        {
            try
            {
                var seatPlan = await _bookingService.GetSeatPlanAsync(busScheduleId);
                return Ok(seatPlan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("book-seat")]
        public async Task<ActionResult<BookSeatResultDto>> BookSeat(BookSeatInputDto input)
        {
            try
            {
                var result = await _bookingService.BookSeatAsync(input);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BookSeatResultDto
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}