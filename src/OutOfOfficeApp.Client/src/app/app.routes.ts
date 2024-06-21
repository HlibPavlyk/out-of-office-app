import { RouterModule, Routes } from '@angular/router';
import {AddEmployeeComponent} from "./employee/add-employee/add-employee.component";
import {EmployeeComponent} from "./employee/employee.component";

export const routes: Routes = [
  { path: 'employees', component: EmployeeComponent },
  { path: 'add-employee', component: AddEmployeeComponent },
  { path: '', redirectTo: '/employees', pathMatch: 'full' }
];
