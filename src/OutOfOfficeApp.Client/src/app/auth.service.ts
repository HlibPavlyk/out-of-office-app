import { Injectable } from '@angular/core';
import {LoginRequestModel} from "./login/login-request.model";
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {LoginResponseModel} from "./login/login-response.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient ) { }
  private apiUrl = 'https://localhost:7082/api/auth';

  login(request: LoginRequestModel): Observable<LoginResponseModel> {
    return this.http.post<LoginResponseModel>(`${this.apiUrl}/login`, request);
  }
}
