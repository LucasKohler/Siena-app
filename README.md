# Siena Voleibol

Hub digital interno da **A.E. Siena** — gestão e desempenho do time (~40 usuários).

## Stack (atual)

| Camada | Tecnologia | Status |
|--------|------------|--------|
| API | .NET 10, ASP.NET Core | Fundação + events/videos (leitura) |
| Mobile | React Native + TypeScript | Planejado ([ADR-0001](docs/architecture/adrs/ADR-0001-mobile-stack.md)) |
| Admin web | A definir | Planejado |

## Documentação

- [AGENTS.md](AGENTS.md) — regras para agentes e desenvolvedores
- [ARCHITECTURE.md](ARCHITECTURE.md)
- [PRODUCT.md](PRODUCT.md) / [DOMAIN.md](DOMAIN.md)
- [DESIGN.md](DESIGN.md)

## Pré-requisitos

- [.NET SDK 10.0.201](global.json) ou compatível
- Node.js LTS (para mobile, fase futura)
- Docker (opcional — não validado neste ambiente)

## Executar a API localmente

```bash
dotnet run --project apps/api/src/Siena.Api/Siena.Api.csproj --urls http://localhost:5000
```

URLs:

```txt
API:     http://localhost:5000
Health:  http://localhost:5000/api/health
Events:  http://localhost:5000/api/events
Videos:  http://localhost:5000/api/videos
Scalar:  http://localhost:5000/scalar
OpenAPI: http://localhost:5000/openapi/v1.json
```

## Build e testes

```bash
dotnet build apps/api/Siena.slnx
dotnet test  apps/api/Siena.slnx
```

Última validação: **build e testes passaram** (7 testes: health, OpenAPI, DI, events, videos).

## Docker

Arquivos em `docker-compose.yml` e `apps/api/Dockerfile`. **Docker não estava instalado** no ambiente de desenvolvimento usado para esta fase — validar localmente:

```bash
docker compose config
docker compose up --build
```

## Estrutura

```txt
apps/api/          # Backend .NET (Siena.slnx)
docs/              # ADRs
.cursor/rules/     # Regras Cursor
```

## Próxima fatia

Presença no treino e auth por telefone (ADR-0002); mobile React Native — ver [MIGRATION-PLAN.md](MIGRATION-PLAN.md).
