# ADR 0002: Autenticação por Telefone (OTP)

## Status

Proposed — **requires human approval** before implementation

## Date

2026-06-01

## Context

O export Google Stitch (`login_onboarding_pt_br`) mostra login com **número de telefone** e aceite de termos/privacidade. O app é interno (~40 usuários) mas trata **dados pessoais** (telefone, nomes em presença) sujeitos a **LGPD**.

Não há provedor SMS/OTP escolhido nem fluxo de sessão definido. Implementar auth sem esta ADR violaria [SECURITY.md](../../../SECURITY.md) e [AGENTS.md](../../../AGENTS.md).

## Decision

**Pendente.** Não implementar envio de OTP nem persistência de telefone em produção até o responsável aprovar:

1. Provedor de SMS/OTP (ou alternativa, ex.: lista allowlist em v1 interna)
2. Modelo de sessão (JWT, cookie, refresh)
3. Papéis (atleta, coach, admin)
4. Textos legais (termos + privacidade)

## Options Considered

### Option 1 — OTP via provedor SMS (Twilio, AWS SNS, etc.)

**Pros:** Alinha ao protótipo Stitch; familiar para usuários.

**Cons:** Custo, configuração, LGPD e retenção de logs; dependência externa.

### Option 2 — Allowlist de telefones (v1 interna)

**Pros:** Máxima simplicidade para ~40 usuários; sem SMS em dev.

**Cons:** Onboarding manual; menos escalável se o clube crescer.

### Option 3 — SSO institucional (futuro)

**Pros:** Centralizado se o clube já tiver IdP.

**Cons:** Provavelmente indisponível no curto prazo.

## Consequences

Until decided:

- API pode expor apenas endpoints públicos de leitura em dev com seed, ou health, sem auth
- Mobile login screen é UI-only até contrato de auth existir

## Validation Plan

Após decisão:

- Testes de integração para fluxo feliz e OTP inválido/expirado
- Revisão LGPD com responsável humano
- `.env.example` documentando variáveis do provedor (sem valores reais)

## Rollback Plan

Desabilitar endpoints de auth; reverter para allowlist ou modo dev documentado.

## Human Approval Required

**Yes** — obrigatório antes de qualquer código de auth ou armazenamento de telefone.
