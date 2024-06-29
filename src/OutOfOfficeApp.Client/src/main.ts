import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import {HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient, withInterceptors} from "@angular/common/http";
import {AuthInterceptor} from "./app/interceptors/auth.interceptor";
import {provideRouter} from "@angular/router";
import {routes} from "./app/app.routes";
import {importProvidersFrom} from "@angular/core";

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    importProvidersFrom(HttpClientModule),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
}).catch(err => console.error(err));
