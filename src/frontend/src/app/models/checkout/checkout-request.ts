import { CheckoutItemRequest } from './checkout-item-request';

export interface CheckoutRequest {
  userId: number | null;
  shippingFullName: string;
  shippingAddressLine1: string;
  shippingCity: string;
  shippingPostalCode: string;
  shippingCountry: string;
  items: CheckoutItemRequest[];
}
