import { AsyncPipe, CurrencyPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { BehaviorSubject, firstValueFrom } from 'rxjs';

import { ApiError } from '../../models/auth/api-error';
import { AuthUser } from '../../models/auth/auth-user';
import { CheckoutResponse } from '../../models/checkout/checkout-response';
import { AuthService } from '../../services/auth.service';
import { CartService } from '../../services/cart.service';
import { CheckoutService } from '../../services/checkout.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [AsyncPipe, CurrencyPipe, DatePipe, ReactiveFormsModule, RouterLink],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly cartService = inject(CartService);
  private readonly checkoutService = inject(CheckoutService);
  private readonly authService = inject(AuthService);
  private readonly errorsSubject = new BehaviorSubject<string[]>([]);
  private readonly orderSubject = new BehaviorSubject<CheckoutResponse | null>(null);

  readonly items$ = this.cartService.items$;
  readonly totalPrice$ = this.cartService.totalPrice$;
  readonly errors$ = this.errorsSubject.asObservable();
  readonly order$ = this.orderSubject.asObservable();

  readonly checkoutForm = this.formBuilder.nonNullable.group({
    shippingFullName: ['', [Validators.required, Validators.maxLength(120)]],
    shippingAddressLine1: ['', [Validators.required, Validators.maxLength(240)]],
    shippingCity: ['', [Validators.required, Validators.maxLength(120)]],
    shippingPostalCode: ['', [Validators.required, Validators.maxLength(40)]],
    shippingCountry: ['Romania', [Validators.required, Validators.maxLength(120)]]
  });

  isSubmitting = false;

  async submit(): Promise<void> {
    this.errorsSubject.next([]);
    this.orderSubject.next(null);

    const cartItems = this.cartService.getSnapshot();

    if (cartItems.length === 0) {
      this.errorsSubject.next(['Cart must contain at least one item.']);
      return;
    }

    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    try {
      const currentUser = await firstValueFrom(this.authService.currentUser$) as AuthUser | null;
      const response = await firstValueFrom(this.checkoutService.placeOrder({
        ...this.checkoutForm.getRawValue(),
        userId: currentUser?.id ?? null,
        items: cartItems.map((item) => ({
          productId: item.product.id,
          quantity: item.quantity
        }))
      }));

      this.orderSubject.next(response);
      this.cartService.clear();
      this.checkoutForm.reset({
        shippingFullName: '',
        shippingAddressLine1: '',
        shippingCity: '',
        shippingPostalCode: '',
        shippingCountry: 'Romania'
      });
    } catch (error) {
      const response = (error as HttpErrorResponse).error as ApiError | undefined;
      this.errorsSubject.next(response?.errors?.length ? response.errors : ['Checkout failed.']);
    } finally {
      this.isSubmitting = false;
    }
  }
}
