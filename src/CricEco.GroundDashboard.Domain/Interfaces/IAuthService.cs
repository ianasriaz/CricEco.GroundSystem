using CricEco.GroundDashboard.Domain.Entities;

namespace CricEco.GroundDashboard.Domain.Interfaces;

/// <summary>
/// Authentication service abstraction - implementation in Infrastructure
/// </summary>
public interface IAuthService
{
    Task<AuthResult> SignInAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<AuthResult> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
    Task SignOutAsync(CancellationToken cancellationToken = default);
    Task<GroundOwner?> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    Task<bool> IsAuthenticatedAsync(CancellationToken cancellationToken = default);
}

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public GroundOwner? User { get; set; }
    public string? ErrorMessage { get; set; }

    public static AuthResult Success(GroundOwner user, string? token = null) =>
        new() { Succeeded = true, User = user, Token = token };

    public static AuthResult Failure(string error) =>
        new() { Succeeded = false, ErrorMessage = error };
}

public class SignUpRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}
