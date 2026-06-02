# Siena Voleibol

Hub digital interno da **A.E. Siena** — gestão e desempenho do time (~40 usuários).

## Stack (atual)

| Camada | Tecnologia | Status |
|--------|------------|--------|
| API | .NET 10, ASP.NET Core | Fundação + domínio + PostgreSQL (EF Core) |
| Mobile | React Native + TypeScript | Planejado ([ADR-0001](docs/architecture/adrs/ADR-0001-mobile-stack.md)) |
| Admin web | A definir | Planejado |

## Documentação

- [AGENTS.md](AGENTS.md) — regras para agentes e desenvolvedores
- [ARCHITECTURE.md](ARCHITECTURE.md)
- [PRODUCT.md](PRODUCT.md) / [DOMAIN.md](DOMAIN.md)
- [DESIGN.md](DESIGN.md)

## Pré-requisitos

- **Docker** com Compose (recomendado: [Docker Desktop](https://www.docker.com/products/docker-desktop/) + integração **WSL 2**, ou Docker Engine no Ubuntu WSL)
- [.NET SDK 10.0.201](global.json) — só para `dotnet build` / `dotnet test` no host (opcional para rodar a API)
- Node.js LTS (para mobile, fase futura)

## Executar o stack (Docker Compose) — recomendado

Na raiz do repositório (PowerShell com WSL, ou terminal Ubuntu):

```bash
cp .env.example .env
docker compose up --build
```

Isso sobe **PostgreSQL** + **API** na mesma rede:

- Postgres: credenciais `siena` / `siena_dev`, banco `siena` (porta no host: **5433** por padrão)
- API: `http://localhost:5000` — em **Development** aplica **migrations EF** e **seed DEV** na subida

Parar: `docker compose down` — dados persistem no volume `siena_pg_data`.

### Docker no Windows sem `docker` no PATH

Use o WSL:

```powershell
wsl -e bash -lc "cd /mnt/c/Users/lucas/Documents/Projects/Siena && docker compose up --build"
```

(Ajuste o caminho se o projeto estiver em outro diretório.)

### Produção em container (sem hot reload)

```bash
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --build
```

## URLs (API no host)

```txt
API:     http://localhost:5000
Health:  http://localhost:5000/api/health
Auth:    POST http://localhost:5000/api/auth/login  (body: `{ "phoneNumber": "+5511999990001" }`)
         GET  http://localhost:5000/api/auth/me      (header: `Authorization: Bearer <token>`)
Trainings: GET  http://localhost:5000/api/trainings/next (Bearer)
           POST http://localhost:5000/api/trainings/{eventId}/attendance (Bearer; Atleta)
Admin:   /api/admin/* (Bearer; Staff = Admin ou Comissão)
Events:  http://localhost:5000/api/events
Videos:  http://localhost:5000/api/videos
Scalar:  http://localhost:5000/scalar
OpenAPI: http://localhost:5000/openapi/v1.json
Postman: importar `postman/Siena-API.postman_collection.json` (ver [postman/README.md](postman/README.md))
```

### API no host (sem Docker) — opcional

Se precisar rodar só o `dotnet` no Windows, aponte para o Postgres do Compose (`localhost:5433`) ou use `scripts/setup-database.ps1` com Postgres nativo. O fluxo padrão do projeto é **100% Compose**.

## Build e testes

```bash
dotnet build apps/api/Siena.slnx
dotnet test  apps/api/Siena.slnx
```

Última validação: **build e testes passaram** (23 testes: health, OpenAPI, DI, events, videos, auth, trainings, admin).

### JWT (produção / Docker)

Definir via ambiente (nunca commitar chave real):

```txt
Jwt__Issuer=siena-api
Jwt__Audience=siena-mobile
Jwt__SigningKey=<mínimo 32 caracteres>
Jwt__AccessTokenMinutes=480
```

Ver [.env.example](.env.example).

## Banco de dados (PostgreSQL)

Criado e migrado **dentro do Compose**: o serviço `api` em `Development` executa `MigrateAsync` + seed na inicialização. Não é necessário `dotnet ef database update` no host.

Ver [ADR-0003](docs/architecture/adrs/ADR-0003-persistencia-postgresql.md).

## Docker

| Arquivo | Função |
|---------|--------|
| `docker-compose.yml` | Postgres + API (dev, hot reload) |
| `docker-compose.prod.yml` | Override: imagem `runtime`, `Production` |
| `apps/api/Dockerfile` | Targets `development` e `runtime` |
| `.env.example` | Variáveis para Compose |

Validar config:

```bash
docker compose config
```

## Estrutura

```txt
apps/api/          # Backend .NET (Siena.slnx)
docs/              # ADRs
.cursor/rules/     # Regras Cursor
```

## Próxima fatia

Mobile React Native (Fase 3) — ver [MIGRATION-PLAN.md](MIGRATION-PLAN.md).
