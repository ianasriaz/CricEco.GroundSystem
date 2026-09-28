using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Domain.Interfaces;

/// <summary>
/// Repository interface for Ground entities - defines contract without implementation details
/// </summary>
public interface IGroundRepository
{
    Task<Ground?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ground>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Ground>> GetByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ground>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(Ground ground, CancellationToken cancellationToken = default);
    Task UpdateAsync(Ground ground, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
