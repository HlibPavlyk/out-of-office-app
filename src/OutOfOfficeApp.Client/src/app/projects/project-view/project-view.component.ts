import {Component, Input} from '@angular/core';
import {NgIf} from "@angular/common";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {EmployeeGetModel} from "../../employee/employee-get.model";
import {EmployeeService} from "../../services/employee.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ProjectGetModel} from "../project-get.model";
import {ProjectService} from "../../services/project.service";

@Component({
  selector: 'app-project-view',
  standalone: true,
  imports: [
    NgIf,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './project-view.component.html',
  styleUrl: './project-view.component.css'
})
export class ProjectViewComponent {
  @Input({required: true}) project!: ProjectGetModel
  errorMessage = '';

  constructor(private projectService: ProjectService, private router: Router, private route: ActivatedRoute ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const projectId = Number(params.get('id'));
      if (projectId) {
        this.projectService.getProject(String(projectId)).subscribe({
          next: (project) => {
            this.project = project;
            console.log('project', project);
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
    this.router.navigate(['/projects'])
      .then(r => console.log('returned to projects list'));
  }
}
