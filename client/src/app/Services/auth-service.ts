import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { ApiBaseUrl } from '../app.config';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  fetchToken(): Promise<string | null> {
    return new Promise((resolve, reject) => {
      this.http
        .get<{ token: string }>(`${ApiBaseUrl}/Auth/token`)
        .pipe(take(1))
        .subscribe(
          (response) => {
            this.setToken(response.token);
            resolve(response.token);
          },
          (error) => {
            reject(error);
          }
        );
    });
  }

  getToken() {
    return localStorage.getItem('token');
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  clearToken() {
    localStorage.removeItem('token');
  }

  async getOrFetchToken(): Promise<string | null> {
    let token = this.getToken();
    if (!token) {
      token = await this.fetchToken();
    }
    return token;
  }
}
