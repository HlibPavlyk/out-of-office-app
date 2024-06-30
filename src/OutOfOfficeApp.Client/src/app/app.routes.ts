import { Routes } from '@angular/router';
import {AddEmployeeComponent} from "./employee/add-employee/add-employee.component";
import {EmployeeComponent} from "./employee/employee.component";
import {LoginComponent} from "./login/login.component";
import {authGuard} from "./guards/auth.guard";
import {HomeComponent} from "./home/home.component";

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
  { path: 'login', component: LoginComponent },
];
