---
name: architecture-review
description: Workflow para revisar arquitetura, boundaries, ADRs, acoplamento, coesão e risco de overengineering.
---

# Architecture review

## Objetivo

Avaliar a arquitetura atual ou uma mudanca proposta, diferenciando fatos observados, inferencias e recomendacoes praticas.

## Quando usar

Use para revisar boundaries, separacao de camadas, modularidade, ADRs, acoplamento, coesao, testabilidade ou risco de overengineering.

## Quando não usar

Nao use para implementar refatoracoes, criar ADR automaticamente, propor microservices sem evidencia ou discutir arquitetura sem arquivos concretos.

## Inputs necessários

- Area ou mudanca a revisar.
- Objetivo tecnico ou funcional.
- Arquivos, modulos ou diff relacionados, se conhecidos.
- Restricoes de escopo.
- Decisoes pendentes ou duvidas arquiteturais.

## Workflow

1. Use `.agents/prompts/multi-agent-architecture-review.md` para revisoes multi-agent.
2. Mantenha a revisao read-only.
3. Use `explorer` para coletar evidencias de estrutura real.
4. Use `architect` para avaliar boundaries, camadas, acoplamento, coesao e ADRs.
5. Verifique Clean Architecture pragmatica e modular monolith.
6. Avalie separacao entre Domain, Application, Infrastructure e Api.
7. Avalie frontend por features/componentes quando houver impacto de UI.
8. Identifique decisoes que precisam de ADR.
9. Liste o que nao deve mudar agora.

## Checklist

- [ ] Fatos observados foram separados de inferencias.
- [ ] Recomendacoes citam arquivos ou evidencias.
- [ ] Overengineering e arquitetura insuficiente foram avaliados.
- [ ] ADRs foram recomendados apenas para decisoes relevantes.
- [ ] Microservices nao foram propostos sem justificativa forte.
- [ ] Abstracoes novas foram evitadas quando nao ha dor real.

## Output esperado

- Diagnostico arquitetural.
- Fatos observados.
- Inferencias.
- Riscos.
- Decisoes que exigem ADR.
- Recomendacoes priorizadas.
- O que nao mudar agora.
- Proximos passos seguros.

## Critérios de aceite

- O review e baseado em evidencias do repositorio.
- As recomendacoes sao proporcionais ao tamanho e maturidade do projeto.
- Nenhuma implementacao e feita durante a revisao.

## Coisas proibidas

- Alterar arquivos.
- Criar ADR sem aprovacao.
- Propor microservices sem justificativa forte.
- Criar abstracoes por estetica.
- Inventar regras de negocio, contratos, tabelas ou dependencias.
- Configurar MCP externo ou deploy.

## Validação humana obrigatória

Exija aprovacao humana para ADRs, mudancas de boundary, breaking changes, refatoracoes amplas, alteracoes em persistencia, contratos publicos ou arquitetura de deploy.

## Relação com agentes e prompts existentes

- Use `architect` como agente central.
- Use `explorer` para evidencias.
- Use `.agents/prompts/multi-agent-architecture-review.md` para orquestracao.
- Consulte `docs/architecture/ARCHITECTURE.md`, `docs/architecture/DOMAIN.md`, `docs/architecture/OVERENGINEERING.md` e `AGENTS.md` como referencias normativas.
