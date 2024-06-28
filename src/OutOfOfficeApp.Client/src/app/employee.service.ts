import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {EmployeeGetModel} from "./employee/employee-get.model";
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {EmployeePostModel} from "./employee/add-employee/employee-post.model";
import {PagedResponse} from "./paged-response.model";
import {CookieService} from "ngx-cookie-service";

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

    const headers = new HttpHeaders({
      Authorization: this.cookieService.get('Authorization')
    });

    return this.http.get<PagedResponse<EmployeeGetModel>>(this.apiUrl, { params, headers });
  }

  getEmployee(id: string): Observable<EmployeeGetModel> {
    return this.http.get<EmployeeGetModel>(`${this.apiUrl}/${id}`);
  }


  addEmployee(employee: EmployeePostModel): Observable<void> {
    return this.http.post<void>(this.apiUrl, employee);
  }

  editEmployee(id: number, employee: EmployeePostModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, employee);
  }

  deactivateEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
