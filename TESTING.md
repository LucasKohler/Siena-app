# Testing — Siena Voleibol

Testes protegem comportamento real. Sem testes placeholder.

---

## Backend (xUnit)

Localização: `apps/api/tests/`

### Priorities

| Área | O que testar |
|------|----------------|
| Health | `GET /api/health` |
| Calendário | listagem e detalhe de evento; 404 |
| Presença | confirmação Eu vou/Não vou; validação |
| Vídeos | listagem; shape do DTO |
| Auth | fluxo OTP quando ADR-0002 implementado |
| OpenAPI | documento disponível |

### Libraries

- xUnit
- FluentAssertions (ou assertions nativas consistentes)
- `WebApplicationFactory` para testes de integração de endpoints quando aplicável

### Commands

```bash
dotnet build apps/api/Siena.slnx
dotnet test apps/api/Siena.slnx
```

---

## Mobile (React Native)

Quando `apps/mobile` existir:

- Testes de parsing do cliente API
- Testes de componente/widget para login, calendário, presença (fluxos críticos)
- `npm test` / Jest conforme template do projeto

```bash
cd apps/mobile
npm run lint
npm test
```

E2E (Detox/Maestro): só se fluxos estabilizarem — ADR opcional.

---

## Docker

```bash
docker compose config
docker compose build
```

---

## Documentation-only changes

```bash
git diff --check
git status --short
```

---

## Rules

- Não apagar testes para "passar build"
- Regression test em bugs quando prático
- Reportar comandos não executados e o motivo
- Não exigir mutation testing ou load testing neste projeto
