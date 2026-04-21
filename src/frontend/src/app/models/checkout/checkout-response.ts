export interface CheckoutResponseItem {
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface CheckoutResponse {
  orderId: number;
  totalAmount: number;
  createdAtUtc: string;
  items: CheckoutResponseItem[];
  message: string;
}
