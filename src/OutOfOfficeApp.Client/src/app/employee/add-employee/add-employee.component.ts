import {Component, OnInit} from '@angular/core';
import {EmployeeService} from "../../employee.service";
import {ActivatedRoute, Router} from "@angular/router";
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
export class AddEmployeeComponent implements OnInit{
  employeeForm: FormGroup;
  statuses: string[] = ['Active', 'Inactive'];
  positions: string[] = ['Employee', 'HRManager', 'ProjectManager', 'Administrator'];
  subdivisions: string[] = ['Development', 'Design', 'DevOps', 'Management'];
  errorMessage = '';
  isEditMode = false;
  employeeId = 1;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute
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

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.employeeId = Number(params.get('id'));
      if (this.employeeId) {
        this.isEditMode = true;
        this.employeeService.getEmployee(String(this.employeeId)).subscribe({
          next: (employee) => this.employeeForm.patchValue({
            fullName: employee.fullName,
            subdivision: employee.subdivision,
            position: employee.position,
            status: employee.status,
            peoplePartnerId: employee.peoplePartner.id,
            outOfOfficeBalance: employee.outOfOfficeBalance
          }),
          error: (err) => {
            this.errorMessage = `Error occurred (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }})
      }
    });
  }

  onCancel() {
    this.router.navigate([''])
      .then(r => console.log('returned to employees list'));
  }

  onSubmit() {
    if (this.employeeForm.valid) {
      const employeeData = this.employeeForm.value;
      if (this.isEditMode) {
        this.employeeService.editEmployee(this.employeeId, employeeData)
          .subscribe({
            next: () => {
              this.router.navigate([''])
                .then(r => console.log('edited employee'));
            },
            error: (err) => {
              this.errorMessage = `Error occurred during update (${err.status})`;
              console.error(`${this.errorMessage} - ${err.message}`);
            }
          });
      } else {
        this.employeeService.addEmployee(employeeData)
          .subscribe({
            next: () => {
              this.router.navigate([''])
                .then(r => console.log('added new employee'));
            },
            error: (err) => {
              this.errorMessage = `Error occurred during creating (${err.status})`;
              console.error(`${this.errorMessage} - ${err.message}`);
            }
          });
      }
    } else {
      console.log('Form is not valid');
    }
  }
}
