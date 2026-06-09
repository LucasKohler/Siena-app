---
name: pr-review
description: Workflow para revisar PRs/diffs com foco em bugs, regressões, segurança, arquitetura, testes e escopo.
---

# PR review

## Objetivo

Revisar PRs ou diffs em modo read-only, priorizando riscos reais de bug, regressao, contrato, seguranca, performance, manutencao e escopo.

## Quando usar

Use antes de merge, ao revisar um diff local, ao avaliar uma branch de feature ou quando o usuario pedir uma revisao tecnica de mudancas.

## Quando não usar

Nao use para formatacao cosmetica isolada, aprovacao automatica de PR ou implementacao de correcoes.

## Inputs necessários

- Link do PR, branch ou diff a revisar.
- Objetivo da mudanca.
- Criterios de aceite.
- Validacoes executadas pelo autor.
- Areas de risco conhecidas.

## Workflow

1. Use `.agents/prompts/multi-agent-pr-review.md` quando a revisao envolver multiplas dimensoes.
2. Mantenha a revisao read-only.
3. Use `explorer` apenas se o diff nao fornecer contexto suficiente.
4. Use `pr-reviewer` para bugs, regressões, contratos, performance, manutencao e escopo.
5. Use `qa-reviewer` para lacunas de teste e risco de regressao.
6. Use `security-reviewer` para auth, secrets, input/output handling e dependencias.
7. Use `architect` para boundaries, acoplamento e overengineering.
8. Consolide os achados em uma unica resposta.

## Checklist

- [ ] O review cita evidencias com caminhos de arquivos.
- [ ] Achados bloqueantes estao separados dos importantes e sugestoes.
- [ ] Comentarios cosmeticos irrelevantes foram evitados.
- [ ] Testes, contratos, seguranca, performance e manutencao foram considerados.
- [ ] Perguntas ao autor foram listadas quando necessario.
- [ ] O PR nao foi aprovado automaticamente.

## Output esperado

- Resumo do PR/diff.
- Achados bloqueantes.
- Achados importantes.
- Sugestoes nao bloqueantes.
- Evidencias com arquivos e contexto minimo.
- Perguntas ao autor.
- Checklist antes de merge.
- Recomendacao final: bloquear, aprovar com ressalvas ou sem bloqueios aparentes.

## Critérios de aceite

- Todo achado relevante tem impacto claro e evidencia.
- A revisao evita opinioes cosmeticas sem risco pratico.
- A resposta final e consolidada, objetiva e acionavel.

## Coisas proibidas

- Alterar arquivos durante o review.
- Aprovar PR automaticamente.
- Fazer deploy.
- Revelar secrets.
- Configurar MCP externo.
- Sugerir mudancas sem impacto real.
- Transformar review em refatoracao ampla.

## Validação humana obrigatória

Exija decisao humana para merge, aceitacao de risco, breaking change, migration, seguranca, performance critica ou ausencia de validacoes importantes.

## Relação com agentes e prompts existentes

- Use `.agents/prompts/multi-agent-pr-review.md` como prompt principal.
- Use `pr-reviewer`, `qa-reviewer`, `security-reviewer`, `architect` e `explorer` conforme necessidade.
- Respeite `CONTRIBUTING.md` para expectativas de PR e validacao.
