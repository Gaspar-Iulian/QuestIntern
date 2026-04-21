# Pasul 1: Backend Product Catalog

Scopul acestui pas este sa avem primul endpoint real:

```http
GET /api/products
```

Acest endpoint returneaza produsele din baza de date. Este cel mai bun prim pas deoarece frontend-ul va avea nevoie de produse inainte de cos si checkout.

## Ce fisiere conteaza

`Program.cs`

- porneste aplicatia ASP.NET Core;
- activeaza controllerele;
- activeaza Swagger in development;
- configureaza CORS pentru Angular pe `http://localhost:4200`;
- inregistreaza dependentele in DI:
  - `SqlConnectionFactory`
  - `IProductRepository`

`Controllers/ProductsController.cs`

- defineste ruta `api/products`;
- primeste requestul HTTP;
- nu stie SQL;
- cere produsele de la repository.

`Repositories/IProductRepository.cs`

- este contractul folosit de controller;
- face codul testabil si usor de inlocuit.

`Repositories/ProductRepository.cs`

- contine SQL-ul pentru produse;
- foloseste ADO.NET prin `Microsoft.Data.SqlClient`;
- respecta cerinta: fara ORM.

`Repositories/SqlConnectionFactory.cs`

- construieste conexiunea SQL din `appsettings.json`;
- separa configurarea conexiunii de codul care executa query-uri.

`Models/Product.cs`

- reprezinta forma unui produs in API.

`database/001_schema_seed.sql`

- creeaza baza de date;
- creeaza tabela `Products`;
- adauga produse initiale.

## Fluxul requestului

```text
Browser / Angular
  -> GET /api/products
  -> ProductsController.GetAll()
  -> IProductRepository.GetAllAsync()
  -> ProductRepository executa SELECT in SQL Server
  -> lista Product
  -> HTTP 200 OK cu JSON
```

## De ce folosim repository aici

Controller-ul trebuie sa se ocupe de HTTP, nu de SQL. Daca punem SQL direct in controller, codul devine greu de testat si de extins. Repository-ul izoleaza accesul la baza de date.

Mai tarziu, pentru checkout, vom adauga un service separat. Acolo vom pune regula critica din cerinta: backend-ul recalculeaza totalul folosind preturile reale din baza de date.
