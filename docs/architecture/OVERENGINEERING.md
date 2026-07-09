# Evitar overengineering — Siena Voleibol

Guia de **proporcionalidade** para um hub interno (~**40 usuários**, tráfego leve). A arquitetura atual (monólito em 4 camadas, PostgreSQL, Docker Compose) está **adequada** — o risco principal é **complexidade desnecessária** em código, abstrações e documentação.

> Leia também: [ARCHITECTURE.md](ARCHITECTURE.md), [AGENTS.md](../../AGENTS.md).

---

## Princípios

1. **Simplicidade é requisito** — não opcional por causa da escala.
2. **Menor mudança segura** — resolver o problema concreto, não preparar para escala imaginária.
3. **Camadas com propósito** — Api, Application, Domain e Infrastructure separam responsabilidades; não são burocracia.
4. **Duplicação real > abstração prematura** — unificar só quando a repetição já incomoda manutenção.
5. **Documentar o que existe** — evitar specs aspiracionais que viram dívida de leitura.

---

## Manter (já proporcional)

| Item | Por quê |
|------|---------|
| Monólito em 4 camadas | Separação clara sem microserviços |
| Repositórios por agregado (`IEventRepository`, etc.) | Testabilidade e troca de persistência sem `IRepository<T>` genérico |
| Serviços de aplicação explícitos | Use cases legíveis sem MediatR/CQRS |
| Testes de integração de endpoint | Contratos HTTP reais (`WebApplicationFactory`) |
| Docker Compose (API + PostgreSQL) | Dev reproduzível para ~40 usuários |
| ADRs | Decisões relevantes (auth, banco, stack) registradas uma vez |
| EF Core + migrations | Persistência simples e versionada |

---

## Evitar (sem ADR + necessidade comprovada)

| Padrão / lib | Motivo |
|--------------|--------|
| Microserviços, CQRS, Saga, Outbox, event-bus | Escala e operação incompatíveis com o projeto |
| MediatR / handlers por comando | Indireção sem ganho em CRUD interno |
| `IRepository<T>` genérico | Esconde queries de domínio; preferir interfaces específicas |
| Redis, filas, cache distribuído | Tráfego leve; memória/PostgreSQL bastam |
| Kubernetes, service mesh | Docker Compose cobre dev e deploy inicial |
| `packages/shared` cross-app | Só quando houver duplicação real entre mobile e API |
| Colapsar camadas (“tudo no Api”) | Perde testabilidade por economia falsa |
| Docs longos duplicando código | Manter conciso; linkar ADRs e DOMAIN |

---

## Sinais de alerta (red flags)

- PR introduz **nova camada ou biblioteca** sem problema concreto descrito.
- Abstração para “usar no futuro” (genéricos, factories, plugins).
- Três ou mais arquivos mudam para renomear/mover sem mudança de comportamento.
- Mapeamento PT-BR duplicado em vários serviços (preferir `DomainLabels` centralizado).
- Referências a JSON seed como persistência primária (legado — PostgreSQL é padrão).
- Endpoint, tabela ou regra de negócio **não documentados** em DOMAIN/PRODUCT.
- Testes que só mockam mocks, sem assert de contrato ou fluxo.

---

## Dívida conhecida (simplificar ao tocar)

| Área | Ação preferida |
|------|----------------|
| Labels PT duplicados | **Feito:** `Siena.Domain/DomainLabels.cs` |
| Arquivos `Infrastructure/Data/*.json` | **Removidos;** seed via `DatabaseSeeder` |
| `IEventQuery` + `IEventCommand` | **Feito:** `IEventService` único |
| `IVideoQueryService` | **Feito:** `VideosEndpoints` → `IVideoQueryService` + `VideoMappings` |
| `ListPending` no approval service | **Feito:** `AdminEndpoints` → `IAttendanceApprovalService.ListPendingAsync` |
| Consultas de telefone em memória | Mover para SQL/EF quando alterar auth |
| Docs desatualizados | Corrigir no mesmo PR que muda comportamento |
| Tooling `.agents/` / Cursor | **Manter** — import consciente do Portfolio; trim fora de ciclo de produto |

---

## Checklist de proporcionalidade (PRs e agentes)

Antes de abrir ou aprovar um PR, responda:

- [ ] A mudança resolve um requisito **documentado** (Stitch, DOMAIN, ADR)?
- [ ] Existe alternativa **mais simples** que atende o mesmo caso?
- [ ] O diff é o **menor seguro** (sem refactor oportunista)?
- [ ] Novas dependências NuGet/npm têm **justificativa** explícita?
- [ ] Testes cobrem **comportamento** (endpoint, validação, fluxo), não só compilação?
- [ ] Docs atualizados **só se** contrato ou comando mudou?
- [ ] Mais de 2 áreas (Backend + Mobile + Docker…)? → considerar **split de PR**.

Se duas ou mais respostas forem “não” ou “não sei”, **pare** e simplifique ou peça revisão humana.

---

## Referências

- [ARCHITECTURE.md](ARCHITECTURE.md) — diagrama e camadas
- [ADR-0003](adrs/ADR-0003-persistencia-postgresql.md) — PostgreSQL
- [ADR-0002](adrs/ADR-0002-autenticacao-telefone.md) — auth por telefone (Accepted)
