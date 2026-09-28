using CricEco.GroundDashboard.Application.Common;
using CricEco.GroundDashboard.Application.DTOs;
using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;
using MediatR;

namespace CricEco.GroundDashboard.Application.Features.Bookings.Queries;

/// <summary>
/// Query to get all bookings
/// </summary>
public record GetAllBookingsQuery : IQuery<Result<IEnumerable<BookingDto>>>;

/// <summary>
/// Query to get bookings by ground ID
/// </summary>
public record GetBookingsByGroundQuery(Guid GroundId) : IQuery<Result<IEnumerable<BookingDto>>>;

/// <summary>
/// Query to get bookings by status
/// </summary>
public record GetBookingsByStatusQuery(string Status) : IQuery<Result<IEnumerable<BookingDto>>>;

/// <summary>
/// Query to get pending bookings
/// </summary>
public record GetPendingBookingsQuery : IQuery<Result<IEnumerable<BookingDto>>>;

/// <summary>
/// Query to get a single booking by ID
/// </summary>
public record GetBookingByIdQuery(Guid BookingId) : IQuery<Result<BookingDto>>;

/// <summary>
/// Query to get dashboard statistics
/// </summary>
public record GetDashboardStatsQuery(Guid? ManagerId = null) : IQuery<Result<DashboardStatsDto>>;

// Handler implementations
public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<IEnumerable<BookingDto>>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<IEnumerable<BookingDto>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
        var dtos = bookings.Select(BookingDto.FromEntity);
        return Result<IEnumerable<BookingDto>>.Success(dtos);
    }
}

public class GetBookingsByGroundQueryHandler : IRequestHandler<GetBookingsByGroundQuery, Result<IEnumerable<BookingDto>>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingsByGroundQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<IEnumerable<BookingDto>>> Handle(GetBookingsByGroundQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetByGroundIdAsync(request.GroundId, cancellationToken);
        var dtos = bookings.Select(BookingDto.FromEntity);
        return Result<IEnumerable<BookingDto>>.Success(dtos);
    }
}

public class GetBookingsByStatusQueryHandler : IRequestHandler<GetBookingsByStatusQuery, Result<IEnumerable<BookingDto>>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingsByStatusQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<IEnumerable<BookingDto>>> Handle(GetBookingsByStatusQuery request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<BookingStatus>(request.Status, true, out var status))
            return Result<IEnumerable<BookingDto>>.Failure("Invalid status");

        var bookings = await _bookingRepository.GetByStatusAsync(status, cancellationToken);
        var dtos = bookings.Select(BookingDto.FromEntity);
        return Result<IEnumerable<BookingDto>>.Success(dtos);
    }
}

public class GetPendingBookingsQueryHandler : IRequestHandler<GetPendingBookingsQuery, Result<IEnumerable<BookingDto>>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetPendingBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<IEnumerable<BookingDto>>> Handle(GetPendingBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetPendingAsync(cancellationToken);
        var dtos = bookings.Select(BookingDto.FromEntity);
        return Result<IEnumerable<BookingDto>>.Success(dtos);
    }
}

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        return Result<BookingDto>.Success(BookingDto.FromEntity(booking));
    }
}

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
{
    private readonly IGroundRepository _groundRepository;
    private readonly IBookingRepository _bookingRepository;

    public GetDashboardStatsQueryHandler(IGroundRepository groundRepository, IBookingRepository bookingRepository)
    {
        _groundRepository = groundRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Ground> grounds;
        if (request.ManagerId.HasValue)
            grounds = await _groundRepository.GetByManagerIdAsync(request.ManagerId.Value, cancellationToken);
        else
            grounds = await _groundRepository.GetAllAsync(cancellationToken);

        var allBookings = await _bookingRepository.GetAllAsync(cancellationToken);
        var groundsList = grounds.ToList();
        var bookingsList = allBookings.ToList();

        var pendingCount = bookingsList.Count(b => b.Status == BookingStatus.Requested);
        var confirmedBookings = bookingsList.Where(b => b.Status == BookingStatus.Approved).ToList();
        
        var totalRevenue = confirmedBookings.Sum(b => b.TotalAmount);

        var stats = new DashboardStatsDto
        {
            TotalGrounds = groundsList.Count,
            PendingRequests = pendingCount,
            ConfirmedBookings = confirmedBookings.Count,
            TotalRevenue = totalRevenue,
            RecentBookings = confirmedBookings
                .OrderByDescending(b => b.StartsAt)
                .Take(5)
                .Select(BookingDto.FromEntity)
                .ToList(),
            Grounds = groundsList
                .Take(4)
                .Select(GroundDto.FromEntity)
                .ToList()
        };

        return Result<DashboardStatsDto>.Success(stats);
    }
}
