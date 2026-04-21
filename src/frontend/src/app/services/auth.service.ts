import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { AuthResponse } from '../models/auth/auth-response';
import { AuthUser } from '../models/auth/auth-user';
import { LoginRequest } from '../models/auth/login-request';
import { RegisterUserRequest } from '../models/auth/register-user-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5000/api/auth';
  private readonly storageKey = 'quest-user';
  private readonly currentUserSubject = new BehaviorSubject<AuthUser | null>(this.readStoredUser());

  readonly currentUser$ = this.currentUserSubject.asObservable();

  constructor(private readonly http: HttpClient) {}

  register(request: RegisterUserRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request).pipe(
      tap((response) => this.setCurrentUser(response.user))
    );
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, request).pipe(
      tap((response) => this.setCurrentUser(response.user))
    );
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.currentUserSubject.next(null);
  }

  private setCurrentUser(user: AuthUser): void {
    localStorage.setItem(this.storageKey, JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private readStoredUser(): AuthUser | null {
    const rawUser = localStorage.getItem(this.storageKey);

    if (!rawUser) {
      return null;
    }

    try {
      return JSON.parse(rawUser) as AuthUser;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }
}
