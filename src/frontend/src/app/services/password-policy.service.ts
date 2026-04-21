import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PasswordPolicyService {
  validate(password: string): string[] {
    const errors: string[] = [];

    if (password.length < 8) {
      errors.push('At least 8 characters');
    }

    if (!/[A-Z]/.test(password)) {
      errors.push('One uppercase letter');
    }

    if (!/[a-z]/.test(password)) {
      errors.push('One lowercase letter');
    }

    if (!/[0-9]/.test(password)) {
      errors.push('One number');
    }

    if (!/[^A-Za-z0-9]/.test(password)) {
      errors.push('One special character');
    }

    return errors;
  }
}
