using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;

namespace CricEco.GroundDashboard.Infrastructure.Services;

/// <summary>
/// In-memory auth service for demo/testing
/// Can be replaced with Supabase Auth when ready
/// </summary>
public class SupabaseAuthService : IAuthService
{
    private readonly IGroundOwnerRepository _groundOwnerRepository;
    private GroundOwner? _currentUser;

    // Demo credentials
    private const string DemoEmail = "owner@criceco.pk";
    private const string DemoPassword = "Owner@123";

    public SupabaseAuthService(IGroundOwnerRepository groundOwnerRepository)
    {
        _groundOwnerRepository = groundOwnerRepository;
    }

    public async Task<AuthResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // Check demo credentials
        if (email.Equals(DemoEmail, StringComparison.OrdinalIgnoreCase) && password == DemoPassword)
        {
            var owner = await _groundOwnerRepository.GetByEmailAsync(email, cancellationToken);
            if (owner != null)
            {
                _currentUser = owner;
                return AuthResult.Success(owner, "demo-token");
            }
        }

        // In a real implementation, this would call Supabase Auth
        return AuthResult.Failure("Invalid email or password");
    }

    public async Task<AuthResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        // Check if email already exists
        if (await _groundOwnerRepository.ExistsAsync(request.Email, cancellationToken))
        {
            return AuthResult.Failure("Email already registered");
        }

        // Create new owner
        var owner = GroundOwner.Create(
            request.Email,
            request.FullName,
            request.BusinessName,
            request.PhoneNumber);

        await _groundOwnerRepository.AddAsync(owner, cancellationToken);
        _currentUser = owner;

        return AuthResult.Success(owner, "demo-token");
    }

    public Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        _currentUser = null;
        return Task.CompletedTask;
    }

    public Task<GroundOwner?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_currentUser);
    }

    public Task<bool> IsAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_currentUser != null);
    }
}
