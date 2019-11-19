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
import { Project, ViewProject } from 'src/app/models/project';
import { User } from 'src/app/models/user';
import { ProjectComponent } from './project.component';
import { By } from '@angular/platform-browser';

describe('ProjectComponent', () => {
  let component: ProjectComponent;
  let fixture: ComponentFixture<ProjectComponent>;

  class SharedServiceMock extends SharedService {
    GetAllProjects() {
      return new Observable<ViewProject[]>();
    }

    GetProjectById(item: number) {
      return new Observable<Project>();
    }

    AddProject(item: Project) {
      return of('Project Added Successfully');
    }

    UpdateProject(item: Project) {
      return of('Project Updated Successfully');
    }

    SuspendProject(item: number) {
      return of('Project Suspended Successfully');
    }

    GetAllUsers() {
      return new Observable<User[]>();
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProjectComponent],
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
    fixture = TestBed.createComponent(ProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('Component Creation', () => {
    expect(component).toBeTruthy();
  });

  it('Add Function', () => {
    component.Add();
    expect(component.msg).toEqual('Project Added Successfully');
  });

  it('Edit Function', () => {
    let a = new Project();
    a.Priority = '0';
    component.Edit(1);
    expect(component.item).toEqual(a);
  });

  it('Reset Function', () => {
    let a = {};
    component.Reset();
    expect(component.item.ProjectId).toEqual(undefined);
    expect(component.item.ProjectName).toEqual(undefined);
    expect(component.item.Priority).toEqual('0');
    expect(component.msg).toEqual(a);
  });

  it('GetAll Function', () => {
    let a = new Project();
    a.Priority = '0';
    component.GetAll();
    expect(component.item).toEqual(a);
  });

  it('Suspend Function', () => {
    component.Suspend(1);
    expect(component.msg).toEqual('Project Suspended Successfully');
  });

  it('checkChanged TRUE Function', () => {
    let de = fixture.debugElement.query(By.css('.form-check-input'));
    let el = de.nativeElement;
    spyOn(component, 'checkChanged');
    el.click();
    expect(component.checkChanged).toHaveBeenCalled();
  });

  it('checkChanged FALSE Function', () => {
    let de = fixture.debugElement.query(By.css('.form-check-input'));
    let el = de.nativeElement;
    spyOn(component, 'checkChanged');
    el.click();
    el.click();

    expect(component.item.StartDate).toEqual(undefined);
    expect(component.item.EndDate).toEqual(undefined);
  });
});
