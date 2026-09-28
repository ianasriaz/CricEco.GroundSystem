using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Application.DTOs;

/// <summary>
/// Data Transfer Object for Dashboard statistics
/// </summary>
public class DashboardStatsDto
{
    public int TotalGrounds { get; set; }
    public int PendingRequests { get; set; }
    public int ConfirmedBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<BookingDto> RecentBookings { get; set; } = new();
    public List<GroundDto> Grounds { get; set; } = new();
}

/// <summary>
/// Data Transfer Object for GroundOwner
/// </summary>
public class GroundOwnerDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    public static GroundOwnerDto FromEntity(GroundOwner owner)
    {
        return new GroundOwnerDto
        {
            Id = owner.Id,
            Email = owner.Email,
            FullName = owner.FullName,
            BusinessName = owner.BusinessName,
            PhoneNumber = owner.PhoneNumber
        };
    }
}
