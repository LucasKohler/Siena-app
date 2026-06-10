# Project Structure — Siena Voleibol

> **Documento histórico.** Estrutura canônica atual: [README.md](../../README.md) e [docs/ai/README.md](../ai/README.md). Mantido como log da migração Portfolio → Siena.

## Siena — documentação atual

```txt
Siena/
├── .cursor/
│   └── rules/
│       ├── siena-core.mdc
│       ├── ai-workflow.mdc
│       ├── agent-interaction.mdc
│       ├── backend-dotnet.mdc
│       ├── mobile-app.mdc
│       └── documentation.mdc
├── docs/
│   └── architecture/
│       └── adrs/
│           ├── ADR_TEMPLATE.md
│           ├── ADR-0001-mobile-stack.md      # React Native — Accepted
│           └── ADR-0002-autenticacao-telefone.md
├── AGENTS.md
├── ARCHITECTURE.md
├── AI-CONFIG.md
├── DESIGN.md                                 # extraído do Stitch
├── DOMAIN.md
├── PRODUCT.md
├── CODING_STANDARDS.md
├── TESTING.md
├── SECURITY.md
├── CONTRIBUTING.md
├── PROJECT-STRUCTURE.md
├── MIGRATION-PLAN.md
└── stitch_siena_voleibol_digital_hub.zip     # export Google Stitch
```

---

## Siena — estrutura alvo (código, backend-first)

```txt
Siena/
├── apps/
│   ├── api/
│   │   ├── Siena.slnx
│   │   ├── Dockerfile
│   │   ├── .dockerignore
│   │   ├── src/
│   │   │   ├── Siena.Api/
│   │   │   │   ├── Endpoints/
│   │   │   │   ├── Configuration/
│   │   │   │   └── Program.cs
│   │   │   ├── Siena.Application/
│   │   │   ├── Siena.Domain/
│   │   │   └── Siena.Infrastructure/    # EF + DatabaseSeeder (DEV)
│   │   └── tests/
│   │       ├── Siena.Api.Tests/
│   │       └── Siena.Application.Tests/
│   ├── mobile/                        # Expo + Expo Router (Fase 3)
│   │   ├── app/                       # rotas (login, tabs, treino, admin)
│   │   ├── src/
│   │   │   ├── api/
│   │   │   ├── auth/
│   │   │   ├── components/
│   │   │   ├── features/
│   │   │   ├── theme/
│   │   │   └── utils/
│   │   ├── __tests__/
│   │   ├── package.json
│   │   └── README.md
│   └── admin-web/                     # futuro
├── docker-compose.yml
├── docker-compose.override.yml
├── .env.example
├── .editorconfig
├── .gitignore
└── global.json
```

---

## Stitch export — telas (referência visual)

Conteúdo de `stitch_siena_voleibol_digital_hub.zip`:

| Pasta | Conteúdo |
|-------|----------|
| `login_onboarding_pt_br/` | Login telefone |
| `calend_rio_pt_br/` | Calendário + tabs |
| `presen_a_no_treino_pt_br/` | Presença |
| `v_deos_pt_br/` | Vídeos |
| `aba_1_financeiro/` | Placeholder |
| `aba_3_destaques/` | Placeholder |
| `admin_mobile/` | Admin (screenshot) |
| `painel_admin_web/` | Placeholder |
| `siena_voleibol_identity/DESIGN.md` | → copiado para raiz `DESIGN.md` |

---

## Portfolio — referência (estrutura resumida)

Monorepo em `C:\Users\lucas\Documents\Projects\Portfolio`:

```txt
Portfolio/
├── apps/api/          # .NET 4 layers + tests
├── apps/web/          # Next.js (Siena substitui por mobile)
├── .agents/           # AI agents, prompts, skills
├── docs/architecture/adrs/
├── stitch_the_kohler_portfolio/
└── [AGENTS.md, ARCHITECTURE.md, AI_WORKFLOW.md, ...]
```

Reutilizar padrão de `apps/api/`; não portar `apps/web` nem Stitch do portfolio.

---

## Convenções herdadas

| Portfolio | Siena |
|-----------|-------|
| `apps/api/src/{Layer}/` | Igual |
| Endpoints por domínio | events, attendance, videos, auth |
| `DatabaseSeeder` | Dados DEV em PostgreSQL |
| Sem Turborepo/Nx | Igual |

---

## Config files (when code phase starts)

| File | Purpose |
|------|---------|
| `global.json` | .NET SDK pin |
| `.env.example` | API URL, CORS |
| `docker-compose.yml` | API service |
