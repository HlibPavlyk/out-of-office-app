import { Component } from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {EmployeeService} from "../../services/employee.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ProjectService} from "../../services/project.service";

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './project-form.component.html',
  styleUrl: './project-form.component.css'
})
export class ProjectFormComponent {
  projectForm: FormGroup;
  statuses: string[] = ['Active', 'Inactive'];
  projectTypes: string[] = ['Finance', 'Marketing', 'IT','Administration'];
  errorMessage = '';
  isEditMode = false;
  isViewMode = false;
  projectId = 1;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.projectForm = this.fb.group({
      projectType: ['', Validators.required],
      projectManagerId: [Validators.min(1), Validators.required],
      startDate: [''],
      endDate: [''],
      comment: [null, Validators.nullValidator],
      status: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.projectId = Number(params.get('id'));
      if (this.projectId) {
        this.isEditMode = true;
        this.projectService.getProject(String(this.projectId)).subscribe({
          next: (project) => this.projectForm.patchValue({
            projectType: project.projectType,
            projectManagerId: project.projectManager.id,
            startDate: project.startDate,
            endDate: project.endDate,
            comment: project.comment,
            status: project.status
          }),
          error: (err) => {
            this.errorMessage = `Error occurred (${err.status})`;
            console.error(`${this.errorMessage} - ${err.message}`);
          }})
      }
    });
  }

  onCancel() {
    this.router.navigate(['/projects'])
      .then(r => console.log('returned to employees list'));
  }

  onSubmit() {
    if (this.projectForm.valid) {
      const projectData = this.projectForm.value;
      if (this.isEditMode) {
        this.projectService.editProject(this.projectId, projectData)
          .subscribe({
            next: () => {
              this.router.navigate(['/projects'])
                .then(r => console.log('edited project'));
            },
            error: (err) => {
              this.errorMessage = `Error occurred during update (${err.status})`;
              console.error(`${this.errorMessage} - ${err.message}`);
            }
          });
      } else {
        this.projectService.addProject(projectData)
          .subscribe({
            next: () => {
              this.router.navigate(['/projects'])
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
