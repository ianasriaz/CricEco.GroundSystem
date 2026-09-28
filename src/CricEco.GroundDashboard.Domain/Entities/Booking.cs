namespace CricEco.GroundDashboard.Domain.Entities;

/// <summary>
/// Represents a booking/reservation for a cricket ground
/// </summary>
public class Booking
{
    public Guid Id { get; set; }
    public Guid GroundId { get; set; }
    public Ground? Ground { get; set; }
    public Guid? RequestedBy { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsWalkIn { get; set; }

    // Calculated property
    public decimal TotalAmount => (decimal)(EndsAt - StartsAt).TotalHours * (Ground?.HourlyRate ?? 0);

    public Booking()
    {
    }

    public static Booking Create(
        Guid groundId,
        DateTime startsAt,
        DateTime endsAt,
        string customerName,
        string? customerPhone = null,
        Guid? requestedBy = null,
        bool isWalkIn = false)
    {
        return new Booking
        {
            Id = Guid.NewGuid(),
            GroundId = groundId,
            StartsAt = startsAt,
            EndsAt = endsAt,
            CustomerName = customerName,
            CustomerPhone = customerPhone,
            RequestedBy = requestedBy,
            Status = BookingStatus.Requested,
            CreatedAt = DateTime.UtcNow,
            IsWalkIn = isWalkIn
        };
    }

    public void Approve() => Status = BookingStatus.Approved;
    public void Reject() => Status = BookingStatus.Rejected;
    public void Cancel() => Status = BookingStatus.Cancelled;

    public bool Overlaps(DateTime start, DateTime end)
    {
        return StartsAt < end && EndsAt > start;
    }

    public TimeSpan Duration => EndsAt - StartsAt;
}

public enum BookingStatus
{
    Requested,
    Approved,
    Rejected,
    Cancelled,
    Completed
}
