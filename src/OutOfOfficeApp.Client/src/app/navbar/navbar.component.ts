import {Component, OnInit} from '@angular/core';
import {Router, RouterLink, RouterOutlet} from "@angular/router";
import {NgIf} from "@angular/common";
import {AuthService} from "../services/auth.service";
import {UserModel} from "../login/user.model";

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink,
    NgIf,
    RouterOutlet,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  user: UserModel | undefined;

  constructor(protected authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.authService.user().subscribe(user => {
      if (user) {
        this.user = user;
        console.log(`User is logged in as ${user.email}`);
      }
    });
    this.user = this.authService.getUser();
  }

  onLogout(): void{
    this.user = undefined;
    this.authService.logout();
    this.router.navigate(['login']);
    console.log('User is logged out');
  }

}
