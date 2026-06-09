# Adotar arquitetura de agentes (.agents/) do Portfolio em Siena

## Contexto / Objetivo / Escopo

**Contexto**
Siena (React Native + .NET) já adota o padrão de governança de IA do Portfolio:
- `AGENTS.md` (raiz + apps)
- `AI-CONFIG.md` (documenta o mapeamento Portfolio → Siena, fluxo Opus + Cursor AUTO)
- `.cursor/rules/plan-orchestration.md` (referencia explicitamente subagentes `explorer`, `architect`, `qa-reviewer`, `backend-worker`, `frontend-worker`, `security-reviewer` + validação Siena-specific: `Siena.slnx` + `apps/mobile` typecheck/test)
- `.cursor/permissions.json` já permite tools do plan-broker

Porém, falta a **estrutura concreta de assets de agentes** que existe em `../Portfolio/.agents/`:
- `agents/*.toml` (definições de role + developer_instructions + sandbox_mode read-only vs workspace-write)
- `prompts/*.md` (prompts reutilizáveis para feature-plan, implement, reviews, create-endpoint etc.)
- `skills/*/SKILL.md` (workflows compostos: feature-delivery, test-strategy, pr-review, security-review, architecture-review, release-preparation)
- `config.toml` + `README.md`

O `plan-orchestration.md` de Siena já assume esses agentes (tabela de subagentes), mas sem os arquivos `.agents` o time e os agentes não têm as definições concretas, prompts canônicos e workflows estruturados para uso consistente (manual, Cursor agent ou Grok subagent orchestration).

**Objetivo**
Copiar a arquitetura de agentes madura do Portfolio e adaptá-la para Siena de forma proporcional (baixo tráfego, ~40 usuários internos, Clean Architecture pragmática .NET + React Native/Expo).

**Escopo (Documentation + AI Tooling)**
- Adicionar e adaptar `.agents/` completa na raiz de Siena.
- Atualizar referências em `AGENTS.md`, `AI-CONFIG.md` e (se necessário) `.cursor/rules/plan-orchestration.md`.
- Adaptar para stack real de Siena: `apps/api/Siena.slnx`, `apps/mobile` (React Native + TS + Expo, sem Next.js/Tailwind por enquanto), comandos de validação existentes na plan-orchestration e AI-CONFIG.
- Incluir `pr-reviewer` (presente no Portfolio, útil para multi-agent).
- Não criar features de domínio (Financeiro, Calendário, Presença, Vídeos etc.), não inventar endpoints/tabelas, não alterar código de app.
- Manter fidelidade ao `AGENTS.md` de Siena (prioridade máxima) e ao `docs/OVERENGINEERING.md`.

Fora de escopo: configuração de MCP, deploy, secrets, mudanças em mobile/api, novas skills específicas de voleibol (podem vir depois).

## BDD (Gherkin) — se aplicável

```gherkin
Feature: Adoção da arquitetura de agentes multi-agente
  Como engenheiro (humano ou agente de planejamento)
  Eu quero ter .agents/ adaptado do Portfolio
  Para que possa usar roles consistentes (explorer, workers, reviewers) + prompts/skills reutilizáveis
  Respeitando as regras de Siena (AGENTS.md, plan-orchestration, proporcionalidade)

  Scenario: Estrutura copiada e adaptada
    Given que existe .agents/ no Portfolio com agents/*.toml, prompts/*.md e skills/*
    When o agente de planejamento copia e adapta para Siena
    Then .agents/ existe na raiz de Siena
    And agents/*.toml referenciam Siena.slnx e apps/mobile (não Portfolio.slnx ou apps/web)
    And frontend prompts e skills mencionam React Native/Expo ou são genéricos o suficiente
    And comandos de validação batem com plan-orchestration.md (dotnet Siena.slnx + mobile typecheck/test)
    And nenhum segredo ou URL de produção é introduzido
    And AGENTS.md e AI-CONFIG.md referenciam .agents/ como fonte de assets de agente

  Scenario: Planejamento multi-agente respeita Siena
    Given um task não-trivial (ex.: presença em treino)
    When usa multi-agent-feature-plan + explorer + architect + qa-reviewer
    Then o plano gerado cita apenas arquivos reais de Siena
    And workers são mencionados apenas como próxima etapa após aprovação humana
    And validação recomendada usa os comandos Siena-específicos

  Scenario: Workers write-safe
    Given backend-worker ou frontend-worker é invocado
    When o escopo write é aprovado
    Then ele respeita sandbox workspace-write
    And não edita arquivos fora da área declarada (api vs mobile)
    And roda validação Siena (build/test/typecheck)
```

## Passos ordenados

1. Ler referências obrigatórias (AGENTS.md Siena + Portfolio, AI-CONFIG.md, .cursor/rules/plan-orchestration.md, .cursor/permissions.json, docs/OVERENGINEERING.md, ARCHITECTURE.md, DESIGN.md, o PLAN_BROKER_GUIDE.md, estrutura completa de `.agents/` no Portfolio via list/read).
2. Executar `git status` e confirmar árvore limpa.
3. Criar diretório `.agents/` (e subdirs agents/, prompts/, skills/) na raiz de Siena.
4. Copiar `config.toml` (inalterado ou com comentário Siena).
5. Adaptar e copiar cada `agents/*.toml`:
   - Atualizar `description` e `developer_instructions` com paths corretos de Siena (`apps/api/Siena.slnx`, `apps/mobile`).
   - frontend-worker: trocar Next.js/Tailwind por React Native + TypeScript + Expo; ajustar validação para `cd apps/mobile && npm run typecheck && npm test` (ou lint equivalente do projeto); mencionar app/ e src/ do Siena; referenciar DESIGN.md + regras móveis.
   - backend-worker: Siena.slnx, referenciar Siena AGENTS + CODING_STANDARDS + TESTING.
   - Incluir pr-reviewer.toml (novo para Siena).
   - Manter sandbox_mode exato.
