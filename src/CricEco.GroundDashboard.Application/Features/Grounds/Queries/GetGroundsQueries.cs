using CricEco.GroundDashboard.Application.Common;
using CricEco.GroundDashboard.Application.DTOs;
using CricEco.GroundDashboard.Domain.Interfaces;
using MediatR;

namespace CricEco.GroundDashboard.Application.Features.Grounds.Queries;

/// <summary>
/// Query to get all grounds for the current user
/// </summary>
public record GetAllGroundsQuery : IQuery<Result<IEnumerable<GroundDto>>>;

/// <summary>
/// Query to get grounds by manager ID
/// </summary>
public record GetGroundsByManagerQuery(Guid ManagerId) : IQuery<Result<IEnumerable<GroundDto>>>;

/// <summary>
/// Query to get a single ground by ID
/// </summary>
public record GetGroundByIdQuery(Guid GroundId) : IQuery<Result<GroundDto>>;

/// <summary>
/// Query to search grounds
/// </summary>
public record SearchGroundsQuery(string SearchTerm) : IQuery<Result<IEnumerable<GroundDto>>>;

// Handler implementations
public class GetAllGroundsQueryHandler : IRequestHandler<GetAllGroundsQuery, Result<IEnumerable<GroundDto>>>
{
    private readonly IGroundRepository _groundRepository;

    public GetAllGroundsQueryHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result<IEnumerable<GroundDto>>> Handle(GetAllGroundsQuery request, CancellationToken cancellationToken)
    {
        var grounds = await _groundRepository.GetAllAsync(cancellationToken);
        var dtos = grounds.Select(GroundDto.FromEntity);
        return Result<IEnumerable<GroundDto>>.Success(dtos);
    }
}

public class GetGroundsByManagerQueryHandler : IRequestHandler<GetGroundsByManagerQuery, Result<IEnumerable<GroundDto>>>
{
    private readonly IGroundRepository _groundRepository;

    public GetGroundsByManagerQueryHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result<IEnumerable<GroundDto>>> Handle(GetGroundsByManagerQuery request, CancellationToken cancellationToken)
    {
        var grounds = await _groundRepository.GetByManagerIdAsync(request.ManagerId, cancellationToken);
        var dtos = grounds.Select(GroundDto.FromEntity);
        return Result<IEnumerable<GroundDto>>.Success(dtos);
    }
}

public class GetGroundByIdQueryHandler : IRequestHandler<GetGroundByIdQuery, Result<GroundDto>>
{
    private readonly IGroundRepository _groundRepository;

    public GetGroundByIdQueryHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result<GroundDto>> Handle(GetGroundByIdQuery request, CancellationToken cancellationToken)
    {
        var ground = await _groundRepository.GetByIdAsync(request.GroundId, cancellationToken);
        
        if (ground == null)
            return Result<GroundDto>.Failure("Ground not found");

        return Result<GroundDto>.Success(GroundDto.FromEntity(ground));
    }
}

public class SearchGroundsQueryHandler : IRequestHandler<SearchGroundsQuery, Result<IEnumerable<GroundDto>>>
{
    private readonly IGroundRepository _groundRepository;

    public SearchGroundsQueryHandler(IGroundRepository groundRepository)
    {
        _groundRepository = groundRepository;
    }

    public async Task<Result<IEnumerable<GroundDto>>> Handle(SearchGroundsQuery request, CancellationToken cancellationToken)
    {
        var grounds = await _groundRepository.SearchAsync(request.SearchTerm, cancellationToken);
        var dtos = grounds.Select(GroundDto.FromEntity);
        return Result<IEnumerable<GroundDto>>.Success(dtos);
    }
}
