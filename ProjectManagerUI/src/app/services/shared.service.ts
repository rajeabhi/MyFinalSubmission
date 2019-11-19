import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Task, ViewTask } from '../models/task';
import { User } from '../models/user';
import { Project, ViewProject } from '../models/project';

@Injectable({
  providedIn: 'root'
})

export class SharedService {
  url: string = 'http: //localhost: 51083/';
  constructor(private _http: Http) { }

  //#region User
  AddUser(item: User): Observable<any> {
    return this._http.post(this.url + 'AddUser', item).pipe(map((response: Response) => <any>response.json()));
  }

  GetAllUsers(): Observable<User[]> {
    return this._http.get(this.url + 'GetAllUsers').pipe(map((response: Response) => <User[]>response.json()));
  }

  DeleteUser(Id: number): Observable<any> {
    return this._http.delete(this.url + 'DeleteUser/' + Id).pipe(map((response: Response) => <any>response.json()));
  }

  GetUserById(Id: number): Observable<User> {
    return this._http.get(this.url + 'GetUser/' + Id).pipe(map((response: Response) => <User>response.json()));
  }

  UpdateUser(item: User): Observable<any> {
    return this._http.put(this.url + 'UpdateUser', item).pipe(map((response: Response) => <any>response.json()));
  }
  //#endregion

  //#region Project
  AddProject(item: Project): Observable<any> {
    return this._http.post(this.url + 'AddProject', item).pipe(map((response: Response) => <any>response.json()));
  }

  GetAllProjects(): Observable<ViewProject[]> {
    return this._http.get(this.url + 'GetAllProjects').pipe(map((response: Response) => <ViewProject[]>response.json()));
  }

  SuspendProject(Id: number): Observable<any> {
    return this._http.get(this.url + 'SuspendProject/' + Id).pipe(map((response: Response) => <any>response.json()));
  }

  GetProjectById(Id: number): Observable<Project> {
    return this._http.get(this.url + 'GetProject/' + Id).pipe(map((response: Response) => <Project>response.json()));
  }

  UpdateProject(item: Project): Observable<any> {
    return this._http.put(this.url + 'UpdateProject', item).pipe(map((response: Response) => <any>response.json()));
  }

  GetProjectList(): Observable<any> {
    return this._http.get(this.url + 'GetProjectList').pipe(map((response: Response) => <any>response.json()));
  }
  //#endregion

  //#region Task
  GetAllTasks(Id: number): Observable<ViewTask[]> {
    return this._http.get(this.url + 'GetAllTasks/' + Id).pipe(map((response: Response) => <ViewTask[]>response.json()));
  }

  GetTaskById(Id: number): Observable<Task> {
    return this._http.get(this.url + 'GetTask/' + Id).pipe(map((response: Response) => <Task>response.json()));
  }

  GetParentTasks(): Observable<any> {
    return this._http.get(this.url + 'GetParentTasks').pipe(map((response: Response) => <any>response.json()));
  }

  AddTask(item: Task): Observable<any> {
    return this._http.post(this.url + 'AddTask', item).pipe(map((response: Response) => <any>response.json()));
  }

  DeleteTask(Id: number): Observable<any> {
    return this._http.delete(this.url + 'DeleteTask/' + Id).pipe(map((response: Response) => <any>response.json()));
  }

  UpdateTask(item: Task): Observable<any> {
    return this._http.put(this.url + 'UpdateTask', item).pipe(map((response: Response) => <any>response.json()));
  }

  EndTask(Id: number): Observable<any> {
    return this._http.get(this.url + 'EndTask/' + Id).pipe(map((response: Response) => <any>response.json()));
  }
  //#endregion
}
