import {Component, OnInit} from '@angular/core';
import {EmployeeGetModel} from "./employee-get.model";
import {EmployeeService} from "../services/employee.service";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {Router, RouterLink} from "@angular/router";
import {AuthService} from "../services/auth.service";
import {compare} from "../services/share.functions";

@Component({
  selector: 'app-employee',
  standalone: true,
  imports: [
    NgForOf,
    FormsModule,
    RouterLink,
    NgClass,
    NgIf
  ],
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.css'
})
export class EmployeeComponent  implements OnInit {
  employees: EmployeeGetModel[] = [];
  filteredEmployees: EmployeeGetModel[] = [];
  search = '';
  sortColumn = '';
  sortOrder = 'asc';
  page = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(private employeeService: EmployeeService, private router:Router, protected authService: AuthService ) { }

  ngOnInit(): void {
    this.loadEmployees(this.page);
  }

  loadEmployees(page:number): void {
    this.employeeService.getEmployees(page, this.pageSize)
      .subscribe(pagedResponse => {
        this.totalPages = pagedResponse.totalPages;
        this.employees = pagedResponse.items
        this.applyFilters();
      });
  }

  applyFilters(): void {
    this.filteredEmployees = this.employees;

    if (this.search) {
      this.filteredEmployees = this.filteredEmployees.filter(e => e.fullName.toLowerCase()
        .includes(this.search.toLowerCase()));
    }

    if (this.sortColumn) {
      this.filteredEmployees = this.filteredEmployees.sort((a, b) => {
        const isAsc = this.sortOrder === 'asc';
        switch (this.sortColumn) {
          case 'id': return compare(a.id, b.id, isAsc);
          case 'fullName': return compare(a.fullName.toLowerCase(), b.fullName.toLowerCase(), isAsc);
          case 'subdivision': return compare(a.subdivision.toLowerCase(), b.subdivision.toLowerCase(), isAsc);
          case 'position': return compare(a.position.toLowerCase(), b.position.toLowerCase(), isAsc);
          case 'status': return compare(a.status.toLowerCase(), b.status.toLowerCase(), isAsc);
          case 'partnerName': return compare(a.peoplePartner.fullName.toLowerCase(),
            b.peoplePartner.fullName.toLowerCase(), isAsc);
          case 'balance': return compare(a.outOfOfficeBalance, b.outOfOfficeBalance, isAsc);
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
      this.loadEmployees(newPage);
      }
  }

  editEmployee(id: number): void {
    this.router.navigate(['/edit-employee/', id])
      .then(r => console.log('navigated to edit-employee'));
  }

  deactivateEmployee(id: number): void {
    this.employeeService.deactivateEmployee(id).subscribe(() =>
      this.loadEmployees(this.page));
  }

  assignEmployeeToProject(id: number): void {
    this.router.navigate(['/view-employee/', id])
      .then(r => console.log('navigated to view-employee'));
  }
}

