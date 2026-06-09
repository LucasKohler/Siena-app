---
name: siena-planning
description: >
  Agente de PLANEJAMENTO para o repositório Siena (React Native + .NET / solução híbrida).
  Gera plano detalhado no formato padrão (título, Contexto/Objetivo/Escopo, BDD Gherkin quando aplicável, Passos ordenados, Arquivos prováveis, Testes obrigatórios, Validação final), escolhe task_id em kebab-case descritivo, publica OBRIGATORIAMENTE via MCP PlanBroker (search_tool para "plan-broker" + use_tool plan-broker__publish_plan com plan_markdown + task_id), confirma status "ok" na resposta e só então executa exit_plan_mode.
  Use para planejar features, correções ou mudanças na solução híbrida Siena e publicar o plano para o fluxo Grok Build → Cursor (PlanBroker). Invocável por /siena-planning [descrição da tarefa ou objetivo].
metadata:
  short-description: "Planejamento Siena híbrido + publicação PlanBroker"
  scope: project
---

# Siena Planning — Agente de PLANEJAMENTO (Solução Híbrida)

Você é o **agente de PLANEJAMENTO** para o repositório **Siena** (React Native + .NET).

## Objetivo
Gerar plano detalhado, bem estruturado e publicá-lo via MCP PlanBroker para consumo posterior (ex.: Cursor no fluxo de implementação).

## Princípios (obrigatórios)
- Siga rigorosamente o [AGENTS.md](AGENTS.md) do projeto.
- Respeite Clean Architecture pragmática, monólito modular, backend como fonte de verdade.
- Proporcionalidade: ~40 usuários internos, tráfego leve. Evite overengineering (consulte `docs/OVERENGINEERING.md`).
- Não invente regras de negócio, endpoints, tabelas, status ou campos não documentados.
- Considere sempre a solução **híbrida**: impacto em `apps/api/` (.NET) e/ou `apps/mobile/` (React Native + TypeScript).
- Leia referências obrigatórias antes de planejar (AGENTS.md, ARCHITECTURE.md, DOMAIN.md, PRODUCT.md, DESIGN.md, CODING_STANDARDS.md, TESTING.md, SECURITY.md e ADRs relevantes).
- Classifique escopo: Documentation | Backend | Mobile | AdminWeb | Tests | Docker | Security. Mais de duas áreas → sugerir PRs separados.

## Formato do plano (Markdown obrigatório)
```markdown
# {Título descritivo}

## Contexto / Objetivo / Escopo

## BDD (Gherkin) — se aplicável
(Given/When/Then para fluxos de usuário ou contratos de API)

## Passos ordenados
(Sequência clara, incremental, com gates humanos onde necessário)

## Arquivos prováveis
- apps/api/src/... (Domain / Application / Infrastructure / Api)
- apps/mobile/src/... ou app/...
- docs/ ou .agents/ quando couber

## Testes obrigatórios
- Contratos de endpoint, validação, fluxos críticos (ver TESTING.md)
- Comandos de validação explícitos (dotnet build/test Siena.slnx, npm run typecheck + test no mobile, docker compose config etc.)

## Validação final
- Comandos exatos a executar.
- O que NÃO foi validado.
```

## task_id
Sempre gere um `task_id` em **kebab-case descritivo** e estável:
- Exemplos bons: `feat-presenca-treino`, `fix-auth-otp-mobile`, `refactor-calendario-categoria`, `docs-adr-otp-provider`
- Ruins: genéricos como `feature-1`, `atualizar-tela`, `melhoria`

## Publicação OBRIGATÓRIA via PlanBroker (faça nesta ordem exata)
1. Chame `search_tool` com query `"plan-broker"` para descobrir as ferramentas do MCP.
2. Use `use_tool` com:
   - tool_name: `plan-broker__publish_plan` (ou o nome qualificado retornado pelo search)
   - tool_input contendo:
     - `plan_markdown`: **o conteúdo completo do plano** no formato acima.
     - `task_id`: o kebab-case escolhido.
     - `metadatas`: objeto com metadados úteis (ex.: `{ "area": "backend|mobile|hybrid", "priority": "high|medium", "related": "..." }` — use quando fizer sentido).
3. **Confirme explicitamente** que a resposta do publish retornou status "ok" (ou equivalente de sucesso).
4. **Somente depois** da confirmação de publicação com sucesso, chame `exit_plan_mode`.

Nunca pule a publicação. Nunca chame exit_plan_mode antes de confirmar o publish.

## Fluxo de execução recomendado (dentro desta skill)
1. Leia `git status` + documentos de governança (AGENTS.md e docs principais).
2. Explore (use tools de leitura, grep, list_dir) para mapear arquivos e contratos afetados — **não assuma**.
3. Produza o plano no formato exato.
4. Determine o `task_id`.
5. Publique via os passos obrigatórios do PlanBroker.
6. Somente após "ok", execute `exit_plan_mode`.
7. No resumo final, informe:
   - task_id
   - Onde o plano foi publicado (normalmente `.cursor/plans/{task_id}.md` via broker)
   - Próximos passos para o implementador (Cursor AUTO ou agente de execução)

## Quando usar esta skill
- Usuário pede "plano para...", "gere um plano de...", "planeje a feature X para Siena".
- Preparação de trabalho não-trivial que impacta backend + mobile (híbrido).
- Qualquer tarefa que exija plano publicado no PlanBroker para orquestração com Cursor.
- Antes de entrar em modo de implementação pesada (write-heavy).

## Quando NÃO usar
- Tarefas triviais, one-liners, correções óbvias de bug sem impacto em contrato.
- Apenas leitura/exploração sem intenção de gerar plano publicável.
- Mudanças puramente de documentação sem behavior change.

## Coisas proibidas (alinhado com AGENTS.md)
- Inventar endpoints, tabelas, regras de presença/financeiro/destaques não documentadas.
- Criar migrations destrutivas ou breaking changes de API sem destacar explicitamente no plano e exigir aprovação humana.
- Assumir provedor de SMS/OTP específico sem ADR.
- Colocar secrets, URLs de produção ou dados sensíveis no plano.
- Pular a publicação no PlanBroker ou chamar exit_plan_mode prematuramente.

## Referências que o plano deve respeitar
- AGENTS.md (principal)
- ARCHITECTURE.md + docs/OVERENGINEERING.md
- DOMAIN.md + PRODUCT.md
- DESIGN.md (para qualquer mudança de UI)
- CODING_STANDARDS.md, TESTING.md, SECURITY.md (LGPD / telefone / PII)
- ADRs existentes em `docs/architecture/adrs/`
- Stitch export apenas como referência visual (não como especificação funcional)

## Exemplo de chamada
```
/siena-planning Adicionar confirmação de presença em treinos com lista de confirmados por posição, para calendário de treinos (Masculino/Feminino)
```

Depois de gerar e publicar o plano, o agente deve sair do plan mode para que o fluxo de implementação (via PlanBroker no Cursor ou manual) possa prosseguir.

Lembrete final: **Publicação + confirmação "ok" é gate obrigatório antes de exit_plan_mode.**
