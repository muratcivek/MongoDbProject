using Travel.WEB.DTOs.ReservationDTOs;

namespace Travel.WEB.Services.Reservation
{
    public interface IReservationService
    {
        Task<ResultReservationDto> CreateAsync(
       CreateReservationDto createReservationDto);

        Task<List<ResultReservationDto>>
            GetAllAsync();

        Task<List<ResultReservationDto>>
            GetByRouteIdAsync(string routeId);

        Task<ResultReservationDto?>
            GetByIdAsync(string id);

        Task UpdateStatusAsync(
            UpdateReservationStatusDto dto);

        Task DeleteAsync(string id);

        Task CreateIndexesAsync();

        Task<ResultReservationDto?> GetByCodeAndEmailAsync(
    string reservationCode,
    string email);
        Task<List<ReservationCalendarItemDto>> GetCalendarAsync(
    DateTime startDate,
    DateTime endDate);

        Task<List<ResultReservationDto>> GetByTravelDateAsync(
            DateTime date);

    }
}