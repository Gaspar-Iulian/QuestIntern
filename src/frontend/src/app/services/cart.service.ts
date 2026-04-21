import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';

import { CartItem } from '../models/cart-item';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly itemsSubject = new BehaviorSubject<CartItem[]>([]);

  readonly items$ = this.itemsSubject.asObservable();
  readonly totalQuantity$ = this.items$.pipe(
    map((items) => items.reduce((total, item) => total + item.quantity, 0))
  );
  readonly totalPrice$ = this.items$.pipe(
    map((items) => items.reduce((total, item) => total + item.product.price * item.quantity, 0))
  );

  getSnapshot(): CartItem[] {
    return this.itemsSubject.value;
  }

  add(product: Product): void {
    const items = this.itemsSubject.value;
    const existingItem = items.find((item) => item.product.id === product.id);

    if (existingItem) {
      this.itemsSubject.next(
        items.map((item) =>
          item.product.id === product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item
        )
      );
      return;
    }

    this.itemsSubject.next([...items, { product, quantity: 1 }]);
  }

  decrease(productId: number): void {
    const items = this.itemsSubject.value
      .map((item) =>
        item.product.id === productId
          ? { ...item, quantity: item.quantity - 1 }
          : item
      )
      .filter((item) => item.quantity > 0);

    this.itemsSubject.next(items);
  }

  remove(productId: number): void {
    this.itemsSubject.next(this.itemsSubject.value.filter((item) => item.product.id !== productId));
  }

  clear(): void {
    this.itemsSubject.next([]);
  }
}
