# Coding Standards — Siena Voleibol

Padrões para um app interno simples (~40 usuários). Clareza acima de sofisticação.

---

## General

- Preferir código legível a padrões "enterprise" desnecessários
- Mudanças locais ao comportamento pedido
- Nomes por significado de domínio (evento, presença, vídeo)
- Comentários raros; código autoexplicativo
- `.editorconfig` na raiz; `dotnet format` no backend

---

## Backend (.NET / C#)

### Layers

```txt
apps/api/src/
  Siena.Api/           → Endpoints, Program.cs, CORS, OpenAPI
  Siena.Application/   → Services, DTOs, interfaces
  Siena.Domain/        → Entities, enums (sem ASP.NET)
  Siena.Infrastructure/→ Repositories, JSON/DB, SMS (futuro)
apps/api/tests/
  Siena.Api.Tests/
  Siena.Application.Tests/
```

### Rules

- Domain sem referência a ASP.NET, EF ou filesystem
- Application orquestra casos de uso
- Infrastructure implementa persistência e integrações
- DTOs explícitos nos endpoints; validar requests
- `Program.cs` só composição — lógica em services/endpoints agrupados
- Nullable reference types habilitados
- Erros de domínio previsíveis: `Result<T>` ou respostas tipadas; não vazar stack trace na API

### Naming

- Classes/métodos públicos: `PascalCase`
- Parâmetros/locais: `camelCase`
- Interfaces: `I` + `PascalCase`
- Arquivos: nome do tipo principal (`EventsEndpoints.cs`)

---

## Mobile (React Native / TypeScript)

### Structure

```txt
apps/mobile/src/
  features/
    auth/
    calendar/
    attendance/
    videos/
    financial/      # quando spec existir
    highlights/     # quando spec existir
  core/
    api/
    theme/          # tokens alinhados a DESIGN.md
    navigation/
  shared/
    components/
```

### Rules

- TypeScript strict
- Tema: vermelho `#E30613`, fundo `#F8F8F8`, Inter — [DESIGN.md](DESIGN.md)
- Dados de listas vêm da API; sem catálogo hardcoded em produção
- Estados de loading, empty e error em telas de rede
- Client Components / hooks apenas onde necessário; evitar estado global prematuro

---

## Admin Web (futuro)

- Consumir mesma API que o mobile
- Stack a definir em ADR (pode ser React simples ou extensão do monorepo)
- Mesmos padrões de DTOs e validação

---

## Documentation

- XML docs em APIs públicas do backend quando útil
- Atualizar `DOMAIN.md` / `ARCHITECTURE.md` se contrato ou comportamento mudar
- ADR para decisões não óbvias

---

## Forbidden Patterns (this repo)

- CQRS com handlers separados "por padrão" sem necessidade
- Saga / Outbox / message bus
- Microserviços
- Abstrações vazias (interfaces com uma implementação sem motivo)
- Migration scripts de alto volume estilo "data platform"
