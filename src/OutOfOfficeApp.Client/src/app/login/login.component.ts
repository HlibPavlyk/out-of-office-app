import { Component } from '@angular/core';
import {FormsModule} from "@angular/forms";
import {LoginRequestModel} from "./login-request.model";
import {AuthService} from "../auth.service";
import {CookieService} from "ngx-cookie-service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  model: LoginRequestModel;

  constructor(private authService: AuthService, private cookieService: CookieService, private router: Router) {
    this.model = {email: '', password: ''};
  }

  onLoginFormSubmit() {
    this.authService.login(this.model)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.cookieService.set('Authorization', `Bearer  ${response.token}`,
            undefined, '/', undefined, true, 'Strict')
          this.router.navigate(['']);
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

}
