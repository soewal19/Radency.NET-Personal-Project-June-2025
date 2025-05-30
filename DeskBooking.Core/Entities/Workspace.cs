using System;
using System.Collections.Generic;

namespace DeskBooking.Core.Entities
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public WorkspaceType Type { get; set; }
        public int Capacity { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Photos { get; set; }
        public bool IsAvailable { get; set; }
        public List<Booking> Bookings { get; set; }
    }

    public enum WorkspaceType
    {
        OpenSpace,
        PrivateRoom,
        MeetingRoom
    }
} 