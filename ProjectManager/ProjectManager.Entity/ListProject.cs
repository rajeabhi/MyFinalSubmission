using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Entity
{
    public class ProjectList
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
