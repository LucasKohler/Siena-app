---
name: test-strategy
description: Workflow para definir, revisar e melhorar estratégia de testes sem gerar cobertura artificial.
---

# Test strategy

## Objetivo

Definir ou revisar uma estrategia de testes que prove comportamento relevante com o menor custo razoavel e baixa fragilidade.

## Quando usar

Use ao planejar feature, revisar PR, corrigir bug, avaliar lacunas de cobertura ou decidir entre unit, integration, contract, E2E, regression, performance e security tests.

## Quando não usar

Nao use para gerar testes falsos, snapshots sem valor, cobertura artificial ou suites grandes sem risco correspondente.

## Inputs necessários

- Comportamento ou risco a validar.
- Area do sistema afetada.
- Testes existentes, se conhecidos.
- Validacoes disponiveis.
- Criterios de aceite e historico de regressao, se houver.

## Workflow

1. Use `qa-reviewer` para revisar lacunas, testes frageis e risco de regressao.
2. Identifique o comportamento critico antes de escolher o tipo de teste.
3. Prefira o teste de menor custo que prove o comportamento relevante.
4. Classifique testes como obrigatorios, recomendados ou futuros.
5. Recomende unit tests para regras puras e casos de uso.
6. Recomende integration tests para persistencia, API, serializacao, DI ou infraestrutura real.
7. Recomende contract tests quando houver contrato entre frontend/backend ou API externa.
8. Recomende E2E apenas para fluxos criticos de usuario.
9. Sugira Testcontainers somente quando houver dependencia real de infraestrutura.
10. Exija build/test/lint quando aplicavel.

## Checklist

- [ ] O teste proposto valida comportamento, nao implementacao acidental.
- [ ] Testes frageis ou snapshots inuteis foram evitados.
- [ ] Dependencias externas foram isoladas ou justificadas.
- [ ] Dados de teste sao claros e minimamente suficientes.
- [ ] Validacoes disponiveis foram listadas.
- [ ] Lacunas foram priorizadas por risco.

## Output esperado

- Estrategia de testes por camada.
- Testes obrigatorios, recomendados e futuros.
- Justificativa de custo/beneficio.
- Comandos de validacao.
- Riscos de cobertura insuficiente.
- Itens que exigem decisao humana.

## Critérios de aceite

- A estrategia reduz risco real de regressao.
- Os testes recomendados sao executaveis no contexto do projeto.
- Nao ha incentivo a cobertura artificial.

## Coisas proibidas

- Criar testes que nao validam comportamento relevante.
- Inventar requisitos para justificar teste.
- Adicionar dependencia sem justificativa.
- Usar Testcontainers sem dependencia real de infraestrutura.
- Alterar codigo de producao sem aprovacao.
- Configurar CI/CD inexistente.

## Validação humana obrigatória

Exija decisao humana para aceitar lacunas criticas, adiar testes de contrato, introduzir ferramenta nova, aumentar tempo de pipeline ou testar com dados sensiveis.

## Relação com agentes e prompts existentes

- Use `qa-reviewer` como avaliador principal.
- Use `.agents/prompts/create-unit-tests.md` e `.agents/prompts/create-integration-tests.md` quando houver aprovacao para implementar testes.
- Use prompts multi-agent quando testes dependerem de arquitetura, seguranca ou contrato.
