namespace CricEco.GroundDashboard.Domain.Entities;

/// <summary>
/// Represents a cricket ground/turf facility owned by a venue manager
/// </summary>
public class Ground
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string PitchType { get; set; } = "Turf";
    public List<string> Amenities { get; set; } = new();
    public TimeSpan OpeningTime { get; set; } = TimeSpan.FromHours(6);
    public TimeSpan ClosingTime { get; set; } = TimeSpan.FromHours(23);
    public Guid? ManagerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public Ground()
    {
    }

    public static Ground Create(
        string name,
        string location,
        decimal hourlyRate,
        string pitchType = "Turf",
        List<string>? amenities = null,
        Guid? managerId = null)
    {
        return new Ground
        {
            Id = Guid.NewGuid(),
            Name = name,
            Location = location,
            HourlyRate = hourlyRate,
            PitchType = pitchType,
            Amenities = amenities ?? new List<string> { "Floodlights", "Pavilion", "Changing Rooms" },
            ManagerId = managerId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string name, string location, decimal hourlyRate, string pitchType)
    {
        Name = name;
        Location = location;
        HourlyRate = hourlyRate;
        PitchType = pitchType;
    }

    public void UpdateAmenities(List<string> amenities)
    {
        Amenities = amenities;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
