import { Component } from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ProjectService} from "../../services/project.service";
import {ActivatedRoute, Router} from "@angular/router";
import {LeaveRequestService} from "../../services/leave-request.service";

@Component({
  selector: 'app-leave-request-form',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './leave-request-form.component.html',
  styleUrl: './leave-request-form.component.css'
})

export class LeaveRequestFormComponent {
  leaveRequestForm: FormGroup;
  absenceReasons: string[] = ['Sickness', 'Vacation', 'Personal','Bereavement'];
  errorMessage = '';
  isEditMode = false;
  leaveRequestId = 1;

  constructor(
    private fb: FormBuilder,
    private leaveRequestService: LeaveRequestService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.leaveRequestForm = this.fb.group({
      absenceReason: ['', Validators.required],
      startDate: [Date.now(), Validators.required],
      endDate: [Date.now(), Validators.required],
      comment: [null, Validators.nullValidator]
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.leaveRequestId = Number(params.get('id'));
      if (this.leaveRequestId) {
        this.isEditMode = true;
        this.leaveRequestService.getLeaveRequest(String(this.leaveRequestId)).subscribe({
          next: (leaveRequest) => this.leaveRequestForm.patchValue({
            absenceReason: leaveRequest.absenceReason,
            startDate: leaveRequest.startDate,
            endDate: leaveRequest.endDate,
            comment: leaveRequest.comment,
          }),
          error: (err) => {
            this.errorMessage = `Error occurred (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }})
      }
    });
  }

  onCancel() {
    this.router.navigate(['/leave-requests'])
      .then(r => console.log('returned to leave requests list'));
  }

  onSubmit() {
    if (this.leaveRequestForm.valid) {
      const leaveRequestData = this.leaveRequestForm.value;
      if (this.isEditMode) {
        this.leaveRequestService.editLeaveRequest(this.leaveRequestId, leaveRequestData)
          .subscribe({
            next: () => {
              this.router.navigate(['/leave-requests'])
                .then(r => console.log('edited leave requests'));
            },
            error: (err) => {
              this.errorMessage = `Error occurred during update (${err.status})`;
              console.error(`${this.errorMessage} - ${err.message}`);
            }
          });
      } else {
        this.leaveRequestService.addLeaveRequest(leaveRequestData)
          .subscribe({
            next: () => {
              this.router.navigate(['/leave-requests'])
                .then(r => console.log('leave new request'));
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
