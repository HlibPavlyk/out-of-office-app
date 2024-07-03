import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpParams} from '@angular/common/http';
import {PagedResponse} from "./paged-response.model";
import {LeaveRequestGetModel} from "../leave-requests/leave-request-get.model";
import {LeaveRequestPostModel} from "../leave-requests/leave-request-form/leave-request-post.model";

@Injectable({
  providedIn: 'root'
})
export class LeaveRequestService {
  private apiUrl = 'https://localhost:7082/api/leave-requests';

  constructor(private http: HttpClient) { }

  getLeaveRequests(page: number, pageSize: number): Observable<PagedResponse<LeaveRequestGetModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('addAuth', 'true');

    return this.http.get<PagedResponse<LeaveRequestGetModel>>(this.apiUrl, { params });
  }

  getLeaveRequest(id: string): Observable<LeaveRequestGetModel> {
    return this.http.get<LeaveRequestGetModel>(`${this.apiUrl}/${id}?addAuth=true`);
  }

  addLeaveRequest(leaveRequest: LeaveRequestPostModel): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}?addAuth=true`, leaveRequest);
  }

  editLeaveRequest(id: number, leaveRequest: LeaveRequestPostModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}?addAuth=true`, leaveRequest);
  }

  submitLeaveRequest(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/submit?addAuth=true`, '');
  }

  cancelLeaveRequest(id: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/cancel?addAuth=true`, '');
  }
}
