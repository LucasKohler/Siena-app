# Contributing — Siena Voleibol

## Working Principles

- PRs pequenos e revisáveis
- Ler [AGENTS.md](AGENTS.md) antes de editar
- Ler [AI-CONFIG.md](AI-CONFIG.md) quando usar Opus/AUTO
- Documentação alinhada ao código implementado
- Simplicidade — evitar padrões enterprise sem necessidade

## Before You Start

1. Ler docs da área (`DOMAIN.md`, `ARCHITECTURE.md`, `DESIGN.md`)
2. `git status --short --branch`
3. Menor mudança segura
4. Opus para features não triviais; aprovação humana antes de write-heavy

## Branch Naming

```txt
feature/short-name
bugfix/short-name
refactor/short-name
docs/short-name
chore/short-name
```

## Commit Messages

Conventional Commits:

```txt
feat: add events list endpoint
fix: correct attendance validation
docs: update DOMAIN.md
test: add health endpoint test
```

## Validation

**Backend:**

```bash
dotnet build apps/api/Siena.slnx
dotnet test apps/api/Siena.slnx
```

**Mobile (when exists):**

```bash
cd apps/mobile && npm run lint && npm test
```

**Docker:**

```bash
docker compose config
docker compose build
```

**Docs only:**

```bash
git diff --check
git status --short
```

## Pull Request

Include:

- What / why
- Opus/AUTO usage if applicable
- Commands run and results
- Risks and unvalidated areas
- Contract or API changes
- LGPD/auth impact if any

## Human Review Triggers

Ask before:

- Auth provider / SMS OTP (ADR-0002)
- Database introduction or destructive migration
- Public API breaking changes
- Storing new categories of PII
- New heavy patterns (CQRS, queues, microservices)
- Production deploy or secrets
