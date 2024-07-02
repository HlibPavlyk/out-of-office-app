import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpParams} from '@angular/common/http';
import {PagedResponse} from "./paged-response.model";
import {CookieService} from "ngx-cookie-service";
import {ProjectGetModel} from "../projects/project-get.model";
import {ProjectPostDTO} from "../projects/project-form/project-post.model";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private apiUrl = 'https://localhost:7082/api/projects';

  constructor(private http: HttpClient) { }

  getProjects(page: number, pageSize: number): Observable<PagedResponse<ProjectGetModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('addAuth', 'true');

    return this.http.get<PagedResponse<ProjectGetModel>>(this.apiUrl, { params });
  }

  getProject(id: string): Observable<ProjectGetModel> {
    return this.http.get<ProjectGetModel>(`${this.apiUrl}/${id}?addAuth=true`);
  }


  addProject(project: ProjectPostDTO): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}?addAuth=true`, project);
  }

  editProject(id: number, project: ProjectPostDTO): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}?addAuth=true`, project);
  }
  deactivateProject(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/deactivate?addAuth=true`, '');
  }

  /*assignEmployeeToProject(employeeId: number, projectId: EmployeeAssignModel): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${employeeId}/assign?addAuth=true`, projectId);
  }

  unassignEmployee(employeeId: number): Observable<void>  {
    return this.http.post<void>(`${this.apiUrl}/${employeeId}/unassign?addAuth=true`,'');
  }*/
}
