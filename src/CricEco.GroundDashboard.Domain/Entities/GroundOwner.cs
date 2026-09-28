namespace CricEco.GroundDashboard.Domain.Entities;

/// <summary>
/// Represents a ground owner/venue manager using the dashboard
/// </summary>
public class GroundOwner
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<Ground> Grounds { get; set; } = new List<Ground>();

    public GroundOwner()
    {
    }

    public static GroundOwner Create(
        string email,
        string fullName,
        string businessName,
        string? phoneNumber = null)
    {
        return new GroundOwner
        {
            Id = Guid.NewGuid(),
            Email = email.ToLowerInvariant(),
            FullName = fullName,
            BusinessName = businessName,
            PhoneNumber = phoneNumber,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateProfile(string fullName, string businessName, string? phoneNumber)
    {
        FullName = fullName;
        BusinessName = businessName;
        PhoneNumber = phoneNumber;
    }

    public Ground AddGround(string name, string location, decimal hourlyRate, string pitchType = "Turf")
    {
        var ground = Ground.Create(name, location, hourlyRate, pitchType, managerId: Id);
        Grounds.Add(ground);
        return ground;
    }
}
