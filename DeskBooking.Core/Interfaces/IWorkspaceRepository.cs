using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeskBooking.Core.Entities;

namespace DeskBooking.Core.Interfaces
{
    public interface IWorkspaceRepository : IRepository<Workspace>
    {
        Task<IEnumerable<Workspace>> GetAvailableWorkspacesAsync(DateTime startTime, DateTime endTime);
        Task<IEnumerable<Workspace>> GetWorkspacesByTypeAsync(WorkspaceType type);
        Task<bool> IsWorkspaceAvailableAsync(Guid workspaceId, DateTime startTime, DateTime endTime);
    }
} 