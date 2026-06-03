# ADR 0004: Mobile — Expo + Expo Router

## Status

Accepted

## Date

2026-06-03

## Context

Fase 3 do Siena inicia o cliente React Native ([ADR-0001](ADR-0001-mobile-stack.md)). O export Google Stitch define telas mobile (login, abas, presença, admin). É necessário escolher runtime, navegação e persistência de sessão sem overengineering.

## Decision

- **Expo (managed workflow)** em `apps/mobile/`
- **Expo Router** (file-based) para auth gate, tabs e stack (presença, admin)
- **expo-secure-store** para JWT
- **fetch** nativo + hook `useApi` (sem react-query)
- Tipografia **Inter** via `@expo-google-fonts/inter`
- Ícones **@expo/vector-icons** (MaterialIcons)

Abas v1: Financeiro (placeholder UI), Calendário, Vídeos. Destaques adiado. Admin mobile para Staff (Administrador / Comissão).

## Consequences

Positivas:

- Setup rápido para ~40 usuários internos; Expo Go para DEV
- Rotas alinhadas às telas Stitch
- Token seguro no dispositivo

Negativas:

- Dependência do ecossistema Expo para builds nativos
- Sub-telas admin CRUD completas ficam para iteração seguinte (stubs + listagem usuários)

## Validation Plan

- [x] `npm run typecheck` e `npm test` em `apps/mobile`
- [ ] Smoke manual: login seed, abas, presença, admin (Staff)

## Rollback Plan

Substituir `apps/mobile/` por React Native CLI mantendo contratos `/api/*`; atualizar este ADR e `mobile-app.mdc`.

## Human Approval Required

Alinhado à decisão de stack ADR-0001. OTP/SMS continua fora de escopo (ADR-0002).
