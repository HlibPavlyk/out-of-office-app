import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {CookieService} from "ngx-cookie-service";
import {AuthService} from "../auth.service";
import {jwtDecode} from "jwt-decode";

export const authGuard: CanActivateFn = (route, state) => {
  const cokkieService = inject(CookieService);
  const authService = inject(AuthService);
  const router = inject(Router);

  let token = cokkieService.get('Authorization');

  if (token) {
    token = token.replace('Bearer ', '');
    const decodedToken: any = jwtDecode(token);

    const expirationDate = decodedToken.exp * 1000;
    const currentTime = new Date().getTime();
    const user = authService.getUser();

    if (expirationDate < currentTime) {
      authService.logout();
      return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })
    } else {
      if (user?.roles.includes('Administrator')) {
        return true;
      } else {
        alert('You do not have permission to access this page');
        return false;      }
    }
  } else {
    authService.logout();
    return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })
  }
};
