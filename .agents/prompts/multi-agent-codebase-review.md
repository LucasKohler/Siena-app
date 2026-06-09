# Multi-Agent Codebase Review

## Objetivo

Fazer uma revisão read-only do codebase usando múltiplos agentes especializados
e consolidar os achados em uma única resposta acionável.

## Quando usar

Use quando precisar entender a estrutura atual, riscos, lacunas de testes,
segurança, arquitetura, manutenção ou prontidão do projeto antes de qualquer
implementação.

## Quando não usar

Não use para implementar correções, mover arquivos, criar código, criar
migrations, alterar contratos, configurar CI/CD, fazer deploy ou executar
mudanças automáticas.

## Agentes envolvidos

- `explorer`
- `architect`
- `qa-reviewer`
- `security-reviewer`
- `pr-reviewer`, se fizer sentido como consolidador técnico
- Agente principal como consolidador final

## Ordem de execução

1. `explorer` mapeia estrutura, entrypoints, módulos, dependências, testes,
   configuração e arquivos relevantes.
2. `architect` avalia arquitetura, boundaries, acoplamento, coesão, Clean
   Architecture pragmática e overengineering.
3. `qa-reviewer` avalia lacunas de testes, testes frágeis e ausência de
   regressão.
4. `security-reviewer` avalia secrets, auth, autorização, input validation,
   output handling, logs sensíveis e dependências.
5. `pr-reviewer` consolida riscos de bugs, regressões, contratos, performance,
   manutenibilidade e escopo, se útil.
6. O agente principal deduplica tudo e entrega uma resposta única.

## Inputs necessários

- Objetivo da revisão.
- Escopo desejado, se houver.
- Áreas fora de escopo, se houver.
- Critérios especiais de risco, se houver.

## Escopo permitido

- Ler arquivos.
- Mapear estrutura e dependências.
- Identificar riscos e lacunas.
- Recomendar próximos passos seguros.
- Citar caminhos de arquivos como evidência.

## Escopo proibido

- Alterar arquivos.
- Criar código.
- Criar migrations.
- Instalar dependências.
- Alterar contratos públicos.
- Configurar MCP externo.
- Configurar CI/CD.
- Alterar Docker.
- Fazer deploy.
- Criar plano de implementação detalhado demais.
- Sair implementando correções.

## Regras de segurança

- Todos os agentes deste fluxo operam em modo read-only.
- Nenhum agente pode mexer em secrets, credenciais, tokens, produção ou dados
  privados.
- Nenhum agente pode configurar MCP externo.
- Nenhum agente pode executar ação destrutiva.
- O agente principal deve declarar o que não foi validado.

## Regras contra overengineering

- Priorize riscos reais observados no repositório.
- Não recomende microservices, filas, observabilidade completa ou arquitetura
  pesada sem evidência forte.
- Diferencie melhoria necessária de melhoria futura.

## Regras contra alucinação

- Não invente endpoints, tabelas, migrations, contratos, dependências ou regras
  de negócio.
- Diferencie fatos observados de inferências.
- Cite arquivos reais para cada achado relevante.
- Se algo não puder ser validado, diga explicitamente.

## Output esperado

- Resumo executivo.
- Achados por agente.
- Severidade.
- Evidências com caminhos de arquivos.
- Riscos.
- Recomendações priorizadas.
- Próximos passos seguros.
- Itens não validados.

## Critérios de aceite

- Nenhum arquivo foi alterado.
- Achados têm evidência concreta.
- Severidade está clara.
- Recomendações são priorizadas e realistas.
- O agente principal entrega uma resposta consolidada, não respostas soltas.

## Checklist de validação humana

- Confirmar que a revisão foi read-only.
- Confirmar que achados críticos têm evidência.
- Confirmar que próximos passos não ampliam o escopo indevidamente.
- Confirmar que nenhuma recomendação exige alteração sem plano posterior.

## Exemplo de uso

```txt
Use este prompt para fazer uma revisão multi-agent read-only do codebase.
Não altere arquivos. Consolide achados de explorer, architect, qa-reviewer,
security-reviewer e pr-reviewer em uma única resposta com severidade,
evidências e próximos passos seguros.
```
