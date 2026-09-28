using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Domain.Interfaces;

/// <summary>
/// Repository interface for GroundOwner entities
/// </summary>
public interface IGroundOwnerRepository
{
    Task<GroundOwner?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GroundOwner?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroundOwner>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(GroundOwner owner, CancellationToken cancellationToken = default);
    Task UpdateAsync(GroundOwner owner, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
