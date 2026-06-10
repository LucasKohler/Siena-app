---
name: siena-orchestration
description: >
  Orquestra roles multi-agente do Siena (.agents/agents/*.toml) no Cursor.
  Use when implementing features, multi-agent workflows, backend or mobile changes,
  PR reviews, architecture review, security review, or when the user mentions
  explorer, backend-worker, frontend-worker, qa-reviewer, pr-reviewer, or
  architect roles. Maps .toml roles to Cursor Task subagents and scoped write flows.
---

# Siena orchestration — roles `.agents/` no Cursor

Fonte canônica dos papéis: `.agents/agents/*.toml`. Esta skill **não duplica** instruções — lê o `.toml` correspondente e aplica o mecanismo Cursor adequado.

## Mapeamento role → mecanismo Cursor

| Role (`.agents/agents/`) | Mecanismo Cursor | Sandbox |
|--------------------------|------------------|---------|
| `explorer.toml` | Tool `Task` com `subagent_type: explore` e `readonly: true` | Read-only real |
| `architect.toml` | Agente principal em modo read-only; siga `developer_instructions` do toml | Read-only |
| `qa-reviewer.toml` | Agente principal read-only; siga `developer_instructions` | Read-only |
| `security-reviewer.toml` | Agente principal read-only; siga `developer_instructions` | Read-only |
| `pr-reviewer.toml` | Agente principal read-only; siga `developer_instructions` | Read-only |
| `backend-worker.toml` | Agente principal com escopo **apenas** `apps/api/` | Write após aprovação humana |
| `frontend-worker.toml` | Agente principal com escopo **apenas** `apps/mobile/` | Write após aprovação humana |

**Único subagente nativo com sandbox enforced:** `Task` + `explore` + `readonly: true`. Workers não têm tipo nativo — o agente principal implementa obedecendo o `.toml`.

## Sequência obrigatória (features não triviais)

Espelha [.agents/skills/feature-delivery/SKILL.md](../../../.agents/skills/feature-delivery/SKILL.md):

1. **Entender** — ler `AGENTS.md`, docs de área e `git status`.
2. **Explore** — `Task` (`subagent_type: explore`, `readonly: true`) OU leitura direta seguindo `explorer.toml`.
3. **Revisões read-only** (quando aplicável) — `architect`, `qa-reviewer`, `security-reviewer` conforme risco; ler cada `.toml` antes de agir.
4. **Consolidar** — plano único: escopo, passos, riscos, testes, validação.
5. **Aprovação humana** — gate obrigatório antes de write-heavy.
6. **Implementar** — `backend-worker` **ou** `frontend-worker` (nunca os dois em paralelo no mesmo write-set).
7. **Validar** — comandos abaixo; reportar o que não foi executado.
8. **Entregar** — arquivos tocados, resultados, riscos, próximo passo.

Para planos publicados via PlanBroker: siga [.cursor/rules/plan-orchestration.md](../../rules/plan-orchestration.md).

## Regras de paralelismo e escopo

- **Nunca** rode `backend-worker` e `frontend-worker` em paralelo se compartilharem contrato, tipo, módulo ou doc técnica.
- Se backend e mobile dependem do mesmo contrato: **sequência** — contrato/backend → validação → mobile → revisão.
- Leitura (`explorer`, reviewers, `architect`) pode ser paralela.
- Respeite `sandbox_mode` de cada `.toml`; não edite fora do escopo declarado do worker.

## Validação Siena

| Área | Comando |
|------|---------|
| Backend | `dotnet build apps/api/Siena.slnx` e `dotnet test apps/api/Siena.slnx` |
| Mobile | `cd apps/mobile && npm run typecheck && npm test` |
| Docker | `docker compose config` (+ `docker compose build` se Dockerfiles mudaram) |
| Só docs | `git diff --check` |

Não afirme que build/test passou sem executar o comando.

## Prompts e skills compostos

- Planejamento multi-agent: `.agents/prompts/multi-agent-feature-plan.md`
- Review de PR: `.agents/prompts/multi-agent-pr-review.md` + skill `.agents/skills/pr-review/`
- Implementação pontual: `.agents/prompts/implement-feature.md`, `create-endpoint.md`, `create-frontend-component.md`

## Gates humanos (pare antes de prosseguir)

- Breaking change de API ou deep links
- Migration destrutiva ou PII
- Provedor SMS/OTP (ADR-0002)
- Secrets, deploy, auth não documentada

Ver `AGENTS.md` seção 6.

## O que esta skill **não** faz

- Não substitui PlanBroker MCP (publicação/consulta de planos).
- Não cria subagentes customizados além dos tipos nativos do Cursor (`explore`, etc.).
- Não enforça `sandbox_mode` dos `.toml` por arquivo — depende de `readonly: true` no Task ou disciplina de escopo no write.
