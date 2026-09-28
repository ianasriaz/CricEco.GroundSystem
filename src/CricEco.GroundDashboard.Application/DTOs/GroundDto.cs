using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Application.DTOs;

/// <summary>
/// Data Transfer Object for Ground entity
/// </summary>
public class GroundDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string PitchType { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
    public string OperatingHours { get; set; } = "06:00 AM – 11:00 PM";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public static GroundDto FromEntity(Ground ground)
    {
        return new GroundDto
        {
            Id = ground.Id,
            Name = ground.Name,
            Location = ground.Location,
            HourlyRate = ground.HourlyRate,
            PitchType = ground.PitchType,
            Amenities = ground.Amenities,
            IsActive = ground.IsActive,
            CreatedAt = ground.CreatedAt
        };
    }
}

public class CreateGroundRequest
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string PitchType { get; set; } = "Turf";
    public List<string> Amenities { get; set; } = new();
}

public class UpdateGroundRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string PitchType { get; set; } = string.Empty;
    public List<string> Amenities { get; set; } = new();
}
