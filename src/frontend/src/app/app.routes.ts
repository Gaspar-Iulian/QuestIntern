import { Routes } from '@angular/router';

import { CartComponent } from './features/cart/cart.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { LoginComponent } from './features/login/login.component';
import { ProductListComponent } from './features/product-list/product-list.component';
import { RegisterUserComponent } from './features/register-user/register-user.component';

export const routes: Routes = [
  { path: '', component: ProductListComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: CheckoutComponent },
  { path: 'register', component: RegisterUserComponent },
  { path: 'login', component: LoginComponent },
  { path: '**', redirectTo: '' }
];