6. Copiar todos os `prompts/*.md`. Adaptar os stack-specific:
   - `create-frontend-component.md`: generalizar ou adicionar nota/variante para componentes RN (Expo Router, TSX em app/ ou src/components, styling atual do Siena).
   - `create-endpoint.md`, `feature-plan.md`, `implement-feature.md`, `review-pr.md` etc.: atualizar menções genéricas de "Portfolio" se houver; adicionar exemplos leves de Siena (treino, presença) quando didático, sem inventar regras.
   - Manter a estrutura de "Reusable Prompt", Checklist, Expected Output, Acceptance Criteria.
7. Copiar/adaptar `skills/* /SKILL.md`:
   - feature-delivery, test-strategy, pr-review, security-review, architecture-review, release-preparation.
   - Trocar paths de exemplo, comandos de build/test, referências a web → mobile quando aplicável.
   - Manter o espírito "use explorer antes de write", "peça aprovação humana para write-heavy", "não rode workers em paralelo no mesmo write-set".
8. Adaptar/copiar `.agents/README.md` (mantém genérico).
9. Atualizar `AGENTS.md` (Siena): adicionar `.agents/` na lista de referências obrigatórias (prioridade 2 ou 3) e mencionar em "Referências obrigatórias" ou seção de AI workflow.
10. Atualizar `AI-CONFIG.md`: marcar que `.agents/` agora está presente, expandir a tabela "Portfolio → Cursor mapping", adicionar seção curta "Como usar os assets de .agents/" (leitura de tomls para subagents, uso de prompts em feature plans, skills como composição).
11. Revisar `.cursor/rules/plan-orchestration.md`: adicionar `pr-reviewer` na tabela de subagentes (se o multi-agent prompts o usarem), garantir que mencione que definições concretas estão em `.agents/agents/*.toml`.
12. (Opcional, baixo esforço) Popular `docs/ai/` com versões adaptadas de AI_WORKFLOW.md / AI_PROMPTS.md do Portfolio ou criar índice apontando para .agents/ + AI-CONFIG.
13. Validar adaptações (grep por "Portfolio.slnx", "apps/web", "Next.js" dentro de `.agents/` de Siena — deve retornar vazio).
14. Publicar o plano via MCP PlanBroker (search + use_tool publish_plan) com task_id kebab-case.
15. Atualizar docs no mesmo "PR conceitual" (nenhum commit automático).
16. Reportar o que não foi validado (ex.: execução real de subagent com os novos tomls depende de setup Cursor/agent CLI + MCP plan-broker ativo).

## Arquivos prováveis

```
.agents/
  README.md (copiado + possível ajuste)
  config.toml (copiado)
  agents/
    architect.toml (adaptado)
    backend-worker.toml (adaptado - Siena.slnx)
    explorer.toml (adaptado)
    frontend-worker.toml (adaptado - RN/Expo)
    pr-reviewer.toml (novo)
    qa-reviewer.toml (adaptado)
    security-reviewer.toml (adaptado)
  prompts/
    ... (todos os 20 arquivos, alguns com notas Siena/RN)
  skills/
    architecture-review/SKILL.md (adaptado)
    feature-delivery/SKILL.md (adaptado)
    pr-review/SKILL.md (adaptado)
    release-preparation/SKILL.md (adaptado)
    security-review/SKILL.md (adaptado)
    test-strategy/SKILL.md (adaptado)
AGENTS.md (atualizado - referência .agents)
AI-CONFIG.md (atualizado - mapping + instruções de uso)
.cursor/rules/plan-orchestration.md (possível pequena atualização para pr-reviewer + menção .agents)
.cursor/plans/feat-adopt-portfolio-agents-architecture.md (publicado pelo broker)
docs/ai/ (opcional: índice ou arquivos)
```

## Testes obrigatórios

Não há testes unitários para assets de prompt (são documentação executável por humanos/agentes).

Validação obrigatória (conforme plan-orchestration e AI-CONFIG para área "Apenas docs"):

```bash
git diff --check
git status --short
```

Adicionais de sanidade (devem passar sem erro):
- `ls -la .agents .agents/agents .agents/prompts .agents/skills`
- `grep -r "Portfolio.slnx" .agents/ || echo "ok - nenhum remanescente"`
- `grep -r "apps/web" .agents/ || echo "ok - nenhum apps/web"`
- Leitura manual de 2-3 tomls e 2 prompts adaptados + 1 skill
- Confirmação de que comandos de validação nos workers/prompts/skills batem com a tabela de plan-orchestration.md (Siena.slnx + mobile typecheck/test)

Se MCP plan-broker estiver indisponível na sessão: fallback manual para gravação em `.cursor/plans/{task_id}.md` + update `index.json` (documentado no PLAN_BROKER_GUIDE).

## Validação final

- Estrutura .agents/ espelha a do Portfolio com adaptações Siena.
- Todos os arquivos de worker referenciam os comandos de build/test/typecheck corretos do projeto Siena.
- Nenhum arquivo de app ou contrato foi alterado.
- AGENTS.md e AI-CONFIG.md declaram .agents/ como parte do workflow de agentes.
- Plano publicado com sucesso via PlanBroker (status "ok") ou fallback file-based documentado.
- `exit_plan_mode` só após confirmação de publicação.
- Riscos/não-validados reportados (ex.: dependência de setup de `agent` CLI do Cursor e de plan-broker MCP ativo para uso pleno dos subagentes; prompts de frontend podem precisar de refinamento quando Siena mobile adicionar NativeWind/Tamagui ou admin-web).
