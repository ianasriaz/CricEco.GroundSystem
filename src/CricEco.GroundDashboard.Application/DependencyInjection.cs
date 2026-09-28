using CricEco.GroundDashboard.Application.Features.Bookings.Queries;
using CricEco.GroundDashboard.Application.Features.Grounds.Commands;
using CricEco.GroundDashboard.Application.Features.Grounds.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CricEco.GroundDashboard.Application;

/// <summary>
/// Dependency injection extension for Application layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR with all handlers from this assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Register FluentValidation if needed
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
