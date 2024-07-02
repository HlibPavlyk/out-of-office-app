import { Injectable } from '@angular/core';
import {LoginRequestModel} from "../login/login-request.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {LoginResponseModel} from "../login/login-response.model";
import {UserModel} from "../login/user.model";
import {CookieService} from "ngx-cookie-service";
import {Router} from "@angular/router";
import {authGuard} from "../guards/auth.guard";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userSubject = new BehaviorSubject<UserModel | undefined>(this.getUser());
  user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient, private cookieService: CookieService) { }
  private apiUrl = 'https://localhost:7082/api/auth';

  login(request: LoginRequestModel): Observable<LoginResponseModel> {
    return this.http.post<LoginResponseModel>(`${this.apiUrl}/login`, request);
  }

  setUser(user: UserModel): void {
    this.userSubject.next(user);
    localStorage.setItem('user-email', user.email);
    localStorage.setItem('user-roles', user.roles.join());
  }

  user(): Observable<UserModel | undefined> {
    return this.user$;
  }

  getUser(): UserModel | undefined {
    const email = localStorage.getItem('user-email');
    const roles = localStorage.getItem('user-roles');

    if (email && roles) {
      return {
        email,
        roles: roles.split(',')
      };
    }
    return undefined;
  }

  logout(): void {
    localStorage.clear();
    this.cookieService.delete('Authorization', '/');
    this.userSubject.next(undefined);
  }

  hasRoles(roles: string[]): boolean {
    const user = this.getUser();
    if (user && user.roles){
      return roles.some(role => user.roles.includes(role));
    } else {
      return false;
    }
  }
}
