import { Component } from '@angular/core';
import {RouterLink, RouterOutlet} from '@angular/router';
import {EmployeeComponent} from "./employee/employee.component";
import {HttpClientModule} from "@angular/common/http";
import {NavbarComponent} from "./navbar/navbar.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, EmployeeComponent, HttpClientModule, RouterLink, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'OutOfOfficeApp.Client';
}
