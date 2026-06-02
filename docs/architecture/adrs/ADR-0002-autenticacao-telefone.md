# ADR 0002: Autenticação por Telefone (OTP)

## Status

**Accepted** — v1 implementada com allowlist + JWT (2026-06-01)

## Date

2026-06-01

## Context

O export Google Stitch (`login_onboarding_pt_br`) mostra login com **número de telefone** e aceite de termos/privacidade. O app é interno (~40 usuários) mas trata **dados pessoais** (telefone, nomes em presença) sujeitos a **LGPD**.

## Decision

**v1 (implementada):** [Opção 2 — Allowlist interna](#option-2--allowlist-de-telefones-v1-interna) + **JWT Bearer** (HMAC, sem refresh token nesta fatia).

1. **Login:** `POST /api/auth/login` com `phoneNumber`; telefone deve existir na allowlist PostgreSQL (`users`).
2. **Sessão:** JWT com claims `sub`, `name`, `role` (`Athlete`, `Coach`, `Admin`); expiração configurável via `Jwt:AccessTokenMinutes`.
3. **Papéis:** `Athlete` (label "Atleta"), `Coach` ("Comissão"), `Admin` ("Administrador").
4. **Endpoints públicos nesta v1:** `GET /api/events`, `GET /api/videos` permanecem sem auth; `GET /api/auth/me` exige Bearer token.
5. **OTP/SMS:** follow-up futuro (Opção 1); não implementado na Fase 2c.

**Trade-off de segurança:** allowlist v1 trata **posse do número** como único fator — adequado para ambiente interno pequeno com onboarding manual; **não** substitui OTP para exposição ampla.

Textos legais (termos + privacidade): conteúdo humano pendente; mobile pode exibir placeholders até spec legal.

## Options Considered

### Option 1 — OTP via provedor SMS (Twilio, AWS SNS, etc.)

**Pros:** Alinha ao protótipo Stitch; familiar para usuários.

**Cons:** Custo, configuração, LGPD e retenção de logs; dependência externa.

**Status:** follow-up pós-v1.

### Option 2 — Allowlist de telefones (v1 interna)

**Pros:** Máxima simplicidade para ~40 usuários; sem SMS em dev.

**Cons:** Onboarding manual; fator único (telefone); menos escalável se o clube crescer.

**Status:** **escolhida e implementada** na Fase 2c.

### Option 3 — SSO institucional (futuro)

**Pros:** Centralizado se o clube já tiver IdP.

**Cons:** Provavelmente indisponível no curto prazo.

## Consequences

- Allowlist DEV via `DatabaseSeeder` (PostgreSQL) — não reutilizar dados de seed em produção.
- Chave JWT via `Jwt__SigningKey` (env); nunca commitar valor real de produção.
- Presença no treino pode usar auth na próxima fatia.
- OTP exigirá ADR amendment ou ADR filha quando provedor for escolhido.

## Validation Plan

- [x] Testes de integração: login allowlist, 401 telefone desconhecido, `/api/auth/me` com/sem token
- [ ] Revisão LGPD com responsável humano (produção)
- [x] `.env.example` com variáveis `Jwt__*` (placeholders)

## Rollback Plan

Desabilitar `MapAuthEndpoints` e `AddSienaAuth`; reverter para apenas leitura pública.

## Human Approval Required

**Obtida** para v1 allowlist + JWT (decisão do responsável em 2026-06-01). OTP em produção requer nova aprovação.
