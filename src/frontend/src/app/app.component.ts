import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthService } from './services/auth.service';
import { CartService } from './services/cart.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [AsyncPipe, RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  private readonly cartService = inject(CartService);
  private readonly authService = inject(AuthService);

  cartCount$ = this.cartService.totalQuantity$;
  currentUser$ = this.authService.currentUser$;

  logout(): void {
    this.authService.logout();
  }
}
