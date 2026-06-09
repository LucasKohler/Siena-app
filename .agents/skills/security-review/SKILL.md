---
name: security-review
description: Workflow para revisar riscos de segurança, privacidade, secrets, auth, input/output handling e permissões de agentes.
---

# Security review

## Objetivo

Revisar riscos de seguranca e privacidade em modo read-only, com severidade, evidencia e recomendacoes seguras.

## Quando usar

Use para revisar auth, autorizacao, input/output handling, logs, dependencias, CORS, headers, docs expostas, secrets, dados sensiveis, MCP/prompt injection ou permissoes de agentes.

## Quando não usar

Nao use para rotacionar secrets, alterar producao, configurar MCP externo, aplicar correcoes automaticamente ou fazer auditoria sem evidencias.

## Inputs necessários

- Area, diff ou fluxo a revisar.
- Tipo de dado tratado.
- Superficies externas: APIs, forms, uploads, auth, logs, integrações.
- Contexto de ambiente, quando conhecido.
- Restricoes de escopo.

## Workflow

1. Use `.agents/prompts/multi-agent-security-review.md`.
2. Mantenha a revisao read-only.
3. Use `security-reviewer` como agente principal.
4. Use `explorer` para localizar arquivos sensiveis.
5. Use `qa-reviewer`, `architect` ou `pr-reviewer` quando houver impacto em testes, arquitetura ou diff.
6. Classifique achados por severidade.
7. Cite evidencias com caminho de arquivo e contexto minimo.
8. Recomende a acao segura mais simples.
9. Declare o que nao foi validado.

## Checklist

- [ ] Secrets e dados sensiveis foram tratados sem exposicao de valor bruto.
- [ ] Auth e autorizacao foram avaliadas quando aplicavel.
- [ ] Input validation e output handling foram revisados.
- [ ] Logs sensiveis foram considerados.
- [ ] Dependencias, CORS, headers e docs expostas foram avaliadas quando aplicavel.
- [ ] MCP/prompt injection e permissoes de agentes foram considerados quando relevantes.
- [ ] Alarmismo sem evidencia foi evitado.

## Output esperado

- Resumo executivo.
- Achados por severidade.
- Evidencias.
- Impacto.
- Recomendacao pratica.
- Testes recomendados.
- Validacao humana necessaria.
- Proximos passos seguros.

## Critérios de aceite

- Nenhum valor bruto de secret e revelado.
- Todo achado relevante tem evidencia e impacto.
- Recomendacoes nao executam alteracoes destrutivas ou de producao.

## Coisas proibidas

- Revelar secrets, tokens, chaves, senhas ou credenciais.
- Rotacionar secrets automaticamente.
- Alterar arquivos sem aprovacao explicita.
- Fazer deploy.
- Configurar MCP externo.
- Fazer alarmismo sem base concreta.
- Aprovar risco alto sem intervencao humana.

## Validação humana obrigatória

Exija intervencao humana para secrets reais, auth/autorizacao, dados pessoais, configuracao de producao, provider config, risco alto ou qualquer acao que envolva credenciais.

## Relação com agentes e prompts existentes

- Use `security-reviewer` como agente central.
- Use `.agents/prompts/multi-agent-security-review.md` para orquestracao.
- Respeite `SECURITY.md`, `AGENTS.md` e as restricoes de sandbox dos agentes.
