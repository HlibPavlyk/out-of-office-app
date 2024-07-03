import {Component, Input} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {EmployeeGetModel} from "../../employee/employee-get.model";
import {EmployeeService} from "../../services/employee.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ApprovalRequestGetModel} from "../approval-request-get.model";
import {ApprovalRequestService} from "../../services/approval-request.service";
import {AuthService} from "../../services/auth.service";

@Component({
  selector: 'app-approval-request-view',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './approval-request-view.component.html',
  styleUrl: './approval-request-view.component.css'
})
export class ApprovalRequestViewComponent {
  approvalRequestForm: FormGroup;
  @Input({required: true}) approvalRequest!: ApprovalRequestGetModel
  errorMessage = '';
  approveRequestId: number;

  constructor(
    private fb: FormBuilder,
    private approvalRequestService: ApprovalRequestService,
    private router: Router,
    private route: ActivatedRoute,
    protected authService: AuthService
  ) {
    this.approveRequestId = 1;
    this.approvalRequestForm = this.fb.group({
      comment: [null, Validators.nullValidator]
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.approveRequestId = Number(params.get('id'));
      if (this.approveRequestId) {
        this.approvalRequestService.getApprovalRequest(String(this.approveRequestId)).subscribe({
          next: (approvalRequest) => {
            this.approvalRequest = approvalRequest;
            if (approvalRequest.comment != null){
              this.approvalRequestForm.patchValue({
                comment: approvalRequest.comment
              });
            }
          },
          error: (err) => {
            this.errorMessage = `Error occurred (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }
        })
      }
    });
  }

  onApprove() {
    if (this.approvalRequestForm.valid) {
      const requestData = this.approvalRequestForm.value;
      this.approvalRequestService.approveApprovalRequest(this.approveRequestId, requestData)
        .subscribe({
          next: () => {
            this.router.navigate(['/approval-requests'])
              .then(r => console.log('approve request'));
          },
          error: (err) => {
            this.errorMessage = `Error occurred during approving (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }
        });
    } else {
      console.log('Form is not valid');
    }
  }

  onCancel() {
    this.router.navigate(['/approval-requests'])
      .then(r => console.log('returned to approval requests list'));
  }

  onReject() {
    const requestData = this.approvalRequestForm.value;
    this.approvalRequestService.rejectApprovalRequest(this.approveRequestId, requestData).subscribe({
      next: () => {
        this.router.navigate(['/approval-requests'])
          .then(r => console.log('returned to approval requests list'));
      },
      error: (err) => {
        this.errorMessage = `Error occurred during rejecting (${err.status})`;
        console.error(`${this.errorMessage} - ${err.message}`);
      }
    });
  }
}
