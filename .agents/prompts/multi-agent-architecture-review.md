# Multi-Agent Architecture Review

## Objetivo

Avaliar a arquitetura do projeto ou de uma mudança específica com múltiplos
agentes, sem alterar arquivos.

## Quando usar

Use antes de refactors, mudanças de estrutura, introdução de banco, novos
contratos, novas integrações, mudanças cross-cutting ou decisões que possam
exigir ADR.

## Quando não usar

Não use para tarefas triviais. Não use para justificar abstrações sem dor real.
Não use para criar ADR automaticamente.

## Agentes envolvidos

- `architect`
- `explorer`
- `qa-reviewer`
- `security-reviewer`, se houver auth, dados sensíveis, integrações externas ou
  exposição
- `pr-reviewer`, se houver diff
- Agente principal como consolidador final

## Ordem de execução

1. `explorer` mapeia estrutura, dependências, entrypoints e arquivos
   relevantes.
2. `architect` avalia boundaries, camadas, acoplamento, coesão,
   overengineering, arquitetura insuficiente e ADRs.
3. `qa-reviewer` avalia testabilidade e proteção contra regressão.
4. `security-reviewer` avalia impacto estrutural de segurança quando aplicável.
5. `pr-reviewer` avalia coerência do diff quando houver diff.
6. O agente principal consolida diagnóstico e próximos passos.

## Inputs necessários

- Área ou mudança a revisar.
- Objetivo técnico ou problema observado.
- Diff, se houver.
- Restrições de escopo.

## Escopo permitido

- Ler arquivos.
- Avaliar arquitetura.
- Identificar decisões que exigem ADR.
- Recomendar próximos passos.
- Separar fatos e inferências.

## Escopo proibido

- Alterar arquivos.
- Criar ADR automaticamente sem aprovação.
- Propor microservices sem justificativa forte.
- Propor abstrações sem dor real.
- Criar código, migrations, contratos ou dependências.
- Configurar MCP externo.
- Fazer deploy.

## Regras de segurança

- Fluxo read-only.
- Não tocar secrets, credenciais, produção ou dados privados.
- Se houver auth, dados sensíveis ou exposição externa, inclua
  `security-reviewer`.
- Breaking changes devem ser destacados.

## Regras contra overengineering

- Preferir Clean Architecture pragmática e modular monolith.
- Não recomendar camadas, mediators, shared packages ou microservices sem
  evidência.
- Dizer explicitamente o que não mudar agora.

## Regras contra alucinação

- Não invente regras de negócio, endpoints, tabelas, migrations ou contratos.
- Cite caminhos reais.
- Diferencie fato observado de inferência.
- Se algo exigir validação, diga o que não foi validado.

## Output esperado

- Diagnóstico arquitetural.
- Fatos observados.
- Inferências.
- Riscos.
- Decisões que exigem ADR.
- Recomendações priorizadas.
- O que não mudar agora.
- Próximos passos.

## Critérios de aceite

- Nenhum arquivo foi alterado.
- Recomendações têm evidência.
- Overengineering é evitado.
- ADRs são recomendados quando necessários, não criados automaticamente.
- O agente principal entrega resposta consolidada.

## Checklist de validação humana

- Confirmar se a decisão realmente exige ADR.
- Confirmar que a recomendação cabe no estágio atual do projeto.
- Confirmar que não há mudança de contrato sem destaque.
- Confirmar que não há proposta de arquitetura excessiva.

## Exemplo de uso

```txt
Use este prompt para revisar a arquitetura da área X em modo read-only. Avalie
Clean Architecture pragmática, modularidade, acoplamento, testabilidade,
segurança quando aplicável, necessidade de ADR e o que não mudar agora.
```
