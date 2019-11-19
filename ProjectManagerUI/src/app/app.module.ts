import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { AppComponent } from './app.component';
import { UserComponent } from './ui/user/user.component';
import { ProjectComponent } from './ui/project/project.component';
import { ViewTaskComponent } from './ui/view-task/view-task.component';
import { AddTaskComponent } from './ui/add-task/add-task.component';
import { FilterPipe } from './pipes/filter.pipe';
import { HeaderComponent } from './ui/shared/header/header.component';
import { HomeComponent } from './ui/home/home.component';
import { SharedService } from './services/shared.service';

const routes: Routes = [
  { 'path': '', 'redirectTo': '/home', 'pathMatch': 'full' },
  { 'path': 'home', 'component': HomeComponent },
  { 'path': 'addtask', 'component': AddTaskComponent },
  { 'path': 'viewtask', 'component': ViewTaskComponent },
  { 'path': 'addtask/:tid', 'component': AddTaskComponent },
  { 'path': 'user', 'component': UserComponent },
  { 'path': 'project', 'component': ProjectComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    ProjectComponent,
    ViewTaskComponent,
    AddTaskComponent,
    FilterPipe,
    HeaderComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    NgbModule,
    RouterModule.forRoot(routes),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ButtonsModule.forRoot(),
    HttpModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
