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
import {ApprovalRequestsComponent} from "./approval-requests/approval-requests.component";
import {ApprovalRequestViewComponent} from "./approval-requests/approval-request-view/approval-request-view.component";

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  {
    path: 'employees',
    component: EmployeeComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager'] }
  },
  {
    path: 'add-employee',
    component: AddEmployeeComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager'] }
  },
  {
    path: 'edit-employee/:id',
    component: AddEmployeeComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager'] }
  },
  {
    path: 'view-employee/:id',
    component: ViewEmployeeComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager'] }
  },
  {
    path: 'projects',
    component: ProjectsComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager', 'Employee'] }
  },
  {
    path: 'add-project',
    component: ProjectFormComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'ProjectManager'] }
  },
  {
    path: 'edit-project/:id',
    component: ProjectFormComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'ProjectManager'] }
  },
  {
    path: 'view-project/:id',
    component: ProjectViewComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager', 'Employee'] }
  },
  {
    path: 'leave-requests',
    component: LeaveRequestsComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager', 'Employee'] }
  },
  {
    path: 'add-leave-request',
    component: LeaveRequestFormComponent,
    canActivate: [authGuard],
    data: { roles: ['Employee'] }
  },
  {
    path: 'edit-leave-request/:id',
    component: LeaveRequestFormComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'Employee'] }
  },
  {
    path: 'view-leave-request/:id',
    component: LeaveRequestViewComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager', 'Employee'] }
  },
  {
    path: 'approval-requests',
    component: ApprovalRequestsComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager', 'Employee'] }
  },
  {
    path: 'view-approval-request/:id',
    component: ApprovalRequestViewComponent,
    canActivate: [authGuard],
    data: { roles: ['Administrator', 'HRManager', 'ProjectManager', 'Employee'] }
  },
  { path: 'login', component: LoginComponent },
];
