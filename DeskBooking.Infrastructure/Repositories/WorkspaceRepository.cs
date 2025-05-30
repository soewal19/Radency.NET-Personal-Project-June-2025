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
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkspaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Workspace> GetByIdAsync(Guid id)
        {
            return await _context.Workspaces
                .Include(w => w.Bookings)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Workspace>> GetAllAsync()
        {
            return await _context.Workspaces
                .Include(w => w.Bookings)
                .ToListAsync();
        }

        public async Task<Workspace> AddAsync(Workspace entity)
        {
            await _context.Workspaces.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Workspace entity)
        {
            _context.Workspaces.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Workspace entity)
        {
            _context.Workspaces.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Workspace>> GetAvailableWorkspacesAsync(DateTime startTime, DateTime endTime)
        {
            return await _context.Workspaces
                .Include(w => w.Bookings)
                .Where(w => w.IsAvailable && !w.Bookings.Any(b =>
                    b.Status != BookingStatus.Cancelled &&
                    ((b.StartTime <= startTime && b.EndTime > startTime) ||
                     (b.StartTime < endTime && b.EndTime >= endTime) ||
                     (b.StartTime >= startTime && b.EndTime <= endTime))))
                .ToListAsync();
        }

        public async Task<IEnumerable<Workspace>> GetWorkspacesByTypeAsync(WorkspaceType type)
        {
            return await _context.Workspaces
                .Include(w => w.Bookings)
                .Where(w => w.Type == type)
                .ToListAsync();
        }

        public async Task<bool> IsWorkspaceAvailableAsync(Guid workspaceId, DateTime startTime, DateTime endTime)
        {
            var workspace = await _context.Workspaces
                .Include(w => w.Bookings)
                .FirstOrDefaultAsync(w => w.Id == workspaceId);

            if (workspace == null || !workspace.IsAvailable)
                return false;

            return !workspace.Bookings.Any(b =>
                b.Status != BookingStatus.Cancelled &&
                ((b.StartTime <= startTime && b.EndTime > startTime) ||
                 (b.StartTime < endTime && b.EndTime >= endTime) ||
                 (b.StartTime >= startTime && b.EndTime <= endTime)));
        }
    }
} 