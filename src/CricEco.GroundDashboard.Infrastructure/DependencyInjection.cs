using CricEco.GroundDashboard.Domain.Interfaces;
using CricEco.GroundDashboard.Infrastructure.Persistence;
using CricEco.GroundDashboard.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Supabase;

namespace CricEco.GroundDashboard.Infrastructure;

/// <summary>
/// Dependency injection extension for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string supabaseUrl,
        string supabaseKey)
    {
        // Configure Supabase client
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true,
            AutoRefreshToken = true
        };

        var client = new Supabase.Client(supabaseUrl, supabaseKey, options);
        client.Auth.LoadSession();

        services.AddSingleton(client);

        // Register repositories
        services.AddScoped<IGroundRepository, SupabaseGroundRepository>();
        services.AddScoped<IBookingRepository, SupabaseBookingRepository>();
        services.AddScoped<IGroundOwnerRepository, SupabaseGroundOwnerRepository>();

        // Register services
        services.AddScoped<IAuthService, SupabaseAuthService>();

        return services;
    }
}
