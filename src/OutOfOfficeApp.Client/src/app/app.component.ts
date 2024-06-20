import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {EmployeeComponent} from "./employee/employee.component";
import {HttpClientModule} from "@angular/common/http";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, EmployeeComponent, HttpClientModule ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'OutOfOfficeApp.Client';
}
