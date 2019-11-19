using System.Web.Http;
using ProjectManager.Entity;
using ProjectManager.DAL;
using ProjectManager.BAL;

namespace ProjectManager.Controllers
{
    public class ProjectManagerController : ApiController
    {
        private readonly ProjectManagerDbContext _db;

        public ProjectManagerController() { _db = new ProjectManagerDbContext(); }

        public ProjectManagerController(ProjectManagerDbContext db)
        {
            _db = db;
        }

        #region Users
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetAllUsers());
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        public IHttpActionResult GetUser(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetUserById(id));
        }

        [HttpPost]
        [Route("AddUser")]
        public IHttpActionResult AddUser(Users user)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.AddUser(user));
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser(Users user)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.UpdateUser(user));
        }

        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.DeleteUser(id));
        }
        #endregion

        #region Projects
        [Route("GetAllProjects")]
        public IHttpActionResult GetAllProjects()
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetAllProjects());
        }

        [HttpGet]
        [Route("GetProject/{id}")]
        public IHttpActionResult GetProject(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetProjectById(id));
        }

        [Route("GetProjectList")]
        public IHttpActionResult GetProjectList()
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetProjectList());
        }

        [HttpPost]
        [Route("AddProject")]
        public IHttpActionResult AddProject(Projects project)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.AddProject(project));
        }

        [HttpPut]
        [Route("UpdateProject")]
        public IHttpActionResult UpdateProject(Projects project)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.UpdateProject(project));
        }

        [HttpGet]
        [Route("SuspendProject/{id}")]
        public IHttpActionResult SuspendProject(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.SuspendProject(id));
        }
        #endregion  

        #region Tasks
        [Route("GetAllTasks/{id}")]
        public IHttpActionResult GetAllTasks(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetAllTasks(id));
        }

        [HttpGet]
        [Route("GetTask/{id}")]
        public IHttpActionResult GetTask(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetTaskById(id));
        }

        [Route("GetParentTasks")]
        public IHttpActionResult GetParentTasks()
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.GetParentTasks());
        }

        [HttpPost]
        [Route("AddTask")]
        public IHttpActionResult AddTask(Tasks task)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.AddTask(task));
        }

        [HttpPut]
        [Route("UpdateTask")]
        public IHttpActionResult UpdateTask(Tasks task)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.UpdateTask(task));
        }

        [HttpGet]
        [Route("EndTask/{id}")]
        public IHttpActionResult EndTask(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.EndTask(id));
        }

        [HttpDelete]
        [Route("DeleteTask/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            ProjectManagerBL obj = new ProjectManagerBL(_db);
            return Ok(obj.DeleteTask(id));
        }
        #endregion  
    }
}
