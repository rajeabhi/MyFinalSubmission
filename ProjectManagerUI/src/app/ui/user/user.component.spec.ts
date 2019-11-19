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
import { User } from 'src/app/models/user';
import { UserComponent } from './user.component';

describe('UserComponent', () => {
  let component: UserComponent;
  let fixture: ComponentFixture<UserComponent>;

  class SharedServiceMock extends SharedService {
    GetAllUsers() {
      return new Observable<User[]>();
    }

    GetUserById() {
      return new Observable<User>();
    }

    AddUser(item: User) {
      return of('User Added Successfully');
    }

    UpdateUser(item: User) {
      return of('User Updated Successfully');
    }

    DeleteUser(item: number) {
      return of('User Deleted Successfully');
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UserComponent],
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
    fixture = TestBed.createComponent(UserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('Component Creation', () => {
    expect(component).toBeTruthy();
  });

  it('Add Function', () => {
    component.Add();
    expect(component.msg).toEqual('User Added Successfully');
  });

  it('Edit Function', () => {
    let a = new User();
    component.Edit(1);
    expect(component.item).toEqual(a);
  });

  it('Reset Function', () => {
    let a = {};
    component.Reset();
    expect(component.item.EmployeeId).toEqual(undefined);
    expect(component.msg).toEqual(a);
  });

  it('GetAll Function', () => {
    let a = new User();
    component.GetAll();
    expect(component.item).toEqual(a);
  });

  it('Delete Function', () => {
    component.Delete(1);
    expect(component.msg).toEqual('User Deleted Successfully');
  });
});
