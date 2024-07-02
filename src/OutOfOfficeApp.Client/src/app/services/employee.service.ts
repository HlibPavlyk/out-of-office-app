import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {EmployeeGetModel} from "../employee/employee-get.model";
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {EmployeePostModel} from "../employee/add-employee/employee-post.model";
import {PagedResponse} from "./paged-response.model";
import {CookieService} from "ngx-cookie-service";
import {EmployeeAssignModel} from "../employee/view-employee/employee-assign.model";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7082/api/employees';

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  getEmployees(page: number, pageSize: number): Observable<PagedResponse<EmployeeGetModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('addAuth', 'true');

    return this.http.get<PagedResponse<EmployeeGetModel>>(this.apiUrl, { params });
  }

  getEmployee(id: string): Observable<EmployeeGetModel> {
    return this.http.get<EmployeeGetModel>(`${this.apiUrl}/${id}?addAuth=true`);
  }


  addEmployee(employee: EmployeePostModel): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}?addAuth=true`, employee);
  }

  editEmployee(id: number, employee: EmployeePostModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}?addAuth=true`, employee);
  }

  deactivateEmployee(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}?addAuth=true/deactivate`, '');
  }

  assignEmployeeToProject(employeeId: number, projectId: EmployeeAssignModel): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${employeeId}/assign?addAuth=true`, projectId);
  }

  unassignEmployee(employeeId: number): Observable<void>  {
    return this.http.post<void>(`${this.apiUrl}/${employeeId}/unassign?addAuth=true`,'');
  }
}
