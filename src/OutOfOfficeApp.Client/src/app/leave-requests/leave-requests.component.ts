import { Component } from '@angular/core';
import {Router, RouterLink} from "@angular/router";
import {AuthService} from "../services/auth.service";
import {compare} from "../services/share.functions";
import {LeaveRequestGetModel} from "./leave-request-get.model";
import {LeaveRequestService} from "../services/leave-request.service";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

class LeaveRequestsGetModel {
}

@Component({
  selector: 'app-leave-requests',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    FormsModule,
    RouterLink,
    NgClass,
    DatePipe
  ],
  templateUrl: './leave-requests.component.html',
  styleUrl: './leave-requests.component.css'
})
export class LeaveRequestsComponent {
  leaveRequests: LeaveRequestGetModel[] = [];
  filteredLeaveRequests: LeaveRequestGetModel[] = [];
  search = '';
  sortColumn = '';
  sortOrder = 'asc';
  page = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(private leaveRequestService: LeaveRequestService, private router:Router, protected authService: AuthService ) { }

  ngOnInit(): void {
    this.loadLeaveRequests(this.page);
  }

  loadLeaveRequests(page:number): void {
    this.leaveRequestService.getLeaveRequests(page, this.pageSize)
      .subscribe(pagedResponse => {
        this.totalPages = pagedResponse.totalPages;
        this.leaveRequests = pagedResponse.items
        this.applyFilters();
      });
  }

  applyFilters(): void {
    this.filteredLeaveRequests = this.leaveRequests;

    if (this.search) {
      this.filteredLeaveRequests = this.filteredLeaveRequests.filter(e => e.id.toString()
        .includes(this.search.toLowerCase()));
    }

    if (this.sortColumn) {
      this.filteredLeaveRequests = this.filteredLeaveRequests.sort((a, b) => {
        const isAsc = this.sortOrder === 'asc';
        switch (this.sortColumn) {
          case 'id': return compare(a.id, b.id, isAsc);
          case 'employee': return compare(a.employee.fullName.toLowerCase(),
            b.employee.fullName.toLowerCase(), isAsc);
          case 'startDate': return compare(a.startDate.toLowerCase(), b.startDate.toLowerCase(), isAsc);
          case 'endDate': return compare(a.endDate.toLowerCase(), b.endDate.toLowerCase(), isAsc);
          case 'status': return compare(a.status.toLowerCase(), b.status.toLowerCase(), isAsc);
          default: return 0;
        }
      });
    }
  }

  onSearchChange(search: string): void {
    this.search = search;
    this.applyFilters();
  }

  onSortChange(column: string): void {
    this.sortColumn = column;
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    this.applyFilters();
  }

  onPageChange(newPage: number) {
    if (newPage > 0 && newPage <= this.totalPages) {
      this.page = newPage;
      this.loadLeaveRequests(newPage);
    }
  }

  onEditLeaveRequest(id: number): void {
    this.router.navigate(['/edit-leave-request/', id])
      .then(r => console.log('navigated to edit-leave-request'));
  }

  onLeaveRequestDetails(id: number) {
    this.router.navigate(['/view-leave-request/', id])
      .then(r => console.log('navigated to view-leave-request'));
  }

  onSubmitLeaveRequest(id: number) {
    this.leaveRequestService.submitLeaveRequest(id).subscribe(() =>
      this.loadLeaveRequests(this.page));
  }

  onCancelLeaveRequest(id: number) {
    this.leaveRequestService.cancelLeaveRequest(id).subscribe(() =>
      this.loadLeaveRequests(this.page));
  }

}
