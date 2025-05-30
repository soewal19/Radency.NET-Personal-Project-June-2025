using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeskBooking.Core.Entities;
using DeskBooking.Core.Interfaces;
using DeskBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.Workspace)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Workspace)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _context.Bookings.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Booking entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Bookings.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Booking entity)
        {
            _context.Bookings.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userEmail)
        {
            return await _context.Bookings
                .Include(b => b.Workspace)
                .Where(b => b.UserEmail == userEmail)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetWorkspaceBookingsAsync(Guid workspaceId, DateTime startTime, DateTime endTime)
        {
            return await _context.Bookings
                .Include(b => b.Workspace)
                .Where(b => b.WorkspaceId == workspaceId &&
                           b.Status != BookingStatus.Cancelled &&
                           ((b.StartTime <= startTime && b.EndTime > startTime) ||
                            (b.StartTime < endTime && b.EndTime >= endTime) ||
                            (b.StartTime >= startTime && b.EndTime <= endTime)))
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingBookingAsync(Guid workspaceId, DateTime startTime, DateTime endTime, Guid? excludeBookingId = null)
        {
            var query = _context.Bookings
                .Where(b => b.WorkspaceId == workspaceId &&
                           b.Status != BookingStatus.Cancelled &&
                           ((b.StartTime <= startTime && b.EndTime > startTime) ||
                            (b.StartTime < endTime && b.EndTime >= endTime) ||
                            (b.StartTime >= startTime && b.EndTime <= endTime)));

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync();
        }
    }
} 