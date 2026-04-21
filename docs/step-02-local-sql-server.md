# Pasul 2: SQL Server local cu Docker

Scopul acestui pas este sa avem o baza MS SQL pornita local, compatibila cu cerinta exercitiului.

## De ce Docker

Pe Linux este mai simplu sa rulam SQL Server intr-un container decat sa il instalam direct pe sistem. Aplicatia .NET se conecteaza la container prin:

```text
localhost:1433
```

## Fisiere importante

`docker-compose.yml`

- porneste un container SQL Server 2022;
- expune portul `1433`;
- monteaza folderul `database/` in container la `/database`;
- pastreaza datele intr-un volum Docker.

`src/backend/Ecommerce.Api/appsettings.json`

- contine connection string-ul folosit de backend:

```text
Server=localhost,1433;Database=QuestEcommerce;User Id=sa;Password=Quest_dev_2026!;TrustServerCertificate=True
```

## Pornire SQL Server

Din radacina proiectului:

```bash
docker compose up -d sqlserver
```

Verificare:

```bash
docker ps
```

## Creare baza si seed produse

Dupa ce containerul este pornit, rulam scriptul SQL din interiorul containerului.

Imaginea SQL Server poate avea `sqlcmd` in una dintre aceste locatii:

```bash
docker exec quest-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Quest_dev_2026!' -C -i /database/001_schema_seed.sql
```

sau:

```bash
docker exec quest-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'Quest_dev_2026!' -i /database/001_schema_seed.sql
```

Daca niciuna nu exista, instalam separat `sqlcmd` pe host sau folosim Azure Data Studio / SSMS.

## Ce urmeaza dupa baza

Pornim backend-ul:

```bash
cd src/backend/Ecommerce.Api
dotnet run
```

Apoi testam:

```bash
curl http://localhost:5000/api/products
```

Portul exact poate diferi; `dotnet run` il afiseaza in consola.
