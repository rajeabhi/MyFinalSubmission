namespace ProjectManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(nullable: false, maxLength: 20),
                        Priority = c.Int(nullable: false),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        ManagerId = c.Int(nullable: false),
                        IsSuspended = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Task = c.String(nullable: false, maxLength: 20),
                        ProjectId = c.Int(nullable: false),
                        ParentTaskId = c.Int(),
                        Priority = c.Int(),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        UserId = c.Int(),
                        IsEnded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.String(nullable: false, maxLength: 10),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
            DropTable("dbo.Projects");
        }
    }
}
