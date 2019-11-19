import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as $ from 'jquery';
import 'datatables.net-bs4';
import { ViewTask } from '../../models/task';
import { SharedService } from '../../services/shared.service';

@Component({
  selector: 'app-view-task',
  templateUrl: './view-task.component.html',
  styleUrls: ['./view-task.component.css']
})
export class ViewTaskComponent implements OnInit {
  list: ViewTask[];
  ndate: Date;
  msg: any;
  projectId: number;
  itemProject: any;
  isDataTableInit: boolean;
  table: any;
  constructor(private router: Router, private _service: SharedService) {
    this.ndate = new Date();
    this.isDataTableInit = false;
    this._service.GetProjectList().subscribe(i => this.itemProject = i);
  }

  ngOnInit() {
  }

  AllowEnd(EndDate: string) {
    return (new Date(Date.parse(EndDate)) < this.ndate);
  }

  Edit(): void {
    this.router.navigateByUrl('add');
  }

  EndTask(TaskId: number) {
    this._service.EndTask(TaskId).subscribe(i => { this.msg = i; this.GetAllTasks(); });
  }

  Delete(TaskId: number) {
    this._service.DeleteTask(TaskId).subscribe(i => { this.msg = i; this.GetAllTasks(); });
  }

  GetAllTasks() {
    $('table').DataTable().destroy();
    this._service.GetAllTasks(this.projectId).subscribe(i => { this.list = i; this.SetDataTable(); });
  }

  SetDataTable() {
    setTimeout(function () {
      $(function () {
        $('table').DataTable({ 'bPaginate': false, 'bInfo': false });
      });
    }, 300);
  }
}
