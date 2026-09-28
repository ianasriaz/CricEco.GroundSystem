using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;

namespace CricEco.GroundDashboard.Infrastructure.Persistence;

/// <summary>
/// In-memory implementation of IGroundOwnerRepository for demo/testing
/// </summary>
public class SupabaseGroundOwnerRepository : IGroundOwnerRepository
{
    private static readonly List<GroundOwner> _owners = new()
    {
        GroundOwner.Create(
            "owner@criceco.pk",
            "Test Owner",
            "Gaddafi Turf & Sports Club",
            "+92 300 1234567")
    };

    static SupabaseGroundOwnerRepository()
    {
        // Set the demo owner ID
        _owners[0].Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d");
    }

    public Task<GroundOwner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var owner = _owners.FirstOrDefault(o => o.Id == id);
        return Task.FromResult(owner);
    }

    public Task<GroundOwner?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var owner = _owners.FirstOrDefault(o => 
            o.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(owner);
    }

    public Task<IEnumerable<GroundOwner>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_owners.AsEnumerable());
    }

    public Task AddAsync(GroundOwner owner, CancellationToken cancellationToken = default)
    {
        _owners.Add(owner);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(GroundOwner owner, CancellationToken cancellationToken = default)
    {
        var existing = _owners.FirstOrDefault(o => o.Id == owner.Id);
        if (existing != null)
        {
            var index = _owners.IndexOf(existing);
            _owners[index] = owner;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var owner = _owners.FirstOrDefault(o => o.Id == id);
        if (owner != null)
            _owners.Remove(owner);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_owners.Any(o => 
            o.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
    }
}
