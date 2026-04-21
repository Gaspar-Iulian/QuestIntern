import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

import { ApiError } from '../../models/auth/api-error';
import { AuthService } from '../../services/auth.service';
import { PasswordPolicyService } from '../../services/password-policy.service';

@Component({
  selector: 'app-register-user',
  standalone: true,
  imports: [AsyncPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css'
})
export class RegisterUserComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly passwordPolicy = inject(PasswordPolicyService);
  private readonly router = inject(Router);
  private readonly errorsSubject = new BehaviorSubject<string[]>([]);

  readonly errors$ = this.errorsSubject.asObservable();
  readonly registerForm = this.formBuilder.nonNullable.group({
    fullName: ['', [Validators.required, Validators.maxLength(120)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    password: ['', [Validators.required]]
  });

  passwordHints = this.passwordPolicy.validate('');
  isSubmitting = false;

  updatePasswordHints(): void {
    this.passwordHints = this.passwordPolicy.validate(this.registerForm.controls.password.value);
  }

  submit(): void {
    this.errorsSubject.next([]);
    this.updatePasswordHints();

    if (this.registerForm.invalid || this.passwordHints.length > 0) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.authService.register(this.registerForm.getRawValue()).subscribe({
      next: () => this.router.navigateByUrl('/'),
      error: (error: HttpErrorResponse) => {
        const response = error.error as ApiError | undefined;
        this.errorsSubject.next(response?.errors?.length ? response.errors : ['Registration failed.']);
        this.isSubmitting = false;
      }
    });
  }
}
