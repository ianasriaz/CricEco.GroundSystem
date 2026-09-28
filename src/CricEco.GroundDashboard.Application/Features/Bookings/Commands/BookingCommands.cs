using CricEco.GroundDashboard.Application.Common;
using CricEco.GroundDashboard.Application.DTOs;
using CricEco.GroundDashboard.Domain.Entities;
using CricEco.GroundDashboard.Domain.Interfaces;
using MediatR;

namespace CricEco.GroundDashboard.Application.Features.Bookings.Commands;

/// <summary>
/// Command to create a new booking
/// </summary>
public record CreateBookingCommand(
    Guid GroundId,
    DateTime StartsAt,
    DateTime EndsAt,
    string CustomerName,
    string? CustomerPhone = null,
    bool IsWalkIn = false) : ICommand<Result<BookingDto>>;

/// <summary>
/// Command to update booking status (Approve/Reject/Cancel)
/// </summary>
public record UpdateBookingStatusCommand(
    Guid BookingId,
    string Status) : ICommand<Result<BookingDto>>;

/// <summary>
/// Command to delete a booking
/// </summary>
public record DeleteBookingCommand(Guid BookingId) : ICommand<Result>;

// Handler implementations
public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IGroundRepository _groundRepository;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IGroundRepository groundRepository)
    {
        _bookingRepository = bookingRepository;
        _groundRepository = groundRepository;
    }

    public async Task<Result<BookingDto>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Validate ground exists
        var ground = await _groundRepository.GetByIdAsync(request.GroundId, cancellationToken);
        if (ground == null)
            return Result<BookingDto>.Failure("Ground not found");

        // Validate time range
        if (request.EndsAt <= request.StartsAt)
            return Result<BookingDto>.Failure("End time must be after start time");

        if (request.StartsAt < DateTime.UtcNow)
            return Result<BookingDto>.Failure("Cannot book in the past");

        // Check for overlapping bookings
        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
            request.GroundId, 
            request.StartsAt, 
            request.EndsAt, 
            cancellationToken: cancellationToken);

        if (hasOverlap)
            return Result<BookingDto>.Failure("This time slot is already booked");

        var booking = Booking.Create(
            request.GroundId,
            request.StartsAt,
            request.EndsAt,
            request.CustomerName,
            request.CustomerPhone,
            isWalkIn: request.IsWalkIn);

        await _bookingRepository.AddAsync(booking, cancellationToken);

        // Return DTO with ground information populated
        var bookingDto = BookingDto.FromEntity(booking);
        bookingDto.GroundName = ground.Name;
        bookingDto.GroundLocation = ground.Location;

        return Result<BookingDto>.Success(bookingDto);
    }
}

public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, Result<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public UpdateBookingStatusCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<BookingDto>> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<BookingStatus>(request.Status, true, out var status))
            return Result<BookingDto>.Failure("Invalid status");

        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        
        if (booking == null)
            return Result<BookingDto>.Failure("Booking not found");

        // Apply status change
        switch (status)
        {
            case BookingStatus.Approved:
                booking.Approve();
                break;
            case BookingStatus.Rejected:
                booking.Reject();
                break;
            case BookingStatus.Cancelled:
                booking.Cancel();
                break;
            default:
                return Result<BookingDto>.Failure("Cannot change to this status");
        }

        await _bookingRepository.UpdateAsync(booking, cancellationToken);

        return Result<BookingDto>.Success(BookingDto.FromEntity(booking));
    }
}

public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;

    public DeleteBookingCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);
        
        if (booking == null)
            return Result.Failure("Booking not found");

        await _bookingRepository.DeleteAsync(request.BookingId, cancellationToken);
        return Result.Success();
    }
}
