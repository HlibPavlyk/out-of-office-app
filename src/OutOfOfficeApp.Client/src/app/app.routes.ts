import { Routes } from '@angular/router';
import {AddEmployeeComponent} from "./employee/add-employee/add-employee.component";
import {EmployeeComponent} from "./employee/employee.component";
import {LoginComponent} from "./login/login.component";
import {authGuard} from "./guards/auth.guard";
import {HomeComponent} from "./home/home.component";
import {ViewEmployeeComponent} from "./employee/view-employee/view-employee.component";
import {ProjectFormComponent} from "./projects/project-form/project-form.component";
import {ProjectsComponent} from "./projects/projects.component";
import {ProjectViewComponent} from "./projects/project-view/project-view.component";
import {LeaveRequestFormComponent} from "./leave-requests/leave-request-form/leave-request-form.component";
import {LeaveRequestViewComponent} from "./leave-requests/leave-request-view/leave-request-view.component";
import {LeaveRequestsComponent} from "./leave-requests/leave-requests.component";

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'employees',
    component: EmployeeComponent,
    canActivate: [authGuard]
  },
  { path: 'home', component: HomeComponent },
  { path: 'add-employee', component: AddEmployeeComponent },
  { path: 'edit-employee/:id', component: AddEmployeeComponent },
  { path: 'view-employee/:id', component: ViewEmployeeComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: 'add-project', component: ProjectFormComponent },
  { path: 'edit-project/:id', component: ProjectFormComponent },
  { path: 'view-project/:id', component: ProjectViewComponent },
  { path: 'leave-requests', component: LeaveRequestsComponent },
  { path: 'add-leave-request', component: LeaveRequestFormComponent },
  { path: 'edit-leave-request/:id', component: LeaveRequestFormComponent },
  { path: 'view-leave-request/:id', component: LeaveRequestViewComponent },
  { path: 'login', component: LoginComponent },
];
