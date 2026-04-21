# Pasul 4: Register, Login si Password Policy

Scopul acestui pas este sa adaugam autentificare simpla:

```http
POST /api/auth/register
POST /api/auth/login
```

Nu folosim ORM. Toate operatiile SQL sunt facute prin ADO.NET.

## Backend

`database/002_users.sql` creeaza tabela `Users`.

Parola nu se salveaza niciodata in clar. Salvam doar `PasswordHash` si `PasswordSalt`.

`Services/PasswordPolicy.cs` valideaza parola pe backend:

- minim 8 caractere;
- cel putin o litera mare;
- cel putin o litera mica;
- cel putin o cifra;
- cel putin un caracter special.

`Services/PasswordHasher.cs` foloseste PBKDF2 cu SHA256 si 100000 iteratii.

Flux register:

```text
Angular Register form
  -> POST /api/auth/register
  -> AuthController
  -> AuthService
  -> PasswordPolicy
  -> PasswordHasher
  -> UserRepository
  -> dbo.Users
```

Flux login:

```text
Angular Login form
  -> POST /api/auth/login
  -> AuthController
  -> AuthService
  -> UserRepository.GetByEmailAsync()
  -> PasswordHasher.Verify()
  -> AuthResponse
```

## Frontend

Fisiere importante:

- `services/auth.service.ts`
- `services/password-policy.service.ts`
- `features/register-user/*`
- `features/login/*`

`AuthService` tine user-ul curent intr-un `BehaviorSubject` si il salveaza in `localStorage`, ca header-ul sa poata afisa instant numele userului dupa login/register.

Tema vizuala a fost mutata spre un stil glassy inspirat de iOS:

- fundal luminos;
- header translucid sticky;
- carduri cu `backdrop-filter`;
- butoane rotunjite;
- formulare in panel glass.
