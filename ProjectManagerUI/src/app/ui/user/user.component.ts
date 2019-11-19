//Recommitting to Git
import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { SharedService } from '../../services/shared.service';
import * as $ from 'jquery';
import 'datatables.net-bs4';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})

export class UserComponent implements OnInit {
  item: User;
  list: User[];
  msg: any;
  userId: number;
  constructor(private _service: SharedService, private _active: ActivatedRoute, private router: Router) {
    this.item = new User();
    this.msg = {};
    this.GetAll();
  }

  ngOnInit() {
    $(document).ready(setTimeout(function () {
      $(function () {
        $('table').DataTable({ 'bPaginate': false, 'bInfo': false });
      });
    }, 500))
  }

  Add() {
    if (this.userId == undefined) {
      this._service.AddUser(this.item).subscribe(i => { this.msg = i; this.GetAll(); });
    } else {
      this._service.UpdateUser(this.item).subscribe(i => { this.msg = i; this.GetAll(); this.userId = undefined; });
    }
    this.item = new User();
  }

  Edit(uId: number) {
    this.userId = uId;
    this._service.GetUserById(this.userId).subscribe(i => this.item = i);
  }

  Reset() {
    this.userId = undefined;
    this.item = new User();
    this.msg = {};
  }

  GetAll() {
    this._service.GetAllUsers().subscribe(i => this.list = i);
  }

  Delete(UserId: number) {
    this._service.DeleteUser(UserId).subscribe(i => { this.msg = i; this.GetAll(); });
  }
}
