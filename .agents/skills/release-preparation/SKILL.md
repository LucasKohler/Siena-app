---
name: release-preparation
description: Workflow para preparar uma release com checklist, riscos, validações, rollback e documentação.
---

# Release preparation

## Objetivo

Preparar uma release com escopo claro, validacoes explicitas, riscos conhecidos, rollback planejado e notas objetivas, sem executar deploy automaticamente.

## Quando usar

Use antes de publicar uma versao, fechar milestone, preparar release notes, revisar readiness operacional ou consolidar mudancas para entrega.

## Quando não usar

Nao use para fazer deploy, configurar pipeline, criar branch protection, alterar secrets, criar CI/CD ou automatizar release sem aprovacao humana.

## Inputs necessários

- Escopo da release.
- Commits, PRs ou mudancas incluidas.
- Validacoes executadas e pendentes.
- Riscos conhecidos.
- Impacto em contratos, migrations, config, seguranca, performance e documentacao.
- Ambiente alvo e plano de rollback esperado, se houver.

## Workflow

1. Revise o escopo funcional e tecnico da release.
2. Liste mudancas funcionais, tecnicas e documentais.
3. Verifique impactos em contratos, migrations, configs, secrets, performance e seguranca.
4. Liste validacoes executadas e nao executadas.
5. Identifique riscos, mitigacoes e responsaveis por decisao.
6. Prepare plano de rollback proporcional ao risco.
7. Recomende smoke tests e checklist pos-deploy, mesmo que CI/CD ainda nao exista.
8. Gere release notes objetivas.
9. Peça intervencao humana para risco alto.

## Checklist

- [ ] Escopo da release esta claro.
- [ ] Mudancas funcionais e tecnicas foram separadas.
- [ ] Breaking changes foram destacados.
- [ ] Migrations foram revisadas, se existirem.
- [ ] Configs e secrets foram tratados sem exposicao.
- [ ] Validacoes executadas e pendentes foram listadas.
- [ ] Rollback e smoke test foram definidos.
- [ ] Release notes nao prometem o que nao foi entregue.

## Output esperado

- Resumo da release.
- Lista de mudancas.
- Riscos e mitigacoes.
- Validacoes executadas e nao executadas.
- Impactos em contrato, dados, config, seguranca, performance e docs.
- Plano de rollback.
- Checklist pos-deploy recomendado.
- Release notes objetivas.

## Critérios de aceite

- A release pode ser avaliada por humano sem depender de contexto oculto.
- Riscos altos estao destacados antes de qualquer deploy.
- Validacoes pendentes nao sao apresentadas como executadas.

## Coisas proibidas

- Fazer deploy automaticamente.
- Alterar pipeline ou CI/CD inexistente.
- Alterar secrets ou revelar valores.
- Criar migration destrutiva sem aprovacao.
- Aprovar release com risco alto sem intervencao humana.
- Assumir branch protection, PR template ou CI/CD existentes.

## Validação humana obrigatória

Exija aprovacao humana para deploy, rollback real, risco alto, breaking changes, migrations, mudancas em auth, config sensivel, dados pessoais ou performance critica.

## Relação com agentes e prompts existentes

- Use `.agents/prompts/prepare-release.md` para preparacao single-agent.
- Use `pr-reviewer`, `qa-reviewer` e `security-reviewer` quando a release envolver diffs, lacunas de teste ou risco de seguranca.
- Consulte `CONTRIBUTING.md`, `docs/ai/AI-CONFIG.md` e `.agents/skills/release-preparation/SKILL.md` para criterios de processo.
