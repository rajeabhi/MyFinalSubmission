using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    public class DetailedTasks
    {
        public int TaskId { get; set; }
        public string Task { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? ParentTaskId { get; set; }
        public string ParentTaskName { get; set; }
        public int? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEnded { get; set; }
    }
}
