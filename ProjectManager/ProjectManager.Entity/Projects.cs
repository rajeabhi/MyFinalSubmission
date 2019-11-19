using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    [Table("Projects")]
    public class Projects
    {
        [Key]
        [Required]
        public int ProjectId { get; set; }

        [StringLength(20)]
        [Required]
        public string ProjectName { get; set; }

        [Required]
        public int Priority { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public int ManagerId { get; set; }

        public bool IsSuspended { get; set; }
    }
}
