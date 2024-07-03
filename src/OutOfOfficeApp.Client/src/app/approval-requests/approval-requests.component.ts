import { Component } from '@angular/core';
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {Router, RouterLink} from "@angular/router";
import {AuthService} from "../services/auth.service";
import {compare} from "../services/share.functions";
import {ApprovalRequestGetModel} from "./approval-request-get.model";
import {ApprovalRequestService} from "../services/approval-request.service";

@Component({
  selector: 'app-approval-requests',
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    FormsModule,
    RouterLink,
    NgClass
  ],
  templateUrl: './approval-requests.component.html',
  styleUrl: './approval-requests.component.css'
})
export class ApprovalRequestsComponent {
  approvalRequests: ApprovalRequestGetModel[] = [];
  filteredApprovalRequests: ApprovalRequestGetModel[] = [];
  search = '';
  sortColumn = '';
  sortOrder = 'asc';
  page = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(private approvalRequestService: ApprovalRequestService, private router:Router, protected authService: AuthService ) { }

  ngOnInit(): void {
    this.loadApprovalRequests(this.page);
  }

  loadApprovalRequests(page:number): void {
    this.approvalRequestService.getApprovalRequests(page, this.pageSize)
      .subscribe(pagedResponse => {
        this.totalPages = pagedResponse.totalPages;
        this.approvalRequests = pagedResponse.items
        this.applyFilters();
      });
  }

  applyFilters(): void {
    this.filteredApprovalRequests = this.approvalRequests;

    if (this.search) {
      this.filteredApprovalRequests = this.filteredApprovalRequests.filter(e => e.id.toString()
        .includes(this.search.toLowerCase()));
    }

    if (this.sortColumn) {
      this.filteredApprovalRequests = this.filteredApprovalRequests.sort((a, b) => {
        const isAsc = this.sortOrder === 'asc';
        switch (this.sortColumn) {
          case 'id': return compare(a.id, b.id, isAsc);
          case 'employee': return compare(a.approver.fullName.toLowerCase(),
            b.approver.fullName.toLowerCase(), isAsc);
          case 'leaveRequestId': return compare(a.leaveRequestId, b.leaveRequestId, isAsc);
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
      this.loadApprovalRequests(newPage);
    }
  }

  onApprovalRequestDetails(id: number) {
    this.router.navigate(['/view-approval-request/', id])
      .then(r => console.log('navigated to view-approval-request'));
  }

}
