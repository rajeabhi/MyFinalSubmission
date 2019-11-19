using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    [Table("Tasks")]
    public class Tasks
    {
        [Key]
        [Required]
        public int TaskId { get; set; }

        [StringLength(20)]
        [Required]
        public string Task { get; set; }

        public int ProjectId { get; set; }

        public int? ParentTaskId { get; set; }

        public int? Priority { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public int? UserId { get; set; }

        public bool IsEnded { get; set; }
    }
}
