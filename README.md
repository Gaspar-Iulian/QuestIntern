# Quest E-Commerce Exercise

Aplicatie full-stack pentru cerinta din documentul Word:

- Backend: ASP.NET Core Web API, C#, MS SQL, fara ORM.
- Frontend: Angular SPA.
- Functionalitati: produse, cos, checkout, inregistrare, login.
- Regula importanta: totalul comenzii se calculeaza pe backend din baza de date, nu se accepta totalul trimis de frontend.

## Arhitectura propusa

```text
src/
  backend/
    Ecommerce.Api/
      Controllers/     endpoint-uri HTTP
      Models/          entitati simple folosite in API
      Repositories/    acces la SQL cu ADO.NET, fara ORM
      Services/        logica de business
      Requests/        payload-uri primite de API
      Responses/       payload-uri returnate de API
  frontend/            aplicatia Angular
tests/
  backend/
    Ecommerce.Api.Tests/
database/
  001_schema_seed.sql  schema si produse initiale
```

Flux backend:

```text
Angular -> Controller -> Service -> Repository -> MS SQL
```

Pentru primul pas am creat endpoint-ul de catalog:

```http
GET /api/products
```

Acesta citeste produsele din tabela `Products`.

## Cerinte locale

Pentru rulare locala sunt necesare:

- .NET SDK 8 sau mai nou;
- Node.js si npm;
- Docker pentru SQL Server local.

## Rulare backend

Din radacina proiectului:

```bash
docker compose up -d sqlserver
docker exec quest-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Quest_dev_2026!' -C -i /database/001_schema_seed.sql
docker exec quest-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Quest_dev_2026!' -C -i /database/002_users.sql
docker exec quest-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Quest_dev_2026!' -C -i /database/003_orders.sql
cd src/backend/Ecommerce.Api
dotnet restore
dotnet run --urls http://localhost:5000
```

API-ul va porni pe:

```text
http://localhost:5000
```

## Restaurare baza de date

Ruleaza scripturile:

```sql
database/001_schema_seed.sql
database/002_users.sql
database/003_orders.sql
```

Primul script creeaza baza `QuestEcommerce`, tabela `Products` si cateva produse initiale. Al doilea script creeaza tabela `Users`. Al treilea script creeaza tabelele `Orders` si `OrderItems`.

## Pasii de implementare

1. Catalog produse backend: `GET /api/products`. Implementat.
2. SQL Server local cu Docker si seed produse. Implementat.
3. Frontend Angular: lista produse si buton "Add to Cart". Implementat.
4. Cart state management cu `BehaviorSubject`. Implementat.
5. Cart page cu cantitati si total local. Implementat.
6. Register/Login backend si frontend, cu password policy. Implementat.
7. Checkout backend: calcul total pe server din tabela `Products`. Implementat.
8. Checkout frontend: formular adresa si plasare comanda. Implementat.
9. Teste unitare pentru calculul totalului si password policy/hash. Implementat.

## Rulare frontend

Din radacina proiectului:

```bash
cd src/frontend
npm install
npm start -- --host 0.0.0.0 --port 4200
```

Aplicatia Angular ruleaza pe:

```text
http://localhost:4200
```

## Rulare teste backend

Din radacina proiectului:

```bash
dotnet test tests/backend/Ecommerce.Api.Tests/Ecommerce.Api.Tests.csproj
```
