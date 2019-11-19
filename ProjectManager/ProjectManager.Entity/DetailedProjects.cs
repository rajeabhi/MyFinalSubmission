using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    public class DetailedProjects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsSuspended { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
    }
}
