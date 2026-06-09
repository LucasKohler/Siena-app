---
name: feature-delivery
description: Workflow para planejar, implementar e validar uma feature usando AI-Driven Development com escopo controlado.
---

# Feature delivery

## Objetivo

Entregar features de forma incremental, validada e rastreavel, usando agentes e prompts existentes sem aumentar escopo artificialmente.

## Quando usar

Use quando uma feature exigir discovery, impacto em backend/frontend, mudanca em contrato, alteracao de comportamento, testes novos ou coordenacao entre agentes.

## Quando não usar

Nao use para correcoes triviais, ajustes puramente textuais, tarefas read-only ou exploracoes sem intencao de implementar.

## Inputs necessários

- Descricao da feature e problema que ela resolve.
- Comportamento esperado e criterios de aceite.
- Escopo permitido e proibido.
- Areas possivelmente afetadas.
- Riscos conhecidos, contratos envolvidos e validacoes esperadas.

## Workflow

1. Entenda a feature antes de editar arquivos.
2. Para features medias ou grandes, use `.agents/prompts/multi-agent-feature-plan.md`.
3. Use `explorer` para localizar arquivos, modulos, fluxos, contratos e testes relevantes.
4. Use `architect` quando houver impacto em boundaries, camadas, contratos ou ADR.
5. Consolide um plano unico com escopo, etapas, riscos, testes e validacoes.
6. Peça aprovacao humana antes de qualquer tarefa write-heavy.
7. Acione `backend-worker` ou `frontend-worker` apenas quando o write set estiver claro.
8. Nao acione workers em paralelo no mesmo arquivo, modulo, contrato, tipo compartilhado, fluxo funcional ou documentacao tecnica relacionada.
9. Implemente em passos pequenos, validando build, test e lint quando aplicavel.
10. Finalize com resumo de arquivos alterados, comandos executados, riscos remanescentes e proximos passos.

## Checklist

- [ ] A feature foi entendida sem inventar regra de negocio.
- [ ] Arquivos e fluxos relevantes foram identificados.
- [ ] O plano consolidado foi aprovado antes de write-heavy.
- [ ] Workers nao atuaram em write sets conflitantes.
- [ ] Testes relevantes foram criados ou justificados.
- [ ] Validacoes disponiveis foram executadas ou declaradas como nao executadas.
- [ ] Breaking changes foram destacados.

## Output esperado

- Plano ou implementacao incremental, conforme aprovacao.
- Lista de arquivos criados, alterados e movidos.
- Comandos executados e resultado.
- Testes adicionados ou justificativa para nao adicionar.
- Riscos, impactos e proximos passos.

## Critérios de aceite

- A feature atende aos criterios de aceite informados.
- O escopo foi respeitado.
- Nenhum contrato publico, migration ou dependencia foi alterado sem destaque e aprovacao.
- As validacoes relevantes foram executadas ou explicitamente declaradas como pendentes.

## Coisas proibidas

- Inventar endpoint, tabela, contrato, regra de negocio ou dependencia.
- Fazer deploy.
- Configurar MCP externo.
- Mexer em secrets ou revelar secrets.
- Criar migration destrutiva sem aprovacao humana.
- Usar workers em paralelo no mesmo write set.
- Expandir escopo sem aprovacao humana.

## Validação humana obrigatória

Exija aprovacao humana para tarefas write-heavy, breaking changes, migrations, alteracoes de auth, mudancas em secrets/configuracao sensivel, impacto em dados ou qualquer ambiguidade de requisito.

## Relação com agentes e prompts existentes

- Use `explorer` e `architect` antes de implementacoes relevantes.
- Use `.agents/prompts/multi-agent-feature-plan.md` para planejamento multi-agent.
- Use `backend-worker` e `frontend-worker` somente apos plano aprovado.
- Use `qa-reviewer` e `security-reviewer` para revisar testes e riscos antes do fechamento.
