using ProjectManager.Entity;
using ProjectManager.DAL;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectManager.BAL
{
    public class ProjectManagerBL
    {
        private readonly ProjectManagerDbContext _db;

        public ProjectManagerBL(ProjectManagerDbContext db)
        {
            _db = db;
        }

        #region Users
        public string AddUser(Users user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return "User Added Successfully";
        }

        public List<Users> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public Users GetUserById(int id)
        {
            return _db.Users.Find(id);
        }

        public string UpdateUser(Users user)
        {
            Users getUser = _db.Users.First(k => k.UserId == user.UserId);
            getUser.FirstName = user.FirstName;
            getUser.LastName = user.LastName;
            getUser.EmployeeId = user.EmployeeId;
            _db.SaveChanges();
            return "User Updated Successfully";
        }

        public string DeleteUser(int id)
        {
            if (_db.Tasks.Where(t => t.UserId == id).Count() > 0 || _db.Projects.Where(t => t.ManagerId == id).Count() > 0)
                throw new System.Exception("ERROR:User is in use for Task or Project.");

            Users user = _db.Users.Find(id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return "Users Deleted Successfully";
        }
        #endregion

        #region Projects
        public string AddProject(Projects project)
        {
            if (project.StartDate != null && project.EndDate != null)
            {
                project.StartDate = DateTime.SpecifyKind(project.StartDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
                project.EndDate = DateTime.SpecifyKind(project.EndDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
            }

            _db.Projects.Add(project);
            _db.SaveChanges();
            return "Project Added Successfully";
        }

        public List<DetailedProjects> GetAllProjects()
        {
            return _db.Projects.GroupJoin(_db.Users, r => r.ManagerId, q => q.UserId,
                (r, q) => new DetailedProjects
                {
                    ProjectId = r.ProjectId,
                    ProjectName = r.ProjectName,
                    ManagerId = r.ManagerId,
                    ManagerName = q.Select(a => a.FirstName + " " + a.LastName).FirstOrDefault(),
                    Priority = r.Priority,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    IsSuspended = r.IsSuspended,
                    TotalTasks = _db.Tasks.Where(t => t.ProjectId == r.ProjectId).Count(),
                    CompletedTasks = _db.Tasks.Where(t => t.ProjectId == r.ProjectId && t.IsEnded == true).Count()
                }).ToList();
        }

        public Projects GetProjectById(int id)
        {
            return _db.Projects.Find(id);
        }

        public List<ProjectList> GetProjectList()
        {
            return _db.Projects.Where(p => p.IsSuspended == false).Select(s => new ProjectList { ProjectId = s.ProjectId, ProjectName = s.ProjectName }).ToList();
        }

        public string UpdateProject(Projects project)
        {
            if (project.StartDate != null && project.EndDate != null)
            {
                project.StartDate = DateTime.SpecifyKind(project.StartDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
                project.EndDate = DateTime.SpecifyKind(project.EndDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
            }

            Projects getProject = _db.Projects.First(k => k.ProjectId == project.ProjectId);
            getProject.ProjectId = project.ProjectId;
            getProject.ProjectName = project.ProjectName;
            getProject.ManagerId = project.ManagerId;
            getProject.Priority = project.Priority;
            getProject.StartDate = project.StartDate;
            getProject.EndDate = project.EndDate;
            _db.SaveChanges();
            return "Project Updated Successfully";
        }

        public string SuspendProject(int id)
        {
            Projects project = _db.Projects.Find(id);
            project.IsSuspended = true;
            _db.SaveChanges();
            return "Project Suspended Successfully";
        }
        #endregion

        #region Tasks
        public string AddTask(Tasks task)
        {
            if (task.StartDate != null && task.EndDate != null)
            {
                task.StartDate = DateTime.SpecifyKind(task.StartDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
                task.EndDate = DateTime.SpecifyKind(task.EndDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
            }

            _db.Tasks.Add(task);
            _db.SaveChanges();
            return "Task Added Successfully";
        }

        public List<DetailedTasks> GetAllTasks(int id)
        {
            return _db.Tasks.Where(t => t.ProjectId == id)
                    .GroupJoin(_db.Users, t => t.UserId, u => u.UserId, (t, u) => new { t, u })
                    .GroupJoin(_db.Tasks, t => t.t.ParentTaskId, q => q.TaskId,
                        (r, q) => new DetailedTasks
                        {
                            TaskId = r.t.TaskId,
                            Task = r.t.Task,
                            ParentTaskId = r.t.ParentTaskId,
                            ParentTaskName = q.Select(a => a.Task).FirstOrDefault(),
                            Priority = r.t.Priority,
                            StartDate = r.t.StartDate,
                            EndDate = r.t.EndDate,
                            IsEnded = r.t.IsEnded,
                            UserId = r.t.UserId,
                            ProjectId = r.t.ProjectId,
                            UserName = r.u.Select(a => a.FirstName + " " + a.LastName).FirstOrDefault()
                        }).
                        ToList();
        }

        public Tasks GetTaskById(int id)
        {
            return _db.Tasks.Find(id);
        }

        public List<ParentTasks> GetParentTasks()
        {
            return _db.Tasks.Where(t => t.ParentTaskId == null).Select(s => new ParentTasks { ParentTaskId = s.TaskId, ParentTaskName = s.Task }).ToList();
        }

        public string UpdateTask(Tasks task)
        {
            if (task.StartDate != null && task.EndDate != null)
            {
                task.StartDate = DateTime.SpecifyKind(task.StartDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
                task.EndDate = DateTime.SpecifyKind(task.EndDate ?? DateTime.Now, DateTimeKind.Utc).ToLocalTime();
            }

            Tasks getTask = _db.Tasks.First(k => k.TaskId == task.TaskId);
            getTask.Task = task.Task;
            getTask.ParentTaskId = task.ParentTaskId;
            getTask.Priority = task.Priority;
            getTask.StartDate = task.StartDate;
            getTask.EndDate = task.EndDate;
            getTask.ProjectId = task.ProjectId;
            getTask.UserId = task.UserId;
            _db.SaveChanges();
            return "Task Updated Successfully";
        }

        public string EndTask(int id)
        {
            Tasks task = _db.Tasks.Find(id);
            task.IsEnded = true;
            _db.SaveChanges();
            return "Task Ended Successfully";
        }

        public string DeleteTask(int id)
        {
            Tasks task = _db.Tasks.Find(id);
            _db.Tasks.Remove(task);
            _db.SaveChanges();
            return "Task Deleted Successfully";
        }
        #endregion
    }
}
