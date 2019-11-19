//Recommitting to Git
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule, BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { MockBackend } from '@angular/http/testing';
import { Http } from '@angular/http';
import { SharedService } from '../../services/shared.service';
import { Observable, of } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { Task } from 'src/app/models/task';
import { User } from 'src/app/models/user';
import { AddTaskComponent } from './add-task.component';
import { By } from '@angular/platform-browser';

describe('AddTaskComponent', () => {
  let component: AddTaskComponent;
  let fixture: ComponentFixture<AddTaskComponent>;

  class SharedServiceMock extends SharedService {
    GetParentTasks() {
      return new Observable<any>();
    }

    GetAllUsers() {
      return new Observable<User[]>();
    }

    GetProjectList() {
      return new Observable<any>();
    }

    AddTask(item: Task) {
      return of('Task Added Successfully');
    }

    UpdateTask(item: Task) {
      return of('Task Updated Successfully');
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AddTaskComponent],
      imports: [FormsModule, BsDatepickerModule.forRoot(), RouterTestingModule],
      providers: [
        { provide: Http, deps: [MockBackend] },
        { provide: SharedService, useClass: SharedServiceMock },
        { provide: BsDatepickerConfig, useClass: BsDatepickerConfig }
      ],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('Component Creation', () => {
    expect(component).toBeTruthy();
  });

  it('Add Function', () => {
    component.Add();
    expect(component.msg).toEqual('Task Added Successfully');
  });

  it('Edit Function', () => {
    component.taskId = 1;
    component.Add();
    expect(component.msg).toEqual('Task Updated Successfully');
  });

  it('Reset Function', () => {
    let a = {};
    component.Reset();
    expect(component.item.Priority).toEqual('0');
    expect(component.item.ProjectId).toEqual(undefined);
    expect(component.item.Task).toEqual(undefined);
    expect(component.msg).toEqual(a);
  });

  it('SetDefaultDates Function', () => {
    component.SetDefaultDates();
    expect(component.item.StartDate.getDate()).toEqual((new Date()).getDate());
    expect(component.item.EndDate.getDate()).toEqual(new Date((new Date().getTime() + (60 * 60 * 24 * 1000))).getDate());
  });

  it('checkChanged TRUE Function', () => {
    let de = fixture.debugElement.query(By.css('.form-check-input'));
    let el = de.nativeElement;
    spyOn(component, 'checkChanged');
    el.click();
    expect(component.checkChanged).toHaveBeenCalled();
    expect(component.item.StartDate.getDate()).toEqual((new Date()).getDate());
    expect(component.item.EndDate.getDate()).toEqual(new Date((new Date().getTime() + (60 * 60 * 24 * 1000))).getDate());
  });

  it('checkChanged FALSE Function', () => {
    let de = fixture.debugElement.query(By.css('.form-check-input'));
    let el = de.nativeElement;
    spyOn(component, 'checkChanged');
    el.click();
    el.click();

    expect(component.item.ParentTaskId).toEqual(undefined);
    expect(component.item.UserId).toEqual(undefined);
    expect(component.item.Priority).toEqual('0');
  });
});
