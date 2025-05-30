using System;
using System.Threading.Tasks;
using DeskBooking.Core.DTOs;
using DeskBooking.Core.Entities;
using DeskBooking.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DeskBooking.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IValidator<CreateBookingDto> _createValidator;
        private readonly IValidator<UpdateBookingDto> _updateValidator;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IBookingRepository bookingRepository,
            IWorkspaceRepository workspaceRepository,
            IValidator<CreateBookingDto> createValidator,
            IValidator<UpdateBookingDto> updateValidator,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _workspaceRepository = workspaceRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var workspace = await _workspaceRepository.GetByIdAsync(dto.WorkspaceId);
            if (workspace == null)
            {
                throw new ArgumentException("Workspace not found");
            }

            if (!workspace.IsAvailable)
            {
                throw new InvalidOperationException("Workspace is not available");
            }

            if (await _bookingRepository.HasOverlappingBookingAsync(dto.WorkspaceId, dto.StartTime, dto.EndTime))
            {
                throw new InvalidOperationException("Selected time slot is not available");
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                WorkspaceId = dto.WorkspaceId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);

            return MapToDto(booking, workspace);
        }

        public async Task<BookingDto> UpdateBookingAsync(Guid id, UpdateBookingDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new ArgumentException("Booking not found");
            }

            var workspace = await _workspaceRepository.GetByIdAsync(dto.WorkspaceId);
            if (workspace == null)
            {
                throw new ArgumentException("Workspace not found");
            }

            if (!workspace.IsAvailable)
            {
                throw new InvalidOperationException("Workspace is not available");
            }

            if (await _bookingRepository.HasOverlappingBookingAsync(dto.WorkspaceId, dto.StartTime, dto.EndTime, id))
            {
                throw new InvalidOperationException("Selected time slot is not available");
            }

            booking.UserName = dto.UserName;
            booking.UserEmail = dto.UserEmail;
            booking.WorkspaceId = dto.WorkspaceId;
            booking.StartTime = dto.StartTime;
            booking.EndTime = dto.EndTime;
            booking.UpdatedAt = DateTime.UtcNow;

            await _bookingRepository.UpdateAsync(booking);

            return MapToDto(booking, workspace);
        }

        public async Task DeleteBookingAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                throw new ArgumentException("Booking not found");
            }

            booking.Status = BookingStatus.Cancelled;
            booking.UpdatedAt = DateTime.UtcNow;

            await _bookingRepository.UpdateAsync(booking);
        }

        private static BookingDto MapToDto(Booking booking, Workspace workspace)
        {
            return new BookingDto
            {
                Id = booking.Id,
                UserName = booking.UserName,
                UserEmail = booking.UserEmail,
                WorkspaceId = booking.WorkspaceId,
                WorkspaceName = workspace.Name,
                WorkspaceType = workspace.Type.ToString(),
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt
            };
        }
    }
} 