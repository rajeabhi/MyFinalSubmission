using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ProjectManager.Controllers;
using ProjectManager.BAL;
using ProjectManager.DAL;
using ProjectManager.Entity;
using Assert = NUnit.Framework.Assert;
using System.Net.Http;
using System.Web.Http.Results;

namespace ProjectManager.Test.Controller
{
    [TestFixture]
    public class TestProjectManager
    {
        List<Users> userTestData = new List<Users>();
        List<Projects> projectTestData = new List<Projects>();
        List<Tasks> taskTestData = new List<Tasks>();
        Mock<ProjectManagerDbContext> dbContext = new Mock<ProjectManagerDbContext>();
        [SetUp]
        public void TestInitialize()
        {
            userTestData = new List<Users>() {
                new Users { UserId = 1, EmployeeId = "IT001", FirstName = "John", LastName = "Smith" },
                new Users { UserId = 2, EmployeeId = "IT002", FirstName = "Kevin", LastName = "Pratt" },
                new Users { UserId = 3, EmployeeId = "IT003", FirstName = "David", LastName = "Han" }
            };
            var setUserObject = GetQueryableMockDbSet(userTestData);
            dbContext.Setup(m => m.Set<Users>()).Returns(() => setUserObject);
            dbContext.Setup(m => m.SaveChanges());

            projectTestData = new List<Projects>() {
                new Projects { ProjectId = 1, ProjectName = "QA & E", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 1, Priority = 3, IsSuspended = false },
                new Projects { ProjectId = 2, ProjectName = "EDUCATION", ManagerId = 2, Priority = 3, IsSuspended = false },
                new Projects { ProjectId = 3, ProjectName = "BANK", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 3, Priority = 15, IsSuspended = true }
            };
            var setProjectObject = GetQueryableMockDbSet(projectTestData);
            dbContext.Setup(m => m.Set<Projects>()).Returns(() => setProjectObject);
            dbContext.Setup(m => m.SaveChanges());

            taskTestData = new List<Tasks>() {
                new Tasks { TaskId = 1, Task = "ParentTask1", ProjectId = 1, IsEnded = false },
                new Tasks { TaskId = 2, Task = "ParentTask2", ProjectId = 2, IsEnded = false },
                new Tasks { TaskId = 3, Task = "Task1", ProjectId = 1, ParentTaskId = 1, Priority = 9, StartDate = new DateTime(), EndDate = new DateTime(), UserId = 1, IsEnded = false },
                new Tasks { TaskId = 4, Task = "Task2", ProjectId = 2, ParentTaskId = 2, Priority = 16, StartDate = new DateTime(), EndDate = new DateTime(), UserId = 2, IsEnded = false },
                new Tasks { TaskId = 5, Task = "Task3", ProjectId = 1, ParentTaskId = 1, Priority = 7, StartDate = new DateTime(), EndDate = new DateTime(), UserId = 3, IsEnded = true }

            };
            var setTaskObject = GetQueryableMockDbSet(taskTestData);
            dbContext.Setup(m => m.Set<Tasks>()).Returns(() => setTaskObject);
            dbContext.Setup(m => m.SaveChanges());
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            //dbSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<T>(ids => queryable.FirstOrDefault(d => d. == (int)ids[0]));

            Type type = typeof(T);
            string colName = sourceList[0].GetType().GetProperties()[0].Name;
            var pk = type.GetProperty(colName);

            dbSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] id) =>
            {
                var param = Expression.Parameter(type, "t");
                var col = Expression.Property(param, colName);
                var body = Expression.Equal(col, Expression.Constant(id[0]));
                var lambda = Expression.Lambda<Func<T, bool>>(body, param);
                return queryable.FirstOrDefault(lambda);
            });
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            return dbSet.Object;
        }

        #region User
        [Test, Order(1)]
        public void GetAllUserTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetAllUsers() as OkNegotiatedContentResult<List<Users>>;
            Assert.AreEqual(result.Content.Count, 3);
            Assert.AreEqual(result.Content[0].EmployeeId, "IT001");
        }

        [Test, Order(2)]
        public void GetUserByIdTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetUser(1) as OkNegotiatedContentResult<Users>;
            Assert.AreEqual(result.Content.UserId, 1);
        }

        [Test, Order(4)]
        public void AddUserTest()
        {
            Users newUser = new Users() { UserId = 4, EmployeeId = "IT004", FirstName = "Robert", LastName = "Jose" };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.AddUser(newUser) as OkNegotiatedContentResult<string>;
            Assert.AreEqual(result.Content, "User Added Successfully");

            var confirmResult = tb.GetUser(4) as OkNegotiatedContentResult<Users>;
            Assert.AreEqual(confirmResult.Content, newUser);
        }

        [Test, Order(5)]
        public void UpdateUserTest()
        {
            Users newUser = new Users() { UserId = 3, EmployeeId = "IT006", FirstName = "David", LastName = "Han" };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.UpdateUser(newUser) as OkNegotiatedContentResult<string>;
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result.Content, "User Updated Successfully");

            var confirmResult = tb.GetUser(3) as OkNegotiatedContentResult<Users>;
            Assert.AreEqual(confirmResult.Content.EmployeeId, newUser.EmployeeId);
        }

        [Test, Order(7)]
        public void DeleteUserTest()
        {
            Users newUser = new Users() { UserId = 6, EmployeeId = "IT010", FirstName = "Eliss", LastName = "Sturn" };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            tb.AddUser(newUser);

            var result = tb.DeleteUser(6) as OkNegotiatedContentResult<string>;
            Assert.AreEqual(result.Content, "Users Deleted Successfully");
        }
        #endregion

        #region Project
        [Test, Order(1)]
        public void GetAllProjectTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetAllProjects() as OkNegotiatedContentResult<List<DetailedProjects>>;
            Assert.AreEqual(result.Content.Count, 3);
            Assert.AreEqual(result.Content[0].ProjectName, "QA & E");
        }

        [Test, Order(2)]
        public void GetProjectByIdTask()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetProject(1) as OkNegotiatedContentResult<Projects>;
            Assert.AreEqual(result.Content.ProjectId, 1);
        }

        [Test, Order(3)]
        public void GetProjectListTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetProjectList() as OkNegotiatedContentResult<List<ProjectList>>;
            Assert.AreEqual(result.Content.Count, 2);
        }

        [Test, Order(4)]
        public void AddProjectTest()
        {
            Projects newProject = new Projects() { ProjectId = 4, ProjectName = "MEDICAL", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 1, Priority = 13, IsSuspended = false };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.AddProject(newProject) as OkNegotiatedContentResult<string>;
            Assert.AreEqual(result.Content, "Project Added Successfully");

            var confirmResult = tb.GetProject(4) as OkNegotiatedContentResult<Projects>;
            Assert.AreEqual(confirmResult.Content, newProject);
        }

        [Test, Order(5)]
        public void UpdateProjectTest()
        {
            Projects newProject = new Projects() { ProjectId = 3, ProjectName = "BANKING", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 3, Priority = 15, IsSuspended = true };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.UpdateProject(newProject) as OkNegotiatedContentResult<string>;
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result.Content, "Project Updated Successfully");

            var confirmResult = tb.GetProject(3) as OkNegotiatedContentResult<Projects>;
            Assert.AreEqual(confirmResult.Content.ProjectName, newProject.ProjectName);
        }

        [Test, Order(6)]
        public void SuspendProjectTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.SuspendProject(1) as OkNegotiatedContentResult<string>;
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result.Content, "Project Suspended Successfully");

            var confirmResult = tb.GetProject(1) as OkNegotiatedContentResult<Projects>;
            Assert.AreEqual(confirmResult.Content.IsSuspended, true);
        }
        #endregion

        #region Tasks
        [Test, Order(1)]
        public void GetAllTaskTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object); 
            var result = tb.GetAllTasks(2) as OkNegotiatedContentResult<List<DetailedTasks>>;
            Assert.AreEqual(result.Content.Count, 2);
            Assert.AreEqual(result.Content[0].Task, "ParentTask2");
        }

        [Test, Order(2)]
        public void GetTaskByIdTask()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetTask(1) as OkNegotiatedContentResult<Tasks>;
            Assert.AreEqual(result.Content.TaskId, 1);
        }

        [Test, Order(3)]
        public void GetParentTasks()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.GetParentTasks() as OkNegotiatedContentResult<List<ParentTasks>>;
            Assert.AreEqual(result.Content.Count, 2);
        }

        [Test, Order(4)]
        public void AddTaskTest()
        {
            Tasks newTask = new Tasks() { TaskId = 6, Task = "Task 6", ParentTaskId = 0, Priority = 5, StartDate = new DateTime(), EndDate = new DateTime(), IsEnded = true, ProjectId = 1, UserId = 3 };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.AddTask(newTask) as OkNegotiatedContentResult<string>;
            Assert.AreEqual(result.Content, "Task Added Successfully");

            var confirmResult = tb.GetTask(6) as OkNegotiatedContentResult<Tasks>;
            Assert.AreEqual(confirmResult.Content, newTask);
        }

        [Test, Order(5)]
        public void UpdateTaskTest()
        {
            Tasks newTask = new Tasks() { TaskId = 5, Task = "Task5", ProjectId = 1, ParentTaskId = 1, Priority = 7, StartDate = new DateTime(), EndDate = new DateTime(), UserId = 1, IsEnded = true };
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.UpdateTask(newTask) as OkNegotiatedContentResult<string>;
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result.Content, "Task Updated Successfully");

            var confirmResult = tb.GetTask(5) as OkNegotiatedContentResult<Tasks>;
            Assert.AreEqual(confirmResult.Content.Task, newTask.Task);
        }

        [Test, Order(6)]
        public void EndTaskTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.EndTask(5) as OkNegotiatedContentResult<string>;
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result.Content, "Task Ended Successfully");

            var confirmResult = tb.GetTask(5) as OkNegotiatedContentResult<Tasks>;
            Assert.AreEqual(confirmResult.Content.IsEnded, true);
        }

        [Test, Order(7)]
        public void DeleteTaskTest()
        {
            ProjectManagerController tb = new ProjectManagerController(dbContext.Object);
            var result = tb.DeleteTask(4) as OkNegotiatedContentResult<string>;
            Assert.AreEqual(result.Content, "Task Deleted Successfully");
        }
        #endregion
    }
}
