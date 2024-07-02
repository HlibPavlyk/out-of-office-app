import {Component, Input} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ProjectGetModel} from "../../projects/project-get.model";
import {ProjectService} from "../../services/project.service";
import {ActivatedRoute, Router} from "@angular/router";
import {LeaveRequestGetModel} from "../leave-request-get.model";
import {LeaveRequestService} from "../../services/leave-request.service";

@Component({
  selector: 'app-leave-request-view',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './leave-request-view.component.html',
  styleUrl: './leave-request-view.component.css'
})
export class LeaveRequestViewComponent {
  @Input({required: true}) leaveRequest!: LeaveRequestGetModel
  errorMessage = '';

  constructor(private leaveRequestService: LeaveRequestService, private router: Router, private route: ActivatedRoute ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const leaveRequestId = Number(params.get('id'));
      if (leaveRequestId) {
        this.leaveRequestService.getLeaveRequest(String(leaveRequestId)).subscribe({
          next: (leaveRequest) => {
            this.leaveRequest = leaveRequest;
            console.log('leaveRequest: ', leaveRequest);
          },
          error: (err) => {
            this.errorMessage = `Error occurred (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }
        })
      }
    });
  }

  onCancel() {
    this.router.navigate(['/leave-requests'])
      .then(r => console.log('returned to leave requests list'));
  }
}
