using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ProjectManager.BAL;
using ProjectManager.DAL;
using ProjectManager.Entity;
using Assert = NUnit.Framework.Assert;

namespace ProjectManager.Test.BusinessLogic
{
    [TestFixture]
    public class TestProjectManager
    {
        DateTime testDate = new DateTime();
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
                new Projects { ProjectId = 1, ProjectName = "QA & E", StartDate = testDate, EndDate = testDate, ManagerId = 1, Priority = 3, IsSuspended = false },
                new Projects { ProjectId = 2, ProjectName = "EDUCATION", ManagerId = 2, Priority = 3, IsSuspended = false },
                new Projects { ProjectId = 3, ProjectName = "BANK", StartDate = testDate, EndDate = testDate, ManagerId = 3, Priority = 15, IsSuspended = true }
            };
            var setProjectObject = GetQueryableMockDbSet(projectTestData);
            dbContext.Setup(m => m.Set<Projects>()).Returns(() => setProjectObject);
            dbContext.Setup(m => m.SaveChanges());

            taskTestData = new List<Tasks>() {
                new Tasks { TaskId = 1, Task = "ParentTask1", ProjectId = 1, IsEnded = false },
                new Tasks { TaskId = 2, Task = "ParentTask2", ProjectId = 2, IsEnded = false },
                new Tasks { TaskId = 3, Task = "Task1", ProjectId = 1, ParentTaskId = 1, Priority = 9, StartDate = testDate, EndDate = testDate, UserId = 1, IsEnded = false },
                new Tasks { TaskId = 4, Task = "Task2", ProjectId = 2, ParentTaskId = 2, Priority = 16, StartDate = testDate, EndDate = testDate, UserId = 2, IsEnded = false },
                new Tasks { TaskId = 5, Task = "Task3", ProjectId = 1, ParentTaskId = 1, Priority = 7, StartDate = testDate, EndDate = testDate, UserId = 3, IsEnded = true }

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

        #region Users
        [Test, Order(1)]
        public void GetAllUserTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            List<Users> result = tb.GetAllUsers();
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[1].UserId, 2);
            Assert.AreEqual(result[1].EmployeeId, "IT002");
            Assert.AreEqual(result[1].FirstName, "Kevin");
            Assert.AreEqual(result[1].LastName, "Pratt");
        }

        [Test, Order(2)]
        public void GetUserByIdTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            Users result = tb.GetUserById(1);
            Assert.AreEqual(result.UserId, 1);
        }

        [Test, Order(4)]
        public void AddUserTest()
        {
            Users newUser = new Users() { UserId = 4, EmployeeId = "IT004", FirstName = "Robert", LastName = "Jose" };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.AddUser(newUser);
            Assert.AreEqual(result, "User Added Successfully");

            Users confirmResult = tb.GetUserById(4);
            Assert.AreEqual(confirmResult, newUser);
        }

        [Test, Order(5)]
        public void UpdateUserTest()
        {
            Users newUser = new Users() { UserId = 3, EmployeeId = "IT006", FirstName = "David", LastName = "Han" };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.UpdateUser(newUser);
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result, "User Updated Successfully");

            Users confirmResult = tb.GetUserById(3);
            Assert.AreEqual(confirmResult.EmployeeId, newUser.EmployeeId);
        }

        [Test, Order(7)]
        public void DeleteUserErrorTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = "";
            try { tb.DeleteUser(3); } catch (Exception ex) { result = ex.Message; }
            Assert.AreEqual(result, "ERROR:User is in use for Task or Project.");
        }

        [Test, Order(8)]
        public void DeleteUserSuccessTest()
        {
            Users newUser = new Users() { UserId = 4, EmployeeId = "IT004", FirstName = "Robert", LastName = "Jose" };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            tb.AddUser(newUser);

            string result = tb.DeleteUser(10);
            Assert.AreEqual(result, "Users Deleted Successfully");
        }
        #endregion

        #region Projects
        [Test, Order(1)]
        public void GetAllProjectTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            List<DetailedProjects> result = tb.GetAllProjects();
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[0].ProjectId, 1);
            Assert.AreEqual(result[0].ProjectName, "QA & E");
            Assert.AreEqual(result[0].Priority, 3);
            Assert.AreEqual(result[0].StartDate, testDate);
            Assert.AreEqual(result[0].EndDate, testDate);
            Assert.AreEqual(result[0].ManagerId, 1);
            Assert.AreEqual(result[0].IsSuspended, false);
            Assert.AreEqual(result[0].TotalTasks, 3);
            Assert.AreEqual(result[0].CompletedTasks, 1);
            Assert.AreEqual(result[0].ManagerName, "John Smith");

        }

        [Test, Order(2)]
        public void GetProjectByIdTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            Projects result = tb.GetProjectById(1);
            Assert.AreEqual(result.ProjectId, 1);
        }

        [Test, Order(3)]
        public void GetProjectListTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            List<ProjectList> result = tb.GetProjectList();
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].ProjectId, 1);
            Assert.AreEqual(result[0].ProjectName, "QA & E");
        }

        [Test, Order(4)]
        public void AddProjectTest()
        {
            Projects newProject = new Projects() { ProjectId = 4, ProjectName = "MEDICAL", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 1, Priority = 13, IsSuspended = false };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.AddProject(newProject);
            Assert.AreEqual(result, "Project Added Successfully");

            Projects confirmResult = tb.GetProjectById(4);
            Assert.AreEqual(confirmResult, newProject);
        }

        [Test, Order(5)]
        public void UpdateProjectTest()
        {
            Projects newProject = new Projects() { ProjectId = 3, ProjectName = "BANKING", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 3, Priority = 15, IsSuspended = true };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.UpdateProject(newProject);
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result, "Project Updated Successfully");

            Projects confirmResult = tb.GetProjectById(3);
            Assert.AreEqual(confirmResult.ProjectName, newProject.ProjectName);
        }

        [Test, Order(6)]
        public void SuspendProjectTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.SuspendProject(1);
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result, "Project Suspended Successfully");

            Projects confirmResult = tb.GetProjectById(1);
            Assert.AreEqual(confirmResult.IsSuspended, true);
        }
        #endregion

        #region Tasks
        [Test, Order(1)]
        public void GetAllTaskTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            List<DetailedTasks> result = tb.GetAllTasks(1);
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual(result[1].TaskId, 3);
            Assert.AreEqual(result[1].Task, "Task1");
            Assert.AreEqual(result[1].ProjectId, 1);
            Assert.AreEqual(result[1].ParentTaskId, 1);
            Assert.AreEqual(result[1].Priority, 9);
            Assert.AreEqual(result[1].StartDate, testDate);
            Assert.AreEqual(result[1].EndDate, testDate);
            Assert.AreEqual(result[1].UserId, 1);
            Assert.AreEqual(result[1].IsEnded, false);
            Assert.AreEqual(result[1].ParentTaskName, "ParentTask1");
            Assert.AreEqual(result[1].UserName, "John Smith");
            Assert.AreEqual(result[1].ProjectName, null);
        }

        [Test, Order(2)]
        public void GetTaskByIdTask()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            Tasks result = tb.GetTaskById(1);
            Assert.AreEqual(result.TaskId, 1);
        }

        [Test, Order(3)]
        public void GetParentTasks()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            List<ParentTasks> result = tb.GetParentTasks();
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(result[0].ParentTaskId, 1);
            Assert.AreEqual(result[0].ParentTaskName, "ParentTask1");
        }

        [Test, Order(4)]
        public void AddTaskTest()
        {
            Tasks newTask = new Tasks() { TaskId = 7, Task = "Task 6", ParentTaskId = 0, Priority = 5, StartDate = new DateTime(), EndDate = new DateTime(), IsEnded = true, ProjectId = 1, UserId = 3 };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.AddTask(newTask);
            Assert.AreEqual(result, "Task Added Successfully");

            Tasks confirmResult = tb.GetTaskById(7);
            Assert.AreEqual(confirmResult, newTask);
        }

        [Test, Order(5)]
        public void UpdateTaskTest()
        {
            Tasks newTask = new Tasks() { TaskId = 5, Task = "Task5", ProjectId = 1, ParentTaskId = 1, Priority = 7, StartDate = new DateTime(), EndDate = new DateTime(), UserId = 1, IsEnded = true };
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.UpdateTask(newTask);
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result, "Task Updated Successfully");

            Tasks confirmResult = tb.GetTaskById(5);
            Assert.AreEqual(confirmResult.Task, newTask.Task);
        }

        [Test, Order(6)]
        public void EndTaskTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.EndTask(1);
            dbContext.Verify(x => x.SaveChanges());
            Assert.AreEqual(result, "Task Ended Successfully");

            Tasks confirmResult = tb.GetTaskById(1);
            Assert.AreEqual(confirmResult.IsEnded, true);
        }

        [Test, Order(7)]
        public void DeleteTaskTest()
        {
            ProjectManagerBL tb = new ProjectManagerBL(dbContext.Object);
            string result = tb.DeleteTask(3);
            Assert.AreEqual(result, "Task Deleted Successfully");
        }
        #endregion
    }
}
