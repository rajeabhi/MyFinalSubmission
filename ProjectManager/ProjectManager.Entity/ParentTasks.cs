using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    public class ParentTasks
    {
        public int ParentTaskId { get; set; }
        public string ParentTaskName { get; set; }
    }
}
