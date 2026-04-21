import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';

import { CartService } from '../../services/cart.service';
import { Product } from '../../models/product';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [AsyncPipe, CurrencyPipe, RouterLink],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  private readonly cartService = inject(CartService);

  items$ = this.cartService.items$;
  totalPrice$ = this.cartService.totalPrice$;

  increase(product: Product): void {
    this.cartService.add(product);
  }

  decrease(productId: number): void {
    this.cartService.decrease(productId);
  }

  remove(productId: number): void {
    this.cartService.remove(productId);
  }
}
