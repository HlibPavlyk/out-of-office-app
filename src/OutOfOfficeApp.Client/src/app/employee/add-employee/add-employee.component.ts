import { Component } from '@angular/core';
import {EmployeePostModel} from "../../../dto/employee-post.model";
import {EmployeeService} from "../../employee.service";
import {Router} from "@angular/router";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule,
    NgForOf
  ],
  templateUrl: './add-employee.component.html',
  styleUrl: './add-employee.component.css'
})
export class AddEmployeeComponent {
  employeeForm: FormGroup;
  statuses: string[] = ['Active', 'Inactive'];
  positions: string[] = ['Employee', 'HRManager', 'ProjectManager', 'Administrator'];
  subdivisions: string[] = ['Development', 'Design', 'DevOps', 'Management'];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router
  ) {
    this.employeeForm = this.fb.group({
      fullName: ['', Validators.required],
      subdivision: ['', Validators.required],
      position: ['', Validators.required],
      status: ['', Validators.required],
      peoplePartnerId: [1, Validators.required],
      outOfOfficeBalance: [0, [Validators.required, Validators.min(0)]],
    });
  }

  onCancel() {
    this.router.navigate(['']).then(r => console.log('returned to employees list'));
  }

  onSubmit() {
    if (this.employeeForm.valid) {
      const employeeData = this.employeeForm.value;
      this.employeeService.addEmployee(employeeData).subscribe(() => {
        this.router.navigate(['']).then(r => console.log('navigated to employees list'));
      });
    } else {
      console.log('Form is not valid');
      // Додаткова логіка для обробки невалідної форми, якщо необхідно
    }
  }
}
