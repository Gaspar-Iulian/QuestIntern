# Pasul 5: Checkout cu total calculat pe backend

Scopul acestui pas este sa implementam regula cea mai importanta din cerinta:

```text
Frontend-ul nu trimite totalul comenzii.
Backend-ul calculeaza totalul din tabela Products.
```

## Endpoint

```http
POST /api/checkout
```

Payload-ul contine adresa si itemii din cos:

```json
{
  "userId": 1,
  "shippingFullName": "Demo User",
  "shippingAddressLine1": "Strada Test 10",
  "shippingCity": "Bucharest",
  "shippingPostalCode": "010101",
  "shippingCountry": "Romania",
  "items": [
    { "productId": 1, "quantity": 2 },
    { "productId": 2, "quantity": 1 }
  ]
}
```

Observatie: nu exista camp `totalAmount` in request.

## Backend

Fisiere importante:

- `Controllers/CheckoutController.cs`
- `Services/CheckoutService.cs`
- `Repositories/OrderRepository.cs`
- `Requests/CheckoutRequest.cs`
- `Responses/CheckoutResponse.cs`
- `database/003_orders.sql`

Flux:

```text
Angular Checkout form
  -> POST /api/checkout
  -> CheckoutController
  -> CheckoutService
  -> ProductRepository.GetByIdsAsync()
  -> calculeaza UnitPrice si LineTotal din Products
  -> OrderRepository.CreateAsync()
  -> dbo.Orders + dbo.OrderItems
```

## Calcul total

Pentru fiecare item:

```text
LineTotal = Products.Price * Quantity
```

Apoi:

```text
TotalAmount = suma tuturor LineTotal
```

Exemplu testat:

```text
2 * 29.99 + 1 * 19.99 = 79.97
```

Backend-ul a returnat:

```json
{
  "orderId": 1,
  "totalAmount": 79.97
}
```

## De ce salvam ProductName si UnitPrice in OrderItems

Comanda trebuie sa ramana istoric corect. Daca produsul isi schimba numele sau pretul peste o luna, order-ul vechi trebuie sa arate ce a cumparat clientul atunci, nu datele noi din catalog.

## Frontend

Fisiere importante:

- `features/checkout/*`
- `services/checkout.service.ts`
- `models/checkout/*`

Checkout-ul foloseste itemii curenti din `CartService`, construieste requestul cu:

```text
productId + quantity
```

si dupa comanda reusita goleste cosul cu `CartService.clear()`.
