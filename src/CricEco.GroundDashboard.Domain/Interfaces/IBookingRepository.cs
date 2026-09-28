using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Domain.Interfaces;

/// <summary>
/// Repository interface for Booking entities
/// </summary>
public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByGroundIdAsync(Guid groundId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetPendingAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> HasOverlappingBookingAsync(Guid groundId, DateTime start, DateTime end, Guid? excludeBookingId = null, CancellationToken cancellationToken = default);
}
