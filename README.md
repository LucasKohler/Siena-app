# Siena Voleibol

Hub digital interno da **A.E. Siena** — gestão e desempenho do time (~40 usuários).

## Stack

| Camada | Tecnologia | Status |
|--------|------------|--------|
| API | .NET 10, ASP.NET Core, PostgreSQL (EF Core) | Em produção interna |
| Mobile | Expo + Expo Router + TypeScript | [apps/mobile](apps/mobile/) — Fase 3 |
| Admin web | A definir | Planejado (Fase 4) |

Referências: [ADR-0001](docs/architecture/adrs/ADR-0001-mobile-stack.md) (RN), [ADR-0003](docs/architecture/adrs/ADR-0003-persistencia-postgresql.md) (Postgres), [ADR-0004](docs/architecture/adrs/ADR-0004-mobile-expo-router.md) (Expo).

## Documentação

| Documento | Conteúdo |
|-----------|----------|
| [AGENTS.md](AGENTS.md) | Regras para agentes e desenvolvedores |
| [ARCHITECTURE.md](ARCHITECTURE.md) | Camadas e decisões |
| [PRODUCT.md](PRODUCT.md) / [DOMAIN.md](DOMAIN.md) | Produto e domínio |
| [DESIGN.md](DESIGN.md) | Identidade visual (Stitch) |
| [MIGRATION-PLAN.md](MIGRATION-PLAN.md) | Fases do projeto |
| [apps/mobile/README.md](apps/mobile/README.md) | App mobile (Expo) |

## Pré-requisitos

- **Docker** + Compose (recomendado: Docker Desktop + WSL 2, ou Engine no Ubuntu WSL)
- [.NET SDK 10.0.201](global.json) — build/test da API no host (opcional para rodar a API)
- **Node.js LTS** — app mobile (`apps/mobile`)

## Início rápido

### 1. API + banco (Docker)

Na raiz do repositório:

```bash
cp .env.example .env
docker compose up --build
```

- **Postgres:** `localhost:5433` (host), usuário/senha `siena` / `siena_dev`, banco `siena`
- **API:** `http://localhost:5000` — em Development aplica migrations EF e seed DEV na subida

Parar: `docker compose down` (dados no volume `siena_pg_data`).

**Windows sem `docker` no PATH** — use WSL (ajuste o caminho do projeto):

```powershell
wsl -e bash -lc "cd /mnt/c/Users/lucas/Documents/Projects/Siena && docker compose up --build"
```

### 2. App mobile (Expo)

Com a API no ar:

```bash
cd apps/mobile
cp .env.example .env
npm install
npm start
```

Configure `EXPO_PUBLIC_API_URL` no `.env` (ver tabela em [apps/mobile/README.md](apps/mobile/README.md)).

**Login DEV (seed):**

| Telefone | Papel |
|----------|-------|
| +5511999990001 | Administrador |
| +5511999990002 | Comissão |
| +5511999990003 | Atleta |
| +5511999990004 | Atleta |

### Produção em container (API, sem hot reload)

```bash
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --build
```

## URLs da API

```txt
API:       http://localhost:5000
Health:    http://localhost:5000/api/health
Auth:      POST /api/auth/login   { "phoneNumber": "+5511999990001" }
           GET  /api/auth/me      Authorization: Bearer <token>
Events:    GET  /api/events
Videos:    GET  /api/videos
Trainings: GET  /api/trainings/next
           POST /api/trainings/{eventId}/attendance   (Atleta; "Eu vou" | "Não vou")
Admin:     /api/admin/*   (Staff: Administrador ou Comissão)
Scalar:    http://localhost:5000/scalar
OpenAPI:   http://localhost:5000/openapi/v1.json
```

Postman: [postman/Siena-API.postman_collection.json](postman/Siena-API.postman_collection.json) — ver [postman/README.md](postman/README.md).

Calendário: categorias **Masculino** e **Feminino** apenas.

## Build e testes

**API:**

```bash
dotnet build apps/api/Siena.slnx
dotnet test  apps/api/Siena.slnx
```

**Mobile:**

```bash
cd apps/mobile
npm run typecheck
npm test
npm run smoke:api    # requer API em :5000
```

## JWT (produção / Docker)

Definir via ambiente (nunca commitar chave real):

```txt
Jwt__Issuer=siena-api
Jwt__Audience=siena-mobile
Jwt__SigningKey=<mínimo 32 caracteres>
Jwt__AccessTokenMinutes=480
```

Ver [.env.example](.env.example).

## Estrutura do repositório

```txt
apps/api/              # Backend .NET (Siena.slnx)
apps/mobile/           # Expo + Expo Router
docs/architecture/adrs/
postman/
docker-compose.yml
.cursor/rules/
```

Detalhes: [PROJECT-STRUCTURE.md](PROJECT-STRUCTURE.md).

## Próximas fatias

- Financeiro e Destaques (spec de produto)
- Admin web simples (Fase 4)
- OTP/SMS em produção ([ADR-0002](docs/architecture/adrs/ADR-0002-autenticacao-telefone.md))

Ver [MIGRATION-PLAN.md](MIGRATION-PLAN.md).
