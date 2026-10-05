using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;

namespace CricEco.GroundDashboard.Infrastructure.Persistence;

/// <summary>
/// In-memory implementation of IBookingRepository for demo/testing
/// </summary>
public class SupabaseBookingRepository : IBookingRepository
{
    private static readonly List<Booking> _bookings = new()
    {
        Booking.Create(
            Guid.Parse("e4b1a111-2222-3333-4444-555566667771"),
            DateTime.Now.AddHours(2),
            DateTime.Now.AddHours(4),
            "Babar Azam (Captain)",
            "+92 300 8472910",
            Guid.Parse("11111111-1111-1111-1111-111111111111")),
        Booking.Create(
            Guid.Parse("e4b1a111-2222-3333-4444-555566667771"),
            DateTime.Now.AddHours(26),
            DateTime.Now.AddHours(28),
            "Shaheen Afridi (Captain)",
            "+92 321 5566778",
            Guid.Parse("22222222-2222-2222-2222-222222222222")),
        Booking.Create(
            Guid.Parse("e4b1a111-2222-3333-4444-555566667771"),
            DateTime.Now.AddHours(-3),
            DateTime.Now.AddHours(-1),
            "Mohammad Rizwan",
            "+92 333 4455667",
            Guid.Parse("33333333-3333-3333-3333-333333333333"))
    };

    static SupabaseBookingRepository()
    {
        // Approve some bookings for demo
        _bookings[2].Approve();
    }

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(booking);
    }

    public Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_bookings.AsEnumerable());
    }

    public Task<IEnumerable<Booking>> GetByGroundIdAsync(Guid groundId, CancellationToken cancellationToken = default)
    {
        var bookings = _bookings.Where(b => b.GroundId == groundId);
        return Task.FromResult(bookings);
    }

    public Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status, CancellationToken cancellationToken = default)
    {
        var bookings = _bookings.Where(b => b.Status == status);
        return Task.FromResult(bookings);
    }

    public Task<IEnumerable<Booking>> GetByDateRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        var bookings = _bookings.Where(b => b.StartsAt >= start && b.EndsAt <= end);
        return Task.FromResult(bookings);
    }

    public Task<IEnumerable<Booking>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        return GetByStatusAsync(BookingStatus.Requested, cancellationToken);
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        var existing = _bookings.FirstOrDefault(b => b.Id == booking.Id);
        if (existing != null)
        {
            var index = _bookings.IndexOf(existing);
            _bookings[index] = booking;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking != null)
            _bookings.Remove(booking);
        return Task.CompletedTask;
    }

    public Task<bool> HasOverlappingBookingAsync(Guid groundId, DateTime start, DateTime end, Guid? excludeBookingId = null, CancellationToken cancellationToken = default)
    {
        var bookings = _bookings.Where(b => 
            b.GroundId == groundId && 
            b.Status != BookingStatus.Rejected && 
            b.Status != BookingStatus.Cancelled);

        if (excludeBookingId.HasValue)
            bookings = bookings.Where(b => b.Id != excludeBookingId.Value);

        return Task.FromResult(bookings.Any(b => b.Overlaps(start, end)));
    }
}
