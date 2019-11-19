using Moq;
using NBench;
using ProjectManager.Controllers;
using ProjectManager.DAL;
using ProjectManager.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ProjectManager.Test.Performance
{
    class PerformanceTest
    {
        private readonly Dictionary<int, int> dictionary = new Dictionary<int, int>();

        private const string AddCounterName = "AddCounter";
        private Counter addCounter;
        private int key;

        private const int AcceptableMinAddThroughput = 20000000;
        private const int IterationForPerfomanceTest = 1000;

        List<Users> userTestData = new List<Users>();
        List<Projects> projectTestData = new List<Projects>();
        List<Tasks> taskTestData = new List<Tasks>();
        Mock<ProjectManagerDbContext> dbContext = new Mock<ProjectManagerDbContext>();
        ProjectManagerController tb;

        [PerfSetup]
        public void Setup()
        {
            //addCounter = context.GetCounter(AddCounterName);
            //key = 0;

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

            tb = new ProjectManagerController(dbContext.Object);
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
        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetAllUser_IterationsMode()
        {
            var result = tb.GetAllUsers() as OkNegotiatedContentResult<List<Users>>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetUserByIdTest_IterationsMode()
        {
            var result = tb.GetUser(1) as OkNegotiatedContentResult<Users>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void AddUserTest_IterationsMode()
        {
            Users newUser = new Users() { UserId = 4, EmployeeId = "IT004", FirstName = "Robert", LastName = "Jose" };
            var result = tb.AddUser(newUser) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void UpdateUserTest_IterationsMode()
        {
            Users newUser = new Users() { UserId = 3, EmployeeId = "IT006", FirstName = "David", LastName = "Han" };
            var result = tb.UpdateUser(newUser) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void DeleteUserTest_IterationsMode()
        {
            var result = tb.DeleteUser(6) as OkNegotiatedContentResult<string>;
        }
        #endregion

        #region Projects
        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetAllProjectTest_IterationsMode()
        {
            var result = tb.GetAllProjects() as OkNegotiatedContentResult<List<DetailedProjects>>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetProjectByIdTask_IterationsMode()
        {
            var result = tb.GetProject(1) as OkNegotiatedContentResult<Projects>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetProjectListTest_IterationsMode()
        {
            var result = tb.GetProjectList() as OkNegotiatedContentResult<List<ProjectList>>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void AddProjectTest_IterationsMode()
        {
            Projects newProject = new Projects() { ProjectId = 4, ProjectName = "MEDICAL", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 1, Priority = 13, IsSuspended = false };
            var result = tb.AddProject(newProject) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void UpdateProjectTest_IterationsMode()
        {
            Projects newProject = new Projects() { ProjectId = 3, ProjectName = "BANKING", StartDate = new DateTime(), EndDate = new DateTime(), ManagerId = 3, Priority = 15, IsSuspended = true };
            var result = tb.UpdateProject(newProject) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void SuspendProjectTest_IterationsMode()
        {
            var result = tb.SuspendProject(1) as OkNegotiatedContentResult<string>;
        }
        #endregion
        
        #region Tasks
        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetAllTaskTest_IterationsMode()
        {
            var result = tb.GetAllTasks(2) as OkNegotiatedContentResult<List<DetailedTasks>>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetTaskByIdTask_IterationsMode()
        {
            var result = tb.GetTask(1) as OkNegotiatedContentResult<Tasks>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void GetParentTasks_IterationsMode()
        {
            var result = tb.GetParentTasks() as OkNegotiatedContentResult<List<ParentTasks>>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void AddTaskTest_IterationsMode()
        {
            Tasks newTask = new Tasks() { TaskId = 6, Task = "Task 6", ParentTaskId = 0, Priority = 5, StartDate = new DateTime(), EndDate = new DateTime(), IsEnded = true, ProjectId = 1, UserId = 3 };
            var result = tb.AddTask(newTask) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void UpdateTaskTest_IterationsMode()
        {
            Tasks newTask = new Tasks() { TaskId = 5, Task = "Task5", ProjectId = 1, ParentTaskId = 1, Priority = 7, StartDate = new DateTime(), EndDate = new DateTime(), UserId = 1, IsEnded = true };
            var result = tb.UpdateTask(newTask) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void EndTaskTest_IterationsMode()
        {
            var result = tb.EndTask(5) as OkNegotiatedContentResult<string>;
        }

        [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Test, NumberOfIterations = IterationForPerfomanceTest, RunTimeMilliseconds = 1000)]
        [ElapsedTimeAssertion(MinTimeMilliseconds = 0, MaxTimeMilliseconds = 1000)]
        public void DeleteTaskTest_IterationsMode()
        {
            var result = tb.DeleteTask(4) as OkNegotiatedContentResult<string>;
        }
        #endregion

        [PerfCleanup]
        public void Cleanup()
        {
            dictionary.Clear();
        }
    }

}

