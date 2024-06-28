import { Routes } from '@angular/router';
import {AddEmployeeComponent} from "./employee/add-employee/add-employee.component";
import {EmployeeComponent} from "./employee/employee.component";
import {LoginComponent} from "./login/login.component";

export const routes: Routes = [
  { path: '', redirectTo: '/employees', pathMatch: 'full' },
  { path: 'employees', component: EmployeeComponent },
  { path: 'add-employee', component: AddEmployeeComponent },
  { path: 'edit-employee/:id', component: AddEmployeeComponent },
  { path: 'login', component: LoginComponent },
];
