using System;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Booking;

namespace Application.Contracts.Interfaces
{
    public interface IBookingService
    {
        Task<SeatPlanDto> GetSeatPlanAsync(Guid busScheduleId);
        Task<BookSeatResultDto> BookSeatAsync(BookSeatInputDto input);
    }
}