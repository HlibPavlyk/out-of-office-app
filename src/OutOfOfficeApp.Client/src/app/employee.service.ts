import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {EmployeeGetModel} from "../dto/employee-get.model";
import {HttpClient, HttpParams} from '@angular/common/http';
import {EmployeePostModel} from "../dto/employee-post.model";
import {PagedResponse} from "./employee/paged-response.model";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7082/api/employees';

  constructor(private http: HttpClient) { }

  getEmployees(page: number, pageSize: number): Observable<PagedResponse<EmployeeGetModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', pageSize.toString());

    return this.http.get<PagedResponse<EmployeeGetModel>>(this.apiUrl, { params });
  }

  addEmployee(employee: EmployeePostModel): Observable<void> {
    return this.http.post<void>(this.apiUrl, employee);
  }


  updateEmployee(id: number, employee: EmployeePostModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, employee);
  }

  deactivateEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
