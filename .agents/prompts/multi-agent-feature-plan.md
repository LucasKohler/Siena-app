# Multi-Agent Feature Plan

## Objetivo

Planejar uma feature com múltiplos agentes antes de qualquer implementação,
produzindo um plano incremental, validável e aprovado por humano.

## Quando usar

Use quando a feature tocar múltiplas áreas, contratos, rotas, validação,
arquitetura, dados, segurança, backend e frontend, ou quando houver incerteza
sobre impacto.

## Quando não usar

Não use para implementar diretamente. Não use para tarefas triviais com um único
arquivo e impacto claro. Não use para criar escopo maior do que o pedido.

## Agentes envolvidos

- `explorer`
- `architect`
- `qa-reviewer`
- `security-reviewer`, se houver risco
- `backend-worker` somente como agente futuro de implementação
- `frontend-worker` somente como agente futuro de implementação
- Agente principal como consolidador final

## Ordem de execução

1. `explorer` localiza arquivos, módulos, contratos, testes e fluxos
   relacionados.
2. `architect` avalia onde a feature entra na arquitetura e se há necessidade de
   ADR.
3. `qa-reviewer` propõe testes necessários e riscos de regressão.
4. `security-reviewer` avalia riscos se houver dados sensíveis, auth, input
   externo, exposição, dependências ou provider externo.
5. O agente principal consolida um plano incremental.
6. A implementação só pode começar após aprovação humana.

## Inputs necessários

- Descrição da feature.
- Comportamento esperado.
- Restrições de escopo.
- Contratos ou rotas afetados, se conhecidos.
- Critérios de aceite esperados.

## Escopo permitido

- Ler arquivos.
- Mapear impacto.
- Identificar riscos.
- Propor plano incremental.
- Propor testes e validações.
- Apontar decisões que exigem humano ou ADR.

## Escopo proibido

- Alterar arquivos.
- Implementar a feature.
- Acionar `backend-worker` ou `frontend-worker` para escrever sem aprovação.
- Criar migrations.
- Alterar contrato público sem destacar breaking change.
- Inventar endpoints, tabelas, contratos, dependências ou regras de negócio.
- Configurar MCP externo.
- Fazer deploy.

## Regras de segurança

- A saída inicial é plano, não código.
- Qualquer tarefa write-heavy exige plano consolidado e aprovação humana.
- Não acione `backend-worker` e `frontend-worker` em paralelo quando houver
  possibilidade de editar o mesmo arquivo, módulo, contrato, tipo
  compartilhado, fluxo funcional ou documentação técnica relacionada.
- Se backend e frontend dependerem do mesmo contrato ou fluxo, planeje a
  implementação em sequência: contrato/backend, validação, frontend e revisão
  final.
- Nenhum agente pode mexer em secrets, credenciais, produção ou dados privados.
- Se a feature envolver dados sensíveis, auth ou exposição externa, inclua
  revisão de segurança.

## Regras contra overengineering

- Planeje a menor mudança que entrega a feature.
- Não proponha novas camadas, pacotes, filas, microservices ou shared packages
  sem dor real.
- Se uma decisão puder esperar, marque como futuro.

## Regras contra alucinação

- Não invente regra de negócio.
- Não invente contrato.
- Não invente tabela ou migration.
- Não assuma que CI/CD, PR template ou skills existem.
- Se PR template for necessário e não existir, registre apenas como recomendação
  futura; não crie nesta tarefa.
- Baseie o plano em arquivos reais.

## Output esperado

- Entendimento da feature.
- Arquivos provavelmente afetados.
- Riscos.
- Fora de escopo.
- Plano incremental.
- Testes necessários.
- Validação necessária.
- Decisões que exigem humano.
- Critérios de aceite.
- Próximo prompt recomendado para implementação.

## Critérios de aceite

- Nenhum arquivo foi alterado.
- Plano é incremental.
- Riscos e breaking changes são destacados.
- Workers são citados apenas como etapa futura após aprovação.
- Se houver ambiguidade, o output recomenda perguntar antes de implementar.

## Checklist de validação humana

- Confirmar que o escopo está correto.
- Confirmar que o plano não adiciona trabalho não solicitado.
- Confirmar que riscos, testes e validação são suficientes.
- Aprovar explicitamente qualquer etapa workspace-write.

## Exemplo de uso

```txt
Use este prompt para planejar a feature X em modo multi-agent. Não implemente.
Mapeie arquivos, arquitetura, testes, riscos de segurança, plano incremental e
critérios de aceite. Indique o próximo prompt para implementação somente após
aprovação humana.
```
