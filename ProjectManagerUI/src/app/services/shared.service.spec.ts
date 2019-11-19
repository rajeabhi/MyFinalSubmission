import { Http, BaseRequestOptions, Response, ResponseOptions, RequestMethod, HttpModule } from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';
import { SharedService } from './shared.service';
import { User } from 'src/app/models/user';
import { ViewProject, Project } from 'src/app/models/project';
import { ViewTask, Task } from 'src/app/models/task';
import { TestBed, async, fakeAsync } from '@angular/core/testing';
import { HttpClient } from 'selenium-webdriver/http';

describe('SharedService', () => {
  let service: SharedService;
  let backend: MockBackend;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpModule],
      providers: [
        SharedService,
        MockBackend,
        BaseRequestOptions,
        {
          provide: Http,
          useFactory: (backendInstance: MockBackend, defaultOptions: BaseRequestOptions) => {
            return new Http(backendInstance, defaultOptions);
          },
          deps: [MockBackend, BaseRequestOptions]
        }
      ],
    });

    backend = TestBed.get(MockBackend);
    service = TestBed.get(SharedService);
  }));

  it('service created', () => {
    const service: SharedService = TestBed.get(SharedService);
    expect(service).toBeTruthy();
  });

  //#region User
  it('GetAllUsers', (done) => {
    let response = [{ UserId: 1, EmployeeId: 'IT001', FirstName: 'John', LastName: 'Smith' },
    { UserId: 2, EmployeeId: 'IT002', FirstName: 'Kevin', LastName: 'Pratt' },
    { UserId: 3, EmployeeId: 'IT003', FirstName: 'David', LastName: 'Han' }];

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetAllUsers().subscribe((response) => {
      expect(response.length).toEqual(3);
      expect(response[0].UserId).toEqual(1);
      expect(response[0].EmployeeId).toEqual('IT001');
      done();
    });
  });

  it('GetUserById', (done) => {
    let response = { UserId: 2, EmployeeId: 'IT002', FirstName: 'Kevin', LastName: 'Pratt' };

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetUserById(2).subscribe((response) => {
      expect(response.UserId).toEqual(2);
      expect(response.EmployeeId).toEqual('IT002');
      done();
    });
  });

  it('AddUser', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('User Added Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    let list: User;
    service.AddUser(list).subscribe((response) => {
      expect(response).toEqual('User Added Successfully');
      done();
    });
  });

  it('DeleteUser', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('User Deleted Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    service.DeleteUser(1).subscribe((response) => {
      expect(response).toEqual('User Deleted Successfully');
      done();
    });
  });

  it('UpdateUser', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('User Updated Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    let list: User;
    service.UpdateUser(list).subscribe((response) => {
      expect(response).toEqual('User Updated Successfully');
      done();
    });
  });
  //#endregion

  //#region Project 
  it('GetProject', (done) => {
    let response = [{ ProjectId: 1, ProjectName: 'QA & E', StartDate: new Date(), EndDate: new Date(), ManagerId: 1, ManagerName: 'John Smith', Priority: 3, IsSuspended: false },
    { ProjectId: 2, ProjectName: 'EDUCATION', StartDate: new Date(), EndDate: new Date(), ManagerId: 2, ManagerName: 'Kevin Pratt', Priority: 3, IsSuspended: false },
    { ProjectId: 3, ProjectName: 'BANK', StartDate: new Date(), EndDate: new Date(), ManagerId: 3, ManagerName: 'John Smith', Priority: 15, IsSuspended: true }];

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetAllProjects().subscribe((response) => {
      expect(response.length).toEqual(3);
      expect(response[0].ProjectId).toEqual(1);
      expect(response[0].ProjectName).toEqual('QA & E');
      expect(response[1].IsSuspended).toBeFalsy();
      done();
    });
  });

  it('GetProjectById', (done) => {
    let response = { ProjectId: 2, ProjectName: 'EDUCATION', StartDate: new Date(), EndDate: new Date(), ManagerId: 2, ManagerName: 'Kevin Pratt', Priority: 3, IsSuspended: false };

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetProjectById(2).subscribe((response) => {
      expect(response.ProjectId).toEqual(2);
      expect(response.IsSuspended).toBeFalsy();
      done();
    });
  });

  it('GetProjectList', (done) => {
    let response = [{ ProjectId: 1, ProjectName: 'QA & E' },
    { ProjectId: 2, ProjectName: 'EDUCATION' },
    { ProjectId: 3, ProjectName: 'BANK' }];

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetProjectList().subscribe((response) => {
      expect(response.length).toEqual(3);
      done();
    });
  });

  it('AddProject', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Project Added Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    let list: Project;
    service.AddProject(list).subscribe((response) => {
      expect(response).toEqual('Project Added Successfully');
      done();
    });
  });

  it('UpdateProject', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Project Updated Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    let list: Project;
    service.UpdateProject(list).subscribe((response) => {
      expect(response).toEqual('Project Updated Successfully');
      done();
    });
  });

  it('SuspendProject', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Project Suspended Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    service.SuspendProject(1).subscribe((response) => {
      expect(response).toEqual('Project Suspended Successfully');
      done();
    });
  });
  //#endregion

  //#region Task 
  it('GetAll', (done) => {
    let response = [{ TaskId: 1, Task: 'ParentTask1', ProjectId: 1, ParentTaskId: undefined, Priority: undefined, StartDate: undefined, EndDate: undefined, UserId: undefined, IsEnded: false, ParentTaskName: undefined, ProjectName: 'QA & E', UserName: undefined },
    { TaskId: 2, Task: 'ParentTask2', ProjectId: 2, ParentTaskId: undefined, Priority: undefined, StartDate: undefined, EndDate: undefined, UserId: undefined, IsEnded: false, ParentTaskName: undefined, ProjectName: 'EDUCATION', UserName: undefined },
    { TaskId: 3, Task: 'Task1', ProjectId: 1, ParentTaskId: 1, Priority: '9', StartDate: new Date(), EndDate: new Date(), UserId: 1, IsEnded: false, ParentTaskName: 'ParentTask1', ProjectName: 'QA & E', UserName: 'John Smith' },
    { TaskId: 4, Task: 'Task1', ProjectId: 2, ParentTaskId: 2, Priority: '13', StartDate: new Date(), EndDate: new Date(), UserId: 2, IsEnded: false, ParentTaskName: 'ParentTask2', ProjectName: 'EDUCATION', UserName: 'Kevin Pratt' },
    { TaskId: 5, Task: 'Task1', ProjectId: 1, ParentTaskId: 1, Priority: '21', StartDate: new Date(), EndDate: new Date(), UserId: 1, IsEnded: true, ParentTaskName: 'ParentTask1', ProjectName: 'QA & E', UserName: 'John Smith' }];

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetAllTasks(1).subscribe((response) => {
      expect(response.length).toEqual(5);
      expect(response[0].TaskId).toEqual(1);
      expect(response[1].IsEnded).toBeFalsy();
      done();
    });
  });

  it('GetTaskById', (done) => {
    let response = { TaskId: 3, Task: 'Task1', ProjectId: 1, ParentTaskId: 1, Priority: '9', StartDate: new Date(), EndDate: new Date(), UserId: 1, IsEnded: false, ParentTaskName: 'ParentTask1', ProjectName: 'QA & E', UserName: 'John Smith' };

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetTaskById(3).subscribe((response) => {
      expect(response.TaskId).toEqual(3);
      expect(response.IsEnded).toBeFalsy();
      done();
    });
  });

  it('GetParentTasks', (done) => {
    let response = [{ 'TaskId': 1, 'TaskName': 'U1' }, { 'TaskId': 2, 'TaskName': 'U2' }, { 'TaskId': 3, 'TaskName': 'U3' }];

    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify(response)
      });
      connection.mockRespond(new Response(options));
    });

    service.GetParentTasks().subscribe((response) => {
      expect(response.length).toEqual(3);
      done();
    });
  });

  it('AddTask', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Task Added Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    let list: Task;
    service.AddTask(list).subscribe((response) => {
      expect(response).toEqual('Task Added Successfully');
      done();
    });
  });

  it('DeleteTask', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Task Deleted Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    service.DeleteTask(1).subscribe((response) => {
      expect(response).toEqual('Task Deleted Successfully');
      done();
    });
  });

  it('UpdateTask', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Task Updated Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    let list: Task;
    service.UpdateTask(list).subscribe((response) => {
      expect(response).toEqual('Task Updated Successfully');
      done();
    });
  });

  it('EndTask', (done) => {
    backend.connections.subscribe((connection: MockConnection) => {
      let options = new ResponseOptions({
        body: JSON.stringify('Task Ended Successfully'),
        status: 200
      });
      connection.mockRespond(new Response(options));
    });

    service.EndTask(1).subscribe((response) => {
      expect(response).toEqual('Task Ended Successfully');
      done();
    });
  });
  //#endregion
});