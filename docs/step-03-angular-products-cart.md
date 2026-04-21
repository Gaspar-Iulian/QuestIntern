# Pasul 3: Angular Products si Cart State

Scopul acestui pas este sa avem prima parte vizibila a aplicatiei:

- lista de produse citita din backend;
- buton `Add to Cart`;
- counter de cart in header;
- pagina de cart cu cantitati si total local.

## Fisiere importante

`app.config.ts`

- adauga `provideHttpClient()`;
- fara acesta, Angular nu poate injecta `HttpClient` in servicii.

`app.routes.ts`

- defineste rutele:
  - `/` pentru produse;
  - `/cart` pentru cos.

`services/product.service.ts`

- face request la backend:

```text
GET http://localhost:5000/api/products
```

`services/cart.service.ts`

- tine starea cosului intr-un `BehaviorSubject`;
- expune observabile:
  - `items$`
  - `totalQuantity$`
  - `totalPrice$`

`features/product-list/product-list.component.*`

- afiseaza produsele;
- trimite produsul selectat catre `CartService.add(product)`.

`features/cart/cart.component.*`

- citeste cosul din `CartService`;
- permite cresterea/scaderea cantitatii;
- calculeaza totalul local pentru afisare.

## De ce BehaviorSubject

`BehaviorSubject` pastreaza ultima valoare a cosului si o emite imediat oricarui subscriber nou. De aceea header-ul poate afisa instant `Cart (3)` dupa ce apasam `Add to Cart`, fara refresh si fara sa mutam manual date intre componente.

Flux:

```text
ProductListComponent
  -> CartService.add(product)
  -> BehaviorSubject emite lista noua
  -> Header primeste totalQuantity$
  -> CartComponent primeste items$
```

## Observatie importanta

Totalul din cart este doar pentru afisare. La checkout, backend-ul trebuie sa recalculeze totalul din baza de date folosind `Product.Id` si cantitatile primite. Asta respecta regula din cerinta: nu avem voie sa avem incredere in totalul trimis de frontend.
