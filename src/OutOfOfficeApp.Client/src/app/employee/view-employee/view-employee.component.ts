import {Component, Input} from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {EmployeeGetModel} from "../employee-get.model";
import {EmployeeService} from "../../employee.service";
import {ActivatedRoute, Router} from "@angular/router";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-view-employee',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './view-employee.component.html',
  styleUrl: './view-employee.component.css'
})
export class ViewEmployeeComponent {
  employeeForm: FormGroup;
 @Input({required: true}) employee!: EmployeeGetModel
  employeeId:number
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.employeeForm = this.fb.group({
      projectId: ['', Validators.required],
    });
    this.employeeId = 1;
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.employeeId = Number(params.get('id'));
      if (this.employeeId) {
        this.employeeService.getEmployee(String(this.employeeId)).subscribe({
          next: (employee) => {
            this.employee = employee;
          },
          error: (err) => {
            this.errorMessage = `Error occurred (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }
        })
      }
    });
  }

  onAssign() {
       if (this.employeeForm.valid) {
         const employeeData = this.employeeForm.value;
         this.employeeService.assignEmployeeToProject(this.employeeId, employeeData.projectId)
           .subscribe({
             next: () => {
               this.router.navigate(['/employees'])
                 .then(r => console.log('edited employee'));
             },
             error: (err) => {
               this.errorMessage = `Error occurred during assigning (${err.status})`;
               console.error(`${this.errorMessage} - ${err.message}`);
             }
           });
       } else {
         console.log('Form is not valid');
       }
  }

  onCancel() {
    this.router.navigate(['/employees'])
      .then(r => console.log('returned to employees list'));
  }
}
