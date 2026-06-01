# Architecture — Siena Voleibol

Arquitetura do hub digital interno da **A.E. Siena**. Referência de engenharia: monorepo **Portfolio** (Clean Architecture pragmática). Referência de produto: export Google Stitch (`stitch_siena_voleibol_digital_hub.zip`).

> **Escala:** ~40 usuários internos, tráfego leve. **Simplicidade** é requisito, não opcional.

---

## Product context

| Item | Valor |
|------|-------|
| App | Siena Voleibol — gestão e desempenho do time |
| Users | Atletas, comissão, admin (~40) |
| Mobile | React Native + TypeScript ([ADR-0001](docs/architecture/adrs/ADR-0001-mobile-stack.md)) |
| Backend | .NET LTS, ASP.NET Core — **desenvolvido primeiro** |
| Admin | Painel web simples + admin mobile (mesma API) |
| Design | [DESIGN.md](DESIGN.md) |

### Features (from Stitch)

- Login por telefone ([ADR-0002](docs/architecture/adrs/ADR-0002-autenticacao-telefone.md) pendente)
- Tabs: Financeiro (*a definir*), Calendário, Destaques (*a definir*), Vídeos
- Presença no treino (Eu vou / Não vou)
- Admin (mobile + web)

Ver [PRODUCT.md](PRODUCT.md) e [DOMAIN.md](DOMAIN.md).

### Explicitly out of scope

Padrões de **outro contexto** (ex. rascunho enterprise) que **não** se aplicam:

- Migração MongoDB → SQL Server em alto volume
- CQRS, Saga, event-driven, Outbox, microserviços
- Kubernetes, mutation/load testing enterprise, Blazor

---

## Target monorepo

```txt
/
  apps/
    api/              # 1º — ASP.NET Core
    mobile/           # 2º — React Native
    admin-web/        # futuro — painel admin
  docs/
    architecture/adrs/
  .cursor/rules/
  stitch_siena_voleibol_digital_hub.zip   # referência visual
  AGENTS.md
  ARCHITECTURE.md
  DESIGN.md
  DOMAIN.md
  PRODUCT.md
  docker-compose.yml
```

---

## System diagram

```mermaid
flowchart LR
  Mobile[App React Native]
  AdminWeb[Painel Admin Web]
  API[ASP.NET Core API]
  AppLayer[Application]
  Domain[Domain]
  Infra[Infrastructure]
  Data[(JSON seed / DB simples)]

  Mobile --> API
  AdminWeb --> API
  API --> AppLayer
  AppLayer --> Domain
  Infra --> AppLayer
  Infra --> Data
```

---

## Backend (Clean Architecture — pragmatic)

```txt
Siena.Api
  Endpoints, OpenAPI/Scalar, CORS, composition

Siena.Application
  Use cases, DTOs, service interfaces

Siena.Domain
  Domain models, enums — no infrastructure deps

Siena.Infrastructure
  Repositories, JSON seeds, external services (SMS later)
```

Dependency direction:

```txt
Api -> Application
Api -> Infrastructure
Infrastructure -> Application
Infrastructure -> Domain
Application -> Domain
Domain -> (none outward)
```

### Endpoint groups (planned — spec before code)

| Group | Purpose |
|-------|---------|
| Health | Liveness |
| Auth | After ADR-0002 |
| Events | Calendar |
| Attendance | Training presence |
| Videos | Official channel list |
| Admin | Content management (later) |

---

## Mobile

- Feature folders: `auth`, `calendar`, `attendance`, `videos`, etc.
- Theme from [DESIGN.md](DESIGN.md) (`#E30613`, Inter)
- API client typed to backend DTOs
- Bottom tabs matching Stitch: Financeiro, Calendário, Destaques, Vídeos

---

## Data strategy

1. **v0:** JSON files in `Infrastructure/Data/` (Portfolio pattern)
2. **v1:** Simple relational DB when justified — light ADR, no over-modeling
3. **PII:** phone, attendance — only after LGPD review ([SECURITY.md](SECURITY.md))

---

## Infrastructure (dev)

- `docker-compose.yml`: API service only
- Mobile runs on host emulator/device
- `.env.example` for API URLs and CORS

---

## Architectural guardrails

- No microservices
- No `packages/shared` until real duplication
- DTOs at API boundary
- ADR before: database, auth provider, new messaging layer
- Preserve API contracts unless breaking change approved

---

## Reuse from Portfolio

| Portfolio | Siena |
|-----------|-------|
| 4-layer .NET structure | Reuse |
| Endpoint groups + xUnit | Reuse |
| JSON repository pattern | Reuse |
| Next.js `apps/web` | Replace with React Native |
| Stitch portfolio export | Replace with Siena Stitch + DESIGN.md |

---

## Pending ADRs

| ADR | Topic |
|-----|-------|
| [0001](docs/architecture/adrs/ADR-0001-mobile-stack.md) | React Native — Accepted |
| [0002](docs/architecture/adrs/ADR-0002-autenticacao-telefone.md) | Phone auth / OTP — Proposed |
| TBD | Database when needed |
| TBD | Admin web stack |
| TBD | Financeiro / Destaques when specced |

---

## References

- Portfolio: `C:\Users\lucas\Documents\Projects\Portfolio\ARCHITECTURE.md`
- Portfolio implementation: `apps/api/src/`
