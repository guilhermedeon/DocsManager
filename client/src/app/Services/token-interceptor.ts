import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from './auth-service';
import { Observable, from } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Skip token for login or refresh endpoints
    if (req.url.includes('token')) {
      console.log('Skipping token attachment for request:', req.url);
      return next.handle(req);
    }

    this.authService.clearToken(); // Clear existing token to force fetch

    return from(this.authService.getOrFetchToken()).pipe(
      switchMap(token => {
        if (token) {
          console.log('Attaching token to request:', token);
          const cloned = req.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });
          return next.handle(cloned);
        } else {
          return next.handle(req);
        }
      })
    );
  }
}
