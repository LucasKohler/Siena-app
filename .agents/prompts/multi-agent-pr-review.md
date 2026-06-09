# Multi-Agent PR Review

## Objetivo

Revisar um PR ou diff com múltiplos agentes, sem alterar arquivos, priorizando
bugs, regressões, contratos, testes, segurança, performance, manutenibilidade e
escopo.

## Quando usar

Use antes de merge, antes de release, após uma mudança significativa ou quando o
diff tocar áreas sensíveis.

## Quando não usar

Não use para comentários cosméticos. Não use para aprovar automaticamente. Não
use para reescrever o PR.

## Agentes envolvidos

- `pr-reviewer`
- `security-reviewer`
- `qa-reviewer`
- `architect`
- `explorer`, se for necessário localizar contexto
- Agente principal como consolidador final

## Ordem de execução

1. `explorer` localiza contexto se o diff não for suficiente.
2. `pr-reviewer` revisa bugs, regressões, contratos, performance,
   manutenibilidade e escopo.
3. `security-reviewer` revisa segurança, secrets, auth, input/output handling e
   dependências.
4. `qa-reviewer` revisa testes faltantes, testes frágeis e risco de regressão.
5. `architect` revisa boundaries, acoplamento, overengineering e coerência
   arquitetural.
6. O agente principal consolida achados em uma única revisão.

## Inputs necessários

- Diff ou descrição do PR.
- Objetivo do PR.
- Arquivos alterados.
- Validações executadas, se houver.
- Áreas fora de escopo, se houver.

## Escopo permitido

- Ler diff e arquivos relacionados.
- Apontar achados com severidade.
- Fazer perguntas ao autor.
- Recomendar validações antes de merge.

## Escopo proibido

- Alterar arquivos.
- Aprovar automaticamente o PR.
- Fazer comentários cosméticos irrelevantes.
- Fazer review genérico sem evidência.
- Sugerir mudanças sem impacto real.
- Configurar MCP externo.
- Fazer deploy.
- Criar migrations ou dependências.

## Regras de segurança

- Review é read-only.
- Não mexa em secrets, credenciais ou produção.
- Não execute ações destrutivas.
- Breaking changes devem ser destacados.

## Regras contra overengineering

- Foque em achados que mudam comportamento, segurança, contrato,
  manutenibilidade ou risco.
- Não bloqueie PR por preferência estilística isolada.
- Não proponha refactor amplo se uma correção local resolve o problema.

## Regras contra alucinação

- Cite arquivos e trechos quando possível.
- Diferencie fato de inferência.
- Não invente requisitos ou contratos.
- Não assuma CI/CD, PR template ou skills existentes.

## Output esperado

- Resumo do PR/diff.
- Achados bloqueantes.
- Achados importantes.
- Sugestões não bloqueantes.
- Evidências com arquivos/trechos.
- Perguntas ao autor.
- Checklist antes de merge.
- Recomendação final: bloquear, aprovar com ressalvas ou sem bloqueios
  aparentes.

## Critérios de aceite

- Nenhum arquivo foi alterado.
- Achados são acionáveis.
- Comentários cosméticos são evitados.
- A recomendação final é clara.
- O agente principal entrega revisão consolidada.

## Checklist de validação humana

- Confirmar que o reviewer não alterou o diff.
- Confirmar achados bloqueantes.
- Confirmar validações antes de merge.
- Confirmar se breaking changes foram documentados.

## Exemplo de uso

```txt
Use este prompt para revisar este diff em modo multi-agent read-only. Não altere
arquivos. Foque em bugs, regressões, contratos, testes, segurança, performance,
manutenibilidade e escopo. Entregue uma única revisão consolidada.
```
