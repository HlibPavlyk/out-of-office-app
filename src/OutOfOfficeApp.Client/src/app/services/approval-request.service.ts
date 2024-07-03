import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpParams} from '@angular/common/http';
import {PagedResponse} from "./paged-response.model";
import {ApprovalRequestGetModel} from "../approval-requests/approval-request-get.model";
import {ApprovalRequestPostModel} from "../approval-requests/approval-request-view/approval-request-post.model";

@Injectable({
  providedIn: 'root'
})
export class ApprovalRequestService {
  private apiUrl = 'https://localhost:7082/api/approval-requests';

  constructor(private http: HttpClient) { }

  getApprovalRequests(page: number, pageSize: number): Observable<PagedResponse<ApprovalRequestGetModel>> {
    let params = new HttpParams();
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', pageSize.toString());
    params = params.append('addAuth', 'true');

    return this.http.get<PagedResponse<ApprovalRequestGetModel>>(this.apiUrl, { params });
  }

  getApprovalRequest(id: string): Observable<ApprovalRequestGetModel> {
    return this.http.get<ApprovalRequestGetModel>(`${this.apiUrl}/${id}?addAuth=true`);
  }

  approveApprovalRequest(id: number, comment: ApprovalRequestPostModel): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/approve?addAuth=true`, comment);
  }

  rejectApprovalRequest(id: number, comment: ApprovalRequestPostModel): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${id}/reject?addAuth=true`, comment);
  }
}
