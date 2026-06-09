# AI assets — Siena Voleibol

Índice dos assets de agente adaptados do Portfolio. Fonte canônica: `.agents/` na raiz do repositório.

| Recurso | Caminho | Uso |
|---------|---------|-----|
| Roles | `.agents/agents/*.toml` | Definições de subagente (sandbox, instruções) |
| Prompts | `.agents/prompts/*.md` | Tarefas reutilizáveis (feature-plan, endpoint, review) |
| Skills | `.agents/skills/*/SKILL.md` | Workflows compostos (feature-delivery, pr-review, …) |
| Config | `.agents/config.toml` | Limites de threads/depth para multi-agent local |
| Governança | `AGENTS.md`, `AI-CONFIG.md` | Regras do projeto e mapeamento Opus/AUTO |
| Orquestração | `.cursor/rules/plan-orchestration.md` | Fluxo Grok Build → PlanBroker → Cursor |

Ver [AI-CONFIG.md](../AI-CONFIG.md) seção "Como usar os assets de `.agents/`".
