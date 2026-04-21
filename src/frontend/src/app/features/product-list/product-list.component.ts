import { AsyncPipe, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';

import { CartService } from '../../services/cart.service';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [AsyncPipe, CurrencyPipe],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  private readonly productService = inject(ProductService);
  private readonly cartService = inject(CartService);

  products$ = this.productService.getProducts();

  addToCart(product: Product): void {
    this.cartService.add(product);
  }

  handleImageError(event: Event): void {
    const image = event.target as HTMLImageElement;
    image.src = '/product-placeholder.svg';
  }
}
