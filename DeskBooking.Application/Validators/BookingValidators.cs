using System;
using DeskBooking.Core.DTOs;
using FluentValidation;

namespace DeskBooking.Application.Validators
{
    public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.UserEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.WorkspaceId)
                .NotEmpty();

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Start time must be in the future");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.EndTime)
                .Must((dto, endTime) => (endTime - dto.StartTime).TotalDays <= 30)
                .WithMessage("Booking duration cannot exceed 30 days");
        }
    }

    public class UpdateBookingDtoValidator : AbstractValidator<UpdateBookingDto>
    {
        public UpdateBookingDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.UserEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.WorkspaceId)
                .NotEmpty();

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Start time must be in the future");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.EndTime)
                .Must((dto, endTime) => (endTime - dto.StartTime).TotalDays <= 30)
                .WithMessage("Booking duration cannot exceed 30 days");
        }
    }
} 