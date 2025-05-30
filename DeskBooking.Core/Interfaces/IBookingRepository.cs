using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeskBooking.Core.Entities;

namespace DeskBooking.Core.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userEmail);
        Task<IEnumerable<Booking>> GetWorkspaceBookingsAsync(Guid workspaceId, DateTime startTime, DateTime endTime);
        Task<bool> HasOverlappingBookingAsync(Guid workspaceId, DateTime startTime, DateTime endTime, Guid? excludeBookingId = null);
    }
} 