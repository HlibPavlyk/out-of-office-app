import { Component } from '@angular/core';
import {FormsModule} from "@angular/forms";
import {LoginRequestModel} from "./login-request.model";
import {AuthService} from "../services/auth.service";
import {CookieService} from "ngx-cookie-service";
import {Router} from "@angular/router";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    NgIf
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  model: LoginRequestModel;
  errorMessage: string;

  constructor(private authService: AuthService, private cookieService: CookieService, private router: Router) {
    this.model = {email: '', password: ''};
    this.errorMessage = '';
  }

  onLoginFormSubmit() {
    this.authService.login(this.model)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.cookieService.set('Authorization', `Bearer  ${response.token}`,
            undefined, '/', undefined, true, 'Strict');
          this.authService.setUser({
            email: this.model.email,
            roles: response.roles
          });

          this.router.navigate(['']);
        },
        error: (err) => {
          this.errorMessage = `Incorrect login or password (${err.status})`;
          console.error(`${this.errorMessage} - ${err.message}`);
        }
      });
  }

}
