import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {EmployeeGetModel} from "../dto/employee-get.model";
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7082/api/employees';

  constructor(private http: HttpClient) { }

  getEmployees(page: number): Observable<EmployeeGetModel[]> {
    return this.http.get<EmployeeGetModel[]>(`${this.apiUrl}?page=${page}`);
  }

  getAmountOfEmployeePages(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/pages`);
  }

  addEmployee(employee: EmployeeGetModel): Observable<void> {
    return this.http.post<void>(this.apiUrl, employee);
  }

  updateEmployee(id: number, employee: EmployeeGetModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, employee);
  }

  deactivateEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
