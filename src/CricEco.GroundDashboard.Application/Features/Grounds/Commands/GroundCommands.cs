using CricEco.GroundDashboard.Application.Common;
using CricEco.GroundDashboard.Application.DTOs;
using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;
using MediatR;

namespace CricEco.GroundDashboard.Application.Features.Grounds.Commands;

/// <summary>
/// Command to create a new ground
/// </summary>
public record CreateGroundCommand(
    string Name,
    string Location,
    decimal HourlyRate,
    string PitchType,
    List<string> Amenities,
    Guid? ManagerId = null) : ICommand<Result<GroundDto>>;

/// <summary>
/// Command to update an existing ground
/// </summary>
public record UpdateGroundCommand(
    Guid GroundId,
    string Name,
    string Location,
    decimal HourlyRate,
    string PitchType,
    List<string> Amenities) : ICommand<Result<GroundDto>>;

/// <summary>
/// Command to delete a ground
/// </summary>
public record DeleteGroundCommand(Guid GroundId) : ICommand<Result>;

// Handler implementations
public class CreateGroundCommandHandler : IRequestHandler<CreateGroundCommand, Result<GroundDto>>
{
    private readonly IGroundRepository _groundRepository;

    public CreateGroundCommandHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result<GroundDto>> Handle(CreateGroundCommand request, CancellationToken cancellationToken)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<GroundDto>.Failure("Name is required");

        if (string.IsNullOrWhiteSpace(request.Location))
            return Result<GroundDto>.Failure("Location is required");

        if (request.HourlyRate <= 0)
            return Result<GroundDto>.Failure("Hourly rate must be greater than 0");

        var ground = Ground.Create(
            request.Name.Trim(),
            request.Location.Trim(),
            request.HourlyRate,
            request.PitchType,
            request.Amenities,
            request.ManagerId);

        await _groundRepository.AddAsync(ground, cancellationToken);

        return Result<GroundDto>.Success(GroundDto.FromEntity(ground));
    }
}

public class UpdateGroundCommandHandler : IRequestHandler<UpdateGroundCommand, Result<GroundDto>>
{
    private readonly IGroundRepository _groundRepository;

    public UpdateGroundCommandHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result<GroundDto>> Handle(UpdateGroundCommand request, CancellationToken cancellationToken)
    {
        var ground = await _groundRepository.GetByIdAsync(request.GroundId, cancellationToken);
        
        if (ground == null)
            return Result<GroundDto>.Failure("Ground not found");

        ground.UpdateDetails(
            request.Name.Trim(),
            request.Location.Trim(),
            request.HourlyRate,
            request.PitchType);

        ground.UpdateAmenities(request.Amenities);

        await _groundRepository.UpdateAsync(ground, cancellationToken);

        return Result<GroundDto>.Success(GroundDto.FromEntity(ground));
    }
}

public class DeleteGroundCommandHandler : IRequestHandler<DeleteGroundCommand, Result>
{
    private readonly IGroundRepository _groundRepository;

    public DeleteGroundCommandHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result> Handle(DeleteGroundCommand request, CancellationToken cancellationToken)
    {
        var exists = await _groundRepository.ExistsAsync(request.GroundId, cancellationToken);
        
        if (!exists)
            return Result.Failure("Ground not found");

        await _groundRepository.DeleteAsync(request.GroundId, cancellationToken);
        return Result.Success();
    }
}
