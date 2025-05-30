using System;
using System.Collections.Generic;

namespace DeskBooking.Core.DTOs
{
    public class WorkspaceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Photos { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CreateWorkspaceDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Photos { get; set; }
    }

    public class UpdateWorkspaceDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Photos { get; set; }
        public bool IsAvailable { get; set; }
    }
} 