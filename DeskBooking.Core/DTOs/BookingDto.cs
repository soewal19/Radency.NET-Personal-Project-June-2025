using System;

namespace DeskBooking.Core.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public string WorkspaceType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateBookingDto
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid WorkspaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class UpdateBookingDto
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid WorkspaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
} 