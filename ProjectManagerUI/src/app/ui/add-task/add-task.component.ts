import { Component, OnInit } from '@angular/core';
import { Task } from '../../models/task';
import { SharedService } from '../../services/shared.service';
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.css']
})

export class AddTaskComponent implements OnInit {
  item: Task;
  itemParentTask: any;
  itemUser: any;
  itemProject: any;
  chkSetDates: boolean;
  msg: any;
  taskId: number;
  constructor(private _service: SharedService, private _active: ActivatedRoute) {
    this.Reset();
    this.msg = {};
    this.taskId = undefined;
    this._active.params.subscribe(k => this.taskId = k['tid']);
    if (this.taskId != undefined) {
      this._service.GetTaskById(this.taskId).subscribe(i => { this.item = i; this.chkSetDates = (this.item.StartDate == undefined) });
    }
  }

  ngOnInit() {
  }

  PreLoad() {
    this._service.GetParentTasks().subscribe(i => this.itemParentTask = i);
    this._service.GetAllUsers().subscribe(i => this.itemUser = i);
    this._service.GetProjectList().subscribe(i => this.itemProject = i);
  }

  Add() {
    if ((this.item.StartDate != undefined || this.item.EndDate != undefined) && this.item.StartDate > this.item.EndDate) {
      alert('Start Date cannot be greater than End Date.');
      return;
    }
    if (this.taskId == undefined) {
      this._service.AddTask(this.item).subscribe(i => { this.msg = i; this.Reset(); });
    } else {
      this._service.UpdateTask(this.item).subscribe(i => this.msg = i);
    }
  }

  Reset() {
    this.item = new Task();
    this.PreLoad();
    this.item.Priority = '0';
    this.chkSetDates = false;
    this.SetDefaultDates();
  }

  checkChanged(e) {
    this.chkSetDates = e.target.checked;
    if (!e.target.checked) {
      this.SetDefaultDates();
    } else {
      this.item.StartDate = undefined;
      this.item.EndDate = undefined;
    }
    this.item.ParentTaskId = undefined;
    this.item.UserId = undefined;
    this.item.Priority = '0';
    return;
  }

  SetDefaultDates() {
    this.item.StartDate = new Date();
    this.item.EndDate = new Date((new Date().getTime() + (60 * 60 * 24 * 1000)));
  }
}
