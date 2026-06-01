# ADR 0001: Mobile Stack (React Native)

## Status

Accepted (decisão do responsável) — implementação posterior

## Date

2026-05-28

## Context

O Siena é um app mobile (iOS e Android) para a associação de voleibol. O projeto de referência **Portfolio** usa Next.js (web); o Siena é mobile-first. O backend permanece em .NET, reaproveitando a disciplina e as camadas do Portfolio.

O desenvolvimento começará **pelo backend**; o cliente mobile vem depois.

## Decision

Usar **React Native (TypeScript)** em `apps/mobile/` como cliente principal.

## Consequences

Positivas:

- Stack de cliente em TypeScript, próxima do ecossistema já usado no Portfolio (TS/React).
- Codebase única para iOS e Android, alinhada à regra de "monorepo simples".

Negativas / custos:

- Pipeline de lint/test do mobile separado do .NET.
- Integrações nativas específicas exigirão avaliação caso a caso.

## Validation Plan

A definir quando o mobile entrar em escopo (após o backend). Mínimo esperado: lint + testes do app e uma tela consumindo `GET /api/health`.

## Rollback Plan

Caso React Native seja revisto, substituir `apps/mobile/` mantendo os contratos da API inalterados e atualizar `ARCHITECTURE.md`, `AI-CONFIG.md` e a regra `.cursor/rules/mobile-app.mdc`.

## Human Approval Required

Decisão de stack confirmada pelo responsável. Demais decisões mobile (navegação, libs, auth) exigem ADRs próprios quando surgirem.
