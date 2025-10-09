import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor( private http: HttpClient ) { }

  fetchToken(){
    this.http.get<{ token: string }>('http://localhost:5000/token')
    .pipe(take(1))
    .subscribe(response => {
      this.setToken(response.token);
    });
  }

  getToken(){
    return localStorage.getItem('token');
  }

  setToken(token: string){
    localStorage.setItem('token', token);
  }

  clearToken(){
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    // Simple check for token existence; in a real app, you'd verify token validity
    return !!token;
  }
}
