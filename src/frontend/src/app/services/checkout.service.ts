import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CheckoutRequest } from '../models/checkout/checkout-request';
import { CheckoutResponse } from '../models/checkout/checkout-response';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  private readonly apiUrl = 'http://localhost:5000/api/checkout';

  constructor(private readonly http: HttpClient) {}

  placeOrder(request: CheckoutRequest): Observable<CheckoutResponse> {
    return this.http.post<CheckoutResponse>(this.apiUrl, request);
  }
}
