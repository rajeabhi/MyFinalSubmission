import { Component, OnInit } from '@angular/core';
import { Project, ViewProject } from '../../models/project';
import { SharedService } from '../../services/shared.service';
import * as $ from 'jquery';
import 'datatables.net-bs4';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css']
})

export class ProjectComponent implements OnInit {
  item: Project;
  list: ViewProject[];
  msg: any;
  projectId: number;
  itemManager: any;
  chkSetDates: boolean;
  constructor(private _service: SharedService) {
    this.item = new Project();
    this.item.Priority = '0';
    this.msg = {};
    this.chkSetDates = false;
    this._service.GetAllUsers().subscribe(i => this.itemManager = i);
    this.GetAll();
  }

  ngOnInit() {
    $(document).ready(setTimeout(function () {
      $(function () {
        $('table').DataTable({ 'bPaginate': false, 'bInfo': false });
      });
    }, 500));
  }

  Add() {
    if (this.chkSetDates && this.item.StartDate > this.item.EndDate) {
      alert('Start Date cannot be greater than End Date.');
      return;
    }

    if (this.projectId == undefined) {
      this._service.AddProject(this.item).subscribe(i => { this.msg = i; this.GetAll(); });
    } else {
      this._service.UpdateProject(this.item).subscribe(i => { this.msg = i; this.GetAll(); this.projectId = undefined; });
    }
    this.Reset();
  }

  Edit(uId: number) {
    this.projectId = uId;
    this._service.GetProjectById(this.projectId).subscribe(i => { this.item = i; this.chkSetDates = i.StartDate == undefined ? false : true; });
  }

  Reset() {
    this.projectId = undefined;
    this.item = new Project();
    this.item.Priority = '0';
    this.chkSetDates = false;
  }

  GetAll() {
    this._service.GetAllProjects().subscribe(i => this.list = i);
  }

  Suspend(projectId: number) {
    this._service.SuspendProject(projectId).subscribe(i => { this.msg = i; this.GetAll(); });
  }

  checkChanged(e) {
    this.chkSetDates = e.target.checked;
    if (e.target.checked) {
      this.item.StartDate = new Date();
      this.item.EndDate = new Date((new Date().getTime() + (60 * 60 * 24 * 1000)));
    } else {
      this.item.StartDate = this.item.EndDate = undefined;
    }
    return;
  }
}
