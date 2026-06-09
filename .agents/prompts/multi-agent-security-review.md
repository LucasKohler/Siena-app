# Multi-Agent Security Review

## Objetivo

Fazer uma revisão de segurança multi-agent em modo read-only, com achados
baseados em evidência e próximos passos seguros.

## Quando usar

Use antes de release, antes de adicionar provedores externos, ao revisar
endpoints, dados sensíveis, auth, dependências, Docker, logs, CORS, MCP ou
permissões de agentes.

## Quando não usar

Não use para rotacionar secrets automaticamente, alterar configuração de
produção, instalar ferramentas, configurar MCP externo ou aplicar correções sem
plano aprovado.

## Agentes envolvidos

- `security-reviewer`
- `explorer`
- `qa-reviewer`, se houver necessidade de teste de segurança/regressão
- `pr-reviewer`, se houver diff
- `architect`, se houver impacto estrutural
- Agente principal como consolidador final

## Ordem de execução

1. `explorer` localiza arquivos e fluxos sensíveis.
2. `security-reviewer` avalia secrets, dados sensíveis, auth, autorização,
   validação, output handling, logs, dependências, CORS, headers, docs expostas,
   MCP/prompt injection quando aplicável e permissões de agentes.
3. `qa-reviewer` recomenda testes de segurança/regressão quando necessário.
4. `architect` avalia impacto estrutural quando aplicável.
5. `pr-reviewer` revisa diffs quando houver.
6. O agente principal consolida achados por severidade.

## Inputs necessários

- Escopo da revisão.
- Diff ou área de código, se houver.
- Ambiente alvo, se conhecido.
- Mudanças de configuração, se houver.
- Restrições e fora de escopo.

## Escopo permitido

- Ler arquivos.
- Identificar riscos de segurança.
- Classificar severidade.
- Recomendar mitigação prática.
- Recomendar testes.
- Indicar validação humana necessária.

## Escopo proibido

- Alterar arquivos.
- Rotacionar secrets automaticamente.
- Alterar configuração de produção.
- Configurar MCP externo.
- Fazer deploy.
- Instalar dependências.
- Executar ações destrutivas.
- Criar migrations.
- Fazer alarmismo sem evidência.

## Regras de segurança

- Fluxo read-only.
- Não tocar secrets, credenciais, tokens, produção ou dados privados.
- Não exfiltrar conteúdo sensível em respostas.
- Se encontrar possível secret, token, chave, senha ou credencial, não revele o
  valor bruto. Informe apenas o caminho do arquivo, tipo provável de segredo,
  contexto mínimo e ação recomendada.
- Nunca copie secrets para a resposta final.
- Não configurar MCP externo.
- Qualquer ação sobre auth, secrets, provider config, produção ou dados pessoais
  exige aprovação humana.

## Regras contra overengineering

- Recomende mitigação proporcional ao risco.
- Não proponha ferramentas ou plataformas pesadas sem necessidade real.
- Diferencie risco atual de hardening futuro.

## Regras contra alucinação

- Não invente vulnerabilidades sem evidência.
- Cite arquivos reais.
- Diferencie fato, inferência e hipótese.
- Não assuma CI/CD, PR template, MCP externo ou skills existentes.

## Output esperado

- Resumo executivo.
- Achados por severidade.
- Evidências.
- Impacto.
- Recomendação prática.
- Testes recomendados.
- Validação humana necessária.
- Próximos passos seguros.
- Itens não validados.

## Critérios de aceite

- Nenhum arquivo foi alterado.
- Achados têm evidência.
- Severidade é clara.
- Recomendações são práticas e proporcionais.
- O agente principal entrega resposta consolidada.

## Checklist de validação humana

- Confirmar se há secrets ou dados sensíveis envolvidos.
- Confirmar se alguma correção exige aprovação de produção.
- Confirmar que achados críticos têm evidência suficiente.
- Confirmar próximos passos antes de qualquer alteração.

## Exemplo de uso

```txt
Use este prompt para fazer uma revisão de segurança multi-agent read-only.
Não altere arquivos. Avalie secrets, auth, validação, output handling, logs,
dependências, CORS, Docker, MCP/prompt injection quando aplicável e permissões
de agentes. Consolide achados por severidade.
```
