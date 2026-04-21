# Pasul 6: Backend Unit Tests

Scopul acestui pas este sa verificam regulile importante fara sa depindem de SQL Server real.

## Proiect

```text
tests/backend/Ecommerce.Api.Tests
```

Proiectul foloseste xUnit si referentiaza proiectul API:

```text
Ecommerce.Api.Tests -> Ecommerce.Api
```

## Teste adaugate

`PasswordPolicyTests`

- parola valida nu returneaza erori;
- parola slaba returneaza erorile de policy.

`PasswordHasherTests`

- parola corecta trece verificarea;
- parola gresita pica verificarea.

`CheckoutServiceTests`

- totalul comenzii este calculat din preturile produselor din repository;
- comanda nu este creata daca stocul este insuficient;
- comanda nu este creata daca produsul nu exista.

## De ce folosim fake repositories

Acestea sunt unit tests, nu integration tests. Vrem sa testam logica din `CheckoutService`, nu conexiunea SQL.

De aceea folosim:

```text
FakeProductRepository
FakeOrderRepository
```

Fake-ul pentru order repository captureaza `OrderDraft`, astfel incat testul poate verifica exact ce ar fi fost salvat:

```text
TotalAmount
UnitPrice
LineTotal
ShippingFullName trim-uit
```

## Rulare

```bash
dotnet test tests/backend/Ecommerce.Api.Tests/Ecommerce.Api.Tests.csproj
```

Rezultat curent:

```text
Passed: 7
Failed: 0
```
