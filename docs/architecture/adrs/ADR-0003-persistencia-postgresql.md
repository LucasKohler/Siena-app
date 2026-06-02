# ADR 0003: Persistência PostgreSQL (EF Core)

## Status

**Accepted** — implementado com EF Core + Npgsql (2026-06-01)

## Date

2026-06-01

## Context

O Siena (~40 usuários internos) iniciou com repositórios JSON em `Infrastructure/Data/` (padrão Portfolio). Com auth (allowlist), presença mutável (`attendances.json`) e deploy previsto em **multi-instância / cloud**, arquivos JSON não são adequados para produção (concorrência, backup, PII).

[ARCHITECTURE.md](../../../ARCHITECTURE.md) prevê: v0 JSON → v1 banco relacional simples com ADR.

## Decision

Adotar **PostgreSQL** como banco de produção e **EF Core** com provider **Npgsql**.

1. **DbContext:** `SienaDbContext` em Infrastructure com entidades de persistência mapeadas para o domínio existente.
2. **Repositórios:** implementações EF substituem `Json*Repository` nas interfaces de Application (contratos inalterados).
3. **Migrations:** versionadas no repositório; `Database.Migrate()` em Development/Docker no startup.
4. **Seed DEV:** `DatabaseSeeder` idempotente (dados fictícios alinhados ao antigo JSON); não usar atletas reais em produção.
5. **Testes:** SQLite (arquivo temporário) via `ConnectionStrings:Default` + `Persistence:Provider=Sqlite` no `WebApplicationFactory` — sem exigir Docker Postgres na CI local.
6. **Docker Compose:** serviço `postgres` com healthcheck; API recebe `ConnectionStrings__Default`.

JSON em `Data/` permanece como referência/legado de seed; **não** é mais a fonte de verdade em runtime.

## Options Considered

### PostgreSQL (escolhida)

**Pros:** Multi-instância, cloud-friendly, backup simples, EF Core maduro.

**Cons:** Mais um serviço no compose (aceitável).

### SQLite em produção

**Pros:** Zero servidor.

**Cons:** Inadequado para multi-instância/cloud (escolha de deploy do responsável).

### Manter JSON

**Pros:** Zero migração.

**Cons:** Escrita de presença frágil; não escala para staging/prod.

### MongoDB / SQL Server

Fora do escopo ou desproporcional para o tamanho do projeto.

## Consequences

- Nova variável `ConnectionStrings__Default` (e credenciais Postgres no compose) — ver `.env.example`.
- PII (telefone, presença) em banco: revisão LGPD humana antes de produção real.
- Pacotes EF adicionados em Infrastructure.
- Testes continuam passando com provider Sqlite em memória/arquivo temp.

## Validation Plan

- [x] `dotnet build` / `dotnet test` com Sqlite nos testes de integração
- [ ] `docker compose up` com Postgres + API (validar localmente quando Docker disponível)
- [x] Endpoints auth, events, videos, trainings inalterados no contrato HTTP

## Rollback Plan

Reverter DI para repositórios JSON e remover `AddDbContext`; manter migrations no histórico git.

## Human Approval Required

**Obtida** — alinhada à escolha de deploy multi-instância/cloud e ao plano de persistência.
