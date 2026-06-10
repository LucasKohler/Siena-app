# Documentação — Siena Voleibol

Índice da documentação do repositório. **Entrada:** [AGENTS.md](../../AGENTS.md) (raiz) e [README.md](../../README.md).

## Estrutura `docs/`

```txt
docs/
├── architecture/          # Arquitetura, domínio, proporcionalidade
│   ├── ARCHITECTURE.md
│   ├── DOMAIN.md
│   ├── OVERENGINEERING.md
│   └── adrs/
├── product/               # Produto e design (Stitch)
│   ├── PRODUCT.md
│   └── DESIGN.md
├── process/               # Padrões de engenharia
│   ├── CODING_STANDARDS.md
│   └── TESTING.md
├── ai/                    # Workflow IA e assets de agente
│   ├── AI-CONFIG.md
│   └── README.md          # (este arquivo)
└── history/               # Logs históricos (não normativos)
    ├── MIGRATION-PLAN.md
    └── PROJECT-STRUCTURE.md
```

Raiz (governança GitHub + agentes): `AGENTS.md`, `CONTRIBUTING.md`, `SECURITY.md`, `README.md`.

## Assets de agente (fora de `docs/`)

| Recurso | Caminho | Uso |
|---------|---------|-----|
| Roles | `.agents/agents/*.toml` | Definições de subagente (sandbox, instruções) |
| Prompts | `.agents/prompts/*.md` | Tarefas reutilizáveis (feature-plan, endpoint, review) |
| Skills | `.agents/skills/*/SKILL.md` | Workflows compostos (feature-delivery, pr-review, …) |
| Orquestração Cursor | `.cursor/skills/siena-orchestration/SKILL.md` | Auto-invocável — mapeia roles `.toml` → Task/rules |
| Config | `.agents/config.toml` | Limites de threads/depth para multi-agent local |
| Orquestração planos | `.cursor/rules/plan-orchestration.md` | Fluxo Grok Build → PlanBroker → Cursor |

Ver [AI-CONFIG.md](AI-CONFIG.md) seções "Como usar os assets de `.agents/`" e "Camada Cursor-native".
