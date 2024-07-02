import { Component } from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {EmployeeGetModel} from "../employee/employee-get.model";
import {EmployeeService} from "../services/employee.service";
import {Router, RouterLink} from "@angular/router";
import {AuthService} from "../services/auth.service";
import {ProjectService} from "../services/project.service";
import {ProjectGetModel} from "./project-get.model";
import {compare} from "../services/share.functions";

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule,
    FormsModule,
    RouterLink,
    NgClass
  ],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.css'
})
export class ProjectsComponent {
  projects: ProjectGetModel[] = [];
  filteredProjects: ProjectGetModel[] = [];
  search = '';
  sortColumn = '';
  sortOrder = 'asc';
  page = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(private projectService: ProjectService, private router:Router, protected authService: AuthService ) { }

  ngOnInit(): void {
    this.loadProjects(this.page);
  }

  loadProjects(page:number): void {
    this.projectService.getProjects(page, this.pageSize)
      .subscribe(pagedResponse => {
        this.totalPages = pagedResponse.totalPages;
        this.projects = pagedResponse.items
        this.applyFilters();
      });
  }

  applyFilters(): void {
    this.filteredProjects = this.projects;

    if (this.search) {
      this.filteredProjects = this.filteredProjects.filter(e => e.id.toString()
        .includes(this.search.toLowerCase()));
    }

    if (this.sortColumn) {
      this.filteredProjects = this.filteredProjects.sort((a, b) => {
        const isAsc = this.sortOrder === 'asc';
        switch (this.sortColumn) {
          case 'id': return compare(a.id, b.id, isAsc);
          case 'projectType': return compare(a.projectType.toLowerCase(), b.projectType.toLowerCase(), isAsc);
          case 'projectManager': return compare(a.projectManager.fullName.toLowerCase(),
            b.projectManager.fullName.toLowerCase(), isAsc);
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
      this.loadProjects(newPage);
    }
  }

  onEditProject(id: number): void {
    this.router.navigate(['/edit-project/', id])
      .then(r => console.log('navigated to edit-project'));
  }

  onProjectDetails(id: number) {
    this.router.navigate(['/view-project/', id])
      .then(r => console.log('navigated to view-project'));
  }

  onDeactivateProject(id: number) {
    this.projectService.deactivateProject(id).subscribe(() =>
      this.loadProjects(this.page));
  }

}
