//Recommitting to Git
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Http } from '@angular/http';
import { MockBackend } from '@angular/http/testing';
import { SharedService } from 'src/app/services/shared.service';
import { Observable, of } from 'rxjs';
import { ViewTask } from 'src/app/models/task';
import { Router } from '@angular/router';
import { ViewTaskComponent } from './view-task.component';
import { inject } from '@angular/core/testing';

class MockRouter {
  navigateByUrl(url: string) { return url; }
}

describe('ViewTaskComponent', () => {
  let component: ViewTaskComponent;
  let fixture: ComponentFixture<ViewTaskComponent>;

  class SharedServiceMock extends SharedService {
    GetAllTasks() {
      let list: ViewTask[] = [{ TaskId: 1, Task: 'ParentTask1', ProjectId: 1, ParentTaskId: undefined, Priority: undefined, StartDate: undefined, EndDate: undefined, UserId: undefined, IsEnded: false, ParentTaskName: undefined, ProjectName: 'QA & E', UserName: undefined },
      { TaskId: 2, Task: 'ParentTask2', ProjectId: 2, ParentTaskId: undefined, Priority: undefined, StartDate: undefined, EndDate: undefined, UserId: undefined, IsEnded: false, ParentTaskName: undefined, ProjectName: 'EDUCATION', UserName: undefined },
      { TaskId: 3, Task: 'Task1', ProjectId: 1, ParentTaskId: 1, Priority: '9', StartDate: new Date(), EndDate: new Date(), UserId: 1, IsEnded: false, ParentTaskName: 'ParentTask1', ProjectName: 'QA & E', UserName: 'John Smith' },
      { TaskId: 4, Task: 'Task1', ProjectId: 2, ParentTaskId: 2, Priority: '13', StartDate: new Date(), EndDate: new Date(), UserId: 2, IsEnded: false, ParentTaskName: 'ParentTask2', ProjectName: 'EDUCATION', UserName: 'Kevin Pratt' },
      { TaskId: 5, Task: 'Task1', ProjectId: 1, ParentTaskId: 1, Priority: '21', StartDate: new Date(), EndDate: new Date(), UserId: 1, IsEnded: true, ParentTaskName: 'ParentTask1', ProjectName: 'QA & E', UserName: 'John Smith' }];

      return new Observable<ViewTask[]>();
    }

    EndTask(item: any) {
      return of('Task Ended Successfully');
    }

    DeleteTask(item: any) {
      return of('Task Deleted Successfully');
    }

    GetProjectList() {
      return new Observable<any>();
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ViewTaskComponent],
      imports: [FormsModule, RouterTestingModule],
      providers: [
        { provide: Http, deps: [MockBackend] },
        { provide: Router, useClass: MockRouter },
        { provide: SharedService, useClass: SharedServiceMock },
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('Component Creation', () => {
    expect(component).toBeTruthy();
  });

  it('AllowEnd for Future Date', () => {
    let d = new Date();
    d.setDate(d.getDate() + 3);
    expect(component.AllowEnd(d.toDateString())).toBeFalsy();
  });

  it('AllowEnd for Past Date', () => {
    let d = new Date();
    d.setDate(d.getDate() - 3);
    expect(component.AllowEnd(d.toDateString())).toBeTruthy();
  });

  it('AllowEnd for Past Date', () => {
    let d = new Date();
    d.setDate(d.getDate() - 3);
    expect(component.AllowEnd(d.toDateString())).toBeTruthy();
  });

  it('Edit Function', inject([Router], (router: Router) => {
    const spy = spyOn(router, 'navigateByUrl');
    component.Edit();
    const url = spy.calls.first().args[0];
    console.log(url);
    expect(url).toBe('add');
  }));

  it('EndTask Function', () => {
    component.EndTask(1);
    expect(component.msg).toEqual('Task Ended Successfully');
  });

  it('Delete Function', () => {
    component.Delete(1);
    expect(component.msg).toEqual('Task Deleted Successfully');
  });
});
