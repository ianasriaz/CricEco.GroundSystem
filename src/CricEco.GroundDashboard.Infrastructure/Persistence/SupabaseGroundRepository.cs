using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;

namespace CricEco.GroundDashboard.Infrastructure.Persistence;

/// <summary>
/// In-memory implementation of IGroundRepository for demo/testing
/// Can be replaced with Supabase implementation when ready
/// </summary>
public class SupabaseGroundRepository : IGroundRepository
{
    private static readonly List<Ground> _grounds = new()
    {
        Ground.Create(
            "Gaddafi Turf & Sports Club",
            "Gulberg III, Lahore",
            3500,
            "Turf",
            new List<string> { "Floodlights", "Pavilion", "Changing Rooms", "Parking Available" },
            Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d")),
        Ground.Create(
            "Model Town Cricket Arena",
            "Model Town J-Block, Lahore",
            2800,
            "AstroTurf",
            new List<string> { "Floodlights", "Practice Nets", "Pavilion", "Parking Available" },
            Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"))
    };

    public Task<Ground?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ground = _grounds.FirstOrDefault(g => g.Id == id);
        return Task.FromResult(ground);
    }

    public Task<IEnumerable<Ground>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_grounds.AsEnumerable());
    }

    public Task<IEnumerable<Ground>> GetByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        var grounds = _grounds.Where(g => g.ManagerId == managerId);
        return Task.FromResult(grounds);
    }

    public Task<IEnumerable<Ground>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var grounds = _grounds.Where(g => 
            g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            g.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(grounds);
    }

    public Task AddAsync(Ground ground, CancellationToken cancellationToken = default)
    {
        _grounds.Add(ground);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Ground ground, CancellationToken cancellationToken = default)
    {
        var existing = _grounds.FirstOrDefault(g => g.Id == ground.Id);
        if (existing != null)
        {
            var index = _grounds.IndexOf(existing);
            _grounds[index] = ground;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ground = _grounds.FirstOrDefault(g => g.Id == id);
        if (ground != null)
            _grounds.Remove(ground);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_grounds.Any(g => g.Id == id));
    }
}
