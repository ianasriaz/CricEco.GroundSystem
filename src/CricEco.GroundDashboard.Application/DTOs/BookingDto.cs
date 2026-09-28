using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Application.DTOs;

/// <summary>
/// Data Transfer Object for Booking entity
/// </summary>
public class BookingDto
{
    public Guid Id { get; set; }
    public Guid GroundId { get; set; }
    public string GroundName { get; set; } = string.Empty;
    public string GroundLocation { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsWalkIn { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }

    public static BookingDto FromEntity(Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            GroundId = booking.GroundId,
            GroundName = booking.Ground?.Name ?? "Unknown",
            GroundLocation = booking.Ground?.Location ?? "",
            CustomerName = booking.CustomerName,
            CustomerPhone = booking.CustomerPhone,
            StartsAt = booking.StartsAt,
            EndsAt = booking.EndsAt,
            Status = booking.Status.ToString(),
            IsWalkIn = booking.IsWalkIn,
            CreatedAt = booking.CreatedAt,
            TotalAmount = booking.TotalAmount
        };
    }
}

public class CreateBookingRequest
{
    public Guid GroundId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public bool IsWalkIn { get; set; } = false;
}

public class UpdateBookingStatusRequest
{
    public Guid BookingId { get; set; }
    public string Status { get; set; } = string.Empty; // "Approved", "Rejected", "Cancelled"
}
