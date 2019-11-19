using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Required]
        public int UserId { get; set; }

        [StringLength(10)]
        [Required]
        public string EmployeeId { get; set; }

        [StringLength(20)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(20)]
        [Required]
        public string LastName { get; set; }
    }
}
