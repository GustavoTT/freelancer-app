import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5104/api/Users'

  constructor(private http: HttpClient, private router: Router) {}

  login(user: { userName: string; password: string }) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, user);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  register(user: {
    userName: string;
    email: string;
    fullName: string;
    password: string;
  }) {
    return this.http.post(`${this.apiUrl}`, user);
  }

}
